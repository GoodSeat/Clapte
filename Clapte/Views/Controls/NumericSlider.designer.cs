namespace GoodSeat.Clapte.Views.Controls
{
    partial class NumericSlider
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
            this.txtBox_input = new System.Windows.Forms.TextBox();
            this._picBox = new System.Windows.Forms.PictureBox();
            this._btnSub = new Clapte.Views.Components.ImageButton(this.components);
            this._btnAdd = new Clapte.Views.Components.ImageButton(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._picBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnSub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnAdd)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBox_input
            // 
            this.txtBox_input.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBox_input.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBox_input.Location = new System.Drawing.Point(0, 6);
            this.txtBox_input.Name = "txtBox_input";
            this.txtBox_input.Size = new System.Drawing.Size(114, 15);
            this.txtBox_input.TabIndex = 1;
            this.txtBox_input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBox_input.Visible = false;
            this.txtBox_input.Enter += new System.EventHandler(this.txtBox_input_Enter);
            this.txtBox_input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_input_KeyDown);
            this.txtBox_input.Leave += new System.EventHandler(this.txtBox_input_Leave);
            // 
            // _picBox
            // 
            this._picBox.BackColor = System.Drawing.Color.White;
            this._picBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._picBox.Location = new System.Drawing.Point(0, 0);
            this._picBox.Name = "_picBox";
            this._picBox.Size = new System.Drawing.Size(114, 25);
            this._picBox.TabIndex = 0;
            this._picBox.TabStop = false;
            // 
            // _btnSub
            // 
            this._btnSub.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._btnSub.BackColor = System.Drawing.Color.Transparent;
            this._btnSub.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnSub.DownMove = 1;
            this._btnSub.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_Sub;
            this._btnSub.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_Sub_Unfocus;
            this._btnSub.Location = new System.Drawing.Point(7, 4);
            this._btnSub.Name = "_btnSub";
            this._btnSub.Size = new System.Drawing.Size(16, 16);
            this._btnSub.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._btnSub.TabIndex = 3;
            this._btnSub.TabStop = false;
            this._btnSub.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_Sub_Unfocus;
            this._btnSub.Visible = false;
            this._btnSub.Click += new System.EventHandler(this._btnSub_Click);
            // 
            // _btnAdd
            // 
            this._btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._btnAdd.BackColor = System.Drawing.Color.Transparent;
            this._btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnAdd.DownMove = 1;
            this._btnAdd.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_Add;
            this._btnAdd.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_Add_Unfocus;
            this._btnAdd.Location = new System.Drawing.Point(93, 4);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size(16, 16);
            this._btnAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._btnAdd.TabIndex = 2;
            this._btnAdd.TabStop = false;
            this._btnAdd.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_Add_Unfocus;
            this._btnAdd.Visible = false;
            this._btnAdd.Click += new System.EventHandler(this._btnAdd_Click);
            // 
            // NumericSlider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._btnSub);
            this.Controls.Add(this._btnAdd);
            this.Controls.Add(this.txtBox_input);
            this.Controls.Add(this._picBox);
            this.Font = new System.Drawing.Font("HGｺﾞｼｯｸM", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "NumericSlider";
            this.Size = new System.Drawing.Size(114, 25);
            this.Leave += new System.EventHandler(this.txtBox_input_Leave);
            ((System.ComponentModel.ISupportInitialize)(this._picBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnSub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._btnAdd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox _picBox;
        private System.Windows.Forms.TextBox txtBox_input;
        private Clapte.Views.Components.ImageButton _btnAdd;
        private Clapte.Views.Components.ImageButton _btnSub;
    }
}
