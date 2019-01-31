// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Clapte.Views
{
    /// <summary>
    /// ClapteのViewに関連する例外を表します。
    /// </summary>
    public class ClapteViewException : Exception
    {
        /// <summary>
        /// ClapteのViewに関連する例外を初期化します。
        /// </summary>
        public ClapteSolverException() : base() { }

        /// <summary>
        /// ClapteのViewに関連する例外を初期化します。
        /// </summary>
        /// <param name="message">エラー情報を表すメッセージ。</param>
        public ClapteSolverException(string message) : base(message) { }

        /// <summary>
        /// ClapteのViewに関連する例外を初期化します。
        /// </summary>
        /// <param name="message">エラー情報を表すメッセージ。</param>
        /// <param name="innerException">内部例外。</param>
        public ClapteSolverException(string message, Exception innerException) : base(message, innerException) { }

    }
}
