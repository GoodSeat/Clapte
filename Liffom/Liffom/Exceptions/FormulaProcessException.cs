// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom
{
    /// <summary>
    /// Formulaの数式処理中に発生した例外情報を表します。
    /// </summary>
    [Serializable()]
    public class FormulaProcessException : Exception
    {
        /// <summary>
        /// Formulaの数式処理中に発生した例外情報を初期化します。
        /// </summary>
        public FormulaProcessException() : base() { }

        /// <summary>
        /// Formulaの数式処理中に発生した例外情報を初期化します。
        /// </summary>
        /// <param name="message">例外に関する情報。</param>
        public FormulaProcessException(string message) : base(message) { }

        /// <summary>
        /// Formulaの数式処理中に発生した例外情報を初期化します。
        /// </summary>
        /// <param name="message">例外に関する情報。</param>
        /// <param name="innerException">現在の例外の原因となる例外。</param>
        public FormulaProcessException(string message, Exception innerException) : base(message, innerException) { }
    }
}
