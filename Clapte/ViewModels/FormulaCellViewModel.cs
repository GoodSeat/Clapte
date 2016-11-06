using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GoodSeat.Clapte.Models;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Clapte.Solvers;
using System.Threading;

namespace GoodSeat.Clapte.ViewModels
{
    /// <summary>
    /// 数式セルのビューモデルを表します。
    /// </summary>
    public class FormulaCellViewModel : IDisposable
    {
        /// <summary>
        /// 数式セルのビューモデルを初期化します。
        /// </summary>
        /// <param name="text">初期化テキスト。</param>
        /// <param name="solver">計算に用いるソルバ。</param>
        /// <param name="previousCells">この数式セルより前方に定義されている可変数の数式セル。</param>
        public FormulaCellViewModel(string text, SolverViewModel solver, params FormulaCellViewModel[] previousCells)
        {
            List<FormulaCell> previous = new List<FormulaCell>();
            foreach (var cell in previousCells) previous.Add(cell.Target);

            Target = new FormulaCell(text, solver.Target, previous.ToArray());

            if (string.IsNullOrEmpty(text))
            {
                Indent = "";
            }
            else
            {
                int indentCount = text.Length - text.TrimStart().Length;
                Indent = text.Substring(0, indentCount).Replace("\r","").Replace("\n","");
            }
        }

        BackgroundWorker _backWorker;

        /// <summary>
        /// 対象とする数式セルを取得します。
        /// </summary>
        public FormulaCell Target { get; private set; }

        /// <summary>
        /// 初期化に用いた文字列を取得します。
        /// </summary>
        public string CacheText { get { return Target.CacheText; } }

        /// <summary>
        /// 数式の結果が評価されているか否かを取得します。
        /// </summary>
        public bool Evaluated { get { return Target.Evaluated; } }

        /// <summary>
        /// 数式セルの文字列に対して設定されたインデントを取得します。
        /// </summary>
        public string Indent { get; private set; }

        /// <summary>
        /// 関連付けるオブジェクトを設定もしくは取得します。
        /// </summary>
        public object Tag { get; set; }



        /// <summary>
        /// この数式セルを、依存関係を含めて一意に識別する文字列を生成して取得します。
        /// </summary>
        public string GetUniqueText() { return Target.GetUniqueText(); }

        /// <summary>
        /// 数式セルの評価を実行します。
        /// </summary>
        public void Evaluate(SolverViewModel solver)
        {
            Target.Evaluate(solver.Target);
        }

        /// <summary>
        /// 非同期に数式セルの評価を実行します。評価終了時、<see cref="ExitEvaluate"/>イベントが呼び出されます。
        /// </summary>
        public void EvaluateAsync(SolverViewModel solver)
        {
            if (_backWorker != null || Target.Evaluated) return;

            _backWorker = new BackgroundWorker();
            _backWorker.DoWork += new DoWorkEventHandler(WorkEvaluate);
            _backWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkEvaluateCompleted);

            _backWorker.RunWorkerAsync(solver);
        }

        /// <summary>
        /// 対象とする数式セルの評価を実行します。
        /// </summary>
        private void WorkEvaluate(object setnder, DoWorkEventArgs e)
        {
            var solver = e.Argument as SolverViewModel;

            while (!Target.CanEvaluate) Thread.Sleep(50);

            Target.Evaluate(solver.Target);
        }

        /// <summary>
        /// 対象とする数式セルの評価が終了した時の処理を実行します。
        /// </summary>
        private void WorkEvaluateCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_backWorker != null)
            {
                _backWorker.DoWork -= new DoWorkEventHandler(WorkEvaluate);
                _backWorker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(WorkEvaluateCompleted);
                _backWorker.Dispose();
                _backWorker = null;
            }

            ExitEvaluate?.Invoke(this, e);
        }

        /// <summary>
        /// 数式セルの評価終了時に呼び出されます。
        /// </summary>
        public event EventHandler ExitEvaluate;

        /// <summary>
        /// 数式セルを表す文字列を返します。
        /// </summary>
        public override string ToString()
        {
            return "ViewModel::" + Target.ToString();
        }

        public void Dispose()
        {
            if (_backWorker != null) _backWorker.Dispose();
        }
    }
}
