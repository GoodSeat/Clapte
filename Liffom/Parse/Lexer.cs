using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// Liffom.Formulaの解析に用いる字句解析器を表します。
	/// </summary>
	public abstract class Lexer
	{
		/// <summary>
		/// 文字列に対するスキャン結果を表します。
		/// </summary>
		public enum ScanResult
		{
			/// <summary>
			/// トークン化可能な文字列です。
			/// </summary>
			Match,
			/// <summary>
			/// トークン化不可能な文字列ですが、後方に文字列を追加することで、トークン化可能となる可能性があります。
			/// </summary>
			NoMatch,
			/// <summary>
			/// トークン化不可能な文字列で、後方に文字列を追加しても、トークン化可能となることはありません。
			/// </summary>
			NeverMatch
		}

		/// <summary>
		/// 指定文字列をスキャンし、トークン化可能か否かを取得します。
		/// </summary>
		/// <param name="text">スキャン対象の文字列</param>
		/// <returns>スキャン結果</returns>
		public abstract ScanResult Scan(string text);

		/// <summary>
		/// 指定文字列をトークンに変換します。<paramref name="text"/>は、<see cref="Scan"/>メソッドの結果がMatchであることが保証されています。
		/// </summary>
		/// <param name="text">トークン化対象の文字列。</param>
		/// <returns>トークン</returns>
		public abstract IEnumerable<Token> Tokenize(string text);

	}
}
