using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sgry.Azuki;
using GoodSeat.Clapte.Models;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.Views.InputSupports;
using GoodSeat.Clapte.ViewModels;
using GoodSeat.Sio.Xml.Serialization;
using GoodSeat.Sio.Xml;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Units;
using System.Text.RegularExpressions;

namespace GoodSeat.Clapte.Views.Forms
{
    /// <summary>
    /// ClaptePadフォームを表します。
    /// </summary>
    public partial class FormOfClaptePad : ClapteFormBase, ISerializable
    {
        private const string _hotSaveFilename = "ClaptePadHotText.txth";
        private const string _claptePadHelpFilename = "ClaptePadHelp.txt";

        /// <summary>
        /// ClaptePadフォームを初期化します。(デザイナ用)
        /// </summary>
        private FormOfClaptePad()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ClaptePadフォームを初期化します。
        /// </summary>
        /// <param name="target">表示対象のFormulaCellListViewModelオブジェクト。</param>
        public FormOfClaptePad(FormOfMain mainForm)
        {
            InitializeComponent();

            ShowOKButton = false;
            ShowCancelButton = false;

            OwnerMainForm = mainForm;
            Target = new FormulaCellListViewModel(mainForm.ClapteCore.Solver, mainForm.UserConstants, mainForm.UserFunctions);
            Target.ResultChanged += new EventHandler(Target_ResultChanged);
            Target.SolversUpdated += new EventHandler(Target_SolversUpdated);
            Target.EvaluateStarted += Target_EvaluateStarted;
            Target.EvaluateFinished += Target_EvaluateFinished;

            InputSupportEnumerator = new ClaptePadInputSupportEnumerator(Target);
            Support = new InputSupport(_inputTextBox, this, InputSupportEnumerator);
            Support.ModifyLocation = _splitContainer.Location;
            ArgumentHelper = new FunctionArgumentHelp(_inputTextBox, this, InputSupportEnumerator);
            ArgumentHelper.ModifyLocation = _splitContainer.Location;

            AdditionalInformations = new List<Tuple<FormulaCellContent.AdditionalInformationType, string>>();

            InitializeTextBox();

            Delay = 500;

            HotLoad();
        }

        #region プロパティ

        /// <summary>
        /// カラースキーマを設定もしくは取得します。
        /// </summary>
        public ColorScheme ColorScheme
        {
            get { return _inputTextBox.ColorScheme; }
            set
            {
                _inputTextBox.ColorScheme = value;
                _resultTextBox.ColorScheme = value;
            }
        }


        /// <summary>
        /// フォント情報を設定もしくは取得します。
        /// </summary>
        public FontInfo FontInfo
        {
            get { return _inputTextBox.FontInfo; }
            set
            {
                _inputTextBox.FontInfo = value;
                _resultTextBox.FontInfo = value;
            }
        }

        /// <summary>
        /// 行番号を表示するか否かを設定もしくは取得します。
        /// </summary>
        public bool ShowLineNumber
        {
            get { return _inputTextBox.ShowsLineNumber; }
            set { _inputTextBox.ShowsLineNumber = value; }
        }

        /// <summary>
        /// タブ文字を表示するか否かを設定もしくは取得します。
        /// </summary>
        public bool ShowTabChara
        {
            get { return _inputTextBox.DrawsTab; }
            set
            {
                _inputTextBox.DrawsTab = value;
                _resultTextBox.DrawsTab = value;
            }
        }

        /// <summary>
        /// 改行文字を表示するか否かを設定もしくは取得します。
        /// </summary>
        public bool ShowEolChara
        {
            get { return _inputTextBox.DrawsEolCode; }
            set
            {
                _inputTextBox.DrawsEolCode = value;
                _resultTextBox.DrawsEolCode = value;
            }
        }

        /// <summary>
        /// キャレット位置に下線を表示するか否かを設定もしくは取得します。
        /// </summary>
        public bool ShowUnderLine
        {
            get { return _inputTextBox.HighlightsCurrentLine; }
            set
            {
                _inputTextBox.HighlightsCurrentLine = value;
                _resultTextBox.HighlightsCurrentLine = value;
            }
        }

        /// <summary>
        /// 入力から計算までに遅延時間(ms)を設定もしくは取得します。
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// 入力の際、自動で入力補助を表示するか否かを設定もしくは取得します。
        /// </summary>
        public bool AutoShowInputSupport
        {
            get { return Support.AutoShow; }
            set { Support.AutoShow = value; }
        }

        /// <summary>
        /// キャレット移動時、必要に応じて自動で関数の引数ヘルプを表示するか否かを設定もしくは取得します。
        /// </summary>
        public bool AutoShowArgumentHelp
        {
            get { return ArgumentHelper.AutoShow; }
            set { ArgumentHelper.AutoShow = value; }
        }

