namespace GoodSeat.Clapte.Views.Forms
{
	partial class FormOfCalculator
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOfCalculator));
//			this._textBoxInput = new GoodSeat.Liffom.Intellisence.Formats.FormulaRichTextBox(this.components);
			this._menuInput = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._menuUndo = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._menuCut = new System.Windows.Forms.ToolStripMenuItem();
			this._menuCopy = new System.Windows.Forms.ToolStripMenuItem();
			this._menuPaste = new System.Windows.Forms.ToolStripMenuItem();
			this._menuDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this._menuOpenDefine = new System.Windows.Forms.ToolStripMenuItem();
			this._textBoxAnswer = new System.Windows.Forms.TextBox();
			this._btnExe = new Clapte.Views.Components.ImageButton(this.components);
			this._imgInfo = new System.Windows.Forms.PictureBox();
			this._toolTipBalloon = new System.Windows.Forms.ToolTip(this.components);
			this._menuRedo = new System.Windows.Forms.ToolStripMenuItem();
			this._menuInput.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._btnExe)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._imgInfo)).BeginInit();
			this.SuspendLayout();
			// 
			// _textBoxInput
			// 
			this._textBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._textBoxInput.BackColor = System.Drawing.Color.White;
			this._textBoxInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._textBoxInput.ContextMenuStrip = this._menuInput;
//			this._textBoxInput.DetectUrls = false;
			this._textBoxInput.HideSelection = false;
			this._textBoxInput.Location = new System.Drawing.Point(18, 20);
			this._textBoxInput.Multiline = false;
			this._textBoxInput.Name = "_textBoxInput";
