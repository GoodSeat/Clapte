// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace GoodSeat.Liffom.Utilities
{
    /// <summary>
    /// イベントベースの非同期処理の完了を通知するメソッドを表します。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void EAPCompletedEventHandler<T>(object sender, EAPCompletedEventArgs<T> e);

    /// <summary>
    /// イベントベースの非同期処理終了の結果を保持するイベントデータを表します。
    /// </summary>
    public class EAPCompletedEventArgs<T> : AsyncCompletedEventArgs
    {
        /// <summary>
        /// イベントベースの非同期処理終了の結果を保持するイベントデータを初期化します。
        /// </summary>
        /// <param name="error">捕捉された例外</param>
        /// <param name="cancelled">処理が途中で打ち切られたか</param>
        /// <param name="userState">一意のユーザー状態</param>
        /// <param name="result">処理の結果得られた結果。</param>
        public EAPCompletedEventArgs(Exception error, bool cancelled, object userState, T result)
            : base(error, cancelled, userState)
        {
            Result = result;
        }

        /// <summary>
        /// 処理で得られた結果。
        /// </summary>
        public T Result { get; private set; }
    }
}