        /// <summary>
        /// 説明文も補完対象として使用するか否かを設定もしくは取得します。
        /// </summary>
        public bool InputSupportWithAlsoInfomation
        {
            get { return InputSupportEnumerator.AlsoInfomation; }
            set { InputSupportEnumerator.AlsoInfomation = value; }
        }


        /// <summary>
        /// 親となるClapteの常駐メインフォームを取得します。
        /// </summary>
        public FormOfMain OwnerMainForm { get; private set; }

        /// <summary>
        /// 対象の数式セルリストビューモデルを取得します。
        /// </summary>
        public FormulaCellListViewModel Target { get; private set; }

        /// <summary>
        /// シンタックスハイライトオブジェクトを設定もしくは取得します。
        /// </summary>
        public ClaptePadKeywordHighlighter Highlighter { get; set; }


        /// <summary>
        /// 入力と結果のテキストボックスのスクロール同期処理の無効フラグです。
        /// </summary>
        private bool IgnoreScroll { get; set; }

        /// <summary>
        /// 入力補助オブジェクトを設定もしくは取得します。
        /// </summary>
        private InputSupport Support { get; set; }

        /// <summary>.
        /// 引数ヘルプオブジェクトを設定もしくは取得します。
        /// </summary>.
        private FunctionArgumentHelp ArgumentHelper { get; set; }

        /// <summary>
        /// 入力補助の候補列挙オブジェクトを設定もしくは取得します。
        /// </summary>
        private ClaptePadInputSupportEnumerator InputSupportEnumerator { get; set; }

        /// <summary>
        /// 現在の入力ボックス各行に関連付けられた付加情報リストを設定もしくは取得します。
        /// </summary>
        private List<Tuple<FormulaCellContent.AdditionalInformationType, string>> AdditionalInformations { get; set; }

        #endregion

        #region 処理

        /// <summary>
        /// 入力テキストボックス及び結果テキストボックスの設定を初期化します。
        /// </summary>
        void InitializeTextBox()
        {
            Highlighter = new ClaptePadKeywordHighlighter(Target);
            _inputTextBox.Highlighter = Highlighter;
            _resultTextBox.Highlighter = Highlighter;

            _inputTextBox.View.ColorScheme.SelectionBack = Color.Gray;
            _inputTextBox.View.ColorScheme.LineNumberBack = Color.White;
            _inputTextBox.View.ColorScheme.LineNumberFore = Color.DarkGray;
            _inputTextBox.View.ColorScheme.MatchedBracketBack = Color.PowderBlue;
            _inputTextBox.View.ColorScheme.HighlightColor = Color.Lavender;
            _inputTextBox.ShowsHScrollBar = false;
            _inputTextBox.MouseWheel += _inputTextBox_MouseMove;

            _inputTextBox.SetKeyBind(Keys.Control | Keys.Enter, i => Support.ShowInputSupport(true));
            _inputTextBox.SetKeyBind(Keys.Control | Keys.H, i => ArgumentHelper.ShowArgumentHelp());

            _resultTextBox.View.ColorScheme.SelectionBack = Color.Gray;
            _resultTextBox.View.ColorScheme.LineNumberBack = Color.White;
            _resultTextBox.View.ColorScheme.LineNumberFore = Color.DarkGray;
            _resultTextBox.View.ColorScheme.MatchedBracketBack = Color.PowderBlue;
            _resultTextBox.View.ColorScheme.HighlightColor = Color.Lavender;
            _resultTextBox.ShowsHScrollBar = false;

            int parseErrorID = (int)FormulaCellContent.AdditionalInformationType.ParseError;
            Marking.Register(new MarkingInfo(parseErrorID, "構文解析エラー"));
            _inputTextBox.ColorScheme.SetMarkingDecoration(parseErrorID, new UnderlineTextDecoration(LineStyle.Waved, Color.Red));
        }

        /// <summary>
        /// 前回終了時の入力テキストを復元します。
        /// </summary>
        void HotLoad()
        {
            if (File.Exists(_hotSaveFilename)) _inputTextBox.Text = File.ReadAllText(_hotSaveFilename);
            else                               _menuInsertHelp_Click(this, EventArgs.Empty);
        }

        /// <summary>
        /// 現在のテキストを、次回起動時復元用の外部ファイルに保存します。
        /// </summary>
        void HotSave()
        {
            File.WriteAllText(_hotSaveFilename, _inputTextBox.Text, Encoding.UTF8);
        }

        /// <summary>
        /// ツールチップヘルプ表示を終了します。
        /// </summary>
        private void HideTooltipHelp()
        {
            _toolTipHelp.Hide(_inputTextBox);
            _toolTipHelp.Tag = null;
        }

