using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom
{
    /// <summary>
    /// Formula内で想定と異なる状況が発生した時の例外を表します。
    /// </summary>
    [Serializable()]
    public class FormulaAssertionException : Exception
    {
        /// <summary>
        /// 表明のチェックを行います。
        /// </summary>
        /// <param name="check">表明内容。</param>
        public static void Assert(bool check)
        {
#if DEBUG
            if (!check) throw new FormulaAssertionException();
#endif
        }

        /// <summary>
        /// 表明のチェックを行います。
        /// </summary>
        /// <param name="check">表明内容。</param>
        /// <param name="errorInfo">例外時の説明。</param>
        public static void Assert(bool check, string errorInfo)
        {
#if DEBUG
            if (!check) throw new FormulaAssertionException(errorInfo);
#endif
        }

        /// <summary>
        /// Formula内で想定と異なる状況が発生した例外情報を初期化します。
        /// </summary>
        public FormulaAssertionException() : base() { }

        /// <summary>
        /// Formula内で想定と異なる状況が発生した例外情報を初期化します。
        /// </summary>
        /// <param name="message">例外に関する情報。</param>
        public FormulaAssertionException(string message) : base(message) { }

        /// <summary>
        /// Formula内で想定と異なる状況が発生した例外情報を初期化します。
        /// </summary>
        /// <param name="message">例外に関する情報。</param>
        /// <param name="innerException">現在の例外の原因となる例外。</param>
        public FormulaAssertionException(string message, Exception innerException) : base(message, innerException) { }
    
    }
}
