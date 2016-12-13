namespace GoodSeat.Clapte.Views.Forms
{
    partial class FormOfSetting
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("計算");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("計算機");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("監視対象外");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("区切り数値集計");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("一般設定", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("定数");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("関数");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("単位変換表");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("バージョン情報");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOfSetting));
            this._treeList = new System.Windows.Forms.TreeView();
            this._containerAll = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this._containerAll)).BeginInit();
            this._containerAll.Panel1.SuspendLayout();
            this._containerAll.SuspendLayout();
            this.SuspendLayout();
            // 
            // _treeList
            // 
            this._treeList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._treeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeList.FullRowSelect = true;
            this._treeList.HideSelection = false;
            this._treeList.ItemHeight = 18;
            this._treeList.Location = new System.Drawing.Point(0, 0);
            this._treeList.Name = "_treeList";
            treeNode1.Name = "_nodeCalculate";
            treeNode1.Text = "計算";
            treeNode2.Name = "_nodeCalculator";
            treeNode2.Text = "計算機";
            treeNode3.Name = "_nodeExclude";
            treeNode3.Text = "監視対象外";
            treeNode4.Name = "_nodeSplitData";
            treeNode4.Text = "区切り数値集計";
            treeNode5.Name = "_nodeGeneral";
            treeNode5.Text = "一般設定";
            treeNode6.Name = "_nodeConstant";
            treeNode6.Text = "定数";
            treeNode7.Name = "_nodeFunction";
            treeNode7.Text = "関数";
            treeNode8.Name = "_nodeConvertUnit";
            treeNode8.Text = "単位変換表";
            treeNode9.Name = "_nodeVersion";
            treeNode9.Text = "バージョン情報";
            this._treeList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9});
            this._treeList.ShowRootLines = false;
            this._treeList.Size = new System.Drawing.Size(120, 415);
            this._treeList.TabIndex = 16;
            this._treeList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._treeList_AfterSelect);
            // 
            // _containerAll
            // 
            this._containerAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._containerAll.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._containerAll.Location = new System.Drawing.Point(16, 38);
            this._containerAll.Name = "_containerAll";
            // 
            // _containerAll.Panel1
            // 
            this._containerAll.Panel1.Controls.Add(this._treeList);
            this._containerAll.Size = new System.Drawing.Size(546, 415);
            this._containerAll.SplitterDistance = 120;
            this._containerAll.TabIndex = 17;
            // 
            // FormOfSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(574, 485);
            this.Controls.Add(this._containerAll);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(250, 200);
            this.Name = "FormOfSetting";
            this.ShowCancelButton = true;
            this.ShowOKButton = true;
            this.ShowOption = true;
            this.Text = "Clapteの設定";
            this.Load += new System.EventHandler(this.FormOfSetting_Load);
            this.Controls.SetChildIndex(this._containerAll, 0);
            this._containerAll.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._containerAll)).EndInit();
            this._containerAll.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView _treeList;
        private System.Windows.Forms.SplitContainer _containerAll;
    }
}
