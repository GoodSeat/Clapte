using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Units
{
	/// <summary>
	/// 単位変換データを表します。
	/// </summary>
	[Serializable()]
	public class UnitConvertRecord
	{
		Formula _convertUnit;
		Formula _conversionRatio = 1;
		Formula _conversionAddition = 0;

		string _unitComment = "";

		UnitConvertTable _belongTable;

		/// <summary>
		/// 単位変換データを初期化します。
		/// </summary>
		public UnitConvertRecord() { }

		/// <summary>
		/// 単位変換データを初期化します。
		/// </summary>
		/// <param name="unit">変換先単位数式</param>
		/// <param name="convert">基準単位からの変換倍率</param>
		public UnitConvertRecord(Formula unit, Formula convert)
			: this(unit, convert, "") { }

		/// <summary>
		/// 単位変換データを初期化します。
		/// </summary>
		/// <param name="unit">変換先単位数式</param>
		/// <param name="convert">基準単位からの変換倍率</param>
		/// <param name="comment">単位変換に対するコメント</param>
		public UnitConvertRecord(Formula unit, Formula convert, string comment)
		{
			ConvertUnit = unit;
			UnitComment = comment;
			ConversionRatio = convert;
		}

		/// <summary>
		/// 変換対象の単位を設定もしくは取得します。
		/// </summary>
		public virtual Formula ConvertUnit
		{
			get { return _convertUnit; }
			set
			{
				if (!value.IsUnit(true)) throw new FormulaAssertionException("変換対象の単位数式に、単位として認識できない数式が指定されました。");
				_convertUnit = value.Simplify();
			}
		}

		/// <summary>
		/// 単位のコメントを設定もしくは取得します。
		/// </summary>
		public string UnitComment
		{
			get { return _unitComment; }
			set { _unitComment = value; }
		}

		/// <summary>
		/// 所属テーブルの基準単位からの変換倍率を表す数式を設定もしくは取得します。
		/// </summary>
		public virtual Formula ConversionRatio
		{
			get { return _conversionRatio.Copy(); }
			set { _conversionRatio = value; }
		}

		/// <summary>
		/// 単独単位変換時、基準単位に対して加算する数値を表す数式を設定もしくは取得します。
		/// </summary>
		public virtual Formula ConversionAddition
		{
			get { return _conversionAddition.Copy(); }
			set { _conversionAddition = value; }
		}

		/// <summary>
		/// 所属テーブルを取得します。
		/// </summary>
		public UnitConvertTable BelongTable
		{
			get { return _belongTable; }
			internal set { _belongTable = value; }
		}

		public override string ToString()
		{
			return "UnitConvertRecord:" + ConvertUnit.ToString() + "|" + UnitComment;
		}
	}


}
