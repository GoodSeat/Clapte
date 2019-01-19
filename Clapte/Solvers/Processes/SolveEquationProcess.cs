using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Processes;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Clapte.Solvers;
using System.Threading;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Clapte.Solvers.Processes
{
    /// <summary>
    /// 方程式の求解処理を表します。
    /// </summary>
    public class SolveEquationProcess : Process
    {
        /// <summary>
        /// 常に求解対象とする変数名を取得します。
        /// </summary>
        public static string PermanentSolveTarget { get; set; }

        static SolveEquationProcess() { PermanentSolveTarget = "?"; }

        /// <summary>
        /// 方程式の求解処理を初期化します。
        /// </summary>
        /// <param name="owner">処理の保持者となるソルバ。</param>
        /// <param name="maxTime">計算を中止する時間[ms]。</param>
        /// <param name="solveEquations">適用を試みる可変数の求解処理。</param>
        public SolveEquationProcess(Solver owner, double maxTime, params SolveEquation[] solveEquations)
            : base(owner) 
        {
            SolveEquations = new List<SolveEquation>(solveEquations);
            MaxTime = maxTime;
        }

        DateTime _calcStartTime;

        /// <summary>
        /// 適用を試みる求解処理リストを取得します。
        /// </summary>
        public List<SolveEquation> SolveEquations { get; private set; }

        /// <summary>
        /// 計算を中止する時間[ms]を設定若しくは取得します。
        /// </summary>
        public double MaxTime { get; set; }

        /// <summary>
        /// 計算対象となった入力数式を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error EvaluateFormula(ref Formula input) 
        {
            _calcStartTime = DateTime.Now;
            Formula.FormulaProcessing += Formula_FormulaProcessing;
            try
            {
                if (!(input is Equal)) return null;

                var target = new Equal((input as Equal).LeftHandSide.Calculate(), (input as Equal).RightHandSide.Calculate());

                var variables = target.GetExistFactors<Variable>();
                if (!variables.Any()) return null;
                var x = variables.FirstOrDefault(v => v.Mark == PermanentSolveTarget);
                if (x == null && variables.Count() != 1) return null;
                if (x == null) x = variables.First();

                for (int i = 0; i < SolveEquations.Count; i++)
                {
                    SolveEquations[i].CancelAsync();
                    while (SolveEquations[i].IsBusy()) Thread.Sleep(0);
                }

                Formula result = null;
                for (int i = 0; i < SolveEquations.Count; i++)
                    SolveEquations[i].DoAsync(target, x);
                for (int i = 0; i < SolveEquations.Count; i++)
                {
                    result = SolveEquations[i].Wait();
                    if (result != null) break;
                }
                for (int i = 0; i < SolveEquations.Count; i++)
                {
                    if (SolveEquations[i].IsBusy()) SolveEquations[i].CancelAsync();
                }

                if (result != null)
                {
                    input = ModifyAnswerOfSingleUnit(result);
                    return null;
                }
                else
                {
                    return new Error(Error.Level.Error, "方程式の求解に失敗しました。");
                }
            }
            finally
            {
                Formula.FormulaProcessing -= Formula_FormulaProcessing;
            }
        }

        /// <summary>
        /// 方程式の解を表す数式に単独の単位が含まれる場合に、当該数式に係数1を付加します。
        /// </summary>
        /// <param name="result">対象とする数式。</param>
        /// <returns>単独の単位項に係数1を付与した数式。</returns>
        private Formula ModifyAnswerOfSingleUnit(Formula result)
        {
            if (result is Equal)
            {
                Equal equal = result as Equal;
                equal.RightHandSide = ModifyAnswerOfSingleUnit(equal.RightHandSide);
                return equal;
            }
            else if (result is Argument)
            {
                return new Argument(result.Select(f => ModifyAnswerOfSingleUnit(f)).ToArray());
            }
            else if (result is Unit)
            {
                return 1 * result;
            }
            else
            {
                return result;
            }
        }

        void Formula_FormulaProcessing(Formula sender, EventArgs e, ref bool Cancel)
        {
#if DEBUG
            return;
#endif
            if (DateTime.Now - _calcStartTime > TimeSpan.FromMilliseconds(MaxTime))
            {
                throw new Exception("一定時間以上計算が終了しませんでした。計算式が複雑すぎます。");
            }
        }

    }
}


