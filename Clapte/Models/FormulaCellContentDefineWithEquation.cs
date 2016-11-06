using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.Solvers.Processes;

namespace GoodSeat.Clapte.Models
{
    /// <summary>
    /// 方程式の求解による定数の定義を表す数式セルの計算/定義内容を表します。
    /// </summary>
    public class FormulaCellContentDefineWithEquation : FormulaCellContent
    {
        /// <summary>
        /// 方程式の求解による変数の定義を表す数式セルの計算/定義内容を初期化します。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        /// <param name="f">対象の数式。</param>
        /// <param name="target">定義対象の変数。</param>
        /// <param name="evaluateTarget">具体に評価対象とする数式。</param>
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        protected internal FormulaCellContentDefineWithEquation(string formulaText, Formula f, Variable target, Formula evaluateTarget, params FormulaCell[] previous)
            : base(formulaText, f, evaluateTarget, previous)
        {
            DefineTarget = target;
        }

        /// <summary>
        /// 定義対象の変数を取得します。
        /// </summary>
        public Variable DefineTarget { get; private set; }

        /// <summary>
        /// 評価で得られた定数定義を取得します。
        /// </summary>
        public ConstantDefine EvaluatedDefine { get; private set; }

        /// <summary>
        /// 指定文字列から、数式セルの内容を初期化して取得します。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        /// <param name="solver">数式の構文解析に用いるソルバ。</param>
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        /// <returns>初期化された数式セル内容オブジェクト。</returns>
        protected override FormulaCellContent CreateFrom(string formulaText, Solver solver, params FormulaCell[] previous)
        {
            formulaText = formulaText.Replace("＝", "=");
            if (!formulaText.Contains("=")) return null;

            Formula f;
            solver.TryParse(formulaText, out f);

            var equal = f as Equal;
            if (equal == null) return null;

            var variables = f.GetExistFactors<Variable>();
            if (variables.Count() == 0) // 変数が一つもない。
                return null;
            else if (variables.Count() == 1) // 変数が一つだけ存在。
                return new FormulaCellContentDefineWithEquation(formulaText, f, variables.First(), f, previous);

            var permanentTarget = new Variable(SolveEquationProcess.PermanentSolveTarget);
            if (variables.Contains(permanentTarget)) return null; // 恒久的な求解対象が存在する ⇒ 変数定義とはみなさない

            var noDefinedList = variables.Where(v => !IsDefined(v, solver, previous));
            if (noDefinedList.Count() != 1) return null; // 未定義の変数が1つでない。

            return new FormulaCellContentDefineWithEquation(formulaText, f, noDefinedList.First() as Variable, f, previous);
        }

        /// <summary>
        /// 指定変数の値が定義されているか否かを取得します。
        /// </summary>
        /// <param name="variable">判定対象の変数。</param>
        /// <param name="solver">数式の構文解析に用いるソルバ。</param>
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        /// <returns>定義されているか否か。</returns>
        public static bool IsDefined(Variable variable, Solver solver, params FormulaCell[] previous)
        {
            foreach (var constant in Constant.GetEnableConstants())
                foreach (var name in constant.GetAllDistinguishedNames())
                    if (name == variable.Mark) return true;

            foreach (var constant in solver.UserConstants)
                if (constant.Name == variable.Mark) return true;

            foreach (var cell in previous)
                foreach (var constantName in cell.Content.GetAllDefinedVariableNames())
                    if (constantName == variable.Mark) return true;

            return false;
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
                var equal = result.ResultFormula as Equal;

                EvaluatedDefine = new ConstantDefine(DefineTarget.Mark);
                if (equal.LeftHandSide == DefineTarget)
                {
                    equal.RightHandSide.Format = solver.OutputFormat;
                    EvaluatedDefine.Define = equal.RightHandSide.ToString();
                    result.ResultText = string.Format("{0} = {1}", DefineTarget, EvaluatedDefine.Define);
                }
                else
                {
                    result.ResultLevel = Result.Level.Error;
                    result.ResultText = string.Format("{0}についての求解に失敗しました。", DefineTarget);
                }
            }
            return result;
        }


        /// <summary>
        /// この数式セルの評価で定義されるすべての定数定義を返す反復子を取得します。
        /// </summary>
        public override IEnumerable<ConstantDefine> GetAllConstantDefines() { yield return EvaluatedDefine; }


    }
}
