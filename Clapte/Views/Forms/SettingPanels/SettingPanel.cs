// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GoodSeat.Liffom.Utilities;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    /// <summary>
    /// オプションの設定パネルを表します。
    /// </summary>
    public partial class SettingPanel : UserControl
    {
        DataGridView _dataGrid;
        Control _errorInformControl;
        object _preEditValue;

        /// <summary>
        /// オプション設定パネルを初期化します。
        /// </summary>
        public SettingPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 現在の設定を確定します。
        /// </summary>
        public virtual void OnDeterminSetting() { }

        /// <summary>
        /// 設定を破棄して設定開始時の状態を復元します。
        /// </summary>
        public virtual void OnCancelSetting() { }

        /// <summary>
        /// 指定オブジェクトに関連付けられた状態を表示します。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool OpenDefine(object target) { return false; }




        /// <summary>
        /// 直前のセル編集開始前のセルの値を取得します。
        /// </summary>
        protected object PreEditCellValue
        {
            get { return _preEditValue; }
        }

        /// <summary>
        /// 設定パネルで用いるデータグリッドビュー、及びその編集エラーを表示するコントロールを登録します。
        /// </summary>
        /// <param name="dataGrid">使用するデータグリッドビュー</param>
        /// <param name="errorInformControl">エラー表示に用いるコントロール</param>
        protected void RegistEditGridView(DataGridView dataGrid, Control errorInformControl)
        {
            if (_dataGrid != null) _dataGrid.Dispose();

            _dataGrid = dataGrid;
            _dataGrid.CellBeginEdit += new DataGridViewCellCancelEventHandler(_dataGrid_CellBeginEdit);
            _dataGrid.CellEndEdit += new DataGridViewCellEventHandler(_dataGrid_CellEndEdit);

            _errorInformControl = errorInformControl;
        }

        /// <summary>
        /// 登録済みデータグリッドビューのセル編集開始時のコールバック関数です。
        /// </summary>
        protected void _dataGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _errorInformControl.Visible = false;
            _preEditValue = _dataGrid[e.ColumnIndex, e.RowIndex].Value;
            OnCellBeginEdit(e);
        }

        /// <summary>
        /// 登録済みデータグリッドビューのセル編集終了時のコールバック関数です。
        /// </summary>
        protected void _dataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                OnCellEndEdit(e);
            }
            catch (Exception exc)
            {
                _errorInformControl.Text = exc.Message.Replace("\r", "").Replace("\n", " ");
                _errorInformControl.Visible = true;
                _dataGrid[e.ColumnIndex, e.RowIndex].Value = _preEditValue;
            }
        }

        /// <summary>
        /// 登録済みデータグリッドビューのセル編集開始時に呼び出されます。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCellBeginEdit(DataGridViewCellCancelEventArgs e) { }
        
        /// <summary>
        /// 登録済みデータグリッドビューのセル編集終了時に呼び出されます。入力エラーとする場合、例外を投げてください。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCellEndEdit(DataGridViewCellEventArgs e) { }

        /// <summary>
        /// 指定オブジェクトから取得される文字列を返します。
        /// </summary>
        /// <param name="o">取得対象のオブジェクト。nullの場合、""を返します。</param>
        /// <returns>オブジェクトを表す文字列。</returns>
        protected string GetStringFrom(object o)
        {
            if (o == null) return "";
            else return o.ToString();
        }

    }
}
