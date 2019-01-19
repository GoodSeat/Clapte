using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Processes
{
    /// <summary>
    /// 連立方程式の求解処理を表します。
    /// </summary>
    public class SolveSimultaneousEquation : Process
    {
        public override bool IsSupportMultipleConcurrentInvocations
        {
            get { return true; }
        }

        /// <summary>
        /// 処理対象とする数式数の下限値を取得します。
        /// </summary>
        public override int TargetArgumentsMinQty { get { return 2; } }

        /// <summary>
        /// 処理対象とする数式数の上限値を取得します。
        /// </summary>
        public override int TargetArgumentsMaxQty { get { return int.MaxValue; } }

        /// <summary>
        /// 求められた解に対して、有効桁数の設定処理を行うか否かを設定もしくは取得します。
        /// </summary>
        public bool CheckSignificantDigits { get; set; }

        /// <summary>
        /// 等式リストを指定して、連立方程式の解を非同期に算出します。
        /// </summary>
        /// <param name="formulas">求階対象の等式リスト。</param>
        /// <param name="targets">解を算出する可変数の変数。</param>
        public void SolveAsync(List<Equal> formulas, params Variable[] targets)
        {
            var list = new List<Formula>();
            list.AddRange(formulas);
            list.Add(new Argument(targets));
            DoAsync(list.ToArray());
        }

        /// <summary>
        /// 等式リストを指定して、連立方程式の解を算出します。
        /// </summary>
        /// <param name="formulas">求解対象の等式リスト。</param>
        /// <param name="targets">解を算出する可変数の変数。</param>
        /// <returns>解を表す等式リスト。</returns>
        public List<List<Equal>> Solve(List<Equal> formulas, params Variable[] targets)
        {
            var fs = new List<Equal>(formulas.Select(f => f.Simplify() as Equal));
            var xs = new List<Variable>(targets);

            return SolveRest(fs, xs);
        }

        /// <summary>
        /// 求解対象の可変数の方程式、及び求解対象の変数からなる引数から、連立方程式の解を算出します。
        /// </summary>
        /// <param name="userState">一意のユーザー状態。</param>
        /// <param name="targets">求解対象の可変数の方程式。ただし、最後の要素は求解対象の変数からなる引数。</param>
        /// <returns>解を表す引数。</returns>
        protected override Formula OnDo(object userState, params Formula[] targets)
        {
            var arg = targets.Last() as Argument;
            if (arg == null) throw new ArgumentException("SolveSimultaneousEquationの最後の引数は、求解対象の変数リストを表す引数としてください。");

            var xs = new List<Variable>();
            foreach (var x in arg)
            {
                if (!(x is Variable)) throw new ArgumentException("SolveSimultaneousEquationの最後の引数は、求解対象の変数リストを表す引数としてください。");
                xs.Add(x as Variable);
            }

            var formulas = new List<Equal>();
            for (int i = 0; i < targets.Length - 1; i++)
            {
                formulas.Add(targets[i] as Equal);
                if (!(targets[i] is Equal)) throw new ArgumentException("SolveSimultaneousEquationの最後を除く引数は、求解対象の等式リストとしてください。");
            }

            var solvesList = Solve(formulas, xs.ToArray());
            if (solvesList.Count == 1) return ListToMaybeArgument(solvesList[0]);
            else return new Argument(solvesList.Select(solves => ListToMaybeArgument(solves)).ToArray());
        }

        /// <summary>
        /// 方程式リストと求解対象の変数リストを指定して、解を求めます。
        /// </summary>
        /// <param name="fs">方程式リスト。</param>
        /// <param name="xs">求解対象の変数リスト。</param>
        /// <returns>解("変数 = 解"からなる等式)リストの全組合せ。</returns>
        private List<List<Equal>> SolveRest(List<Equal> fs, List<Variable> xs)
        {
            if (xs.Count == 0) return new List<List<Equal>>();
            var x = xs[0];
            Equal fUsed;
            List<Equal> sols = SolveAnyAbout(x, fs, out fUsed);

            var nxs = new List<Variable>(xs);
            nxs.Remove(x);

            if (fUsed == null) return SolveRest(fs, nxs);

            List<List<Equal>> result = new List<List<Equal>>();
            foreach (var sol in sols)
            {
                // 残りの式に、ここで得られた解を代入
                List<Equal> nfs = new List<Equal>();
                foreach (var f in fs)
                {
                    if (f == fUsed) continue;
                    var ft = f.Substituted(sol.LeftHandSide, sol.RightHandSide);
                    nfs.Add(ft.Simplify() as Equal);
                }

                // 残りの式で、残りの変数について解く
                var rs = SolveRest(nfs, nxs);
                if (rs.Count == 0)
                {
                    var r = new List<Equal>();
                    r.Add(sol.Simplify() as Equal);
                    result.Add(r);
                }
                else
                {
                    // 残りの変数の解の各組合せに、ここで得られた解を登録
                    foreach (var r in rs) r.Insert(0, sol.Substituted(r.ToArray()).Simplify() as Equal);
                    result.AddRange(rs);
                }
            }
            return result;
        }

        /// <summary>
        /// 与えられた等式リストのいずれかを用いて、<see cref="x"/>の解を求めます。
        /// </summary>
        /// <param name="x">求解対象の変数。</param>
        /// <param name="fs">等式リスト。</param>
        /// <param name="fUsed">求解に用いられた等式。</param>
        /// <returns><see cref="x"/>について算出した解リスト。</returns>
        private List<Equal> SolveAnyAbout(Variable x, List<Equal> fs, out Equal fUsed)
        {
            var solver = new SolveAlgebraicEquation();
            solver.CheckSignificantDigits = CheckSignificantDigits;

            fUsed = null;
            var sols = new List<Equal>();
            foreach (var f in fs)
            {
                if (!f.Contains(x)) continue;

                var solution = solver.Solve(f, x);

                if (solution.RightHandSide is Argument)
                    sols.AddRange(solution.RightHandSide.Select(r => new Equal(x, r)));
                else
                    sols.Add(solution);

                fUsed = f;
                return sols;
            }
            return sols;
        }


        private Formula ListToMaybeArgument(List<Equal> solves)
        {
            if (solves.Count == 1) return solves[0];
            else return new Argument(solves.ToArray());
        }

    }
}
