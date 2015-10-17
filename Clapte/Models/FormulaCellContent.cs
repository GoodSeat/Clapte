using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.Solvers.Processes;
using GoodSeat.Liffom.Formulas.Functions;

namespace GoodSeat.Clapte.Models
{
    /// <summary>
    /// 数式セルの計算/定義内容を表します。
    /// </summary>
    /// <remarks>
    /// ・解釈不可
    ///     コメント文字ではないが、数式への変換に失敗するもの
    ///     5 + 等
    /// ・単なる数式
    ///        これ以降の3つのパターンにあたらない式
    ///        5 + a 等
    /// ・定数の単純定義
    ///        a = 3 + b 等
    ///        （a = 2 * a + 3 は、右辺にもaを含むため対象とはならない）
    ///    ・方程式による定数の定義
    ///        未定義の変数をただ一つ含む式
    ///        a^2 + 2 * a + 1 = 0 等
    ///    ・関数の定義
    ///        test(a, b) = a^2 + b 等
    /// </remarks>
    public class FormulaCellContent
    {
        static List<FormulaCellContent> s_protTypes = new List<FormulaCellContent>();
        static FormulaCellContentComment s_commentContent = new FormulaCellContentComment();

        static FormulaCellContent()
        {
            s_protTypes.Add(new FormulaCellContentDefineConstant(null, null, null, null, null));
            s_protTypes.Add(new FormulaCellContentDefineFunction(null, null, null, null));
            s_protTypes.Add(new FormulaCellContentDefineWithEquation(null, null, null, null));
            s_protTypes.Add(new FormulaCellContent(null, null, null));
            s_protTypes.Add(s_commentContent);
        }


        /// <summary>
        /// 数式セルの計算/定義内容を、テキスト種類に応じて初期化して取得します。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        /// <param name="solver">数式の構文解析に用いるソルバ。</param>
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        /// <returns>初期化された数式セル内容オブジェクト。</returns>
        public static FormulaCellContent CreateFormulaCellContent(string formulaText, Solver solver, params FormulaCell[] previous)
        {
            // 速度改善のため、空白文字のみの構成はコメントにしてすぐ返す
            if (string.IsNullOrWhiteSpace(formulaText)) return s_commentContent.CreateFrom(formulaText, solver, previous);

            var proc = solver.GetProcessOf<EvaluateUserDefineProcess>();
            proc.CustomDefineConstants.Clear();
            proc.CustomDefineFunctions.Clear();

            foreach (var cell in previous)
            {
                foreach (var funcName in cell.Content.GetAllDefinedFunctionNames())
                {
                    FunctionDefine def = new FunctionDefine();
                    UserFunction func = new UserFunction(funcName);
                    def.Target = func;
                    proc.CustomDefineFunctions.Add(def);
                }
            }

            foreach (var protType in s_protTypes)
            {
                var content = protType.CreateFrom(formulaText, solver, previous);
                if (content != null) return content;
            }
            return null;
        }

        /// <summary>
        /// 数式セルの計算/定義内容を初期化します。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        /// <param name="f">対象の数式。</param>
        /// <param name="evaluateTarget">具体に評価対象とする数式。</param>
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        protected internal FormulaCellContent(string formulaText, Formula f, Formula evaluateTarget, params FormulaCell[] previous)
        {
            FormulaText = formulaText;
            TargetFormula = f;
            EvaluateTargetFormula = evaluateTarget;

            PreDemandEvaluateFormulaCells = CreatePreDemandEvaluateFormulaCellsList(evaluateTarget, previous);
        }

        /// <summary>
        /// 初期化元となった数式文字列。
        /// </summary>
        public string FormulaText { get; private set; }

        /// <summary>
        /// 評価対象とする数式を取得します。
        /// </summary>
        public Formula TargetFormula { get; private set; }

        /// <summary>
        /// 具体に評価対象とする数式を取得します。
        /// </summary>
        protected Formula EvaluateTargetFormula { get; private set; }

        /// <summary>
        /// 評価結果を表す文字列を取得します。
        /// </summary>
        public string ResultText { get; protected set; }

        /// <summary>
        /// 元の数式を結果のテキストに含めるか否かを設定もしくは取得します。
        /// </summary>
        private bool ContainBaseFormulaInResult { get; set; }

        /// <summary>
        /// この数式セルより先に評価されるべき数式セルの一覧を取得します。
        /// </summary>
        public List<FormulaCell> PreDemandEvaluateFormulaCells { get; private set; }

        /// <summary>
        /// 評価を開始するのに必要な数式セルが既に評価されているかを判定します。
        /// </summary>
        public bool CanEvaluate
        {
            get
            {
                foreach (var cell in PreDemandEvaluateFormulaCells)
                    if (!cell.Evaluated) return false;
                return true;
            }
        }

        /// <summary>
        /// 数式の結果が評価されているか否かを取得します。
        /// </summary>
        public bool Evaluated { get { return ResultText != null; } }


        /// <summary>
        /// 指定文字列から、数式セルの内容を初期化して取得します。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        /// <param name="solver">数式の構文解析に用いるソルバ。</param>
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        /// <returns>初期化された数式セル内容オブジェクト。</returns>
        protected virtual FormulaCellContent CreateFrom(string formulaText, Solver solver, params FormulaCell[] previous)
        {
            bool endWithEqual = false;
            if (formulaText.EndsWith("=")) 
            {
                endWithEqual = true;
                formulaText = formulaText.TrimEnd(' ', '=');
            }

            Formula f;
            if (!solver.TryParse(formulaText, out f)) return null;

            var content = new FormulaCellContent(formulaText, f, f, previous);
            content.ContainBaseFormulaInResult = endWithEqual;
            return content;
        }



