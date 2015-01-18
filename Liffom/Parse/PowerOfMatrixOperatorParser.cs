using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 行列の乗算の演算構文解析器を表します。
	/// </summary>
	public class PowerOfMatrixOperatorParser : SeriesOperatorParser
	{
		/// <summary>
		/// 乗算の演算構文解析器を初期化します。
		/// </summary>
		public PowerOfMatrixOperatorParser() { }

		/// <summary>
		/// 乗算の演算構文解析器を初期化します。
		/// </summary>
		/// <param name="progressiveParse">前進解析か否か。</param>
		public PowerOfMatrixOperatorParser(bool progressiveParse) { ProgressiveParse = progressiveParse; }

		/// <summary>
		/// 連続する乗算を、前方から解決するか否かを設定もしくは取得します。
		/// </summary>
		/// <remarks>
		/// <para>
		/// 例えば2^2^3のような数式に対し、
		/// <list type="bullet">
		/// <item><see cref="ProgressiveParse"/>==trueの場合 → ((2^2)^3) </item>
		/// <item><see cref="ProgressiveParse"/>==trueの場合 → (2^(2^3))</item>
		/// といった違いがあります。
		/// </list>
		/// </remarks>
		/// </para>
		public bool ProgressiveParse { get; set; }

		public override IEnumerable<Lexer> GetAllBelongOperatorLexers()
		{
			yield return new PowerOfMatrixOperatorLexer(this);
		}

		public override Formula GetParsedFormula(Formula initial, params KeyValuePair<OperatorToken, Formula>[] subsequents)
		{
			Formula result = initial;
			foreach (var pair in subsequents)
			{
				result = new PowerOfMatrix(result, pair.Value);
			}
			return result;
		}
	}
}
