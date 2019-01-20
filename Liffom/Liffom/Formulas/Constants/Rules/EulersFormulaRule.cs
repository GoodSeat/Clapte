using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Functions.Trigonometric.Rules;

namespace GoodSeat.Liffom.Formulas.Constants.Rules
{
    /// <summary>
    /// オイラーの公式を規定するルールを表します。
    /// e^(R + iE) → e^R * (cos(E) + i*sin(E))
    /// </summary>
    public class EulersFormulaRule : Rule
    {
        static EulersFormulaRule s_entity;

        /// <summary>
        /// オイラーの公式を規定するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static EulersFormulaRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new EulersFormulaRule();
                return s_entity;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target)
        { 
            var pow = target as Power;
            if (pow == null) return false;
            if (pow.Base != Napiers.e && pow.Base != Napiers.e.Value) return false;

            Formula R, E;
            if (!Imaginary.GetRealAndImaginary(pow.Exponent, out R, out E)) return false;
            if (E == 0) return false;

            return true;
        }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var pow = target as Power;
            Formula R, E;
            Imaginary.GetRealAndImaginary(pow.Exponent, out R, out E);

            var e = Napiers.e;
            var i = Imaginary.i;
            return (e ^ R) * (new Cos(E) + i * new Sin(E));
        }

        protected override IEnumerable<Type> OnGetPostDemandRules()
        {
            yield return typeof(EulersFormulaInverseRule); // 逆変換の整理傾向
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return new EulersFormulaInverseRule();
        }

        public override string Information
        {
            get { return "オイラーの公式を適用します。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("e^(i*pi)"),
                Formula.Parse("e^0 * (cos(pi)+i*sin(pi))")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("e^(5+3i)"),
                Formula.Parse("e^5*(cos(3) + i*sin(3))")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new EulersFormulaRule();
        }

        public override Rule GetClone()
        {
            return new EulersFormulaRule();
        }
    }

    /// <summary>
    /// オイラーの公式を規定するルールを表します。
    /// cos(x) + i*sin(x) → e^(ix)
    /// </summary>
    public class EulersFormulaInverseRule : CombinationPatternRule
    {
        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            formula1 = new Cos(a) * b;
            formula2 = new Product(new Sin(a), b, Imaginary.i);
            (b as RulePatternVariable).AdmitMultiplyOne = true;
        }

        protected override Formula GetRuledFormula() { return b * Napiers.e ^ (Imaginary.i * a); }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Sum; }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(TrigonometircSymmetryRule); // 一般角に変換済み
            yield return typeof(EulersFormulaRule); // 逆変換の展開傾向
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return new EulersFormulaRule();
        }

        public override string Information
        {
            get { return "オイラーの公式を適用します。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("cos(x)+i*sin(x)"),
                Formula.Parse("(1*e)^(i*x)")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new EulersFormulaInverseRule();
        }

        public override Rule GetClone()
        {
            return new EulersFormulaInverseRule();
        }

        /// <summary>
        /// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
        /// </summary>
        protected override bool RetryAll { get { return false; } }
    }


}
