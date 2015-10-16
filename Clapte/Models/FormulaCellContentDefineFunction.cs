using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Clapte.ViewModels.ClapteCommands;
using GoodSeat.Clapte.Solvers.Processes;

namespace GoodSeat.Clapte.Models
{
    /// <summary>
    /// 関数の定義を表す数式セルの計算/定義内容を表します。
    /// </summary>
	public class FormulaCellContentDefineFunction : FormulaCellContent
	{
		/// <summary>
		/// 関数の定義コマンド文字列を検知する正規表現オブジェクトを設定もしくは取得します。
		/// </summary>
		static Regex DefineRegex { get; set; }

		static FormulaCellContentDefineFunction()
		{
			DefineRegex = new Regex(@"(?<name>[^( ]+)\((?<argument>[^)]*)\)\s*\=\s*(?<define>.+)");
		}

		/// <summary>
		/// 関数の定義を表す数式セルの計算/定義内容を初期化します。
		/// </summary>
		/// <param name="formulaText">初期化対象のテキスト。</param>
		/// <param name="f">対象の数式。</param>
		/// <param name="target">定義対象の関数。</param>
		/// <param name="evaluateTarget">具体に評価対象とする数式。</param>
		/// <param name="previous">前方に宣言されている可変数の数式セル。</param>
		protected internal FormulaCellContentDefineFunction(string formulaText, Formula f, UserFunction target, Formula evaluateTarget, params FormulaCell[] previous)
			: base(formulaText, f, evaluateTarget, previous)
		{
			DefineTarget = target;
		}

		/// <summary>
		///	定義対象の関数を取得します。
		/// </summary>
		public UserFunction DefineTarget { get; private set; }

		/// <summary>
		/// 評価で得られた定数定義を取得します。
		/// </summary>
		public FunctionDefine EvaluatedDefine { get; private set; }

		/// <summary>
		/// 指定文字列から、数式セルの内容を初期化して取得します。
		/// </summary>
		/// <param name="formulaText">初期化対象のテキスト。</param>
		/// <param name="solver">数式の構文解析に用いるソルバ。</param>
		/// <param name="previous">前方に宣言されている可変数の数式セル。</param>
		/// <returns>初期化された数式セル内容オブジェクト。</returns>
		protected override FormulaCellContent CreateFrom(string formulaText, Solver solver, params FormulaCell[] previous)
		{
			var match = DefineRegex.Match(formulaText);
			if (!match.Success) return null;

			string name = match.Groups["name"].Value;
			Formula f;
			if (!solver.TryParse(name, out f)) return null;
			if (!(f is Variable) && !(f is Unit)) return null;

			var target = new UserFunction(name);

			var useVariables = DefineFunctionCommand.GetUseVariableList(match, solver.Parser);
			if (useVariables == null) return null;
			target.UseVariable = useVariables;

			string defText = match.Groups["define"].Value.Trim(); 
			Formula define;
			if (!solver.TryParse(defText, out define)) return null;

			return new FormulaCellContentDefineFunction(defText, define, target, define, previous);
		}

		/// <summary>
		/// この数式セルで定義される変数名をすべて返す反復子を取得します。
		/// </summary>
		public override IEnumerable<string> GetAllDefinedFunctionNames() { yield return DefineTarget.Name; }

		/// <summary>
		/// この数式セルの数式を評価します。
		/// </summary>
		/// <param name="solver">評価に用いるソルバ。</param>
		/// <returns>評価結果を表す文字列。</returns>
		protected override Result OnEvaluate(Solver solver)
		{
			var evaluateUserDefineProc = solver.GetProcessOf<EvaluateUserDefineProcess>();
			var replaceUnitProc = solver.GetProcessOf<ReplaceVariableToUnitProcess>();
			foreach (var variable in DefineTarget.UseVariable)
			{
				ConstantDefine delete = null;
				foreach (var def in evaluateUserDefineProc.CustomDefineConstants)
				{
					if (def.Name != variable.Mark) continue;
					delete = def;
					break;
				}
				if (delete != null) evaluateUserDefineProc.CustomDefineConstants.Remove(delete);
				replaceUnitProc.IgnoreVariableNames.Add(variable.Mark);
			}

			var result = solver.Solve(FormulaText);

			replaceUnitProc.IgnoreVariableNames.Clear();

			if (result.ResultLevel == Result.Level.Success)
			{
				EvaluatedDefine = new FunctionDefine(DefineTarget);
				EvaluatedDefine.Define = FormulaText;
				DefineTarget.UseFormula = result.ResultFormula;

				result.ResultText = string.Format("{0} = {1}", DefineTarget.NameForView, result.ResultFormula);
			}
            return result;
		}


		/// <summary>
		/// この数式セルの評価で定義されるすべての定数定義を返す反復子を取得します。
		/// </summary>
		public override IEnumerable<FunctionDefine> GetAllFunctionDefines() 
        {
            if (EvaluatedDefine == null) yield break;
            yield return EvaluatedDefine;
        }


	}
}
