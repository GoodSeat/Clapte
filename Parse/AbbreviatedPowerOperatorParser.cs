using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formats.Powers;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 省略された乗算記号の構文解析器を表します。
	/// </summary>
	public class AbbreviatedPowerOperatorParser : SeriesOperatorParser
	{
		Regex TargetVariableRegex { get; set; }

		/// <summary>
		/// 省略された乗算の演算構文解析器を初期化します。
		/// </summary>
		/// <param name="owner">所属する構文解析器。</param>
		public AbbreviatedPowerOperatorParser(FormulaParser owner) : this(owner, true) { }

		/// <summary>
		/// 省略された乗算の演算構文解析器を初期化します。
		/// </summary>
		/// <param name="owner">所属する構文解析器。</param>
		/// <param name="progressiveParse">前進解析か否か。</param>
		public AbbreviatedPowerOperatorParser(FormulaParser owner, bool progressiveParse) 
		{
			Owner = owner;
			TargetVariableRegex = new Regex(@"(?<newmark>\S*\D)(?<exponent>\d+)$");
			ProgressiveParse = progressiveParse;
		}

		/// <summary>
		/// 所属する構文解析器を設定もしくは取得します。
		/// </summary>
		FormulaParser Owner { get; set; }

		/// <summary>
		/// 連続する乗算を、前方から解決するか否かを設定もしくは取得します。
		/// </summary>
		/// <remarks>
		/// <para>
		/// 例えば2^2^3のような数式に対し、
		/// <list type="bullet">
		/// <item><see cref="ProgressiveParse"/>==trueの場合 → ((2^2)^3) </item>
		/// <item><see cref="ProgressiveParse"/>==falseの場合 → (2^(2^3))</item>
		/// といった違いがあります。
		/// </list>
		/// </remarks>
		/// </para>
		public bool ProgressiveParse { get; set; }

		public override IEnumerable<Lexer> GetAllBelongOperatorLexers()
		{
			yield break;
		}

		public override Formula GetParsedFormula(Formula initial, params KeyValuePair<OperatorToken, Formula>[] subsequents)
		{
			Formula result;

			if (ProgressiveParse)
			{
				result = initial;
				foreach (var pair in subsequents)
				{
					result = result ^ pair.Value;
				}
			}
			else
			{
				result = subsequents[subsequents.Length - 1].Value;
				for (int i = subsequents.Length - 2; i >= 0; i--)
				{
					result = subsequents[i].Value ^ result;
				}
				result = initial ^ result;
			}

			result.Format.SetProperty(new DivisionFormatProperty(false)); // 1/x 形式とはしない
			return result;
		}

		public override bool ModifyTokenRelation(Token targetToken)
		{
			var formulaToken = targetToken as FormulaToken;
			if (formulaToken == null) return false;

			var variable = formulaToken.ParsedFormula as Variable;
			if (variable == null) return false;

			var mark = variable.Mark;
			var match = TargetVariableRegex.Match(mark);
			if (!match.Success) return false;

			var newMark = match.Groups["newmark"].Value;
			var expText = match.Groups["exponent"].Value;
			var exponent = new Numeric(expText);

			var token1 = new FormulaToken(newMark, Owner.Parse(newMark));
			var token2 = new FormulaToken(expText, exponent);

			targetToken.InsertPrevious(token1);
			targetToken.InsertPrevious(new AbbreviatedPowerOperatorToken(this));
			targetToken.InsertPrevious(token2);
			targetToken.Remove();
			return true;
		}
	}
}
