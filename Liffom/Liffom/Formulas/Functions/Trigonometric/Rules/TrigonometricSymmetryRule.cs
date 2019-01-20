using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Extensions;
using GoodSeat.Liffom.Formulas.Rules;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Functions.Trigonometric.Rules
{
    /// <summary>
    /// 三角関数の対称性を規定するルールを表します。
    /// </summary>
    public class TrigonometircSymmetryRule : Rule
    {
        static TrigonometircSymmetryRule s_entity;

        /// <summary>
        /// 三角関数の対称性を規定するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static TrigonometircSymmetryRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new TrigonometircSymmetryRule();
                return s_entity;
            }
        }

        enum TrigonometricType
        {
            Sin, Cos, Tan
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is TrigonometricFunction; }

        protected override Formula OnTryMatchRule(Formula target)
        {
            TrigonometricType TargetType;
            if (target is Sin) TargetType = TrigonometricType.Sin;
            else if (target is Cos) TargetType = TrigonometricType.Cos;
            else if (target is Tan) TargetType = TrigonometricType.Tan;
            else return null;

            Formula theta = target[0];

            Rule rule = new SymmetryXAxisAngleRule();
            if (rule.TryMatchRule(ref theta)) // θ=0に対して対称(反角公式)
            {
                switch (TargetType)
                {
                    case TrigonometricType.Sin: return -1 * new Sin(theta);
                    case TrigonometricType.Cos: return new Cos(theta);
                    case TrigonometricType.Tan: return -1 * new Tan(theta);
                }
            }

            rule = new SymmetryYeqXLineAngleRule();
            if (rule.TryMatchRule(ref theta)) // θ=π/4に対して対称(余角公式)
            {
                switch (TargetType)
                {
                    case TrigonometricType.Sin: return new Cos(theta);
                    case TrigonometricType.Cos: return new Sin(theta);
                    case TrigonometricType.Tan: return 1 / new Tan(theta); // =cotθ
                }
            }

            rule = new SymmetryYAxisAngleRule();
            if (rule.TryMatchRule(ref theta)) // θ=π/2に対して対称(補角公式)
            {
                switch (TargetType)
                {
                    case TrigonometricType.Sin: return new Sin(theta);
                    case TrigonometricType.Cos: return -1 * new Cos(theta);
                    case TrigonometricType.Tan: return -1 * new Tan(theta);
                }
            }
            return null;
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(CalculateProductOfMolecularNumericRule);
            yield return typeof(GeneralAngleOnTrigonometricRule); // 一般角の公式を適用済み
        }


        /// <summary>
        /// θ=0（直線y=0）に対しての対称性を考慮するための角度か否かを判定するルールを表します。
        /// </summary>
        class SymmetryXAxisAngleRule : Rule
        {
            static SymmetryXAxisAngleRule s_entity;

            /// <summary>
            /// 積算の約分ルールの実体を取得します。
            /// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
            /// </summary>
            public static SymmetryXAxisAngleRule Entity
            {
                get
                {
                    if (s_entity == null)
                    {
                        s_entity = new SymmetryXAxisAngleRule();
                    }
                    return s_entity;
                }
            }

            protected override Formula OnTryMatchRule(Formula target) { return new Product(true, -1 * target).DeformWithRules(false, CalculateProductOfMolecularNumericRule.Entity, RemoveInvalidOneOnProductRule.Entity); }

            protected internal override bool IsTargetTypeFormula(Formula target) { return target.IsNegative(); }

            public override string Information
            {
                get { return "θ=0（直線y=0）に対しての対称性を考慮するための角度か否かを判定します。"; }
            }

            public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
            {
                yield return new KeyValuePair<Formula, Formula>(
                    Formula.Parse("-θ"),
                    Formula.Parse("θ")
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

        /// <summary>
        /// θ=π/4（直線y=x）に対しての対称性を考慮するための角度か否かを判定するルールを表します。
        /// </summary>
        class SymmetryYeqXLineAngleRule : PatternRule
        {
            protected override Formula GetRulePatternFormula() { return Constants.Pi.pi / 2 + a; }

            protected override Formula GetRuledFormula() { return -a; }

            protected internal override bool IsTargetTypeFormula(Formula target) { return true; }

            public override string Information
            {
                get { return "θ=π/4（直線y=x）に対しての対称性を考慮するための角度か否かを判定します。"; }
            }

            public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
            {
                yield return new KeyValuePair<Formula, Formula>(
                    Formula.Parse("π/2+θ"),
                    Formula.Parse("-θ")
                    );
            }

            protected override IEnumerable<Rule> OnGetAllPatternSample()
            {
                yield return new SymmetryYeqXLineAngleRule();
            }

            public override Rule GetClone()
            {
                return new SymmetryYeqXLineAngleRule();
            }
        }

        /// <summary>
        /// θ=π/2（直線x=0）に対しての対称性を考慮するための角度か否かを判定するルールを表します。
        /// </summary>
        class SymmetryYAxisAngleRule : PatternRule
        {
            protected override Formula GetRulePatternFormula() { return Constants.Pi.pi + a; }

            protected override Formula GetRuledFormula() { return -a; }

            protected internal override bool IsTargetTypeFormula(Formula target) { return true; }

            public override string Information
            {
                get { return "θ=π/2（直線x=0）に対しての対称性を考慮するための角度か否かを判定します。"; }
            }

            public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
            {
                yield return new KeyValuePair<Formula, Formula>(
                    Formula.Parse("π+a"),
                    Formula.Parse("-a")
                    );
            }

            protected override IEnumerable<Rule> OnGetAllPatternSample()
            {
                yield return new SymmetryYAxisAngleRule();
            }

            public override Rule GetClone()
            {
                return new SymmetryYAxisAngleRule();
            }
        }


        public override string Information { get { return "三角関数の対称性を考慮した数式の変形を行います。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("sin(-θ)"),
                Formula.Parse("-sin(θ)")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("sin(pi/2-θ)"),
                Formula.Parse("cos(-(-θ))")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("sin(pi-θ)"),
                Formula.Parse("sin(-(-θ))")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return Entity; }

        public override Rule GetClone() { return Entity; }

    }
}
