namespace GoodSeat.Clapte.Views.Forms
{
	partial class ClapteFormBase
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this._labelTitle = new System.Windows.Forms.Label();
			this._toolHelpTip = new System.Windows.Forms.ToolTip(this.components);
			this._btnOption = new Clapte.Views.Components.ImageButton(this.components);
			this._btnClose = new Clapte.Views.Components.ImageButton(this.components);
			this._btnTopMost = new Clapte.Views.Components.ImageButton(this.components);
			this._picTitleBar = new Clapte.Views.Components.ChameleonPictureBox(this.components);
			this._btnOK = new Clapte.Views.Components.ImageButton(this.components);
			this._btnCancel = new Clapte.Views.Components.ImageButton(this.components);
			this._frameBottomRight = new Clapte.Views.Components.ChameleonPictureBox(this.components);
			this._frameRight = new Clapte.Views.Components.ChameleonPictureBox(this.components);
			this._frameTopRight = new Clapte.Views.Components.ChameleonPictureBox(this.components);
			this._frameBottom = new Clapte.Views.Components.ChameleonPictureBox(this.components);
			this._frameTop = new Clapte.Views.Components.ChameleonPictureBox(this.components);
			this._frameBottomLeft = new Clapte.Views.Components.ChameleonPictureBox(this.components);
			this._frameLeft = new Clapte.Views.Components.ChameleonPictureBox(this.components);
			this._frameTopLeft = new Clapte.Views.Components.ChameleonPictureBox(this.components);
			((System.ComponentModel.ISupportInitialize)(this._btnOption)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnClose)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnTopMost)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._picTitleBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnOK)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._btnCancel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._frameBottomRight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._frameRight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._frameTopRight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._frameBottom)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._frameTop)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._frameBottomLeft)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._frameLeft)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._frameTopLeft)).BeginInit();
			this.SuspendLayout();
			// 
			// _labelTitle
			// 
			this._labelTitle.AutoSize = true;
			this._labelTitle.BackColor = System.Drawing.Color.White;
			this._labelTitle.Font = new System.Drawing.Font("HGPｺﾞｼｯｸE", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this._labelTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this._labelTitle.Location = new System.Drawing.Point(15, 13);
			this._labelTitle.Name = "_labelTitle";
			this._labelTitle.Size = new System.Drawing.Size(88, 12);
			this._labelTitle.TabIndex = 13;
			this._labelTitle.Text = "ClapteFormBase";
			this._labelTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this._labelTitle.Visible = false;
			// 
			// _toolHelpTip
			// 
			this._toolHelpTip.BackColor = System.Drawing.Color.White;
			// 
			// _btnOption
			// 
			this._btnOption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnOption.BackColor = System.Drawing.Color.White;
			this._btnOption.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnOption.DownMove = 1;
			this._btnOption.FocusImage = null;
			this._btnOption.Location = new System.Drawing.Point(476, 9);
			this._btnOption.Name = "_btnOption";
			this._btnOption.Size = new System.Drawing.Size(18, 16);
			this._btnOption.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnOption.TabIndex = 15;
			this._btnOption.TabStop = false;
			this._toolHelpTip.SetToolTip(this._btnOption, "オプションを表示します");
			this._btnOption.UnFocusImage = null;
			this._btnOption.Visible = false;
			this._btnOption.Click += new System.EventHandler(this._btnOption_Click);
			// 
			// _btnClose
			// 
			this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnClose.BackColor = System.Drawing.Color.White;
			this._btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnClose.DownMove = 1;
			this._btnClose.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_ClearFilter;
			this._btnClose.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_ClearFilter_Unfocus;
			this._btnClose.Location = new System.Drawing.Point(516, 10);
			this._btnClose.Name = "_btnClose";
			this._btnClose.Size = new System.Drawing.Size(14, 14);
			this._btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnClose.TabIndex = 12;
			this._btnClose.TabStop = false;
			this._toolHelpTip.SetToolTip(this._btnClose, "閉じる");
			this._btnClose.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_ClearFilter_Unfocus;
			this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
			// 
			// _btnTopMost
			// 
			this._btnTopMost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._btnTopMost.BackColor = System.Drawing.Color.White;
			this._btnTopMost.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnTopMost.DownMove = 1;
			this._btnTopMost.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_PushPin;
			this._btnTopMost.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_PushPin_Abort;
			this._btnTopMost.Location = new System.Drawing.Point(495, 9);
			this._btnTopMost.Name = "_btnTopMost";
			this._btnTopMost.Size = new System.Drawing.Size(18, 16);
			this._btnTopMost.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnTopMost.TabIndex = 11;
			this._btnTopMost.TabStop = false;
			this._toolHelpTip.SetToolTip(this._btnTopMost, "最前面表示の有効化／無効化");
			this._btnTopMost.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Icon_PushPin_Abort;
			this._btnTopMost.Click += new System.EventHandler(this._btnTopMost_Click);
			// 
			// _picTitleBar
			// 
			this._picTitleBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._picTitleBar.BackColor = System.Drawing.Color.White;
			this._picTitleBar.HitPosition = Clapte.Views.Components.ChameleonPictureBox.WindowsHitPosition.Transparent;
			this._picTitleBar.Location = new System.Drawing.Point(16, 12);
			this._picTitleBar.Name = "_picTitleBar";
			this._picTitleBar.Size = new System.Drawing.Size(465, 20);
			this._picTitleBar.TabIndex = 14;
			this._picTitleBar.TabStop = false;
			// 
			// _btnOK
			// 
			this._btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._btnOK.BackColor = System.Drawing.Color.White;
			this._btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnOK.DownMove = 1;
			this._btnOK.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_OK;
			this._btnOK.Image = global::GoodSeat.Clapte.Properties.Resources.Image_OK_Unfocus;
			this._btnOK.Location = new System.Drawing.Point(369, 332);
			this._btnOK.Name = "_btnOK";
			this._btnOK.Size = new System.Drawing.Size(79, 23);
			this._btnOK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnOK.TabIndex = 9;
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
			this._btnCancel.Location = new System.Drawing.Point(453, 332);
			this._btnCancel.Name = "_btnCancel";
			this._btnCancel.Size = new System.Drawing.Size(79, 23);
			this._btnCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnCancel.TabIndex = 10;
			this._btnCancel.TabStop = false;
			this._btnCancel.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_Cancel_Unfocus;
			this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
			// 
			// _frameBottomRight
			// 
			this._frameBottomRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._frameBottomRight.HitPosition = Clapte.Views.Components.ChameleonPictureBox.WindowsHitPosition.Transparent;
			this._frameBottomRight.Image = global::GoodSeat.Clapte.Properties.Resources.Frame_BottomRight;
			this._frameBottomRight.Location = new System.Drawing.Point(483, 310);
			this._frameBottomRight.Name = "_frameBottomRight";
			this._frameBottomRight.Size = new System.Drawing.Size(56, 50);
			this._frameBottomRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this._frameBottomRight.TabIndex = 8;
			this._frameBottomRight.TabStop = false;
			// 
			// _frameRight
			// 
			this._frameRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._frameRight.HitPosition = Clapte.Views.Components.ChameleonPictureBox.WindowsHitPosition.Transparent;
			this._frameRight.Image = global::GoodSeat.Clapte.Properties.Resources.Frame_Right;
			this._frameRight.Location = new System.Drawing.Point(483, 48);
			this._frameRight.Name = "_frameRight";
			this._frameRight.Size = new System.Drawing.Size(56, 277);
			this._frameRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this._frameRight.TabIndex = 7;
			this._frameRight.TabStop = false;
			// 
			// _frameTopRight
			// 
			this._frameTopRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._frameTopRight.HitPosition = Clapte.Views.Components.ChameleonPictureBox.WindowsHitPosition.Transparent;
			this._frameTopRight.Image = global::GoodSeat.Clapte.Properties.Resources.Frame_TopRight;
			this._frameTopRight.Location = new System.Drawing.Point(483, 0);
			this._frameTopRight.Name = "_frameTopRight";
			this._frameTopRight.Size = new System.Drawing.Size(56, 49);
			this._frameTopRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this._frameTopRight.TabIndex = 6;
			this._frameTopRight.TabStop = false;
			// 
			// _frameBottom
			// 
			this._frameBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._frameBottom.HitPosition = Clapte.Views.Components.ChameleonPictureBox.WindowsHitPosition.Transparent;
			this._frameBottom.Image = global::GoodSeat.Clapte.Properties.Resources.Frame_Bottom;
			this._frameBottom.Location = new System.Drawing.Point(54, 310);
			this._frameBottom.Name = "_frameBottom";
			this._frameBottom.Size = new System.Drawing.Size(441, 50);
			this._frameBottom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this._frameBottom.TabIndex = 5;
			this._frameBottom.TabStop = false;
			// 
			// _frameTop
			// 
			this._frameTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._frameTop.HitPosition = Clapte.Views.Components.ChameleonPictureBox.WindowsHitPosition.Transparent;
			this._frameTop.Image = global::GoodSeat.Clapte.Properties.Resources.Frame_Top;
			this._frameTop.Location = new System.Drawing.Point(54, 0);
			this._frameTop.Name = "_frameTop";
			this._frameTop.Size = new System.Drawing.Size(441, 49);
			this._frameTop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this._frameTop.TabIndex = 4;
			this._frameTop.TabStop = false;
			// 
			// _frameBottomLeft
			// 
			this._frameBottomLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._frameBottomLeft.HitPosition = Clapte.Views.Components.ChameleonPictureBox.WindowsHitPosition.Transparent;
			this._frameBottomLeft.Image = global::GoodSeat.Clapte.Properties.Resources.Frame_BottomLeft;
			this._frameBottomLeft.Location = new System.Drawing.Point(0, 310);
			this._frameBottomLeft.Name = "_frameBottomLeft";
			this._frameBottomLeft.Size = new System.Drawing.Size(56, 50);
			this._frameBottomLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this._frameBottomLeft.TabIndex = 2;
			this._frameBottomLeft.TabStop = false;
			// 
			// _frameLeft
			// 
			this._frameLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this._frameLeft.HitPosition = Clapte.Views.Components.ChameleonPictureBox.WindowsHitPosition.Transparent;
			this._frameLeft.Image = global::GoodSeat.Clapte.Properties.Resources.Frame_Left;
			this._frameLeft.Location = new System.Drawing.Point(0, 48);
			this._frameLeft.Name = "_frameLeft";
			this._frameLeft.Size = new System.Drawing.Size(56, 277);
			this._frameLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this._frameLeft.TabIndex = 1;
			this._frameLeft.TabStop = false;
			// 
			// _frameTopLeft
			// 
			this._frameTopLeft.HitPosition = Clapte.Views.Components.ChameleonPictureBox.WindowsHitPosition.Transparent;
			this._frameTopLeft.Image = global::GoodSeat.Clapte.Properties.Resources.Frame_TopLeft;
			this._frameTopLeft.Location = new System.Drawing.Point(0, 0);
			this._frameTopLeft.Name = "_frameTopLeft";
			this._frameTopLeft.Size = new System.Drawing.Size(56, 49);
			this._frameTopLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this._frameTopLeft.TabIndex = 0;
			this._frameTopLeft.TabStop = false;
			// 
			// ClapteFormBase
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.White;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(538, 360);
			this.ControlBox = false;
			this.Controls.Add(this._btnOption);
			this.Controls.Add(this._btnClose);
			this.Controls.Add(this._picTitleBar);
			this.Controls.Add(this._btnOK);
			this.Controls.Add(this._btnCancel);
			this.Controls.Add(this._frameBottomRight);
			this.Controls.Add(this._btnTopMost);
			this.Controls.Add(this._frameRight);
			this.Controls.Add(this._frameTopRight);
			this.Controls.Add(this._frameBottom);
			this.Controls.Add(this._frameTop);
			this.Controls.Add(this._frameBottomLeft);
			this.Controls.Add(this._frameLeft);
			this.Controls.Add(this._frameTopLeft);
			this.Controls.Add(this._labelTitle);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("HGPｺﾞｼｯｸM", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ClapteFormBase";
			this.Text = "ClapteFormBase";
			this.Load += new System.EventHandler(this.ClapteFormBase_Load);
			((System.ComponentModel.ISupportInitialize)(this._btnOption)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnClose)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnTopMost)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._picTitleBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnOK)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._btnCancel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._frameBottomRight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._frameRight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._frameTopRight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._frameBottom)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._frameTop)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._frameBottomLeft)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._frameLeft)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._frameTopLeft)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Components.ChameleonPictureBox _frameTopLeft;
		private Components.ChameleonPictureBox _frameLeft;
		private Components.ChameleonPictureBox _frameBottomLeft;
		private Components.ChameleonPictureBox _frameTop;
		private Components.ChameleonPictureBox _frameBottom;
		private Components.ChameleonPictureBox _frameTopRight;
		private Components.ChameleonPictureBox _frameRight;
		private Components.ChameleonPictureBox _frameBottomRight;
		private Components.ImageButton _btnOK;
		private Components.ImageButton _btnCancel;
		private Components.ImageButton _btnTopMost;
		private Components.ImageButton _btnClose;
		private System.Windows.Forms.Label _labelTitle;
		private Components.ChameleonPictureBox _picTitleBar;
		private Components.ImageButton _btnOption;
		protected System.Windows.Forms.ToolTip _toolHelpTip;


	}
}