//			this._textBoxInput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this._textBoxInput.Size = new System.Drawing.Size(236, 22);
			this._textBoxInput.TabIndex = 16;
			this._textBoxInput.Text = "";
			this._textBoxInput.Enter += new System.EventHandler(this._textBoxInput_Enter);
			this._textBoxInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this._textBoxInput_KeyDown);
			// 
			// _menuInput
			// 
			this._menuInput.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuUndo,
            this._menuRedo,
            this.toolStripSeparator1,
            this._menuCut,
            this._menuCopy,
            this._menuPaste,
            this._menuDelete,
            this.toolStripSeparator2,
            this._menuOpenDefine});
			this._menuInput.Name = "_menuInput";
			this._menuInput.Size = new System.Drawing.Size(183, 192);
			this._menuInput.Opening += new System.ComponentModel.CancelEventHandler(this._menuInput_Opening);
			// 
			// _menuUndo
			// 
			this._menuUndo.Name = "_menuUndo";
			this._menuUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this._menuUndo.Size = new System.Drawing.Size(182, 22);
			this._menuUndo.Text = "元に戻す(&U)";
			this._menuUndo.Click += new System.EventHandler(this._menuUndo_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(179, 6);
			// 
			// _menuCut
			// 
			this._menuCut.Name = "_menuCut";
			this._menuCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this._menuCut.Size = new System.Drawing.Size(182, 22);
			this._menuCut.Text = "切り取り(&T)";
			this._menuCut.Click += new System.EventHandler(this._menuCut_Click);
			// 
			// _menuCopy
			// 
			this._menuCopy.Name = "_menuCopy";
			this._menuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this._menuCopy.Size = new System.Drawing.Size(182, 22);
			this._menuCopy.Text = "コピー(&C)";
			this._menuCopy.Click += new System.EventHandler(this._menuCopy_Click);
			// 
			// _menuPaste
			// 
			this._menuPaste.Name = "_menuPaste";
			this._menuPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this._menuPaste.Size = new System.Drawing.Size(182, 22);
			this._menuPaste.Text = "貼り付け(&P)";
			this._menuPaste.Click += new System.EventHandler(this._menuPaste_Click);
			// 
			// _menuDelete
			// 
			this._menuDelete.Name = "_menuDelete";
			this._menuDelete.Size = new System.Drawing.Size(182, 22);
			this._menuDelete.Text = "削除(&D)";
			this._menuDelete.Click += new System.EventHandler(this._menuDelete_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(179, 6);
			// 
			// _menuOpenDefine
			// 
			this._menuOpenDefine.Name = "_menuOpenDefine";
			this._menuOpenDefine.ShortcutKeys = System.Windows.Forms.Keys.F12;
			this._menuOpenDefine.Size = new System.Drawing.Size(182, 22);
			this._menuOpenDefine.Text = "定義を参照(&J)";
			this._menuOpenDefine.Click += new System.EventHandler(this._menuOpenDefine_Click);
			// 
			// _textBoxAnswer
			// 
			this._textBoxAnswer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._textBoxAnswer.BackColor = System.Drawing.Color.White;
			this._textBoxAnswer.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._textBoxAnswer.Location = new System.Drawing.Point(56, 67);
			this._textBoxAnswer.Name = "_textBoxAnswer";
			this._textBoxAnswer.ReadOnly = true;
			this._textBoxAnswer.Size = new System.Drawing.Size(192, 12);
			this._textBoxAnswer.TabIndex = 17;
			// 
			// _btnExe
			// 
			this._btnExe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._btnExe.Cursor = System.Windows.Forms.Cursors.Hand;
			this._btnExe.DownMove = 1;
			this._btnExe.FocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_Exe;
			this._btnExe.Image = global::GoodSeat.Clapte.Properties.Resources.Image_Exe_Unfocus;
			this._btnExe.Location = new System.Drawing.Point(7, 61);
			this._btnExe.Name = "_btnExe";
			this._btnExe.Size = new System.Drawing.Size(48, 21);
			this._btnExe.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._btnExe.TabIndex = 18;
			this._btnExe.TabStop = false;
			this._btnExe.UnFocusImage = global::GoodSeat.Clapte.Properties.Resources.Image_Exe_Unfocus;
			this._btnExe.Click += new System.EventHandler(this._btnExe_Click);
			// 
			// _imgInfo
			// 
			this._imgInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._imgInfo.Image = global::GoodSeat.Clapte.Properties.Resources.Icon_Info;
			this._imgInfo.Location = new System.Drawing.Point(262, 64);
			this._imgInfo.Name = "_imgInfo";
			this._imgInfo.Size = new System.Drawing.Size(16, 16);
			this._imgInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._imgInfo.TabIndex = 19;
			this._imgInfo.TabStop = false;
			this._imgInfo.Visible = false;
			// 
			// _toolTipBalloon
			// 
			this._toolTipBalloon.BackColor = System.Drawing.Color.White;
			this._toolTipBalloon.IsBalloon = true;
			// 
			// _menuRedo
			// 
			this._menuRedo.Name = "_menuRedo";
			this._menuRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this._menuRedo.Size = new System.Drawing.Size(182, 22);
			this._menuRedo.Text = "やり直す(&R)";
			this._menuRedo.Click += new System.EventHandler(this._menuRedo_Click);
			// 
			// FormOfCalculator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(281, 86);
			this.Controls.Add(this._textBoxInput);
			this.Controls.Add(this._imgInfo);
			this.Controls.Add(this._btnExe);
			this.Controls.Add(this._textBoxAnswer);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(100, 70);
			this.Name = "FormOfCalculator";
			this.ShowCancelButton = true;
			this.ShowInTaskbar = false;
			this.ShowOKButton = true;
			this.ShowOption = true;
			this.Text = "計算機";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOfCalculator_FormClosing);
			this.Load += new System.EventHandler(this.FormOfCalculator_Load);
			this.Enter += new System.EventHandler(this.FormOfCalculator_Enter);
			this.Controls.SetChildIndex(this._textBoxAnswer, 0);
			this.Controls.SetChildIndex(this._btnExe, 0);
			this.Controls.SetChildIndex(this._imgInfo, 0);
			this.Controls.SetChildIndex(this._textBoxInput, 0);
			this._menuInput.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._btnExe)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._imgInfo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox _textBoxInput;
//		private GoodSeat.Liffom.Intellisence.Formats.FormulaRichTextBox _textBoxInput;
		private System.Windows.Forms.TextBox _textBoxAnswer;
		private Components.ImageButton _btnExe;
		private System.Windows.Forms.PictureBox _imgInfo;
		private System.Windows.Forms.ToolTip _toolTipBalloon;
		private System.Windows.Forms.ContextMenuStrip _menuInput;
		private System.Windows.Forms.ToolStripMenuItem _menuUndo;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem _menuCut;
		private System.Windows.Forms.ToolStripMenuItem _menuCopy;
		private System.Windows.Forms.ToolStripMenuItem _menuPaste;
		private System.Windows.Forms.ToolStripMenuItem _menuDelete;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem _menuOpenDefine;
		private System.Windows.Forms.ToolStripMenuItem _menuRedo;
	}
}
