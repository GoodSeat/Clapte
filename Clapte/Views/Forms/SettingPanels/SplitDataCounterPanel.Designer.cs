namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    partial class SplitDataCounterPanel
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
            this._groupSetting = new System.Windows.Forms.GroupBox();
            this._numMaxLength = new Clapte.Views.Controls.NumericSlider();
            this._labelMaxLength = new System.Windows.Forms.Label();
            this._checkListData = new System.Windows.Forms.CheckedListBox();
            this._labelContents = new System.Windows.Forms.Label();
            this._checkListSplit = new System.Windows.Forms.CheckedListBox();
            this._labelSplitMark = new System.Windows.Forms.Label();
            this._checkEnable = new System.Windows.Forms.CheckBox();
            this._groupSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // _groupSetting
            // 
            this._groupSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupSetting.Controls.Add(this._numMaxLength);
            this._groupSetting.Controls.Add(this._labelMaxLength);
            this._groupSetting.Controls.Add(this._checkListData);
            this._groupSetting.Controls.Add(this._labelContents);
            this._groupSetting.Controls.Add(this._checkListSplit);
            this._groupSetting.Controls.Add(this._labelSplitMark);
            this._groupSetting.Enabled = false;
            this._groupSetting.Location = new System.Drawing.Point(6, 8);
            this._groupSetting.Name = "_groupSetting";
            this._groupSetting.Size = new System.Drawing.Size(288, 248);
            this._groupSetting.TabIndex = 0;
            this._groupSetting.TabStop = false;
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
            this._numMaxLength.Location = new System.Drawing.Point(154, 26);
            this._numMaxLength.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this._numMaxLength.Minimum = new decimal(new int[] {
            20,
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
            this._numMaxLength.Size = new System.Drawing.Size(103, 19);
            this._numMaxLength.SlideChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numMaxLength.TabIndex = 5;
            this._numMaxLength.UnderBar = false;
            this._numMaxLength.Unit = "文字";
            this._numMaxLength.UseToolTip = true;
            this._numMaxLength.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this._numMaxLength.VisibleBackBar = true;
            // 
            // _labelMaxLength
            // 
            this._labelMaxLength.AutoSize = true;
            this._labelMaxLength.Location = new System.Drawing.Point(15, 30);
            this._labelMaxLength.Name = "_labelMaxLength";
            this._labelMaxLength.Size = new System.Drawing.Size(128, 12);
            this._labelMaxLength.TabIndex = 4;
            this._labelMaxLength.Text = "対象とする最大文字列長";
            // 
            // _checkListData
            // 
            this._checkListData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._checkListData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._checkListData.CheckOnClick = true;
            this._checkListData.FormattingEnabled = true;
            this._checkListData.Items.AddRange(new object[] {
            "数値個数",
            "合計",
            "平均",
            "最大値",
            "最小値"});
            this._checkListData.Location = new System.Drawing.Point(22, 162);
            this._checkListData.Name = "_checkListData";
            this._checkListData.Size = new System.Drawing.Size(243, 72);
            this._checkListData.TabIndex = 3;
            // 
            // _labelContents
            // 
            this._labelContents.AutoSize = true;
            this._labelContents.Location = new System.Drawing.Point(14, 146);
            this._labelContents.Name = "_labelContents";
            this._labelContents.Size = new System.Drawing.Size(57, 12);
            this._labelContents.TabIndex = 2;
            this._labelContents.Text = "集計データ";
            // 
            // _checkListSplit
            // 
            this._checkListSplit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._checkListSplit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._checkListSplit.CheckOnClick = true;
            this._checkListSplit.FormattingEnabled = true;
            this._checkListSplit.Items.AddRange(new object[] {
            "改行",
            "半角スペース",
            "全角スペース",
            "タブ",
            "カンマ"});
            this._checkListSplit.Location = new System.Drawing.Point(22, 73);
            this._checkListSplit.MultiColumn = true;
            this._checkListSplit.Name = "_checkListSplit";
            this._checkListSplit.Size = new System.Drawing.Size(243, 58);
            this._checkListSplit.TabIndex = 1;
            // 
            // _labelSplitMark
            // 
            this._labelSplitMark.AutoSize = true;
            this._labelSplitMark.Location = new System.Drawing.Point(15, 57);
            this._labelSplitMark.Name = "_labelSplitMark";
            this._labelSplitMark.Size = new System.Drawing.Size(61, 12);
            this._labelSplitMark.TabIndex = 0;
            this._labelSplitMark.Text = "区切り文字";
            // 
            // _checkEnable
            // 
            this._checkEnable.AutoSize = true;
            this._checkEnable.Location = new System.Drawing.Point(17, 6);
            this._checkEnable.Name = "_checkEnable";
            this._checkEnable.Size = new System.Drawing.Size(175, 16);
            this._checkEnable.TabIndex = 0;
            this._checkEnable.Text = "区切り数値の集計を有効にする";
            this._checkEnable.UseVisualStyleBackColor = true;
            // 
            // SplitDataCounterPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._checkEnable);
            this.Controls.Add(this._groupSetting);
            this.Name = "SplitDataCounterPanel";
            this.Size = new System.Drawing.Size(298, 260);
            this._groupSetting.ResumeLayout(false);
            this._groupSetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox _groupSetting;
        private Controls.NumericSlider _numMaxLength;
        private System.Windows.Forms.Label _labelMaxLength;
        private System.Windows.Forms.CheckedListBox _checkListData;
        private System.Windows.Forms.Label _labelContents;
        private System.Windows.Forms.CheckedListBox _checkListSplit;
        private System.Windows.Forms.Label _labelSplitMark;
        private System.Windows.Forms.CheckBox _checkEnable;
    }
}