        /// <summary>
        /// 入力されているテキストに応じて、スクロールバーの表示有無を切り替えます。
        /// </summary>
        private void SetVisibleOfScrollBar()
        {
            var view = _inputTextBox.View;
            int visibleLineCount = view.VisibleTextAreaSize.Height / view.LineSpacing;

            _resultTextBox.ShowsVScrollBar = (_inputTextBox.LineCount > visibleLineCount);
            _resultTextBox.UpdateScrollBarRange();
        }

        /// <summary>
        /// 現在のキャレット位置、もしくは選択範囲に基づいて、その定義元にジャンプします。
        /// </summary>
        /// <param name="azuki">対象のAzukiコントロール。</param>
        private void JumpDefine(Sgry.Azuki.WinForms.AzukiControl azuki)
        {
            string targetText = azuki.GetSelectedText();
            string postText;
            int lineIndex;
            string txt = azuki.GetCaretWord(out lineIndex, out postText);
            if (string.IsNullOrEmpty(targetText)) targetText = txt;
            if (targetText == null) return;

            var helpTarget = InputSupportEnumerator.GetInputSupportCandidateFromText(targetText, lineIndex, postText);
            if (helpTarget == null)
            {
                var target = Target.BaseSolver.Target.Parser.Parse(targetText);
                if (target is Variable) target = new Unit(targetText);

                var unit = target as Unit;
                if (unit != null && unit.UnitType != null) OwnerMainForm.OpenDefine(target);

                return;
            }

            int jump = -1;
            if (helpTarget.Tag is FunctionDefine)
            {
                var def = helpTarget.Tag as FunctionDefine;

                int line = 0;
                foreach (var cell in Target)
                {
                    if (cell.Target.Content.GetAllDefinedFunctionNames().Select(u => u.Item1).Contains(def.Name)) jump = line;
                    if (line++ >= lineIndex) break;
                }
                if (jump == -1) OwnerMainForm.OpenDefine(def.Target);
            }
            else if (helpTarget.Tag is ConstantDefine)
            {
                var def = helpTarget.Tag as ConstantDefine;

                int line = 0;
                foreach (var cell in Target)
                {
                    if (cell.Target.Content.GetAllDefinedVariableNames().Contains(def.Name)) jump = line;
                    if (line++ >= lineIndex) break;
                }
                if (jump == -1) OwnerMainForm.OpenDefine(def.Target);
            }
            else if (helpTarget.Tag is UnitConvertRecord)
            {
                OwnerMainForm.OpenDefine((helpTarget.Tag as UnitConvertRecord).ConvertUnit);
            }
            if (jump < 0) return;

            azuki.Document.SetCaretIndex(jump, 0);
            azuki.ScrollToCaret();
        }

        /// <summary>
        /// 対象のシンタックスカラーを取得します。
        /// </summary>
        /// <param name="target">対象タイプ。</param>
        /// <returns>シンタックスカラー。</returns>
        public Color GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget target)
        {
            Color fore, back;
            ColorScheme.GetColor(Highlighter.GetCharClassOf(target), out fore, out back);
            return fore;
        }

