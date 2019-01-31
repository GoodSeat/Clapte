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
using System.Text.RegularExpressions;
using GoodSeat.Clapte.Views.Forms.SettingPanels;
using GoodSeat.Clapte.Models.Windows;
using GoodSeat.Clapte.ViewModels;

namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    /// <summary>
    /// Clapteの監視対象外リストの設定パネルを表します。
    /// </summary>
    public partial class ExcludeListSettingPanel : SettingPanel
    {
        Dictionary<WindowIdentifyInfo.PropertyType, CheckBox> _validMap = new Dictionary<WindowIdentifyInfo.PropertyType,CheckBox>();
        Dictionary<WindowIdentifyInfo.PropertyType, TextBox> _textMap = new Dictionary<WindowIdentifyInfo.PropertyType,TextBox>();
        Dictionary<WindowIdentifyInfo.PropertyType, CheckBox> _regexMap = new Dictionary<WindowIdentifyInfo.PropertyType,CheckBox>();
        Dictionary<WindowIdentifyInfo.PropertyType, ComboBox> _matchMap = new Dictionary<WindowIdentifyInfo.PropertyType, ComboBox>();

        /// <summary>
        /// Clapteの監視対象外リストの設定パネルを初期化します。
        /// </summary>
        /// <param name="clapteCore"></param>
        public ExcludeListSettingPanel(ClapteCoreViewModel clapteCore)
        {
            InitializeComponent();

            Target = clapteCore;

            _validMap.Add(WindowIdentifyInfo.PropertyType.FileName, _checkValidFileName);
            _validMap.Add(WindowIdentifyInfo.PropertyType.ProductName, _checkValidProductName);
            _validMap.Add(WindowIdentifyInfo.PropertyType.TitleBarText, _checkValidTitle);
            _validMap.Add(WindowIdentifyInfo.PropertyType.ClassName, _checkValidClassName);

            _textMap.Add(WindowIdentifyInfo.PropertyType.FileName, _textBoxFileName);
            _textMap.Add(WindowIdentifyInfo.PropertyType.ProductName, _textBoxProductName);
            _textMap.Add(WindowIdentifyInfo.PropertyType.TitleBarText, _textBoxTitle);
            _textMap.Add(WindowIdentifyInfo.PropertyType.ClassName, _textBoxClassName);

            _regexMap.Add(WindowIdentifyInfo.PropertyType.FileName, _checkRegexFileName);
            _regexMap.Add(WindowIdentifyInfo.PropertyType.ProductName, _checkRegexProductName);
            _regexMap.Add(WindowIdentifyInfo.PropertyType.TitleBarText, _checkRegexTitle);
            _regexMap.Add(WindowIdentifyInfo.PropertyType.ClassName, _checkRegexClassName);

            _matchMap.Add(WindowIdentifyInfo.PropertyType.FileName, _cmbMatchFileName);
            _matchMap.Add(WindowIdentifyInfo.PropertyType.ProductName, _cmbMatchProductName);
            _matchMap.Add(WindowIdentifyInfo.PropertyType.TitleBarText, _cmbMatchTitle);
            _matchMap.Add(WindowIdentifyInfo.PropertyType.ClassName, _cmbMatchClassName);

            DownloadSetting();
        }

        /// <summary>
        /// 設定対象とするClapteCoreを設定若しくは取得します。
        /// </summary>
        private ClapteCoreViewModel Target { get; set; }

        private void DownloadSetting()
        {
            foreach (var item in Target.ExcludeTargetList)
            {
                _listView.Items.Add(CreateListViewItemOf(item));
            }
        }

        ListViewItem CreateListViewItemOf(WindowIdentifyInfo item)
        {
            List<string> stringList = new List<string>();
            if (item.GetValidOf(WindowIdentifyInfo.PropertyType.FileName))
                stringList.Add(item.GetTextOf(WindowIdentifyInfo.PropertyType.FileName));
            else
                stringList.Add("");

            if (item.GetValidOf(WindowIdentifyInfo.PropertyType.ProductName))
                stringList.Add(item.GetTextOf(WindowIdentifyInfo.PropertyType.ProductName));
            else
                stringList.Add("");

            if (item.GetValidOf(WindowIdentifyInfo.PropertyType.TitleBarText))
                stringList.Add(item.GetTextOf(WindowIdentifyInfo.PropertyType.TitleBarText));
            else
                stringList.Add("");

            if (item.GetValidOf(WindowIdentifyInfo.PropertyType.ClassName))
                stringList.Add(item.GetTextOf(WindowIdentifyInfo.PropertyType.ClassName));
            else
                stringList.Add("");

            var listItem = new ListViewItem(stringList.ToArray());
            listItem.Tag = new WindowIdentifyInfo(item);
            return listItem;
        }

        private void _btnAddFilter_Click(object sender, EventArgs e)
        {
            for (WindowIdentifyInfo.PropertyType type = WindowIdentifyInfo.PropertyType.TitleBarText; type <= WindowIdentifyInfo.PropertyType.ClassName; type++)
            {
                _validMap[type].Checked = false;
                _textMap[type].Text = "";
                _regexMap[type].Checked = false;
                _matchMap[type].SelectedIndex = 0;
            }

            _panelExcludeSetting.Visible = true;
        }

        private void _btnChangeFilter_Click(object sender, EventArgs e)
        {
            if (_listView.SelectedItems.Count == 0) return;
            WindowIdentifyInfo info = _listView.SelectedItems[0].Tag as WindowIdentifyInfo;

            for (WindowIdentifyInfo.PropertyType type = WindowIdentifyInfo.PropertyType.TitleBarText; type <= WindowIdentifyInfo.PropertyType.ClassName; type++)
            {
                _validMap[type].Checked = info.GetValidOf(type);
                _textMap[type].Text = info.GetTextOf(type);
                _regexMap[type].Checked = info.GetUsingRegexIn(type);
                _matchMap[type].SelectedIndex = (int)info.GetMatchTypeOf(type);
            }

            _panelExcludeSetting.Tag = _listView.SelectedItems[0];
            _panelExcludeSetting.Visible = true;
        }

        private void _btnDeleteFilter_Click(object sender, EventArgs e)
        {
            if (_listView.SelectedItems.Count == 0) return;
            _listView.Items.Remove(_listView.SelectedItems[0]);
        }

        private void _panelExcludeSetting_VisibleChanged(object sender, EventArgs e)
        {
            _btnAddFilter.Enabled = !_panelExcludeSetting.Visible;
            _btnChangeFilter.Enabled = !_panelExcludeSetting.Visible;
            _btnDeleteFilter.Enabled = !_panelExcludeSetting.Visible;

            _labelErrorMsg.Text = "";

            LeavePickApplication();
        }

        public override void OnDeterminSetting()
        {
            Target.ExcludeTargetList.Clear();
            foreach (ListViewItem item in _listView.Items)
                Target.ExcludeTargetList.Add(item.Tag as WindowIdentifyInfo);

            _timer.Enabled = false;
            base.OnDeterminSetting();
        }

        public override void OnCancelSetting()
        {
            _timer.Enabled = false;
            base.OnCancelSetting();
        }



        bool _saveTopMost;

        private void _btnPickApplication_Click(object sender, EventArgs e)
        {
            if (_timer.Enabled)
                LeavePickApplication();
            else
                EnterPickApplication();
        }

        private void EnterPickApplication()
        {
            _timer.Enabled = true;
            _labelPick.Visible = _timer.Enabled;
            _saveTopMost = ((Form)TopLevelControl).TopMost;
            ((Form)TopLevelControl).TopMost = true;
        }

        private void LeavePickApplication()
        {
            _timer.Enabled = false;
            _labelPick.Visible = _timer.Enabled;
            ((Form)TopLevelControl).TopMost = _saveTopMost;
        }

        private void _btnOK_Click(object sender, EventArgs e)
        {
            bool isOK = false;

            WindowIdentifyInfo wid = new WindowIdentifyInfo();
            for (WindowIdentifyInfo.PropertyType type = WindowIdentifyInfo.PropertyType.TitleBarText; type <= WindowIdentifyInfo.PropertyType.ClassName; type++)
            {
                wid.SetValidOf(type, _validMap[type].Checked);
                wid.SetTextOf(type, _textMap[type].Text);
                wid.SetUsingRegexIn(type, _regexMap[type].Checked);
                wid.SetMatchTypeOf(type, (WindowIdentifyInfo.MatchType)_matchMap[type].SelectedIndex);
                
                // 正規表現のチェック
                if (_regexMap[type].Checked)
                {
                    try
                    {
                        Regex regex = new Regex(_textMap[type].Text);
                    }
                    catch
                    {
                        _labelErrorMsg.Text = "正規表現が不正です。";
                        return;
                    }
                }

                // 空文字チェック
                if (_validMap[type].Checked)
                {
                    if (string.IsNullOrEmpty(_textMap[type].Text))
                    {
                        _labelErrorMsg.Text = "文字列が空です。";
                        return;
                    }
                    isOK = true;
                }
            }
            if (!isOK)
            {
                _labelErrorMsg.Text = "↑ 対象とする項目にチェックを入れてください。";
                return;
            }

            if (_panelExcludeSetting.Tag != null)
            {
                ListViewItem item = _panelExcludeSetting.Tag as ListViewItem;
                int index = _listView.Items.IndexOf(item);
                _listView.Items.RemoveAt(index);
                _listView.Items.Insert(index, CreateListViewItemOf(wid));
                _listView.Items[index].Selected = true;
            }
            else
            {
                _listView.Items.Add(CreateListViewItemOf(wid));
            }
            _panelExcludeSetting.Visible = false;
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            _panelExcludeSetting.Visible = false;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            try
            {
                WindowInfo info = WindowInfo.PointedWindow;
                _textBoxFileName.Text = info.FileName;
                _textBoxProductName.Text = info.ProductName;
                _textBoxTitle.Text = info.TitleBarText;
                _textBoxClassName.Text = info.ClassName;

                info = WindowInfo.ActiveWindowInfo;
                if (info.FullPath.ToLower() == Application.ExecutablePath.ToLower()) return;
            }
            catch
            {
                _labelErrorMsg.Text = "ウインドウ情報の取得に失敗しました。";
            }

            LeavePickApplication();
        }

    }
}
