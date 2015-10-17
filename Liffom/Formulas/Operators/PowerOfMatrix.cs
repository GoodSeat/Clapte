using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Reals;
using GoodSeat.Liffom.Utilities;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators.Rules.Powers;
using GoodSeat.Liffom.Formulas.Operators.Rules.Products;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Formats;

namespace GoodSeat.Liffom.Formulas.Operators
{
    /// <summary>
    /// 累乗を表します。
    /// </summary>
    [Serializable()]
    public class PowerOfMatrix : OperatorBinary
    {
        /// <summary>
        /// 累乗を作成します。
        /// </summary>
        /// <param name="baseFormula">累乗対象。</param>
        /// <param name="exponent">指数。</param>
        public PowerOfMatrix(Formula baseFormula, Formula exponent)
            : base(baseFormula, exponent) { }

        #region プロパティ

        /// <summary>
        /// 累乗の対象数式を取得します。
        /// </summary>
        public Formula Base { get { return Forword; } }

        /// <summary>
        /// 指数を取得します。
        /// </summary>
        public Formula Exponent { get { return Backword; } }

        /// <summary>
        /// 演算が満たす法則を取得します。
        /// </summary>
        public override OperatorLaw Law { get { return OperatorLaw.None; } }
        
        /// <summary>
        /// 演算の評価優先度を表す数値を取得します。
        /// 原則として、OperatorPriorityの値を返してください。
        /// </summary>
        public override int EvaluatePriority { get { return (int)OperatorPriority.Power; } }

        /// <summary>
        /// 引数を指定して、演算を生成します。
        /// </summary>
        /// <param name="args">初期化に用いるか変数の数式。</param>
        public override Operator CreateOperator(params Formula[] args)
        {
            return new PowerOfMatrix(args[0], args[1]);
        }

        #endregion

        protected override Formula.CompareResult IsLargerThan(Formula other)
        {
            Formula thisExp = Exponent;
            Formula otherExp = 1;
            if (other is PowerOfMatrix) otherExp = (other as PowerOfMatrix).Exponent;

            double thisPowValue = 0;
            if (thisExp is Numeric) thisPowValue = (thisExp as Numeric);

            double otherPowValue = 0;
            if (otherExp is Numeric) otherPowValue = (otherExp as Numeric);

            if (thisPowValue < 0 && otherPowValue >= 0) return CompareResult.Larger;
            if (thisPowValue >= 0 && otherPowValue < 0) return CompareResult.Smaller;

            return base.IsLargerThan(other);
        }

        public override string GetText()
        {
            string exp =  Exponent.ToString();
            string data = Base.ToString();

            string result = data + "^^" + exp;
            return result;
        }

        protected override bool OnCheckPatternMatch(Formula f)
        {
            if (!(f is PowerOfMatrix)) // 検証対象が累乗でない
            {
                if (Exponent is RulePatternVariable && (Exponent as RulePatternVariable).AdmitPowerOne) // ^^1としてのみ許可
                {
                    if (!Exponent.IsPatternRuleMatchWith(1, false)) return false;
                    return base.OnCheckPatternMatch(new PowerOfMatrix(f, 1));
                }
            }

            return base.OnCheckPatternMatch(f);
        }
        
        /// <summary>
        /// 指定処理に関連するルールをすべて返す反復子を取得します。
        /// </summary>
        /// <param name="deformToken">変形識別トークン。</param>
        /// <param name="sender">ルール適用対象となる最上位親数式。</param>
        /// <param name="history">変形履歴情報。</param>
        /// <returns>変形に関連するルールを返す反復子。</returns>
        public override IEnumerable<Rule> GetRelatedRulesOf(DeformToken deformToken, Formula sender, DeformHistory history) 
        {
            foreach (var rule in base.GetRelatedRulesOf(deformToken, sender, history)) yield return rule;
        }

    }
}
