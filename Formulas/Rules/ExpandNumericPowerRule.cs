using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Rules.Products;
using GoodSeat.Liffom.Formulas.Operators.Rules.Powers;

namespace GoodSeat.Liffom.Formulas.Rules
{
    /// <summary>
    /// 整数乗の累乗展開を規定するルールを表します。
    /// </summary>
    public class ExpandNumericPowerRule : Rule
    {
        static ExpandNumericPowerRule s_entity;

        /// <summary>
        /// 整数乗の累乗展開を規定するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static ExpandNumericPowerRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new ExpandNumericPowerRule();
                return s_entity;
            }
        }


        protected internal override bool IsTargetTypeFormula(Formula target)
        {
            var power = target as Power;
            if (power == null) return false;

            if (!(power.Base is OperatorMultiple) && !(power.Base is Numeric)) return false;

            Numeric exponent = power.Exponent as Numeric;
            if (exponent == null || !(exponent.IsInteger)) return false;
            if (exponent == 1 || exponent == -1) return false; // ^1、^-1は適用対象外

            return true;
        }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var power = target as Power;
            if (power == null) return null;

            Numeric exp = power.Exponent as Numeric;
            if (exp <= 0) return null; // OnGetPreDemandRuleにより解決済みのはずなので、対象としない

            List<Formula> sectionList = new List<Formula>();
            for (int i = 0; i < exp.Data; i++) sectionList.Add(power.Base);

            Formula result = new Product(sectionList.ToArray());

            return result;
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(FactorOutMinusOneOfExponentRule); // 負数による累乗では、^-1を括り出し済み
            yield return typeof(TidyUpPowerOfZeroRule); // 0の累乗は解決済み
        }

        protected override IEnumerable<Type> OnGetPostDemandRules()
        {
            yield return typeof(CombineProductToPowerRule); // 逆変換の整理傾向
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return new CombineProductToPowerRule(); // a*a→a^2
        }

        public override string Information
        {
            get { return "整数乗の累乗展開を規定するルールです。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("(5+a)^2"),
                Formula.Parse("(5+a)*(5+a)")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return Entity;
        }

        public override Rule GetClone()
        {
            return Entity;
        }
    }
}
