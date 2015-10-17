using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        protected internal FormulaCellContentComment() : base(null, null, null) { }

        /// <summary>
        /// 数式を保持しない数式セルの内容を初期化します。
        /// </summary>
        protected override FormulaCellContent CreateFrom(string formulaText, Solvers.Solver solver, params FormulaCell[] previous)
        {
            var content = new FormulaCellContentComment();

            content.ResultText = formulaText;
            return content;
        }
    }
}
