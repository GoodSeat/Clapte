using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Rules.Products;
using GoodSeat.Liffom.Extensions;

namespace GoodSeat.Liffom.Formulas.Functions.Rules
{
    /// <summary>
    /// √の中身の整理を規定するルールを表します。
    /// </summary>
    public class FactorOutInnerRootRule : Rule
    {
        static FactorOutInnerRootRule s_entity;

        /// <summary>
        /// √の中身の整理を規定するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static FactorOutInnerRootRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new FactorOutInnerRootRule();
                return s_entity;
            }
        }


        protected internal override bool IsTargetTypeFormula(Formula target)
        {
            var root = target as Root;
            if (root == null) return false;

            var exp = root[1] as Numeric;
            if (exp == null) return false;

            return exp.IsInteger;
        }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var root = target as Root;
            var exp = root[1] as Numeric;

            int exponent = (int)exp.Data;
            if (exponent == 1) return root[0];
            if (exponent == 0) return 1; // TODO:これは明確な間違え
            if (exponent < 0) return 1 / new Root(root[0], -1 * exponent);

            var io = SieveIOFrom(root[0], exponent);
            if (io == null) return null;
            if (io.Item2 == 1) return io.Item1;
            return io.Item1 * new Root(io.Item2, exp);
        }


        /// <summary>
        /// <see cref="exp"/>√<see cref="f"/>において、ルートの外に括りだされる数式と中に残る数式からなる組を取得します。
        /// ただし、外に括りだせる数式がない場合には、nullを返します。
        /// </summary>
        /// <param name="f">ルートの中の数式。</param>
        /// <param name="exp">ルートの基数。</param>
        /// <returns>ルートの外に括りだされる数式と、中に残る数式の組。ただし、外に括りだせる数式がない場合には、null。</returns>
        private Tuple<Formula, Formula> SieveIOFrom(Formula f, int exp)
        {
            if (f is Sum) return SieveIOfromSum(f as Sum, exp);
            else return SieveIOfromTerm(f, exp);
        }

        /// <summary>
        /// <see cref="exp"/>√<see cref="sum"/>において、ルートの外に括りだされる数式と中に残る数式からなる組を取得します。
        /// ただし、外に括りだせる数式がない場合には、nullを返します。
        /// </summary>
        /// <param name="sum">ルートの中の加算。</param>
        /// <param name="exp">ルートの基数。</param>
        /// <returns>ルートの外に括りだされる数式と、中に残る数式の組。ただし、外に括りだせる数式がない場合には、null。</returns>
        private Tuple<Formula, Formula> SieveIOfromSum(Sum sum, int exp)
        {
            Formula gcd = null;
            foreach (var f in sum)
            {
                if (gcd == null) gcd = f;
                else gcd = Polynomial.GCD(f, gcd);

                if (gcd == 1 || gcd == null) break;
            }
            if (gcd == 1 || gcd == null) return null;

            var io = SieveIOfromTerm(gcd, exp);
            if (io == null) return null; // outされるものがない

            if (io.Item2 != 1) gcd = (gcd / io.Item2).Combine(); 

            var list = new List<Formula>();
            foreach (var f in sum) list.Add((f / gcd).Combine());

            return Tuple.Create(io.Item1, new Sum(list.ToArray()) as Formula);
        }

        /// <summary>
        /// <see cref="exp"/>√<see cref="f"/>において、ルートの外に括りだされる数式と中に残る数式からなる組を取得します。
        /// ただし、外に括りだせる数式がない場合には、nullを返します。
        /// </summary>
        /// <param name="f">ルートの中の(加算でない)数式。</param>
        /// <param name="exp">ルートの基数。</param>
        /// <returns>ルートの外に括りだされる数式と、中に残る数式の組。ただし、外に括りだせる数式がない場合には、null。</returns>
        private Tuple<Formula, Formula> SieveIOfromTerm(Formula f, int exp)
        {
            var factors = GetFactorsFrom(f);

            var mapOut = new Dictionary<Formula, int>();
            var mapIn = new Dictionary<Formula, int>();
            var done = new List<Formula>();

            foreach (var t in factors)
            {
                if (done.Contains(t)) continue;
                int n = factors.Count(c => c == t);
                int eOut = n / exp;
                int eIn = n % exp;

                if (eOut != 0) mapOut.Add(t, eOut);
                if (eIn != 0) mapIn.Add(t, eIn);
                done.Add(t);
            }
            if (mapOut.Count == 0) return null;

            Formula fOut = BuildFormulaFromMap(mapOut);
            Formula fIn = BuildFormulaFromMap(mapIn);

            return Tuple.Create(fOut, fIn);
        }

        /// <summary>
        /// 数式-指数マップから数式を構成して取得します。
        /// </summary>
        /// <param name="map">対象の数式-指数マップ。</param>
        /// <returns>構成された数式。</returns>
        private static Formula BuildFormulaFromMap(Dictionary<Formula, int> map)
        {
            var list = new List<Formula>();
            foreach (var o in map)
            {
                if (o.Value == 1) list.Add(o.Key);
                else list.Add(o.Key ^ o.Value);
            }
            Formula f = 1;
            if (list.Count == 1) f = list[0];
            else if (list.Count > 1) f = new Product(list.ToArray());

            return f;
        }

        /// <summary>
        /// 指定した数式を構成する全ての因子リストを取得します。係数となる整数については、素因数分解して因子リストを構成します。
        /// </summary>
        /// <param name="gcd">因子リストを取得する対象の数式。</param>
        /// <returns>因子リスト。</returns>
        private List<Formula> GetFactorsFrom(Formula gcd)
        {
            var result = new List<Formula>();
            if (gcd is Numeric) result.AddRange(GetFactorsFromNumeric(gcd as Numeric));
            else if (gcd is AtomicFormula) result.Add(gcd);
            else
            {
                var a = new RulePatternVariable("a");
                a.AdmitMultiplyOne = true;
                a.CheckTarget = (f => f is Numeric);

                var b = new RulePatternVariable("b");
                b.CheckTarget = (f => !(f is Numeric));

                var rule = a * b;
                if (!gcd.PatternMatch(rule)) result.Add(gcd);
                else
                {
                    result.AddRange(GetFactorsFromNumeric(a.MatchedFormula as Numeric));
                    result.AddRange(GetFactorsFromNonNumeric(b.MatchedFormula));
                }
            }
            return result;
        }

        private IEnumerable<Formula> GetFactorsFromNumeric(Numeric n)
        {
            if (!n.IsInteger) yield return n;
            else
            {
                var fc = PrimeFactor.PrimeFactorize(n, false);
                if (fc is Product) foreach (var c in fc) yield return c;
                else yield return fc;
            }
        }

        private IEnumerable<Formula> GetFactorsFromNonNumeric(Formula f)
        {
            if (f is Power)
            {
                Numeric exp = (f as Power).Exponent as Numeric;
                if (exp == null) yield return f;
                else
                {
                    var b = (f as Power).Base;
                    foreach (var c in GetFactorsFrom(b))
                    {
                        for (int i = 0; i < Math.Abs(exp); i++)
                        {
                            if (exp < 0) yield return c ^ -1;
                            else yield return c;
                        }
                    }
                }
            }
            else if (f is Product)
            {
                foreach (var t in f)
                    foreach (var c in GetFactorsFromNonNumeric(t)) yield return c;
            }
            else
                yield return f;
        }


        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(CombineProductToPowerRule); // 逆変換の展開傾向
        }

        protected override IEnumerable<Type> OnGetPostDemandRules()
        {
            yield return typeof(CalculateFunctionRule); // √8 → 8^(1/2)
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return new CombineProductToPowerRule(); // 5*5^(1/2) → 5^(3/2)
        }


        public override string Information
        {
            get { return "√の中身の整理を規定するルールです。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("sqrt(8*a^3)"),
                Formula.Parse("(2*a)*sqrt(2*a)")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("sqrt(120*a+80)"),
                Formula.Parse("2*sqrt(30*a+20)")
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
