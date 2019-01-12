using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Sio;
using GoodSeat.Clapte.ViewModels;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Sio.Utilities;

namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    /// <summary>
    /// ユーザー定義変数の設定パネルを表します。
    /// </summary>
    public partial class UserConstantPanel : SettingPanel
    {
//        DataGridIntellisenceSupport _formulaIntelisence;

        /// <summary>
        /// ユーザー定義変数の設定パネルを初期化します。
        /// </summary>
        /// <param name="clapteWatcher"></param>
        public UserConstantPanel(ConstantListViewModel target, ClapteCoreViewModel clapteCore)
        {
            InitializeComponent();

            Target = target;
            ClapteCore = clapteCore;
            IgnoreEvent = false;

            DownloadSetting();

            RegistEditGridView(_dataGridVariable, _labelError);

            _cmbFilter.SelectedIndex = 0;
            _cmbFilter.SelectedIndexChanged += new EventHandler(_cmbFilter_SelectedIndexChanged);

            this.Disposed += new EventHandler(UserConstantPanel_Disposed);
            Target.DefineAdded += new EventHandler<ListChangeEventArgs<ConstantDefine>>(Target_DefineAdded);
            Target.DefineRemoved += new EventHandler<ListChangeEventArgs<ConstantDefine>>(Target_DefineRemoved);
            Target.DefineChanged += new EventHandler<ListChangeEventArgs<ConstantDefine>>(Target_DefineChanged);

            // データグリッドビュー上の数式インテリセンス構成
//            _formulaIntelisence = new DataGridIntellisenceSupport(_dataGridVariable);
//            _formulaIntelisence.CellEndEdit += new DataGridViewCellEventHandler(_dataGrid_CellEndEdit);
        }

        /// <summary>
        /// 設定対象の定数定義管理ビューモデルを設定もしくは取得します。
        /// </summary>
        ConstantListViewModel Target { get; set; }

        /// <summary>
        /// 対象となるClapteCoreViewModelオブジェクトを設定もしくは取得します。
        /// </summary>
        ClapteCoreViewModel ClapteCore { get; set; }

        /// <summary>
        /// 定数定義変更時に、数式として解釈可能かを検査するか否かを設定もしくは取得します。
        /// </summary>
        private bool CheckDefineAsFormula { get; set; }

        /// <summary>
        /// 定数定義リストからの変更通知イベントを無視するか否かを設定もしくは取得します。
        /// </summary>
        private bool IgnoreEvent { get; set; }


        /// <summary>
        /// 設定からコントロールに状態を反映します。
        /// </summary>
        private void DownloadSetting()
        {
            if (IgnoreEvent) return;

            _dataGridVariable.Rows.Clear();

            foreach (ConstantDefine def in Target.GetSystemConstants())
                _dataGridVariable.Rows.Add(CreateVariableRow(def, true));
            foreach (ConstantDefine def in Target.Target)
                _dataGridVariable.Rows.Add(CreateVariableRow(def, false));
        }
        
        /// <summary>
        /// 定義の存在しない適当な定数名を取得します。
        /// </summary>
        /// <returns>定義の存在しない適当な定数名。</returns>
        private string GetNotExistConstantName()
        {
            int count = 1;
            string name = "NewVariable";
            bool existSame = false;
            do
            {
                existSame = false;
                name = "NewVariable" + (count++).ToString();
                foreach (DataGridViewRow rowCheck in _dataGridVariable.Rows)
                {
                    if (GetStringFrom(rowCheck.Cells[0].Value) == name)
                    {
                        existSame = true;
                        break;
                    }
                }
            }
            while (existSame);
            return name;
        }

        /// <summary>
        /// 指定定数定義を表すグリッドビューの行を生成して取得します。
        /// </summary>
        /// <param name="def">定数定義。</param>
        /// <param name="asSystem">システム定義か否か。</param>
        /// <returns>定数定義を表す行データ。</returns>
        DataGridViewRow CreateVariableRow(ConstantDefine def, bool asSystem)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.Tag = def;

            row.Cells.Add(new DataGridViewTextBoxCell());
            row.Cells.Add(new DataGridViewTextBoxCell());
            row.Cells.Add(new DataGridViewTextBoxCell());
            row.Cells[0].Value = def.Name;
            row.Cells[1].Value = def.Define;
            row.Cells[2].Value = def.Information;

            if (asSystem)
            {
                string name = "";
                foreach (string n in (def.Target as Constant).GetAllDistinguishedNames())
                    name += n + ", ";
                name = name.TrimEnd(' ', ',');

                row.Cells[0].Value = name;
                row.Cells[0].Style.BackColor = Color.WhiteSmoke;
                row.Cells[1].Style.BackColor = Color.WhiteSmoke;
                row.Cells[2].Style.BackColor = Color.WhiteSmoke;
                row.ReadOnly = true;
            }
            return row;
        }
    
    

        // 表示アイテムフィルターの変更
        void _cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in _dataGridVariable.Rows)
            {
                var def = row.Tag as ConstantDefine;

                if (def.Target is Constant) row.Visible = (_cmbFilter.SelectedIndex != 2);
                else row.Visible = (_cmbFilter.SelectedIndex != 1);
            }
        }

        // 編集開始時
        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                //TargetWatcher.Solver.RenewIntellisence(_formulaIntelisence.Intelisence, Core.Solver.IntellisenceType.All);
                //_formulaIntelisence.StartInputFormula(e);
            }
        }

        // 編集終了時
        protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
        {
            var target = (_dataGridVariable.Rows[e.RowIndex].Tag as ConstantDefine);

            // 名前の設定
            string newName = GetStringFrom(_dataGridVariable[0, e.RowIndex].Value);
            foreach (DataGridViewRow row in _dataGridVariable.Rows)
            {
                if (row.Index == e.RowIndex) continue;
                if (GetStringFrom(row.Cells[0].Value) == newName) throw new Exception("同名の変数が既に存在します。");
            }

            // 定数の定義
            string value = null;
            if (_dataGridVariable[1, e.RowIndex].Value == null) value = "0";
            else value = GetStringFrom(_dataGridVariable[1, e.RowIndex].Value);

            var parser = SolverViewModel.CreateFormulaParser(true, false, true, true);

            Formula f;
            bool result = parser.TryParse(newName, out f);
            if (!result || (!(f is Variable) && !(f is Unit))) 
            {
                throw new Exception("\"" + newName + "\"を変数名として使用することはできません。");
            }

            if (CheckDefineAsFormula) parser.Parse(value);

            // 設定
            target.Name = newName;
            target.Define = value;
            target.Information = GetStringFrom(_dataGridVariable[2, e.RowIndex].Value);
        }


        // 定数定義リストに定義が追加された時
        void Target_DefineAdded(object sender, ListChangeEventArgs<ConstantDefine> e)
        {
            DownloadSetting();
            if (e.Items.Length == 0) return;

            _dataGridVariable.ClearSelection();
            var item = e.Items[0];
            foreach (DataGridViewRow rowCheck in _dataGridVariable.Rows)
            {
                if (rowCheck.Tag == item)
                {
                    rowCheck.Selected = true;
                    break;
                }
            }
            if (!_dataGridVariable.Rows[_dataGridVariable.RowCount - 1].Displayed)
            {
                _dataGridVariable.FirstDisplayedScrollingRowIndex = _dataGridVariable.RowCount - 1 - _dataGridVariable.DisplayedRowCount(false);
            }
        }

        // 定数定義リストから定義が削除された時
        void Target_DefineRemoved(object sender, ListChangeEventArgs<ConstantDefine> e)
        {
            DownloadSetting();
        }

        // 定数定義リストの定義が変更された時
        void Target_DefineChanged(object sender, ListChangeEventArgs<ConstantDefine> e)
        {
            DownloadSetting();
        }



        // 新規変数の追加
        private void _btnAdd_Click(object sender, EventArgs e)
        {
            if (_cmbFilter.SelectedIndex == 1) _cmbFilter.SelectedIndex = 0;

            // 変数名探索
            string name = GetNotExistConstantName();

            ConstantDefine def = new ConstantDefine(name);
            def.Information = "ユーザー定義変数";
            def.Define = "0";

            Target.Target.Add(def);
        }

        // 選択変数の削除
        private void _btnDelete_Click(object sender, EventArgs e)
        {
            _labelError.Text = "削除対象の行ヘッダーをクリックして、行を選択してから削除してください。";
            _labelError.Visible = true;

            List<ConstantDefine> removeList = new List<ConstantDefine>();
            foreach (DataGridViewRow row in _dataGridVariable.SelectedRows)
            {
                _labelError.Visible = false;

                var def = row.Tag as ConstantDefine;
                if (def.Target is Constant)
                {
                    _labelError.Text = "システム定義の定数は削除できません。";
                    _labelError.Visible = true;
                    return;
                }

                _dataGridVariable.Rows.Remove(row);
                removeList.Add(def);
            }

            foreach (var def in removeList) Target.Target.Remove(def);
        }

        // 行削除
        private void _variableList_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
            _btnDelete_Click(sender, e);
        }

        // リソース破棄
        void UserConstantPanel_Disposed(object sender, EventArgs e)
        {
            Target.DefineAdded -= new EventHandler<ListChangeEventArgs<ConstantDefine>>(Target_DefineAdded);
            Target.DefineRemoved -= new EventHandler<ListChangeEventArgs<ConstantDefine>>(Target_DefineRemoved);
            Target.DefineChanged -= new EventHandler<ListChangeEventArgs<ConstantDefine>>(Target_DefineChanged);
//            _formulaIntelisence.Dispose();
        }

        // ソート時比較関数
        private void _dataGridVariable_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            e.SortResult = GetStringFrom(e.CellValue1).CompareTo(GetStringFrom(e.CellValue2));
        }

        // 定義参照受付
        public override bool OpenDefine(object target)
        {
            if (!(target is Formula)) return false;

            Formula targetVariable = target as Formula;

            _dataGridVariable.ClearSelection();
            foreach (DataGridViewRow row in _dataGridVariable.Rows)
            {
                var def = row.Tag as ConstantDefine;
                if (targetVariable == def.Target)
                {
                    row.Selected = true;
                    _dataGridVariable.FirstDisplayedScrollingRowIndex = row.Index;
                    return true;
                }
            }
            return base.OpenDefine(target);
        }

    }
}
