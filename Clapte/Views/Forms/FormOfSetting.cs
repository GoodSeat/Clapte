using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GoodSeat.Sio.Xml;
using GoodSeat.Clapte.Views.Forms.SettingPanels;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Constants;

namespace GoodSeat.Clapte.Views.Forms
{
    /// <summary>
    /// Clapteの設定画面を表します。
    /// </summary>
    public partial class FormOfSetting : ClapteFormBase
    {
        /// <summary>
        /// Clapteの設定画面を初期化します。
        /// </summary>
        private FormOfSetting()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clapteの設定画面を初期化します。
        /// </summary>
        /// <param name="clapteWatcher"></param>
        public FormOfSetting(FormOfMain owner)
        {
            InitializeComponent();
            OwnerMainForm = owner;

            _treeList.ExpandAll();
            _treeList.SelectedNode = _treeList.Nodes[0];
        }

        /// <summary>
        /// 親となるメインフォームを設定もしくは取得します。
        /// </summary>
        FormOfMain OwnerMainForm { get; set; }

        /// <summary>
        /// 読み込み時の設定復元用のXmlエレメントを設定もしくは取得します。
        /// </summary>
        XmlElement BackUpXml { get; set; }

        /// <summary>
        /// 読み込み時、復元用Xmlを作成します。
        /// </summary>
        private void FormOfSetting_Load(object sender, EventArgs e)
        {
            BackUpXml = OwnerMainForm.CreateSerializeXmlElement();
        }
        
        /// <summary>
        /// ノードタイトルから対応する設定パネルを生成して返します。
        /// </summary>
        /// <param name="title">ノードタイトル。</param>
        /// <returns>ノードタイトルに対応する設定パネル。</returns>
        SettingPanel CreateSettingPanel(string title)
        {
            switch (title)
            {
                case "一般設定":
                    return new GeneralSettingPanel(OwnerMainForm.ClapteCore, OwnerMainForm, OwnerMainForm.HotkeyManager);
                case "監視対象外":
                    return new ExcludeListSettingPanel(OwnerMainForm.ClapteCore);
                case "定数":
                    return new UserConstantPanel(OwnerMainForm.UserConstants, OwnerMainForm.ClapteCore);
                case "関数":
                    return new UserFunctionPanel(OwnerMainForm.UserFunctions, OwnerMainForm.ClapteCore);
                case "単位換算表":
                    return new UnitConvertTablePanel();
                case "区切り数値集計":
                    return new SplitDataCounterPanel(OwnerMainForm.ClapteCore.SplitDataCountCommand);
                case "計算機":
                    return new CalculatorSettingPanel(OwnerMainForm.ClaptePadView);
                case "計算":
                    return new CalculateSettingPanel(OwnerMainForm.ClapteCore);
                case "バージョン情報":
                    return new VersionInformationPanel();
            }
            return null;
        }

        /// <summary>
        /// 指定数式の定義の設定を開きます。
        /// </summary>
        /// <param name="target">定義を参照する数式。</param>
        public void OpenDefine(Formula target)
        {
            if (target is Unit) _treeList.SelectedNode = _treeList.Nodes[3];
            else if (target is Variable || target is Constant) _treeList.SelectedNode = _treeList.Nodes[1];
            else if (target is Function) _treeList.SelectedNode = _treeList.Nodes[2];

            if (!(_treeList.SelectedNode.Tag as SettingPanel).OpenDefine(target))
                MessageBox.Show(target.ToString() + "は定義されていません。", "定義の参照", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        

        private void _treeList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = _treeList.SelectedNode;

            if (selectedNode.Tag == null) selectedNode.Tag = CreateSettingPanel(selectedNode.Text);
            if (selectedNode.Tag == null) return; // 未実装です。

            _containerAll.Panel2.Controls.Clear();

            Control panel = selectedNode.Tag as Control;
            _containerAll.Panel2.Controls.Add(panel);
            panel.Dock = DockStyle.Fill;
        }

        protected override void OnOK(EventArgs e)
        {
            foreach (TreeNode node in GetAllNodes())
            {
                SettingPanel panel = node.Tag as SettingPanel;
                if (panel == null) continue;

                panel.OnDeterminSetting();
            }
            OwnerMainForm.ClapteCore.Solver.UpdateSetting();
            this.Close();
        }

        protected override void OnCancel(EventArgs e)
        {
            foreach (TreeNode node in GetAllNodes())
            {
                SettingPanel panel = node.Tag as SettingPanel;
                if (panel == null) continue;

                panel.OnCancelSetting();
            }
            OwnerMainForm.RestoreFromXmlElement(BackUpXml);
            this.Close();
        }


        /// <summary>
        /// 設定項目に存在する、子を含むすべてのノードを返す反復子を取得します。
        /// </summary>
        /// <returns>すべてのノードを返す反復子。</returns>
        IEnumerable<TreeNode> GetAllNodes()
        {
            foreach (TreeNode node in GetAllNodes(_treeList.Nodes)) yield return node;
        }

        /// <summary>
        /// 指定ノードのすべて子孫ノードを返す反復子を取得します。
        /// </summary>
        /// <param name="nodes">起点とするノード。</param>
        /// <returns>すべてのノードを返す反復子。</returns>
        IEnumerable<TreeNode> GetAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                yield return node;
                foreach (TreeNode child in GetAllNodes(node.Nodes)) yield return child;
            }
        }


    }
}
