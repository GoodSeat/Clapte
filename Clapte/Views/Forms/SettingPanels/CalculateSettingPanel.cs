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

            // 計算の設定
            switch (TargetSolver.NumericPrecision)
            {
                case Liffom.Formulas.Numeric.RealType.DoubleModified: _radioPrecisionDouble.Checked = true; break;
                case Liffom.Formulas.Numeric.RealType.Decimal: _radioPrecisionDecimal.Checked = true; break;
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


        public override void OnDeterminSetting()
        {
            base.OnDeterminSetting();

            // 計算の設定
            if (_radioPrecisionDouble.Checked) TargetSolver.NumericPrecision = Liffom.Formulas.Numeric.RealType.DoubleModified;
            if (_radioPrecisionDecimal.Checked) TargetSolver.NumericPrecision = Liffom.Formulas.Numeric.RealType.Decimal;
            if (_radioPrecisionCustom.Checked) TargetSolver.NumericPrecision = Liffom.Formulas.Numeric.RealType.BigDecimal;
            TargetSolver.PrecisionDigitOfBigDecimal = (int)_numPrecisionDigit.Value;

            TargetSolver.ConsiderValidDigit = (_cmbValidPrecision.SelectedIndex == 0);
            TargetSolver.MidpointRound = (_cmbRounding.SelectedIndex == 0) ? MidpointRounding.AwayFromZero : MidpointRounding.ToEven;
            TargetSolver.MaxTime = (double)(_numLimitTime.Value * 1000);

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

//            CalculateCommand.DontCopySolutionVariable = _checkDontCopyVariable.Checked;
        }

        private void _radioPrecisionCustom_CheckedChanged(object sender, EventArgs e)
        {
            _labelPrecisionDigit.Enabled = _radioPrecisionCustom.Checked;
            _numPrecisionDigit.Enabled = _radioPrecisionCustom.Checked;
        }
    }
}
