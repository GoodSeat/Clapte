using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Extensions;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Comparers;

namespace GoodSeat.Liffom.Processes
{
    /// <summary>
    /// 代数方程式を代数的に解く処理を表します。
    /// </summary>
    public class SolveAlgebraicEquation : SolveEquation
    {
        /// <summary>
        /// 一意のユーザー情報を指定して、複数の同時呼び出しを許可するか否かを取得します。
        /// </summary>
        public override bool IsSupportMultipleConcurrentInvocations { get { return false; } }



        /// <summary>
        /// 解に虚数を許容するか否かを設定もしくは取得します。
        /// </summary>
        public bool AdmitImaginary { get; set; }

        /// <summary>
        /// 現在の対象変数の最大指数を設定もしくは取得します。
        /// </summary>
        private int MaxExponent { get; set; }

        /// <summary>
        /// 現在の対象変数の最小指数を設定もしくは取得します。
        /// </summary>
        private int MinExponent { get; set; }

        /// <summary>
        /// 現在の対象変数の指数とその係数の対応マップを設定もしくは取得します。
        /// </summary>
        private Dictionary<int, Formula> CoefficientMap { get; set; }



        /// <summary>
        /// 指定された数式に対して、処理を実行します。
        /// </summary>
        /// <param name="target">求解対象の等式</param>
        /// <param name="about">求解対象の変数</param>
        /// <param name="userState">一意のユーザー状態</param>
        public override Equal Solve(object userState, Equal target, Variable about)
        {
            if (!target.Contains(about)) throw new FormulaProcessException("対象の等式の中に、変数 " + about.ToString() + " が存在しません。");

            target = GetCollected(target, about); // aboutについて整理
            SetCoefficientMap(target, about);
            SetExponentInformation();

            var solutions = new List<Formula>(); // 解リスト
            if (MinExponent > 0) solutions.Add(0); // ax^2 + bx = 0 など
            ModifyCoefficientMap(); // MinExponent を0にする

            int gcd = 0;
            var result = TryReduceDegree(about, ref target, out gcd);
            if (result != null) return new Equal(about, result);
            SetExponentInformation();

            if (MaxExponent < 1)
                throw new FormulaProcessException("対象の数式の解は、解の公式で求めることはできません。");
            else if (MaxExponent == 1) // 1次式
                solutions.Add(GetSolution(GetFactor(1), GetFactor(0)));
            else if (MaxExponent == 2) // 2次式
                solutions.AddRange(GetSolution(GetFactor(2), GetFactor(1), GetFactor(0)));
            else if (MaxExponent == 3) // 3次式
                solutions.AddRange(GetSolution(GetFactor(3), GetFactor(2), GetFactor(1), GetFactor(0)));
            else if (MaxExponent == 4)
                throw new FormulaProcessException("解の公式による4次式の求解はサポートされていません。"); // 4次式            
            else 
                throw new FormulaProcessException("解の公式によって5次以上の高次式を求解することはできません。");

            solutions = GetFormalSolutions(target, about, solutions);

            if (solutions.Count == 0) throw new FormulaProcessException("解は見つかりませんでした。");

            if (gcd == 1)
            {
                if (solutions.Count == 1) return new Equal(about, solutions[0]);
                else return new Equal(about, new Argument(solutions.ToArray()));
            }
            else // 求められた解 y について、 x^gcd = y である
            {
                var list = new List<Formula>();
                foreach (var sol in solutions)
                {
                    var solver = new SolveAlgebraicEquation();
                    var s = solver.Solve(new Equal(about ^ new Numeric(gcd), sol), about).RightHandSide;

                    if (s is Argument) list.AddRange(s);
                    else list.Add(s);
                }
                if (list.Count == 1) return new Equal(about, list[0]);
                else return new Equal(about, new Argument(list.ToArray()));
            }
        }

