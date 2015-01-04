using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liffom.Parse
{
	/// <summary>
	/// 構文解析に用いるトークンを表します。
	/// </summary>
	public class Token
	{
		Token _previousToken;
		Token _nextToken;

		/// <summary>
		/// 構文解析に用いるトークンを初期化します。
		/// </summary>
		/// <param name="targetText">トークン化の元となった文字列</param>
		public Token(string targetText)
		{
			TargetText = targetText;
		}

		/// <summary>
		/// トークン化のもととなった文字列を取得します。
		/// </summary>
		public string TargetText { get; private set; }


		/// <summary>
		/// このトークンから指定した終了トークンまでの元文字列を取得します。
		/// </summary>
		/// <param name="end">終了トークン</param>
		/// <returns>元文字列</returns>
		public string GetBaseText(Token end)
		{
			string result = "";
			Token target = this;
			while (target != end)
			{
				result += target.TargetText;
				target = target.NextToken;
			}
			result += target.TargetText;

			return result;
		}

		/// <summary>
		/// トークン間の関係を調整します。
		/// </summary>
		/// <returns>関係の調整操作があったか否か</returns>
		public virtual bool ModifyTokenRelation() { return false; }


		/// <summary>
		/// トークン列の先頭のトークンを取得します。
		/// </summary>
		public Token TopToken
		{
			get
			{
				if (PreviousToken == null) return this;
				else return PreviousToken.TopToken;
			}
		}

		/// <summary>
		/// トークン列の中における、このトークンの一つ前のトークンを取得します。
		/// </summary>
		public Token PreviousToken { get { return _previousToken; } }

		/// <summary>
		/// このトークンの一つ前に、指定トークンを挿入します。
		/// </summary>
		/// <param name="token">挿入トークン</param>
		public void InsertPrevious(Token token)
		{
			token._previousToken= _previousToken;
			if (_previousToken != null) _previousToken._nextToken = token;

			token._nextToken = this;
			this._previousToken = token;
		}


		/// <summary>
		/// トークン列の末尾のトークンを取得します。
		/// </summary>
		public Token LastToken
		{
			get
			{
				if (NextToken == null) return this;
				else return NextToken.LastToken;
			}
		}

		/// <summary>
		/// トークン列の中における、このトークンの一つ後のトークンを取得します。
		/// </summary>
		public Token NextToken { get { return _nextToken; } }

		/// <summary>
		/// このトークンの一つ後に、指定トークンを挿入します。
		/// </summary>
		/// <param name="token">挿入トークン</param>
		public void InsertNext(Token token)
		{
			token._nextToken = _nextToken;
			if (_nextToken != null) _nextToken._previousToken = token;

			token._previousToken = this;
			this._nextToken = token;
		}

		/// <summary>
		/// このトークンをトークン列から除外します。
		/// </summary>
		public void Remove()
		{
			if (PreviousToken != null)
				PreviousToken._nextToken = NextToken;
			if (NextToken != null)
				NextToken._previousToken = PreviousToken;
		}

		/// <summary>
		/// トークン列中の位置を表す0から始まるインデックスを取得します。
		/// </summary>
		public int Index
		{
			get
			{
				if (PreviousToken == null) return 0;
				return PreviousToken.Index + 1;
			}
		}

		/// <summary>
		/// トークン列中の、このトークンより後にあるトークンの数を取得します。
		/// </summary>
		public int PostTokenCount
		{
			get
			{
				if (NextToken == null) return 0;
				return NextToken.PostTokenCount + 1;
			}
		}

		/// <summary>
		/// トークン列中のトークン数を取得します。
		/// </summary>
		public int TokenCount
		{
			get { return Index + PostTokenCount + 1; }
		}

		public override string ToString()
		{
			return string.Format("[{0}]", TargetText);
		}
	}
}
