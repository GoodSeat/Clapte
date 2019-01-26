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
using GoodSeat.Clapte.ViewModels;
using GoodSeat.Sio.Xml.Serialization;
using GoodSeat.Sio.Xml;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Deforms;
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

            _treeViewHistory.DrawMode = TreeViewDrawMode.OwnerDrawText;
            _treeViewHistory.DrawNode += _treeViewHistory_DrawNode;

            _inputTextBox.LineDrawn += _inputTextBox_LineDrawn;
            _resultTextBox.LineDrawn += _inputTextBox_LineDrawn;

            ShowOKButton = false;
            ShowCancelButton = false;

            OwnerMainForm = mainForm;
            Target = new FormulaCellListViewModel(mainForm.ClapteCore.Solver, mainForm.UserConstants, mainForm.UserFunctions);
            Target.ResultChanged += new EventHandler(Target_ResultChanged);
            Target.SolversUpdated += new EventHandler(Target_SolversUpdated);
            Target.EvaluateStarted += Target_EvaluateStarted;
            Target.EvaluateFinished += Target_EvaluateFinished;

            InitializeTextBox();

            EditorViewModel = new ClaptePadViewModel(this, _inputTextBox, _splitContainerAll.Location);

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
        public bool ShowUnderLine { get; set; }

        /// <summary>
        /// 入力から計算までに遅延時間(ms)を設定もしくは取得します。
        /// </summary>
        public int Delay { get; set; }



        /// <summary>
        /// 親となるClapteの常駐メインフォームを取得します。
        /// </summary>
        public FormOfMain OwnerMainForm { get; private set; }

        /// <summary>
        /// 対象の数式セルリストビューモデルを取得します。
        /// </summary>
        public FormulaCellListViewModel Target { get; private set; }

        /// <summary>
        /// 入力テキストの編集ビューモデルを取得します。
        /// </summary>
        public ClaptePadViewModel EditorViewModel { get; private set; }

        /// <summary>
        /// シンタックスハイライトオブジェクトを設定もしくは取得します。
        /// </summary>
        public ClaptePadKeywordHighlighter Highlighter { get; set; }


        /// <summary>
        /// 入力と結果のテキストボックスのスクロール同期処理の無効フラグです。
        /// </summary>
        private bool IgnoreScroll { get; set; }

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

            _inputTextBox.SetKeyBind(Keys.Control | Keys.F, i => OpenFindPanel());

            _resultTextBox.View.ColorScheme.SelectionBack = Color.Gray;
            _resultTextBox.View.ColorScheme.LineNumberBack = Color.White;
            _resultTextBox.View.ColorScheme.LineNumberFore = Color.DarkGray;
            _resultTextBox.View.ColorScheme.MatchedBracketBack = Color.PowderBlue;
            _resultTextBox.View.ColorScheme.HighlightColor = Color.Lavender;
            _resultTextBox.ShowsHScrollBar = false;

            _resultTextBox.SetKeyBind(Keys.Control | Keys.F, i => OpenFindPanel());

            ShowUnderLine = true;

            int parseErrorID = (int)FormulaCellContent.AdditionalInformationType.ParseError;
            Marking.Register(new MarkingInfo(parseErrorID, "構文解析エラー"));
            _inputTextBox.ColorScheme.SetMarkingDecoration(parseErrorID, new UnderlineTextDecoration(LineStyle.Waved, Color.Red));

            Marking.Register(new MarkingInfo(2, "検索のマッチ"));
            _inputTextBox.ColorScheme.SetMarkingDecoration(2, new BgColorTextDecoration(Color.Orange));
            _resultTextBox.ColorScheme.SetMarkingDecoration(2, new BgColorTextDecoration(Color.Orange));

            _panelFind.Parent = _inputTextBox;

            _greekLetterModableForFindBox = new GreekLetterModable(_textBoxFind);
            _greekLetterModableForReplaceBox = new GreekLetterModable(_textBoxReplace);
        }

        GreekLetterModable _greekLetterModableForFindBox;
        GreekLetterModable _greekLetterModableForReplaceBox;

        protected override void OnCancel(EventArgs e)
        {
            if (FormOfMain.IsCalculatorMode) OwnerMainForm.Close();
            else Hide();
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
        void HotSave() { File.WriteAllText(_hotSaveFilename, _inputTextBox.Text, Encoding.UTF8); }

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
            var targetFormula = EditorViewModel.JumpDefine(azuki);
            if (targetFormula != null) OwnerMainForm.OpenDefine(targetFormula);
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
        /// 現在のキャレット位置に基づいて、変形履歴ビューを更新します。
        /// </summary>
        void UpdateTreeViewOfDeformHistory()
        {
            if (!_menuVisibleDeformHistory.Checked) { _treeViewHistory.Nodes.Clear(); return; }

            int line, column;
            var textBox = _resultTextBox.Focused ? _resultTextBox : _inputTextBox;
            textBox.Document.GetCaretIndex(out line, out column);

            if (Target.Count() <= line) { _treeViewHistory.Nodes.Clear(); return; }

            var cell = Target.ElementAt(line);
            var history = cell.Target.Content.DeformHistory;
            if (history == null) { _treeViewHistory.Nodes.Clear(); return; }

            if (_treeViewHistory.Nodes.Count != 0)
            {
                var historyCurrent = _treeViewHistory.Nodes[0].Tag;
                if (history == historyCurrent) return;
            }
            _treeViewHistory.Nodes.Clear();

            var colorOfRule = GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Comment);
            var colorOfError = GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Error);
            foreach (var n in EditorViewModel.CreateTreeNodeOfDeformHistories(history, colorOfRule, colorOfError, _contextMenuHistoryNode))
            {
                _treeViewHistory.Nodes.Add(n);
            }
        }

        /// <summary>
        /// 検索パネルを開きます。
        /// </summary>
        private void OpenFindPanel()
        {
            var textBox = _panelFind.Parent as Sgry.Azuki.WinForms.AzukiControl;
            if (textBox == null) textBox = _inputTextBox;

            string word = "";

            int b, e;
            textBox.Document.GetSelection(out b, out e);
            if (e > b) word = textBox.Document.GetTextInRange(b, e);

            if (!_panelFind.Visible || !string.IsNullOrEmpty(word)) _textBoxFind.Text = word;
            _panelFind.Visible = true;
            _textBoxFind.Focus();
        }

        #endregion

        #region イベント対応(ビューモデルとのやり取り)

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
            EditorViewModel.AdditionalInformations.Clear();

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
                EditorViewModel.AdditionalInformations.Add(addInfo);
            }
            SetVisibleOfScrollBar();

            IgnoreScroll = true;
            _resultTextBox.Text = resultText;
            _resultTextBox.View.ScrollPos = _inputTextBox.View.ScrollPos;
            _resultTextBox.UpdateScrollBarRange();
            IgnoreScroll = false;

            Highlighter.Renew(_inputTextBox.Text);

            _lastCaretLineIndex = -1; // _inputTextBox_CaretMovedメソッド内で、強制的に再描画(マーク描画のため)させるため
            _inputTextBox_CaretMoved(_inputTextBox, null);

            if (_panelFind.Visible) _textBoxFind_TextChanged(sender, e);
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

        #endregion

        #region イベント対応(ビュー)

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

            if (_panelFind.Visible) _textBoxFind_TextChanged(sender, e);
        }

        private void _inputTextBox_FontChanged(object sender, EventArgs e)
        {
            if (sender == _inputTextBox) _resultTextBox.Font = _inputTextBox.Font;
            else _inputTextBox.Font = _resultTextBox.Font;

            _treeViewHistory.Font = _inputTextBox.Font;
            _panelFind.Font = new Font(_inputTextBox.Font.FontFamily, 9.0f);
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

        private void _treeViewHistory_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.DrawDefault = ((e.State & TreeNodeStates.Selected) == 0);
            if (e.DrawDefault) return;

            Color backColor = e.Node.BackColor;
            Color foreColor = e.Node.ForeColor;
            if (backColor == Color.Empty) backColor = Color.LightGray;
            if (foreColor == Color.Empty) foreColor = ((TreeView)sender).ForeColor;

            var rect = e.Node.Bounds;
            rect.Width = (int)(rect.Width * 1.1);
            using (Brush b = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(b, rect);
            }
            using (Brush b = new SolidBrush(foreColor))
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                e.Graphics.DrawString(e.Node.Text, e.Node.NodeFont ?? ((TreeView)sender).Font, b, rect);
            }
        }

        private void FormOfClaptePad_FormClosing(object sender, FormClosingEventArgs e) { HotSave(); }

        private void FormOfClaptePad_Resize(object sender, EventArgs e) { SetVisibleOfScrollBar(); }

        private void _timerDelay_Tick(object sender, EventArgs e)
        {
            _timerDelay.Enabled = false;
            Target.NotifyChangeText(_inputTextBox.Text);
        }

        int _lastCaretLineIndex;

        private void _inputTextBox_CaretMoved(object sender, EventArgs e)
        {
            int line, column;
            _inputTextBox.Document.GetCaretIndex(out line, out column);

            UpdateTreeViewOfDeformHistory();

            bool refreshResult = false;
            int lineOther;
            _resultTextBox.Document.GetCaretIndex(out lineOther, out column);
            if (line != lineOther)
            {
                if (line >= _resultTextBox.Document.LineCount) line = _resultTextBox.Document.LineCount - 1;
                _resultTextBox.Document.SetCaretIndex(line, 0);
                _resultTextBox.Refresh();
                refreshResult = true;
            }

            if (line != _lastCaretLineIndex)
            {
                _inputTextBox.Refresh();
                _lastCaretLineIndex = line;
                if (!refreshResult) _resultTextBox.Refresh();
            }
        }

        private void _resultTextBox_CaretMoved(object sender, EventArgs e)
        {
            if (IgnoreScroll) return;

            int line, lineOther, column;
            _resultTextBox.Document.GetCaretIndex(out line, out column);
            _inputTextBox.Document.GetCaretIndex(out lineOther, out column);

            if (line != lineOther)
            {
                if (line >= _inputTextBox.Document.LineCount) line = _inputTextBox.Document.LineCount - 1;
                _inputTextBox.Document.SetCaretIndex(line, 0);
            }
        }

        private void _inputTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            var textBox = sender as Sgry.Azuki.WinForms.AzukiControl;
            if (textBox == null) return;

            string helpText = EditorViewModel.HelpMessageOnMouse(textBox);
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

        private void _btnUpdate_Click(object sender, EventArgs e) { Target.RenewAll(_inputTextBox.Text); }

        private void _btnAbort_Click(object sender, EventArgs e) { Target.AbortEvaluate(); }

        private void _btnAllDelete_Click(object sender, EventArgs e) { _inputTextBox.Text = ""; }

        private void _btnSave_MouseEnter(object sender, EventArgs e) { (sender as Control).BringToFront(); }

        private void _picStatus_VisibleChanged(object sender, EventArgs e)
        {
            _btnAbort.Visible = _picStatus.Visible;
            _btnUpdate.Visible = !_picStatus.Visible;
        }

        private void _btnMinimize_Click(object sender, EventArgs e) { WindowState = FormWindowState.Minimized; }

        private void _btnHelp_Click(object sender, EventArgs e) { ClapteHelp.Show(this, "計算機"); }

        private void _inputTextBox_LineDrawn(object sender, LineDrawEventArgs e)
        {
            var textBox = (Sgry.Azuki.WinForms.AzukiControl)sender;

            //現在行の背景描画
            int caretIndex = textBox.CaretIndex;
            int lineIndex = textBox.GetLineIndexFromCharIndex(caretIndex);
            if (e.LineIndex == lineIndex && ShowUnderLine)
            {
                IGraphics ig = e.Graphics;
                ig.BackColor = ColorScheme.HighlightColor;

                var pt = e.Position;
                ig.RemoveClipRect();
                ig.FillRectangle(pt.X, pt.Y + textBox.View.LineHeight, textBox.Width, 1);
            }
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

            var selected = _inputTextBox.GetSelectedText();

            _menuSolveSimultaneousEquation.Enabled = selected.Contains("\n");
            Formula dummy;
            _menuDefineAsConstantOfFunction.Enabled = !string.IsNullOrEmpty(selected) && Target.BaseSolver.Target.TryParse(selected, out dummy);
            _txtBoxDefineConstantOfFunctionName.Enabled = _menuDefineAsConstantOfFunction.Enabled;
            _txtBoxDefineConstantOfFunctionName.Text = "";

            if (_splitContainerAll.Panel2.Height < 5)
            {
                _menuVisibleDeformHistory.Checked = false;
                _menuVisibleDeformHistoryResult.Checked = false;
            }
            else if (!_splitContainerAll.Panel2Collapsed)
            {
                _menuVisibleDeformHistory.Checked = true;
                _menuVisibleDeformHistoryResult.Checked = true;
            }

            try
            {
                var f = EditorViewModel.FormulaOnCaret();
                _menuExpand.Enabled = true;
                _menuTidyUp.Enabled = true;
                _menuSimplify.Enabled = true;
                _menuFactorize.Enabled = true;
                _menuSubstitute.Enabled = true;
                _menuConvertUnit.Enabled = true;

                _txtBoxTargetUnit.Text = EditorViewModel.DetectTargetUnitOnCaretLine();
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

        private void _menuFindAndReplace_Click(object sender, EventArgs e) { OpenFindPanel(); }

        private void _menuSolveSimultaneousEquation_Click(object sender, EventArgs e)
        {
            EditorViewModel.EditSelectedLines((text, isLast) => { return (isLast ? "{_ " : "{  ") + text; });
        }

        private void _menuCommentOut_Click(object sender, EventArgs e)
        {
            EditorViewModel.EditSelectedLines((text, isLast) => { return "# " + text; });
        }

        private void _menuUnCommentOut_Click(object sender, EventArgs e)
        {
            EditorViewModel.EditSelectedLines((text, isLast) => {
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

        private void _menuExpand_Click(object sender, EventArgs e) { EditorViewModel.DeformFormula(f => f.Expand()); }

        private void _menuTidyUp_Click(object sender, EventArgs e) { EditorViewModel.DeformFormula(f => f.Combine()); }

        private void _menuSimplify_Click(object sender, EventArgs e) { EditorViewModel.DeformFormula(f => f.Simplify()); }

        private void _menuFactorize_Click(object sender, EventArgs e)
        {
            EditorViewModel.DeformFormula(f => {
                var proc = new Liffom.Processes.Factorize();
                if (!(f is Liffom.Formulas.Operators.Comparers.Comparer)) return proc.Do(f.Simplify());

                var fc = f as Liffom.Formulas.Operators.Comparers.Comparer;
                fc.LeftHandSide  = proc.Do(fc.LeftHandSide.Simplify());
                fc.RightHandSide = proc.Do(fc.RightHandSide.Simplify());
                return fc;
                });
        }

        private void _menuSubstitute_DropDownOpening(object sender, EventArgs e)
        {
            _menuSubstitute.DropDown.Items.Clear();

            Formula f = null;
            try { f = EditorViewModel.FormulaOnCaret(); }
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
                        EditorViewModel.DeformFormula(f2 => f2.Substituted(v, Target.BaseSolver.Target.Parse(valueBox.Text)));

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
                EditorViewModel.SetTargetUnitOnCaretLine(_txtBoxTargetUnit.Text);
                _contextMenuEdit.Hide();
            }
        }

        private void _txtBoxDefineConstantOfFunctionName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var name = _txtBoxDefineConstantOfFunctionName.Text.Trim();
                if (!string.IsNullOrEmpty(name))
                {
                    var def = _inputTextBox.GetSelectedText().Trim();
                    string errMsg = EditorViewModel.InsertDefineWithName(name);
                    if (errMsg != null)
                    {
                        _contextMenuEdit.Hide();
                        _toolTipHelp.Show(errMsg, _inputTextBox, 3000);
                        return;
                    }

                    _panelFind.Visible = true;
                    _textBoxFind.Text = def;
                    _textBoxReplace.Text = name;
                    if (_findMatchCase) _btnToggleFindMatchCase_Click(sender, e);
                    if (_findRegex) _btnToggleFindUseRegex_Click(sender, e);
                    _textBoxReplace.Focus();
                }
                _contextMenuEdit.Hide();
            }
        }

        private void _menuVisibleDeformHistory_Click(object sender, EventArgs e)
        {
            _menuVisibleDeformHistory.Checked = !_menuVisibleDeformHistory.Checked;
            _splitContainerAll.Panel2Collapsed = !_menuVisibleDeformHistory.Checked;

            if (!_splitContainerAll.Panel2Collapsed)
            {
                var height = _splitContainerAll.Height;
                if (_splitContainerAll.Panel2.Height < height / 4) _splitContainerAll.SplitterDistance = height * 3 / 4;

                var textBox = _resultTextBox.Focused ? _resultTextBox : _inputTextBox;
                if (textBox.Focused) textBox.ScrollToCaret();
            }

            _menuVisibleDeformHistoryResult.Checked = _menuVisibleDeformHistory.Checked;

            if (_menuVisibleDeformHistoryResult.Checked)
            {
                UpdateTreeViewOfDeformHistory();
            }
        }

        private void _menuHideDeformHistory_Click(object sender, EventArgs e) { _splitContainerAll.Panel2Collapsed = true; }

        private void _menuExpandHistory_Click(object sender, EventArgs e) { _treeViewHistory.ExpandAll(); }

        private void _menuFoldHistory_Click(object sender, EventArgs e) { _treeViewHistory.CollapseAll(); }

        private void _menuCopyFormulaInHistory_Click(object sender, EventArgs e)
        {
            var tag = _treeViewHistory.SelectedNode?.Tag;
            if (tag is DeformHistory)
            {
                Clipboard.SetText((tag as DeformHistory).First().FormulaText);
            }
            else if (tag is DeformHistoryNode)
            {
                Clipboard.SetText((tag as DeformHistoryNode).FormulaText);
            }
        }

        #endregion

        #endregion

        #region 検索パネル関連

        private SearchResult Find(Document document, bool next, int pos)
        {
            SearchResult result;
            if (_findRegex)
            {
                RegexOptions option = _findMatchCase ? RegexOptions.None : RegexOptions.IgnoreCase;
                if (!next) option = option | RegexOptions.RightToLeft;
                Regex regex = new Regex(_textBoxFind.Text, option);
                result = next ? document.FindNext(regex, pos) : document.FindPrev(regex, pos);
            }
            else
            {
                result = next ? document.FindNext(_textBoxFind.Text, pos, _findMatchCase) : document.FindPrev(_textBoxFind.Text, pos, _findMatchCase);
            }
            if (result != null && result.Begin == result.End) return null;
            return result;
        }

        private bool IsMatchWithFindBoxText(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            if (_findRegex)
            {
                RegexOptions option = _findMatchCase ? RegexOptions.None : RegexOptions.IgnoreCase;
                Regex regex = new Regex(_textBoxFind.Text, option);
                return string.IsNullOrEmpty(regex.Replace(text, ""));
            }
            else
            {
                if (_findMatchCase) return text == _textBoxFind.Text;
                return text.ToLower() == _textBoxFind.Text.ToLower();
            }
        }

        private void _textBoxFind_TextChanged(object sender, EventArgs e)
        {
            _labelFind.Visible = string.IsNullOrEmpty(_textBoxFind.Text);

            // マーカー付与
            const int id = 2;
            var boxs = new List<Sgry.Azuki.WinForms.AzukiControl>();
            boxs.Add(_inputTextBox);
            boxs.Add(_resultTextBox);
            foreach (var textBox in boxs)
            {
                var document = textBox.Document;
                document.Unmark(0, document.Length, id);

                if (!string.IsNullOrWhiteSpace(_textBoxFind.Text))
                {
                    int pos = 0;
                    do
                    {
                        try
                        {
                            var result = Find(document, true, pos);
                            if (result == null) break;

                            document.Mark(result.Begin, result.End, id);
                            pos = result.End;
                        }
                        catch
                        {
                            break;
                        }
                    } while (true);
                }
                textBox.Refresh();
            }
        }

        private void _textBoxReplace_TextChanged(object sender, EventArgs e)
        {
            _labelReplace.Visible = string.IsNullOrEmpty(_textBoxReplace.Text);
        }

        private void _labelFind_Click(object sender, EventArgs e) { _textBoxFind.Focus(); }

        private void _labelReplace_Click(object sender, EventArgs e) { _textBoxReplace.Focus(); }

        private void _btnFindNext_Click(object sender, EventArgs e)
        {
            _toolTipFind.RemoveAll();

            var textBox = _panelFind.Parent as Sgry.Azuki.WinForms.AzukiControl;
            if (textBox == null) textBox = _inputTextBox;

            var document = textBox.Document;

            int begin, end;
            document.GetSelection(out begin, out end);

            try
            {
                var result = Find(document, true, end);
                if (result != null)
                {
                    document.SetSelection(result.Begin, result.End);
                    textBox.ScrollToCaret();
                }
                else
                {
                    _toolTipFind.Show("見つかりませんでした", _textBoxFind, 1000);
                }
            }
            catch (Exception exc) { _toolTipFind.Show(exc.Message, _textBoxFind, 1000); }
        }

        private void _btnFindPrev_Click(object sender, EventArgs e)
        {
            _toolTipFind.RemoveAll();

            var textBox = _panelFind.Parent as Sgry.Azuki.WinForms.AzukiControl;
            if (textBox == null) textBox = _inputTextBox;

            var document = textBox.Document;

            int begin, end;
            document.GetSelection(out begin, out end);

            try
            {
                var result = Find(document, false, begin);
                if (result != null)
                {
                    document.SetSelection(result.Begin, result.End);
                    textBox.ScrollToCaret();
                }
                else
                {
                    _toolTipFind.Show("見つかりませんでした", _textBoxFind, 1000);
                }
            }
            catch (Exception exc) { _toolTipFind.Show(exc.Message, _textBoxFind, 1000); }
        }

        private void _inputTextBox_Enter(object sender, EventArgs e)
        {
            var textBox = sender as Sgry.Azuki.WinForms.AzukiControl;
            _panelFind.Parent = textBox;

            int add = 0;
            if (textBox == _resultTextBox) add = 16;
            _panelFind.Left = textBox.Width - _panelFind.Width - add;
        }

        private void _btnHideFindPanel_Click(object sender, EventArgs e)
        {
            _panelFind.Visible = false;
            _textBoxFind.Text = "";
            _textBoxReplace.Text = "";

            _panelFind.Parent.Focus();
        }

        private void _btnFindNext_MouseEnter(object sender, EventArgs e)
        {
            if (sender != _btnToggleFindUseRegex) (sender as Control).BringToFront();
            _toolTipFind.RemoveAll();
        }

        private void _btnReplaceAndNext_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_textBoxFind.Text)) return;

            var textBox = _panelFind.Parent as Sgry.Azuki.WinForms.AzukiControl;
            if (textBox == null) textBox = _inputTextBox;
            if (textBox == _resultTextBox) return;

            string word = "";

            int b, end;
            textBox.Document.GetSelection(out b, out end);
            if (end > b) word = textBox.Document.GetTextInRange(b, end);

            if (IsMatchWithFindBoxText(word)) textBox.Document.Replace(_textBoxReplace.Text);

            _btnFindNext_Click(sender, e);
        }

        private void _btnReplaceAndPrev_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_textBoxFind.Text)) return;

            var textBox = _panelFind.Parent as Sgry.Azuki.WinForms.AzukiControl;
            if (textBox == null) textBox = _inputTextBox;
            if (textBox == _resultTextBox) return;

            string word = "";

            int b, end;
            textBox.Document.GetSelection(out b, out end);
            if (end > b) word = textBox.Document.GetTextInRange(b, end);

            if (IsMatchWithFindBoxText(word)) textBox.Document.Replace(_textBoxReplace.Text);

            _btnFindPrev_Click(sender, e);
        }

        private void _btnReplaceAll_Click(object sender, EventArgs e)
        {
            var textBox = _panelFind.Parent as Sgry.Azuki.WinForms.AzukiControl;
            if (textBox == null) textBox = _inputTextBox;
            if (textBox == _resultTextBox) return;
            if (string.IsNullOrEmpty(_textBoxFind.Text)) return;

            try
            {
                var text = _findRegex ? _textBoxFind.Text : Regex.Escape(_textBoxFind.Text);
                RegexOptions option = _findMatchCase ? RegexOptions.None : RegexOptions.IgnoreCase;
                Regex regex = new Regex(text, option);

                EditorViewModel.EditAllLines(l => regex.Replace(l, _textBoxReplace.Text));
            }
            catch (Exception exc) { _toolTipFind.Show(exc.Message, _textBoxFind, 1000); }
        }

        private void _btnToggleFindPanelPosition_Click(object sender, EventArgs e)
        {
            if (_panelFind.Anchor == (AnchorStyles.Top | AnchorStyles.Right))
            {
                _panelFind.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                _panelFind.Top = _inputTextBox.Height - _panelFind.Height;
            }
            else
            {
                _panelFind.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                _panelFind.Top = 0;
            }
        }

        private void _textBoxFind_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) _btnHideFindPanel_Click(sender, e);
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Shift) _btnFindPrev_Click(sender, e);
                else _btnFindNext_Click(sender, e);
            }
        }

        private void _textBoxReplace_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) _btnHideFindPanel_Click(sender, e);
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Shift) _btnReplaceAndPrev_Click(sender, e);
                else _btnReplaceAndNext_Click(sender, e);
            }
        }

        bool _findMatchCase;
        private void _btnToggleFindMatchCase_Click(object sender, EventArgs e)
        {
            _findMatchCase = !_findMatchCase;
            _btnToggleFindMatchCase.UnFocusImage = _findMatchCase ? Properties.Resources.Icon_MatchCase : Properties.Resources.Icon_MatchCase_Unfocus;

            _textBoxFind_TextChanged(sender, e);
        }

        bool _findRegex;
        private void _btnToggleFindUseRegex_Click(object sender, EventArgs e)
        {
            _findRegex = !_findRegex;
            _btnToggleFindUseRegex.UnFocusImage = _findRegex ? Properties.Resources.Icon_UseRegex : Properties.Resources.Icon_UseRegex_Unfocus;

            _textBoxFind_TextChanged(sender, e);
        }
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
            EditorViewModel.AutoShowInputSupport = bool.Parse(xmlElement.GetAttribute("AutoShowInputSupport", "True"));
            EditorViewModel.AutoShowArgumentHelp = bool.Parse(xmlElement.GetAttribute("AutoShowArgumentHelp", "True"));
            EditorViewModel.InputSupportWithAlsoInfomation = bool.Parse(xmlElement.GetAttribute("InputSupportWithAlsoInfomation", "True"));

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
            xmlElement.AddAttribute("AutoShowInputSupport", EditorViewModel.InputSupport.AutoShow.ToString());
            xmlElement.AddAttribute("AutoShowArgumentHelp", EditorViewModel.ArgumentHelper.AutoShow.ToString());
            xmlElement.AddAttribute("InputSupportWithAlsoInfomation", EditorViewModel.InputSupportWithAlsoInfomation.ToString());

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
