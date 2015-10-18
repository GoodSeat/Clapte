namespace GoodSeat.Clapte.Views.Forms
{
    partial class FormOfClaptePad
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Sgry.Azuki.FontInfo fontInfo1 = new Sgry.Azuki.FontInfo();
            Sgry.Azuki.FontInfo fontInfo2 = new Sgry.Azuki.FontInfo();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOfClaptePad));
            this._inputTextBox = new Sgry.Azuki.WinForms.AzukiControl();
            this._contextMenuEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._menuUndo = new System.Windows.Forms.ToolStripMenuItem();
            this._menuRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._menuCut = new System.Windows.Forms.ToolStripMenuItem();
            this._menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this._menuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this._menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._menuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._menuJumpDefine = new System.Windows.Forms.ToolStripMenuItem();
            this._menuAddUserDefine = new System.Windows.Forms.ToolStripMenuItem();
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._btnAllDelete = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._resultTextBox = new Sgry.Azuki.WinForms.AzukiControl();
            this._contextMenuResult = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._menuCopyResult = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this._menuSelectAllResult = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this._menuJumpDefineResult = new System.Windows.Forms.ToolStripMenuItem();
            this._menuAddUserDefineResult = new System.Windows.Forms.ToolStripMenuItem();
            this._picStatus = new System.Windows.Forms.PictureBox();
            this._btnAbort = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._btnLoad = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._btnSave = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._timerDelay = new System.Windows.Forms.Timer(this.components);
            this._toolTipHoverHelp = new System.Windows.Forms.ToolTip(this.components);
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._btnSetting = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._contextMenuEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._btnAllDelete)).BeginInit();
            this._contextMenuResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._picStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnAbort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnLoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnSetting)).BeginInit();
            this.SuspendLayout();
            // 
            // _inputTextBox
            // 
            this._inputTextBox.BackColor = System.Drawing.Color.White;
            this._inputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._inputTextBox.ContextMenuStrip = this._contextMenuEdit;
            this._inputTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._inputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._inputTextBox.DrawingOption = ((Sgry.Azuki.DrawingOption)((Sgry.Azuki.DrawingOption.HighlightCurrentLine | Sgry.Azuki.DrawingOption.HighlightsMatchedBracket)));
            this._inputTextBox.DrawsEolCode = false;
            this._inputTextBox.DrawsFullWidthSpace = false;
            this._inputTextBox.DrawsTab = false;
            this._inputTextBox.FirstVisibleLine = 0;
            this._inputTextBox.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 9F);
            fontInfo1.Name = "HGｺﾞｼｯｸM";
            fontInfo1.Size = 9;
            fontInfo1.Style = System.Drawing.FontStyle.Regular;
            this._inputTextBox.FontInfo = fontInfo1;
            this._inputTextBox.ForeColor = System.Drawing.Color.Black;
            this._inputTextBox.Location = new System.Drawing.Point(0, 0);
            this._inputTextBox.Name = "_inputTextBox";
            this._inputTextBox.ScrollPos = new System.Drawing.Point(0, 0);
            this._inputTextBox.ScrollsBeyondLastLine = false;
            this._inputTextBox.ShowsDirtBar = false;
            this._inputTextBox.ShowsHScrollBar = false;
            this._inputTextBox.ShowsLineNumber = false;
            this._inputTextBox.ShowsVScrollBar = false;
            this._inputTextBox.Size = new System.Drawing.Size(330, 326);
            this._inputTextBox.TabIndex = 16;
            this._inputTextBox.ViewWidth = 4097;
            this._inputTextBox.CaretMoved += new System.EventHandler(this._inputTextBox_CaretMoved);
            this._inputTextBox.VScroll += new System.EventHandler(this._inputTextBox_VScroll);
            this._inputTextBox.TextChanged += new System.EventHandler(this._inputTextBox_TextChanged);
            this._inputTextBox.MouseHover += new System.EventHandler(this._inputTextBox_MouseHover);
            // 
            // _contextMenuEdit
            // 
            this._contextMenuEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuUndo,
            this._menuRedo,
            this.toolStripSeparator1,
            this._menuCut,
            this._menuCopy,
            this._menuPaste,
            this._menuDelete,
            this.toolStripSeparator2,
            this._menuSelectAll,
            this.toolStripSeparator3,
            this._menuJumpDefine,
            this._menuAddUserDefine});
            this._contextMenuEdit.Name = "_contextMenuEdit";
            this._contextMenuEdit.Size = new System.Drawing.Size(184, 220);
            this._contextMenuEdit.Opening += new System.ComponentModel.CancelEventHandler(this._contextMenuEdit_Opening);
            // 
            // _menuUndo
            // 
            this._menuUndo.Name = "_menuUndo";
            this._menuUndo.Size = new System.Drawing.Size(183, 22);
            this._menuUndo.Text = "元に戻す(&U)";
            this._menuUndo.Click += new System.EventHandler(this._menuUndo_Click);
            // 
            // _menuRedo
            // 
            this._menuRedo.Name = "_menuRedo";
            this._menuRedo.Size = new System.Drawing.Size(183, 22);
            this._menuRedo.Text = "やり直す(&R)";
            this._menuRedo.Click += new System.EventHandler(this._menuRedo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(180, 6);
            // 
            // _menuCut
            // 
            this._menuCut.Name = "_menuCut";
            this._menuCut.Size = new System.Drawing.Size(183, 22);
            this._menuCut.Text = "切り取り(&T)";
            this._menuCut.Click += new System.EventHandler(this._menuCut_Click);
            // 
            // _menuCopy
            // 
            this._menuCopy.Name = "_menuCopy";
            this._menuCopy.Size = new System.Drawing.Size(183, 22);
            this._menuCopy.Text = "コピー(&C)";
            this._menuCopy.Click += new System.EventHandler(this._menuCopy_Click);
            // 
            // _menuPaste
            // 
            this._menuPaste.Name = "_menuPaste";
            this._menuPaste.Size = new System.Drawing.Size(183, 22);
            this._menuPaste.Text = "貼り付け(&P)";
            this._menuPaste.Click += new System.EventHandler(this._menuPaste_Click);
            // 
            // _menuDelete
            // 
            this._menuDelete.Name = "_menuDelete";
            this._menuDelete.Size = new System.Drawing.Size(183, 22);
            this._menuDelete.Text = "削除(&D)";
            this._menuDelete.Click += new System.EventHandler(this._menuDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(180, 6);
            // 
            // _menuSelectAll
            // 
            this._menuSelectAll.Name = "_menuSelectAll";
            this._menuSelectAll.Size = new System.Drawing.Size(183, 22);
            this._menuSelectAll.Text = "すべて選択(&A)";
            this._menuSelectAll.Click += new System.EventHandler(this._menuSelectAll_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(180, 6);
            // 
            // _menuJumpDefine
            // 
            this._menuJumpDefine.Name = "_menuJumpDefine";
            this._menuJumpDefine.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this._menuJumpDefine.Size = new System.Drawing.Size(183, 22);
            this._menuJumpDefine.Text = "定義を参照(&J)";
            this._menuJumpDefine.Click += new System.EventHandler(this._menuJumpDefine_Click);
            // 
            // _menuAddUserDefine
            // 
            this._menuAddUserDefine.Name = "_menuAddUserDefine";
            this._menuAddUserDefine.Size = new System.Drawing.Size(183, 22);
            this._menuAddUserDefine.Text = "ユーザー定義に登録(&G)";
            this._menuAddUserDefine.Click += new System.EventHandler(this._menuAddUserDefine_Click);
            // 
            // _splitContainer
            // 
            this._splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._splitContainer.Location = new System.Drawing.Point(9, 28);
            this._splitContainer.Name = "_splitContainer";
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this._btnAllDelete);
            this._splitContainer.Panel1.Controls.Add(this._inputTextBox);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._resultTextBox);
            this._splitContainer.Size = new System.Drawing.Size(520, 326);
            this._splitContainer.SplitterDistance = 330;
            this._splitContainer.TabIndex = 17;
            // 
            // _btnAllDelete
            // 
            this._btnAllDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnAllDelete.BackColor = System.Drawing.Color.White;
            this._btnAllDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this._btnAllDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnAllDelete.DownMove = 1;
            this._btnAllDelete.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_ClearFilter;
            this._btnAllDelete.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_ClearFilter_Unfocus;
            this._btnAllDelete.Location = new System.Drawing.Point(315, 1);
            this._btnAllDelete.Name = "_btnAllDelete";
            this._btnAllDelete.Size = new System.Drawing.Size(15, 15);
            this._btnAllDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._btnAllDelete.TabIndex = 22;
            this._btnAllDelete.TabStop = false;
            this._toolHelpTip.SetToolTip(this._btnAllDelete, "テキストを全削除");
            this._btnAllDelete.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_ClearFilter_Unfocus;
            this._btnAllDelete.Click += new System.EventHandler(this._btnAllDelete_Click);
            // 
            // _resultTextBox
            // 
            this._resultTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this._resultTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._resultTextBox.ContextMenuStrip = this._contextMenuResult;
            this._resultTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._resultTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._resultTextBox.DrawingOption = ((Sgry.Azuki.DrawingOption)((Sgry.Azuki.DrawingOption.HighlightCurrentLine | Sgry.Azuki.DrawingOption.HighlightsMatchedBracket)));
            this._resultTextBox.DrawsEolCode = false;
            this._resultTextBox.DrawsFullWidthSpace = false;
            this._resultTextBox.DrawsTab = false;
            this._resultTextBox.FirstVisibleLine = 0;
            this._resultTextBox.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 9F);
            fontInfo2.Name = "HGｺﾞｼｯｸM";
            fontInfo2.Size = 9;
            fontInfo2.Style = System.Drawing.FontStyle.Regular;
            this._resultTextBox.FontInfo = fontInfo2;
            this._resultTextBox.ForeColor = System.Drawing.Color.Black;
            this._resultTextBox.IsReadOnly = true;
            this._resultTextBox.Location = new System.Drawing.Point(0, 0);
            this._resultTextBox.Name = "_resultTextBox";
            this._resultTextBox.ScrollPos = new System.Drawing.Point(0, 0);
            this._resultTextBox.ScrollsBeyondLastLine = false;
            this._resultTextBox.ShowsDirtBar = false;
            this._resultTextBox.ShowsHScrollBar = false;
            this._resultTextBox.ShowsLineNumber = false;
            this._resultTextBox.Size = new System.Drawing.Size(186, 326);
            this._resultTextBox.TabIndex = 17;
            this._resultTextBox.ViewWidth = 4097;
            this._resultTextBox.VScroll += new System.EventHandler(this._resultTextBox_VScroll);
            // 
            // _contextMenuResult
            // 
            this._contextMenuResult.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuCopyResult,
            this.toolStripSeparator5,
            this._menuSelectAllResult,
            this.toolStripSeparator6,
            this._menuJumpDefineResult,
            this._menuAddUserDefineResult});
            this._contextMenuResult.Name = "_contextMenuEdit";
            this._contextMenuResult.Size = new System.Drawing.Size(184, 104);
            // 
            // _menuCopyResult
            // 
            this._menuCopyResult.Name = "_menuCopyResult";
            this._menuCopyResult.Size = new System.Drawing.Size(183, 22);
            this._menuCopyResult.Text = "コピー(&C)";
            this._menuCopyResult.Click += new System.EventHandler(this._menuCopyResult_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(180, 6);
            // 
            // _menuSelectAllResult
            // 
            this._menuSelectAllResult.Name = "_menuSelectAllResult";
            this._menuSelectAllResult.Size = new System.Drawing.Size(183, 22);
            this._menuSelectAllResult.Text = "すべて選択(&A)";
            this._menuSelectAllResult.Click += new System.EventHandler(this._menuSelectAllResult_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(180, 6);
            // 
            // _menuJumpDefineResult
            // 
            this._menuJumpDefineResult.Name = "_menuJumpDefineResult";
            this._menuJumpDefineResult.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this._menuJumpDefineResult.Size = new System.Drawing.Size(183, 22);
            this._menuJumpDefineResult.Text = "定義を参照(&J)";
            this._menuJumpDefineResult.Click += new System.EventHandler(this._menuJumpDefineResult_Click);
            // 
            // _menuAddUserDefineResult
            // 
            this._menuAddUserDefineResult.Name = "_menuAddUserDefineResult";
            this._menuAddUserDefineResult.Size = new System.Drawing.Size(183, 22);
            this._menuAddUserDefineResult.Text = "ユーザー定義に登録(&G)";
            this._menuAddUserDefineResult.Click += new System.EventHandler(this._menuAddUserDefineResult_Click);
            // 
            // _picStatus
            // 
            this._picStatus.Image = global::GoodSeat.Clapte.Properties.Resources.status_anim;
            this._picStatus.Location = new System.Drawing.Point(94, 12);
            this._picStatus.Name = "_picStatus";
            this._picStatus.Size = new System.Drawing.Size(47, 16);
            this._picStatus.TabIndex = 17;
            this._picStatus.TabStop = false;
            this._picStatus.Visible = false;
            this._picStatus.VisibleChanged += new System.EventHandler(this._picStatus_VisibleChanged);
            // 
            // _btnAbort
            // 
            this._btnAbort.BackColor = System.Drawing.Color.White;
            this._btnAbort.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this._btnAbort.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnAbort.DownMove = 1;
            this._btnAbort.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_ClearFilter;
            this._btnAbort.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_ClearFilter_Unfocus;
            this._btnAbort.Location = new System.Drawing.Point(73, 11);
            this._btnAbort.Name = "_btnAbort";
            this._btnAbort.Size = new System.Drawing.Size(15, 15);
            this._btnAbort.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._btnAbort.TabIndex = 20;
            this._btnAbort.TabStop = false;
            this._toolHelpTip.SetToolTip(this._btnAbort, "評価の中止");
            this._btnAbort.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_ClearFilter_Unfocus;
            this._btnAbort.Visible = false;
            this._btnAbort.Click += new System.EventHandler(this._btnAbort_Click);
            // 
            // _btnLoad
            // 
            this._btnLoad.BackColor = System.Drawing.Color.White;
            this._btnLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this._btnLoad.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnLoad.DownMove = 1;
            this._btnLoad.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_Open;
            this._btnLoad.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_Open_Unfocus_mini;
            this._btnLoad.Location = new System.Drawing.Point(29, 4);
            this._btnLoad.Name = "_btnLoad";
            this._btnLoad.Size = new System.Drawing.Size(26, 26);
            this._btnLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._btnLoad.TabIndex = 19;
            this._btnLoad.TabStop = false;
            this._toolHelpTip.SetToolTip(this._btnLoad, "テキストファイルの読込");
            this._btnLoad.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_Open_Unfocus_mini;
            this._btnLoad.Click += new System.EventHandler(this._btnLoad_Click);
            this._btnLoad.MouseEnter += new System.EventHandler(this._btnSave_MouseEnter);
            // 
            // _btnSave
            // 
            this._btnSave.BackColor = System.Drawing.Color.White;
            this._btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this._btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnSave.DownMove = 1;
            this._btnSave.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_Save;
            this._btnSave.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_Save_Unfocus_mini;
            this._btnSave.Location = new System.Drawing.Point(5, 4);
            this._btnSave.Name = "_btnSave";
            this._btnSave.Size = new System.Drawing.Size(26, 26);
            this._btnSave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._btnSave.TabIndex = 18;
            this._btnSave.TabStop = false;
            this._toolHelpTip.SetToolTip(this._btnSave, "テキストファイルに保存");
            this._btnSave.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_Save_Unfocus_mini;
            this._btnSave.Click += new System.EventHandler(this._btnSave_Click);
            this._btnSave.MouseEnter += new System.EventHandler(this._btnSave_MouseEnter);
            // 
            // _timerDelay
            // 
            this._timerDelay.Tick += new System.EventHandler(this._timerDelay_Tick);
            // 
            // _openFileDialog
            // 
            this._openFileDialog.DefaultExt = "txt";
            this._openFileDialog.Filter = "テキストファイル(*.txt)|*.txt|すべてのファイル|*.*";
            this._openFileDialog.InitialDirectory = "Notes";
            this._openFileDialog.Title = "テキストの読み込み";
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.DefaultExt = "txt";
            this._saveFileDialog.Filter = "入力テキスト(*.txt)|*.txt|すべてのファイル|*.*";
            this._saveFileDialog.InitialDirectory = "Notes";
            this._saveFileDialog.Title = "テキストの保存";
            // 
            // _btnSetting
            // 
            this._btnSetting.BackColor = System.Drawing.Color.White;
            this._btnSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this._btnSetting.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnSetting.DownMove = 1;
            this._btnSetting.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_Setting;
            this._btnSetting.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_Setting_Unfocus;
            this._btnSetting.Location = new System.Drawing.Point(48, 4);
            this._btnSetting.Name = "_btnSetting";
            this._btnSetting.Size = new System.Drawing.Size(26, 26);
            this._btnSetting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._btnSetting.TabIndex = 21;
            this._btnSetting.TabStop = false;
            this._toolHelpTip.SetToolTip(this._btnSetting, "設定を開く");
            this._btnSetting.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_Setting_Unfocus;
            this._btnSetting.Click += new System.EventHandler(this._btnSetting_Click);
            this._btnSetting.MouseEnter += new System.EventHandler(this._btnSave_MouseEnter);
            // 
            // FormOfClaptePad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 360);
            this.Controls.Add(this._btnSetting);
            this.Controls.Add(this._picStatus);
            this.Controls.Add(this._btnAbort);
            this.Controls.Add(this._btnLoad);
            this.Controls.Add(this._btnSave);
            this.Controls.Add(this._splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormOfClaptePad";
            this.ShowCancelButton = true;
            this.ShowOKButton = true;
            this.ShowOption = true;
            this.Text = "";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOfClaptePad_FormClosing);
            this.Resize += new System.EventHandler(this.FormOfClaptePad_Resize);
            this.Controls.SetChildIndex(this._splitContainer, 0);
            this.Controls.SetChildIndex(this._btnSave, 0);
            this.Controls.SetChildIndex(this._btnLoad, 0);
            this.Controls.SetChildIndex(this._btnAbort, 0);
            this.Controls.SetChildIndex(this._picStatus, 0);
            this.Controls.SetChildIndex(this._btnSetting, 0);
            this._contextMenuEdit.ResumeLayout(false);
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._btnAllDelete)).EndInit();
            this._contextMenuResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._picStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnAbort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnLoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnSetting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sgry.Azuki.WinForms.AzukiControl _inputTextBox;
        private System.Windows.Forms.SplitContainer _splitContainer;
        private Sgry.Azuki.WinForms.AzukiControl _resultTextBox;
        private System.Windows.Forms.Timer _timerDelay;
        private System.Windows.Forms.ToolTip _toolTipHoverHelp;
        private Components.ImageButton _btnSave;
        private Components.ImageButton _btnLoad;
        private System.Windows.Forms.OpenFileDialog _openFileDialog;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private Components.ImageButton _btnAbort;
        private System.Windows.Forms.PictureBox _picStatus;
        private System.Windows.Forms.ContextMenuStrip _contextMenuEdit;
        private System.Windows.Forms.ToolStripMenuItem _menuUndo;
        private System.Windows.Forms.ToolStripMenuItem _menuRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem _menuCut;
        private System.Windows.Forms.ToolStripMenuItem _menuCopy;
        private System.Windows.Forms.ToolStripMenuItem _menuPaste;
        private System.Windows.Forms.ToolStripMenuItem _menuDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem _menuSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem _menuJumpDefine;
        private System.Windows.Forms.ToolStripMenuItem _menuAddUserDefine;
        private Components.ImageButton _btnSetting;
        private Components.ImageButton _btnAllDelete;
        private System.Windows.Forms.ContextMenuStrip _contextMenuResult;
        private System.Windows.Forms.ToolStripMenuItem _menuCopyResult;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem _menuSelectAllResult;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem _menuJumpDefineResult;
        private System.Windows.Forms.ToolStripMenuItem _menuAddUserDefineResult;
    }
}
