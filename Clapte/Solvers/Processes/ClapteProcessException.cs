using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Clapte.Solvers.Processes
{
	/// <summary>
	/// Clapteのソルバにおける処理において発生する例外を表します。
	/// </summary>
	public class ClapteProcessException : Exception
	{
		/// <summary>
		/// Clapteのソルバにおける処理において発生する例外を初期化します。
		/// </summary>
		public ClapteProcessException() : base() { }

		/// <summary>
		/// Clapteのソルバにおける処理において発生する例外を初期化します。
		/// </summary>
		/// <param name="message">エラー情報。</param>
		public ClapteProcessException(string message) : base(message) { }

		/// <summary>
		/// Clapteのソルバにおける処理において発生する例外を初期化します。
		/// </summary>
		/// <param name="message">エラー情報。</param>
		/// <param name="innerException">内部の例外。</param>
		public ClapteProcessException(string message, Exception innerException) : base(message, innerException) { }
	}
}
