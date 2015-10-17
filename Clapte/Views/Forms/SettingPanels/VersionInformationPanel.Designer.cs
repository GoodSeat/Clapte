namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    partial class VersionInformationPanel
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
            this._labelTitle = new System.Windows.Forms.Label();
            this._labelRights = new System.Windows.Forms.Label();
            this._labelVersion = new System.Windows.Forms.Label();
            this._labelDate = new System.Windows.Forms.Label();
            this._linkMail = new System.Windows.Forms.LinkLabel();
            this._linkHP = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // _labelTitle
            // 
            this._labelTitle.AutoSize = true;
            this._labelTitle.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelTitle.Location = new System.Drawing.Point(11, 10);
            this._labelTitle.Name = "_labelTitle";
            this._labelTitle.Size = new System.Drawing.Size(92, 31);
            this._labelTitle.TabIndex = 0;
            this._labelTitle.Text = "Clapte";
            // 
            // _labelRights
            // 
            this._labelRights.AutoSize = true;
            this._labelRights.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelRights.Location = new System.Drawing.Point(48, 49);
            this._labelRights.Name = "_labelRights";
            this._labelRights.Size = new System.Drawing.Size(234, 15);
            this._labelRights.TabIndex = 1;
            this._labelRights.Text = "Copyright (C) 2012 Iiseki All Rights Reserved.";
            // 
            // _labelVersion
            // 
            this._labelVersion.AutoSize = true;
            this._labelVersion.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelVersion.Location = new System.Drawing.Point(102, 22);
            this._labelVersion.Name = "_labelVersion";
            this._labelVersion.Size = new System.Drawing.Size(67, 19);
            this._labelVersion.TabIndex = 2;
            this._labelVersion.Text = "ver 1.0.0";
            // 
            // _labelDate
            // 
            this._labelDate.AutoSize = true;
            this._labelDate.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelDate.Location = new System.Drawing.Point(181, 25);
            this._labelDate.Name = "_labelDate";
            this._labelDate.Size = new System.Drawing.Size(63, 15);
            this._labelDate.TabIndex = 3;
            this._labelDate.Text = "R20130102";
            // 
            // _linkMail
            // 
            this._linkMail.AutoSize = true;
            this._linkMail.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._linkMail.Location = new System.Drawing.Point(18, 113);
            this._linkMail.Name = "_linkMail";
            this._linkMail.Size = new System.Drawing.Size(128, 15);
            this._linkMail.TabIndex = 4;
            this._linkMail.TabStop = true;
            this._linkMail.Text = "GoodSeat@hotmail.co.jp";
            this._linkMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkMail_LinkClicked);
            // 
            // _linkHP
            // 
            this._linkHP.AutoSize = true;
            this._linkHP.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._linkHP.Location = new System.Drawing.Point(18, 93);
            this._linkHP.Name = "_linkHP";
            this._linkHP.Size = new System.Drawing.Size(251, 15);
            this._linkHP.TabIndex = 5;
            this._linkHP.TabStop = true;
            this._linkHP.Text = "https://sites.google.com/site/eatbaconandham/home";
            this._linkHP.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkHP_LinkClicked);
            // 
            // VersionInformationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._linkHP);
            this.Controls.Add(this._linkMail);
            this.Controls.Add(this._labelDate);
            this.Controls.Add(this._labelVersion);
            this.Controls.Add(this._labelRights);
            this.Controls.Add(this._labelTitle);
            this.Name = "VersionInformationPanel";
            this.Size = new System.Drawing.Size(339, 261);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _labelTitle;
        private System.Windows.Forms.Label _labelRights;
        private System.Windows.Forms.Label _labelVersion;
        private System.Windows.Forms.Label _labelDate;
        private System.Windows.Forms.LinkLabel _linkMail;
        private System.Windows.Forms.LinkLabel _linkHP;
    }
}
