using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Clapte.Solvers
{
	/// <summary>
	/// Clapteの処理結果を表します。
	/// </summary>
	public class Result
	{
		/// <summary>
		/// 結果の概要を表します。
		/// </summary>
		public enum Level
		{
			/// <summary>
			/// 計算に成功しました。
			/// </summary>
			Success,
			/// <summary>
			/// 計算は中断されました。
			/// </summary>
			Abort,
			/// <summary>
			/// 計算はエラー終了しました。
			/// </summary>
			Error
		}


		/// <summary>
		/// 結果を初期化します。
		/// </summary>
		/// <param name="level">結果の概要レベル。</param>
		/// <param name="resultText">処理の結果。</param>
		/// <param name="formula">結果を表す数式。</param>
		/// <param name="errors">検知された可変数のエラー情報。</param>
		public Result(Level level, string resultText, Formula formula, params Error[] errors)
		{
			ResultText = resultText;
			ResultLevel = level;
			ResultFormula = formula;

			Errors = new List<Error>(errors);
		}

		/// <summary>
		/// 結果を表す文字列を取得します。
		/// </summary>
		public string ResultText { get; private set; }

		/// <summary>
		/// 結果の概要を取得します。
		/// </summary>
		public Level ResultLevel { get; private set; }

		/// <summary>
		/// エラー情報リストを取得します。
		/// </summary>
		public List<Error> Errors { get; private set; }

		/// <summary>
		/// 結果を表す数式を取得します。計算に失敗した場合、nullが返されます。
		/// </summary>
		public Formula ResultFormula { get; private set; }
	}
}
