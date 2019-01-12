namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    partial class CalculateSettingPanel
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
            this._groupSolveEquation = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._groupBrent = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this._numLowerLimitBrent = new GoodSeat.Clapte.Views.Controls.NumericSlider();
            this._numTryMaxCountBrent = new GoodSeat.Clapte.Views.Controls.NumericSlider();
            this._numErrorToleranceBrent = new GoodSeat.Clapte.Views.Controls.NumericSlider();
            this._numUpperLimitBrent = new GoodSeat.Clapte.Views.Controls.NumericSlider();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._groupNewton = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this._numTryMaxCountNewton = new GoodSeat.Clapte.Views.Controls.NumericSlider();
            this._numErrorToleranceNewton = new GoodSeat.Clapte.Views.Controls.NumericSlider();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._numInitialSolutionNewton = new GoodSeat.Clapte.Views.Controls.NumericSlider();
            this._checkDontCopyVariable = new System.Windows.Forms.CheckBox();
            this._groupSolver = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this._numLimitTime = new GoodSeat.Clapte.Views.Controls.NumericSlider();
            this._cmbCalculateMode = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this._cmbRounding = new System.Windows.Forms.ComboBox();
            this._cmbValidPrecision = new System.Windows.Forms.ComboBox();
            this._labelRounding = new System.Windows.Forms.Label();
            this._labelPrecision = new System.Windows.Forms.Label();
            this._groupPresicion = new System.Windows.Forms.GroupBox();
            this._labelPrecisionDigit = new System.Windows.Forms.Label();
            this._numPrecisionDigit = new GoodSeat.Clapte.Views.Controls.NumericSlider();
            this._radioPrecisionCustom = new System.Windows.Forms.RadioButton();
            this._radioPrecisionDouble = new System.Windows.Forms.RadioButton();
            this._groupSolveEquation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this._groupBrent.SuspendLayout();
            this._groupNewton.SuspendLayout();
            this._groupSolver.SuspendLayout();
            this._groupPresicion.SuspendLayout();
            this.SuspendLayout();
            // 
            // _groupSolveEquation
            // 
            this._groupSolveEquation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupSolveEquation.Controls.Add(this.splitContainer1);
            this._groupSolveEquation.Controls.Add(this._checkDontCopyVariable);
            this._groupSolveEquation.Location = new System.Drawing.Point(6, 166);
            this._groupSolveEquation.Name = "_groupSolveEquation";
            this._groupSolveEquation.Size = new System.Drawing.Size(397, 172);
            this._groupSolveEquation.TabIndex = 12;
            this._groupSolveEquation.TabStop = false;
            this._groupSolveEquation.Text = "方程式の求解";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(10, 20);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._groupBrent);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._groupNewton);
            this.splitContainer1.Size = new System.Drawing.Size(375, 141);
            this.splitContainer1.SplitterDistance = 186;
            this.splitContainer1.TabIndex = 3;
            // 
            // _groupBrent
            // 
            this._groupBrent.Controls.Add(this.label11);
            this._groupBrent.Controls.Add(this._numLowerLimitBrent);
            this._groupBrent.Controls.Add(this._numTryMaxCountBrent);
            this._groupBrent.Controls.Add(this._numErrorToleranceBrent);
            this._groupBrent.Controls.Add(this._numUpperLimitBrent);
            this._groupBrent.Controls.Add(this.label6);
            this._groupBrent.Controls.Add(this.label7);
            this._groupBrent.Controls.Add(this.label5);
            this._groupBrent.Controls.Add(this.label4);
            this._groupBrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this._groupBrent.Location = new System.Drawing.Point(0, 0);
            this._groupBrent.Name = "_groupBrent";
            this._groupBrent.Size = new System.Drawing.Size(186, 141);
            this._groupBrent.TabIndex = 1;
            this._groupBrent.TabStop = false;
            this._groupBrent.Text = "ブレント法";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(114, 82);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(26, 12);
            this.label11.TabIndex = 11;
            this.label11.Text = "1.0E";
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
            this._numLowerLimitBrent.Location = new System.Drawing.Point(113, 22);
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
            this._numLowerLimitBrent.Precision = 1;
            this._numLowerLimitBrent.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this._numLowerLimitBrent.ShowButton = true;
            this._numLowerLimitBrent.Size = new System.Drawing.Size(60, 20);
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
            10000,
            0,
            0,
            -2147418112});
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
            this._numTryMaxCountBrent.Location = new System.Drawing.Point(113, 106);
            this._numTryMaxCountBrent.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this._numTryMaxCountBrent.Minimum = new decimal(new int[] {
            1,
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
            this._numTryMaxCountBrent.Size = new System.Drawing.Size(60, 20);
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
            this._numErrorToleranceBrent.Location = new System.Drawing.Point(140, 78);
            this._numErrorToleranceBrent.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this._numErrorToleranceBrent.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this._numErrorToleranceBrent.Name = "_numErrorToleranceBrent";
            this._numErrorToleranceBrent.Precision = 0;
            this._numErrorToleranceBrent.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this._numErrorToleranceBrent.ShowButton = true;
            this._numErrorToleranceBrent.Size = new System.Drawing.Size(33, 20);
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
            7,
            0,
            0,
            -2147483648});
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
            this._numUpperLimitBrent.Location = new System.Drawing.Point(113, 50);
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
            this._numUpperLimitBrent.Precision = 1;
            this._numUpperLimitBrent.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this._numUpperLimitBrent.ShowButton = true;
            this._numUpperLimitBrent.Size = new System.Drawing.Size(60, 20);
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
            10000,
            0,
            0,
            65536});
            this._numUpperLimitBrent.VisibleBackBar = false;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "最大試行回数";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(48, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "許容誤差";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "解の存在上限値";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "解の存在下限値";
            // 
            // _groupNewton
            // 
            this._groupNewton.Controls.Add(this.label12);
            this._groupNewton.Controls.Add(this._numTryMaxCountNewton);
            this._groupNewton.Controls.Add(this._numErrorToleranceNewton);
            this._groupNewton.Controls.Add(this.label3);
            this._groupNewton.Controls.Add(this.label2);
            this._groupNewton.Controls.Add(this.label1);
            this._groupNewton.Controls.Add(this._numInitialSolutionNewton);
            this._groupNewton.Dock = System.Windows.Forms.DockStyle.Fill;
            this._groupNewton.Location = new System.Drawing.Point(0, 0);
            this._groupNewton.Name = "_groupNewton";
            this._groupNewton.Size = new System.Drawing.Size(185, 141);
            this._groupNewton.TabIndex = 0;
            this._groupNewton.TabStop = false;
            this._groupNewton.Text = "ニュートン法";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(107, 54);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 12);
            this.label12.TabIndex = 11;
            this.label12.Text = "1.0E";
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
            this._numTryMaxCountNewton.Location = new System.Drawing.Point(107, 78);
            this._numTryMaxCountNewton.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this._numTryMaxCountNewton.Minimum = new decimal(new int[] {
            1,
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
            this._numTryMaxCountNewton.Size = new System.Drawing.Size(60, 20);
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
            this._numErrorToleranceNewton.Location = new System.Drawing.Point(135, 50);
            this._numErrorToleranceNewton.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this._numErrorToleranceNewton.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this._numErrorToleranceNewton.Name = "_numErrorToleranceNewton";
            this._numErrorToleranceNewton.Precision = 0;
            this._numErrorToleranceNewton.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this._numErrorToleranceNewton.ShowButton = true;
            this._numErrorToleranceNewton.Size = new System.Drawing.Size(33, 20);
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
            7,
            0,
            0,
            -2147483648});
            this._numErrorToleranceNewton.VisibleBackBar = false;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "最大試行回数";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "許容誤差";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 28);
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
            this._numInitialSolutionNewton.Location = new System.Drawing.Point(108, 22);
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
            this._numInitialSolutionNewton.Size = new System.Drawing.Size(60, 20);
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
            // _checkDontCopyVariable
            // 
            this._checkDontCopyVariable.AutoSize = true;
            this._checkDontCopyVariable.Location = new System.Drawing.Point(10, 164);
            this._checkDontCopyVariable.Name = "_checkDontCopyVariable";
            this._checkDontCopyVariable.Size = new System.Drawing.Size(257, 16);
            this._checkDontCopyVariable.TabIndex = 2;
            this._checkDontCopyVariable.Text = "結果コピー時、“【変数】 = ”の部分はコピーしない";
            this._checkDontCopyVariable.UseVisualStyleBackColor = true;
            this._checkDontCopyVariable.Visible = false;
            // 
            // _groupSolver
            // 
            this._groupSolver.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupSolver.Controls.Add(this.label9);
            this._groupSolver.Controls.Add(this._numLimitTime);
            this._groupSolver.Controls.Add(this._cmbCalculateMode);
            this._groupSolver.Controls.Add(this.label10);
            this._groupSolver.Controls.Add(this._cmbRounding);
            this._groupSolver.Controls.Add(this._cmbValidPrecision);
            this._groupSolver.Controls.Add(this._labelRounding);
            this._groupSolver.Controls.Add(this._labelPrecision);
            this._groupSolver.Location = new System.Drawing.Point(7, 67);
            this._groupSolver.Name = "_groupSolver";
            this._groupSolver.Size = new System.Drawing.Size(396, 87);
            this._groupSolver.TabIndex = 11;
            this._groupSolver.TabStop = false;
            this._groupSolver.Text = "計算";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(187, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 55;
            this.label9.Text = "最大計算時間";
            // 
            // _numLimitTime
            // 
            this._numLimitTime.BackBarColor = System.Drawing.Color.Lavender;
            this._numLimitTime.ClickChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numLimitTime.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this._numLimitTime.EnableUpDown = false;
            this._numLimitTime.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._numLimitTime.Location = new System.Drawing.Point(268, 52);
            this._numLimitTime.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this._numLimitTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numLimitTime.Name = "_numLimitTime";
            this._numLimitTime.Precision = 4;
            this._numLimitTime.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this._numLimitTime.ShowButton = true;
            this._numLimitTime.Size = new System.Drawing.Size(116, 22);
            this._numLimitTime.SlideChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numLimitTime.TabIndex = 54;
            this._numLimitTime.UnderBar = false;
            this._numLimitTime.Unit = "sec";
            this._numLimitTime.UseToolTip = true;
            this._numLimitTime.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._numLimitTime.VisibleBackBar = true;
            // 
            // _cmbCalculateMode
            // 
            this._cmbCalculateMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbCalculateMode.FormattingEnabled = true;
            this._cmbCalculateMode.Items.AddRange(new object[] {
            "小数計算",
            "分数計算"});
            this._cmbCalculateMode.Location = new System.Drawing.Point(75, 53);
            this._cmbCalculateMode.Name = "_cmbCalculateMode";
            this._cmbCalculateMode.Size = new System.Drawing.Size(99, 20);
            this._cmbCalculateMode.TabIndex = 52;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 57);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 12);
            this.label10.TabIndex = 53;
            this.label10.Text = "計算モード";
            // 
            // _cmbRounding
            // 
            this._cmbRounding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbRounding.FormattingEnabled = true;
            this._cmbRounding.Items.AddRange(new object[] {
            "四捨五入",
            "最近接偶数への丸め"});
            this._cmbRounding.Location = new System.Drawing.Point(268, 21);
            this._cmbRounding.Name = "_cmbRounding";
            this._cmbRounding.Size = new System.Drawing.Size(116, 20);
            this._cmbRounding.TabIndex = 51;
            // 
            // _cmbValidPrecision
            // 
            this._cmbValidPrecision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbValidPrecision.FormattingEnabled = true;
            this._cmbValidPrecision.Items.AddRange(new object[] {
            "考慮する",
            "考慮しない"});
            this._cmbValidPrecision.Location = new System.Drawing.Point(75, 21);
            this._cmbValidPrecision.Name = "_cmbValidPrecision";
            this._cmbValidPrecision.Size = new System.Drawing.Size(99, 20);
            this._cmbValidPrecision.TabIndex = 6;
            // 
            // _labelRounding
            // 
            this._labelRounding.AutoSize = true;
            this._labelRounding.Location = new System.Drawing.Point(187, 25);
            this._labelRounding.Name = "_labelRounding";
            this._labelRounding.Size = new System.Drawing.Size(75, 12);
            this._labelRounding.TabIndex = 50;
            this._labelRounding.Text = "端数丸め方法";
            // 
            // _labelPrecision
            // 
            this._labelPrecision.AutoSize = true;
            this._labelPrecision.Location = new System.Drawing.Point(16, 25);
            this._labelPrecision.Name = "_labelPrecision";
            this._labelPrecision.Size = new System.Drawing.Size(53, 12);
            this._labelPrecision.TabIndex = 7;
            this._labelPrecision.Text = "有効数字";
            // 
            // _groupPresicion
            // 
            this._groupPresicion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupPresicion.Controls.Add(this._labelPrecisionDigit);
            this._groupPresicion.Controls.Add(this._numPrecisionDigit);
            this._groupPresicion.Controls.Add(this._radioPrecisionCustom);
            this._groupPresicion.Controls.Add(this._radioPrecisionDouble);
            this._groupPresicion.Location = new System.Drawing.Point(6, 10);
            this._groupPresicion.Name = "_groupPresicion";
            this._groupPresicion.Size = new System.Drawing.Size(397, 49);
            this._groupPresicion.TabIndex = 3;
            this._groupPresicion.TabStop = false;
            this._groupPresicion.Text = "数値精度(有効桁数)";
            // 
            // _labelPrecisionDigit
            // 
            this._labelPrecisionDigit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this._labelPrecisionDigit.AutoSize = true;
            this._labelPrecisionDigit.Enabled = false;
            this._labelPrecisionDigit.Location = new System.Drawing.Point(223, 22);
            this._labelPrecisionDigit.Name = "_labelPrecisionDigit";
            this._labelPrecisionDigit.Size = new System.Drawing.Size(53, 12);
            this._labelPrecisionDigit.TabIndex = 10;
            this._labelPrecisionDigit.Text = "有効桁数";
            // 
            // _numPrecisionDigit
            // 
            this._numPrecisionDigit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this._numPrecisionDigit.BackBarColor = System.Drawing.Color.Lavender;
            this._numPrecisionDigit.ClickChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numPrecisionDigit.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this._numPrecisionDigit.Enabled = false;
            this._numPrecisionDigit.EnableUpDown = false;
            this._numPrecisionDigit.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._numPrecisionDigit.Location = new System.Drawing.Point(282, 18);
            this._numPrecisionDigit.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this._numPrecisionDigit.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this._numPrecisionDigit.Name = "_numPrecisionDigit";
            this._numPrecisionDigit.Precision = 0;
            this._numPrecisionDigit.Sensitivity = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this._numPrecisionDigit.ShowButton = true;
            this._numPrecisionDigit.Size = new System.Drawing.Size(44, 20);
            this._numPrecisionDigit.SlideChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._numPrecisionDigit.TabIndex = 9;
            this._numPrecisionDigit.UnderBar = false;
            this._numPrecisionDigit.Unit = "";
            this._numPrecisionDigit.UseToolTip = true;
            this._numPrecisionDigit.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this._numPrecisionDigit.VisibleBackBar = true;
            // 
            // _radioPrecisionCustom
            // 
            this._radioPrecisionCustom.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this._radioPrecisionCustom.AutoSize = true;
            this._radioPrecisionCustom.Location = new System.Drawing.Point(158, 20);
            this._radioPrecisionCustom.Name = "_radioPrecisionCustom";
            this._radioPrecisionCustom.Size = new System.Drawing.Size(59, 16);
            this._radioPrecisionCustom.TabIndex = 2;
            this._radioPrecisionCustom.TabStop = true;
            this._radioPrecisionCustom.Text = "高精度";
            this._radioPrecisionCustom.UseVisualStyleBackColor = true;
            this._radioPrecisionCustom.CheckedChanged += new System.EventHandler(this._radioPrecisionCustom_CheckedChanged);
            // 
            // _radioPrecisionDouble
            // 
            this._radioPrecisionDouble.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this._radioPrecisionDouble.AutoSize = true;
            this._radioPrecisionDouble.Location = new System.Drawing.Point(59, 20);
            this._radioPrecisionDouble.Name = "_radioPrecisionDouble";
            this._radioPrecisionDouble.Size = new System.Drawing.Size(47, 16);
            this._radioPrecisionDouble.TabIndex = 0;
            this._radioPrecisionDouble.TabStop = true;
            this._radioPrecisionDouble.Text = "通常";
            this._radioPrecisionDouble.UseVisualStyleBackColor = true;
            // 
            // CalculateSettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._groupSolveEquation);
            this.Controls.Add(this._groupSolver);
            this.Controls.Add(this._groupPresicion);
            this.Name = "CalculateSettingPanel";
            this.Size = new System.Drawing.Size(409, 379);
            this._groupSolveEquation.ResumeLayout(false);
            this._groupSolveEquation.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this._groupBrent.ResumeLayout(false);
            this._groupBrent.PerformLayout();
            this._groupNewton.ResumeLayout(false);
            this._groupNewton.PerformLayout();
            this._groupSolver.ResumeLayout(false);
            this._groupSolver.PerformLayout();
            this._groupPresicion.ResumeLayout(false);
            this._groupPresicion.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.GroupBox _groupPresicion;
        private System.Windows.Forms.RadioButton _radioPrecisionCustom;
        private System.Windows.Forms.RadioButton _radioPrecisionDouble;
        private System.Windows.Forms.Label _labelPrecisionDigit;
        private Controls.NumericSlider _numPrecisionDigit;
        private System.Windows.Forms.GroupBox _groupSolver;
        private System.Windows.Forms.Label label9;
        private Controls.NumericSlider _numLimitTime;
        private System.Windows.Forms.ComboBox _cmbCalculateMode;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox _cmbRounding;
        private System.Windows.Forms.ComboBox _cmbValidPrecision;
        private System.Windows.Forms.Label _labelRounding;
        private System.Windows.Forms.Label _labelPrecision;
        private System.Windows.Forms.GroupBox _groupSolveEquation;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
    }
}
