using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
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
    ///        コメント文字ではないが、数式への変換に失敗するもの。
    ///        5 + 等
    /// ・継続行
    ///        行の末尾が " _" で終わる行。後続の行で併せて評価される。
    ///        1 + 2 + 3 + 4 + 5 + _ 等
    /// ・定数の単純定義
    ///        a = 3 + b 等
    ///        （a = 2 * a + 3 は、右辺にもaを含むため対象とはならない）
    /// ・方程式による定数の定義
    ///        未定義の変数をただ一つ含む式。
    ///        a^2 + 2 * a + 1 = 0 等
    /// ・連立方程式による定数の定義
    ///        行の先頭が "{" あるいは "{_"(連立方程式の終端を示す) で始まり、当該波括弧が閉じない式。
    ///        {  x + y = 3
    ///        {_ x - y = -1 等
    /// ・関数の定義
    ///        test(a, b) = a^2 + b 等
    /// ・単なる数式
    ///        上記のいずれのパターンにもあたらない式。
    ///        5 + a 等
    /// </remarks>
    public class FormulaCellContent
    {
        static List<FormulaCellContent> s_protTypes = new List<FormulaCellContent>();

        static FormulaCellContent()
        {
            s_protTypes.Add(new FormulaCellContentDefineConstant(null, null, null, null, null));
            s_protTypes.Add(new FormulaCellContentDefineFunction(null, null, null, null));
            s_protTypes.Add(new FormulaCellContentDefineWithEquation(null, null, null, null));
            s_protTypes.Add(new FormulaCellContentDefineWithSimultaneousEquation(null));
            s_protTypes.Add(new FormulaCellContent(null, null, null, null));
            s_protTypes.Add(new FormulaCellContentComment());
            s_protTypes.Add(new FormulaCellContentContinuation(null));
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
            // コメント等、評価の必要のない内容の初期化を試みる
            var noCalculateCell = s_protTypes.Select(p => p.TryCreateNoCalculateTarget(formulaText, solver, previous))
                                             .FirstOrDefault(p => p != null);
            if (noCalculateCell != null) return noCalculateCell;

            // 継続行の処理
            foreach (var content in previous.Select(cell => cell.Content).Reverse())
            {
                if (!(content is FormulaCellContentContinuation)) break;
                formulaText = content.FormulaText + formulaText;
            }

            // 変数・関数定義参照
            var proc = solver.GetProcessOf<EvaluateUserDefineProcess>();
            proc.CustomDefineConstants.Clear();
            proc.CustomDefineFunctions.Clear();
            foreach (var cell in previous)
            {
                foreach (var userFunc in cell.Content.GetAllDefinedFunctionNames())
                {
                    FunctionDefine def = new FunctionDefine();
                    UserFunction func = new UserFunction(userFunc.Item1);
                    func.UseVariable = userFunc.Item2;
                    def.Target = func;
                    proc.CustomDefineFunctions.Add(def);
                }
            }

            // セルの初期化
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
        /// <param name="notReferenceVariableNames">この数式セルで参照しない変数名。</param>
        /// <param name="notReferenceFunctionNames">この数式セルで参照しない関数名。</param>
        protected internal FormulaCellContent(
            string formulaText, Formula f, Formula evaluateTarget, FormulaCell[] previous,
            IEnumerable<string> notReferenceVariableNames = null, IEnumerable<string> notReferenceFunctionNames = null
        )
        {
            FormulaText = formulaText;
            TargetFormula = f;
            EvaluateTargetFormula = evaluateTarget;

            if (notReferenceVariableNames != null) _notReferenceVariableNames.AddRange(notReferenceVariableNames);
            if (notReferenceFunctionNames != null) _notReferenceFunctionNames.AddRange(notReferenceFunctionNames);

            PreDemandEvaluateFormulaCells = CreatePreDemandEvaluateFormulaCellsList(previous);
            if (previous != null)
            {
                foreach (var cell in previous.Reverse())
                {
                    if (!cell.Content.IsContinuation) break;
                    if (!PreDemandEvaluateFormulaCells.Contains(cell)) PreDemandEvaluateFormulaCells.Add(cell);
                }
            }
        }

        /// <summary>
        /// 付加情報の種別を表します。
        /// </summary>
        public enum AdditionalInformationType
        {
            /// <summary>構文解析のエラー。</summary>
            ParseError
        }

        /// <summary>
        /// 初期化元となった数式文字列を取得します。
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
        /// 評価結果を表す文字列を設定もしくは取得します。
        /// </summary>
        public string ResultText { get; set; }

        /// <summary>
        /// 数式評価で行なわれた数式変形の履歴を取得します。
        /// </summary>
        public Liffom.Deforms.DeformHistory DeformHistory { get; private set; }

        /// <summary>
        /// 評価済み結果の概要レベルを取得します。
        /// </summary>
        public Result.Level? ResultLevel { get; private set; }

        /// <summary>
        /// 何らかの付加情報を表す文字列を設定もしくは取得します。
        /// </summary>
        public Tuple<AdditionalInformationType, string> AdditionalInformation { get; set; }

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
        /// この数式セルが実際の評価を行わず、後続の行に評価を移譲するか否かを取得します。
        /// </summary>
        public virtual bool IsContinuation { get { return false; } }


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
        /// 指定文字列から、評価にあたって計算処理の不要な数式セルの内容の初期化を試みます。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        /// <param name="solver">数式の構文解析に用いるソルバ。</param>
        /// <param name="previous">前方に宣言されているか変数の数式セル。</param>
        /// <returns>初期化された数式セル内容オブジェクト。生成対象とならなかった場合には、null。</returns>
        protected virtual FormulaCellContent TryCreateNoCalculateTarget(string formulaText, Solver solver, params FormulaCell[] previous)
        { return null; }


        /// <summary>
        /// 指定数式を評価するのにあたって、先に評価されるべき数式セルのリストを取得します。
        /// </summary>
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        /// <returns>先に評価されるべき数式セルリスト。</returns>
        private List<FormulaCell> CreatePreDemandEvaluateFormulaCellsList(params FormulaCell[] previous)
        {
            var result = new List<FormulaCell>();

            foreach (var mark in GetAllReferenceVariableNames())
            {
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
            foreach (var name in GetAllReferenceFunctionNames())
            {
                bool picked = false;

                for (int i = previous.Length - 1; i >= 0; i--)
                {
                    foreach (var define in previous[i].Content.GetAllDefinedFunctionNames())
                    {
                        if (define.Item1 != name) continue;

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
        /// この数式セルで定義される関数名とその引数変数リストをすべて返す反復子を取得します。
        /// </summary>
        public virtual IEnumerable<Tuple<string, List<Variable>>> GetAllDefinedFunctionNames() { yield break; }


        /// <summary>
        /// この数式セルで参照する定数名をすべて返す反復子を取得します。
        /// </summary>
        public IEnumerable<string> GetAllReferenceVariableNames()
        {
            if (EvaluateTargetFormula == null) yield break;
            foreach (var varialble in EvaluateTargetFormula.GetExistFactors<Variable>())
            {
                if (_notReferenceVariableNames.Contains(varialble.Mark)) continue;
                yield return varialble.Mark;
            }
        }
        List<string> _notReferenceVariableNames = new List<string>();

        /// <summary>
        /// この数式セルで参照するユーザー定義関数名をすべて返す反復子を取得します。
        /// </summary>
        public IEnumerable<string> GetAllReferenceFunctionNames()
        {
            if (EvaluateTargetFormula == null) yield break;
            foreach (var func in EvaluateTargetFormula.GetExistFactors<UserFunction>())
            {
                if (_notReferenceFunctionNames.Contains(func.Name)) continue;
                yield return func.Name;
            }
        }
        List<string> _notReferenceFunctionNames = new List<string>();


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
            Result result = null;
            try
            {
                result = OnEvaluate(solver);

                ResultText = result.ResultText;
                ResultLevel = result.ResultLevel;
                if (result.ResultLevel != Result.Level.Success) ResultText = "!!! " + ResultText.Replace("\n", " ").Replace("\r", "");
                else if (ContainBaseFormulaInResult) ResultText = FormulaText + " = " + ResultText;
            }
            catch (Exception e)
            {
                ResultText = "!!! " + e.Message.Replace("\n", " ").Replace("\r", "");
            }
            finally
            {
                DeformHistory = solver.GetProcessOf<CalculateFormulaProcess>().LastCalculateHistory;
                if (DeformHistory != null) BuildDeformHistoryOfConvertUnit(DeformHistory, solver, result);
            }
        }

        /// <summary>
        /// 指定の数式変形履歴に、単位変換による数式変形履歴を追加します。
        /// </summary>
        /// <param name="history">履歴の追加先。</param>
        /// <param name="solver">評価に用いたソルバ。</param>
        /// <param name="result">計算結果。</param>
        private void BuildDeformHistoryOfConvertUnit(Liffom.Deforms.DeformHistory history, Solver solver, Result result)
        {
            if (result != null && solver.GetProcessOf<ConvertToSpecifiedUnitProcess>().LastConvertHistories != null)
            {
                var histories = solver.GetProcessOf<ConvertToSpecifiedUnitProcess>().LastConvertHistories;
                bool existChild = true;
                foreach (var h in histories)
                {
                    if (h.First().Formula == history.Last().Formula)
                    {
                        existChild = false;
                        foreach (var node in h.Skip(1)) history.Add(node);
                    }
                    else
                    {
                        history.Last().AddChildHistory(h);
                    }
                }

                if (existChild)
                {
                    var dummyRule = new Liffom.Deforms.DeformChildrenRule(null, history.Last());
                    history.Add(new Liffom.Deforms.DeformHistoryNode(result.ResultFormula, dummyRule));
                }
            }
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