        /// <summary>
        /// 指定数式を評価するのにあたって、先に評価されるべき数式セルのリストを取得します。
        /// </summary>
        /// <param name="f">評価対象の数式。</param>
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        /// <returns>先に評価されるべき数式セルリスト。</returns>
        private List<FormulaCell> CreatePreDemandEvaluateFormulaCellsList(Formula f, params FormulaCell[] previous)
        {
            var result = new List<FormulaCell>();
            if (f == null) return result;

            foreach (var variable in f.GetExistFactor<Variable>())
            {
                var mark = variable.Mark;
                bool picked = false;

                for (int i = previous.Length - 1; i >= 0; i--)
                {
                    foreach (var define in previous[i].Content.GetAllDefinedVariableNames())
                    {
                        if (define != mark) continue;

                        result.Add(previous[i]);
                        picked = true;
                        break;
                    }
                    if (picked) break;
                }
            }
            foreach (var func in f.GetExistFactor<UserFunction>())
            {
                var name = func.Name;
                bool picked = false;

                for (int i = previous.Length - 1; i >= 0; i--)
                {
                    foreach (var define in previous[i].Content.GetAllDefinedFunctionNames())
                    {
                        if (define != name) continue;

                        result.Add(previous[i]);
                        picked = true;
                        break;
                    }
                    if (picked) break;
                }
            }
            return result;
        }



        /// <summary>
        /// この数式セルで定義される変数名をすべて返す反復子を取得します。
        /// </summary>
        public virtual IEnumerable<string> GetAllDefinedVariableNames() { yield break; }

        /// <summary>
        /// この数式セルで定義される関数名をすべて返す反復子を取得します。
        /// </summary>
        public virtual IEnumerable<string> GetAllDefinedFunctionNames() { yield break; }


        /// <summary>
        /// この数式セルで参照する定数名をすべて返す反復子を取得します。
        /// </summary>
        public IEnumerable<string> GetAllReferenceVariableNames()
        {
            if (EvaluateTargetFormula == null) yield break;
            foreach (var varialble in EvaluateTargetFormula.GetExistFactor<Variable>())
            {
                yield return varialble.Mark;
            }
        }

        /// <summary>
        /// この数式セルで参照するユーザー定義関数名をすべて返す反復子を取得します。
        /// </summary>
        public IEnumerable<string> GetAllReferenceFunctionNames()
        {
            if (EvaluateTargetFormula == null) yield break;
            foreach (var func in EvaluateTargetFormula.GetExistFactor<UserFunction>())
            {
                yield return func.Name;
            }
        }


        /// <summary>
        /// この数式セルの数式を評価します。
        /// </summary>
        /// <param name="solver">評価に用いるソルバ。</param>
        public void Evaluate(Solver solver)
        {
            if (!CanEvaluate) throw new InvalidOperationException("数式を評価するのに必要な他の数式セルで、評価が未遂です。");
            if (Evaluated) throw new InvalidOperationException("すでに数式の評価が実施されています。");

            // 前方の数式セルで定義された定義をソルバに設定
            var proc = solver.GetProcessOf<EvaluateUserDefineProcess>();
            proc.CustomDefineConstants.Clear();
            proc.CustomDefineFunctions.Clear();

            foreach (var name in GetAllReferenceVariableNames())
            {
                foreach (var cell in PreDemandEvaluateFormulaCells)
                {
                    var define = cell.GetVariableDefineOf(name);
                    if (define == null) continue;

                    proc.CustomDefineConstants.Add(define);
                    break;
                }
            }
            foreach (var name in GetAllReferenceFunctionNames())
            {
                foreach (var cell in PreDemandEvaluateFormulaCells)
                {
                    var define = cell.GetFunctionDefineOf(name);
                    if (define == null) continue;

                    proc.CustomDefineFunctions.Add(define);
                    break;
                }
            }

            // 結果をセット
            var result = OnEvaluate(solver);
            ResultText = result.ResultText;
            if (result.ResultLevel != Result.Level.Success) ResultText = "!!! " + ResultText;
            else if (ContainBaseFormulaInResult) ResultText = FormulaText + " = " + ResultText;
        }

        /// <summary>
        /// この数式セルの数式を評価します。
        /// </summary>
        /// <param name="solver">評価に用いるソルバ。</param>
        /// <returns>評価結果を表す文字列。</returns>
        protected virtual Result OnEvaluate(Solver solver)
        {
            try
            {
                return solver.Solve(FormulaText);
            }
            catch (Exception e)
            {
                return new Result(Result.Level.Error, e.Message, null, new Error(Error.Level.Error, e.Message));
            }
        }


        /// <summary>
        /// この数式セルの評価で定義されるすべての定数定義を返す反復子を取得します。
        /// </summary>
        public virtual IEnumerable<ConstantDefine> GetAllConstantDefines() { yield break; }

        /// <summary>
        /// この数式セルの評価で定義されるすべての関数定義を返す反復子を取得します。
        /// </summary>
        public virtual IEnumerable<FunctionDefine> GetAllFunctionDefines() { yield break; }


    }
}

