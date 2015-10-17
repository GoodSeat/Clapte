using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;

namespace GoodSeat.Liffom.Formulas.Functions.Trigonometric.Rules
{
    /// <summary>
    /// 逆三角関数の数値化に関する公式を規定するルールを表します。
    /// </summary>
    public class CalculateInverseTrigonometicFunctionRule : Rule
    {
        static CalculateInverseTrigonometicFunctionRule s_entity;

        /// <summary>
        /// 逆関数の特性を規定するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static CalculateInverseTrigonometicFunctionRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new CalculateInverseTrigonometicFunctionRule();
                return s_entity;
            }
        }


        protected internal override bool IsTargetTypeFormula(Formula target)
        {
            return target is Function && target.Contains<TrigonometricFunction>();
        }

        protected override Formula OnTryMatchRule(Formula target)
        {
            if (target is TrigonometricFunction)
            {
                var triFunction = target as TrigonometricFunction;

                // TODO: 本来、 |x|≦1 ?
                Formula x = triFunction[0][0];
                if (target is Sin && triFunction[0] is ArcSin) return x;
                if (target is Sin && triFunction[0] is ArcCos) return new Sqrt(1 - (x ^ 2));
                if (target is Sin && triFunction[0] is ArcTan) return x / new Sqrt(1 + (x ^ 2));

                if (target is Cos && triFunction[0] is ArcSin) return new Sqrt(1 - (x ^ 2));
                if (target is Cos && triFunction[0] is ArcCos) return x;
                if (target is Cos && triFunction[0] is ArcCos) return 1 / new Sqrt(1 + (x ^ 2));

                if (target is Tan && triFunction[0] is ArcSin) return x / new Sqrt(1 - (x ^ 2));
                if (target is Tan && triFunction[0] is ArcCos) return new Sqrt(1 - (x ^ 2)) / x;
                if (target is Tan && triFunction[0] is ArcTan) return x;
            }
            else
            {
                var arcFunction = target as Function;
                var triFunction = target[0] as TrigonometricFunction;
                if (triFunction == null) return null;

                // TODO: 本来、 -π/2≦θ≦π/2
                if (arcFunction is ArcSin && triFunction is Sin) return triFunction[0];
                if (arcFunction is ArcCos && triFunction is Cos) return triFunction[0];
                if (arcFunction is ArcTan && triFunction is Tan) return triFunction[0];
            }
            return null;
        }

        public override string Information { get { return "逆三角関数の数値化に関する公式を規定するルールです。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("asin(sin(x))"),
                Formula.Parse("x")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("sin(acos(x))"),
                Formula.Parse("sqrt(1-x^2)")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return Entity; }

        public override Rule GetClone() { return Entity; }

    }
}
