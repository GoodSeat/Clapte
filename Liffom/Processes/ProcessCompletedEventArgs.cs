using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Liffom.Formulas;

namespace Liffom.Processes
{
	/// <summary>
	/// 数式処理の完了を通知するメソッドを表します。
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void ProcessCompletedEventHandler(object sender, ProcessCompletedEventArgs e);

	/// <summary>
	/// 数式処理終了の結果を保持するイベントデータを表します。
	/// </summary>
	public class ProcessCompletedEventArgs : AsyncCompletedEventArgs
	{
		/// <summary>
		/// 数式処理終了の結果を保持するイベントデータを初期化します。
		/// </summary>
		/// <param name="error">捕捉された例外</param>
		/// <param name="cancelled">処理が途中で打ち切られたか</param>
		/// <param name="userState">一意のユーザー状態</param>
		/// <param name="result">処理の結果得られた数式</param>
		public ProcessCompletedEventArgs(Exception error, bool cancelled, object userState, Formula result)
			: base(error, cancelled, userState)
		{
			Result = result;
		}

		/// <summary>
		/// 処理で得られた数式。
		/// </summary>
		public Formula Result { get; private set; }
	}
}
