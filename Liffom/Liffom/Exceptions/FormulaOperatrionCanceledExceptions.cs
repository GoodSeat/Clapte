using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Liffom
{
    /// <summary>
    /// 数式処理がユーザーによりキャンセルされたことを表す例外です。
    /// </summary>
    public class FormulaOperationCanceledExceptions : OperationCanceledException
    {
        /// <summary>
        /// 数式処理がユーザーによりキャンセルされたことを表す例外情報を初期化します。
        /// </summary>
        public FormulaOperationCanceledExceptions() : base() { }

        /// <summary>
        /// 数式処理がユーザーによりキャンセルされたことを表す例外情報を初期化します。
        /// </summary>
        /// <param name="message">例外に関する情報。</param>
        public FormulaOperationCanceledExceptions(string message) : base(message) { }

        /// <summary>
        /// 数式処理がユーザーによりキャンセルされたことを表す例外情報を初期化します。
        /// </summary>
        /// <param name="message">例外に関する情報。</param>
        /// <param name="innerException">現在の例外の原因となる例外。</param>
        public FormulaOperationCanceledExceptions(string message, Exception innerException) : base(message, innerException) { }
    }
}
