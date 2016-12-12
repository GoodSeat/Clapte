using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using GoodSeat.Sio.Xml;
using GoodSeat.Clapte.Solvers;
using System.Windows.Forms;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Clapte.ViewModels
{
    /// <summary>
    /// ユーザーによる入力テキストを数式セルに変換し、評価するViewModelを表します。
    /// </summary>
    public class FormulaCellListViewModel : IEnumerable<FormulaCellViewModel>
    {
        static int s_evaluateWorkerCount = 2;

        /// <summary>
        /// ユーザーによる入力テキストを数式セルに変換し、評価するViewModelを表します。
        /// </summary>
        /// <param name="solver">数式の評価に使用するソルバ。</param>
        /// <param name="constantList">評価で参照すべきユーザー定義定数リスト。</param>
        /// <param name="functionList">評価で参照すべきユーザー定義関数リスト。</param>
        public FormulaCellListViewModel(SolverViewModel solver, ConstantListViewModel constantList, FunctionListViewModel functionList)
        {
            UserConstantList = constantList.Target;
            UserFunctionList = functionList.Target;
            ConstantList = constantList;
            FunctionList = functionList;

            FormulaCellList = new List<FormulaCellViewModel>();

            FormulaCellCreateWorker = new BackgroundWorker();
            FormulaCellCreateWorker.WorkerSupportsCancellation = true;
            FormulaCellCreateWorker.DoWork += new DoWorkEventHandler(CreateFormulaCellListDoWork);
            FormulaCellCreateWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CreateFormulaCellListCreated);

            Solvers = new List<SolverViewModel>();
            FormulaCellEvaluateWorkers = new List<BackgroundWorker>();

            BaseSolver = solver;
            BaseSolver.SettingUpdated += new EventHandler(BaseSolver_SettingUpdated);

            Formula.FormulaProcessing += Formula_FormulaProcessing;
        }

        object _evaluateLockObject = new object();
        object _createLockObject = new object();

        bool _recreateFlag = false;
        string _targetText;
        SolverViewModel _baseSolver;
        int _abortFlag = 0;

        #region イベント

        /// <summary>
        /// 数式セルの評価に用いるソルバの更新が完了した時に呼び出されます。
        /// </summary>
        public event EventHandler SolversUpdated;

        /// <summary>
        /// いずれかの行の結果に変更があった時に呼び出されます。
        /// </summary>
        public event EventHandler ResultChanged;

        /// <summary>
        /// 数式の評価開始時に呼びされます。
        /// </summary>
        public event EventHandler EvaluateStarted;

        /// <summary>
        /// 数式の評価終了時に呼びされます。
        /// </summary>
        public event EventHandler EvaluateFinished;

        #endregion


        /// <summary>
        /// 数式セルの生成を行うバックグラウンドワーカーを設定もしくは取得します。
        /// </summary>
        BackgroundWorker FormulaCellCreateWorker { get; set; }

        /// <summary>
        /// 数式セルの評価を行うバックグラウンドワーカーリストを設定もしくは取得します。
        /// </summary>
        List<BackgroundWorker> FormulaCellEvaluateWorkers { get; set; }


        /// <summary>
        /// 数式セルの評価に用いるソルバを取得します。
        /// </summary>
        public SolverViewModel BaseSolver
        {
            get { return _baseSolver; }
            private set
            {
                _baseSolver = value;
                InitializeSolver(s_evaluateWorkerCount);
            }
        }

        /// <summary>
        /// 数式の評価処理中か否かを取得します。
        /// </summary>
        public bool IsEvaluating
        {
            get
            {
                foreach (var evaluateWorker in FormulaCellEvaluateWorkers)
                {
                    if (evaluateWorker.IsBusy) return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 数式セルの評価に用いるソルバリストを設定もしくは取得します。
        /// </summary>
        private List<SolverViewModel> Solvers { get; set; }

        /// <summary>
        /// 定数リスト管理オブジェクトを取得します。
        /// </summary>
        public ConstantListViewModel ConstantList { get; private set; }

        /// <summary>
        /// 関数リスト管理オブジェクトを取得します。
        /// </summary>
        public FunctionListViewModel FunctionList { get; private set; }

        /// <summary>
        /// ユーザー定義定数リストを設定もしくは取得します。
        /// </summary>
        public IList<ConstantDefine> UserConstantList { get; set; }

        /// <summary>
        /// ユーザー定義関数リストを設定もしくは取得します。
        /// </summary>
        public IList<FunctionDefine> UserFunctionList { get; set; }

        /// <summary>
        /// 現在有効な数式セルリストを設定もしくは取得します。
        /// </summary>
        private List<FormulaCellViewModel> FormulaCellList { get; set; }

        /// <summary>
        /// 全ての数式セルを再評価します。
        /// </summary>
        /// <param name="text">変化後の入力テキスト。</param>
        public void RenewAll(string text)
        {
            FormulaCellList.Clear();
            NotifyChangeText(text);
        }

        /// <summary>
        /// 数式セルの評価を強制中止します。
        /// </summary>
        public void AbortEvaluate()
        {
            foreach (var evaluateWorker in FormulaCellEvaluateWorkers)
            {
                if (!evaluateWorker.IsBusy) continue;

                if (!evaluateWorker.CancellationPending)
                {
                    _abortFlag++;
                    evaluateWorker.CancelAsync();
                }
            }

            foreach (var targetViewModel in FormulaCellList)
            {
                var target = targetViewModel.Target;

                if (target.Content.ResultText == null) target.Content.ResultText = "";
            }
            if (ResultChanged != null) ResultChanged(this, EventArgs.Empty);
            if (EvaluateFinished != null) EvaluateFinished(null, EventArgs.Empty);
        }


        /// <summary>
        /// 数式セルリストの再生成要請フラグを設定もしくは取得します。
        /// </summary>
        private bool RecreateFlag
        {
            get { lock (_createLockObject) return _recreateFlag; }
            set { lock (_createLockObject) _recreateFlag = value; }
        }

        /// <summary>
        /// 最後に通知された数式セルリスト生成用の元となる文字列を設定もしくは取得します。
        /// </summary>
        private string TargetText
        {
            get { lock (_createLockObject) return _targetText; }
            set { lock (_createLockObject) _targetText = value; }
        }


        /// <summary>
        /// 計算に使用するソルバとワーカーを初期化します。
        /// </summary>
        /// <param name="threadCount">計算に使用するスレッド数。</param>
        private void InitializeSolver(int threadCount)
        {
            foreach (var worker in FormulaCellEvaluateWorkers) 
            {
                worker.DoWork -= new DoWorkEventHandler(EvaluateFormulaCellDoWork);
                worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(EvaluateFormulaCellCompleted);
                worker.ProgressChanged -= new ProgressChangedEventHandler(EvaluateProgressChanged);
                worker.Dispose();
            }
            FormulaCellEvaluateWorkers.Clear();

            Solvers.Clear();
            for (int i = 0; i < threadCount; i++)
            {
                var solver = new SolverViewModel();
                var xmlElement = new XmlElement("setting");
                BaseSolver.OnSerialize(xmlElement);
                solver.OnDeserialize(xmlElement);

                solver.Target.AbortLevel = Error.Level.Error;
                solver.Target.UserConstants = UserConstantList;
                solver.Target.UserFunctions = UserFunctionList;
                Solvers.Add(solver);

                var worker = new BackgroundWorker();
                worker.WorkerSupportsCancellation = true;
                worker.WorkerReportsProgress = true;
                worker.DoWork += new DoWorkEventHandler(EvaluateFormulaCellDoWork);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(EvaluateFormulaCellCompleted);
                worker.ProgressChanged += new ProgressChangedEventHandler(EvaluateProgressChanged);
                FormulaCellEvaluateWorkers.Add(worker);
            }

            SolversUpdated?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// ユーザーの入力テキストに変化のあったことを通知します。
        /// </summary>
        /// <param name="text">変化後の入力テキスト。</param>
        public void NotifyChangeText(string text)
        {
            TargetText = text;

            if (FormulaCellCreateWorker.IsBusy)
                RecreateFlag = true;
            else
                FormulaCellCreateWorker.RunWorkerAsync();
        }


        /// <summary>
        /// 数式セルリストの生成処理を実行します。
        /// </summary>
        private void CreateFormulaCellListDoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            do
            {
                RecreateFlag = false;
                string text = TargetText;

                var list = CreateFormulaCellList(text, worker);
                CombineNewFormulaCellList(list, worker);

                foreach (var evaluateWorker in FormulaCellEvaluateWorkers)
                {
                    if (worker.CancellationPending) break;
                    if (RecreateFlag) break;
                    while (evaluateWorker.IsBusy)
                    {
                        if (!evaluateWorker.CancellationPending) evaluateWorker.CancelAsync();
                        Application.DoEvents();
                        Thread.Sleep(0);
                    }
                }
                Application.DoEvents();
            }
            while (RecreateFlag);

            e.Cancel = worker.CancellationPending;
        }
        
        /// <summary>
        /// 数式セルリストの生成処理終了時の処理を実行します。
        /// </summary>
        private void CreateFormulaCellListCreated(object sender, RunWorkerCompletedEventArgs e)
        {
            _abortFlag = 0;
            if (e.Cancelled) return;
            if (ResultChanged != null) ResultChanged(this, e);

            int count = 0;
            foreach (var worker in FormulaCellEvaluateWorkers)
            {
                worker.RunWorkerAsync(count++);
            }
            if (EvaluateStarted != null) EvaluateStarted(this, e);
        }

        /// <summary>
        /// 指定した文字列から、数式セルリストを生成して取得します。
        /// </summary>
        /// <param name="text">数式セルリストの生成元文字列。</param>
        private List<FormulaCellViewModel> CreateFormulaCellList(string text, BackgroundWorker worker)
        {
            // 行順、解決順インデックス、文字列 のタプルのリスト
            List<Tuple<int, int[], string>> lines = new List<Tuple<int, int[], string>>();

            // 行順、解決順インデックスを振る
            int lineIndex = 0; // 行順
            List<int> sectionNum = new List<int>(); // 解決順インデックス
            Tuple<int, string> continueInfo = null; // 継続行の情報(解決順インデント、先頭の"|"を含む行文字列)
            foreach (string lineText in text.Replace("\r", "").Split('\n'))
            {
                int currentIndent = GetWhereIndentAndSolveText(lineText).Item1;
                string currentText = lineText;

                if (lineText.EndsWith(" _")) // 継続行の処理
                {
                    if (continueInfo == null || continueInfo.Item1 == currentIndent)
                    {
                        string preText = (continueInfo != null) ? continueInfo.Item2 + "\n" : "";
                        continueInfo = Tuple.Create(currentIndent, preText + lineText);
                        continue;
                    }
                    else
                    {
                        var newInf = Tuple.Create(currentIndent, lineText);
                        currentIndent = continueInfo.Item1;
                        currentText = continueInfo.Item2;
                        continueInfo = newInf;
                    }
                }
                else if (continueInfo != null) // 継続行の結合
                {
                    currentText = continueInfo.Item2 + "\n" + currentText;
                    continueInfo = null;
                }

                while (sectionNum.Count > currentIndent + 1) sectionNum.RemoveAt(sectionNum.Count - 1);

                if (sectionNum.Count - 1 < currentIndent) sectionNum.Add(0);
                else
                {
                    int chapter = sectionNum.Last();
                    sectionNum.RemoveAt(sectionNum.Count - 1);
                    sectionNum.Add(++chapter);
                }

                lines.Add(Tuple.Create(lineIndex++, sectionNum.ToArray(), currentText));
            }

            lines.Sort((item1, item2) => CompareSection(item1?.Item2, item2?.Item2)); // 解決順でソート

            // 解決順で数式セルを評価
            var result = new List<Tuple<int, FormulaCellViewModel>>();
            foreach (var item in lines)
            {
                if (worker.CancellationPending) break;
                if (RecreateFlag) break;

                foreach (string textLine in item.Item3.Split('\n')) // 継続行の処理
                {
                    var textTarget = GetWhereIndentAndSolveText(textLine).Item2;
                    result.Add(Tuple.Create(item.Item1, new FormulaCellViewModel(textTarget, BaseSolver, result.Select(r => r.Item2).ToArray())));
                }
            }

            // 行順に戻して返す
            return result.OrderBy(r => r.Item1).Select(r => r.Item2).ToList();
        }

        /// <summary>
        /// 解決順を表す数列を比較した結果を返します。
        /// </summary>
        /// <param name="sectionNum1">比較対象の数列1。</param>
        /// <param name="sectionNum2">比較対象の数列2。/param>
        /// <returns>比較結果。</returns>
        private int CompareSection(IEnumerable<int> sectionNum1, IEnumerable<int> sectionNum2)
        {
            var section1 = sectionNum1;
            var section2 = sectionNum2;
            while (true)
            {
                if (section1 == null && section2 == null) return 0;
                if (section1.Count() == 0 && section2.Count() == 0) return 0;
                if (section2 == null || section2.Count() == 0) return -1;
                if (section1 == null || section1.Count() == 0) return 1;
                if (section1.First() < section2.First()) return -1;
                if (section1.First() > section2.First()) return 1;

                section1 = section1.Skip(1);
                section2 = section2.Skip(1);
            }
        }

        /// <summary>
        /// 指定文字列の評価順インデントと、評価対象とする文字列を取得します。
        /// </summary>
        /// <param name="text">判定対象の文字列。</param>
        /// <returns>指定文字列の評価順インデントと、評価対象とする文字列からなるタプル。</returns>
        /// <example>
        /// "|  |  x = 4 " -> (2, "      x = 4 ")
        /// </example>
        private Tuple<int, string> GetWhereIndentAndSolveText(string text)
        {
            int trimCount = text.Length - text.Trim().TrimStart(' ', '|').Length;

            string trimText = text.Substring(0, trimCount);
            int indentCount = trimText.Count(c => c == '|');

            return Tuple.Create(indentCount, trimText.Replace('|', ' ') + text.Substring(trimCount));
        }

        /// <summary>
        /// 生成された新しい数式セルリストをもとに、現状の数式セルリストを置き換えます。
        /// </summary>
        /// <param name="newFormulaCells">新しい数式セルリスト。</param>
        private void CombineNewFormulaCellList(List<FormulaCellViewModel> newFormulaCells, BackgroundWorker worker)
        {
            foreach (var newFormulaCell in newFormulaCells)
            {
                var newText = newFormulaCell.GetUniqueText();
                foreach (var oldFormulaCell in FormulaCellList)
                {
                    if (worker.CancellationPending) return;
                    if (RecreateFlag) return;

                    if (newText != oldFormulaCell.GetUniqueText()) continue;

                    newFormulaCell.Target.Content = oldFormulaCell.Target.Content;
                    break;
                }
            }
            FormulaCellList = newFormulaCells;
#if DEBUG
            Console.WriteLine("CombineNewFormulaCellList");
#endif
        }

        /// <summary>
        /// 各数式セルを順次評価する処理を実行します。
        /// </summary>
        private void EvaluateFormulaCellDoWork(object sender, DoWorkEventArgs e)
        {
            int id = (int)e.Argument; // スレッドに割り当てられた番号
            var solver = Solvers[id]; // このスレッドで使用するソルバ

            var thisWorker = sender as BackgroundWorker;
            bool evaluated = true;
            while (evaluated && !thisWorker.CancellationPending)
            {
                evaluated = false;
                foreach (var cell in FormulaCellList)
                {
                    if (thisWorker.CancellationPending) break;

                    lock (_evaluateLockObject)
                    {
                        if (cell.Evaluated) continue; // すでに評価済み
                        if (cell.Tag != null || !cell.Target.CanEvaluate) // 他スレッドで評価中 or 評価に必要な他セルの評価が未実施
                        {
                            evaluated = true; 
                            continue;
                        }
                        cell.Tag = thisWorker; // このスレッドで評価中であることを明示
                    }
#if DEBUG
                    Console.WriteLine("EvaluateFormulaCellDoWork::" + id.ToString() + "::" + cell.CacheText);
#endif
                    cell.Evaluate(solver);
//                    Thread.Sleep(1000); // 計算に時間がかかる場合を想定
#if DEBUG
                    Console.WriteLine("EvaluateFormulaCellDoWork::" + id.ToString() + ":: →" + cell.Target.Content.ResultText);
#endif
                    if (thisWorker.CancellationPending) break;

                    thisWorker.ReportProgress(1);
                    cell.Tag = null;
                    evaluated = true;
                }
            }
        }

        /// <summary>
        /// いずれかの数式セルの評価が終了した時の処理を実行します。
        /// </summary>
        private void EvaluateProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (ResultChanged != null) ResultChanged(this, e);
        }

        /// <summary>
        /// 全ての数式セルの評価が終了した時の処理を実行します。
        /// </summary>
        private void EvaluateFormulaCellCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (EvaluateFinished != null) EvaluateFinished(sender, e);
        }


        /// <summary>
        /// 指定行の結果を取得します。
        /// </summary>
        /// <param name="row">取得対象の行番号。</param>
        /// <returns>
        /// 指定行に対応する数式セルの評価結果。
        /// 複数行にわたる数式セルでは、対応するResultオブジェクトは1行目に対してのみ返され、その他の行ではnullが返されます。
        /// </returns>
        public string GetResultOf(int row)
        {
            if (row >= FormulaCellList.Count) return null;

            var targetViewModel = FormulaCellList[row];
            var target = targetViewModel.Target;

            var result = target.Content.ResultText;
            if (result == null)
            {
                if (targetViewModel.Tag == null) result = "~~~ 計算待機中...";
                else result = "~~~ 計算実行中...";
            }
            if (target.CommentText != null) 
            {
                if (!string.IsNullOrEmpty(result)) result += " ";
                result += target.CommentText;
            }
            result = targetViewModel.Indent + result;

            return result;
        }

        /// <summary>
        /// 指定行番号のFormulaCellViewModelオブジェクトを取得します。
        /// </summary>
        public FormulaCellViewModel this[int index]
        {
            get 
            {
                if (FormulaCellList.Count <= index) return null;

                return FormulaCellList[index];
            }
        }

        /// <summary>
        /// ソルバの設定更新時に呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BaseSolver_SettingUpdated(object sender, EventArgs e)
        {
            InitializeSolver(s_evaluateWorkerCount);
        }

        /// <summary>
        /// 数式評価中に定期的に呼び出されます。
        /// </summary>
        void Formula_FormulaProcessing(Formula sender, EventArgs e, ref bool Cancel)
        {
            if (_abortFlag > 0)
            {
                Cancel = true;
                _abortFlag--;
            }
        }
        
        #region IEnumerable<FormulaCellViewModel> メンバー

        public IEnumerator<FormulaCellViewModel> GetEnumerator()
        {
            return FormulaCellList.GetEnumerator();
        }

        #endregion

        #region IEnumerable メンバー

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return FormulaCellList.GetEnumerator();
        }

        #endregion
    }
}