        /// <summary>
        /// 求解対象の方程式の次元を下げることを試みます。
        /// </summary>
        /// <param name="about">求解対象の変数。</param>
        /// <param name="target">求解対象の等式。次元を下げることに成功した場合、次元を下げた後の等式。</param>
        /// <param name="gcd">次元を除すことのできる最大公約数。次元を下げることができない場合、1。</param>
        /// <returns>明らかな解が見つかった場合はその解。それ以外の場合、null。</returns>
        private Formula TryReduceDegree(Variable about, ref Equal target, out int gcd)
        {
            gcd = MaxExponent;
            for (int i = 1; i < MaxExponent; i++)
            {
                if (GetFactor(i) == 0) continue;
                gcd = (int)Polynomial.GCD(new Numeric(gcd), new Numeric(i));
                if (gcd == 1) return null;
            }

            // いくら高次でも、a x^n + b = 0 は、 x = (-b/a)^(1/n)とできる
            if (gcd == MaxExponent)
            {
                if (gcd <= 2)
                {
                    gcd = 1;
                    return null; // ただし3次以下は普通に解く
                }
                else
                {
                    return (-GetFactor(0) / GetFactor(MaxExponent)).Simplify() ^ (1 / new Numeric(MaxExponent));
                }
            }

            // x^6 + 3 x^3 - 9 = 0 は、y^2 + 3y - 9 = 0 where y = x^3 として解くくことができる
            for (int i = 1; i <= MaxExponent / gcd; i++)
            {
                var factor = GetFactor(i * gcd);
                CoefficientMap.Remove(i * gcd);
                CoefficientMap.Add(i, factor);
            }
            var lhs = GetFactor(0);
            for (int i = 1; i <= MaxExponent / gcd; i++) lhs += GetFactor(i) * (about ^ new Numeric(i));
            target.LeftHandSide = lhs.Simplify();
            return null;
        }

        ///
        /// <summary>
        /// 指定式を、<see cref="about"/>に関して整理して取得します。
        /// </summary>
        /// <param name="target">整理対象の等式</param>
        /// <param name="about">整理対象の等式</param>
        /// <returns></returns>
        private Equal GetCollected(Equal target, Variable about)
        {
            target.LeftHandSide -= target.RightHandSide;
            target.RightHandSide = 0;
            target.LeftHandSide = target.LeftHandSide.DeformFormula(new CollectToken(about));
            return target;
        }

        /// <summary>
        /// <paramref name="target"/>に関して係数の対応マップを作成して<see cref="CoefficientMap"/>にセットします。
        /// </summary>
        /// <param name="target">係数の対応マップの作成対象の等式</param>
        private void SetCoefficientMap(Equal target, Variable about)
        {
            CoefficientMap = new Dictionary<int, Formula>();

            // ルールを作成
            RulePatternVariable a = new RulePatternVariable("a");
            a.AdmitMultiplyOne = true;
            RulePatternVariable b = new RulePatternVariable("b");
            b.AdmitPowerOne = true;
            b.CheckTarget = f => Numeric.IsNumericOnly(f) ;
            Formula rule = a * (about ^ b);
            Formula rule2 = a * ((about ^ b) ^ -1);

            List<Formula> noCoefficients = new List<Formula>();
            Formula surplus = null; // ax^2.7 + x^1.7 = 0 → ax^2 + x^1 = 0 として解くために、このケースではsurplus=0.7として記録しておく
            if (target.LeftHandSide is Sum)
            {
                foreach (var f in target.LeftHandSide)
                {
                    Formula n = null;
                    if (f.PatternMatch(rule)) n = b.MatchedFormula;
                    else if (f.PatternMatch(rule2)) n = b.MatchedFormula * -1;

                    if (n != null)
                    {
                        if (surplus == null)
                        {
                            Numeric num = n.Numerate() as Numeric;
                            int round = (int)num.Figure;
                            surplus = (n - round).Combine();
                        }
                        Numeric coef = (n - surplus).Numerate() as Numeric;
                        if (!coef.IsInteger) throw new FormulaProcessException("対象の数式の解は、解の公式で求めることはできません。");
                        CoefficientMap.Add((int)coef, a.MatchedFormula);
                    }
                    else
                    {
                        noCoefficients.Add(f);
                    }
                }
            }
            else
            {
                if (!target.LeftHandSide.PatternMatch(rule) && !target.LeftHandSide.PatternMatch(rule2)) throw new FormulaProcessException("対象の数式の解は、解の公式で求めることはできません。");

                if (a.MatchedFormula == 0) throw new FormulaProcessException("解は不定です。");
                noCoefficients.Add(a.MatchedFormula);
            }

            if (noCoefficients.Count != 0) // aboutのかからない0次項を作成
            {
                if (surplus != null && surplus != 0) throw new FormulaProcessException("対象の数式の解は、解の公式で求めることはできません。");

                if (noCoefficients.Count == 1) CoefficientMap.Add(0, noCoefficients[0]);
                else CoefficientMap.Add(0, new Sum(noCoefficients.ToArray()));
            }
        }

