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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOfClaptePad));
            Sgry.Azuki.FontInfo fontInfo1 = new Sgry.Azuki.FontInfo();
            Sgry.Azuki.FontInfo fontInfo2 = new Sgry.Azuki.FontInfo();
            this._inputTextBox = new Sgry.Azuki.WinForms.AzukiControl();
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._resultTextBox = new Sgry.Azuki.WinForms.AzukiControl();
            this._picStatus = new System.Windows.Forms.PictureBox();
            this._btnAbort = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._btnLoad = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._btnSave = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._timerDelay = new System.Windows.Forms.Timer(this.components);
            this._toolTipHoverHelp = new System.Windows.Forms.ToolTip(this.components);
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._picStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnAbort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnLoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnSave)).BeginInit();
            this.SuspendLayout();
            // 
            // _inputTextBox
            // 
            this._inputTextBox.BackColor = System.Drawing.Color.White;
            this._inputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
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
            this._splitContainer.Panel1.Controls.Add(this._inputTextBox);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._resultTextBox);
            this._splitContainer.Size = new System.Drawing.Size(520, 326);
            this._splitContainer.SplitterDistance = 330;
            this._splitContainer.TabIndex = 17;
            // 
            // _resultTextBox
            // 
            this._resultTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this._resultTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
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
            // _picStatus
            // 
            this._picStatus.Image = global::GoodSeat.Clapte.Properties.Resources.status_anim;
            this._picStatus.Location = new System.Drawing.Point(73, 12);
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
            this._btnAbort.Location = new System.Drawing.Point(52, 11);
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
            this._btnLoad.Location = new System.Drawing.Point(27, 4);
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
            this._openFileDialog.Filter = "Clapteテキスト|*.txt;*.txtr;*.txta|すべてのファイル|*.*";
            this._openFileDialog.InitialDirectory = "Notes";
            this._openFileDialog.Title = "テキストの読み込み";
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.DefaultExt = "txt";
            this._saveFileDialog.Filter = "入力テキスト|*.txt|結果テキスト|*.txtr|入力及び結果テキスト|*.txta|すべてのファイル|*.*";
            this._saveFileDialog.InitialDirectory = "Notes";
            this._saveFileDialog.Title = "テキストの保存";
            // 
            // FormOfClaptePad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 360);
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
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._picStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnAbort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnLoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnSave)).EndInit();
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
    }
}
