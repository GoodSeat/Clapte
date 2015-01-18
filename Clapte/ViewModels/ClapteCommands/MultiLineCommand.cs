using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Clapte.Solvers;

namespace GoodSeat.Clapte.ViewModels.ClapteCommands
{
	/// <summary>
    /// 多行の連続実行処理のコマンドを表します。
	/// </summary>
	public class MultiLineCommand : ClapteCommand
	{
		/// <summary>
		/// 多行の連続実行処理のコマンドを初期化します。
		/// </summary>
		/// <param name="owner">親となるClapteCoreModelViewオブジェクト。</param>
		public MultiLineCommand(ClapteCoreViewModel owner)
			: base(owner) 
		{
			CommandList = new List<ClapteCommand>();
		}

		/// <summary>
		/// 実行対象とする初期化済みコマンドリストを設定もしくは取得します。
		/// </summary>
		private List<ClapteCommand> CommandList { get; set; }

		/// <summary>
		/// 指定したテキストから、対応するコマンドを初期化して取得します。
		/// </summary>
		/// <param name="line">コマンドの初期化に使用する文字列。</param>
		/// <returns>初期化されたコマンド。対応するコマンドがない場合には、null。</returns>
		private ClapteCommand TryInitialize(string line)
		{
			var candidates = new List<ClapteCommand>();
			candidates.Add(new DefineConstantCommand(Owner));
			candidates.Add(new DefineFunctionCommand(Owner));
			candidates.Add(new DeleteConstantCommand(Owner));
			candidates.Add(new DeleteFunctionCommand(Owner));

			foreach (var command in candidates)
			{
				var result = command.TryInitializeCommand(line);
				if (result != null) return command;
			}
			return null;
		}

		public override ClapteCommandResult TryInitializeCommand(string text)
		{
			var lines = text.Replace("\r","").Split('\n');
			CommandList.Clear();

			foreach (var line in lines)
			{
				if (string.IsNullOrEmpty(line)) continue;

				var command = TryInitialize(line);
				if (command == null) return null;
				CommandList.Add(command);
			}
			if (CommandList.Count == 0) return null;

			string title = "複数処理の連続実行";
			string message = string.Format("操作の続行により、以下の処理を連続実行します。\n{0}", text);

			return new ClapteCommandResult(title, message, ToolTipIcon.Info);
		}

		public override ClapteCommandResult DoAction()
		{
			if (CommandList.Count == 0) return null;

			string message = "";
			foreach (var command in CommandList)
			{
				var result = command.DoAction();
				if (result == null) continue;
				message += result.Message + "\n";
			}
			CommandList.Clear();

			string title = "複数処理の連続実行";
			return new ClapteCommandResult(title, message.TrimEnd('\n'), ToolTipIcon.Info);
		}

	}
}



