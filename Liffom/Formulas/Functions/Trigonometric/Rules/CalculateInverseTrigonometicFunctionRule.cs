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

        /// <summary>
        /// 指定数式が本クラスの処理対象となる数式か否かを簡易判定します。
        /// </summary>
        protected internal override bool IsTargetTypeFormula(Formula target)
        {
            return target is Function && target.Contains<TrigonometricFunction>();
        }

        /// <summary>
        /// 処理ルールに従って数式に対する変換処理を試みます。
        /// </summary>
        /// <param name="formula">処理対象の数式</param>
        /// <returns>変形後の数式。変形の対象とならない、もしくは結果的に変形のない場合、null。</returns>
        protected override Formula OnTryMatchRule(Formula target)
        {
            if (target is TrigonometricFunction)
                OnTryMatchRule(target as TrigonometricFunction);
            else
                OnTryMatchRule(target as Function);

            return null;
        }

        /// <summary>
        /// 逆三角形関数を引数とした三角関数を対象に、ルールの適用を試みます。
        /// </summary>
        /// <param name="triFunction">対象の三角関数。</param>
        /// <returns>ルールの適用結果。適用対象とならなかった場合、null。</returns>
        private Formula OnTryMatchRule(TrigonometricFunction triFunction)
        {
            // TODO: 本来、 |x|≦1 ?
            Formula x = triFunction[0][0];
            if (triFunction is Sin)
            {
                if (triFunction[0] is ArcSin) return x;
                if (triFunction[0] is ArcCos) return new Sqrt(1 - (x ^ 2));
                if (triFunction[0] is ArcTan) return x / new Sqrt(1 + (x ^ 2));
            }
            else if (triFunction is Cos)
            {
                if (triFunction[0] is ArcSin) return new Sqrt(1 - (x ^ 2));
                if (triFunction[0] is ArcCos) return x;
                if (triFunction[0] is ArcTan) return 1 / new Sqrt(1 + (x ^ 2));
            }
            else if (triFunction is Tan)
            {
                if (triFunction[0] is ArcSin) return x / new Sqrt(1 - (x ^ 2));
                if (triFunction[0] is ArcCos) return new Sqrt(1 - (x ^ 2)) / x;
                if (triFunction[0] is ArcTan) return x;
            }
            return null;
        }

        /// <summary>
        /// 三角形関数を引数とした逆三角関数を対象に、ルールの適用を試みます。
        /// </summary>
        /// <param name="triFunction">対象の逆三角関数。</param>
        /// <returns>ルールの適用結果。適用対象とならなかった場合、null。</returns>
        private Formula OnTryMatchRule(Function arcFunction)
        {
            var triFunction = arcFunction[0] as TrigonometricFunction;
            if (triFunction == null) return null;

            // TODO: 本来、 -π/2≦θ≦π/2
            if (arcFunction is ArcSin && triFunction is Sin) return triFunction[0];
            if (arcFunction is ArcCos && triFunction is Cos) return triFunction[0];
            if (arcFunction is ArcTan && triFunction is Tan) return triFunction[0];
            return null;
        }

        /// <summary>
        /// このルールの説明を取得します。
        /// </summary>
        public override string Information { get { return "逆三角関数の数値化に関する公式を規定するルールです。"; } }

        /// <summary>
        /// 変形前数式と変形後数式から成る、このルールの変形例を順次返す反復子を取得します。ここで取得される例は、単体テストで自動検証されます。
        /// </summary>
        /// <returns>変形例(変形前の数式と、変形適用後の数式を表す組合せ)を返す反復子。</returns>
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

        /// <summary>
        /// この型のルールのプロパティで、考えうるすべてのパターンのインスタンスを生成して返す反復子を取得します。複数パターンのない場合、単にthisを返してください。
        /// </summary>
        /// <returns>このルールで考えられる全てのパターンのインスタンスを返す反復子。</returns>
        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return Entity; }

        /// <summary>
        /// クローンを取得します。再帰呼び出し可能なルールである場合、単にthisを返してください。
        /// </summary>
        /// <returns>複製されたルール。</returns>
        public override Rule GetClone() { return Entity; }

    }
}
