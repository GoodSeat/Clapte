using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Rules;
using GoodSeat.Liffom.Formulas.Rules;

namespace GoodSeat.Liffom.Formulas.Matrices.Rules
{
    /// <summary>
    /// 同項の積算を累乗にまとめるルールを表します。
    /// ((a^^b)^^d).((a^^c)^^e) → a^^(bd + ce)
    /// </summary>
    /// <remarks>
    /// このルールにて、二重の累乗を想定しているのは、整理では ^-1 を括り出すため。
    /// </remarks>
    public class CombineProductOfMatrixToPowerRule : CombinationPatternRule
    {
        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            (b as RulePatternVariable).AdmitPowerOne = true;
            (c as RulePatternVariable).AdmitPowerOne = true;
            (d as RulePatternVariable).AdmitPowerOne = true;
            (e as RulePatternVariable).AdmitPowerOne = true;

            formula1 = new PowerOfMatrix(new PowerOfMatrix(a, b), d); 
            formula2 = new PowerOfMatrix(new PowerOfMatrix(a, c), e); 
        }

        protected override Formula GetRuledFormula()
        {
            Formula r1;
            if (b == 1) r1 = d;
            else if (d == 1) r1 = b;
            else r1 = b * d;

            Formula r2;
            if (c == 1) r2 = e;
            else if (e == 1) r2 = c;
            else r2 = e * c;

            return new PowerOfMatrix(a, (r1 + r2));
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is ProductOfMatrix; }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(ExpandNumericPowerOfMatrixRule); // 逆変換の展開傾向
            yield return typeof(DistributivePropertyRule); // 同数式に対する展開傾向
            yield return typeof(MatrixMultiplicationRule); // 計算できるなら計算する
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return ExpandNumericPowerOfMatrixRule.Entity; // (a+b)^^2 → (a+b).(a+b)
        }

        public override string Information { get { return "同項の積算を累乗にまとめるルールです。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("(a+b).(a+b)"),
                Formula.Parse("(a+b)^^(1+1)")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("a.a^^(1/2)"),
                Formula.Parse("a^^(1+1/2)")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() 
        {
            yield return new CombineProductOfMatrixToPowerRule();
        }

        public override Rule GetClone() { return new CombineProductOfMatrixToPowerRule(); }

        /// <summary>
        /// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
        /// </summary>
        protected override bool RetryAll { get { return false; } }

        /// <summary>
        /// このルールの変形において、組み合わせが逆転しても結果が変わらないか否かを取得します。
        /// </summary>
        protected override bool Reversible { get { return true; } }
    }
}
