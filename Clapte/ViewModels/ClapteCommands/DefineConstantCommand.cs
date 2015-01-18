using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Sio.Xml;
using GoodSeat.Sio.Xml.Serialization;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Parse;

namespace GoodSeat.Clapte.ViewModels.ClapteCommands
{
	/// <summary>
    /// ユーザー定義定数の追加コマンドを表します。
	/// </summary>
	public class DefineConstantCommand : ClapteCommand
	{
		/// <summary>
		/// 定数の定義コマンド文字列を検知する正規表現オブジェクトを設定もしくは取得します。
		/// </summary>
		static Regex DefineRegex { get; set; }

		static DefineConstantCommand()
		{
			DefineRegex = new Regex(@"^\s*(?<name>\S+)\s*\:\=\s*(?<define>[^【]+)(【(?<comment>.*)】)?");
		}

		/// <summary>
		/// ユーザー定義定数の追加コマンドを表します。
		/// </summary>
		/// <param name="owner">親となるClapteCoreModelViewオブジェクト。</param>
		public DefineConstantCommand(ClapteCoreViewModel owner)
			: base(owner) 
		{
		}

		/// <summary>
		/// 追加候補となるユーザー定義定数の定義オブジェクトを設定もしくは取得します。
		/// </summary>
		private ConstantDefine Target { get; set; }

		/// <summary>
		/// <see cref="Target"/>と同名ユーザー定義定数の定義オブジェクトを設定もしくは取得します。
		/// </summary>
		private ConstantDefine AlreadyExist { get; set; }

		/// <summary>
		/// 追加対象となるユーザー定義定数リストを取得します。
		/// </summary>
		private IList<ConstantDefine> TargetList { get { return Owner.Solver.Target.UserConstants; } }

		/// <summary>
		/// 数式の構文解析器を取得します。
		/// </summary>
		private FormulaParser Parser { get { return Owner.Solver.Target.Parser; } }

		/// <summary>
		/// 指定した文字列から変数定義を取り出します。
		/// </summary>
		/// <param name="targetText">判定対象の文字列。</param>
		/// <returns>取り出された変数定義。取り出せない場合にはnull。</returns>
		public ConstantDefine PickDefineFrom(string targetText)
		{
			var funcLexer = Parser.GetLexerOf<FunctionLexer>();
			try
			{
				funcLexer.ScanNotDefinedUserFunction = true;

				var match = DefineRegex.Match(targetText);
				if (!match.Success) return null;

				string name = match.Groups["name"].Value;
				Formula f;
				if (!Parser.TryParse(name, out f)) return null;
				if (!(f is Variable) && !(f is Unit)) return null;

				string defText = match.Groups["define"].Value.Trim();
				Formula define;
				if (!Parser.TryParse(defText, out define)) return null;

				var constantDefine = new ConstantDefine(name);
				constantDefine.Define = defText;

				if (match.Groups["comment"] != null) constantDefine.Information = match.Groups["comment"].Value.Trim();

				return constantDefine;
			}
			finally
			{
				funcLexer.ScanNotDefinedUserFunction = false;
			}
		}


		public override ClapteCommandResult TryInitializeCommand(string text)
		{
            string targetText = text.Replace("\n","").Replace("\r","");

			Target = PickDefineFrom(targetText);
			if (Target == null) return null;

			AlreadyExist = null;
			foreach (var exist in TargetList)
			{
				if (exist.Name == Target.Name)
				{
					AlreadyExist = exist;
					break;
				}
			}

			string message = string.Format("操作の続行により、ユーザー定義定数 {0} を追加します。", Target.Name);
			if (AlreadyExist != null)
			{
				message = string.Format("操作の続行により、ユーザー定義定数 {0} の定義を更新します。\n※ 現在の定義 {0}={1}【{2}】",
                                                Target.Name, AlreadyExist.Define, AlreadyExist.Information);
			}
			return new ClapteCommandResult("ユーザー定義定数の定義", message, ToolTipIcon.Info);
		}

		public override ClapteCommandResult DoAction()
		{
			if (Target == null) return null;
			if (AlreadyExist != null) TargetList.Remove(AlreadyExist);
			TargetList.Add(Target);

			string title = "ユーザー定義定数の定義";
			string message = string.Format("ユーザー定義定数 {0} を{1}しました。", Target.Name, 
					(AlreadyExist == null) ? "追加" : "更新");

			Target = null;
			return new ClapteCommandResult(title, message, ToolTipIcon.Info);
		}

	}
}
