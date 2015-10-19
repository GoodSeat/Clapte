using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom
{
    /// <summary>
    /// Formulaの数式処理中に発生した例外情報を初期化します。
    /// </summary>
    public class FormulaRuleException : Exception
    {
        /// <summary>
        /// Formulaの数式処理中に発生した例外情報を初期化します。
        /// </summary>
        public FormulaRuleException() : base() { }

        /// <summary>
        /// Formulaの数式処理中に発生した例外情報を初期化します。
        /// </summary>
        /// <param name="message">例外に関する情報。</param>
        public FormulaRuleException(string message) : base(message) { }

        /// <summary>
        /// Formulaの数式処理中に発生した例外情報を初期化します。
        /// </summary>
        /// <param name="message">例外に関する情報。</param>
        /// <param name="innerException">現在の例外の原因となる例外。</param>
        public FormulaRuleException(string message, Exception innerException) : base(message, innerException) { }
    }
}
