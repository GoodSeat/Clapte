using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Liffom.Reals;

namespace GoodSeat.Liffom.Processes
{
    /// <summary>
    /// ブレント法による解の算出処理を表します。
    /// </summary>
    /// <see cref="http://en.wikipedia.org/wiki/Brent%27s_method"/>
    /// <see cref="http://ja.wikipedia.org/wiki/%E3%83%96%E3%83%AC%E3%83%B3%E3%83%88%E6%B3%95"/>
    public class BrentMethod : SolveEquation
    {
        /// <summary>
        /// 一意のユーザー情報を指定して、複数の同時呼び出しを許可するか否かを取得します。
        /// </summary>
        public override bool IsSupportMultipleConcurrentInvocations { get { return true; } }

        double _errorTolerance = 1E-6;            //    許容誤差
        int _maxTryCount = 500;        // 最大試行回数

        Numeric _lowerLimit = new Numeric(-1000d), _upperLimit = new Numeric(1000d);

        
        /// <summary>
        /// 解の許容誤差を設定もしくは取得します。
        /// </summary>
        public double ErrorTolerance
        {
            get { return _errorTolerance; }
            set { _errorTolerance = value; }
        }

        /// <summary>
        /// 最大試行回数を設定もしくは取得します。
        /// </summary>
        public int MaxTryCount
        {
            get { return _maxTryCount; }
            set { _maxTryCount = value; }
        }

        /// <summary>
        /// 解の存在上限値を設定もしくは取得します。
        /// </summary>
        public Numeric UpperLimit
        {
            get { return _upperLimit; }
            set { _upperLimit = value; }
        }

        /// <summary>
        /// 解の存在下限値を設定もしくは取得します。
        /// </summary>
        public Numeric LowerLimit
        {
            get { return _lowerLimit; }
            set { _lowerLimit = value; }
        }



        /// <summary>
        /// 指定された数式に対して、処理を実行します。
        /// </summary>
        /// <param name="target">求解対象の等式。</param>
        /// <param name="about">求解対象の変数。</param>
        /// <param name="userState">一意のユーザー状態。</param>
        public override Equal Solve(object userState, Equal target, Variable about)
        {
            return GetSolution(target, about, userState);
        }

        /// <summary>
        /// ブレント法により解を求めます。
        /// </summary>
        /// <param name="target">対象方程式。</param>
        /// <param name="x">求める変数。</param>
        /// <param name="userState">一意のユーザー状態。</param>
        /// <returns>求められた解。ただし、最大試行回数まで解が見つからなかった場合、null。</returns>
        private Equal GetSolution(Equal target, Variable x, object userState)
        {
            if (!target.Contains(x))
                throw new FormulaProcessException(string.Format("数式「{0}」に、求解対象の変数{1}が存在しません。", target, x));
#if DEBUG
            DateTime start = DateTime.Now;
            Console.WriteLine("ブレント法による解の算出開始：" + start.ToLongTimeString());
#endif
            Formula f = (target[0] - target[1]).Numerate().Combine(); // 対象の方程式 f(x) = 0 を解く。
            if (f.Contains(v => (v is Variable && v != x)))
                throw new FormulaProcessException(string.Format("数式「{0}」に{1}以外の変数が存在するため、ブレント法で解くことができません。", target, x));

            Numeric upper = UpperLimit.Copy() as Numeric;
            Numeric lower = LowerLimit.Copy() as Numeric;

            // それなりに区間を探す
            SearchLimit(f, x, ref upper, ref lower, userState);

            Numeric solution = null;
            if (!IsCanceled(userState)) solution = GetSolution(f, x, upper, lower, userState);
#if DEBUG
            DateTime end = DateTime.Now;
            Console.WriteLine("ブレント法による解の算出終了：計算時間：" + (end - start).ToString());
#endif
            if (IsCanceled(userState)) return null;
            return new Equal(x, GetModifiedSolution(f, x, solution, ErrorTolerance));
        }

