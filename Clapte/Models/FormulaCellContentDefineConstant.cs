using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.Solvers.Processes;

namespace GoodSeat.Clapte.Models
{
    /// <summary>
    /// 変数の定義を表す数式セルの計算/定義内容を表します。
    /// </summary>
    public class FormulaCellContentDefineConstant : FormulaCellContent
    {
		/// <summary>
		/// 変数の定義を表す数式セルの計算/定義内容を初期化します。
		/// </summary>
		/// <param name="formulaText">初期化対象のテキスト。</param>
		/// <param name="f">対象の数式。</param>
		/// <param name="target">定義対象の変数。</param>
		/// <param name="evaluateTarget">具体に評価対象とする数式。</param>
		/// <param name="unitProc">目標単位を保持する単位変換処理。</param>
		/// <param name="previous">前方に宣言されている可変数の数式セル。</param>
		protected internal FormulaCellContentDefineConstant(string formulaText, Formula f, Variable target, Formula evaluateTarget, ConvertToSpecifiedUnitProcess unitProc, params FormulaCell[] previous)
			: base(formulaText, f, evaluateTarget, previous)
		{
			DefineTarget = target;
            UnitProcess = unitProc;
		}

		/// <summary>
		///	定義対象の変数を取得します。
		/// </summary>
		public Variable DefineTarget { get; private set; }

		/// <summary>
		/// 評価で得られた定数定義を取得します。
		/// </summary>
		public ConstantDefine EvaluatedDefine { get; private set; }

        /// <summary>
        /// 目標単位の検知と変換を行う処理を設定若しくは取得します。
        /// </summary>
        private ConvertToSpecifiedUnitProcess UnitProcess { get; set; }

		/// <summary>
		/// 指定文字列から、数式セルの内容を初期化して取得します。
		/// </summary>
		/// <param name="formulaText">初期化対象のテキスト。</param>
		/// <param name="solver">数式の構文解析に用いるソルバ。</param>
		/// <param name="previous">前方に宣言されている可変数の数式セル。</param>
		/// <returns>初期化された数式セル内容オブジェクト。</returns>
		protected override FormulaCellContent CreateFrom(string formulaText, Solver solver, params FormulaCell[] previous)
		{
			if (!formulaText.Contains("=")) return null;
            if (formulaText.TrimEnd(' ').EndsWith("=")) return null;

            var unitProc = new ConvertToSpecifiedUnitProcess(solver, Formula.CombineToken);
            unitProc.CheckInputText(ref formulaText);

			Formula f;
			solver.TryParse(formulaText, out f);

			var equal = f as Equal;
			if (equal == null) return null;

			var target = equal.LeftHandSide as Variable;
			if (target == null) return null;
            if (target.Mark == SolveEquationProcess.PermanentSolveTarget) return null; // "?"は変数名として許可しない

			if (equal.RightHandSide.Contains(target)) return null;

            string[] split = formulaText.Split('=');

			return new FormulaCellContentDefineConstant(formulaText.Substring(split[0].Length + 1), f, target, equal.RightHandSide, unitProc, previous);
		}

		/// <summary>
		/// この数式セルで定義される変数名をすべて返す反復子を取得します。
		/// </summary>
		public override IEnumerable<string> GetAllDefinedVariableNames() { yield return DefineTarget.Mark; }

		/// <summary>
		/// この数式セルの数式を評価します。
		/// </summary>
		/// <param name="solver">評価に用いるソルバ。</param>
		/// <returns>評価結果を表す文字列。</returns>
		protected override Result OnEvaluate(Solver solver)
		{
			var result = solver.Solve(FormulaText);

			if (result.ResultLevel == Result.Level.Success)
			{
				EvaluatedDefine = new ConstantDefine(DefineTarget.Mark);
                var res = result.ResultFormula;
                UnitProcess.CheckOutputFormula(ref res);
				EvaluatedDefine.Define = res.ToString();

                result.ResultText  = string.Format("{0} = {1}", DefineTarget, res.ToString());
			}
            return result;
		}


		/// <summary>
		/// この数式セルの評価で定義されるすべての定数定義を返す反復子を取得します。
		/// </summary>
		public override IEnumerable<ConstantDefine> GetAllConstantDefines() 
        {
            if (EvaluatedDefine == null) yield break;
            yield return EvaluatedDefine;
        }


    }
}


