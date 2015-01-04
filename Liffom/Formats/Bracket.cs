using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Liffom.Formats
{
	/// <summary>
	/// 数式括弧を表します。
	/// </summary>
	[Serializable()]
	public class Bracket : FormatProperty
	{
		/// <summary>
		/// 括弧なしを表します。このフィールドは定数です。
		/// </summary>
		public static readonly Bracket NoBracket = new Bracket(Kind.None);

		/// <summary>
		/// 角括弧を表します。このフィールドは定数です。
		/// </summary>
		public static readonly Bracket SquareBracket = new Bracket(Kind.Brackets);

		/// <summary>
		/// 波括弧を表します。このフィールドは定数です。
		/// </summary>
		public static readonly Bracket CurlyBracket = new Bracket(Kind.Braces);
		
		/// <summary>
		/// 丸括弧を表します。このフィールドは定数です。
		/// </summary>
		public static readonly Bracket Parenthese = new Bracket(Kind.Parentheses);


		/// <summary>
		/// 数式括弧の種類を表します。
		/// </summary>
		public enum Kind
		{
			/// <summary>
			/// 括弧を使用しません。
			/// </summary>
			None,
			/// <summary>
			/// "()"による括弧を表します。
			/// </summary>
			Parentheses,
			/// <summary>
			/// "{}"による括弧を表します。
			/// </summary>
			Braces,
			/// <summary>
			/// "[]"による括弧を表します。
			/// </summary>
			Brackets
		}

		Kind _kind;


		/// <summary>
		/// 数式括弧を初期化します。
		/// </summary>
		public Bracket() : this(Kind.None) { }

		/// <summary>
		/// 数式括弧を初期化します。
		/// </summary>
		/// <param name="kind">種類</param>
		public Bracket(Kind kind) { _kind = kind; }

		/// <summary>
		/// 括弧タイプを設定もしくは取得します。
		/// </summary>
		public Kind Type
		{
			get { return _kind; }
			set { _kind = value; }
		}

		/// <summary>
		/// 開始括弧を取得します。
		/// </summary>
		public virtual string Start
		{
			get
			{
				string start = "";
				switch (_kind)
				{
					case Kind.Parentheses: start = "("; break;
					case Kind.Braces: start = "{"; break;
					case Kind.Brackets: start = "["; break;
				} return start;
			}
		}

		/// <summary>
		/// 終了括弧を取得します。
		/// </summary>
		public virtual string End
		{
			get
			{
				string end = "";
				switch (_kind)
				{
					case Kind.Parentheses: end = ")"; break;
					case Kind.Braces: end = "}"; break;
					case Kind.Brackets: end = "]"; break;
				} return end;
			}
		}

		/// <summary>
		/// 指定文字列を括弧で囲った文字列に変換して返します。
		/// </summary>
		/// <param name="f">囲う対象の数式文字列。</param>
		/// <returns>括弧で囲われた数式文字列。</returns>
		public string EncloseFormula(string f)
		{
			return Start + f + End;
		}

		/// <summary>
		/// 数式の出力文字列を修正します。
		/// </summary>
		/// <param name="ownerFormat">この書式情報を保持する書式オブジェクト。</param>
		/// <param name="baseText">変更前の出力文字列。</param>
		internal protected override string OnModifyOutputText(Format ownerFormat, string baseText) 
		{
			if (Type != Kind.None) return EncloseFormula(baseText);

			if (ownerFormat.Parent != null)
			{
				if (ownerFormat.Target.IsUnit() && !ownerFormat.Parent.Target.IsUnit())
				{
					return SquareBracket.EncloseFormula(baseText);
				}

				var ownerOperator = ownerFormat.Parent.Target as Operator;
				if (ownerOperator != null && ownerOperator.NeedBracket(ownerFormat.Target))
				{
					return Parenthese.EncloseFormula(baseText);
				}
			}
			return baseText;
		}

		/// <summary>
		/// 親数式に設定された書式属性が、子数式に継承されるか否かを取得します。
		/// </summary>
		public override bool Inheritable { get { return false; } }
		
	}
}
