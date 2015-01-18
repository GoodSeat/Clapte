namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
	partial class UnitConvertTablePanel
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
			this._labelError = new System.Windows.Forms.Label();
			this._dataGridUnitTable = new System.Windows.Forms.DataGridView();
			this._columnUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._columnComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._cmbUnitType = new System.Windows.Forms.ComboBox();
			this._groupUnitType = new System.Windows.Forms.GroupBox();
			this._btnMoveDown = new Clapte.Views.Components.ImageButton(this.components);
			this._btnMoveUp = new Clapte.Views.Components.ImageButton(this.components);
			this._btnDeleteRecord = new Clapte.Views.Components.ImageButton(this.components);
			this._btnAddRecord = new Clapte.Views.Components.ImageButton(this.components);
			this._txtUnitTypeComment = new System.Windows.Forms.TextBox();
			this._panelTableProperty = new System.Windows.Forms.Panel();
			this._labelErrorTable = new System.Windows.Forms.Label();
			this._btnOK = new Clapte.Views.Components.ImageButton(this.components);
			this._btnCancel = new Clapte.Views.Components.ImageButton(this.components);
			this._txtBoxUnitTableComment = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this._txtBoxUnitTableName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this._panelNewRecord = new System.Windows.Forms.Panel();
			this._cmbWithPrefix = new System.Windows.Forms.ComboBox();
			this._radioWithoutPrefix = new System.Windows.Forms.RadioButton();
			this._radioWithPrefix = new System.Windows.Forms.RadioButton();
			this._btnOKRecord = new Clapte.Views.Components.ImageButton(this.components);
			this._btnCancelRecord = new Clapte.Views.Components.ImageButton(this.components);
			this._toolTip = new System.Windows.Forms.ToolTip(this.components);
			this._btnChangeTable = new Clapte.Views.Components.ImageButton(this.components);
			this._btnDeleteTable = new Clapte.Views.Components.ImageButton(this.components);
			this._btnAddTable = new Clapte.Views.Components.ImageButton(this.components);
			this._panelSelectAddChangeTarget = new System.Windows.Forms.Panel();
			this._radioChangeColumn = new System.Windows.Forms.RadioButton();
			this._radioChangeRow = new System.Windows.Forms.RadioButton();
			this._btnOKChangeAdd = new Clapte.Views.Components.ImageButton(this.components);
			this._btnCancelChangeAdd = new Clapte.Views.Components.ImageButton(this.components);
			((System.ComponentModel.ISupportInitialize)(this._dataGridUnitTable)).BeginInit();
			this._groupUnitType.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._btnMoveDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnMoveUp)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnDeleteRecord)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnAddRecord)).BeginInit();
			this._panelTableProperty.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._btnOK)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnCancel)).BeginInit();
			this._panelNewRecord.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._btnOKRecord)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnCancelRecord)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnChangeTable)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnDeleteTable)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnAddTable)).BeginInit();
			this._panelSelectAddChangeTarget.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._btnOKChangeAdd)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnCancelChangeAdd)).BeginInit();
			this.SuspendLayout();
			// 
			// _labelError
			// 
			this._labelError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelError.AutoSize = true;
			this._labelError.ForeColor = System.Drawing.Color.Red;
			this._labelError.Location = new System.Drawing.Point(4, 337);
			this._labelError.Name = "_labelError";
			this._labelError.Size = new System.Drawing.Size(154, 12);
			this._labelError.TabIndex = 12;
			this._labelError.Text = "同名の変数が既に存在します。";
			this._labelError.Visible = false;
			// 
			// _dataGridUnitTable
			// 
			this._dataGridUnitTable.AllowUserToAddRows = false;
			this._dataGridUnitTable.AllowUserToResizeRows = false;
			this._dataGridUnitTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._dataGridUnitTable.BackgroundColor = System.Drawing.Color.White;
			this._dataGridUnitTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._dataGridUnitTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._dataGridUnitTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._columnUnit,
            this._columnComment});
			this._dataGridUnitTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this._dataGridUnitTable.Location = new System.Drawing.Point(6, 42);
			this._dataGridUnitTable.Name = "_dataGridUnitTable";
			this._dataGridUnitTable.RowHeadersWidth = 20;
			this._dataGridUnitTable.RowTemplate.Height = 21;
			this._dataGridUnitTable.Size = new System.Drawing.Size(381, 292);
			this._dataGridUnitTable.TabIndex = 11;
			this._dataGridUnitTable.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this._dataGridUnitTable_UserDeletingRow);
			// 
			// _columnUnit
			// 
			this._columnUnit.Frozen = true;
			this._columnUnit.HeaderText = "単位";
			this._columnUnit.Name = "_columnUnit";
			this._columnUnit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this._columnUnit.Width = 40;
			// 
			// _columnComment
			// 
			this._columnComment.HeaderText = "呼び名";
			this._columnComment.Name = "_columnComment";
			this._columnComment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// _cmbUnitType
			// 
			this._cmbUnitType.DropDownHeight = 306;
			this._cmbUnitType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cmbUnitType.FormattingEnabled = true;
			this._cmbUnitType.IntegralHeight = false;
			this._cmbUnitType.Location = new System.Drawing.Point(11, 8);
			this._cmbUnitType.Name = "_cmbUnitType";
			this._cmbUnitType.Size = new System.Drawing.Size(121, 20);
			this._cmbUnitType.Sorted = true;
			this._cmbUnitType.TabIndex = 10;
			this._cmbUnitType.SelectedIndexChanged += new System.EventHandler(this._cmbUnitType_SelectedIndexChanged);
			// 
			// _groupUnitType
			// 
			this._groupUnitType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._groupUnitType.Controls.Add(this._btnMoveDown);
			this._groupUnitType.Controls.Add(this._btnMoveUp);
			this._groupUnitType.Controls.Add(this._btnDeleteRecord);
			this._groupUnitType.Controls.Add(this._labelError);
			this._groupUnitType.Controls.Add(this._btnAddRecord);
			this._groupUnitType.Controls.Add(this._txtUnitTypeComment);
			this._groupUnitType.Controls.Add(this._dataGridUnitTable);
			this._groupUnitType.Location = new System.Drawing.Point(3, 14);
			this._groupUnitType.Name = "_groupUnitType";
			this._groupUnitType.Size = new System.Drawing.Size(393, 355);
			this._groupUnitType.TabIndex = 16;
			this._groupUnitType.TabStop = false;
			// 
			// _btnMoveDown
			// 
			this._btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnMoveDown.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnMoveDown.DownMove = 1;
			this._btnMoveDown.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_MoveDown;
			this._btnMoveDown.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_MoveDown_Unfocus;
			this._btnMoveDown.Location = new System.Drawing.Point(286, 14);
			this._btnMoveDown.Name = "_btnMoveDown";
			this._btnMoveDown.Size = new System.Drawing.Size(24, 24);
			this._btnMoveDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnMoveDown.TabIndex = 20;
			this._btnMoveDown.TabStop = false;
			this._toolTip.SetToolTip(this._btnMoveDown, "選択した単位レコードを下に移動");
			this._btnMoveDown.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_MoveDown_Unfocus;
			this._btnMoveDown.Click += new System.EventHandler(this._btnMoveDown_Click);
			// 
			// _btnMoveUp
			// 
			this._btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnMoveUp.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnMoveUp.DownMove = 1;
			this._btnMoveUp.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_MoveUp;
			this._btnMoveUp.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_MoveUp_Unfocus;
			this._btnMoveUp.Location = new System.Drawing.Point(306, 7);
			this._btnMoveUp.Name = "_btnMoveUp";
			this._btnMoveUp.Size = new System.Drawing.Size(24, 24);
			this._btnMoveUp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnMoveUp.TabIndex = 19;
			this._btnMoveUp.TabStop = false;
			this._toolTip.SetToolTip(this._btnMoveUp, "選択した単位レコードを上に移動");
			this._btnMoveUp.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_MoveUp_Unfocus;
			this._btnMoveUp.Click += new System.EventHandler(this._btnMoveUp_Click);
			// 
			// _btnDeleteRecord
			// 
			this._btnDeleteRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnDeleteRecord.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnDeleteRecord.DownMove = 1;
			this._btnDeleteRecord.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_DeleteUnitRecord;
			this._btnDeleteRecord.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_DeleteUnitRecord_Unfocus;
			this._btnDeleteRecord.Location = new System.Drawing.Point(361, 11);
			this._btnDeleteRecord.Name = "_btnDeleteRecord";
			this._btnDeleteRecord.Size = new System.Drawing.Size(26, 26);
			this._btnDeleteRecord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnDeleteRecord.TabIndex = 14;
			this._btnDeleteRecord.TabStop = false;
			this._toolTip.SetToolTip(this._btnDeleteRecord, "選択した単位レコードの削除");
			this._btnDeleteRecord.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_DeleteUnitRecord_Unfocus;
			this._btnDeleteRecord.Click += new System.EventHandler(this._btnDeleteRecord_Click);
			// 
			// _btnAddRecord
			// 
			this._btnAddRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnAddRecord.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnAddRecord.DownMove = 1;
			this._btnAddRecord.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_AddUnitRecord;
			this._btnAddRecord.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_AddUnitRecord_Unfocus;
			this._btnAddRecord.Location = new System.Drawing.Point(332, 11);
			this._btnAddRecord.Name = "_btnAddRecord";
			this._btnAddRecord.Size = new System.Drawing.Size(28, 28);
			this._btnAddRecord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnAddRecord.TabIndex = 13;
			this._btnAddRecord.TabStop = false;
			this._toolTip.SetToolTip(this._btnAddRecord, "新規単位レコードの追加");
			this._btnAddRecord.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_AddUnitRecord_Unfocus;
			this._btnAddRecord.Click += new System.EventHandler(this._btnAddRecord_Click);
			// 
			// _txtUnitTypeComment
			// 
			this._txtUnitTypeComment.BackColor = System.Drawing.Color.White;
			this._txtUnitTypeComment.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._txtUnitTypeComment.Location = new System.Drawing.Point(17, 24);
			this._txtUnitTypeComment.Name = "_txtUnitTypeComment";
			this._txtUnitTypeComment.ReadOnly = true;
			this._txtUnitTypeComment.Size = new System.Drawing.Size(250, 12);
			this._txtUnitTypeComment.TabIndex = 15;
			// 
			// _panelTableProperty
			// 
			this._panelTableProperty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._panelTableProperty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelTableProperty.Controls.Add(this._labelErrorTable);
			this._panelTableProperty.Controls.Add(this._btnOK);
			this._panelTableProperty.Controls.Add(this._btnCancel);
			this._panelTableProperty.Controls.Add(this._txtBoxUnitTableComment);
			this._panelTableProperty.Controls.Add(this.label2);
			this._panelTableProperty.Controls.Add(this._txtBoxUnitTableName);
			this._panelTableProperty.Controls.Add(this.label1);
			this._panelTableProperty.Location = new System.Drawing.Point(11, 28);
			this._panelTableProperty.Name = "_panelTableProperty";
			this._panelTableProperty.Size = new System.Drawing.Size(370, 134);
			this._panelTableProperty.TabIndex = 16;
			this._panelTableProperty.Visible = false;
			this._panelTableProperty.VisibleChanged += new System.EventHandler(this._panelTableProperty_VisibleChanged);
			// 
			// _labelErrorTable
			// 
			this._labelErrorTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelErrorTable.AutoSize = true;
			this._labelErrorTable.ForeColor = System.Drawing.Color.Red;
			this._labelErrorTable.Location = new System.Drawing.Point(12, 106);
			this._labelErrorTable.Name = "_labelErrorTable";
			this._labelErrorTable.Size = new System.Drawing.Size(178, 12);
			this._labelErrorTable.TabIndex = 17;
			this._labelErrorTable.Text = "同名の単位次元が既に存在します。";
			this._labelErrorTable.Visible = false;
			// 
			// _btnOK
			// 
			this._btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._btnOK.BackColor = System.Drawing.Color.White;
			this._btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnOK.DownMove = 1;
			this._btnOK.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_OK;
			this._btnOK.Image = global::GoodSeat.Clapte.Properties.Resources.Image_OK_Unfocus;
			this._btnOK.Location = new System.Drawing.Point(200, 106);
			this._btnOK.Name = "_btnOK";
			this._btnOK.Size = new System.Drawing.Size(79, 23);
			this._btnOK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnOK.TabIndex = 19;
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
			this._btnCancel.Location = new System.Drawing.Point(284, 106);
			this._btnCancel.Name = "_btnCancel";
			this._btnCancel.Size = new System.Drawing.Size(79, 23);
			this._btnCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnCancel.TabIndex = 20;
			this._btnCancel.TabStop = false;
			this._btnCancel.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_Cancel_Unfocus;
			this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
			// 
			// _txtBoxUnitTableComment
			// 
			this._txtBoxUnitTableComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._txtBoxUnitTableComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._txtBoxUnitTableComment.Location = new System.Drawing.Point(14, 72);
			this._txtBoxUnitTableComment.Name = "_txtBoxUnitTableComment";
			this._txtBoxUnitTableComment.Size = new System.Drawing.Size(339, 19);
			this._txtBoxUnitTableComment.TabIndex = 18;
			this._txtBoxUnitTableComment.Text = "ユーザー定義の単位次元";
			this._txtBoxUnitTableComment.KeyDown += new System.Windows.Forms.KeyEventHandler(this._txtBoxUnitTableComment_KeyDown);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 57);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 12);
			this.label2.TabIndex = 17;
			this.label2.Text = "コメント";
			// 
			// _txtBoxUnitTableName
			// 
			this._txtBoxUnitTableName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._txtBoxUnitTableName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._txtBoxUnitTableName.Location = new System.Drawing.Point(14, 27);
			this._txtBoxUnitTableName.Name = "_txtBoxUnitTableName";
			this._txtBoxUnitTableName.Size = new System.Drawing.Size(339, 19);
			this._txtBoxUnitTableName.TabIndex = 16;
			this._txtBoxUnitTableName.Text = "UserUnitType";
			this._txtBoxUnitTableName.KeyDown += new System.Windows.Forms.KeyEventHandler(this._txtBoxUnitTableName_KeyDown);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(157, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "単位次元 （Weight、Length等）";
			// 
			// _panelNewRecord
			// 
			this._panelNewRecord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._panelNewRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelNewRecord.Controls.Add(this._cmbWithPrefix);
			this._panelNewRecord.Controls.Add(this._radioWithoutPrefix);
			this._panelNewRecord.Controls.Add(this._radioWithPrefix);
			this._panelNewRecord.Controls.Add(this._btnOKRecord);
			this._panelNewRecord.Controls.Add(this._btnCancelRecord);
			this._panelNewRecord.Location = new System.Drawing.Point(11, 28);
			this._panelNewRecord.Name = "_panelNewRecord";
			this._panelNewRecord.Size = new System.Drawing.Size(370, 92);
			this._panelNewRecord.TabIndex = 16;
			this._panelNewRecord.Visible = false;
			this._panelNewRecord.VisibleChanged += new System.EventHandler(this._panelTableProperty_VisibleChanged);
			// 
			// _cmbWithPrefix
			// 
			this._cmbWithPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._cmbWithPrefix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cmbWithPrefix.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this._cmbWithPrefix.FormattingEnabled = true;
			this._cmbWithPrefix.Location = new System.Drawing.Point(34, 12);
			this._cmbWithPrefix.Name = "_cmbWithPrefix";
			this._cmbWithPrefix.Size = new System.Drawing.Size(319, 20);
			this._cmbWithPrefix.TabIndex = 25;
			// 
			// _radioWithoutPrefix
			// 
			this._radioWithoutPrefix.AutoSize = true;
			this._radioWithoutPrefix.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._radioWithoutPrefix.Location = new System.Drawing.Point(19, 39);
			this._radioWithoutPrefix.Name = "_radioWithoutPrefix";
			this._radioWithoutPrefix.Size = new System.Drawing.Size(132, 16);
			this._radioWithoutPrefix.TabIndex = 24;
			this._radioWithoutPrefix.TabStop = true;
			this._radioWithoutPrefix.Text = "単位名「kg」として登録";
			this._radioWithoutPrefix.UseVisualStyleBackColor = true;
			this._radioWithoutPrefix.CheckedChanged += new System.EventHandler(this.AddRecordPrefixChanged);
			// 
			// _radioWithPrefix
			// 
			this._radioWithPrefix.AutoSize = true;
			this._radioWithPrefix.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._radioWithPrefix.Location = new System.Drawing.Point(19, 14);
			this._radioWithPrefix.Name = "_radioWithPrefix";
			this._radioWithPrefix.Size = new System.Drawing.Size(219, 16);
			this._radioWithPrefix.TabIndex = 23;
			this._radioWithPrefix.TabStop = true;
			this._radioWithPrefix.Text = "接頭辞「k(キロ)」＋単位名「g」として登録";
			this._radioWithPrefix.UseVisualStyleBackColor = true;
			this._radioWithPrefix.CheckedChanged += new System.EventHandler(this.AddRecordPrefixChanged);
			// 
			// _btnOKRecord
			// 
			this._btnOKRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._btnOKRecord.BackColor = System.Drawing.Color.White;
			this._btnOKRecord.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnOKRecord.DownMove = 1;
			this._btnOKRecord.Enabled = false;
			this._btnOKRecord.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_OK;
			this._btnOKRecord.Image = global::GoodSeat.Clapte.Properties.Resources.Image_OK_Unfocus;
			this._btnOKRecord.Location = new System.Drawing.Point(200, 64);
			this._btnOKRecord.Name = "_btnOKRecord";
			this._btnOKRecord.Size = new System.Drawing.Size(79, 23);
			this._btnOKRecord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnOKRecord.TabIndex = 21;
			this._btnOKRecord.TabStop = false;
			this._btnOKRecord.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_OK_Unfocus;
			this._btnOKRecord.Click += new System.EventHandler(this._btnOKRecord_Click);
			// 
			// _btnCancelRecord
			// 
			this._btnCancelRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._btnCancelRecord.BackColor = System.Drawing.Color.White;
			this._btnCancelRecord.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnCancelRecord.DownMove = 1;
			this._btnCancelRecord.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_Cancel;
			this._btnCancelRecord.Image = global::GoodSeat.Clapte.Properties.Resources.Image_Cancel_Unfocus;
			this._btnCancelRecord.Location = new System.Drawing.Point(284, 64);
			this._btnCancelRecord.Name = "_btnCancelRecord";
			this._btnCancelRecord.Size = new System.Drawing.Size(79, 23);
			this._btnCancelRecord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnCancelRecord.TabIndex = 22;
			this._btnCancelRecord.TabStop = false;
			this._btnCancelRecord.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_Cancel_Unfocus;
			this._btnCancelRecord.Click += new System.EventHandler(this._btnCancelRecord_Click);
			// 
			// _toolTip
			// 
			this._toolTip.BackColor = System.Drawing.Color.White;
			// 
			// _btnChangeTable
			// 
			this._btnChangeTable.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnChangeTable.DownMove = 1;
			this._btnChangeTable.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_ChangeUnitTable;
			this._btnChangeTable.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_ChangeUnitTable_Unfocus;
			this._btnChangeTable.Location = new System.Drawing.Point(162, 0);
			this._btnChangeTable.Name = "_btnChangeTable";
			this._btnChangeTable.Size = new System.Drawing.Size(26, 26);
			this._btnChangeTable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnChangeTable.TabIndex = 18;
			this._btnChangeTable.TabStop = false;
			this._toolTip.SetToolTip(this._btnChangeTable, "単位表の名前、コメントの編集");
			this._btnChangeTable.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_ChangeUnitTable_Unfocus;
			this._btnChangeTable.Click += new System.EventHandler(this._btnChangeTable_Click);
			// 
			// _btnDeleteTable
			// 
			this._btnDeleteTable.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnDeleteTable.DownMove = 1;
			this._btnDeleteTable.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_DeleteUnitTable;
			this._btnDeleteTable.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_DeleteUnitTable_Unfocus;
			this._btnDeleteTable.Location = new System.Drawing.Point(187, 1);
			this._btnDeleteTable.Name = "_btnDeleteTable";
			this._btnDeleteTable.Size = new System.Drawing.Size(25, 25);
			this._btnDeleteTable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnDeleteTable.TabIndex = 18;
			this._btnDeleteTable.TabStop = false;
			this._toolTip.SetToolTip(this._btnDeleteTable, "現在の単位表の削除");
			this._btnDeleteTable.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_DeleteUnitTable_Unfocus;
			this._btnDeleteTable.Click += new System.EventHandler(this._btnDeleteTable_Click);
			// 
			// _btnAddTable
			// 
			this._btnAddTable.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnAddTable.DownMove = 1;
			this._btnAddTable.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_AddUnitTable;
			this._btnAddTable.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_AddUnitTable_Unfocus;
			this._btnAddTable.Location = new System.Drawing.Point(137, 0);
			this._btnAddTable.Name = "_btnAddTable";
			this._btnAddTable.Size = new System.Drawing.Size(26, 26);
			this._btnAddTable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnAddTable.TabIndex = 17;
			this._btnAddTable.TabStop = false;
			this._toolTip.SetToolTip(this._btnAddTable, "新規単位表の追加");
			this._btnAddTable.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_AddUnitTable_Unfocus;
			this._btnAddTable.Click += new System.EventHandler(this._btnAddTable_Click);
			// 
			// _panelSelectAddChangeTarget
			// 
			this._panelSelectAddChangeTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._panelSelectAddChangeTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelSelectAddChangeTarget.Controls.Add(this._radioChangeColumn);
			this._panelSelectAddChangeTarget.Controls.Add(this._radioChangeRow);
			this._panelSelectAddChangeTarget.Controls.Add(this._btnOKChangeAdd);
			this._panelSelectAddChangeTarget.Controls.Add(this._btnCancelChangeAdd);
			this._panelSelectAddChangeTarget.Location = new System.Drawing.Point(11, 28);
			this._panelSelectAddChangeTarget.Name = "_panelSelectAddChangeTarget";
			this._panelSelectAddChangeTarget.Size = new System.Drawing.Size(370, 92);
			this._panelSelectAddChangeTarget.TabIndex = 19;
			this._panelSelectAddChangeTarget.Visible = false;
			this._panelSelectAddChangeTarget.VisibleChanged += new System.EventHandler(this._panelTableProperty_VisibleChanged);
			// 
			// _radioChangeColumn
			// 
			this._radioChangeColumn.AutoSize = true;
			this._radioChangeColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._radioChangeColumn.Location = new System.Drawing.Point(19, 39);
			this._radioChangeColumn.Name = "_radioChangeColumn";
			this._radioChangeColumn.Size = new System.Drawing.Size(237, 16);
			this._radioChangeColumn.TabIndex = 24;
			this._radioChangeColumn.TabStop = true;
			this._radioChangeColumn.Text = "「degF」の「K」に対する加算値を32に変更する";
			this._radioChangeColumn.UseVisualStyleBackColor = true;
			this._radioChangeColumn.CheckedChanged += new System.EventHandler(this._radioChangeColumn_CheckedChanged);
			// 
			// _radioChangeRow
			// 
			this._radioChangeRow.AutoSize = true;
			this._radioChangeRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._radioChangeRow.Location = new System.Drawing.Point(19, 14);
			this._radioChangeRow.Name = "_radioChangeRow";
			this._radioChangeRow.Size = new System.Drawing.Size(244, 16);
			this._radioChangeRow.TabIndex = 23;
			this._radioChangeRow.TabStop = true;
			this._radioChangeRow.Text = "「degC」の「K」に対する加算値を100に変更する";
			this._radioChangeRow.UseVisualStyleBackColor = true;
			this._radioChangeRow.CheckedChanged += new System.EventHandler(this._radioChangeColumn_CheckedChanged);
			// 
			// _btnOKChangeAdd
			// 
			this._btnOKChangeAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._btnOKChangeAdd.BackColor = System.Drawing.Color.White;
			this._btnOKChangeAdd.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnOKChangeAdd.DownMove = 1;
			this._btnOKChangeAdd.Enabled = false;
			this._btnOKChangeAdd.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_OK;
			this._btnOKChangeAdd.Image = global::GoodSeat.Clapte.Properties.Resources.Image_OK_Unfocus;
			this._btnOKChangeAdd.Location = new System.Drawing.Point(200, 64);
			this._btnOKChangeAdd.Name = "_btnOKChangeAdd";
			this._btnOKChangeAdd.Size = new System.Drawing.Size(79, 23);
			this._btnOKChangeAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnOKChangeAdd.TabIndex = 21;
			this._btnOKChangeAdd.TabStop = false;
			this._btnOKChangeAdd.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_OK_Unfocus;
			this._btnOKChangeAdd.Click += new System.EventHandler(this._btnOKChangeAdd_Click);
			// 
			// _btnCancelChangeAdd
			// 
			this._btnCancelChangeAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._btnCancelChangeAdd.BackColor = System.Drawing.Color.White;
			this._btnCancelChangeAdd.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnCancelChangeAdd.DownMove = 1;
			this._btnCancelChangeAdd.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_Cancel;
			this._btnCancelChangeAdd.Image = global::GoodSeat.Clapte.Properties.Resources.Image_Cancel_Unfocus;
			this._btnCancelChangeAdd.Location = new System.Drawing.Point(284, 64);
			this._btnCancelChangeAdd.Name = "_btnCancelChangeAdd";
			this._btnCancelChangeAdd.Size = new System.Drawing.Size(79, 23);
			this._btnCancelChangeAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnCancelChangeAdd.TabIndex = 22;
			this._btnCancelChangeAdd.TabStop = false;
			this._btnCancelChangeAdd.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_Cancel_Unfocus;
			this._btnCancelChangeAdd.Click += new System.EventHandler(this._btnCancelChangeAdd_Click);
			// 
			// UnitConvertTablePanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this._panelSelectAddChangeTarget);
			this.Controls.Add(this._panelNewRecord);
			this.Controls.Add(this._panelTableProperty);
			this.Controls.Add(this._btnChangeTable);
			this.Controls.Add(this._btnDeleteTable);
			this.Controls.Add(this._btnAddTable);
			this.Controls.Add(this._cmbUnitType);
			this.Controls.Add(this._groupUnitType);
			this.Name = "UnitConvertTablePanel";
			this.Size = new System.Drawing.Size(399, 372);
			((System.ComponentModel.ISupportInitialize)(this._dataGridUnitTable)).EndInit();
			this._groupUnitType.ResumeLayout(false);
			this._groupUnitType.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._btnMoveDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnMoveUp)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnDeleteRecord)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnAddRecord)).EndInit();
			this._panelTableProperty.ResumeLayout(false);
			this._panelTableProperty.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._btnOK)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnCancel)).EndInit();
			this._panelNewRecord.ResumeLayout(false);
			this._panelNewRecord.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._btnOKRecord)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnCancelRecord)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnChangeTable)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnDeleteTable)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnAddTable)).EndInit();
			this._panelSelectAddChangeTarget.ResumeLayout(false);
			this._panelSelectAddChangeTarget.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._btnOKChangeAdd)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnCancelChangeAdd)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Components.ImageButton _btnDeleteRecord;
		private Components.ImageButton _btnAddRecord;
		private System.Windows.Forms.Label _labelError;
		private System.Windows.Forms.DataGridView _dataGridUnitTable;
		private System.Windows.Forms.ComboBox _cmbUnitType;
		private System.Windows.Forms.GroupBox _groupUnitType;
		private Components.ImageButton _btnDeleteTable;
		private Components.ImageButton _btnAddTable;
		private System.Windows.Forms.TextBox _txtUnitTypeComment;
		private Components.ImageButton _btnChangeTable;
		private System.Windows.Forms.Panel _panelTableProperty;
		private System.Windows.Forms.TextBox _txtBoxUnitTableComment;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox _txtBoxUnitTableName;
		private System.Windows.Forms.Label label1;
		private Components.ImageButton _btnOK;
		private Components.ImageButton _btnCancel;
		private System.Windows.Forms.Label _labelErrorTable;
		private System.Windows.Forms.Panel _panelNewRecord;
		private System.Windows.Forms.RadioButton _radioWithoutPrefix;
		private System.Windows.Forms.RadioButton _radioWithPrefix;
		private Components.ImageButton _btnOKRecord;
		private Components.ImageButton _btnCancelRecord;
		private System.Windows.Forms.ComboBox _cmbWithPrefix;
		private Components.ImageButton _btnMoveDown;
		private Components.ImageButton _btnMoveUp;
		private System.Windows.Forms.DataGridViewTextBoxColumn _columnUnit;
		private System.Windows.Forms.DataGridViewTextBoxColumn _columnComment;
		private System.Windows.Forms.ToolTip _toolTip;
		private System.Windows.Forms.Panel _panelSelectAddChangeTarget;
		private System.Windows.Forms.RadioButton _radioChangeColumn;
		private System.Windows.Forms.RadioButton _radioChangeRow;
		private Components.ImageButton _btnOKChangeAdd;
		private Components.ImageButton _btnCancelChangeAdd;
	}
}
