using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Clapte.Solvers;

namespace GoodSeat.Clapte.Models
{
    /// <summary>
    /// 数式を保持しない数式セルの内容を表します。
    /// </summary>
    /// <remarks>
    /// 数式の変換に失敗した場合や、単なるコメント行などの場合に生成されます。
    /// 常に評価済みと判定され、結果を表す文字列は、初期化に使用された文字列となります（コメント文字列は含まれません）。
    /// </remarks>
    public class FormulaCellContentComment : FormulaCellContent
    {
        /// <summary>
        /// 数式を保持しない数式セルの内容を初期化します。
        /// </summary>
        protected internal FormulaCellContentComment() : base(null, null, null, null) { }

        /// <summary>
        /// 数式を保持しない数式セルの内容を初期化して取得します。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        /// <param name="solver">数式の構文解析に用いるソルバ。</param>
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        /// <returns>初期化された数式セル内容オブジェクト。</returns>
        protected override FormulaCellContent CreateFrom(string formulaText, Solvers.Solver solver, params FormulaCell[] previous)
        {
            var content = new FormulaCellContentComment();

            content.ResultText = formulaText;
            return content;
        }

        /// <summary>
        /// 指定文字列から、評価にあたって計算処理の不要な数式セルの内容の初期化を試みます。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        /// <param name="solver">数式の構文解析に用いるソルバ。</param>
        /// <param name="previous">前方に宣言されているか変数の数式セル。</param>
        /// <returns>初期化された数式セル内容オブジェクト。生成対象とならなかった場合には、null。</returns>
        protected override FormulaCellContent TryCreateNoCalculateTarget(string formulaText, Solver solver, params FormulaCell[] previous)
        {
            if (string.IsNullOrWhiteSpace(formulaText)) return CreateFrom(formulaText, solver, previous);
            return null;
        }
    }
}
