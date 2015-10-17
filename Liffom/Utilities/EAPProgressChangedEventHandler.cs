using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace GoodSeat.Liffom.Utilities
{
    /// <summary>
    /// イベントベースの非同期処理状況の変化を通知するメソッドを表します。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void EAPProgressChangedEventHandler<T>(object sender, EAPProgressChangedEventArgs<T> e);

    /// <summary>
    /// イベントベースの非同期処理状況の変化に関するイベントデータを表します。
    /// </summary>
    public class EAPProgressChangedEventArgs<T> : ProgressChangedEventArgs
    {
        /// <summary>
        /// 非同期処理状況の変化に関するイベントデータを初期化します。
        /// </summary>
        /// <param name="progressPercentage">非同期タスクが完了した割合。(1～100）</param>
        /// <param name="userState">一意のユーザー状態。</param>
        /// <param name="current">変形途上の結果。</param>
        public EAPProgressChangedEventArgs(int progressPercentage, object userState, T current)
            : base(progressPercentage, userState)
        {
            Current = current;
        }

        /// <summary>
        /// 処理中の結果。
        /// </summary>
        public T Current { get; private set; }
    }
}

