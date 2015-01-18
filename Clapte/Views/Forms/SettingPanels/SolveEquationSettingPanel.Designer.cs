namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
	partial class SolveEquationSettingPanel
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
			this._groupNewton = new System.Windows.Forms.GroupBox();
			this._numTryMaxCountNewton = new Clapte.Views.Controls.NumericSlider();
			this._numErrorToleranceNewton = new Clapte.Views.Controls.NumericSlider();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this._numInitialSolutionNewton = new Clapte.Views.Controls.NumericSlider();
			this._groupBrent = new System.Windows.Forms.GroupBox();
			this._numLowerLimitBrent = new Clapte.Views.Controls.NumericSlider();
			this._numTryMaxCountBrent = new Clapte.Views.Controls.NumericSlider();
			this._numErrorToleranceBrent = new Clapte.Views.Controls.NumericSlider();
			this._numUpperLimitBrent = new Clapte.Views.Controls.NumericSlider();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this._checkDontCopyVariable = new System.Windows.Forms.CheckBox();
			this._groupNewton.SuspendLayout();
			this._groupBrent.SuspendLayout();
			this.SuspendLayout();
			// 
			// _groupNewton
			// 
			this._groupNewton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._groupNewton.Controls.Add(this._numTryMaxCountNewton);
			this._groupNewton.Controls.Add(this._numErrorToleranceNewton);
			this._groupNewton.Controls.Add(this.label3);
			this._groupNewton.Controls.Add(this.label2);
			this._groupNewton.Controls.Add(this.label1);
			this._groupNewton.Controls.Add(this._numInitialSolutionNewton);
			this._groupNewton.Location = new System.Drawing.Point(13, 8);
			this._groupNewton.Name = "_groupNewton";
			this._groupNewton.Size = new System.Drawing.Size(310, 127);
			this._groupNewton.TabIndex = 0;
			this._groupNewton.TabStop = false;
			this._groupNewton.Text = "ニュートン法";
			// 
			// _numTryMaxCountNewton
			// 
			this._numTryMaxCountNewton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._numTryMaxCountNewton.BackBarColor = System.Drawing.Color.Lavender;
			this._numTryMaxCountNewton.ClickChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numTryMaxCountNewton.Cursor = System.Windows.Forms.Cursors.SizeWE;
			this._numTryMaxCountNewton.EnableUpDown = false;
			this._numTryMaxCountNewton.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this._numTryMaxCountNewton.Location = new System.Drawing.Point(157, 85);
			this._numTryMaxCountNewton.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this._numTryMaxCountNewton.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this._numTryMaxCountNewton.Name = "_numTryMaxCountNewton";
			this._numTryMaxCountNewton.Precision = 0;
			this._numTryMaxCountNewton.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this._numTryMaxCountNewton.ShowButton = true;
			this._numTryMaxCountNewton.Size = new System.Drawing.Size(114, 24);
			this._numTryMaxCountNewton.SlideChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numTryMaxCountNewton.TabIndex = 5;
			this._numTryMaxCountNewton.UnderBar = false;
			this._numTryMaxCountNewton.Unit = "";
			this._numTryMaxCountNewton.UseToolTip = true;
			this._numTryMaxCountNewton.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this._numTryMaxCountNewton.VisibleBackBar = true;
			// 
			// _numErrorToleranceNewton
			// 
			this._numErrorToleranceNewton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._numErrorToleranceNewton.BackBarColor = System.Drawing.Color.Lavender;
			this._numErrorToleranceNewton.ClickChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numErrorToleranceNewton.Cursor = System.Windows.Forms.Cursors.SizeWE;
			this._numErrorToleranceNewton.EnableUpDown = false;
			this._numErrorToleranceNewton.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this._numErrorToleranceNewton.Location = new System.Drawing.Point(157, 53);
			this._numErrorToleranceNewton.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numErrorToleranceNewton.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this._numErrorToleranceNewton.Name = "_numErrorToleranceNewton";
			this._numErrorToleranceNewton.Precision = 10;
			this._numErrorToleranceNewton.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this._numErrorToleranceNewton.ShowButton = true;
			this._numErrorToleranceNewton.Size = new System.Drawing.Size(114, 24);
			this._numErrorToleranceNewton.SlideChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numErrorToleranceNewton.TabIndex = 4;
			this._numErrorToleranceNewton.UnderBar = false;
			this._numErrorToleranceNewton.Unit = "";
			this._numErrorToleranceNewton.UseToolTip = true;
			this._numErrorToleranceNewton.Value = new decimal(new int[] {
            1,
            0,
            0,
            393216});
			this._numErrorToleranceNewton.VisibleBackBar = false;
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(40, 92);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(77, 12);
			this.label3.TabIndex = 3;
			this.label3.Text = "最大試行回数";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(40, 60);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "許容誤差";
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(40, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "初期解";
			// 
			// _numInitialSolutionNewton
			// 
			this._numInitialSolutionNewton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._numInitialSolutionNewton.BackBarColor = System.Drawing.Color.Lavender;
			this._numInitialSolutionNewton.ClickChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numInitialSolutionNewton.Cursor = System.Windows.Forms.Cursors.SizeWE;
			this._numInitialSolutionNewton.EnableUpDown = false;
			this._numInitialSolutionNewton.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this._numInitialSolutionNewton.Location = new System.Drawing.Point(157, 21);
			this._numInitialSolutionNewton.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
			this._numInitialSolutionNewton.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
			this._numInitialSolutionNewton.Name = "_numInitialSolutionNewton";
			this._numInitialSolutionNewton.Precision = 4;
			this._numInitialSolutionNewton.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this._numInitialSolutionNewton.ShowButton = true;
			this._numInitialSolutionNewton.Size = new System.Drawing.Size(114, 24);
			this._numInitialSolutionNewton.SlideChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numInitialSolutionNewton.TabIndex = 0;
			this._numInitialSolutionNewton.UnderBar = false;
			this._numInitialSolutionNewton.Unit = "";
			this._numInitialSolutionNewton.UseToolTip = true;
			this._numInitialSolutionNewton.Value = new decimal(new int[] {
            0,
            0,
            0,
            196608});
			this._numInitialSolutionNewton.VisibleBackBar = false;
			// 
			// _groupBrent
			// 
			this._groupBrent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._groupBrent.Controls.Add(this._numLowerLimitBrent);
			this._groupBrent.Controls.Add(this._numTryMaxCountBrent);
			this._groupBrent.Controls.Add(this._numErrorToleranceBrent);
			this._groupBrent.Controls.Add(this._numUpperLimitBrent);
			this._groupBrent.Controls.Add(this.label6);
			this._groupBrent.Controls.Add(this.label7);
			this._groupBrent.Controls.Add(this.label5);
			this._groupBrent.Controls.Add(this.label4);
			this._groupBrent.Location = new System.Drawing.Point(13, 141);
			this._groupBrent.Name = "_groupBrent";
			this._groupBrent.Size = new System.Drawing.Size(310, 156);
			this._groupBrent.TabIndex = 1;
			this._groupBrent.TabStop = false;
			this._groupBrent.Text = "ブレント法";
			// 
			// _numLowerLimitBrent
			// 
			this._numLowerLimitBrent.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._numLowerLimitBrent.BackBarColor = System.Drawing.Color.Lavender;
			this._numLowerLimitBrent.ClickChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numLowerLimitBrent.Cursor = System.Windows.Forms.Cursors.SizeWE;
			this._numLowerLimitBrent.EnableUpDown = false;
			this._numLowerLimitBrent.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this._numLowerLimitBrent.Location = new System.Drawing.Point(157, 22);
			this._numLowerLimitBrent.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
			this._numLowerLimitBrent.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
			this._numLowerLimitBrent.Name = "_numLowerLimitBrent";
			this._numLowerLimitBrent.Precision = 4;
			this._numLowerLimitBrent.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this._numLowerLimitBrent.ShowButton = true;
			this._numLowerLimitBrent.Size = new System.Drawing.Size(114, 24);
			this._numLowerLimitBrent.SlideChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numLowerLimitBrent.TabIndex = 9;
			this._numLowerLimitBrent.UnderBar = false;
			this._numLowerLimitBrent.Unit = "";
			this._numLowerLimitBrent.UseToolTip = true;
			this._numLowerLimitBrent.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147287040});
			this._numLowerLimitBrent.VisibleBackBar = false;
			// 
			// _numTryMaxCountBrent
			// 
			this._numTryMaxCountBrent.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._numTryMaxCountBrent.BackBarColor = System.Drawing.Color.Lavender;
			this._numTryMaxCountBrent.ClickChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numTryMaxCountBrent.Cursor = System.Windows.Forms.Cursors.SizeWE;
			this._numTryMaxCountBrent.EnableUpDown = false;
			this._numTryMaxCountBrent.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this._numTryMaxCountBrent.Location = new System.Drawing.Point(157, 121);
			this._numTryMaxCountBrent.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this._numTryMaxCountBrent.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this._numTryMaxCountBrent.Name = "_numTryMaxCountBrent";
			this._numTryMaxCountBrent.Precision = 0;
			this._numTryMaxCountBrent.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this._numTryMaxCountBrent.ShowButton = true;
			this._numTryMaxCountBrent.Size = new System.Drawing.Size(114, 24);
			this._numTryMaxCountBrent.SlideChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numTryMaxCountBrent.TabIndex = 8;
			this._numTryMaxCountBrent.UnderBar = false;
			this._numTryMaxCountBrent.Unit = "";
			this._numTryMaxCountBrent.UseToolTip = true;
			this._numTryMaxCountBrent.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this._numTryMaxCountBrent.VisibleBackBar = true;
			// 
			// _numErrorToleranceBrent
			// 
			this._numErrorToleranceBrent.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._numErrorToleranceBrent.BackBarColor = System.Drawing.Color.Lavender;
			this._numErrorToleranceBrent.ClickChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numErrorToleranceBrent.Cursor = System.Windows.Forms.Cursors.SizeWE;
			this._numErrorToleranceBrent.EnableUpDown = false;
			this._numErrorToleranceBrent.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this._numErrorToleranceBrent.Location = new System.Drawing.Point(157, 88);
			this._numErrorToleranceBrent.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numErrorToleranceBrent.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this._numErrorToleranceBrent.Name = "_numErrorToleranceBrent";
			this._numErrorToleranceBrent.Precision = 10;
			this._numErrorToleranceBrent.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this._numErrorToleranceBrent.ShowButton = true;
			this._numErrorToleranceBrent.Size = new System.Drawing.Size(114, 24);
			this._numErrorToleranceBrent.SlideChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numErrorToleranceBrent.TabIndex = 7;
			this._numErrorToleranceBrent.UnderBar = false;
			this._numErrorToleranceBrent.Unit = "";
			this._numErrorToleranceBrent.UseToolTip = true;
			this._numErrorToleranceBrent.Value = new decimal(new int[] {
            1,
            0,
            0,
            393216});
			this._numErrorToleranceBrent.VisibleBackBar = false;
			// 
			// _numUpperLimitBrent
			// 
			this._numUpperLimitBrent.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._numUpperLimitBrent.BackBarColor = System.Drawing.Color.Lavender;
			this._numUpperLimitBrent.ClickChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numUpperLimitBrent.Cursor = System.Windows.Forms.Cursors.SizeWE;
			this._numUpperLimitBrent.EnableUpDown = false;
			this._numUpperLimitBrent.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this._numUpperLimitBrent.Location = new System.Drawing.Point(157, 55);
			this._numUpperLimitBrent.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
			this._numUpperLimitBrent.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
			this._numUpperLimitBrent.Name = "_numUpperLimitBrent";
			this._numUpperLimitBrent.Precision = 4;
			this._numUpperLimitBrent.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this._numUpperLimitBrent.ShowButton = true;
			this._numUpperLimitBrent.Size = new System.Drawing.Size(114, 24);
			this._numUpperLimitBrent.SlideChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this._numUpperLimitBrent.TabIndex = 6;
			this._numUpperLimitBrent.UnderBar = false;
			this._numUpperLimitBrent.Unit = "";
			this._numUpperLimitBrent.UseToolTip = true;
			this._numUpperLimitBrent.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            196608});
			this._numUpperLimitBrent.VisibleBackBar = false;
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(42, 126);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(77, 12);
			this.label6.TabIndex = 5;
			this.label6.Text = "最大試行回数";
			// 
			// label7
			// 
			this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(42, 94);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(53, 12);
			this.label7.TabIndex = 4;
			this.label7.Text = "許容誤差";
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(42, 62);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(87, 12);
			this.label5.TabIndex = 3;
			this.label5.Text = "解の存在上限値";
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(42, 30);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(87, 12);
			this.label4.TabIndex = 2;
			this.label4.Text = "解の存在下限値";
			// 
			// _checkDontCopyVariable
			// 
			this._checkDontCopyVariable.AutoSize = true;
			this._checkDontCopyVariable.Location = new System.Drawing.Point(14, 309);
			this._checkDontCopyVariable.Name = "_checkDontCopyVariable";
			this._checkDontCopyVariable.Size = new System.Drawing.Size(257, 16);
			this._checkDontCopyVariable.TabIndex = 2;
			this._checkDontCopyVariable.Text = "結果コピー時、“【変数】 = ”の部分はコピーしない";
			this._checkDontCopyVariable.UseVisualStyleBackColor = true;
			// 
			// SolveEquationSettingPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._checkDontCopyVariable);
			this.Controls.Add(this._groupBrent);
			this.Controls.Add(this._groupNewton);
			this.Name = "SolveEquationSettingPanel";
			this.Size = new System.Drawing.Size(339, 340);
			this._groupNewton.ResumeLayout(false);
			this._groupNewton.PerformLayout();
			this._groupBrent.ResumeLayout(false);
			this._groupBrent.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox _groupNewton;
		private System.Windows.Forms.Label label1;
		private Controls.NumericSlider _numInitialSolutionNewton;
		private System.Windows.Forms.GroupBox _groupBrent;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private Controls.NumericSlider _numTryMaxCountNewton;
		private Controls.NumericSlider _numErrorToleranceNewton;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private Controls.NumericSlider _numTryMaxCountBrent;
		private Controls.NumericSlider _numErrorToleranceBrent;
		private Controls.NumericSlider _numUpperLimitBrent;
		private Controls.NumericSlider _numLowerLimitBrent;
		private System.Windows.Forms.CheckBox _checkDontCopyVariable;
	}
}
