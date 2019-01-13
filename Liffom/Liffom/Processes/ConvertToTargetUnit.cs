using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Liffom.Processes
{
    /// <summary>
    /// 数式中の単位を指定した目標単位に一括変換する処理を表します。
    /// </summary>
    public class ConvertToTargetUnit : Process
    {
        /// <summary>
        /// 数式中の単位を指定した目標単位に一括変換する処理を初期化します。
        /// </summary>
        /// <param name="convertAddition">換算加算値も考慮して変換を行うか。</param>
        public ConvertToTargetUnit(bool convertAddition) { AlsoConvertAddition = convertAddition; }

        /// <summary>
        /// 数式中の単位を指定した目標単位に一括変換する処理を初期化します。
        /// </summary>
        /// <param name="deformToken">変換時に適用する変形の識別トークン。</param>
        /// <param name="convertAddition">換算加算値も考慮して変換を行うか。</param>
        public ConvertToTargetUnit(DeformToken deformToken, bool convertAddition)
        {
            ApplyDeformToken = deformToken;
            AlsoConvertAddition = convertAddition;
        }

        /// <summary>
        /// 返還後の単位系数に適用する変形トークンを設定もしくは取得します。
        /// </summary>
        public DeformToken ApplyDeformToken { get; set; }

        /// <summary>
        /// 換算加算値も考慮して変換を行うか否かを設定もしくは取得します。
        /// </summary>
        public bool AlsoConvertAddition { get; set; }

        /// <summary>
        /// 一意のユーザー情報を指定して、複数の同時呼び出しを許可するか否かを取得します。
        /// </summary>
        public override bool IsSupportMultipleConcurrentInvocations { get { return true; } }

        /// <summary>
        /// 処理対象とする数式数の下限値を取得します。
        /// </summary>
        public override int TargetArgumentsMinQty { get { return 2; } }

        /// <summary>
        /// 指定された数式に対して、処理を実行します。
        /// </summary>
        protected override Formula OnDo(object userState, params Formula[] targets)
        {
            var target = targets[0].Copy();
            var targetUnit = targets[1];
            if (!targetUnit.IsUnit(true)) throw new FormulaProcessException(string.Format("{0}は単位として扱えません。", targetUnit));

            var conversion = new CalculateUnitConversionFactor();

            var a = new RulePatternVariable("a");
            var b = new RulePatternVariable("b");
            a.CheckTarget = f => !f.IsUnit() && !f.Contains<Unit>();
            b.CheckTarget = f => f.IsUnit(true);

            var pattern = a * b;

            Formula.IsTargetFormula isTarget = (f => f.PatternMatch(pattern));

            bool convertNumeric = (target is Numeric); // 無次元単位への変換に対する例外処理
            if (convertNumeric) isTarget = (f => f is Numeric);

            foreach (var convert in target.GetExistFactors(isTarget))
            {
                Formula aMatched = null, bMatched = null;
                if (!convertNumeric)
                {
                    convert.PatternMatch(pattern);
                    aMatched = a.MatchedFormula;
                    bMatched = b.MatchedFormula;
                }
                else
                {
                    aMatched = convert;
                    bMatched = 1;
                }

                var factor = conversion.Do(bMatched, targetUnit);
                Formula coef = factor * aMatched;
                if (convert == target && conversion.ConversionAddition != null) coef = coef + conversion.ConversionAddition;

                if (ApplyDeformToken != null) coef = coef.DeformFormula(ApplyDeformToken);

                var result = new Product(false, coef, targetUnit);

                target = target.Substitute(convert, result, false);
            }

            return target;
        }

    }


}
