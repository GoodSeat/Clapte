namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    partial class ExcludeListSettingPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this._panelExcludeSetting = new System.Windows.Forms.Panel();
            this._btnPickApplication = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._labelErrorMsg = new System.Windows.Forms.Label();
            this._checkValidClassName = new System.Windows.Forms.CheckBox();
            this._checkValidTitle = new System.Windows.Forms.CheckBox();
            this._checkValidProductName = new System.Windows.Forms.CheckBox();
            this._checkValidFileName = new System.Windows.Forms.CheckBox();
            this._checkRegexClassName = new System.Windows.Forms.CheckBox();
            this._checkRegexTitle = new System.Windows.Forms.CheckBox();
            this._checkRegexProductName = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this._textBoxClassName = new System.Windows.Forms.TextBox();
            this._textBoxTitle = new System.Windows.Forms.TextBox();
            this._textBoxProductName = new System.Windows.Forms.TextBox();
            this._checkRegexFileName = new System.Windows.Forms.CheckBox();
            this._textBoxFileName = new System.Windows.Forms.TextBox();
            this._btnOK = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._btnCancel = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._btnChangeFilter = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._btnDeleteFilter = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._btnAddFilter = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._listView = new System.Windows.Forms.ListView();
            this._columnFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._columnProduct = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._columnTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._columnClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._timer = new System.Windows.Forms.Timer(this.components);
            this._labelPick = new System.Windows.Forms.Label();
            this.toolTipHelp = new System.Windows.Forms.ToolTip(this.components);
            this._cmbMatchFileName = new System.Windows.Forms.ComboBox();
            this._cmbMatchProductName = new System.Windows.Forms.ComboBox();
            this._cmbMatchTitle = new System.Windows.Forms.ComboBox();
            this._cmbMatchClassName = new System.Windows.Forms.ComboBox();
            this._panelExcludeSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._btnPickApplication)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnChangeFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnDeleteFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnAddFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "監視対象外リスト";
            // 
            // _panelExcludeSetting
            // 
            this._panelExcludeSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._panelExcludeSetting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._panelExcludeSetting.Controls.Add(this._cmbMatchClassName);
            this._panelExcludeSetting.Controls.Add(this._cmbMatchTitle);
            this._panelExcludeSetting.Controls.Add(this._cmbMatchProductName);
            this._panelExcludeSetting.Controls.Add(this._cmbMatchFileName);
            this._panelExcludeSetting.Controls.Add(this._labelPick);
            this._panelExcludeSetting.Controls.Add(this._btnPickApplication);
            this._panelExcludeSetting.Controls.Add(this._labelErrorMsg);
            this._panelExcludeSetting.Controls.Add(this._checkValidClassName);
            this._panelExcludeSetting.Controls.Add(this._checkValidTitle);
            this._panelExcludeSetting.Controls.Add(this._checkValidProductName);
            this._panelExcludeSetting.Controls.Add(this._checkValidFileName);
            this._panelExcludeSetting.Controls.Add(this._checkRegexClassName);
            this._panelExcludeSetting.Controls.Add(this._checkRegexTitle);
            this._panelExcludeSetting.Controls.Add(this._checkRegexProductName);
            this._panelExcludeSetting.Controls.Add(this.label2);
            this._panelExcludeSetting.Controls.Add(this._textBoxClassName);
            this._panelExcludeSetting.Controls.Add(this._textBoxTitle);
            this._panelExcludeSetting.Controls.Add(this._textBoxProductName);
            this._panelExcludeSetting.Controls.Add(this._checkRegexFileName);
            this._panelExcludeSetting.Controls.Add(this._textBoxFileName);
            this._panelExcludeSetting.Controls.Add(this._btnOK);
            this._panelExcludeSetting.Controls.Add(this._btnCancel);
            this._panelExcludeSetting.Location = new System.Drawing.Point(8, 27);
            this._panelExcludeSetting.Name = "_panelExcludeSetting";
            this._panelExcludeSetting.Size = new System.Drawing.Size(270, 172);
            this._panelExcludeSetting.TabIndex = 22;
            this._panelExcludeSetting.Visible = false;
            this._panelExcludeSetting.VisibleChanged += new System.EventHandler(this._panelExcludeSetting_VisibleChanged);
            // 
            // _btnPickApplication
            // 
            this._btnPickApplication.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnPickApplication.DownMove = 1;
            this._btnPickApplication.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_PickApplication;
            this._btnPickApplication.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_PickApplication_Unfocus;
            this._btnPickApplication.Location = new System.Drawing.Point(4, 0);
            this._btnPickApplication.Name = "_btnPickApplication";
            this._btnPickApplication.Size = new System.Drawing.Size(26, 26);
            this._btnPickApplication.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._btnPickApplication.TabIndex = 41;
            this._btnPickApplication.TabStop = false;
            this.toolTipHelp.SetToolTip(this._btnPickApplication, "クリックで対象を指定");
            this._btnPickApplication.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_PickApplication_Unfocus;
            this._btnPickApplication.Click += new System.EventHandler(this._btnPickApplication_Click);
            // 
            // _labelErrorMsg
            // 
            this._labelErrorMsg.AutoSize = true;
            this._labelErrorMsg.ForeColor = System.Drawing.Color.Red;
            this._labelErrorMsg.Location = new System.Drawing.Point(9, 129);
            this._labelErrorMsg.Name = "_labelErrorMsg";
            this._labelErrorMsg.Size = new System.Drawing.Size(0, 12);
            this._labelErrorMsg.TabIndex = 40;
            // 
            // _checkValidClassName
            // 
            this._checkValidClassName.AutoSize = true;
            this._checkValidClassName.Location = new System.Drawing.Point(7, 105);
            this._checkValidClassName.Name = "_checkValidClassName";
            this._checkValidClassName.Size = new System.Drawing.Size(61, 16);
            this._checkValidClassName.TabIndex = 39;
            this._checkValidClassName.Text = "クラス名";
            this.toolTipHelp.SetToolTip(this._checkValidClassName, "クラス名を対象とする");
            this._checkValidClassName.UseVisualStyleBackColor = true;
            // 
            // _checkValidTitle
            // 
            this._checkValidTitle.AutoSize = true;
            this._checkValidTitle.Location = new System.Drawing.Point(7, 80);
            this._checkValidTitle.Name = "_checkValidTitle";
            this._checkValidTitle.Size = new System.Drawing.Size(59, 16);
            this._checkValidTitle.TabIndex = 38;
            this._checkValidTitle.Text = "タイトル";
            this.toolTipHelp.SetToolTip(this._checkValidTitle, "タイトルを対象とする");
            this._checkValidTitle.UseVisualStyleBackColor = true;
            // 
            // _checkValidProductName
            // 
            this._checkValidProductName.AutoSize = true;
            this._checkValidProductName.Location = new System.Drawing.Point(7, 55);
            this._checkValidProductName.Name = "_checkValidProductName";
            this._checkValidProductName.Size = new System.Drawing.Size(60, 16);
            this._checkValidProductName.TabIndex = 37;
            this._checkValidProductName.Text = "製品名";
            this.toolTipHelp.SetToolTip(this._checkValidProductName, "製品名を対象とする");
            this._checkValidProductName.UseVisualStyleBackColor = true;
            // 
            // _checkValidFileName
            // 
            this._checkValidFileName.AutoSize = true;
            this._checkValidFileName.Location = new System.Drawing.Point(7, 30);
            this._checkValidFileName.Name = "_checkValidFileName";
            this._checkValidFileName.Size = new System.Drawing.Size(70, 16);
            this._checkValidFileName.TabIndex = 36;
            this._checkValidFileName.Text = "ファイル名";
            this.toolTipHelp.SetToolTip(this._checkValidFileName, "ファイル名を対象とする");
            this._checkValidFileName.UseVisualStyleBackColor = true;
            // 
            // _checkRegexClassName
            // 
            this._checkRegexClassName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._checkRegexClassName.AutoSize = true;
            this._checkRegexClassName.Location = new System.Drawing.Point(232, 107);
            this._checkRegexClassName.Name = "_checkRegexClassName";
            this._checkRegexClassName.Size = new System.Drawing.Size(15, 14);
            this._checkRegexClassName.TabIndex = 35;
            this.toolTipHelp.SetToolTip(this._checkRegexClassName, "正規表現を指定");
            this._checkRegexClassName.UseVisualStyleBackColor = true;
            // 
            // _checkRegexTitle
            // 
            this._checkRegexTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._checkRegexTitle.AutoSize = true;
            this._checkRegexTitle.Location = new System.Drawing.Point(232, 81);
            this._checkRegexTitle.Name = "_checkRegexTitle";
            this._checkRegexTitle.Size = new System.Drawing.Size(15, 14);
            this._checkRegexTitle.TabIndex = 34;
            this.toolTipHelp.SetToolTip(this._checkRegexTitle, "正規表現を指定");
            this._checkRegexTitle.UseVisualStyleBackColor = true;
            // 
            // _checkRegexProductName
            // 
            this._checkRegexProductName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._checkRegexProductName.AutoSize = true;
            this._checkRegexProductName.Location = new System.Drawing.Point(232, 55);
            this._checkRegexProductName.Name = "_checkRegexProductName";
            this._checkRegexProductName.Size = new System.Drawing.Size(15, 14);
            this._checkRegexProductName.TabIndex = 33;
            this.toolTipHelp.SetToolTip(this._checkRegexProductName, "正規表現を指定");
            this._checkRegexProductName.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(212, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "正規表現";
            // 
            // _textBoxClassName
            // 
            this._textBoxClassName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxClassName.Location = new System.Drawing.Point(82, 103);
            this._textBoxClassName.Name = "_textBoxClassName";
            this._textBoxClassName.Size = new System.Drawing.Size(78, 19);
            this._textBoxClassName.TabIndex = 27;
            // 
            // _textBoxTitle
            // 
            this._textBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxTitle.Location = new System.Drawing.Point(82, 78);
            this._textBoxTitle.Name = "_textBoxTitle";
            this._textBoxTitle.Size = new System.Drawing.Size(78, 19);
            this._textBoxTitle.TabIndex = 26;
            // 
            // _textBoxProductName
            // 
            this._textBoxProductName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxProductName.Location = new System.Drawing.Point(82, 53);
            this._textBoxProductName.Name = "_textBoxProductName";
            this._textBoxProductName.Size = new System.Drawing.Size(78, 19);
            this._textBoxProductName.TabIndex = 25;
            // 
            // _checkRegexFileName
            // 
            this._checkRegexFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._checkRegexFileName.AutoSize = true;
            this._checkRegexFileName.Location = new System.Drawing.Point(232, 29);
            this._checkRegexFileName.Name = "_checkRegexFileName";
            this._checkRegexFileName.Size = new System.Drawing.Size(15, 14);
            this._checkRegexFileName.TabIndex = 24;
            this.toolTipHelp.SetToolTip(this._checkRegexFileName, "正規表現を指定");
            this._checkRegexFileName.UseVisualStyleBackColor = true;
            // 
            // _textBoxFileName
            // 
            this._textBoxFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxFileName.Location = new System.Drawing.Point(82, 26);
            this._textBoxFileName.Name = "_textBoxFileName";
            this._textBoxFileName.Size = new System.Drawing.Size(78, 19);
            this._textBoxFileName.TabIndex = 23;
            // 
            // _btnOK
            // 
            this._btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnOK.BackColor = System.Drawing.Color.White;
            this._btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnOK.DownMove = 1;
            this._btnOK.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_OK;
            this._btnOK.Image = global::GoodSeat.Clapte.Properties.Resources.Image_OK_Unfocus;
            this._btnOK.Location = new System.Drawing.Point(105, 143);
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size(79, 23);
            this._btnOK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._btnOK.TabIndex = 21;
            this._btnOK.TabStop = false;
            this._btnOK.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_OK_Unfocus;
            this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCancel.BackColor = System.Drawing.Color.White;
            this._btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnCancel.DownMove = 1;
            this._btnCancel.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_Cancel;
            this._btnCancel.Image = global::GoodSeat.Clapte.Properties.Resources.Image_Cancel_Unfocus;
            this._btnCancel.Location = new System.Drawing.Point(185, 143);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(79, 23);
            this._btnCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._btnCancel.TabIndex = 22;
            this._btnCancel.TabStop = false;
            this._btnCancel.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_Cancel_Unfocus;
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            // 
            // _btnChangeFilter
            // 
            this._btnChangeFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnChangeFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnChangeFilter.DownMove = 1;
            this._btnChangeFilter.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_ChangeFilter;
            this._btnChangeFilter.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_ChangeFilter_Unfocus;
            this._btnChangeFilter.Location = new System.Drawing.Point(226, 3);
            this._btnChangeFilter.Name = "_btnChangeFilter";
            this._btnChangeFilter.Size = new System.Drawing.Size(26, 26);
            this._btnChangeFilter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._btnChangeFilter.TabIndex = 21;
            this._btnChangeFilter.TabStop = false;
            this.toolTipHelp.SetToolTip(this._btnChangeFilter, "条件の編集");
            this._btnChangeFilter.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_ChangeFilter_Unfocus;
            this._btnChangeFilter.Click += new System.EventHandler(this._btnChangeFilter_Click);
            // 
            // _btnDeleteFilter
            // 
            this._btnDeleteFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnDeleteFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnDeleteFilter.DownMove = 1;
            this._btnDeleteFilter.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_DeleteFilter;
            this._btnDeleteFilter.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_DeleteFilter_Unfocues;
            this._btnDeleteFilter.Location = new System.Drawing.Point(251, 4);
            this._btnDeleteFilter.Name = "_btnDeleteFilter";
            this._btnDeleteFilter.Size = new System.Drawing.Size(25, 25);
            this._btnDeleteFilter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._btnDeleteFilter.TabIndex = 20;
            this._btnDeleteFilter.TabStop = false;
            this.toolTipHelp.SetToolTip(this._btnDeleteFilter, "条件の削除");
            this._btnDeleteFilter.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_DeleteFilter_Unfocues;
            this._btnDeleteFilter.Click += new System.EventHandler(this._btnDeleteFilter_Click);
            // 
            // _btnAddFilter
            // 
            this._btnAddFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnAddFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnAddFilter.DownMove = 1;
            this._btnAddFilter.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_AddFilter;
            this._btnAddFilter.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_AddFilter_Unfocus;
            this._btnAddFilter.Location = new System.Drawing.Point(201, 3);
            this._btnAddFilter.Name = "_btnAddFilter";
            this._btnAddFilter.Size = new System.Drawing.Size(26, 26);
            this._btnAddFilter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._btnAddFilter.TabIndex = 19;
            this._btnAddFilter.TabStop = false;
            this.toolTipHelp.SetToolTip(this._btnAddFilter, "条件の新規追加");
            this._btnAddFilter.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_AddFilter_Unfocus;
            this._btnAddFilter.Click += new System.EventHandler(this._btnAddFilter_Click);
            // 
            // _listView
            // 
            this._listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._columnFileName,
            this._columnProduct,
            this._columnTitle,
            this._columnClass});
            this._listView.FullRowSelect = true;
            this._listView.Location = new System.Drawing.Point(10, 31);
            this._listView.MultiSelect = false;
            this._listView.Name = "_listView";
            this._listView.Size = new System.Drawing.Size(265, 182);
            this._listView.TabIndex = 24;
            this._listView.UseCompatibleStateImageBehavior = false;
            this._listView.View = System.Windows.Forms.View.Details;
            // 
            // _columnFileName
            // 
            this._columnFileName.Text = "ファイル名";
            this._columnFileName.Width = 100;
            // 
            // _columnProduct
            // 
            this._columnProduct.Text = "製品名";
            this._columnProduct.Width = 100;
            // 
            // _columnTitle
            // 
            this._columnTitle.Text = "タイトル";
            this._columnTitle.Width = 100;
            // 
            // _columnClass
            // 
            this._columnClass.Text = "クラス名";
            this._columnClass.Width = 100;
            // 
            // _timer
            // 
            this._timer.Interval = 250;
            this._timer.Tick += new System.EventHandler(this._timer_Tick);
            // 
            // _labelPick
            // 
            this._labelPick.AutoSize = true;
            this._labelPick.ForeColor = System.Drawing.Color.Blue;
            this._labelPick.Location = new System.Drawing.Point(33, 7);
            this._labelPick.Name = "_labelPick";
            this._labelPick.Size = new System.Drawing.Size(137, 12);
            this._labelPick.TabIndex = 42;
            this._labelPick.Text = "対象アプリケーションをクリック";
            this._labelPick.Visible = false;
            // 
            // toolTipHelp
            // 
            this.toolTipHelp.BackColor = System.Drawing.Color.White;
            // 
            // _cmbMatchFileName
            // 
            this._cmbMatchFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbMatchFileName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbMatchFileName.FormattingEnabled = true;
            this._cmbMatchFileName.Items.AddRange(new object[] {
            "を含む",
            "と一致"});
            this._cmbMatchFileName.Location = new System.Drawing.Point(166, 25);
            this._cmbMatchFileName.Name = "_cmbMatchFileName";
            this._cmbMatchFileName.Size = new System.Drawing.Size(60, 20);
            this._cmbMatchFileName.TabIndex = 43;
            // 
            // _cmbMatchProductName
            // 
            this._cmbMatchProductName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbMatchProductName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbMatchProductName.FormattingEnabled = true;
            this._cmbMatchProductName.Items.AddRange(new object[] {
            "を含む",
            "と一致"});
            this._cmbMatchProductName.Location = new System.Drawing.Point(166, 53);
            this._cmbMatchProductName.Name = "_cmbMatchProductName";
            this._cmbMatchProductName.Size = new System.Drawing.Size(60, 20);
            this._cmbMatchProductName.TabIndex = 44;
            // 
            // _cmbMatchTitle
            // 
            this._cmbMatchTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbMatchTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbMatchTitle.FormattingEnabled = true;
            this._cmbMatchTitle.Items.AddRange(new object[] {
            "を含む",
            "と一致"});
            this._cmbMatchTitle.Location = new System.Drawing.Point(166, 78);
            this._cmbMatchTitle.Name = "_cmbMatchTitle";
            this._cmbMatchTitle.Size = new System.Drawing.Size(60, 20);
            this._cmbMatchTitle.TabIndex = 45;
            // 
            // _cmbMatchClassName
            // 
            this._cmbMatchClassName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbMatchClassName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbMatchClassName.FormattingEnabled = true;
            this._cmbMatchClassName.Items.AddRange(new object[] {
            "を含む",
            "と一致"});
            this._cmbMatchClassName.Location = new System.Drawing.Point(166, 103);
            this._cmbMatchClassName.Name = "_cmbMatchClassName";
            this._cmbMatchClassName.Size = new System.Drawing.Size(60, 20);
            this._cmbMatchClassName.TabIndex = 46;
            // 
            // ExcludeListSettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._panelExcludeSetting);
            this.Controls.Add(this._btnChangeFilter);
            this.Controls.Add(this._btnDeleteFilter);
            this.Controls.Add(this._btnAddFilter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._listView);
            this.Name = "ExcludeListSettingPanel";
            this.Size = new System.Drawing.Size(281, 231);
            this._panelExcludeSetting.ResumeLayout(false);
            this._panelExcludeSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._btnPickApplication)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnChangeFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnDeleteFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnAddFilter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Components.ImageButton _btnChangeFilter;
        private Components.ImageButton _btnDeleteFilter;
        private Components.ImageButton _btnAddFilter;
        private System.Windows.Forms.Panel _panelExcludeSetting;
        private Components.ImageButton _btnOK;
        private Components.ImageButton _btnCancel;
        private Components.ImageButton _btnPickApplication;
        private System.Windows.Forms.Label _labelErrorMsg;
        private System.Windows.Forms.CheckBox _checkValidClassName;
        private System.Windows.Forms.CheckBox _checkValidTitle;
        private System.Windows.Forms.CheckBox _checkValidProductName;
        private System.Windows.Forms.CheckBox _checkValidFileName;
        private System.Windows.Forms.CheckBox _checkRegexClassName;
        private System.Windows.Forms.CheckBox _checkRegexTitle;
        private System.Windows.Forms.CheckBox _checkRegexProductName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _textBoxClassName;
        private System.Windows.Forms.TextBox _textBoxTitle;
        private System.Windows.Forms.TextBox _textBoxProductName;
        private System.Windows.Forms.CheckBox _checkRegexFileName;
        private System.Windows.Forms.TextBox _textBoxFileName;
        private System.Windows.Forms.ListView _listView;
        private System.Windows.Forms.ColumnHeader _columnFileName;
        private System.Windows.Forms.ColumnHeader _columnProduct;
        private System.Windows.Forms.ColumnHeader _columnTitle;
        private System.Windows.Forms.ColumnHeader _columnClass;
        private System.Windows.Forms.Timer _timer;
        private System.Windows.Forms.Label _labelPick;
        private System.Windows.Forms.ToolTip toolTipHelp;
        private System.Windows.Forms.ComboBox _cmbMatchClassName;
        private System.Windows.Forms.ComboBox _cmbMatchTitle;
        private System.Windows.Forms.ComboBox _cmbMatchProductName;
        private System.Windows.Forms.ComboBox _cmbMatchFileName;
    }
}
