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
using GoodSeat.Liffom.Reals;
using GoodSeat.Liffom.Processes;
using GoodSeat.Clapte.ViewModels;
using GoodSeat.Clapte.Solvers.Processes;

namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    /// <summary>
    /// 方程式の解の算出機能の設定パネルを表します。
    /// </summary>
    public partial class CalculateSettingPanel : SettingPanel
    {
        /// <summary>
        /// 方程式の解の算出機能の設定パネルを初期化します。
        /// </summary>
        /// <param name="clapteWatcher"></param>
        public CalculateSettingPanel(ClapteCoreViewModel target)
        {
            InitializeComponent();

            Target = target;

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
        /// 設定対象となる<see cref="NewtonMethod"/>を設定もしくは取得します。
        /// </summary>
        NewtonMethod TargetNewtonMethod { get { return TargetSolver.NewtonMethod; } }

        /// <summary>
        /// 設定対象となる<see cref="BrentMethod"/>を取得します。
        /// </summary>
        BrentMethod TargetBrentMethod { get { return TargetSolver.BrentMethod; } }

        /// <summary>
        /// 設定を画面上に反映します。
        /// </summary>
        private void DownloadSetting()
        {
            _checkParseAbs.Checked = TargetSolver.ParseAbsPunctuation;
            _checkParseFactorial.Checked = TargetSolver.ParseFactorial;
            _checkPermitOmitMultipleMark.Checked = TargetSolver.PermitOmitProductMark;
            _checkPermitOmitPowerMark.Checked = TargetSolver.PermitOmitPowerMark;

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
                default: _cmbOutputUnit.SelectedIndex = 0; break;
            }
            switch (TargetSolver.DetectUnitMode)
            {
                case ReplaceVariableToUnitProcess.Mode.AllAutoDetect: _cmbDetectUnitMode.SelectedIndex = 0; break;
                case ReplaceVariableToUnitProcess.Mode.OnlyRegisterd: _cmbDetectUnitMode.SelectedIndex = 1; break;
                case ReplaceVariableToUnitProcess.Mode.OnlyExplicit: _cmbDetectUnitMode.SelectedIndex = 2; break;
                default: _cmbDetectUnitMode.SelectedIndex = 0; break;
            }

            switch (TargetSolver.NumericPrecision)
            {
                case Liffom.Formulas.Numeric.RealType.DoubleModified: _radioPrecisionDouble.Checked = true; break;
                case Liffom.Formulas.Numeric.RealType.BigDecimal: _radioPrecisionCustom.Checked = true; break;
                default: throw new NotImplementedException();
            }
            _numPrecisionDigit.Value = TargetSolver.PrecisionDigitOfBigDecimal;

            _cmbValidPrecision.SelectedIndex = TargetSolver.ConsiderValidDigit ? 0 : 1;
            _cmbRounding.SelectedIndex = (TargetSolver.MidpointRound == MidpointRounding.AwayFromZero) ? 0 : 1;
            _numLimitTime.Value = (decimal)(TargetSolver.MaxTime / 1000d);
            switch (TargetSolver.Mode)
            {
                case CalculateMode.Decimal: _cmbCalculateMode.SelectedIndex = 0; break;
                case CalculateMode.Fraction: _cmbCalculateMode.SelectedIndex = 1; break;
                default: throw new NotImplementedException();
            }

            // 方程式求解の設定
            _numInitialSolutionNewton.Value = (decimal)TargetNewtonMethod.InitialSolution;
            _numErrorToleranceNewton.Value = (decimal)new DecimalValue(TargetNewtonMethod.ErrorTolerance).Exponent;
            _numTryMaxCountNewton.Value = (decimal)TargetNewtonMethod.MaxTryCount;

            _numUpperLimitBrent.Value = (decimal)TargetBrentMethod.UpperLimit;
            _numLowerLimitBrent.Value = (decimal)TargetBrentMethod.LowerLimit;
            _numErrorToleranceBrent.Value = (decimal)new DecimalValue(TargetBrentMethod.ErrorTolerance).Exponent;
            _numTryMaxCountBrent.Value = (decimal)TargetBrentMethod.MaxTryCount;

//            _checkDontCopyVariable.Checked = CalculateCommand.DontCopySolutionVariable;
        }

        public override void OnDeterminSetting()
        {
            base.OnDeterminSetting();

            // 計算の設定
            if (_radioPrecisionDouble.Checked) TargetSolver.NumericPrecision = Liffom.Formulas.Numeric.RealType.DoubleModified;
            if (_radioPrecisionCustom.Checked) TargetSolver.NumericPrecision = Liffom.Formulas.Numeric.RealType.BigDecimal;
            TargetSolver.PrecisionDigitOfBigDecimal = (int)_numPrecisionDigit.Value;

            TargetSolver.ConsiderValidDigit = (_cmbValidPrecision.SelectedIndex == 0);
            TargetSolver.MidpointRound = (_cmbRounding.SelectedIndex == 0) ? MidpointRounding.AwayFromZero : MidpointRounding.ToEven;
            TargetSolver.MaxTime = (double)(_numLimitTime.Value * 1000);

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
            switch (_cmbDetectUnitMode.SelectedIndex)
            {
                case 0: TargetSolver.DetectUnitMode = ReplaceVariableToUnitProcess.Mode.AllAutoDetect; break;
                case 1: TargetSolver.DetectUnitMode = ReplaceVariableToUnitProcess.Mode.OnlyRegisterd; break;
                case 2: TargetSolver.DetectUnitMode = ReplaceVariableToUnitProcess.Mode.OnlyExplicit; break;
                default: throw new NotImplementedException(); 
            }

            switch (_cmbCalculateMode.SelectedIndex)
            {
                case 0: TargetSolver.Mode = CalculateMode.Decimal; break;
                case 1: TargetSolver.Mode = CalculateMode.Fraction; break;
                default: throw new NotImplementedException();
            }

            // 方程式の設定
            if (_numLowerLimitBrent.Value > _numUpperLimitBrent.Value)
            {
                decimal tmpSwap = _numLowerLimitBrent.Value;
                _numLowerLimitBrent.Value = _numUpperLimitBrent.Value;
                _numUpperLimitBrent.Value = tmpSwap;
            }

            // 方程式求解の設定
            TargetNewtonMethod.InitialSolution = (double)_numInitialSolutionNewton.Value;
            TargetNewtonMethod.ErrorTolerance = Math.Pow(10.0, (double)_numErrorToleranceNewton.Value);
            TargetNewtonMethod.MaxTryCount = (int)_numTryMaxCountNewton.Value;

            TargetBrentMethod.UpperLimit = (double)_numUpperLimitBrent.Value;
            TargetBrentMethod.LowerLimit = (double)_numLowerLimitBrent.Value;
            TargetBrentMethod.ErrorTolerance = Math.Pow(10.0, (double)_numErrorToleranceBrent.Value);
            TargetBrentMethod.MaxTryCount = (int)_numTryMaxCountBrent.Value;

            TargetSolver.ParseAbsPunctuation = _checkParseAbs.Checked;
            TargetSolver.ParseFactorial = _checkParseFactorial.Checked;
            TargetSolver.PermitOmitProductMark = _checkPermitOmitMultipleMark.Checked;
            TargetSolver.PermitOmitPowerMark = _checkPermitOmitPowerMark.Checked;

//            CalculateCommand.DontCopySolutionVariable = _checkDontCopyVariable.Checked;
        }

        private void _radioPrecisionCustom_CheckedChanged(object sender, EventArgs e)
        {
            _labelPrecisionDigit.Enabled = _radioPrecisionCustom.Checked;
            _numPrecisionDigit.Enabled = _radioPrecisionCustom.Checked;
        }
    }
}