        /// <summary>
        /// Numerical Recipes in Cによるブレント法処理を実行します。
        /// </summary>
        /// <param name="f">f(x)=0における、f(x)。</param>
        /// <param name="x">f(x)=0における、x。</param>
        /// <param name="lowerLimit">解の探索下限値。</param>
        /// <param name="upperLimit">解の探索上限値。</param>
        /// <param name="userState">一意のユーザー状態。</param>
        /// <returns>求められた解。</returns>
        public Numeric GetSolution(Formula f, Variable x, Numeric upperLimit, Numeric lowerLimit, object userState)
        {
            var token = new DeformToken(Formula.SimplifyToken, Formula.CalculateToken, Formula.NumerateToken);

            Real a = lowerLimit.Figure;
            Real b = upperLimit.Figure;
            Real c = b;
            Real d = new Numeric(0).Figure;
            Real e = new Numeric(0).Figure;

            Real fa = (f.Substituted(x, a).DeformFormula(token) as Numeric).Figure;
            Real fb = (f.Substituted(x, b).DeformFormula(token) as Numeric).Figure;

            if (fa == 0) return a;
            if (fb == 0) return b;

            // if f(a) f(b) >= 0 then error-exit
            Real calculated = fa * fb;
            if (calculated == null || calculated.IsNaN) throw new FormulaProcessException("区間の上限と下限を正しく設定できませんでした。");
            if (fa * fb >= 0)
            {
                throw new FormulaProcessException(
                        string.Format("区間の上限(a = {0})と下限(b = {1})で関数の計算値の正負が同じです。f(a) = {2}, f(b) = {3}",
                            a, b, fa, fb));
            }

            Real fc = fb;
            for (int i = 0; i < MaxTryCount; i++)
            {
                if (IsCanceled(userState)) return null;
                Formula.CheckCancelOperation(f);

                if (fb * fc > 0)
                {
                    c = a;
                    fc = fa;
                    e = d = b - a;
                }
                if (Math.Abs(fc) < Math.Abs(fb))
                {
                    a = b; b = c; c = a;
                    fa = fb; fb = fc; fc = fa;
                }

                Real tol1 = (new Numeric(0.5 * ErrorTolerance)).Figure;
                Real xm = new Numeric(0.5 * (c - b));
                if (Math.Abs(xm) <= tol1 || fb == 0) return b;
                if (Math.Abs(e) >= tol1 && Math.Abs(fa) > Math.Abs(fb))
                {
                    Real s = fb / fa;
                    Real p, q;
                    if (a == c)
                    {
                        p = new Numeric(2d * xm * s);
                        q = new Numeric(1d - s);
                    }
                    else
                    {
                        q = fa / fc;
                        var r = fb / fc;
                        p = new Numeric(s * (2 * xm * q * (q - r) - (b - a) * (r - 1)));
                        q = new Numeric((q - 1) * (r - 1) * (s - 1));
                    }
                    if (p > 0) q = -q; // 区間内かどうかチェック

                    p = new Numeric(Math.Abs(p));
                    Real min1 = new Numeric(3 * xm * q - Math.Abs(tol1 * q));
                    Real min2 = new Numeric(Math.Abs(e * q));
                    if (2 * p < (min1 < min2 ? min1 : min2)) // 補間値を採用
                    {
                        e = d;
                        d = p / q;
                    }
                    else // 補間失敗、二分法を使う
                    {
                        d = xm;
                        e = d;
                    }
                }
                else // 区間幅の減少が遅すぎるので二分法を使う
                {
                    d = xm;
                    e = d;
                }
                a = b;
                fa = fb;
                if (Math.Abs(d) > tol1) b = b + d;
                else b = b + (xm > 0 ? tol1 : -tol1);
                fb = (f.Substituted(x, b).DeformFormula(token) as Numeric).Figure;
#if DEBUG
                Console.WriteLine(string.Format("{0}回目の試行:a={1}, b={2}", i, a, b));
#endif
            } 
            throw new FormulaProcessException(MaxTryCount + "の試行回数内では、指定誤差値以内に解が収束しませんでした。");
        }

