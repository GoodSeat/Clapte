// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Utilities
{
    public partial class TestForm : Form
    {
        // テスト処理のタイプ
        enum TreatType
        {
            TidyUp,
            Expand,
            Numerate,
            Calculate,
            Organize
        }

        public TestForm()
        {
            InitializeComponent();

            foreach (TreatType type in Enum.GetValues(typeof(TreatType)))
                _listTreat.Items.Add(type);
        }


        // 新規数式の追加
        private void _btnAdd_Click(object sender, EventArgs e)
        {
            string formulaText = _txtBoxInput.Text;
            _labelError.Text = "";

            try
            {
                Formula formula = Formula.Parse(formulaText);

                TreeNode formulaNode = MakeFormulaNode(formula);
                _treeFormulas.Nodes.Add(formulaNode);

                _treeFormulas.SelectedNode = formulaNode;
            }
            catch (Exception exc)
            {
                _labelError.Text = exc.Message;
            }
        }

        // 処理の実行
        private void _btnTreat_Click(object sender, EventArgs e)
        {
            if (_listTreat.SelectedItem == null) return;
            TreatType type = (TreatType)_listTreat.SelectedItem;

            TreeNode selectedNode = _treeFormulas.SelectedNode;
            if (selectedNode == null) return;
            Formula f = selectedNode.Tag as Formula;

            Formula result = Treat(f, type);

            TreeNode newNode = MakeFormulaNode(result);
            selectedNode.Nodes.Add(newNode);
            _treeFormulas.SelectedNode = newNode;
        }

        // 数式選択時
        private void _treeFormulas_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = _treeFormulas.SelectedNode;
            if (selectedNode == null)return;
            Formula f = selectedNode.Tag as Formula;

            _treeFormulaConsist.Nodes.Clear();

            TreeNode formulaNode = MakeFormulaNode(f);
            AddFormulaConsist(formulaNode, f);

            _treeFormulaConsist.Nodes.Add(formulaNode);
            _treeFormulaConsist.ExpandAll();
        }

        void AddFormulaConsist(TreeNode parentNode, Formula parent)
        {
            foreach (Formula f in parent)
            {
                TreeNode childNode = MakeFormulaNode(f);
                parentNode.Nodes.Add(childNode);

                AddFormulaConsist(childNode, f);
            }
        }

        TreeNode MakeFormulaNode(Formula f)
        {
            TreeNode formulaNode = new TreeNode(f.ToString());
            formulaNode.Tag = f;
            return formulaNode;
        }

        // 数式処理
        Formula Treat(Formula f, TreatType type)
        {
            Formula result = f.Copy();
            switch (type)
            {
                case TreatType.TidyUp: result = result.Combine(); break;
                case TreatType.Expand: result = result.Expand(); break;
                case TreatType.Calculate: result = result.Calculate(); break;
                case TreatType.Numerate: result = result.Numerate(); break;
                default: throw new NotImplementedException();
            }
            return result;
        }
    }
}
