using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Functions.Rules
{
    /// <summary>
    /// 微分法の積を展開するルールを表します。
    /// diff(a*b,x) → diff(a,x)*b + diff(b,x)*a
    /// </summary>
    public class ExpandDifferentiateProductRule : Rule
    {
        static ExpandDifferentiateProductRule s_entity;

        /// <summary>
        /// 微分法の積を展開するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static ExpandDifferentiateProductRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new ExpandDifferentiateProductRule();
                return s_entity;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Differentiate; }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var diff = target as Differentiate;
            var x = diff.Variable;
            var f = diff[0] as Product;
            if (f == null) return null;

            var sumList = new List<Formula>();
            foreach (var section in f)
            {
                var productList = new List<Formula>();
                productList.Add(new Differentiate(section, x));
                foreach (var other in f)
                {
                    if (object.ReferenceEquals(other, section)) continue;
                    productList.Add(other);
                }
                sumList.Add(new Product(productList.ToArray()));
            }
            return new Sum(sumList.ToArray());
        }

        public override string Information
        {
            get { return "微分法の積を展開するルールです。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("diff(sin(x)*x*y, x)"),
                Formula.Parse("diff(sin(x),x)*x*y + diff(x,x)*sin(x)*y + diff(y,x)*sin(x)*x")
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

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(CalculateFunctionRule); // → 0
        }

        protected override IEnumerable<Type> OnGetPostDemandRules()
        {
            yield return typeof(DeformChildrenRule);
        }
    }


}
