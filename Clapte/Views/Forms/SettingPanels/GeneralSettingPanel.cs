using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GoodSeat.Liffom;
using GoodSeat.Clapte.ViewModels;
using GoodSeat.Clapte.Solvers.Processes;

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
            _numMaxLength.Value = TargetSolver.MaxInputTextLength;
            _numMaxUnitLength.Value = TargetSolver.MaxVariableTextLength;

            _cmbValidPrecision.SelectedIndex = TargetSolver.ConsiderValidDigit ? 0 : 1;
            _cmbRounding.SelectedIndex = (TargetSolver.MidpointRound == MidpointRounding.AwayFromZero) ? 0 : 1;

            _numBalloonTime.Value = Target.LimitTime;

            switch (TargetSolver.OutputCharaType)
            {
                case CharaType.Auto: _cmbResultCharType.SelectedIndex = 0; break;
                case CharaType.Half: _cmbResultCharType.SelectedIndex = 1; break;
                case CharaType.All: _cmbResultCharType.SelectedIndex = 2; break;
                default: throw new NotImplementedException(); 
            }
            switch (TargetSolver.OutputUnitFormatType)
            {
                case UnitFormatType.Auto: _cmbOutputUnit.SelectedIndex = 0; break;
                case UnitFormatType.EncloseWithSpace: _cmbOutputUnit.SelectedIndex = 1; break;
                case UnitFormatType.EncloseWithParentheses: _cmbOutputUnit.SelectedIndex = 2; break;
                case UnitFormatType.EncloseWithBrackets: _cmbOutputUnit.SelectedIndex = 3; break;
                default: throw new NotImplementedException(); 
            }
            switch (TargetSolver.Mode)
            {
                case CalculateMode.Decimal: _cmbCalculateMode.SelectedIndex = 0; break;
                case CalculateMode.Fraction: _cmbCalculateMode.SelectedIndex = 1; break;
                default: throw new NotImplementedException();
            }
            //_numLimitTime.Value = (decimal)Target.LimitTime / 1000m;
            _checkPermitAllResult.Checked = !TargetSolver.PermitOnlySingleTermResult ;

            _checkCopyWithClick.Checked = TargetMainForm.ActionWithBalloonClick;
            _checkCopyWithSame.Checked = Target.ActionWithSameCopy;
            _checkCopyWithHotkey.Checked = TargetHotkeyManager.Enable;

            _checkPermitOmitMultipleMark.Checked = TargetSolver.PermitOmitProductMark;
            _checkPermitOmitPowerMark.Checked = TargetSolver.PermitOmitPowerMark;

            _txtBoxHotkey.Text = TargetHotkeyManager.Hotkey;
            _checkAlt.Checked = TargetHotkeyManager.Alt;
            _checkCtrl.Checked = TargetHotkeyManager.Ctrl;
        }

        /// <summary>
        /// 現在の設定を確定します。
        /// </summary>
        public override void OnDeterminSetting()
        {
            TargetSolver.MaxInputTextLength = (int)_numMaxLength.Value;
            TargetSolver.MaxVariableTextLength = (int)_numMaxUnitLength.Value;

            TargetSolver.ConsiderValidDigit = (_cmbValidPrecision.SelectedIndex == 0);
            TargetSolver.MidpointRound = (_cmbRounding.SelectedIndex == 0) ? MidpointRounding.AwayFromZero : MidpointRounding.ToEven;

            Target.LimitTime = (int)_numBalloonTime.Value;

            switch (_cmbResultCharType.SelectedIndex)
            {
                case 0: TargetSolver.OutputCharaType = CharaType.Auto; break;
                case 1: TargetSolver.OutputCharaType = CharaType.Half; break;
                case 2: TargetSolver.OutputCharaType = CharaType.All; break;
                default: throw new NotImplementedException(); 
            }
            switch (_cmbOutputUnit.SelectedIndex)
            {
                case 0: TargetSolver.OutputUnitFormatType = UnitFormatType.Auto; break;
                case 1: TargetSolver.OutputUnitFormatType = UnitFormatType.EncloseWithSpace; break;
                case 2: TargetSolver.OutputUnitFormatType = UnitFormatType.EncloseWithParentheses; break;
                case 3: TargetSolver.OutputUnitFormatType = UnitFormatType.EncloseWithBrackets; break;
                default: throw new NotImplementedException(); 
            }
            switch (_cmbCalculateMode.SelectedIndex)
            {
                case 0: TargetSolver.Mode = CalculateMode.Decimal; break;
                case 1: TargetSolver.Mode = CalculateMode.Fraction; break;
                default: throw new NotImplementedException();
            }
            //TargetWatcher.Solver.MaxCalculateTime = (int)(_numLimitTime.Value * 1000);
            TargetSolver.PermitOnlySingleTermResult = !_checkPermitAllResult.Checked;

            TargetMainForm.ActionWithBalloonClick = _checkCopyWithClick.Checked;
            Target.ActionWithSameCopy = _checkCopyWithSame.Checked;
            TargetHotkeyManager.Enable = _checkCopyWithHotkey.Checked;

            TargetSolver.PermitOmitProductMark = _checkPermitOmitMultipleMark.Checked;
            TargetSolver.PermitOmitPowerMark = _checkPermitOmitPowerMark.Checked;
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
