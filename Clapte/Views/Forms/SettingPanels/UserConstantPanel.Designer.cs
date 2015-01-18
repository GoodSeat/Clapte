namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
	partial class UserConstantPanel
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
			this._cmbFilter = new System.Windows.Forms.ComboBox();
			this._dataGridVariable = new System.Windows.Forms.DataGridView();
			this._columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._columnDefine = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._columnComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._labelError = new System.Windows.Forms.Label();
			this._toolTip = new System.Windows.Forms.ToolTip(this.components);
			this._btnDelete = new Clapte.Views.Components.ImageButton(this.components);
			this._btnAdd = new Clapte.Views.Components.ImageButton(this.components);
			((System.ComponentModel.ISupportInitialize)(this._dataGridVariable)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnDelete)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnAdd)).BeginInit();
			this.SuspendLayout();
			// 
			// _cmbFilter
			// 
			this._cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cmbFilter.FormattingEnabled = true;
			this._cmbFilter.Items.AddRange(new object[] {
            "すべて",
            "システム定義",
            "ユーザー定義"});
			this._cmbFilter.Location = new System.Drawing.Point(5, 8);
			this._cmbFilter.Name = "_cmbFilter";
			this._cmbFilter.Size = new System.Drawing.Size(121, 20);
			this._cmbFilter.TabIndex = 0;
			// 
			// _dataGridVariable
			// 
			this._dataGridVariable.AllowUserToAddRows = false;
			this._dataGridVariable.AllowUserToResizeRows = false;
			this._dataGridVariable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._dataGridVariable.BackgroundColor = System.Drawing.Color.White;
			this._dataGridVariable.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._dataGridVariable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._dataGridVariable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._columnName,
            this._columnDefine,
            this._columnComment});
			this._dataGridVariable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this._dataGridVariable.Location = new System.Drawing.Point(5, 33);
			this._dataGridVariable.Name = "_dataGridVariable";
			this._dataGridVariable.RowHeadersWidth = 20;
			this._dataGridVariable.RowTemplate.Height = 21;
			this._dataGridVariable.Size = new System.Drawing.Size(381, 255);
			this._dataGridVariable.TabIndex = 1;
			this._dataGridVariable.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this._dataGridVariable_SortCompare);
			this._dataGridVariable.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this._variableList_UserDeletingRow);
			// 
			// _columnName
			// 
			this._columnName.Frozen = true;
			this._columnName.HeaderText = "名前";
			this._columnName.Name = "_columnName";
			this._columnName.Width = 90;
			// 
			// _columnDefine
			// 
			this._columnDefine.HeaderText = "定義";
			this._columnDefine.Name = "_columnDefine";
			// 
			// _columnComment
			// 
			this._columnComment.HeaderText = "コメント";
			this._columnComment.Name = "_columnComment";
			this._columnComment.Width = 200;
			// 
			// _labelError
			// 
			this._labelError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelError.AutoSize = true;
			this._labelError.ForeColor = System.Drawing.Color.Red;
			this._labelError.Location = new System.Drawing.Point(5, 292);
			this._labelError.Name = "_labelError";
			this._labelError.Size = new System.Drawing.Size(154, 12);
			this._labelError.TabIndex = 2;
			this._labelError.Text = "同名の変数が既に存在します。";
			this._labelError.Visible = false;
			// 
			// _toolTip
			// 
			this._toolTip.BackColor = System.Drawing.Color.White;
			// 
			// _btnDelete
			// 
			this._btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnDelete.DownMove = 1;
			this._btnDelete.FocusImage = global::Clapte.Properties.Resources.Icon_DeleteVariable;
			this._btnDelete.Image = global::Clapte.Properties.Resources.Icon_DeleteVariable_Unfocus;
			this._btnDelete.Location = new System.Drawing.Point(358, 3);
			this._btnDelete.Name = "_btnDelete";
			this._btnDelete.Size = new System.Drawing.Size(24, 24);
			this._btnDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnDelete.TabIndex = 4;
			this._btnDelete.TabStop = false;
			this._toolTip.SetToolTip(this._btnDelete, "選択した変数の削除");
			this._btnDelete.UnFocusImage = global::Clapte.Properties.Resources.Icon_DeleteVariable_Unfocus;
			this._btnDelete.Click += new System.EventHandler(this._btnDelete_Click);
			// 
			// _btnAdd
			// 
			this._btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnAdd.DownMove = 1;
			this._btnAdd.FocusImage = global::Clapte.Properties.Resources.Icon_AddVariable;
			this._btnAdd.Image = global::Clapte.Properties.Resources.Icon_AddVariable_Unfocus;
			this._btnAdd.Location = new System.Drawing.Point(333, 3);
			this._btnAdd.Name = "_btnAdd";
			this._btnAdd.Size = new System.Drawing.Size(24, 24);
			this._btnAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnAdd.TabIndex = 3;
			this._btnAdd.TabStop = false;
			this._toolTip.SetToolTip(this._btnAdd, "変数の新規追加");
			this._btnAdd.UnFocusImage = global::Clapte.Properties.Resources.Icon_AddVariable_Unfocus;
			this._btnAdd.Click += new System.EventHandler(this._btnAdd_Click);
			// 
			// UserConstantPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this._btnDelete);
			this.Controls.Add(this._btnAdd);
			this.Controls.Add(this._labelError);
			this.Controls.Add(this._dataGridVariable);
			this.Controls.Add(this._cmbFilter);
			this.Name = "UserConstantPanel";
			this.Size = new System.Drawing.Size(389, 307);
			((System.ComponentModel.ISupportInitialize)(this._dataGridVariable)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnDelete)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnAdd)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox _cmbFilter;
		private System.Windows.Forms.DataGridView _dataGridVariable;
		private System.Windows.Forms.Label _labelError;
		private Components.ImageButton _btnAdd;
		private Components.ImageButton _btnDelete;
		private System.Windows.Forms.ToolTip _toolTip;
		private System.Windows.Forms.DataGridViewTextBoxColumn _columnName;
		private System.Windows.Forms.DataGridViewTextBoxColumn _columnDefine;
		private System.Windows.Forms.DataGridViewTextBoxColumn _columnComment;
	}
}