        /// <summary>
        /// 対象のシンタックスカラーを設定します。
        /// </summary>
        /// <param name="target">対象タイプ。</param>
        /// <param name="color">設定する色。</param>
        public void SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget target, Color color)
        {
            ColorScheme.SetColor(Highlighter.GetCharClassOf(target), color, Color.White);
        }

        /// <summary>
        /// 選択されているすべての行についてテキストの変換を行います。
        /// </summary>
        /// <param name="convert">元の文字列と、その行が選択行のうちの最終行か否かを受け取り、文字列を変換する処理。</param>
        private void EditSelectedLines(Func<string, bool, string> convert)
        {
            int caretIndex = _inputTextBox.CaretIndex;

            int begin, end;
            _inputTextBox.GetSelection(out begin, out end);

            int beginLine = _inputTextBox.GetLineHeadIndexFromCharIndex(begin);
            int endLine = _inputTextBox.GetLineHeadIndexFromCharIndex(end);

            int sline = -1, eline = -1;
            int line = 0;
            while (sline == -1 || eline == -1)
            {
                int lineHeadIndex = _inputTextBox.GetLineHeadIndex(line);
                if (lineHeadIndex == beginLine) sline = line;
                if (lineHeadIndex == endLine) eline = line;
                ++line;
            }

            var texts = new List<string>(_inputTextBox.Text.Split('\n').Select(s => s.Replace("\r", "")));
            for (int l = sline; l < eline; ++l) texts[l] = convert(texts[l], false);
            texts[eline] = convert(texts[eline], true);

            int visible1stLine = _inputTextBox.FirstVisibleLine;
            _inputTextBox.Text = string.Join("\r\n", texts);
            _inputTextBox.FirstVisibleLine = visible1stLine;

            _inputTextBox.SetSelection(caretIndex, caretIndex);
        }

        protected override void OnCancel(EventArgs e)
        {
            if (FormOfMain.IsCalculatorMode)
            {
                OwnerMainForm.Close();
            }
            else
            {
                this.Hide();
            }
        }

        /// <summary>
        /// 数式の変形関数を受け取り、変形結果をキャレットの次の行に挿入します。
        /// </summary>
        /// <param name="deform">適用する数式の変形処理。</param>
        private void DeformFormula(Func<Formula, Formula> deform)
        {
            int caretIndex = _inputTextBox.CaretIndex;
            int begin, end;
            _inputTextBox.GetSelection(out begin, out end);
            int lineIndex = _inputTextBox.Document.GetLineIndexFromCharIndex(begin);

            string result = "# 数式処理に失敗しました。";
            try
            {
                // MEMO:現状、単位の自動認識はされない([]で囲ったやつだけが単位として認識される)
                var f1 = FormulaOnCaret();
                var f2 = deform(f1);
                f2.Format = Target.BaseSolver.Target.OutputFormat;
                result = f2.ToString();
            }
            catch (Exception e)
            {
                result += e.Message;
            }

            var texts = new List<string>(_inputTextBox.Text.Split('\n').Select(s => s.Replace("\r", "")));
            texts.Insert(lineIndex + 1, result);

            int visible1stLine = _inputTextBox.FirstVisibleLine;
            _inputTextBox.Text = string.Join("\r\n", texts);
            _inputTextBox.FirstVisibleLine = visible1stLine;

            _inputTextBox.SetSelection(caretIndex, caretIndex);
        }

        /// <summary>
        /// 現在のキャレット上にある数式を取得します。
        /// </summary>
        /// <returns></returns>
        Formula FormulaOnCaret()
        {
            int begin, end;
            _inputTextBox.GetSelection(out begin, out end);

            int lineIndex = _inputTextBox.Document.GetLineIndexFromCharIndex(begin);
            var line = Target.ElementAt(lineIndex);

            // MEMO:現状、単位の自動認識はされない([]で囲ったやつだけが単位として認識される)
            return Target.BaseSolver.Target.Parse(line.Target.Content.FormulaText);
        }

        /// <summary>
        /// 現在のキャレット上の行で指定されている換算目標単位を取得します。換算目標単位の指定がない場合には、nullを返します。
        /// </summary>
        /// <returns></returns>
        string DetectTargetUnitOnCaretLine()
        {
            int begin, end;
            _inputTextBox.GetSelection(out begin, out end);

            int lineIndex = _inputTextBox.Document.GetLineIndexFromCharIndex(begin);

            string text = _inputTextBox.Document.GetLineContent(lineIndex);

            var targetUnitRegex = new Regex(@"^\s*\[(?<targetUnit>[^[\]]+)\]");

            var match = targetUnitRegex.Match(text);
            return match.Success ? match.Groups["targetUnit"].Value : null;
        }

        /// <summary>
        /// 現在のキャレット上の換算目標単位を指定します。nullや空文字が指定された場合、目標単位を削除します。
        /// </summary>
        /// <param name="targetUnit"></param>
        void SetTargetUnitOnCaretLine(string targetUnit)
        {
            int begin, end;
            _inputTextBox.GetSelection(out begin, out end);

            int lineIndex = _inputTextBox.Document.GetLineIndexFromCharIndex(begin);

            string text = _inputTextBox.Document.GetLineContent(lineIndex);

            var targetUnitRegex = new Regex(@"^(?<indent>\s*)\[(?<targetUnit>[^[\]]*)\]");
            var match = targetUnitRegex.Match(text);
            var newText = text;
            if (match.Success)
            {
                if (string.IsNullOrWhiteSpace(targetUnit))
                {
                    newText = Regex.Replace(text, targetUnitRegex.ToString(), "${indent}");
                }
                else
                {
                    newText = Regex.Replace(text, targetUnitRegex.ToString(), "${indent}" + "[" + targetUnit + "]");
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(targetUnit)) newText = "[" + targetUnit + "] " + text;
            }
            if (newText == text) return;

            var texts = new List<string>(_inputTextBox.Text.Split('\n').Select(s => s.Replace("\r", "")));
            texts[lineIndex] = newText;

            int visible1stLine = _inputTextBox.FirstVisibleLine;
            _inputTextBox.Text = string.Join("\r\n", texts);
            _inputTextBox.FirstVisibleLine = visible1stLine;

            int caretIndex = _inputTextBox.Document.GetCharIndexFromLineColumnIndex(lineIndex, 0);
            _inputTextBox.SetSelection(caretIndex, caretIndex);
        }

        #endregion

        #region イベント対応

        /// <summary>
        /// 評価結果の変更時に呼び出されます。
        /// </summary>
        private void Target_ResultChanged(object sender, EventArgs e)
        {
            var document = _inputTextBox.Document;
            foreach (var type in Enum.GetValues(typeof(FormulaCellContent.AdditionalInformationType)))
            {
                document.Unmark(0, document.Length, (int)type);
            }
            AdditionalInformations.Clear();

            string resultText = "";
            for (int i = 0; i < document.LineCount; i++)
            {
                var result = Target.GetResultOf(i);
                resultText += result + "\r\n";

                var addInfo = Target.GetAdditionalInfomationOf(i);
                if (addInfo != null)
                {
                    string lineText = document.GetLineContent(i);
                    int head = document.GetLineHeadIndex(i);
                    int indent = lineText.Length - lineText.TrimStart().Length;
                    int len = lineText.Split('#')[0].TrimEnd().Length;
                    document.Mark(head + indent, head + len, (int)addInfo.Item1);
                }
                AdditionalInformations.Add(addInfo);
            }
            SetVisibleOfScrollBar();

            IgnoreScroll = true;
            _resultTextBox.Text = resultText;
            _resultTextBox.View.ScrollPos = _inputTextBox.View.ScrollPos;
            _resultTextBox.UpdateScrollBarRange();
            IgnoreScroll = false;

            Highlighter.Renew(_inputTextBox.Text);
            _inputTextBox.Refresh(); // Markの表示のため
        }

        /// <summary>
        /// 数式セルの評価ソルバの設定変更が完了した時に呼び出されます。
        /// </summary>
        private void Target_SolversUpdated(object sender, EventArgs e)
        {
            Target.RenewAll(_inputTextBox.Text);
            AzukiExtension.SplitWithFollowNumber = Target.BaseSolver.PermitOmitPowerMark;
        }

        /// <summary>
        /// 数式セルの評価開始時に呼び出されます。
        /// </summary>
        private void Target_EvaluateStarted(object sender, EventArgs e) { _picStatus.Visible = Target.IsEvaluating; }

        /// <summary>
        /// 数式セルの評価終了時に呼び出されます。
        /// </summary>
        private void Target_EvaluateFinished(object sender, EventArgs e) { _picStatus.Visible = Target.IsEvaluating; }

        /// <summary>
        /// 入力ボックスのテキストに変更があったときに呼び出されます。
        /// </summary>
        private void _inputTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Delay == 0)
            {
                Target.NotifyChangeText(_inputTextBox.Text);
            }
            else
            {
                _timerDelay.Enabled = false;
                _timerDelay.Interval = Delay;
                _timerDelay.Enabled = true;
            }
        }

        private void _inputTextBox_FontChanged(object sender, EventArgs e)
        {
            if (sender == _inputTextBox) _resultTextBox.Font = _inputTextBox.Font;
            else _inputTextBox.Font = _resultTextBox.Font;
        }

        private void _inputTextBox_VScroll(object sender, EventArgs e)
        {
            _resultTextBox.View.ScrollPos = _inputTextBox.View.ScrollPos;
            _resultTextBox.UpdateScrollBarRange();

            ArgumentHelper.Hide();
            Support.EscapeInputSupport();
        }

        private void _resultTextBox_VScroll(object sender, EventArgs e)
        {
            if (IgnoreScroll) return;
            _inputTextBox.View.ScrollPos = _resultTextBox.View.ScrollPos;
            _inputTextBox.UpdateCaretGraphic();
        }

        private void FormOfClaptePad_FormClosing(object sender, FormClosingEventArgs e) { HotSave(); }

        private void FormOfClaptePad_Resize(object sender, EventArgs e) { SetVisibleOfScrollBar(); }

        private void _timerDelay_Tick(object sender, EventArgs e)
        {
            _timerDelay.Enabled = false;
            Target.NotifyChangeText(_inputTextBox.Text);
        }

        private void _inputTextBox_CaretMoved(object sender, EventArgs e)
        {
            int line, column;
            _inputTextBox.Document.GetCaretIndex(out line, out column);
            InputSupportEnumerator.CurrentCaretLineNumber = line;
        }

        private void _inputTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            var textBox = sender as Sgry.Azuki.WinForms.AzukiControl;
            if (textBox == null) return;

            int lineIndex;
            string postText;
            string targetText = textBox.GetMouseHoverWord(out lineIndex, out postText);
            char? targetChar = textBox.GetMouseHoverChar();
            int? markID = textBox.GetMouseHoverMarkID();

            string helpText = null;

            int mouseIndex = textBox.GetMouseHoverIndex().GetValueOrDefault(-1);
            var selectedText = textBox.GetSelectedText();
            int begin, end;
            textBox.GetSelection(out begin, out end);
            bool isOnSelected = mouseIndex > begin && mouseIndex < end;
            if (isOnSelected && selectedText.Length > 1)
            {
                selectedText = Regex.Replace(selectedText, "#[^\n]*", "");
                selectedText = Regex.Replace(selectedText, " _ *\r?\n", "");
                int n = lineIndex;
                var line = Target.ElementAt(n);
                while (line.Target.Content.IsContinuation) line = Target.ElementAt(++n);

                var preCells = line.Target.Content.PreDemandEvaluateFormulaCells.Where(c => !c.Content.IsContinuation).ToArray();
                var cell = new FormulaCell(selectedText, Target.BaseSolver.Target, preCells);
                var content = cell.Content;
                if (!(content is FormulaCellContentComment) && content.GetAllDefinedVariableNames().Count() == 0 && content.GetAllDefinedFunctionNames().Count() == 0 && cell.CanEvaluate)
                {
                    if (!cell.Evaluated) cell.Evaluate(Target.BaseSolver.Target);
                    if (content.ResultLevel.GetValueOrDefault(Result.Level.Error) == Result.Level.Success) helpText = content.FormulaText + " = " + content.ResultText;
                }
            }
            if (helpText == null && targetChar.HasValue && targetText != null && targetText.Contains(targetChar.Value))
            {
                var helpTarget = InputSupportEnumerator.GetInputSupportCandidateFromText(targetText, lineIndex, postText);
                helpText = helpTarget?.Information;
            }
            if (helpText == null && markID.HasValue)
            {
                helpText = AdditionalInformations[lineIndex]?.Item2;
            }

            if (string.IsNullOrEmpty(helpText))
            {
                HideTooltipHelp();
                return;
            }

            if (_toolTipHelp.Tag is string && (string)_toolTipHelp.Tag == helpText) return;

            Point position = textBox.PointToClient(Cursor.Position);
            position.Offset(0, textBox.View.LineHeight);

            position.X += _inputTextBox.PointToClient(Cursor.Position).X - textBox.PointToClient(Cursor.Position).X;

            _toolTipHelp.Tag = helpText;
            _toolTipHelp.Show(helpText, _inputTextBox, position, 5000);
        }

        private void _btnSave_Click(object sender, EventArgs e)
        {
            if (_saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            File.WriteAllText(_saveFileDialog.FileName, _inputTextBox.Text, Encoding.UTF8);
        }

        private void _btnLoad_Click(object sender, EventArgs e)
        {
            if (_openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            _inputTextBox.Text = File.ReadAllText(_openFileDialog.FileName);
        }

        private void _btnSetting_Click(object sender, EventArgs e) { OwnerMainForm.OpenSetting(); }

        private void _btnAbort_Click(object sender, EventArgs e) { Target.AbortEvaluate(); }

        private void _btnAllDelete_Click(object sender, EventArgs e) { _inputTextBox.Text = ""; }

        private void _btnSave_MouseEnter(object sender, EventArgs e) { (sender as Control).BringToFront(); }

        private void _picStatus_VisibleChanged(object sender, EventArgs e) { _btnAbort.Visible = _picStatus.Visible; }

        private void _btnMinimize_Click(object sender, EventArgs e) { WindowState = FormWindowState.Minimized; }

        #region コンテキストメニュー

        private void _contextMenuEdit_Opening(object sender, CancelEventArgs e)
        {
            _menuUndo.Enabled = _inputTextBox.CanUndo;
            _menuRedo.Enabled = _inputTextBox.CanRedo;

            _menuCopy.Enabled = _inputTextBox.CanCopy;
            _menuCut.Enabled = _inputTextBox.CanCut;
            _menuPaste.Enabled = _inputTextBox.CanPaste;
            _menuDelete.Enabled = _inputTextBox.CanCut;

            _menuSolveSimultaneousEquation.Enabled = _inputTextBox.GetSelectedText().Contains("\n");

            try
            {
                var f = FormulaOnCaret();
                _menuExpand.Enabled = true;
                _menuTidyUp.Enabled = true;
                _menuSimplify.Enabled = true;
                _menuFactorize.Enabled = true;
                _menuSubstitute.Enabled = true;
                _menuConvertUnit.Enabled = true;

                _txtBoxTargetUnit.Text = DetectTargetUnitOnCaretLine();
            }
            catch
            {
                _menuExpand.Enabled = false;
                _menuTidyUp.Enabled = false;
                _menuSimplify.Enabled = false;
                _menuFactorize.Enabled = false;
                _menuSubstitute.Enabled = false;
                _menuConvertUnit.Enabled = false;
            }
        }

        private void _menuUndo_Click(object sender, EventArgs e) { if (_inputTextBox.CanUndo) _inputTextBox.Undo(); }

        private void _menuRedo_Click(object sender, EventArgs e) { if (_inputTextBox.CanRedo) _inputTextBox.Redo(); }

        private void _menuCut_Click(object sender, EventArgs e) { if (_inputTextBox.CanCut) _inputTextBox.Cut(); }

        private void _menuCopy_Click(object sender, EventArgs e) { if (_inputTextBox.CanCopy) _inputTextBox.Copy(); }

        private void _menuPaste_Click(object sender, EventArgs e) { if (_inputTextBox.CanPaste) _inputTextBox.Paste(); }

        private void _menuDelete_Click(object sender, EventArgs e) { _inputTextBox.Delete(); }

        private void _menuSelectAll_Click(object sender, EventArgs e) { _inputTextBox.SelectAll(); }

        private void _menuJumpDefine_Click(object sender, EventArgs e) { JumpDefine(_inputTextBox); }

        private void _menuAddUserDefine_Click(object sender, EventArgs e) { } // TODO

        private void _menuCopyResult_Click(object sender, EventArgs e) { if (_resultTextBox.CanCopy) _resultTextBox.Copy(); }

        private void _menuSelectAllResult_Click(object sender, EventArgs e) { _resultTextBox.SelectAll(); }

        private void _menuJumpDefineResult_Click(object sender, EventArgs e) { JumpDefine(_resultTextBox); }

        private void _menuAddUserDefineResult_Click(object sender, EventArgs e) { } // TODO

        private void _menuSolveSimultaneousEquation_Click(object sender, EventArgs e)
        {
            EditSelectedLines((text, isLast) => { return (isLast ? "{_ " : "{  ") + text; });
        }

        private void _menuCommentOut_Click(object sender, EventArgs e)
        {
            EditSelectedLines((text, isLast) => { return "# " + text; });
        }

        private void _menuUnCommentOut_Click(object sender, EventArgs e)
        {
            EditSelectedLines((text, isLast) => {
                var textResult = text.TrimStart().TrimStart('#');
                if (textResult.StartsWith(" ")) return textResult.Substring(1);
                return textResult;
            });
        }

        private void _menuInsertHelp_Click(object sender, EventArgs e)
        {
            if (!File.Exists(_claptePadHelpFilename))
            {
                if (sender == _menuInsertHelp) MessageBox.Show(_claptePadHelpFilename + "がありません。", "ヘルプ参照エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int visible1stLine = _inputTextBox.FirstVisibleLine;
            _inputTextBox.Text = _inputTextBox.Text + "\r\n" + File.ReadAllText(_claptePadHelpFilename);
            _inputTextBox.FirstVisibleLine = visible1stLine;
        }

        private void _menuExpand_Click(object sender, EventArgs e)
        {
            DeformFormula(f => f.Expand());
        }

        private void _menuTidyUp_Click(object sender, EventArgs e)
        {
            DeformFormula(f => f.Combine());
        }

        private void _menuSimplify_Click(object sender, EventArgs e)
        {
            DeformFormula(f => f.Simplify());
        }

        private void _menuFactorize_Click(object sender, EventArgs e)
        {
            DeformFormula(f => {
                Liffom.Processes.Factorize proc = new Liffom.Processes.Factorize();
                if (f is Liffom.Formulas.Operators.Comparers.Comparer)
                {
                    var fc = f as Liffom.Formulas.Operators.Comparers.Comparer;
                    fc.LeftHandSide = proc.Do(fc.LeftHandSide);
                    fc.RightHandSide = proc.Do(fc.RightHandSide);
                    return fc;
                }
                else
                {
                    return proc.Do(f.Simplify());
                }
                });
        }

        private void _menuSubstitute_DropDownOpening(object sender, EventArgs e)
        {
            _menuSubstitute.DropDown.Items.Clear();

            Formula f = null;
            try { f = FormulaOnCaret(); }
            catch { }

            if (f != null)
            {
                foreach (var v in f.GetExistFactors<Variable>())
                {
                    var menu = new ToolStripMenuItem(v.ToString() + " =");
                    var valueBox = new ToolStripTextBox();
                    menu.DropDown.Items.Add(valueBox);
                    _menuSubstitute.DropDown.Items.Add(menu);

                    valueBox.KeyUp += (s, e2) =>
                    {
                        if (e2.KeyCode != Keys.Enter) return;
                        if (string.IsNullOrWhiteSpace(valueBox.Text)) return;
                        DeformFormula(f2 => f2.Substituted(v, Target.BaseSolver.Target.Parse(valueBox.Text)));

                        _contextMenuEdit.Hide();
                    };
                }
            }

            if (_menuSubstitute.DropDown.Items.Count == 0)
            {
                var menuDummy = new ToolStripMenuItem("変数がありません");
                menuDummy.Enabled = false;
                _menuSubstitute.DropDown.Items.Add(menuDummy);
            }
        }

        private void _txtBoxTargetUnit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetTargetUnitOnCaretLine(_txtBoxTargetUnit.Text);
                _contextMenuEdit.Hide();
            }
        }

        #endregion

        #endregion

        #region ISerializable メンバー

        /// <summary>
        /// 指定したXmlElementから状態を復元します。
        /// </summary>
        /// <param name="xmlElement">復元元Xml要素。</param>
        public void OnDeserialize(XmlElement xmlElement)
        {
            var fontName = xmlElement.GetAttribute("FontName");
            var fontSize = int.Parse(xmlElement.GetAttribute("FontSize"));
            var fontInfo = new FontInfo(fontName, fontSize, FontStyle.Regular);
            FontInfo = fontInfo;

            ShowTabChara = bool.Parse(xmlElement.GetAttribute("ShowTab", "False"));
            ShowEolChara = bool.Parse(xmlElement.GetAttribute("ShowEol", "False"));
            ShowLineNumber = bool.Parse(xmlElement.GetAttribute("ShowLineNumber", "False"));
            ShowUnderLine = bool.Parse(xmlElement.GetAttribute("ShowUnderLine", "True"));
            Delay = int.Parse(xmlElement.GetAttribute("Delay", "500"));
            AutoShowInputSupport = bool.Parse(xmlElement.GetAttribute("AutoShowInputSupport", "True"));
            AutoShowArgumentHelp = bool.Parse(xmlElement.GetAttribute("AutoShowArgumentHelp", "True"));
            InputSupportWithAlsoInfomation = bool.Parse(xmlElement.GetAttribute("InputSupportWithAlsoInfomation", "True"));

            var colorSchemeElement = xmlElement["ColorScheme"];
            SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Constant, Color.FromArgb(int.Parse(colorSchemeElement["Constant"].GetAttribute("Color"))));
            SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Function, Color.FromArgb(int.Parse(colorSchemeElement["Function"].GetAttribute("Color"))));
            SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Unit, Color.FromArgb(int.Parse(colorSchemeElement["Unit"].GetAttribute("Color"))));
            SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Comment, Color.FromArgb(int.Parse(colorSchemeElement["Comment"].GetAttribute("Color"))));
            SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Operator, Color.FromArgb(int.Parse(colorSchemeElement["Operator"].GetAttribute("Color"))));
        }

        /// <summary>
        /// 状態をXmlElementに保存します。
        /// </summary>
        /// <param name="xmlElement">保存先のXmlElement。</param>
        public void OnSerialize(XmlElement xmlElement)
        {
            xmlElement.AddAttribute("FontName", FontInfo.Name);
            xmlElement.AddAttribute("FontSize", FontInfo.Size.ToString());

            xmlElement.AddAttribute("ShowTab", ShowTabChara.ToString());
            xmlElement.AddAttribute("ShowEol", ShowEolChara.ToString());
            xmlElement.AddAttribute("ShowLineNumber", ShowLineNumber.ToString());
            xmlElement.AddAttribute("ShowUnderLine", ShowUnderLine.ToString());
            xmlElement.AddAttribute("Delay", Delay.ToString());
            xmlElement.AddAttribute("AutoShowInputSupport", Support.AutoShow.ToString());
            xmlElement.AddAttribute("AutoShowArgumentHelp", ArgumentHelper.AutoShow.ToString());
            xmlElement.AddAttribute("InputSupportWithAlsoInfomation", InputSupportWithAlsoInfomation.ToString());

            XmlElement colorScheme = new XmlElement("ColorScheme");
            XmlElement colorConstant = new XmlElement("Constant");
            colorConstant.AddAttribute("Color", GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Constant).ToArgb().ToString());
            colorScheme.AddElements(colorConstant);

            XmlElement colorFunction = new XmlElement("Function");
            colorFunction.AddAttribute("Color", GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Function).ToArgb().ToString());
            colorScheme.AddElements(colorFunction);

            XmlElement colorUnit = new XmlElement("Unit");
            colorUnit.AddAttribute("Color", GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Unit).ToArgb().ToString());
            colorScheme.AddElements(colorUnit);

            XmlElement colorComment = new XmlElement("Comment");
            colorComment.AddAttribute("Color", GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Comment).ToArgb().ToString());
            colorScheme.AddElements(colorComment);

            XmlElement colorOperator = new XmlElement("Operator");
            colorOperator.AddAttribute("Color", GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Operator).ToArgb().ToString());
            colorScheme.AddElements(colorOperator);

            xmlElement.AddElements(colorScheme);
        }

        #endregion
    }
}
