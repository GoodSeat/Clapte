using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Sio.Xml;
using GoodSeat.Sio.Xml.Serialization;

namespace GoodSeat.Clapte.ViewModels.ClapteCommands
{
	/// <summary>
	/// 数式の計算を実行するコマンドを表します。
	/// </summary>
	public class CalculateCommand : ClapteCommand, ISerializable
	{
		/// <summary>
		/// 数式の計算を実行するコマンドを初期化します。
		/// </summary>
		/// <param name="owner">親となるClapteCoreModelViewオブジェクト。</param>
		public CalculateCommand(ClapteCoreViewModel owner)
			: base(owner) 
		{
			Solver = new SolverViewModel();
		}

		/// <summary>
		/// 最後に成功した計算の計算結果を設定もしくは取得します。
		/// </summary>
		public Result LastResult { get; set; }

		/// <summary>
		/// 計算に使用するSolverViewModelを設定もしくは取得します。
		/// </summary>
		public SolverViewModel Solver { get; set; }

		public override ClapteCommandResult TryInitializeCommand(string text)
		{
			var result = Solver.Target.Solve(text);

			if (result != null && result.ResultLevel == Result.Level.Success)
			{
				LastResult = result;
				return new ClapteCommandResult("計算結果", result.ResultText, ToolTipIcon.Info);
			}
			else
			{
				return null;
			}
		}

		public override ClapteCommandResult DoAction()
		{
			try
			{
				Clipboard.SetText(LastResult.ResultText);
				return new ClapteCommandResult("結果コピー", string.Format("\"{0}\"をコピーしました。", LastResult.ResultText), ToolTipIcon.Info);
			}
			catch
			{
				return new ClapteCommandResult("結果コピー失敗", "結果のコピーに失敗しました。\nこのメッセージが表示されている間、再試行できます。", ToolTipIcon.Error);
			}
		}

		#region ISerializable メンバー

		public void OnDeserialize(XmlElement xmlElement)
		{
			Solver.OnDeserialize(xmlElement);
		}

		public void OnSerialize(XmlElement xmlElement)
		{
			Solver.OnSerialize(xmlElement);
		}

		#endregion
	}
}
