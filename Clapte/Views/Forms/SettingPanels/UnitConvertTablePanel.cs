using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Formats;
using GoodSeat.Liffom.Formats.Numerics;
using GoodSeat.Liffom.Formats.Powers;

namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    /// <summary>
    /// 単位変換表の設定パネルを表します。
    /// </summary>
    public partial class UnitConvertTablePanel : SettingPanel
    {
        UnitConvertTable _currentTable;
//        DataGridIntellisenceSupport _formulaIntelisence;

        /// <summary>
        /// 単位変換表の設定パネルを初期化します。
        /// </summary>
        /// <param name="clapteWatcher"></param>
        public UnitConvertTablePanel()
        {
            InitializeComponent();

            RegistEditGridView(_dataGridUnitTable, _labelError);

            foreach (KeyValuePair<string, UnitConvertTable> pair in UnitConvertTable.ValidTables)
                _cmbUnitType.Items.Add(pair.Key);

            if (_cmbUnitType.Items.Count != 0) _cmbUnitType.SelectedIndex = 0;
            else _groupUnitType.Enabled = false;

//            _formulaIntelisence = new DataGridIntellisenceSupport(_dataGridUnitTable);
//            _formulaIntelisence.CellEndEdit += new DataGridViewCellEventHandler(_dataGrid_CellEndEdit);
        }

        /// <summary>
        /// 単位変換表を全て更新します。
        /// </summary>
        void RenewTable()
        {
            _txtUnitTypeComment.Text = _currentTable.TypeComment;

            // 列数の更新
            int columnCount = 2 + CountOfCurrentRecord * 2;
            while (_dataGridUnitTable.Columns.Count != columnCount)
            {
                if (_dataGridUnitTable.Columns.Count > columnCount)
                    _dataGridUnitTable.Columns.RemoveAt(_dataGridUnitTable.Columns.Count - 1);
                else
                {
                    DataGridViewColumn column;
                    if (_dataGridUnitTable.Columns.Count != 0) column = new DataGridViewColumn(_dataGridUnitTable.Columns[0].CellTemplate);
                    else column = new DataGridViewColumn();
                    column.Width = 60;
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    _dataGridUnitTable.Columns.Add(column);
                }
            }
            // 列名の更新
            {
                int columnNo = 2;
                foreach (UnitConvertRecord dataConvert in _currentTable.GetAllRecords(true))
                {
                    DataGridViewColumn column =_dataGridUnitTable.Columns[columnNo++];
                    column.HeaderText = "/ " + dataConvert.ConvertUnit.ToString().Replace("[", "").Replace("]", "");
                    column.Tag = dataConvert;
                }
                foreach (UnitConvertRecord dataConvert in _currentTable.GetAllRecords(true))
                {
                    DataGridViewColumn column = _dataGridUnitTable.Columns[columnNo++];
                    column.HeaderText = "+ " + dataConvert.ConvertUnit.ToString().Replace("[", "").Replace("]", "");
                    column.Tag = dataConvert;
                }
            }

            _dataGridUnitTable.Rows.Clear();
            foreach (UnitConvertRecord data in _currentTable.GetAllRecords(true))
            {
                DataGridViewRow row = new DataGridViewRow();
                row.Tag = data;
                for (int i = 0; i < _dataGridUnitTable.Columns.Count; i++)
                {
                    row.Cells.Add(new DataGridViewTextBoxCell());
                    if (data is BaseUnitConvertRecord) row.Cells[i].Style.BackColor = Color.LightGoldenrodYellow;
                }
                row.Cells[0].Value = data.ConvertUnit.ToString().TrimStart('[').TrimEnd(']');
                row.Cells[1].Value = data.UnitComment;
                _dataGridUnitTable.Rows.Add(row);
            }

            RefreshConvertTable();
        }

        /// <summary>
        /// カレントテーブルの基準単位を含むレコード数を取得します。
        /// </summary>
        /// <returns></returns>
        int CountOfCurrentRecord
        {
            get
            {
                int count = 0;
                foreach (UnitConvertRecord record in _currentTable.GetAllRecords(true)) count++;
                return count;
            }
        }

        /// <summary>
        /// 全ての単位次元の単位変換レコードを返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        IEnumerable<UnitConvertRecord> GetAllUnitRecord()
        {
            foreach (UnitConvertTable table in UnitConvertTable.ValidTables.Values)
            {
                foreach (UnitConvertRecord record in table.GetAllRecords(true))
                {
                    yield return record;
                }
            }
        }
        
        /// <summary>
        /// 同レコードの重複をチェックします。重複がある場合、例外が投げられます。
        /// </summary>
        /// <param name="unit"></param>
        void CheckDoubleUnitRecord(Formula unit, UnitConvertRecord original)
        {
            if (!(unit is Unit)) return; // 組立単位については重複を許可する
            foreach (UnitConvertRecord record in GetAllUnitRecord())
            {
                if (record.Equals(original)) continue;
                if (record.ConvertUnit == unit)
                    throw new Exception(string.Format("既に同名の単位（{0}：{1}）が定義されています。", unit.ToString(), record.BelongTable.UnitType));
                if (record.ConvertUnit is Unit && unit is Unit && (record.ConvertUnit as Unit).UnitName == (unit as Unit).UnitName)
                    throw new Exception(string.Format("既に同名の単位（{0}：{1}）が定義されています。", (unit as Unit).UnitName, record.BelongTable.UnitType));
            }
        }

        /// <summary>
        /// 指定単位扱い数式を単位名として更新登録します。
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="e"></param>
        void RenewUnitNameRecord(Formula unit, DataGridViewCellEventArgs e)
        {
            UnitConvertRecord rowRecord = _dataGridUnitTable.Rows[e.RowIndex].Tag as UnitConvertRecord;
            rowRecord.ConvertUnit = unit;
            foreach (DataGridViewColumn col in _dataGridUnitTable.Columns)
            {
                if (col.Tag == null) continue;
                if (col.Tag.Equals(rowRecord))
                {
                    if (col.Index > CountOfCurrentRecord + 1)
                        col.HeaderText = "+ " + unit.ToString().Replace("[", "").Replace("]", "");
                    else
                        col.HeaderText = "/ " + unit.ToString().Replace("[", "").Replace("]", "");
                }
            }
        }


        /// <summary>
        /// 単位間の変換率テーブルを再描画します。
        /// </summary>
        void RefreshConvertTable()
        {
            Format format = new Format();
            format.SetProperty(new ConsiderDigitFormatProperty(true));
            format.SetProperty(new DivisionFormatProperty(true));

            for (int i = 0; i < _dataGridUnitTable.Rows.Count; i++)
            {
                DataGridViewRow row = _dataGridUnitTable.Rows[i];
                UnitConvertRecord data = row.Tag as UnitConvertRecord;

                int columnNo = 2;
                // 変換倍率の表示
                foreach (UnitConvertRecord dataConvert in _currentTable.GetAllRecords(true))
                {
                    Formula convert = _currentTable.GetConversionRatio(data.ConvertUnit, dataConvert.ConvertUnit);
//                    convert = convert.Numerate();
                    convert.Format = format;

                    if (i+ 2 == columnNo)
                    {
                        row.Cells[columnNo].ReadOnly = true;
                        row.Cells[columnNo].Style.BackColor = Color.WhiteSmoke;
                    }
                    row.Cells[columnNo++].Value = convert.ToString();
                }
                // 変換加算値の表示
                foreach (UnitConvertRecord dataConvert in _currentTable.GetAllRecords(true))
                {
                    Formula convert = _currentTable.GetConvertAddition(data.ConvertUnit, dataConvert.ConvertUnit);
//                    convert = convert.Numerate();
                    convert.Format = format;

                    if (i + CountOfCurrentRecord + 2 == columnNo)
                    {
                        row.Cells[columnNo].ReadOnly = true;
                        row.Cells[columnNo].Style.BackColor = Color.WhiteSmoke;
                    }
                    row.Cells[columnNo++].Value = convert.ToString();
                }
            }
        }


        /// <summary>
        /// 編集開始時
        /// </summary>
        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            //if (e.ColumnIndex > 1) // 変換率編集時、変換加算値編集時
            //{
            //    TargetWatcher.Solver.RenewIntellisence(_formulaIntelisence.Intelisence, Core.Solver.IntellisenceType.Function | Core.Solver.IntellisenceType.Variable);
            //    _formulaIntelisence.StartInputFormula(e);
            //}
            //else if (e.ColumnIndex == 0) // 単位名編集開始時
            //{
            //    TargetWatcher.Solver.RenewIntellisence(_formulaIntelisence.Intelisence, Core.Solver.IntellisenceType.Unit);
            //    _formulaIntelisence.StartInputFormula(e);
            //}
        }

        /// <summary>
        /// 編集終了時
        /// </summary>
        protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
        {
            UnitConvertRecord rowRecord = _dataGridUnitTable.Rows[e.RowIndex].Tag as UnitConvertRecord;
            if (e.ColumnIndex == 0) // 単位名変更時
            {
                string unitName = GetStringFrom(_dataGridUnitTable[e.ColumnIndex, e.RowIndex].Value);
                //                if (GetStringFrom(_preEditValue) == unitName) return;

                unitName = "[" + unitName.TrimStart('[').TrimEnd(']') + "]";
                Formula unit = Formula.Parse(unitName);
                if (!unit.Combine().IsUnit()) throw new Exception(unitName + "は単位名として無効です。");

                // 接頭辞の名前を含む場合、単位名か接頭辞か選択させる
                if (unit is Unit)
                {
                    List<Prefix> candidates = new List<Prefix>();
                    foreach (Prefix prefix in Prefix.GetAllPrefix(false))
                        if (unitName.StartsWith("[" + prefix.ToString()) && unitName != "[" + prefix.ToString() + "]")
                            candidates.Add(prefix);
                    if (candidates.Count != 0)
                    {
                        _panelNewRecord.Tag = e;
                        DisplayNewRecordPanel(unitName, rowRecord, candidates.ToArray());
                        return;
                    }
                }
                // 単位名重複チェック
                CheckDoubleUnitRecord(unit, rowRecord);
                // 登録
                RenewUnitNameRecord(unit, e);
            }
            else if (e.ColumnIndex == 1) // コメントの設定
            {
                rowRecord.UnitComment = GetStringFrom(_dataGridUnitTable[e.ColumnIndex, e.RowIndex].Value);
            }
            else if (e.ColumnIndex > 1 + CountOfCurrentRecord) ConvertAdditionEndEdit(e); // 変換加算値設定時
            else if (e.ColumnIndex > 1) ConvertMultipleEndEdit(e); // 変換率設定時
        }

        #region レコード操作

        /// <summary>
        /// レコードの追加
        /// </summary>
        private void _btnAddRecord_Click(object sender, EventArgs e)
        {
            string nameRecord = "unit";
            bool existSame = false;
            int suffix = 1;
            do
            {
                existSame = false;
                foreach (UnitConvertRecord record in GetAllUnitRecord())
                {
                    if (record.ConvertUnit is Unit && (record.ConvertUnit as Unit).UnitName == nameRecord)
                    {
                        existSame = true;
                        break;
                    }
                }
                if (existSame) { nameRecord = "unit" + (suffix++).ToString(); }
            } while (existSame);

            if (_currentTable.BaseUnit == null)
            {
                BaseUnitConvertRecord baseRecord = new BaseUnitConvertRecord(new Unit(nameRecord), "ユーザー定義単位");
                _currentTable.BaseUnit = baseRecord;
            }
            else
            {
                UnitConvertRecord convertRecord = new UnitConvertRecord(new Unit(nameRecord), 1, "ユーザー定義単位");
                _currentTable.AddRecord(convertRecord);
            }
            RenewTable();

            _dataGridUnitTable.ClearSelection();
            _dataGridUnitTable[0, _dataGridUnitTable.Rows.Count - 1].Selected = true;
        }

        /// <summary>
        /// レコードの削除
        /// </summary>
        private void _btnDeleteRecord_Click(object sender, EventArgs e)
        {
            bool deleted = false;
            _labelError.Text = "削除対象の行ヘッダーをクリックして、行を選択してください。";
            _labelError.Visible = true;
            foreach (DataGridViewRow row in _dataGridUnitTable.SelectedRows)
            {
                deleted |= DeleteRecord(row);

                if (deleted) _labelError.Visible = false;
            }

            if (deleted) RenewTable();
        }

        /// <summary>
        /// 指定行に関連付けられた単位レコードを削除します。
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool DeleteRecord(DataGridViewRow row)
        {
            if (row.Tag is BaseUnitConvertRecord)
            {
                _labelError.Text = "選択行は基準単位のため、削除することはできません。";
                return false;
            }
            else
            {
                _currentTable.RemoveRecord((row.Tag as UnitConvertRecord).ConvertUnit);
                return true;
            }
        }

        /// <summary>
        /// レコードを上へ
        /// </summary>
        private void _btnMoveUp_Click(object sender, EventArgs e) { MoveRecord(true); }

        /// <summary>
        /// レコードを下へ
        /// </summary>
        private void _btnMoveDown_Click(object sender, EventArgs e) { MoveRecord(false); }

        /// <summary>
        /// 選択中のレコードを移動します。
        /// </summary>
        /// <param name="Up"></param>
        void MoveRecord(bool Up)
        {
            List<UnitConvertRecord> moved = new List<UnitConvertRecord>();

            _labelError.Text = "移動対象の行ヘッダーをクリックして、行を選択してください。";
            _labelError.Visible = true;

            List<int> selectedIndex = new List<int>();
            foreach (DataGridViewRow row in _dataGridUnitTable.SelectedRows) selectedIndex.Add(row.Index);
            selectedIndex.Sort();
            if (!Up) selectedIndex.Reverse();

            foreach (int rowIndex in selectedIndex)
            {
                DataGridViewRow row = _dataGridUnitTable.Rows[rowIndex];
                if (row.Tag is BaseUnitConvertRecord)
                {
                    _labelError.Text = "選択行は基準単位のため、最上から移動することはできません。";
                    _labelError.Visible = true;
                    continue;
                }
                _labelError.Visible = false;

                if (Up && rowIndex < 2) continue;
                if (!Up && rowIndex >= CountOfCurrentRecord - 1) continue;

                int targetIndex = Up ? rowIndex - 2 : rowIndex;

                UnitConvertRecord target = row.Tag as UnitConvertRecord;
                _currentTable.RemoveRecord(target.ConvertUnit);
                _currentTable.InsertRecord(targetIndex, target);
                moved.Add(target);
            }
            if (moved.Count == 0) return;

            RenewTable();
            foreach (DataGridViewCell cell in _dataGridUnitTable.SelectedCells) cell.Selected = false;
            foreach (DataGridViewRow row in _dataGridUnitTable.Rows)
            {
                UnitConvertRecord target = row.Tag as UnitConvertRecord;
                if (moved.Contains(target)) row.Selected = true;
            }
        }

        // 行の削除受付
        private void _dataGridUnitTable_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            _btnDeleteRecord_Click(sender, e);
            e.Cancel = true;
        }

        #endregion

        #region テーブル操作

        /// <summary>
        /// テーブルの追加
        /// </summary>
        private void _btnAddTable_Click(object sender, EventArgs e)
        {
            string unitType = "UserUnitType";
            int tryCount = 0;
            while (UnitConvertTable.ValidTables.ContainsKey(unitType))
                unitType = "UserUnitType" + (++tryCount).ToString();

            _txtBoxUnitTableName.Text = unitType;
            _txtBoxUnitTableComment.Text = "ユーザー定義の単位次元";

            _labelErrorTable.Visible = false;
            _panelTableProperty.Visible = true;

            _btnOK.Tag = true;
        }
        private void _btnOK_Click(object sender, EventArgs e)
        {
            _labelErrorTable.Visible = false;
            string unitType = _txtBoxUnitTableName.Text;
            if ((bool)_btnOK.Tag) // 追加
            {
                if (UnitConvertTable.ValidTables.ContainsKey(unitType))
                {
                    _labelErrorTable.Visible = true;
                    return;
                }
                UnitConvertTable table = new UnitConvertTable();
                table.UnitType = unitType;
                table.TypeComment = _txtBoxUnitTableComment.Text;

                UnitConvertTable.ValidTables.Add(unitType, table);

                _cmbUnitType.Items.Add(unitType);
                _cmbUnitType.SelectedItem = unitType;

                // 基準単位を登録
                _btnAddRecord_Click(sender, e);
            }
            else // 変更
            {
                if (UnitConvertTable.ValidTables.ContainsKey(unitType))
                {
                    if (UnitConvertTable.ValidTables[unitType].Equals(_currentTable)) _panelTableProperty.Visible = false;
                    else
                    {
                        _labelErrorTable.Visible = true;
                        return;
                    }
                }
                _currentTable.UnitType = unitType;
                _currentTable.TypeComment = _txtBoxUnitTableComment.Text;
                _cmbUnitType.Items[_cmbUnitType.SelectedIndex] = unitType;

                _txtUnitTypeComment.Text = _txtBoxUnitTableComment.Text;
            }
            _panelTableProperty.Visible = false;
        }
        private void _btnCancel_Click(object sender, EventArgs e)
        {
            _panelTableProperty.Visible = false;
        }
        private void _panelTableProperty_VisibleChanged(object sender, EventArgs e)
        {
            _groupUnitType.Enabled = _btnDeleteTable.Enabled = _btnChangeTable.Enabled = _btnAddTable.Enabled = _cmbUnitType.Enabled = !(sender as Panel).Visible;

            if (_panelTableProperty.Visible)
            {
                _txtBoxUnitTableName.Focus();
                _txtBoxUnitTableName.SelectAll();
            }
        }
        private void _txtBoxUnitTableName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _txtBoxUnitTableComment.Focus();
                _txtBoxUnitTableComment.SelectAll();
            }
        }
        private void _txtBoxUnitTableComment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) _btnOK_Click(sender, e);
        }

        /// <summary>
        /// テーブルの変更
        /// </summary>
        private void _btnChangeTable_Click(object sender, EventArgs e)
        {
            if (_currentTable == null) return;
            _txtBoxUnitTableName.Text = _currentTable.UnitType;
            _txtBoxUnitTableComment.Text = _currentTable.TypeComment;

            _labelErrorTable.Visible = false;
            _panelTableProperty.Visible = true;

            _btnOK.Tag = false;
        }

        /// <summary>
        /// テーブルの削除
        /// </summary>
        private void _btnDeleteTable_Click(object sender, EventArgs e)
        {
            if (_currentTable == null) return;
            UnitConvertTable.ValidTables.Remove(_cmbUnitType.SelectedItem.ToString());
            _cmbUnitType.Items.RemoveAt(_cmbUnitType.SelectedIndex);

            if (_cmbUnitType.Items.Count != 0) _cmbUnitType.SelectedIndex = 0;
            else _groupUnitType.Enabled = false;
        }

        /// <summary>
        /// カレントテーブル変更
        /// </summary>
        private void _cmbUnitType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _labelError.Visible = false;
            _groupUnitType.Enabled = true;
            _currentTable = UnitConvertTable.ValidTables[_cmbUnitType.SelectedItem.ToString()];
            RenewTable();
        }

        #endregion

        #region 接頭辞選択操作

        /// <summary>
        /// レコード追加時の接頭辞判断パネルを表示します。
        /// </summary>
        /// <param name="unitName">単位名</param>
        /// <param name="original">変更前のレコード</param>
        /// <param name="prefix">接頭辞</param>
        void DisplayNewRecordPanel(string unitName, UnitConvertRecord original, params Prefix[] prefixs)
        {
            string name = unitName.TrimStart('[').TrimEnd(']');

            _radioWithoutPrefix.Checked = _radioWithPrefix.Checked = false;

            List<Unit> registUnits = new List<Unit>();
            _cmbWithPrefix.Items.Clear();
            foreach (Prefix prefix in prefixs)
            {
                string text = string.Format("接頭辞「{0}({1})」+単位名「{2}」として登録",
                                                        prefix.ToString(), prefix.Name, name.Substring(prefix.ToString().Length));
                Unit registUnit = new Unit(name.Substring(prefix.ToString().Length), prefix);
                try
                {
                    CheckDoubleUnitRecord(registUnit, original);
                    _cmbWithPrefix.Items.Add(text);
                    registUnits.Add(registUnit);
                    _cmbWithPrefix.SelectedIndex = 0;
                }
                catch { }
            }
            _radioWithPrefix.Enabled = registUnits.Count != 0;
            _radioWithPrefix.Tag = registUnits;

            _radioWithoutPrefix.Text = string.Format("単位名「{0}」として登録", name);
            {
                Unit registUnit = new Unit(name, new NullPrefix());
                _radioWithoutPrefix.Tag = registUnit;
                _radioWithoutPrefix.Enabled = true;
                try
                {
                    CheckDoubleUnitRecord(registUnit, original);
                }
                catch { _radioWithoutPrefix.Enabled = false; }
            }
            _radioWithoutPrefix.Checked = (_radioWithoutPrefix.Enabled && !_radioWithPrefix.Enabled);
            _radioWithPrefix.Checked = (!_radioWithoutPrefix.Enabled && _radioWithPrefix.Enabled);

            _panelNewRecord.Visible = true;
        }

        /// <summary>
        /// OKボタンの使用可否反映
        /// </summary>
        private void AddRecordPrefixChanged(object sender, EventArgs e)
        {
            if (!_radioWithoutPrefix.Checked && !_radioWithPrefix.Checked) _btnOKRecord.Enabled = false;
            else _btnOKRecord.Enabled = true;
        }

        /// <summary>
        /// OK時
        /// </summary>
        private void _btnOKRecord_Click(object sender, EventArgs e)
        {
            DataGridViewCellEventArgs data = _panelNewRecord.Tag as DataGridViewCellEventArgs;
            Unit unit = _radioWithPrefix.Checked ? (_radioWithPrefix.Tag as List<Unit>)[_cmbWithPrefix.SelectedIndex] 
                                                                    : _radioWithoutPrefix.Tag as Unit;
            RenewUnitNameRecord(unit, data);
            _panelNewRecord.Visible = false;
        }

        /// <summary>
        /// キャンセル時
        /// </summary>
        private void _btnCancelRecord_Click(object sender, EventArgs e)
        {
            DataGridViewCellEventArgs data = _panelNewRecord.Tag as DataGridViewCellEventArgs;
            _dataGridUnitTable[data.ColumnIndex, data.RowIndex].Value = PreEditCellValue;
            _panelNewRecord.Visible = false;
        }

        #endregion

        /// <summary>
        /// 変換率編集後の処理
        /// </summary>
        private void ConvertMultipleEndEdit(DataGridViewCellEventArgs e)
        {
            UnitConvertRecord rowRecord = _dataGridUnitTable.Rows[e.RowIndex].Tag as UnitConvertRecord;
            UnitConvertRecord columnRecord = _dataGridUnitTable.Columns[e.ColumnIndex].Tag as UnitConvertRecord;

            // 数式チェック
            string value = null;
            if (_dataGridUnitTable[e.ColumnIndex, e.RowIndex].Value == null)
                value = "1";
            else
                value = GetStringFrom(_dataGridUnitTable[e.ColumnIndex, e.RowIndex].Value);

            Formula f = Formula.Parse(value);
            if (f == 0) throw new Exception("単位変換率に0を指定することはできません。");
            foreach (Unit u in f.GetExistFactor<Unit>()) throw new Exception("変換率に単位を含むことはできません。");

            // 変換率の設定
            rowRecord.BelongTable.SetConversionRatio(rowRecord.ConvertUnit, columnRecord.ConvertUnit, f);

            RefreshConvertTable();
        }

        /// <summary>
        /// 変換加算値編集後の処理
        /// </summary>
        private void ConvertAdditionEndEdit(DataGridViewCellEventArgs e)
        {
            UnitConvertRecord rowRecord = _dataGridUnitTable.Rows[e.RowIndex].Tag as UnitConvertRecord;
            UnitConvertRecord columnRecord = _dataGridUnitTable.Columns[e.ColumnIndex].Tag as UnitConvertRecord;

            // 数式チェック
            string value = null;
            if (_dataGridUnitTable[e.ColumnIndex, e.RowIndex].Value == null)
                value = "0";
            else
                value = GetStringFrom(_dataGridUnitTable[e.ColumnIndex, e.RowIndex].Value);

            Formula f = Formula.Parse(value);
            foreach (Unit u in f.GetExistFactor<Unit>()) throw new Exception("変換加算値に単位を含むことはできません。");
            f = f.Simplify();

            if (rowRecord is BaseUnitConvertRecord || columnRecord is BaseUnitConvertRecord)
            {
                // 変換率の設定
                rowRecord.BelongTable.SetConvertAddition(rowRecord.ConvertUnit, columnRecord.ConvertUnit, f, false);
                RefreshConvertTable();
            }
            else
            {
                _radioChangeRow.Text = string.Format("「{0}」の「{1}」に対する変換加算値を変更する", rowRecord.ConvertUnit.ToString(), _currentTable.BaseUnit.ConvertUnit.ToString());
                _radioChangeColumn.Text = string.Format("「{0}」の「{1}」に対する変換加算値を変更する", columnRecord.ConvertUnit.ToString(), _currentTable.BaseUnit.ConvertUnit.ToString());
                _radioChangeRow.Tag = rowRecord.ConvertUnit;
                _radioChangeColumn.Tag = columnRecord.ConvertUnit;
                _btnOKChangeAdd.Tag = f;
                _btnCancelChangeAdd.Tag = e;

                _radioChangeColumn.Checked = _radioChangeRow.Checked = false;
                _btnOKChangeAdd.Enabled = false;

                _panelSelectAddChangeTarget.Tag = PreEditCellValue;
                _panelSelectAddChangeTarget.Visible = true;
            }
        }

        #region 加算値編集時の編集レコード選択

        private void _btnOKChangeAdd_Click(object sender, EventArgs e)
        {
            Formula rowUnit = _radioChangeRow.Tag as Formula;
            Formula columnUnit = _radioChangeColumn.Tag as Formula;

            try
            {
                // 変換率の設定
                _currentTable.SetConvertAddition(rowUnit, columnUnit, _btnOKChangeAdd.Tag as Formula, _radioChangeRow.Checked);
            }
            catch (Exception exc)
            {
                _labelError.Text = exc.Message;
                _labelError.Visible = true;
                DataGridViewCellEventArgs eTag = _btnCancelChangeAdd.Tag as DataGridViewCellEventArgs;
                _dataGridUnitTable[eTag.ColumnIndex, eTag.RowIndex].Value = _panelSelectAddChangeTarget.Tag;
            }
            RefreshConvertTable();

            _panelSelectAddChangeTarget.Visible = false;
        }

        private void _btnCancelChangeAdd_Click(object sender, EventArgs e)
        {
            DataGridViewCellEventArgs eTag = _btnCancelChangeAdd.Tag as DataGridViewCellEventArgs;
            _dataGridUnitTable[eTag.ColumnIndex, eTag.RowIndex].Value = _panelSelectAddChangeTarget.Tag;
            _panelSelectAddChangeTarget.Visible = false;
        }

        private void _radioChangeColumn_CheckedChanged(object sender, EventArgs e)
        {
            if (_radioChangeRow.Checked || _radioChangeColumn.Checked) _btnOKChangeAdd.Enabled = true;
        }

        #endregion

        // 定義参照受付
        public override bool OpenDefine(object target)
        {
            if (!(target is Unit)) return false;

            Unit targetUnit = target as Unit;
            UnitConvertTable table = targetUnit.BelongTable;
            if (table == null) return false;

            _cmbUnitType.SelectedItem = table.UnitType;

            _dataGridUnitTable.ClearSelection();
            foreach (DataGridViewRow row in _dataGridUnitTable.Rows)
            {
                if ((row.Tag as UnitConvertRecord).ConvertUnit is Unit)
                {
                    Unit checkUnit = (row.Tag as UnitConvertRecord).ConvertUnit as Unit;

                    if (checkUnit.UnitName == targetUnit.UnitName)
                    {
                        row.Selected = true;
                        _dataGridUnitTable.FirstDisplayedScrollingRowIndex = row.Index;
                        return true;
                    }
                }
            }
            return base.OpenDefine(target);
        }




    }
}
