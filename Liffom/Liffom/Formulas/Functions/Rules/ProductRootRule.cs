using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Functions.Rules
{
    /// <summary>
    /// ルート関数同士の積算ルールを表します。
    /// sqrt(a) * sqrt(b) → sqrt(ab)
    /// </summary>
    public class ProductRootRule : CombinationPatternRule
    {
        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            formula1 = new Root(a, c);
            formula2 = new Root(b, c);
        }

        protected override Formula GetRuledFormula() { return new Root(a * b, c); }

        /// <summary>
        /// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
        /// </summary>
        protected override bool RetryAll { get { return false; } }

        /// <summary>
        /// このルールの変形において、組み合わせが逆転しても結果が変わらないか否かを取得します。
        /// </summary>
        protected override bool Reversible { get { return true; } }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Product; }

        public override string Information { get { return "ルート関数同士の積算ルールです。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("sqrt(2) * root(a, 2)"),
                Formula.Parse("root(2 * a, 2)")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return new ProductRootRule(); }

        public override Rule GetClone() { return new ProductRootRule(); }
    }
}
