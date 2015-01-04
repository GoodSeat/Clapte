using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Formulas.Operators;

namespace Liffom.Formulas.Units
{
	/// <summary>
	/// 単位変換テーブルにおける基準単位データを表します。
	/// </summary>
	public class BaseUnitConvertRecord : UnitConvertRecord
	{		
		/// <summary>
		/// 単位変換テーブルにおける基準単位データを初期化します。
		/// </summary>
		public BaseUnitConvertRecord() : this(new Unit("dummy")) { }

		/// <summary>
		/// 単位変換テーブルにおける基準単位データを初期化します。
		/// </summary>
		/// <param name="u">規準とする単位扱い数式。</param>
		public BaseUnitConvertRecord(Formula u) : this(u, "") { }

		/// <summary>
		/// 単位変換テーブルにおける基準単位データを初期化します。
		/// </summary>
		/// <param name="u">規準とする単位扱い数式。</param>
		/// <param name="comment">単位に対するコメント。</param>
		public BaseUnitConvertRecord(Formula u, string comment)
		{
			ConvertUnit = u;
			UnitComment = comment;
		}

		public override Formula ConversionRatio
		{
			get { return 1; }
			set { if (value != 1) throw new FormulaAssertionException("基準単位に対して変換倍率を設定することはできません。"); }
		}

		public override Formula ConversionAddition
		{
			get { return 0; }
			set { if (value != 0) throw new FormulaAssertionException("基準単位に対して加算数式を設定することはできません。"); }
		}
	}

}
