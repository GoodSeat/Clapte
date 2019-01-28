using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.Solvers.Processes;
using GoodSeat.Liffom.Formulas.Constants;

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
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        protected internal FormulaCellContentDefineConstant(string formulaText, Formula f, Variable target, Formula evaluateTarget, params FormulaCell[] previous)
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
            if (formulaText.TrimEnd(' ').EndsWith("=")) return null;

            var preText = formulaText;
            {
                var unitProc = new ConvertToSpecifiedUnitProcess(solver);
                unitProc.CheckInputText(ref formulaText, true);
            }
            var targetUnitText = preText.Substring(0, preText.Length - formulaText.Length); // [cm] 等の部分

            Formula f;
            solver.TryParse(formulaText, out f);

            var equal = f as Equal;
            if (equal == null) return null;

            var target = equal.LeftHandSide as Variable;
            if (target == null)
            {
                if (equal.LeftHandSide is Constant) target = new Variable(equal.LeftHandSide.ToString());
                else return null;
            }
            if (target.Mark == SolveEquationProcess.PermanentSolveTarget) return null; // "?"は変数名として許可しない

            if (equal.RightHandSide.Contains(target)) return null;

            string[] split = formulaText.Split('=');

            var text = targetUnitText + formulaText.Substring(split[0].Length + 1);
            return new FormulaCellContentDefineConstant(text, f, target, equal.RightHandSide, previous);
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
            if (Constant.GetEnableConstants().FirstOrDefault(cst => cst.GetAllDistinguishedNames().Contains(DefineTarget.Mark)) != null)
                throw new ClapteProcessException(string.Format("変数名 {0} はシステムで定義されているため、再定義できません。", DefineTarget.Mark));

            Formula f;
            var result = solver.Solve(FormulaText, out f);
            TargetFormula = f;

            if (result.ResultLevel == Result.Level.Success)
            {
                EvaluatedDefine = new ConstantDefine(DefineTarget.Mark);
                EvaluatedDefine.Define = result.ResultFormula.ToString();

                result.ResultText = string.Format("{0} = {1}", DefineTarget, EvaluatedDefine.Define);
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


