using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Liffom.Formulas.Rules
{
    /// <summary>
    /// 0の含まれる積算において、不要な項を削除するルールを表します。
    /// </summary>
    public class DeleteZeroOnProductRule : CombinationRule
    {
        static DeleteZeroOnProductRule s_entity;

        /// <summary>
        /// 0の含まれる積算において、不要な項を削除するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static DeleteZeroOnProductRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new DeleteZeroOnProductRule();
                return s_entity;
            }
        }

        static Formula.IsTargetFormula s_isRegistedUnit = (Formula f) => f is Unit && (f as Unit).BelongTable != null;

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Product; }

        protected override bool IsTargetCouple(Formula f1, Formula f2)
        {
            if (f1 != 0) return false;

            // 単位表に登録のある単位があるなら、0がかかることを明示するため消去しない（℃→Kの換算などで必要な情報となるため）
            if (f2.Contains(s_isRegistedUnit)) return false;

            // 0除算とならないことを確認
            var rule = CalculatePowerNumericRule.Entity;
            f2 = rule.TryMatchRule(f2);

            return true;
        }

        protected override Formula GetRuledFormula(Formula f1, Formula f2) 
        {
            Numeric zero = f1 as Numeric;
            if (f2 is Numeric && (f2 as Numeric).Figure < 0) f1 = new Numeric(zero.Figure * -1); // +0と-0を区別
            return f1;
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(ReduceFactorOfProductRule); // 約分済み
        }

        public override string Information { get { return "積算に0が含まれる場合に、不要な項を削除するルールです。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("a*0*b*c"),
                Formula.Parse("0")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return Entity; }

        public override Rule GetClone() { return Entity; }

        /// <summary>
        /// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
        /// </summary>
        protected override bool RetryAll { get { return false; } }
    }
}
