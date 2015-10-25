using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Reals;

namespace GoodSeat.Liffom.Formulas.Functions.Trigonometric.Rules
{
    /// <summary>
    /// 三角関数における一般角(-π～π)の公式を規定するルールを表します。
    /// </summary>
    public class GeneralAngleOnTrigonometricRule : Rule
    {
        static GeneralAngleOnTrigonometricRule s_entity;

        /// <summary>
        /// 三角関数における一般角の公式を規定するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static GeneralAngleOnTrigonometricRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new GeneralAngleOnTrigonometricRule();
                return s_entity;
            }
        }

        Unit _rad;

        private Unit Rad 
        {
            get
            {
                if (_rad == null) _rad = new Unit("rad");
                return _rad;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is TrigonometricFunction; }

        protected override Formula OnTryMatchRule(Formula target)
        {
            // radがあったら消去する。
            if (target.Contains(Rad)) return target.Substituted(Rad, 1);

            var func = target as TrigonometricFunction;
            var angle = func[0];

            Rule rule = new GeneralAngleRule();
            if (rule.TryMatchRule(ref angle)) return func.CreateFunction(angle);
            return null;
        }

        public override string Information
        {
            get { return "三角関数における一般角(-π～π)の公式を規定するルールです。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("sin(5*π)"),
                Formula.Parse("sin(0+π*(5-4))")
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


        /// <summary>
        /// 三角関数における一般角への変換ルールを表します。
        /// </summary>
        public class GeneralAngleRule : PatternRule
        {
            protected override Formula GetRulePatternFormula()
            {
                Formula rule = a + b * Constants.Pi.pi;
                (b as RulePatternVariable).CheckTarget = f => Numeric.IsNumericOnly(f);
                (a as RulePatternVariable).AdmitAddZero = true;
                return rule;
            }

            protected override Formula GetRuledFormula()
            {
                Real coef = (b.Numerate() as Numeric).Figure;

                bool aRemoved = false;
                if (a is Numeric && a != 0)
                {
                    var add = a as Numeric;
                    coef += add.Figure / add.Figure.GetPi();
                    aRemoved = true;
                }

                if (!aRemoved && Math.Abs(coef) <= 1) return null;

                int mult = (int)(coef / 2d); // 5.2 → 2
                var newCoef = b - new Numeric(mult * 2); // 5.2 - 2*2 → 1.2

                if (aRemoved) return newCoef * Constants.Pi.pi;
                return a + newCoef * Constants.Pi.pi;
            }

            protected internal override bool IsTargetTypeFormula(Formula target) { return true; }

            public override string Information
            {
                get { return "三角関数における一般角への変換ルールです。"; }
            }

            public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
            {
                yield return new KeyValuePair<Formula, Formula>(
                    Formula.Parse("a+5π"),
                    Formula.Parse("a+π")
                    );
                yield return new KeyValuePair<Formula, Formula>(
                    Formula.Parse("a-5π"),
                    Formula.Parse("a-π")
                    );
                yield return new KeyValuePair<Formula, Formula>(
                    Formula.Parse(Math.PI.ToString() + "+5π"),
                    Formula.Parse("0")
                    );
            }

            protected override IEnumerable<Rule> OnGetAllPatternSample()
            {
                yield return new GeneralAngleRule();
            }

            public override Rule GetClone()
            {
                return new GeneralAngleRule();
            }
        }
    }
}
