using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Formulas
{
    /// <summary>
    /// 計算の続行が不可能となるエラーを含む数式を表します。
    /// </summary>
    public class ErrorFormula : Formula
    {
        /// <summary>
        /// 計算の続行が不可能となるエラーを含む数式を初期化します。
        /// </summary>
        /// <param name="e">エラーの原因となる例外。</param>
        public ErrorFormula(Exception e)
        {
            CauseException = e;
        }

        /// <summary>
        /// エラーの原因となる例外を取得します。
        /// </summary>
        public Exception CauseException { get; private set; }

        public override string GetText()
        {
            return "{" + CauseException.Message + "}";
        }

        protected override string OnGetUniqueText()
        {
            return GetText();
        }
    }
}
