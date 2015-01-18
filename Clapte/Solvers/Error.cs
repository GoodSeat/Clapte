using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Clapte.Solvers
{
	/// <summary>
	/// 処理中に発生したエラーを表します。
	/// </summary>
	public class Error
	{
		/// <summary>
		/// 計算チェックのエラーレベルを表します。
		/// </summary>
		public enum Level
		{
			/// <summary>
			/// 問題ありません。
			/// </summary>
			None,
			/// <summary>
			/// 処理の続行は可能ですが、計算を中止します。
			/// </summary>
			Abort,
			/// <summary>
			/// 計算に失敗した、もしくは処理の続行が不可能です。
			/// </summary>
			Error,
			/// <summary>
			/// 計算の失敗原因を通知する必要のあるエラーです。
			/// </summary>
			Message,
		}


		/// <summary>
		/// エラー情報を初期化します。
		/// </summary>
		/// <param name="level">エラーレベル。</param>
		/// <param name="msg">エラーの説明文。</param>
		public Error(Level level, string msg)
		{
			ErrorLevel = level;
			Message = msg;
		}

		/// <summary>
		/// エラーレベルを取得します。
		/// </summary>
		public Level ErrorLevel { get; private set; }

		/// <summary>
		/// エラーの説明文を取得します。
		/// </summary>
		public string Message { get; private set; }

	}
}
