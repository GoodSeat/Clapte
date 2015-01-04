using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liffom.Parse
{
	/// <summary>
	/// 空白文字の字句解析器を表します。
	/// </summary>
	public class SpaceLexer : Lexer
	{
		/// <summary>
		/// 指定文字が空白トークンとしてトークン化可能か否かを判断します。
		/// </summary>
		/// <param name="text">判定対象の文字列</param>
		/// <returns>判定結果</returns>
		public override Lexer.ScanResult Scan(string text)
		{
			if (text == " ") return ScanResult.Match;
			else return ScanResult.NeverMatch;
		}

		/// <summary>
		/// 空白文字を指定して、空白トークンを初期化して取得します。
		/// </summary>
		/// <param name="text">トークン化対象文字列</param>
		/// <returns>空白トークン</returns>
		public override IEnumerable<Token> Tokenize(string text)
		{
			yield return new SpaceToken(text);
		}
	}
}