        /// <summary>
        /// CoefficientMapに基づいて、最大指数、最小指数の情報を更新します。
        /// </summary>
        private void SetExponentInformation()
        {
            // 最大指数と最小指数をセット
            bool initial = true;
            foreach (var pair in CoefficientMap)
            {
                if (initial)
                {
                    MinExponent = pair.Key;
                    MaxExponent = pair.Key;
                    initial = false;
                }
                else
                {
                    MinExponent = Math.Min(pair.Key, MinExponent);
                    MaxExponent = Math.Max(pair.Key, MaxExponent);
                }
            }
        }

        /// <summary>
        /// MinExponentの情報に基づき、MinExponent=0となるように<see cref="CoefficientMap"/>を調整します。
        /// </summary>
        private void ModifyCoefficientMap()
        {
            var temp = new List<KeyValuePair<int, Formula>>();

            foreach (var pair in CoefficientMap) temp.Add(new KeyValuePair<int,Formula>(pair.Key - MinExponent, pair.Value));
            CoefficientMap.Clear();
            foreach (var pair in temp) CoefficientMap.Add(pair.Key, pair.Value);

            SetExponentInformation();
        }

        /// <summary>
        /// <see cref="CoeffiecntMap"/>から、指定した指数の係数を取得します。
        /// </summary>
        /// <param name="exponent">取得対象の指数</param>
        /// <returns>係数</returns>
        private Formula GetFactor(int exponent)
        {
            Formula f;
            if (!CoefficientMap.TryGetValue(exponent, out f)) f = 0;
            return f;
        }

        /// <summary>
        /// ax + b = 0 の 解x を取得します。
        /// </summary>
        /// <param name="a">a（1次係数）</param>
        /// <param name="b">b（0次係数）</param>
        /// <returns></returns>
        private Formula GetSolution(Formula a, Formula b)
        {
            return (-b / a).Combine();
        }

        /// <summary>
        /// ax^2 + bx + c = 0 の 解x を取得します。
        /// </summary>
        /// <param name="a">a（2次係数）</param>
        /// <param name="b">b（1次係数）</param>
        /// <param name="c">c（0次係数）</param>
        /// <returns></returns>
        private List<Formula> GetSolution(Formula a, Formula b, Formula c)
        {
            Formula pb = ((b ^ 2) - 4 * a * c).Simplify();
            Formula x1 = (-b + new Root(pb)) / (2 * a);
            Formula x2 = (-b - new Root(pb)) / (2 * a);

            x1 = x1.Simplify();
            x2 = x2.Simplify();

            List<Formula> ret = new List<Formula>();
            ret.Add(x1);
            if (pb != 0) ret.Add(x2);

            return ret;
        }

