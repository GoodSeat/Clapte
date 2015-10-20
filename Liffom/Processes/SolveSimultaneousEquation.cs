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
        /// <param name="formulas">求階対象の等式リスト。</param>
        /// <param name="targets">解を算出する可変数の変数。</param>
        /// <returns>解を表す等式リスト。</returns>
        public List<Equal> Solve(List<Equal> formulas, params Variable[] targets)
        {
            var fs = new List<Equal>(formulas.Select(f => f.Simplify() as Equal));
            var xs = new List<Equal>();

            for (int i = 0; i < targets.Length; i++)
            {
                var x = targets[i];

                Equal check = null;
                foreach (var f in fs)
                {
                    if (!f.Contains(x)) continue;

                    var solve = new SolveAlgebraicEquation();
                    xs.Add(solve.Solve(f, x));
                    check = f;
                    break;
                }
                if (check == null) continue;

                fs.Remove(check);
                
                for (int j = 0; j < fs.Count; j++)
                {
                    fs[j] = fs[j].Substitute(xs.Last().LeftHandSide, xs.Last().RightHandSide) as Equal;
                    fs[j] = fs[j].Simplify() as Equal;
                }
            }

            for (int i = 0; i < xs.Count; i++)
            {
                for (int j = i + 1; j < xs.Count; j++)
                    xs[i] = xs[i].Substitute(xs[j].LeftHandSide, xs[j].RightHandSide) as Equal;
                xs[i] = xs[i].Simplify() as Equal;
            }

            return xs;
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

            var solves = Solve(formulas, xs.ToArray());
            if (solves.Count == 1) return solves[0];
            else return new Argument(solves.ToArray());
        }

    }
}
