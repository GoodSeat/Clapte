namespace GoodSeat.Liffom.Utilities
{
    partial class TestForm
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
            this._treeFormulas = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._btnTreat = new System.Windows.Forms.Button();
            this._listTreat = new System.Windows.Forms.ListBox();
            this._treeFormulaConsist = new System.Windows.Forms.TreeView();
            this._txtBoxInput = new System.Windows.Forms.TextBox();
            this._btnAdd = new System.Windows.Forms.Button();
            this._labelError = new System.Windows.Forms.Label();
            this._txtBoxArgument = new System.Windows.Forms.TextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _treeFormulas
            // 
            this._treeFormulas.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeFormulas.Location = new System.Drawing.Point(0, 0);
            this._treeFormulas.Name = "_treeFormulas";
            this._treeFormulas.Size = new System.Drawing.Size(195, 405);
            this._treeFormulas.TabIndex = 0;
            this._treeFormulas.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._treeFormulas_AfterSelect);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._treeFormulas);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._txtBoxArgument);
            this.splitContainer1.Panel2.Controls.Add(this._labelError);
            this.splitContainer1.Panel2.Controls.Add(this._btnAdd);
            this.splitContainer1.Panel2.Controls.Add(this._txtBoxInput);
            this.splitContainer1.Panel2.Controls.Add(this._treeFormulaConsist);
            this.splitContainer1.Panel2.Controls.Add(this._listTreat);
            this.splitContainer1.Panel2.Controls.Add(this._btnTreat);
            this.splitContainer1.Size = new System.Drawing.Size(456, 405);
            this.splitContainer1.SplitterDistance = 195;
            this.splitContainer1.TabIndex = 1;
            // 
            // _btnTreat
            // 
            this._btnTreat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnTreat.Location = new System.Drawing.Point(179, 102);
            this._btnTreat.Name = "_btnTreat";
            this._btnTreat.Size = new System.Drawing.Size(75, 23);
            this._btnTreat.TabIndex = 0;
            this._btnTreat.Text = "実行";
            this._btnTreat.UseVisualStyleBackColor = true;
            this._btnTreat.Click += new System.EventHandler(this._btnTreat_Click);
            // 
            // _listTreat
            // 
            this._listTreat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._listTreat.FormattingEnabled = true;
            this._listTreat.ItemHeight = 12;
            this._listTreat.Location = new System.Drawing.Point(5, 49);
            this._listTreat.MultiColumn = true;
            this._listTreat.Name = "_listTreat";
            this._listTreat.Size = new System.Drawing.Size(170, 76);
            this._listTreat.TabIndex = 1;
            // 
            // _treeFormulaConsist
            // 
            this._treeFormulaConsist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._treeFormulaConsist.Location = new System.Drawing.Point(5, 133);
            this._treeFormulaConsist.Name = "_treeFormulaConsist";
            this._treeFormulaConsist.Size = new System.Drawing.Size(249, 269);
            this._treeFormulaConsist.TabIndex = 3;
            // 
            // _txtBoxInput
            // 
            this._txtBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._txtBoxInput.Location = new System.Drawing.Point(5, 4);
            this._txtBoxInput.Name = "_txtBoxInput";
            this._txtBoxInput.Size = new System.Drawing.Size(171, 19);
            this._txtBoxInput.TabIndex = 4;
            // 
            // _btnAdd
            // 
            this._btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnAdd.Location = new System.Drawing.Point(179, 2);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size(75, 23);
            this._btnAdd.TabIndex = 5;
            this._btnAdd.Text = "追加";
            this._btnAdd.UseVisualStyleBackColor = true;
            this._btnAdd.Click += new System.EventHandler(this._btnAdd_Click);
            // 
            // _labelError
            // 
            this._labelError.AutoSize = true;
            this._labelError.Location = new System.Drawing.Point(5, 28);
            this._labelError.Name = "_labelError";
            this._labelError.Size = new System.Drawing.Size(0, 12);
            this._labelError.TabIndex = 6;
            // 
            // _txtBoxArgument
            // 
            this._txtBoxArgument.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._txtBoxArgument.Location = new System.Drawing.Point(181, 49);
            this._txtBoxArgument.Name = "_txtBoxArgument";
            this._txtBoxArgument.Size = new System.Drawing.Size(73, 19);
            this._txtBoxArgument.TabIndex = 7;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 429);
            this.Controls.Add(this.splitContainer1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView _treeFormulas;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView _treeFormulaConsist;
        private System.Windows.Forms.ListBox _listTreat;
        private System.Windows.Forms.Button _btnTreat;
        private System.Windows.Forms.Button _btnAdd;
        private System.Windows.Forms.TextBox _txtBoxInput;
        private System.Windows.Forms.Label _labelError;
        private System.Windows.Forms.TextBox _txtBoxArgument;
    }
}
