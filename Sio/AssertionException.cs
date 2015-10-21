using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Sio
{
	/// <summary>
	/// 想定と異なる状況が発生しました。
	/// </summary>
    [Serializable()]
	public class AssertionException : Exception
	{
		/// <summary>
		/// 表明のチェックを行います。
		/// </summary>
		/// <param name="check">表明内容</param>
		public static void Assert(bool check)
		{
#if DEBUG
			if (!check) throw new AssertionException();
#endif
		}

		/// <summary>
		/// 表明のチェックを行います。
		/// </summary>
		/// <param name="check">表明内容</param>
		/// <param name="errorInfo">エラー時の説明</param>
		public static void Assert(bool check, string errorInfo)
		{
#if DEBUG
			if (!check) throw new AssertionException(errorInfo);
#endif
		}

		/// <summary>
		/// 想定と異なる状況が発生したエラー情報を初期化します。
		/// </summary>
		public AssertionException() : base() { }

		/// <summary>
		/// 想定と異なる状況が発生したエラー情報を初期化します。
		/// </summary>
		/// <param name="message">エラーに関する情報</param>
		public AssertionException(string message) : base(message) { }

		/// <summary>
		/// 想定と異なる状況が発生したエラー情報を初期化します。
		/// </summary>
		/// <param name="message">エラーに関する情報</param>
		/// <param name="innerException">現在の例外の原因となる例外</param>
		public AssertionException(string message, Exception innerException) : base(message, innerException) { }
	}
}
