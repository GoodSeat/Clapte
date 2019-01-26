namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    partial class UserFunctionPanel
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
            this._btnDelete = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._btnAdd = new GoodSeat.Clapte.Views.Components.ImageButton(this.components);
            this._labelError = new System.Windows.Forms.Label();
            this._dataGridFunction = new System.Windows.Forms.DataGridView();
            this._columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._columnDefine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._columnComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._cmbFilter = new System.Windows.Forms.ComboBox();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._btnDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridFunction)).BeginInit();
            this.SuspendLayout();
            // 
            // _btnDelete
            // 
            this._btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnDelete.DownMove = 1;
            this._btnDelete.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_DeleteFunction;
            this._btnDelete.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_DeleteFunction_Unfocus;
            this._btnDelete.Location = new System.Drawing.Point(362, 1);
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.Size = new System.Drawing.Size(26, 26);
            this._btnDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._btnDelete.TabIndex = 9;
            this._btnDelete.TabStop = false;
            this._toolTip.SetToolTip(this._btnDelete, "選択した関数の削除");
            this._btnDelete.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_DeleteFunction_Unfocus;
            this._btnDelete.Click += new System.EventHandler(this._btnDelete_Click);
            // 
            // _btnAdd
            // 
            this._btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnAdd.DownMove = 1;
            this._btnAdd.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_AddFunction;
            this._btnAdd.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_AddFunction_Unfocus;
            this._btnAdd.Location = new System.Drawing.Point(331, 0);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size(28, 28);
            this._btnAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._btnAdd.TabIndex = 8;
            this._btnAdd.TabStop = false;
            this._toolTip.SetToolTip(this._btnAdd, "関数の新規追加");
            this._btnAdd.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_AddFunction_Unfocus;
            this._btnAdd.Click += new System.EventHandler(this._btnAdd_Click);
            // 
            // _labelError
            // 
            this._labelError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._labelError.AutoSize = true;
            this._labelError.ForeColor = System.Drawing.Color.Red;
            this._labelError.Location = new System.Drawing.Point(4, 292);
            this._labelError.Name = "_labelError";
            this._labelError.Size = new System.Drawing.Size(154, 12);
            this._labelError.TabIndex = 7;
            this._labelError.Text = "同名の変数が既に存在します。";
            this._labelError.Visible = false;
            // 
            // _dataGridFunction
            // 
            this._dataGridFunction.AllowUserToAddRows = false;
            this._dataGridFunction.AllowUserToResizeRows = false;
            this._dataGridFunction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dataGridFunction.BackgroundColor = System.Drawing.Color.White;
            this._dataGridFunction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._dataGridFunction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGridFunction.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._columnName,
            this._columnDefine,
            this._columnComment});
            this._dataGridFunction.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._dataGridFunction.Location = new System.Drawing.Point(4, 33);
            this._dataGridFunction.Name = "_dataGridFunction";
            this._dataGridFunction.RowHeadersWidth = 20;
            this._dataGridFunction.RowTemplate.Height = 21;
            this._dataGridFunction.Size = new System.Drawing.Size(381, 255);
            this._dataGridFunction.TabIndex = 6;
            this._dataGridFunction.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this._dataGridFunction_EditingControlShowing);
            this._dataGridFunction.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this._dataGridFunction_SortCompare);
            this._dataGridFunction.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this._dataGridFunction_UserDeletingRow);
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
            this._columnComment.Width = 150;
            // 
            // _cmbFilter
            // 
            this._cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbFilter.FormattingEnabled = true;
            this._cmbFilter.Items.AddRange(new object[] {
            "すべて",
            "システム定義",
            "ユーザー定義"});
            this._cmbFilter.Location = new System.Drawing.Point(4, 8);
            this._cmbFilter.Name = "_cmbFilter";
            this._cmbFilter.Size = new System.Drawing.Size(121, 20);
            this._cmbFilter.TabIndex = 5;
            // 
            // _toolTip
            // 
            this._toolTip.BackColor = System.Drawing.Color.White;
            // 
            // UserFunctionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._btnDelete);
            this.Controls.Add(this._btnAdd);
            this.Controls.Add(this._labelError);
            this.Controls.Add(this._dataGridFunction);
            this.Controls.Add(this._cmbFilter);
            this.Name = "UserFunctionPanel";
            this.Size = new System.Drawing.Size(389, 307);
            ((System.ComponentModel.ISupportInitialize)(this._btnDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridFunction)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.ImageButton _btnDelete;
        private Components.ImageButton _btnAdd;
        private System.Windows.Forms.Label _labelError;
        private System.Windows.Forms.DataGridView _dataGridFunction;
        private System.Windows.Forms.ComboBox _cmbFilter;
        private System.Windows.Forms.ToolTip _toolTip;
        private System.Windows.Forms.DataGridViewTextBoxColumn _columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn _columnDefine;
        private System.Windows.Forms.DataGridViewTextBoxColumn _columnComment;
    }
}
