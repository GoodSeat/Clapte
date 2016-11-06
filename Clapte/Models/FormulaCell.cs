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
    /// 数式セルを表します。
    /// </summary>
    /// <remarks>
    /// '#'以降の文字列を、コメントとして認識します。
    /// </remarks>
    public class FormulaCell
    {
        /// <summary>
        /// 数式セルを初期化します。
        /// </summary>
        /// <param name="text">元のテキスト。</param>
        /// <param name="solver">計算に用いるソルバ。</param>
        /// <param name="previous">この数式セルより前方に宣言されている可変数の数式セル。</param>
        public FormulaCell(string text, Solver solver, params FormulaCell[] previous)
        {
            CacheText = text.Trim();

            // コメントの取出し
            string[] commentSplit = CacheText.Split('#');
            if (commentSplit.Length > 0)
            {
                FormulaText = CacheText.Substring(0, commentSplit[0].Length).Trim();
                CommentText = CacheText.Substring(commentSplit[0].Length).Trim();
            }

            Content = FormulaCellContent.CreateFormulaCellContent(FormulaText, solver, previous);

            if (Content != null) FormulaText = Content.FormulaText;
        }

        /// <summary>
        /// 数式セルの内容を表すFormulaCellContentオブジェクトを設定もしくは取得します。
        /// </summary>
        public FormulaCellContent Content { get; set; }

        /// <summary>
        /// 初期化に用いた文字列を取得します。
        /// </summary>
        public string CacheText { get; private set; }

        /// <summary>
        /// 数式部分の文字列を取得します。
        /// </summary>
        public string FormulaText { get; private set; }

        /// <summary>
        /// 数式セルに含まれる、'#'を含むコメント文字列を取得します。
        /// </summary>
        public string CommentText { get; private set; }

        /// <summary>
        /// コメントとして扱われるべき数式セルか否かを取得します。
        /// </summary>
        public bool IsAllComment
        {
            get
            {
                return string.IsNullOrEmpty(CacheText) || CacheText.StartsWith("#");
            }
        }


        /// <summary>
        /// 数式の結果が評価されているか否かを取得します。
        /// </summary>
        public bool Evaluated { get { return Content.Evaluated; } }


        /// <summary>
        /// 評価を開始するのに必要な数式セルが既に評価されているかを判定します。
        /// </summary>
        public bool CanEvaluate { get { return Content.CanEvaluate; } }

        /// <summary>
        /// この数式セルの評価において評価される変数の定義を取得します。
        /// </summary>
        /// <param name="name">取得対象の変数名。</param>
        /// <returns>評価結果の定義数式。該当のない等で評価の得られない場合、null。</returns>
        public ConstantDefine GetVariableDefineOf(string name)
        {
            foreach (var def in Content.GetAllConstantDefines())
            {
                if (def == null) continue;
                if (def.Name == name) return def;
            }
            return null;
        }

        /// <summary>
        /// この数式セルの評価において評価される関数の定義を取得します。
        /// </summary>
        /// <param name="name">取得対象の関数名。</param>
        /// <returns>評価結果の定義数式。該当のない等で評価の得られない場合、null。</returns>
        public FunctionDefine GetFunctionDefineOf(string name)
        {
            foreach (var def in Content.GetAllFunctionDefines())
            {
                if (def.Name == name) return def;
            }
            return null;
        }


        /// <summary>
        /// 数式セルに設定された対象数式を評価します。
        /// </summary>
        /// <param name="solver">計算に使用するソルバ。</param>
        public void Evaluate(Solver solver)
        {
            if (IsAllComment) return;

            Content.Evaluate(solver);
        }

        /// <summary>
        /// この数式セルを、依存関係を含めて一意に識別する文字列を生成して取得します。
        /// </summary>
        public string GetUniqueText()
        {
            string result = "CELL::" + FormulaText;

            foreach (var previous in Content.PreDemandEvaluateFormulaCells)
            {
                result += "<-(" + previous.GetUniqueText() + ")";
            }
            return result;
        }

        /// <summary>
        /// 数式セルを表す文字列を返します。
        /// </summary>
        public override string ToString()
        {
            return "FormulaCell::" + CacheText;
        }
    }
}
