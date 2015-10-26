using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Liffom.Formulas.Rules;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Reals;

namespace GoodSeat.Liffom.Processes
{
    /// <summary>
    /// 方程式の求解処理を表します。
    /// </summary>
    public abstract class SolveEquation : Process
    {
        /// <summary>
        /// 処理対象とする数式数の下限値を取得します。
        /// </summary>
        public override int TargetArgumentsMinQty { get { return 2; } }


        /// <summary>
        /// 指定された数式に対して、処理を実行します。
        /// </summary>
        protected override Formula OnDo(object userState, params Formula[] targets)
        {
            return Solve(userState, targets[0] as Equal, targets[1] as Variable);
        }

        /// <summary>
        /// 指定された数式に対して、処理を実行します。
        /// </summary>
        /// <param name="target">求解対象の等式。</param>
        /// <param name="about">求解対象の変数。</param>
        /// <returns>方程式の解を表す等式。解が複数の場合は、x=(x1,x2)のように、引数を解として表現します。</returns>
        public Equal Solve(Equal target, Variable about)
        {
            return Solve(DefaultState, target, about);
        }

        /// <summary>
        /// 指定された数式に対して、処理を実行します。
        /// </summary>
        /// <param name="target">求解対象の等式。</param>
        /// <param name="about">求解対象の変数。</param>
        /// <param name="userState">一意のユーザー状態。</param>
        /// <returns>方程式の解を表す等式。解が複数の場合は、x=(x1,x2)のように、引数を解として表現します。</returns>
        public abstract Equal Solve(object userState, Equal target, Variable about);


        /// <summary>
        /// 得られた近似解と判定誤差値に基づいて、真値を取得します。
        /// </summary>
        /// <param name="f">f(x)=0 における f(x)。</param>
        /// <param name="x">f(x)=0 における x。</param>
        /// <param name="solution">近似解。</param>
        /// <param name="error">近似解を求めるのに使用した許容誤差値。</param>
        /// <returns>解の真値。</returns>
        protected static Formula GetModifiedSolution(Formula f, Variable x, Formula solution, double error)
        {
            // 解のまるめ (f(x)にxを代入したとき、ちゃんと0となるまで解を丸める)
            solution = GetRoundSolution(f, x, solution, error);

            // 解の有効桁数を無限大としてf(x)に代入した時の有効桁数を取得
            int initialValidDigit, initialPrecision;
            bool needCheckValid = GetInitialPrecision(f, x, solution, out initialValidDigit, out initialPrecision);

            // 有効桁数の設定
            if (needCheckValid) solution = GetPrecisionModifiedSolution(f, x, solution, initialValidDigit);

            return solution;
        }

        /// <summary>
        /// 方程式f(x)=0において、xに解を代入したときに左辺が0となるまで解を丸めます。
        /// </summary>
        /// <param name="f">方程式 f(x)=0 におけるf(x)。</param>
        /// <param name="x">方程式 f(x)=0 における x。</param>
        /// <param name="solution">検討対象の解。</param>
        /// <param name="error">計算時に用いた許容誤差値。</param>
        /// <returns></returns>
        private static Formula GetRoundSolution(Formula f, Variable x, Formula solution, double error)
        {
            var token = new DeformToken(Formula.SimplifyToken, Formula.CalculateToken, Formula.NumerateToken);

            int testDigit = (new Numeric(error)).Figure.Exponent + 1; // 許容誤差値の正規化時の指数
            int minExponent = testDigit;
            foreach (Numeric n in solution.GetExistFactor<Numeric>())
                minExponent = Math.Min(minExponent, n.Figure.Exponent - 15);

            for (int test = testDigit; test >= minExponent; test--)
            {
                Formula checkSolve = solution.Copy();
                foreach (Numeric n in checkSolve.GetExistFactor<Numeric>()) n.Figure = n.Figure.Round(-test) as SignificantReal;

                Formula ans = f.Substituted(x, checkSolve).DeformFormula(token);
                if (ans == 0) return checkSolve;
            }
            return solution;
        }

