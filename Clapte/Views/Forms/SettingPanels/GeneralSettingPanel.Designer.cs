namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    partial class GeneralSettingPanel
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
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._numMaxLength = new GoodSeat.Clapte.Views.Controls.NumericSlider();
            this._labelMaxLength = new System.Windows.Forms.Label();
            this._groupInput = new System.Windows.Forms.GroupBox();
            this._labelMaxUnitLength = new System.Windows.Forms.Label();
            this._numMaxUnitLength = new GoodSeat.Clapte.Views.Controls.NumericSlider();
            this._groupResult = new System.Windows.Forms.GroupBox();
            this._checkPermitAllResult = new System.Windows.Forms.CheckBox();
            this._txtBoxHotkey = new System.Windows.Forms.TextBox();
            this._labelHotkey = new System.Windows.Forms.Label();
            this._checkCopyWithHotkey = new System.Windows.Forms.CheckBox();
            this._labelEnableHotkey = new System.Windows.Forms.Label();
            this._checkCopyWithSame = new System.Windows.Forms.CheckBox();
            this._checkCtrl = new System.Windows.Forms.CheckBox();
            this._labelBalloonTime = new System.Windows.Forms.Label();
            this._checkAlt = new System.Windows.Forms.CheckBox();
            this._checkCopyWithClick = new System.Windows.Forms.CheckBox();
            this._numBalloonTime = new GoodSeat.Clapte.Views.Controls.NumericSlider();
            this._groupMode = new System.Windows.Forms.GroupBox();
            this._checkIsCalculatorMode = new System.Windows.Forms.CheckBox();
            this._toolTipHelp = new System.Windows.Forms.ToolTip(this.components);
            this._groupBoxClipboardCalc = new System.Windows.Forms.GroupBox();
            this._groupInput.SuspendLayout();
            this._groupResult.SuspendLayout();
            this._groupMode.SuspendLayout();
            this._groupBoxClipboardCalc.SuspendLayout();
            this.SuspendLayout();
            // 
            // _numMaxLength
            // 
            this._numMaxLength.BackBarColor = System.Drawing.Color.Lavender;
            this._numMaxLength.ClickChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numMaxLength.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this._numMaxLength.EnableUpDown = false;
            this._numMaxLength.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._numMaxLength.Location = new System.Drawing.Point(207, 17);
            this._numMaxLength.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this._numMaxLength.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._numMaxLength.Name = "_numMaxLength";
            this._numMaxLength.Precision = 4;
            this._numMaxLength.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this._numMaxLength.ShowButton = true;
            this._numMaxLength.Size = new System.Drawing.Size(100, 21);
            this._numMaxLength.SlideChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numMaxLength.TabIndex = 0;
            this._toolTipHelp.SetToolTip(this._numMaxLength, "クリップボード計算の対象とする、最大の文字列長を指定します。\r\nクリップボードにコピーされた文字列の長さがこの長さを超過する場合には、計算を実行しません。");
            this._numMaxLength.UnderBar = false;
            this._numMaxLength.Unit = "文字";
            this._numMaxLength.UseToolTip = true;
            this._numMaxLength.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._numMaxLength.VisibleBackBar = true;
            // 
            // _labelMaxLength
            // 
            this._labelMaxLength.AutoSize = true;
            this._labelMaxLength.Location = new System.Drawing.Point(22, 23);
            this._labelMaxLength.Name = "_labelMaxLength";
            this._labelMaxLength.Size = new System.Drawing.Size(152, 12);
            this._labelMaxLength.TabIndex = 1;
            this._labelMaxLength.Text = "計算対象とする最大文字列長";
            this._toolTipHelp.SetToolTip(this._labelMaxLength, "クリップボード計算の対象とする、最大の文字列長を指定します。");
            // 
            // _groupInput
            // 
            this._groupInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupInput.Controls.Add(this._labelMaxUnitLength);
            this._groupInput.Controls.Add(this._numMaxUnitLength);
            this._groupInput.Controls.Add(this._labelMaxLength);
            this._groupInput.Controls.Add(this._numMaxLength);
            this._groupInput.Location = new System.Drawing.Point(9, 22);
            this._groupInput.Name = "_groupInput";
            this._groupInput.Size = new System.Drawing.Size(379, 88);
            this._groupInput.TabIndex = 9;
            this._groupInput.TabStop = false;
            this._groupInput.Text = "数式認識";
            // 
            // _labelMaxUnitLength
            // 
            this._labelMaxUnitLength.AutoSize = true;
            this._labelMaxUnitLength.Location = new System.Drawing.Point(22, 53);
            this._labelMaxUnitLength.Name = "_labelMaxUnitLength";
            this._labelMaxUnitLength.Size = new System.Drawing.Size(170, 12);
            this._labelMaxUnitLength.TabIndex = 5;
            this._labelMaxUnitLength.Text = "単位として認識する最大文字列長";
            this._toolTipHelp.SetToolTip(this._labelMaxUnitLength, "単位記号として自動認識する最大の文字列長を指定します。");
            // 
            // _numMaxUnitLength
            // 
            this._numMaxUnitLength.BackBarColor = System.Drawing.Color.Lavender;
            this._numMaxUnitLength.ClickChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numMaxUnitLength.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this._numMaxUnitLength.EnableUpDown = false;
            this._numMaxUnitLength.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._numMaxUnitLength.Location = new System.Drawing.Point(207, 50);
            this._numMaxUnitLength.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this._numMaxUnitLength.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this._numMaxUnitLength.Name = "_numMaxUnitLength";
            this._numMaxUnitLength.Precision = 4;
            this._numMaxUnitLength.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this._numMaxUnitLength.ShowButton = true;
            this._numMaxUnitLength.Size = new System.Drawing.Size(100, 21);
            this._numMaxUnitLength.SlideChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numMaxUnitLength.TabIndex = 4;
            this._toolTipHelp.SetToolTip(this._numMaxUnitLength, "単位記号として自動認識する最大の文字列長を指定します。\r\n（単位記号の認識モードが\"全ての記号を認識\"の場合のみ有効です。）");
            this._numMaxUnitLength.UnderBar = false;
            this._numMaxUnitLength.Unit = "文字";
            this._numMaxUnitLength.UseToolTip = true;
            this._numMaxUnitLength.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this._numMaxUnitLength.VisibleBackBar = true;
            // 
            // _groupResult
            // 
            this._groupResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupResult.Controls.Add(this._checkPermitAllResult);
            this._groupResult.Controls.Add(this._txtBoxHotkey);
            this._groupResult.Controls.Add(this._labelHotkey);
            this._groupResult.Controls.Add(this._checkCopyWithHotkey);
            this._groupResult.Controls.Add(this._labelEnableHotkey);
            this._groupResult.Controls.Add(this._checkCopyWithSame);
            this._groupResult.Controls.Add(this._checkCtrl);
            this._groupResult.Controls.Add(this._labelBalloonTime);
            this._groupResult.Controls.Add(this._checkAlt);
            this._groupResult.Controls.Add(this._checkCopyWithClick);
            this._groupResult.Controls.Add(this._numBalloonTime);
            this._groupResult.Location = new System.Drawing.Point(9, 117);
            this._groupResult.Name = "_groupResult";
            this._groupResult.Size = new System.Drawing.Size(379, 165);
            this._groupResult.TabIndex = 11;
            this._groupResult.TabStop = false;
            this._groupResult.Text = "出力";
            // 
            // _checkPermitAllResult
            // 
            this._checkPermitAllResult.AutoSize = true;
            this._checkPermitAllResult.Location = new System.Drawing.Point(19, 52);
            this._checkPermitAllResult.Name = "_checkPermitAllResult";
            this._checkPermitAllResult.Size = new System.Drawing.Size(240, 16);
            this._checkPermitAllResult.TabIndex = 52;
            this._checkPermitAllResult.Text = "単項式にならない場合も計算結果を表示する";
            this._checkPermitAllResult.UseVisualStyleBackColor = true;
            // 
            // _txtBoxHotkey
            // 
            this._txtBoxHotkey.BackColor = System.Drawing.Color.White;
            this._txtBoxHotkey.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtBoxHotkey.Enabled = false;
            this._txtBoxHotkey.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txtBoxHotkey.Location = new System.Drawing.Point(201, 132);
            this._txtBoxHotkey.Name = "_txtBoxHotkey";
            this._txtBoxHotkey.ReadOnly = true;
            this._txtBoxHotkey.Size = new System.Drawing.Size(40, 15);
            this._txtBoxHotkey.TabIndex = 49;
            this._txtBoxHotkey.Text = "C";
            this._txtBoxHotkey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._txtBoxHotkey.KeyUp += new System.Windows.Forms.KeyEventHandler(this._txtBoxHotkey_KeyUp);
            // 
            // _labelHotkey
            // 
            this._labelHotkey.AutoSize = true;
            this._labelHotkey.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelHotkey.Location = new System.Drawing.Point(157, 132);
            this._labelHotkey.Name = "_labelHotkey";
            this._labelHotkey.Size = new System.Drawing.Size(41, 15);
            this._labelHotkey.TabIndex = 47;
            this._labelHotkey.Text = "+ key :";
            // 
            // _checkCopyWithHotkey
            // 
            this._checkCopyWithHotkey.AutoSize = true;
            this._checkCopyWithHotkey.Location = new System.Drawing.Point(19, 110);
            this._checkCopyWithHotkey.Name = "_checkCopyWithHotkey";
            this._checkCopyWithHotkey.Size = new System.Drawing.Size(139, 16);
            this._checkCopyWithHotkey.TabIndex = 8;
            this._checkCopyWithHotkey.Text = "ホットキーで結果をコピー";
            this._checkCopyWithHotkey.UseVisualStyleBackColor = true;
            this._checkCopyWithHotkey.CheckedChanged += new System.EventHandler(this._checkCopyWithHotkey_CheckedChanged);
            // 
            // _labelEnableHotkey
            // 
            this._labelEnableHotkey.AutoSize = true;
            this._labelEnableHotkey.ForeColor = System.Drawing.Color.Blue;
            this._labelEnableHotkey.Location = new System.Drawing.Point(249, 134);
            this._labelEnableHotkey.Name = "_labelEnableHotkey";
            this._labelEnableHotkey.Size = new System.Drawing.Size(69, 12);
            this._labelEnableHotkey.TabIndex = 48;
            this._labelEnableHotkey.Text = "○ 使用可能";
            // 
            // _checkCopyWithSame
            // 
            this._checkCopyWithSame.AutoSize = true;
            this._checkCopyWithSame.Location = new System.Drawing.Point(206, 86);
            this._checkCopyWithSame.Name = "_checkCopyWithSame";
            this._checkCopyWithSame.Size = new System.Drawing.Size(164, 16);
            this._checkCopyWithSame.TabIndex = 7;
            this._checkCopyWithSame.Text = "同じ式のコピーで結果をコピー";
            this._checkCopyWithSame.UseVisualStyleBackColor = true;
            // 
            // _checkCtrl
            // 
            this._checkCtrl.AutoSize = true;
            this._checkCtrl.Checked = true;
            this._checkCtrl.CheckState = System.Windows.Forms.CheckState.Checked;
            this._checkCtrl.Enabled = false;
            this._checkCtrl.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._checkCtrl.Location = new System.Drawing.Point(106, 130);
            this._checkCtrl.Name = "_checkCtrl";
            this._checkCtrl.Size = new System.Drawing.Size(45, 19);
            this._checkCtrl.TabIndex = 45;
            this._checkCtrl.Text = "Ctrl";
            this._checkCtrl.UseVisualStyleBackColor = true;
            this._checkCtrl.CheckedChanged += new System.EventHandler(this.HotkeySettingChanged);
            // 
            // _labelBalloonTime
            // 
            this._labelBalloonTime.AutoSize = true;
            this._labelBalloonTime.Location = new System.Drawing.Point(22, 28);
            this._labelBalloonTime.Name = "_labelBalloonTime";
            this._labelBalloonTime.Size = new System.Drawing.Size(92, 12);
            this._labelBalloonTime.TabIndex = 5;
            this._labelBalloonTime.Text = "バルーン表示時間";
            // 
            // _checkAlt
            // 
            this._checkAlt.AutoSize = true;
            this._checkAlt.Checked = true;
            this._checkAlt.CheckState = System.Windows.Forms.CheckState.Checked;
            this._checkAlt.Enabled = false;
            this._checkAlt.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._checkAlt.Location = new System.Drawing.Point(64, 130);
            this._checkAlt.Name = "_checkAlt";
            this._checkAlt.Size = new System.Drawing.Size(42, 19);
            this._checkAlt.TabIndex = 46;
            this._checkAlt.Text = "Alt";
            this._checkAlt.UseVisualStyleBackColor = true;
            this._checkAlt.CheckedChanged += new System.EventHandler(this.HotkeySettingChanged);
            // 
            // _checkCopyWithClick
            // 
            this._checkCopyWithClick.AutoSize = true;
            this._checkCopyWithClick.Location = new System.Drawing.Point(19, 86);
            this._checkCopyWithClick.Name = "_checkCopyWithClick";
            this._checkCopyWithClick.Size = new System.Drawing.Size(173, 16);
            this._checkCopyWithClick.TabIndex = 6;
            this._checkCopyWithClick.Text = "バルーンのクリックで結果をコピー";
            this._checkCopyWithClick.UseVisualStyleBackColor = true;
            // 
            // _numBalloonTime
            // 
            this._numBalloonTime.BackBarColor = System.Drawing.Color.Lavender;
            this._numBalloonTime.ClickChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numBalloonTime.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this._numBalloonTime.EnableUpDown = false;
            this._numBalloonTime.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._numBalloonTime.Location = new System.Drawing.Point(207, 18);
            this._numBalloonTime.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this._numBalloonTime.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this._numBalloonTime.Name = "_numBalloonTime";
            this._numBalloonTime.Precision = 4;
            this._numBalloonTime.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this._numBalloonTime.ShowButton = true;
            this._numBalloonTime.Size = new System.Drawing.Size(100, 22);
            this._numBalloonTime.SlideChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numBalloonTime.TabIndex = 4;
            this._numBalloonTime.UnderBar = false;
            this._numBalloonTime.Unit = "sec";
            this._numBalloonTime.UseToolTip = true;
            this._numBalloonTime.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._numBalloonTime.VisibleBackBar = true;
            // 
            // _groupMode
            // 
            this._groupMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupMode.Controls.Add(this._checkIsCalculatorMode);
            this._groupMode.Location = new System.Drawing.Point(4, 3);
            this._groupMode.Name = "_groupMode";
            this._groupMode.Size = new System.Drawing.Size(396, 44);
            this._groupMode.TabIndex = 12;
            this._groupMode.TabStop = false;
            this._groupMode.Text = "Clapteの動作";
            // 
            // _checkIsCalculatorMode
            // 
            this._checkIsCalculatorMode.AutoSize = true;
            this._checkIsCalculatorMode.Checked = true;
            this._checkIsCalculatorMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this._checkIsCalculatorMode.Location = new System.Drawing.Point(19, 19);
            this._checkIsCalculatorMode.Name = "_checkIsCalculatorMode";
            this._checkIsCalculatorMode.Size = new System.Drawing.Size(88, 16);
            this._checkIsCalculatorMode.TabIndex = 4;
            this._checkIsCalculatorMode.Text = "計算機モード";
            this._toolTipHelp.SetToolTip(this._checkIsCalculatorMode, "計算機モードを有効にすると、以下のような動作となります。\r\n・Clapte起動時に計算機を起動します。\r\n・計算機の右上の「×」をクリックして計算機を閉じた時に、" +
        "Clapteを終了します。");
            this._checkIsCalculatorMode.UseVisualStyleBackColor = true;
            // 
            // _toolTipHelp
            // 
            this._toolTipHelp.AutoPopDelay = 30000;
            this._toolTipHelp.InitialDelay = 500;
            this._toolTipHelp.ReshowDelay = 100;
            this._toolTipHelp.ShowAlways = true;
            // 
            // _groupBoxClipboardCalc
            // 
            this._groupBoxClipboardCalc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupBoxClipboardCalc.Controls.Add(this._groupInput);
            this._groupBoxClipboardCalc.Controls.Add(this._groupResult);
            this._groupBoxClipboardCalc.Location = new System.Drawing.Point(4, 57);
            this._groupBoxClipboardCalc.Name = "_groupBoxClipboardCalc";
            this._groupBoxClipboardCalc.Size = new System.Drawing.Size(396, 296);
            this._groupBoxClipboardCalc.TabIndex = 13;
            this._groupBoxClipboardCalc.TabStop = false;
            this._groupBoxClipboardCalc.Text = "常駐クリップボード計算";
            // 
            // GeneralSettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._groupBoxClipboardCalc);
            this.Controls.Add(this._groupMode);
            this.Name = "GeneralSettingPanel";
            this.Size = new System.Drawing.Size(408, 423);
            this._groupInput.ResumeLayout(false);
            this._groupInput.PerformLayout();
            this._groupResult.ResumeLayout(false);
            this._groupResult.PerformLayout();
            this._groupMode.ResumeLayout(false);
            this._groupMode.PerformLayout();
            this._groupBoxClipboardCalc.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.NumericSlider _numMaxLength;
        private System.Windows.Forms.Label _labelMaxLength;
        private System.Windows.Forms.GroupBox _groupInput;
        private System.Windows.Forms.GroupBox _groupResult;
        private System.Windows.Forms.CheckBox _checkCopyWithHotkey;
        private System.Windows.Forms.CheckBox _checkCopyWithSame;
        private System.Windows.Forms.Label _labelBalloonTime;
        private System.Windows.Forms.CheckBox _checkCopyWithClick;
        private Controls.NumericSlider _numBalloonTime;
        private System.Windows.Forms.TextBox _txtBoxHotkey;
        private System.Windows.Forms.Label _labelHotkey;
        private System.Windows.Forms.Label _labelEnableHotkey;
        private System.Windows.Forms.CheckBox _checkCtrl;
        private System.Windows.Forms.CheckBox _checkAlt;
        private System.Windows.Forms.Label _labelMaxUnitLength;
        private Controls.NumericSlider _numMaxUnitLength;
        private System.Windows.Forms.CheckBox _checkPermitAllResult;
        private System.Windows.Forms.GroupBox _groupMode;
        private System.Windows.Forms.CheckBox _checkIsCalculatorMode;
        private System.Windows.Forms.ToolTip _toolTipHelp;
        private System.Windows.Forms.GroupBox _groupBoxClipboardCalc;
    }
}
