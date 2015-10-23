using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.Views.InputSupports;
using GoodSeat.Clapte.ViewModels;
using Sgry.Azuki;
using Sgry.Azuki.Highlighter;
using GoodSeat.Sio.Xml.Serialization;
using GoodSeat.Sio.Xml;
using System.IO;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Clapte.Views.Forms
{
    /// <summary>
    /// ClaptePadフォームを表します。
    /// </summary>
    public partial class FormOfClaptePad : ClapteFormBase, ISerializable
    {
        private string _hotSaveFilename = "ClaptePadHotText.txth";

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
        }

        /// <summary>
        /// 前回終了時の入力テキストを復元します。
        /// </summary>
        void HotLoad()
        {
            if (File.Exists(_hotSaveFilename)) _inputTextBox.Text = File.ReadAllText(_hotSaveFilename, Encoding.Default);
        }

        /// <summary>
        /// 現在のテキストを、次回起動時復元用の外部ファイルに保存します。
        /// </summary>
        void HotSave()
        {
            File.WriteAllText(_hotSaveFilename, _inputTextBox.Text, Encoding.Default);
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
        /// 指定文字列から引き当てられる最初の入力補助候補を取得します。
        /// </summary>
        /// <param name="targetText">引き当てに用いる文字列。</param>
        /// <param name="lineIndex">対象とする行番号。</param>
        /// <param name="postText">対象単語と同じ行の後方の文字列。</param>
        /// <returns>引き当てられる入力補助候補。</returns>
        public InputSupportCandidate GetInputSupportCandidateFromText(string targetText, int lineIndex, string postText)
        {
            InputSupportEnumerator.CurrentCaretLineNumber = lineIndex;
            var list = new List<InputSupportCandidate>(InputSupportEnumerator.GetAllCandidates(targetText).Where(
                        def => def != null && def.ReplaceText.TrimEnd('(') == targetText));
            if (list.Count == 0) return null;

            var helpTarget = list[0];
            if (list.Count > 1)
            {
                if (postText.StartsWith("("))
                {
                    foreach (var candidate in list)
                        if (candidate.Tag is FunctionDefine) helpTarget = candidate;
                }
                else
                {
                    foreach (var candidate in list)
                        if (!(candidate.Tag is FunctionDefine)) helpTarget = candidate;
                }
            }
            return helpTarget;
        }

        #endregion

        #region イベント対応

        /// <summary>
        /// 評価結果の変更時に呼び出されます。
        /// </summary>
        void Target_ResultChanged(object sender, EventArgs e)
        {
            string resultText = "";
            for (int i = 0; i < _inputTextBox.Document.LineCount; i++)
            {
                var result = Target.GetResultOf(i);
                resultText += result + "\r\n";
            }
            SetVisibleOfScrollBar();

            IgnoreScroll = true;
            _resultTextBox.Text = resultText;
            _resultTextBox.View.ScrollPos = _inputTextBox.View.ScrollPos;
            _resultTextBox.UpdateScrollBarRange();
            IgnoreScroll = false;

            Highlighter.Renew(_inputTextBox.Text);
        }

        /// <summary>
        /// 数式セルの評価ソルバの設定変更が完了した時に呼び出されます。
        /// </summary>
        void Target_SolversUpdated(object sender, EventArgs e)
        {
            Target.RenewAll(_inputTextBox.Text);
        }

        /// <summary>
        /// 数式セルの評価開始時に呼び出されます。
        /// </summary>
        void Target_EvaluateStarted(object sender, EventArgs e)
        {
            _picStatus.Visible = Target.IsEvaluating;
        }

        /// <summary>
        /// 数式セルの評価終了時に呼び出されます。
        /// </summary>
        void Target_EvaluateFinished(object sender, EventArgs e)
        {
            _picStatus.Visible = Target.IsEvaluating;
        }

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

        private void _inputTextBox_VScroll(object sender, EventArgs e)
        {
            _resultTextBox.View.ScrollPos = _inputTextBox.View.ScrollPos;
            _resultTextBox.UpdateScrollBarRange();
        }

        private void _resultTextBox_VScroll(object sender, EventArgs e)
        {
            if (IgnoreScroll) return;
            _inputTextBox.View.ScrollPos = _resultTextBox.View.ScrollPos;
            _inputTextBox.UpdateCaretGraphic();
        }

        protected override void OnCancel(EventArgs e)
        {
            this.Hide();
        }

        private void FormOfClaptePad_FormClosing(object sender, FormClosingEventArgs e)
        {
            HotSave();
        }

        private void FormOfClaptePad_Resize(object sender, EventArgs e)
        {
            SetVisibleOfScrollBar();
        }

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
            int lineIndex;
            string postText;
            string targetText = _inputTextBox.GetMouseHoverWord(out lineIndex, out postText);
            if (targetText == null)
            {
                hideTooltipHelp();
                return;
            }

            var helpTarget = GetInputSupportCandidateFromText(targetText, lineIndex, postText);
            if (helpTarget == null)
            {
                hideTooltipHelp();
                return;
            }

            if (_toolTipHelp.Tag is string && (string)_toolTipHelp.Tag == helpTarget.Information) return;

            Point position = _inputTextBox.PointToClient(Cursor.Position);
            position.Offset(0, _inputTextBox.View.LineHeight);
            _toolTipHelp.Tag = helpTarget.Information;
            _toolTipHelp.Show(helpTarget.Information, _inputTextBox, position, 5000);
        }

        private void hideTooltipHelp()
        {
            _toolTipHelp.Hide(_inputTextBox);
            _toolTipHelp.Tag = null;
        }

        private void _btnSave_Click(object sender, EventArgs e)
        {
            if (_saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            File.WriteAllText(_saveFileDialog.FileName, _inputTextBox.Text, Encoding.Default);
        }

        private void _btnLoad_Click(object sender, EventArgs e)
        {
            if (_openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            _inputTextBox.Text = File.ReadAllText(_openFileDialog.FileName, Encoding.Default);
        }

        private void _btnSetting_Click(object sender, EventArgs e)
        {
            OwnerMainForm.OpenSetting();
        }

        private void _btnAbort_Click(object sender, EventArgs e)
        {
            Target.AbortEvaluate();
        }

        private void _btnAllDelete_Click(object sender, EventArgs e)
        {
            _inputTextBox.Text = "";
        }

        private void _btnSave_MouseEnter(object sender, EventArgs e)
        {
            (sender as Control).BringToFront();
        }

        private void _picStatus_VisibleChanged(object sender, EventArgs e)
        {
            _btnAbort.Visible = _picStatus.Visible;
        }

        #region コンテキストメニュー

        private void _contextMenuEdit_Opening(object sender, CancelEventArgs e)
        {
            _menuUndo.Enabled = _inputTextBox.CanUndo;
            _menuRedo.Enabled = _inputTextBox.CanRedo;

            _menuCopy.Enabled = _inputTextBox.CanCopy;
            _menuCut.Enabled = _inputTextBox.CanCut;
            _menuPaste.Enabled = _inputTextBox.CanPaste;
            _menuDelete.Enabled = _inputTextBox.CanCut;
        }

        private void _menuUndo_Click(object sender, EventArgs e)
        {
            if (_inputTextBox.CanUndo) _inputTextBox.Undo();
        }

        private void _menuRedo_Click(object sender, EventArgs e)
        {
            if (_inputTextBox.CanRedo) _inputTextBox.Redo();
        }

        private void _menuCut_Click(object sender, EventArgs e)
        {
            if (_inputTextBox.CanCut) _inputTextBox.Cut();
        }

        private void _menuCopy_Click(object sender, EventArgs e)
        {
            if (_inputTextBox.CanCopy) _inputTextBox.Copy();
        }

        private void _menuPaste_Click(object sender, EventArgs e)
        {
            if (_inputTextBox.CanPaste) _inputTextBox.Paste();
        }

        private void _menuDelete_Click(object sender, EventArgs e)
        {
            _inputTextBox.Delete();
        }

        private void _menuSelectAll_Click(object sender, EventArgs e)
        {
            _inputTextBox.SelectAll();
        }

        private void _menuJumpDefine_Click(object sender, EventArgs e)
        {
            JumpDefine(_inputTextBox);
        }

        private void _menuAddUserDefine_Click(object sender, EventArgs e)
        {

        }

        private void _menuCopyResult_Click(object sender, EventArgs e)
        {
            if (_resultTextBox.CanCopy) _resultTextBox.Copy();
        }

        private void _menuSelectAllResult_Click(object sender, EventArgs e)
        {
            _resultTextBox.SelectAll();
        }

        private void _menuJumpDefineResult_Click(object sender, EventArgs e)
        {
            JumpDefine(_resultTextBox);
        }

        private void _menuAddUserDefineResult_Click(object sender, EventArgs e)
        {

        }

        private void JumpDefine(Sgry.Azuki.WinForms.AzukiControl azuki)
        {
            string targetText = azuki.GetSelectedText();

            string postText;
            int lineIndex;
            string txt = azuki.GetCaretWord(out lineIndex, out postText);
            if (string.IsNullOrEmpty(targetText)) targetText = txt;
            if (targetText == null) return;

            var helpTarget = GetInputSupportCandidateFromText(targetText, lineIndex, postText);
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
                    if (cell.Target.Content.GetAllDefinedFunctionNames().Contains(def.Name)) jump = line;
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

            if (jump >= 0)
            {
                azuki.Document.SetCaretIndex(jump, 0);
                azuki.ScrollToCaret();
            }
        }


        #endregion

        #endregion

        #region ISerializable メンバー

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

            var colorSchemeElement = xmlElement["ColorScheme"];
            SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Constant, Color.FromArgb(int.Parse(colorSchemeElement["Constant"].GetAttribute("Color"))));
            SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Function, Color.FromArgb(int.Parse(colorSchemeElement["Function"].GetAttribute("Color"))));
            SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Unit, Color.FromArgb(int.Parse(colorSchemeElement["Unit"].GetAttribute("Color"))));
            SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Comment, Color.FromArgb(int.Parse(colorSchemeElement["Comment"].GetAttribute("Color"))));
            SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Operator, Color.FromArgb(int.Parse(colorSchemeElement["Operator"].GetAttribute("Color"))));
        }

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
