// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Liffom.Processes;
using GoodSeat.Clapte.Solvers.Processes;

namespace GoodSeat.Clapte.Models
{
    /// <summary>
    /// 連立方程式の求解による定数の定義を表す数式セルの計算/定義内容を表します。
    /// </summary>
    public class FormulaCellContentDefineWithSimultaneousEquation : FormulaCellContent
    {
        /// <summary>
        /// 連立方程式の求解による変数の定義を表す数式セルの計算/定義内容を初期化します。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        public FormulaCellContentDefineWithSimultaneousEquation(string formulaText) : base(formulaText, null, null, null)
        {
            ResultText = "";
            IsTerminator = false;
        }

        /// <summary>
        /// 連立方程式の求解による変数の定義を表す数式セルの計算/定義内容を初期化します。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        /// <param name="f">対象の数式。</param>
        /// <param name="target">定義対象の変数。</param>
        /// <param name="evaluateTarget">具体に評価対象とする数式。</param>
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        protected internal FormulaCellContentDefineWithSimultaneousEquation(string formulaText, List<Variable> targets, IEnumerable<Equal> evaluateTarget, FormulaCell[] previous)
            : base(formulaText, new Argument(evaluateTarget.ToArray()), new Argument(evaluateTarget.ToArray()), previous, targets.Select(v => v.Mark))
        {
            DefineTargets = targets;
            TargetEquations = new List<Equal>(evaluateTarget);
            IsTerminator = true;
        }

        /// <summary>
        /// このセルコンテンツが、連立方程式の最終行で計算を行う本体セルであるか否かを取得します。
        /// </summary>
        public bool IsTerminator { get; private set; }

        /// <summary>
        /// この数式セルが実際の評価を行わず、後続の行に評価を移譲するか否かを取得します。
        /// </summary>
        public override bool IsContinuation { get { return !IsTerminator; } }

        /// <summary>
        /// 定義対象とする変数リストを取得します。
        /// </summary>
        public List<Variable> DefineTargets { get; private set; } = new List<Variable>();

        /// <summary>
        /// 対象とする等式リストを取得します。
        /// </summary>
        public List<Equal> TargetEquations { get; private set; }

        /// <summary>
        /// 各変数の解マップを取得します。
        /// </summary>
        public Dictionary<Variable, List<Formula>> EvaluatedDefines { get; private set; } = new Dictionary<Variable, List<Formula>>();

        /// <summary>
        /// この数式セルで定義される変数名をすべて返す反復子を取得します。
        /// </summary>
        public override IEnumerable<string> GetAllDefinedVariableNames() { return DefineTargets.Select(v => v.Mark); }

        /// <summary>
        /// この数式セルの評価で定義されるすべての定数定義を返す反復子を取得します。
        /// </summary>
        public override IEnumerable<ConstantDefine> GetAllConstantDefines()
        {
            return EvaluatedDefines.Select(pair =>
            {
                var def = new ConstantDefine(pair.Key.Mark);
                if (pair.Value.Count == 1) def.Define = pair.Value[0].ToString();
                else def.Define = string.Join(", ", pair.Value.Select(f => f.ToString()));
                return def;
            });
        }

        /// <summary>
        /// 指定文字列から、数式セルの内容を初期化して取得します。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        /// <param name="solver">数式の構文解析に用いるソルバ。</param>
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        /// <returns>初期化された数式セル内容オブジェクト。</returns>
        protected override FormulaCellContent CreateFrom(string formulaText, Solver solver, params FormulaCell[] previous)
        {
            if (!formulaText.Trim().StartsWith("{")) return null;
            if (formulaText.Count(c => c == '{') != formulaText.Count(c => c == '}') + 1) return null;

            if (!formulaText.Trim().StartsWith("{_")) return new FormulaCellContentDefineWithSimultaneousEquation(formulaText.Trim().Substring(1));

            var targetTexts = new List<string>();
            targetTexts.Add(formulaText.Trim().Substring(2));
            foreach (var content in previous.Reverse().Select(cell => cell.Content))
            {
                var preContent = content as FormulaCellContentDefineWithSimultaneousEquation;
                if (preContent == null || preContent.IsTerminator) break;
                targetTexts.Add(content.FormulaText);
            }

            Func<string, Formula> parse = t => { Formula f; solver.TryParse(t, out f); return f; };
            var fs = new List<Formula>(targetTexts.Select(parse).Where(f => f != null));

            var targetVariables = new List<Variable>();
            if (fs.Any(f => f is Argument)) // 求解対象とする変数リストあり
            {
                Argument arg = fs.OfType<Argument>().First();
                if (arg.Any(v => !(v is Variable || v is Constant || v is Unit))) return null;
                targetVariables.AddRange(arg.Select(v => new Variable(v.ToString())));

                fs.Remove(arg);
            }
            else // 求解対象とする変数を自動判定
            {
                targetVariables.AddRange(fs.SelectMany(f => f.GetExistFactors<Variable>())
                                           .Distinct()
                                           .Where(v => !FormulaCellContentDefineWithEquation.IsDefined(v, solver, previous)));
                if (targetVariables.Count == 0)
                {
                    targetVariables.AddRange(fs.SelectMany(f => f.GetExistFactors<Variable>()).Distinct());
                }
            }
            if (targetVariables.Count == 0) return null;
            if (!fs.All(f => f is Equal)) return null;

            foreach (var f in fs) f.Format = solver.OutputFormat;
            return new FormulaCellContentDefineWithSimultaneousEquation(string.Join(", ", fs.Select(f => f.ToString())), targetVariables, fs.OfType<Equal>(), previous);
        }