        /// <summary>
        /// 有効桁数を無限大とした解を f(x)=0 に代入した時の左辺の有効桁数から、解の有効桁数の調整が必要か否かを判定して取得します。
        /// </summary>
        /// <param name="f">f(x)=0 における f(x)。</param>
        /// <param name="x">f(x)=0 における x。</param>
        /// <param name="solution">方程式の解。</param>
        /// <param name="initialValidDigit">有効桁数を無限大とした解を代入したときの左辺の最大有効桁。</param>
        /// <param name="initialPrecision">有効桁数を無限大とした解を代入したときの左辺の有効桁数。</param>
        /// <returns>解の有効桁数調整が必要か否か。</returns>
        private static bool GetInitialPrecision(Formula f, Variable x, Formula solution, out int initialValidDigit, out int initialPrecision)
        {
            var token = new DeformToken(Formula.SimplifyToken, Formula.CalculateToken, Formula.NumerateToken);

            // 初期解にて計算した時の解の有効数字を取得
            var copy = solution.Copy();
            foreach (Numeric n in copy.GetExistFactor<Numeric>()) n.SignificantDigits = 100;
            initialValidDigit = int.MinValue; // 初期解で計算したときの最大有効桁
            initialPrecision = 100; // 初期解で計算したときの有効桁数
            Formula checkDigit = f.Substituted(x, copy).DeformFormula(token);

            bool needCheckValid = false;
            foreach (Numeric n in checkDigit.GetExistFactor<Numeric>())
            {
                if (n.SignificantDigits <= Numeric.MaxValidDigits) needCheckValid = true;
                initialValidDigit = Math.Max(n.Figure.Exponent - n.SignificantDigits + 1, initialValidDigit);
                initialPrecision = Math.Min(n.SignificantDigits, initialPrecision);
            }
            return needCheckValid;
        }

        /// <summary>
        /// 方程式の左辺の有効桁数をもとに、解の有効桁数を調整して取得します。
        /// </summary>
        /// <param name="f">f(x)=0 における f(x)。</param>
        /// <param name="x">f(x)=0 における x。</param>
        /// <param name="solution">初期解。</param>
        /// <param name="initialValidDigit">解の有効桁数を無限大としたときの f(x) の最大有効桁。</param>
        /// <returns></returns>
        private static Formula GetPrecisionModifiedSolution(Formula f, Variable x, Formula solution, int initialValidDigit)
        {
            var token = new DeformToken(Formula.SimplifyToken, Formula.CalculateToken, Formula.NumerateToken);

            for (int i = 1; i <= Numeric.MaxValidDigits; i++)
            {
                Formula checkSolution = solution.Copy();
                foreach (Numeric n in checkSolution.GetExistFactor<Numeric>())
                {
                    n.SignificantDigits = i;
                    n.Figure = new Numeric(double.Parse(n.Figure.ToString())).Figure;
                }

                Formula checkDigitResult = f.Substituted(x, checkSolution).DeformFormula(token);
                int postValidDigit = int.MinValue;
                foreach (Numeric n in checkDigitResult.GetExistFactor<Numeric>()) // checkDigitResultは、0.E-3、0.E+2[kN*m^2] 等のはず
                {
                    int validDigit = n.Figure.Exponent - n.SignificantDigits + 1;
                    if (n.Figure.Round(-validDigit) != 0d) continue; // 左辺-右辺が0になっていないならだめ。

                    postValidDigit = Math.Max(validDigit, postValidDigit);
                }
                if (postValidDigit == int.MinValue) continue;

                if (postValidDigit <= initialValidDigit)
                {
                    foreach (Numeric n in solution.GetExistFactor<Numeric>())
                    {
                        if (n.SignificantDigits == 100) continue;
                        n.SignificantDigits = i;
                    }
                    return solution;
                }
            }
            return solution;
        }

    }
}
