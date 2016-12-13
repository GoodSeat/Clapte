namespace GoodSeat.Clapte.Views.Forms
{
    partial class FormOfMain
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
            _clipBoradWatcher.Dispose();
            _hotkeyManager.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOfMain));
            this._notifyIconClapte = new System.Windows.Forms.NotifyIcon(this.components);
            this._menuClapte = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._menuEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._menuConsiderValid = new System.Windows.Forms.ToolStripMenuItem();
            this._menuMode = new System.Windows.Forms.ToolStripMenuItem();
            this._menuOmitPower = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._menuSetting = new System.Windows.Forms.ToolStripMenuItem();
            this._menuCalculator = new System.Windows.Forms.ToolStripMenuItem();
            this._menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this._menuClapte.SuspendLayout();
            this.SuspendLayout();
            // 
            // _notifyIconClapte
            // 
            this._notifyIconClapte.ContextMenuStrip = this._menuClapte;
            this._notifyIconClapte.Icon = ((System.Drawing.Icon)(resources.GetObject("_notifyIconClapte.Icon")));
            this._notifyIconClapte.Text = "Clapte";
            this._notifyIconClapte.Visible = true;
            this._notifyIconClapte.BalloonTipClicked += new System.EventHandler(this._notifyIconClapte_BalloonTipClicked);
            this._notifyIconClapte.Click += new System.EventHandler(this._notifyIconClapte_Click);
            this._notifyIconClapte.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._notifyIconClapte_MouseDoubleClick);
            // 
            // _menuClapte
            // 
            this._menuClapte.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuEnable,
            this.toolStripSeparator2,
            this._menuConsiderValid,
            this._menuMode,
            this._menuOmitPower,
            this.toolStripSeparator3,
            this._menuSetting,
            this._menuCalculator,
            this._menuHelp,
            this.toolStripSeparator1,
            this._menuExit});
            this._menuClapte.Name = "_menuClapte";
            this._menuClapte.Size = new System.Drawing.Size(182, 198);
            this._menuClapte.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this._menuClapte_Closing);
            this._menuClapte.Opening += new System.ComponentModel.CancelEventHandler(this._menuClapte_Opening);
            // 
            // _menuEnable
            // 
            this._menuEnable.Checked = true;
            this._menuEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this._menuEnable.Name = "_menuEnable";
            this._menuEnable.Size = new System.Drawing.Size(181, 22);
            this._menuEnable.Text = "有効(&A)";
            this._menuEnable.Click += new System.EventHandler(this._menuEnable_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(178, 6);
            // 
            // _menuConsiderValid
            // 
            this._menuConsiderValid.Name = "_menuConsiderValid";
            this._menuConsiderValid.Size = new System.Drawing.Size(181, 22);
            this._menuConsiderValid.Text = "有効数字考慮(&V)";
            this._menuConsiderValid.Click += new System.EventHandler(this._menuConsiderValid_Click);
            // 
            // _menuMode
            // 
            this._menuMode.Name = "_menuMode";
            this._menuMode.Size = new System.Drawing.Size(181, 22);
            this._menuMode.Text = "分数計算モード(&F)";
            this._menuMode.Click += new System.EventHandler(this._menuMode_Click);
            // 
            // _menuOmitPower
            // 
            this._menuOmitPower.Name = "_menuOmitPower";
            this._menuOmitPower.Size = new System.Drawing.Size(181, 22);
            this._menuOmitPower.Text = "^ の省略を許可(&O)";
            this._menuOmitPower.Click += new System.EventHandler(this._menuOmitPower_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(178, 6);
            // 
            // _menuSetting
            // 
            this._menuSetting.Name = "_menuSetting";
            this._menuSetting.Size = new System.Drawing.Size(181, 22);
            this._menuSetting.Text = "設定(&S)";
            this._menuSetting.Click += new System.EventHandler(this._menuSetting_Click);
            // 
            // _menuCalculator
            // 
            this._menuCalculator.Name = "_menuCalculator";
            this._menuCalculator.Size = new System.Drawing.Size(181, 22);
            this._menuCalculator.Text = "計算機(&C)";
            this._menuCalculator.Click += new System.EventHandler(this._menuCalculator_Click);
            // 
            // _menuHelp
            // 
            this._menuHelp.Name = "_menuHelp";
            this._menuHelp.Size = new System.Drawing.Size(181, 22);
            this._menuHelp.Text = "ヘルプ(&H)";
            this._menuHelp.Visible = false;
            this._menuHelp.Click += new System.EventHandler(this._menuHelp_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // _menuExit
            // 
            this._menuExit.Name = "_menuExit";
            this._menuExit.Size = new System.Drawing.Size(181, 22);
            this._menuExit.Text = "終了(&X)";
            this._menuExit.Click += new System.EventHandler(this._menuExit_Click);
            // 
            // FormOfMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 37);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormOfMain";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Clapte_FormOfMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOfMain_FormClosing);
            this.Load += new System.EventHandler(this.FormOfMain_Load);
            this._menuClapte.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon _notifyIconClapte;
        private System.Windows.Forms.ContextMenuStrip _menuClapte;
        private System.Windows.Forms.ToolStripMenuItem _menuSetting;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem _menuExit;
        private System.Windows.Forms.ToolStripMenuItem _menuCalculator;
        private System.Windows.Forms.ToolStripMenuItem _menuEnable;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem _menuHelp;
        private System.Windows.Forms.ToolStripMenuItem _menuConsiderValid;
        private System.Windows.Forms.ToolStripMenuItem _menuMode;
        private System.Windows.Forms.ToolStripMenuItem _menuOmitPower;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}