        /// <summary>
        /// 指定範囲において関数が正負をまたがない場合に、適当な範囲を疑似2分法により探します。
        /// </summary>
        /// <param name="f">f(x)=0における、f(x)。</param>
        /// <param name="x">f(x)=0における、x。</param>
        /// <param name="upperLimit">解探索範囲の上限値。</param>
        /// <param name="lowerLimit">解探索範囲の下限値。</param>
        /// <param name="userState">一意のユーザー状態。</param>
        private void SearchLimit(Formula f, Variable x, ref Numeric upperLimit, ref Numeric lowerLimit, object userState)
        {
            var token = new DeformToken(Formula.SimplifyToken, Formula.CalculateToken, Formula.NumerateToken);

            // それなりに区間を探す
            Numeric baseLower = lowerLimit;
            Numeric baseUpper = upperLimit;
            for (int i = 0; i < 3; i++)
            {
                if (IsCanceled(userState)) return;

                lowerLimit = baseLower;
                upperLimit = baseUpper;

                int count = 0;
                Numeric fa = f.Substituted(x, lowerLimit).DeformFormula(token) as Numeric;
                Numeric fb = f.Substituted(x, upperLimit).DeformFormula(token) as Numeric;

                Numeric calculated = (fa * fb).Numerate() as Numeric;
                while ((calculated == null || calculated.Figure.IsInfinity || calculated.Figure.IsNaN || calculated > 0) && count++ < 20)
                {
                    if (IsCanceled(userState)) return;
                    Formula.CheckCancelOperation(f);

                    if (i == 0)
                    {
                        lowerLimit = (lowerLimit + (upperLimit - lowerLimit) / 2.0).Numerate() as Numeric;
                        fa = f.Substituted(x, lowerLimit).DeformFormula(token) as Numeric;
                    }
                    else if (i == 1)
                    {
                        if (baseLower < 0) //  L U +  or  L + U
                        {
                            lowerLimit = (2 * lowerLimit).Numerate() as Numeric;
                            fa = f.Substituted(x, lowerLimit).DeformFormula(token) as Numeric;
                        }
                        else // + L U
                        {
                            upperLimit = (2 * upperLimit).Numerate() as Numeric;
                            fb = f.Substituted(x, upperLimit).DeformFormula(token) as Numeric;
                        }
                    }
                    else if (i == 2)
                    {
                        if (baseLower < 0 && baseUpper > 0) // L + U
                        {
                            upperLimit = (2 * upperLimit).Numerate() as Numeric;
                            fb = f.Substituted(x, upperLimit).DeformFormula(token) as Numeric;
                        }
                        else if (baseLower < 0) // L U +
                        {
                            if (upperLimit < 0) upperLimit = (-1 * upperLimit).Numerate() as Numeric;
                            else upperLimit = (2 * upperLimit).Numerate() as Numeric;
                            fb = f.Substituted(x, upperLimit).DeformFormula(token) as Numeric;
                        }
                        else // + L U
                        {
                            if (lowerLimit > 0) lowerLimit = (-1 * lowerLimit).Numerate() as Numeric;
                            else lowerLimit = (2 * lowerLimit).Numerate() as Numeric;
                            fa = f.Substituted(x, lowerLimit).DeformFormula(token) as Numeric;
                        }
                    }
                    calculated = (fa * fb).Numerate() as Numeric;
                }

                calculated = (fa * fb).Numerate() as Numeric;
                if (calculated != null && !calculated.Figure.IsInfinity && !calculated.Figure.IsNaN && calculated <= 0) break;
            }
        }
    }
}
