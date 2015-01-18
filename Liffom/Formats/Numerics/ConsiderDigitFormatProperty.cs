using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Formats.Numerics
{
	/// <summary>
	/// 数値の有効桁数考慮の表記情報を表します。
	/// </summary>
	[Serializable()]
	public class ConsiderDigitFormatProperty : FormatProperty
	{
		/// <summary>
		/// 数値の有効桁数考慮表記情報を初期化します。
		/// </summary>
		public ConsiderDigitFormatProperty() : this(false) { }

		/// <summary>
		/// 数値の有効桁数考慮表記情報を初期化します。
		/// </summary>
		public ConsiderDigitFormatProperty(bool consider)
		{
			ConsiderDigit = consider;
		}

		/// <summary>
		/// 有効桁数を考慮した表記とするか否かを設定もしくは取得します。
		/// </summary>
		public bool ConsiderDigit { get; set; }

		/// <summary>
		/// bool型からの暗黙的変換を行います。
		/// </summary>
		/// <param name="property">変換対象のプロパティ。</param>
		/// <returns>有効桁数考慮の表記とするか否か。</returns>
		public static implicit operator ConsiderDigitFormatProperty(bool property)
		{
			return new ConsiderDigitFormatProperty(property);
		}

		/// <summary>
		/// bool型への暗黙的変換を行います。
		/// </summary>
		/// <param name="property">変換対象のプロパティ。</param>
		/// <returns>有効桁数考慮の表記とするか否か。</returns>
		public static implicit operator bool(ConsiderDigitFormatProperty property)
		{
			return property.ConsiderDigit;
		}
	}
}





