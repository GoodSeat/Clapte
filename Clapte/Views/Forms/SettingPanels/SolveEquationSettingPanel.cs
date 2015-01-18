using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GoodSeat.Liffom;

namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
	/// <summary>
	/// 方程式の解の算出機能の設定パネルを表します。
	/// </summary>
	public partial class SolveEquationSettingPanel : SettingPanel
	{
		/// <summary>
		/// 方程式の解の算出機能の設定パネルを初期化します。
		/// </summary>
		/// <param name="clapteWatcher"></param>
		public SolveEquationSettingPanel()
		{
			InitializeComponent();

			//_numErrorToleranceNewton.Value = (decimal)TargetWatcher.Solver.NewtonRaphsonTreat.ErrorTolerance;
			//_numInitialSolutionNewton.Value = (decimal)TargetWatcher.Solver.NewtonRaphsonTreat.InitialSolution;
			//_numTryMaxCountNewton.Value = (decimal)TargetWatcher.Solver.NewtonRaphsonTreat.MaxTryCount;

			//_numErrorToleranceBrent.Value = (decimal)TargetWatcher.Solver.BrentMethodTreat.ErrorTolerance;
			//_numLowerLimitBrent.Value = (decimal)TargetWatcher.Solver.BrentMethodTreat.LowerLimit.Data.Data;
			//_numUpperLimitBrent.Value = (decimal)TargetWatcher.Solver.BrentMethodTreat.UpperLimit.Data.Data;
			//_numTryMaxCountNewton.Value = (decimal)TargetWatcher.Solver.BrentMethodTreat.MaxTryCount;

//			_checkDontCopyVariable.Checked = CalculateCommand.DontCopySolutionVariable;
		}

		public override void OnDeterminSetting()
		{
			base.OnDeterminSetting();

			if (_numLowerLimitBrent.Value > _numUpperLimitBrent.Value)
			{
				decimal tmpSwap = _numLowerLimitBrent.Value;
				_numLowerLimitBrent.Value = _numUpperLimitBrent.Value;
				_numUpperLimitBrent.Value = tmpSwap;
			}

			/*
			TargetWatcher.Solver.NewtonRaphsonTreat.ErrorTolerance = (double)_numErrorToleranceNewton.Value;
			TargetWatcher.Solver.NewtonRaphsonTreat.InitialSolution = (double)_numInitialSolutionNewton.Value;
			TargetWatcher.Solver.NewtonRaphsonTreat.MaxTryCount = (int)_numTryMaxCountNewton.Value;

			TargetWatcher.Solver.BrentMethodTreat.ErrorTolerance = (double)_numErrorToleranceBrent.Value;
			TargetWatcher.Solver.BrentMethodTreat.LowerLimit = new Numeric((double)_numLowerLimitBrent.Value);
			TargetWatcher.Solver.BrentMethodTreat.UpperLimit = new Numeric((double)_numUpperLimitBrent.Value);
			TargetWatcher.Solver.BrentMethodTreat.MaxTryCount = (int)_numTryMaxCountBrent.Value;
			*/

//			CalculateCommand.DontCopySolutionVariable = _checkDontCopyVariable.Checked;
		}


	}
}
