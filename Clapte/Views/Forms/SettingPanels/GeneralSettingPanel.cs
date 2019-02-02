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
using GoodSeat.Liffom;
using GoodSeat.Clapte.ViewModels;

namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    /// <summary>
    /// Clapteの全般的な設定パネルを表します。
    /// </summary>
    public partial class GeneralSettingPanel : SettingPanel
    {
        /// <summary>
        /// Clapteの全般的な設定パネルを初期化します。
        /// </summary>
        /// <param name="target">設定対象のClapteCoreViewModelオブジェクト。</param>
        /// <param name="mainForm">設定対象のClapteのメインビュー。</param>
        /// <param name="hotkeyManager">設定対象のホットキー管理オブジェクト。</param>
        public GeneralSettingPanel(ClapteCoreViewModel target, FormOfMain mainForm, HotkeyManager hotkeyManager)
        {
            InitializeComponent();

            Target = target;
            TargetMainForm = mainForm;
            TargetHotkeyManager = hotkeyManager;
            DownloadSetting();
        }

        /// <summary>
        /// 設定対象となるClapteCoreModelViewを設定もしくは取得します。
        /// </summary>
        ClapteCoreViewModel Target { get; set; }

        /// <summary>
        /// 設定対象となるSolverModelViewを取得します。
        /// </summary>
        SolverViewModel TargetSolver { get { return Target.Solver; } }

        /// <summary>
        /// 設定対象のClapteのメインビューを設定もしくは取得します。
        /// </summary>
        FormOfMain TargetMainForm { get; set; }

        /// <summary>
        /// 設定対象となるHotkeyManagerを設定もしくは取得します。
        /// </summary>
        HotkeyManager TargetHotkeyManager { get; set; }

        /// <summary>
        /// 設定を画面上に反映します。
        /// </summary>
        void DownloadSetting()
        {
            _checkIsCalculatorMode.Checked = FormOfMain.IsCalculatorMode;
            _numMaxLength.Value = TargetSolver.MaxInputTextLength;
            _numMaxUnitLength.Value = TargetSolver.MaxVariableTextLength;

            _numBalloonTime.Value = Target.LimitTime;

            _checkPermitAllResult.Checked = !TargetSolver.PermitOnlySingleTermResult ;

            _checkCopyWithClick.Checked = TargetMainForm.ActionWithBalloonClick;
            _checkCopyWithSame.Checked = Target.ActionWithSameCopy;
            _checkCopyWithHotkey.Checked = TargetHotkeyManager.Enable;

            _txtBoxHotkey.Text = TargetHotkeyManager.Hotkey;
            _checkAlt.Checked = TargetHotkeyManager.Alt;
            _checkCtrl.Checked = TargetHotkeyManager.Ctrl;
        }

        /// <summary>
        /// 現在の設定を確定します。
        /// </summary>
        public override void OnDeterminSetting()
        {
            FormOfMain.IsCalculatorMode = _checkIsCalculatorMode.Checked;
            TargetSolver.MaxInputTextLength = (int)_numMaxLength.Value;
            TargetSolver.MaxVariableTextLength = (int)_numMaxUnitLength.Value;

            Target.LimitTime = (int)_numBalloonTime.Value;

            TargetSolver.PermitOnlySingleTermResult = !_checkPermitAllResult.Checked;

            TargetMainForm.ActionWithBalloonClick = _checkCopyWithClick.Checked;
            Target.ActionWithSameCopy = _checkCopyWithSame.Checked;
            TargetHotkeyManager.Enable = _checkCopyWithHotkey.Checked;
        }



        private void _checkCopyWithHotkey_CheckedChanged(object sender, EventArgs e)
        {
            _checkAlt.Enabled = _checkCtrl.Enabled = _txtBoxHotkey.Enabled = _checkCopyWithHotkey.Checked;
        }
        private void HotkeySettingChanged(object sender, EventArgs e)
        {
            RegistHotkey();
        }
        private void _txtBoxHotkey_KeyUp(object sender, KeyEventArgs e)
        {
            _txtBoxHotkey.Text = e.KeyCode.ToString();
            RegistHotkey();
        }

        /// <summary>
        /// 現在の設定に基づき、ホットキーの設定を更新します。
        /// </summary>
        void RegistHotkey()
        {
            bool enable = TargetHotkeyManager.RegisterHotkey(_txtBoxHotkey.Text, _checkAlt.Checked, _checkCtrl.Checked);
            _labelEnableHotkey.Text = enable ? "○ 使用可能" : "× 使用不可";

            if (!_checkCopyWithHotkey.Checked) TargetHotkeyManager.UnregisterHotKey();
        }

    }
}