        /// <summary>
        /// ax^3 + bx^2 + cx + d = 0 の 解x を取得します。
        /// ref:http://hooktail.sub.jp/algebra/CubicEquation/
        /// </summary>
        /// <param name="a">a（3次係数）</param>
        /// <param name="b">b（2次係数）</param>
        /// <param name="c">c（1次係数）</param>
        /// <param name="d">d（0次係数）</param>
        /// <returns></returns>
        static List<Formula> GetSolution(Formula a, Formula b, Formula c, Formula d)
        {
            var deformToken = new DeformToken(Formula.SimplifyToken, Formula.NumerateToken, Formula.CalculateToken);

            Formula i = Imaginary.i;
            Numeric n2 = new Numeric(2d);
            Numeric n3 = new Numeric(3d);

            Formula A = (b / a).Simplify();
            Formula B = (c / a).Simplify();
            Formula C = (d / a).Simplify();

            Formula q = ((C + 2 * (A ^ 3) / new Numeric(27d) - 1 / n3 * A * B) / n2).DeformFormula(deformToken);
            Formula p = ((B - 1 / n3 * (A ^ 2)) / n3).DeformFormula(deformToken);

            Formula w1 = 1;
            Formula w2 = ((-1 + i * (n3 ^ 0.5)) / n2).DeformFormula(deformToken);
            Formula w3 = ((-1 - i * (n3 ^ 0.5)) / n2).DeformFormula(deformToken);

            Formula mid = (((q ^ 2) + (p ^ 3)) ^ 0.5).DeformFormula(deformToken);

            Formula uBase = (-q + mid) ^ (n3 ^ -1);
            uBase = uBase.Simplify().DeformFormula(deformToken);
            Formula u1 = (w1 * uBase).DeformFormula(deformToken);
            Formula u2 = (w2 * uBase).DeformFormula(deformToken);
            Formula u3 = (w3 * uBase).DeformFormula(deformToken);

            Formula vBase = (-q - mid) ^ (n3 ^ -1);
            vBase = vBase.Simplify().DeformFormula(deformToken);

            Formula v1 = (w1 * vBase).DeformFormula(deformToken);
            Formula v2 = (w2 * vBase).DeformFormula(deformToken);
            Formula v3 = (w3 * vBase).DeformFormula(deformToken);

            Formula x1 = u1 + v1 - 1d / n3 * A;
            Formula x2 = u2 + v3 - 1d / n3 * A;
            Formula x3 = u3 + v2 - 1d / n3 * A;

            x1 = x1.DeformFormula(deformToken);
            x2 = x2.DeformFormula(deformToken);
            x3 = x3.DeformFormula(deformToken);

            List<Formula> ret = new List<Formula>();
            ret.Add(x1);
            if (x2 != x1) ret.Add(x2);
            if (x3 != x1 && x3 != x2) ret.Add(x3);

            return ret;
        }


        /// <summary>
        /// 解候補リストをもとに、<see cref="target"/>の解リストを作成して取得します。
        /// </summary>
        /// <param name="target">方程式</param>
        /// <param name="about">求解対象の変数</param>
        /// <param name="solutions">解の候補リスト</param>
        /// <returns></returns>
        private List<Formula> GetFormalSolutions(Equal target, Variable about, List<Formula> solutions)
        {
            List<Formula> result = new List<Formula>();
            foreach (Formula sol in solutions)
            {
                if (sol.Contains(about)) continue; // 解に対象の変数が含まれていたら無効

                // 虚数解で、実数部と虚数部で1.E+13以上の桁数差がある場合、小さい桁の方を誤差として切り捨てる。
                Formula solRounded = sol;
                foreach (Formula imaginary in sol.GetExistFactor(test=>Imaginary.IsComplexNumber(test, false)))
                    solRounded = solRounded.Substitute(imaginary, Imaginary.RoundImaginary(imaginary, Numeric.MaxPrecision));

                if (!AdmitImaginary && solRounded.Contains(Imaginary.i)) continue;

                result.Add(GetModifiedSolution(target.LeftHandSide, about, solRounded, 0));
            }
            result.Sort();
            return result;
        }


    }
}
