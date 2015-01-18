using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formats;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Matrices;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 波括弧トークンを表します。
	/// </summary>
	public class BraceToken : PunctuationToken
	{
		/// <summary>
		/// 波括弧トークンを初期化します。
		/// </summary>
		/// <param name="targetText">トークン元文字列。</param>
		public BraceToken(string targetText) : this(targetText, true) { } 

		/// <summary>
		/// 波括弧トークンを初期化します。
		/// </summary>
		/// <param name="targetText">トークン元文字列。</param>
		/// <param name="evaluateAsMatrix">可能な場合に、括弧内部をマトリクスとして解析するか。</param>
		public BraceToken(string targetText, bool evaluateAsMatrix)
			: base(targetText)
		{
			if (targetText == "{") Type = PunctuationType.Start;
			else if (targetText == "}") Type = PunctuationType.End;
			else throw new FormulaParseException();

			EvaluateAsMatrix = evaluateAsMatrix;
		}

		public override bool IsValidSetPunctuation(PunctuationToken token)
		{
			if (!(token is BraceToken)) return false;

			if (Type == PunctuationType.Start) return token.TargetText == "}";
			else if (Type == PunctuationType.End) return token.TargetText == "{";
			else return false;
		}

		/// <summary>
		/// 波括弧内部のカンマ区切りの数式を、列ベクトルもしくはマトリクスとして解析するか否かを設定もしくは取得します。
		/// </summary>
		public bool EvaluateAsMatrix { get; set; }


		/// <summary>
		/// 区切りの内部が構文解析されたとき、その評価結果を必要に応じて加工します。
		/// </summary>
		/// <param name="parsed">区切り文字内部の解析結果</param>
		/// <returns>加工結果</returns>
		public override Formula OnInnerParsed(Formula parsed) 
		{
			if (EvaluateAsMatrix && parsed is Argument)
			{
				var arg = parsed as Argument;

				bool asMatrix = false;
				var vector = arg[0] as Vector;
				if (vector != null && vector.RowSize == 1)
				{
					asMatrix = true;
					int columnSize = vector.ColumnSize;
					foreach (var other in arg)
					{
						var vectorOther = other as Vector;
						if (vectorOther == null || vectorOther.ColumnSize != columnSize)
						{
							asMatrix = false;
							break;
						}
					}
				}

				if (asMatrix)
				{
					List<Vector> args = new List<Vector>();
					foreach (var v in arg) args.Add(v as Vector);
					return new Matrix(args.ToArray());
				}
				else
				{
					List<Formula> args = new List<Formula>(parsed);
					return new Vector(false, args.ToArray());
				}
			}

			parsed.Format.SetProperty(Bracket.CurlyBracket);
			return parsed;
		}
	}
}
