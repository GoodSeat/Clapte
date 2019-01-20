using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Rules.Products;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Liffom.Formulas.Functions.Rules
{
    /// <summary>
    /// 負数の対数及び複素対数の評価を規定するルールを表します。
    /// </summary>
    public class LogOfImaginaryRule : Rule
    {
        static LogOfImaginaryRule s_entity;

        /// <summary>
        /// 負数の対数及び複素対数の評価を規定するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static LogOfImaginaryRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new LogOfImaginaryRule();
                return s_entity;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target)
        {
            Log log = target as Log;
            if (log == null) return false;

            var a = log[0];
            var b = log[1];

            if (a is Numeric && a < 0) return true;

            Formula aR, aE, bR, bE;
            if (!Imaginary.GetRealAndImaginary(a, out aR, out aE)) return false;
            if (!Imaginary.GetRealAndImaginary(b, out bR, out bE)) return false;

            return (aE != 0 || bE != 0);
        }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var a = target[0];
            var b = target[1];

            var i = Imaginary.i;
            var e = Napiers.e;

            if (b == Napiers.e || b == Napiers.e.Value)
            {
                Formula R, E;
                Imaginary.GetRealAndImaginary(a, out R, out E);
                var r = Imaginary.Abs(R, E);
                var t = Imaginary.Arg(R, E);
                return new Ln(r) + i * t;
            }
            else
            {
                return new Ln(a) / new Ln(b);
            }
        }

        public override string Information
        {
            get { return "負数の対数及び複素対数を評価します。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("ln(i)"),
                Formula.Parse("ln((0^2 + 1^2)^(1/2)) + i*(pi/2)")
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
            yield break;
        }

        protected override IEnumerable<Rule> OnGetReverseRule() 
        {
            yield break;
        }
    }


}

