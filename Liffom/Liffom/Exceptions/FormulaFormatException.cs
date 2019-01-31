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
    /// Formula内の表記規定に抵触する例外を表します。
    /// </summary>
    [Serializable()]
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Formula内の表記規定に抵触する例外情報を初期化します。
        /// </summary>
        public FormulaFormatException() : base() { }

        /// <summary>
        /// Formula内の表記規定に抵触する例外情報を初期化します。
        /// </summary>
        /// <param name="message">例外に関する情報。</param>
        public FormulaFormatException(string message) : base(message) { }

        /// <summary>
        /// Formula内の表記規定に抵触する例外情報を初期化します。
        /// </summary>
        /// <param name="message">例外に関する情報。</param>
        /// <param name="innerException">現在の例外の原因となる例外。</param>
        public FormulaFormatException(string message, Exception innerException) : base(message, innerException) { }
    }

}