        /// <summary>
        /// 指定文字列から、評価にあたって計算処理の不要な数式セルの内容の初期化を試みます。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        /// <param name="solver">数式の構文解析に用いるソルバ。</param>
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        /// <returns>初期化された数式セル内容オブジェクト。生成対象とならなかった場合には、null。</returns>
        protected override FormulaCellContent TryCreateNoCalculateTarget(string formulaText, Solver solver, params FormulaCell[] previous)
        {
            if (!formulaText.Trim().StartsWith("{")) return null;
            if (formulaText.Count(c => c == '{') != formulaText.Count(c => c == '}') + 1) return null;
            if (formulaText.Trim().StartsWith("{_")) return null;

            return new FormulaCellContentDefineWithSimultaneousEquation(formulaText.Trim().Substring(1));
        }

        /// <summary>
        /// この数式セルの数式を評価します。
        /// </summary>
        /// <param name="solver">評価に用いるソルバ。</param>
        /// <returns>評価結果を表す文字列。</returns>
        protected override Result OnEvaluate(Solver solver)
        {
            var evaluateUserDefineProc = solver.GetProcessOf<EvaluateUserDefineProcess>();
            var replaceUnitProc = solver.GetProcessOf<ReplaceVariableToUnitProcess>();
            foreach (var variableName in DefineTargets.Select(v => v.Mark))
            {
                ConstantDefine delete = null;
                foreach (var def in evaluateUserDefineProc.CustomDefineConstants)
                {
                    if (def.Name != variableName) continue;
                    delete = def;
                    break;
                }
                if (delete != null) evaluateUserDefineProc.CustomDefineConstants.Remove(delete);
                replaceUnitProc.IgnoreVariableNames.Add(variableName);
            }

            Formula parsed;
            var result = solver.Solve(FormulaText, out parsed);
            TargetFormula = parsed;

            replaceUnitProc.IgnoreVariableNames.Clear();

            if (result.ResultLevel == Result.Level.Success)
            {
                result.ResultText = "連立方程式の求解に失敗しました。";
                result.ResultLevel = Result.Level.Error;
                var eqs = result.ResultFormula as Argument;
                if (eqs == null || eqs.Any(f => !(f is Equal))) return result;

                try
                {
                    var proc = new SolveSimultaneousEquation();
                    proc.CheckSignificantDigits = Numeric.ConsiderSignificantDigitsInDeforming;
                    var solutions = proc.Solve(new List<Equal>(eqs.OfType<Equal>()), DefineTargets.ToArray());
                    if (solutions == null || solutions.Count == 0) return result;

                    var solutionsDeformed = new List<List<Equal>>();
                    foreach (var solution in solutions)
                    {
                        var solutionDeformed = new List<Equal>();
                        foreach (var sol in solution)
                        {
                            Formula f = sol;
                            solver.GetProcessOf<CalculateFormulaProcess>().EvaluateFormula(ref f);
                            solutionDeformed.Add(f as Equal);
                        }
                        solutionsDeformed.Add(solutionDeformed);
                    }

                    List<string> resultTexts = CreateResultTexts(solutionsDeformed, solver.OutputFormat);

                    result.ResultLevel = Result.Level.Success;
                    result.ResultText = resultTexts.FirstOrDefault();
                    if (resultTexts.Count > 1)
                    {
                        result.ResultText = string.Join(", ", resultTexts.Select(s => "(" + s + ")"));
                    }
                }
                catch
                {
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// 連立方程式の解の組合せから、結果表記のテキストリストを生成して取得します。
        /// </summary>
        /// <param name="solutions">連立方程式の解の組合せ。</param>
        /// <param name="format">テキストの生成に使用する書式。</param>
        /// <returns>連立方程式の結果を表すテキストのリスト。</returns>
        private List<string> CreateResultTexts(List<List<Equal>> solutions, Liffom.Formats.Format format)
        {
            var resultTexts = new List<string>();
            foreach (var defs in solutions)
            {
                foreach (var def in defs)
                {
                    Variable val = def.LeftHandSide as Variable;
                    if (val == null) throw new ClapteProcessException();

                    def.Format = format;

                    if (EvaluatedDefines.ContainsKey(val))
                    {
                        EvaluatedDefines[val].Add(def.RightHandSide);
                    }
                    else
                    {
                        var list = new List<Formula>();
                        list.Add(def.RightHandSide);
                        EvaluatedDefines.Add(val, list);
                    }
                }
                resultTexts.Add(string.Join(", ", defs.Select(s => s.ToString())));
            }

            return resultTexts;
        }


    }
}
