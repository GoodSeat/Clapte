namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    partial class CalculatorSettingPanel
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            _installedFonts.Dispose();
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            Sgry.Azuki.FontInfo fontInfo1 = new Sgry.Azuki.FontInfo();
            this._labelConstant = new System.Windows.Forms.Label();
            this._picColorConstant = new System.Windows.Forms.PictureBox();
            this._picColorFunction = new System.Windows.Forms.PictureBox();
            this._labelFunction = new System.Windows.Forms.Label();
            this._picColorUnit = new System.Windows.Forms.PictureBox();
            this._labelUnit = new System.Windows.Forms.Label();
            this._picColorOperator = new System.Windows.Forms.PictureBox();
            this._labelOperator = new System.Windows.Forms.Label();
            this._groupHighlight = new System.Windows.Forms.GroupBox();
            this._picColorComment = new System.Windows.Forms.PictureBox();
            this._labelComment = new System.Windows.Forms.Label();
            this._colorDialog = new System.Windows.Forms.ColorDialog();
            this._fontDialog = new System.Windows.Forms.FontDialog();
            this._comboFont = new System.Windows.Forms.ComboBox();
            this._numFontSize = new GoodSeat.Clapte.Views.Controls.NumericSlider();
            this._checkVisibleTab = new System.Windows.Forms.CheckBox();
            this._groupFont = new System.Windows.Forms.GroupBox();
            this._checkUnderline = new System.Windows.Forms.CheckBox();
            this._textBoxSample = new Sgry.Azuki.WinForms.AzukiControl();
            this._checkAutoShowInputSupport = new System.Windows.Forms.CheckBox();
            this._checkAutoShowArgumentHint = new System.Windows.Forms.CheckBox();
            this._checkVisibleEol = new System.Windows.Forms.CheckBox();
            this._groupSupport = new System.Windows.Forms.GroupBox();
            this._checkInputSupportAlsoInfomation = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this._picColorConstant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picColorFunction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picColorUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picColorOperator)).BeginInit();
            this._groupHighlight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._picColorComment)).BeginInit();
            this._groupFont.SuspendLayout();
            this._groupSupport.SuspendLayout();
            this.SuspendLayout();
            // 
            // _labelConstant
            // 
            this._labelConstant.AutoSize = true;
            this._labelConstant.Location = new System.Drawing.Point(24, 27);
            this._labelConstant.Name = "_labelConstant";
            this._labelConstant.Size = new System.Drawing.Size(29, 12);
            this._labelConstant.TabIndex = 0;
            this._labelConstant.Text = "定数";
            // 
            // _picColorConstant
            // 
            this._picColorConstant.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._picColorConstant.Location = new System.Drawing.Point(69, 23);
            this._picColorConstant.Name = "_picColorConstant";
            this._picColorConstant.Size = new System.Drawing.Size(37, 22);
            this._picColorConstant.TabIndex = 1;
            this._picColorConstant.TabStop = false;
            this._picColorConstant.Click += new System.EventHandler(this.ColorSampleClicked);
            // 
            // _picColorFunction
            // 
            this._picColorFunction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._picColorFunction.Location = new System.Drawing.Point(69, 51);
            this._picColorFunction.Name = "_picColorFunction";
            this._picColorFunction.Size = new System.Drawing.Size(37, 22);
            this._picColorFunction.TabIndex = 3;
            this._picColorFunction.TabStop = false;
            this._picColorFunction.Click += new System.EventHandler(this.ColorSampleClicked);
            // 
            // _labelFunction
            // 
            this._labelFunction.AutoSize = true;
            this._labelFunction.Location = new System.Drawing.Point(24, 56);
            this._labelFunction.Name = "_labelFunction";
            this._labelFunction.Size = new System.Drawing.Size(29, 12);
            this._labelFunction.TabIndex = 2;
            this._labelFunction.Text = "関数";
            // 
            // _picColorUnit
            // 
            this._picColorUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._picColorUnit.Location = new System.Drawing.Point(196, 23);
            this._picColorUnit.Name = "_picColorUnit";
            this._picColorUnit.Size = new System.Drawing.Size(37, 22);
            this._picColorUnit.TabIndex = 5;
            this._picColorUnit.TabStop = false;
            this._picColorUnit.Click += new System.EventHandler(this.ColorSampleClicked);
            // 
            // _labelUnit
            // 
            this._labelUnit.AutoSize = true;
            this._labelUnit.Location = new System.Drawing.Point(148, 27);
            this._labelUnit.Name = "_labelUnit";
            this._labelUnit.Size = new System.Drawing.Size(29, 12);
            this._labelUnit.TabIndex = 4;
            this._labelUnit.Text = "単位";
            // 
            // _picColorOperator
            // 
            this._picColorOperator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._picColorOperator.Location = new System.Drawing.Point(196, 51);
            this._picColorOperator.Name = "_picColorOperator";
            this._picColorOperator.Size = new System.Drawing.Size(37, 22);
            this._picColorOperator.TabIndex = 7;
            this._picColorOperator.TabStop = false;
            this._picColorOperator.Click += new System.EventHandler(this.ColorSampleClicked);
            // 
            // _labelOperator
            // 
            this._labelOperator.AutoSize = true;
            this._labelOperator.Location = new System.Drawing.Point(137, 57);
            this._labelOperator.Name = "_labelOperator";
            this._labelOperator.Size = new System.Drawing.Size(53, 12);
            this._labelOperator.TabIndex = 6;
            this._labelOperator.Text = "演算記号";
            // 
            // _groupHighlight
            // 
            this._groupHighlight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupHighlight.Controls.Add(this._picColorComment);
            this._groupHighlight.Controls.Add(this._labelComment);
            this._groupHighlight.Controls.Add(this._labelConstant);
            this._groupHighlight.Controls.Add(this._picColorConstant);
            this._groupHighlight.Controls.Add(this._picColorOperator);
            this._groupHighlight.Controls.Add(this._labelFunction);
            this._groupHighlight.Controls.Add(this._labelOperator);
            this._groupHighlight.Controls.Add(this._picColorFunction);
            this._groupHighlight.Controls.Add(this._picColorUnit);
            this._groupHighlight.Controls.Add(this._labelUnit);
            this._groupHighlight.Location = new System.Drawing.Point(12, 11);
            this._groupHighlight.Name = "_groupHighlight";
            this._groupHighlight.Size = new System.Drawing.Size(276, 118);
            this._groupHighlight.TabIndex = 8;
            this._groupHighlight.TabStop = false;
            this._groupHighlight.Text = "シンタックスハイライト";
            // 
            // _picColorComment
            // 
            this._picColorComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._picColorComment.Location = new System.Drawing.Point(69, 79);
            this._picColorComment.Name = "_picColorComment";
            this._picColorComment.Size = new System.Drawing.Size(37, 22);
            this._picColorComment.TabIndex = 9;
            this._picColorComment.TabStop = false;
            this._picColorComment.Click += new System.EventHandler(this.ColorSampleClicked);
            // 
            // _labelComment
            // 
            this._labelComment.AutoSize = true;
            this._labelComment.Location = new System.Drawing.Point(24, 85);
            this._labelComment.Name = "_labelComment";
            this._labelComment.Size = new System.Drawing.Size(38, 12);
            this._labelComment.TabIndex = 8;
            this._labelComment.Text = "コメント";
            // 
            // _fontDialog
            // 
            this._fontDialog.AllowVerticalFonts = false;
            this._fontDialog.ScriptsOnly = true;
            this._fontDialog.ShowEffects = false;
            // 
            // _comboFont
            // 
            this._comboFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._comboFont.FormattingEnabled = true;
            this._comboFont.Location = new System.Drawing.Point(17, 22);
            this._comboFont.Name = "_comboFont";
            this._comboFont.Size = new System.Drawing.Size(176, 20);
            this._comboFont.TabIndex = 9;
            this._comboFont.SelectedIndexChanged += new System.EventHandler(this.SettingChanged);
            // 
            // _numFontSize
            // 
            this._numFontSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._numFontSize.BackBarColor = System.Drawing.Color.Lavender;
            this._numFontSize.ClickChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numFontSize.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this._numFontSize.EnableUpDown = false;
            this._numFontSize.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._numFontSize.Location = new System.Drawing.Point(199, 22);
            this._numFontSize.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this._numFontSize.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this._numFontSize.Name = "_numFontSize";
            this._numFontSize.Precision = 0;
            this._numFontSize.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this._numFontSize.ShowButton = true;
            this._numFontSize.Size = new System.Drawing.Size(67, 20);
            this._numFontSize.SlideChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numFontSize.TabIndex = 10;
            this._numFontSize.UnderBar = false;
            this._numFontSize.Unit = "pt";
            this._numFontSize.UseToolTip = true;
            this._numFontSize.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this._numFontSize.VisibleBackBar = true;
            this._numFontSize.ValueChanged += new System.EventHandler(this.SettingChanged);
            // 
            // _checkVisibleTab
            // 
            this._checkVisibleTab.AutoSize = true;
            this._checkVisibleTab.Location = new System.Drawing.Point(13, 293);
            this._checkVisibleTab.Name = "_checkVisibleTab";
            this._checkVisibleTab.Size = new System.Drawing.Size(117, 16);
            this._checkVisibleTab.TabIndex = 11;
            this._checkVisibleTab.Text = "タブ文字を表示する";
            this._checkVisibleTab.UseVisualStyleBackColor = true;
            this._checkVisibleTab.CheckedChanged += new System.EventHandler(this.SettingChanged);
            // 
            // _groupFont
            // 
            this._groupFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupFont.Controls.Add(this._comboFont);
            this._groupFont.Controls.Add(this._numFontSize);
            this._groupFont.Location = new System.Drawing.Point(12, 135);
            this._groupFont.Name = "_groupFont";
            this._groupFont.Size = new System.Drawing.Size(276, 55);
            this._groupFont.TabIndex = 12;
            this._groupFont.TabStop = false;
            this._groupFont.Text = "フォント";
            // 
            // _checkUnderline
            // 
            this._checkUnderline.AutoSize = true;
            this._checkUnderline.Location = new System.Drawing.Point(13, 315);
            this._checkUnderline.Name = "_checkUnderline";
            this._checkUnderline.Size = new System.Drawing.Size(175, 16);
            this._checkUnderline.TabIndex = 13;
            this._checkUnderline.Text = "キャレット位置に下線を表示する";
            this._checkUnderline.UseVisualStyleBackColor = true;
            this._checkUnderline.CheckedChanged += new System.EventHandler(this.SettingChanged);
            // 
            // _textBoxSample
            // 
            this._textBoxSample.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxSample.BackColor = System.Drawing.Color.White;
            this._textBoxSample.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._textBoxSample.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._textBoxSample.DrawingOption = ((Sgry.Azuki.DrawingOption)((Sgry.Azuki.DrawingOption.HighlightCurrentLine | Sgry.Azuki.DrawingOption.HighlightsMatchedBracket)));
            this._textBoxSample.DrawsEolCode = false;
            this._textBoxSample.DrawsFullWidthSpace = false;
            this._textBoxSample.DrawsTab = false;
            this._textBoxSample.FirstVisibleLine = 0;
            this._textBoxSample.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 9F);
            fontInfo1.Name = "HGｺﾞｼｯｸM";
            fontInfo1.Size = 9;
            fontInfo1.Style = System.Drawing.FontStyle.Regular;
            this._textBoxSample.FontInfo = fontInfo1;
            this._textBoxSample.ForeColor = System.Drawing.Color.Black;
            this._textBoxSample.Location = new System.Drawing.Point(13, 337);
            this._textBoxSample.Name = "_textBoxSample";
            this._textBoxSample.ScrollPos = new System.Drawing.Point(0, 0);
            this._textBoxSample.ShowsDirtBar = false;
            this._textBoxSample.ShowsHScrollBar = false;
            this._textBoxSample.ShowsLineNumber = false;
            this._textBoxSample.ShowsVScrollBar = false;
            this._textBoxSample.Size = new System.Drawing.Size(276, 100);
            this._textBoxSample.TabIndex = 17;
            this._textBoxSample.ViewWidth = 4097;
            // 
            // _checkAutoShowInputSupport
            // 
            this._checkAutoShowInputSupport.AutoSize = true;
            this._checkAutoShowInputSupport.Location = new System.Drawing.Point(14, 18);
            this._checkAutoShowInputSupport.Name = "_checkAutoShowInputSupport";
            this._checkAutoShowInputSupport.Size = new System.Drawing.Size(257, 16);
            this._checkAutoShowInputSupport.TabIndex = 18;
            this._checkAutoShowInputSupport.Text = "入力毎に入力補完を自動表示する (Ctrl+Enter)";
            this._checkAutoShowInputSupport.UseVisualStyleBackColor = true;
            // 
            // _checkAutoShowArgumentHint
            // 
            this._checkAutoShowArgumentHint.AutoSize = true;
            this._checkAutoShowArgumentHint.Location = new System.Drawing.Point(14, 62);
            this._checkAutoShowArgumentHint.Name = "_checkAutoShowArgumentHint";
            this._checkAutoShowArgumentHint.Size = new System.Drawing.Size(230, 16);
            this._checkAutoShowArgumentHint.TabIndex = 19;
            this._checkAutoShowArgumentHint.Text = "関数の引数ヘルプを自動表示する (Ctrl+h)";
            this._checkAutoShowArgumentHint.UseVisualStyleBackColor = true;
            // 
            // _checkVisibleEol
            // 
            this._checkVisibleEol.AutoSize = true;
            this._checkVisibleEol.Location = new System.Drawing.Point(155, 293);
            this._checkVisibleEol.Name = "_checkVisibleEol";
            this._checkVisibleEol.Size = new System.Drawing.Size(124, 16);
            this._checkVisibleEol.TabIndex = 20;
            this._checkVisibleEol.Text = "改行文字を表示する";
            this._checkVisibleEol.UseVisualStyleBackColor = true;
            this._checkVisibleEol.CheckedChanged += new System.EventHandler(this.SettingChanged);
            // 
            // _groupSupport
            // 
            this._groupSupport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupSupport.Controls.Add(this._checkInputSupportAlsoInfomation);
            this._groupSupport.Controls.Add(this._checkAutoShowInputSupport);
            this._groupSupport.Controls.Add(this._checkAutoShowArgumentHint);
            this._groupSupport.Location = new System.Drawing.Point(12, 196);
            this._groupSupport.Name = "_groupSupport";
            this._groupSupport.Size = new System.Drawing.Size(276, 91);
            this._groupSupport.TabIndex = 21;
            this._groupSupport.TabStop = false;
            this._groupSupport.Text = "補助";
            // 
            // _checkInputSupportAlsoInfomation
            // 
            this._checkInputSupportAlsoInfomation.AutoSize = true;
            this._checkInputSupportAlsoInfomation.Location = new System.Drawing.Point(14, 40);
            this._checkInputSupportAlsoInfomation.Name = "_checkInputSupportAlsoInfomation";
            this._checkInputSupportAlsoInfomation.Size = new System.Drawing.Size(221, 16);
            this._checkInputSupportAlsoInfomation.TabIndex = 20;
            this._checkInputSupportAlsoInfomation.Text = "説明文も入力補完の引き当てに考慮する";
            this._checkInputSupportAlsoInfomation.UseVisualStyleBackColor = true;
            // 
            // CalculatorSettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._textBoxSample);
            this.Controls.Add(this._groupSupport);
            this.Controls.Add(this._checkVisibleEol);
            this.Controls.Add(this._checkUnderline);
            this.Controls.Add(this._groupFont);
            this.Controls.Add(this._checkVisibleTab);
            this.Controls.Add(this._groupHighlight);
            this.Name = "CalculatorSettingPanel";
            this.Size = new System.Drawing.Size(301, 449);
            ((System.ComponentModel.ISupportInitialize)(this._picColorConstant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picColorFunction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picColorUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picColorOperator)).EndInit();
            this._groupHighlight.ResumeLayout(false);
            this._groupHighlight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._picColorComment)).EndInit();
            this._groupFont.ResumeLayout(false);
            this._groupSupport.ResumeLayout(false);
            this._groupSupport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _labelConstant;
        private System.Windows.Forms.PictureBox _picColorConstant;
        private System.Windows.Forms.PictureBox _picColorFunction;
        private System.Windows.Forms.Label _labelFunction;
        private System.Windows.Forms.PictureBox _picColorUnit;
        private System.Windows.Forms.Label _labelUnit;
        private System.Windows.Forms.PictureBox _picColorOperator;
        private System.Windows.Forms.Label _labelOperator;
        private System.Windows.Forms.GroupBox _groupHighlight;
        private System.Windows.Forms.ColorDialog _colorDialog;
        private System.Windows.Forms.FontDialog _fontDialog;
        private System.Windows.Forms.ComboBox _comboFont;
        private Controls.NumericSlider _numFontSize;
        private System.Windows.Forms.CheckBox _checkVisibleTab;
        private System.Windows.Forms.PictureBox _picColorComment;
        private System.Windows.Forms.Label _labelComment;
        private System.Windows.Forms.GroupBox _groupFont;
        private System.Windows.Forms.CheckBox _checkUnderline;
        private Sgry.Azuki.WinForms.AzukiControl _textBoxSample;
        private System.Windows.Forms.CheckBox _checkAutoShowInputSupport;
        private System.Windows.Forms.CheckBox _checkAutoShowArgumentHint;
        private System.Windows.Forms.CheckBox _checkVisibleEol;
        private System.Windows.Forms.GroupBox _groupSupport;
        private System.Windows.Forms.CheckBox _checkInputSupportAlsoInfomation;
    }
}
