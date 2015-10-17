using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Processes
{
    /// <summary>
    /// 数式処理状況の変化を通知するメソッドを表します。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ProgressChangedEventHandler(object sender, ProcessProgressChangedEventArgs e);

    /// <summary>
    /// 数式処理状況の変化に関するイベントデータを表します。
    /// </summary>
    public class ProcessProgressChangedEventArgs : ProgressChangedEventArgs
    {
        /// <summary>
        /// 数式処理状況の変化に関するイベントデータを初期化します。
        /// </summary>
        /// <param name="progressPercentage">非同期タスクが完了した割合。(1～100）</param>
        /// <param name="userState">一意のユーザー状態</param>
        /// <param name="current">変形途上の数式</param>
        public ProcessProgressChangedEventArgs(int progressPercentage, object userState, Formula current)
            : base(progressPercentage, userState)
        {
            Current = current;
        }

        /// <summary>
        /// 処理中の数式。
        /// </summary>
        public Formula Current { get; private set; }
    }
}
