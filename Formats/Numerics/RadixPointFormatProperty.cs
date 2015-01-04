using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Formats.Numerics
{
	/// <summary>
	/// 数値の小数点の表記情報を表します。
	/// </summary>
	[Serializable()]
	public class RadixPointFormatProperty : FormatProperty
	{
		/// <summary>
		/// 小数点表記用文字のタイプを表します。
		/// </summary>
		public enum RadixPointType
		{
			/// <summary>
			/// 小数点をピリオドで表記します。
			/// </summary>
			Period,
			/// <summary>
			/// 小数点をカンマで表記します。
			/// </summary>
			Comma,
		}


		/// <summary>
		/// 数値の小数点表記情報を初期化します。
		/// </summary>
		public RadixPointFormatProperty() : this(RadixPointType.Period) { }

		/// <summary>
		/// 数値の小数点表記情報を初期化します。
		/// </summary>
		/// <param name="type">小数点文字のタイプ。</param>
		public RadixPointFormatProperty(RadixPointType type)
		{
			Type = type;
		}

		/// <summary>
		/// 小数点文字のタイプを設定もしくは取得します。
		/// </summary>
		public RadixPointType Type { get; set; }

		/// <summary>
		/// 小数点文字として用いるテキストを取得します。
		/// </summary>
		public char RadixPointChara
		{
			get
			{
				switch (Type)
				{
					case RadixPointType.Period: return '.';
					case RadixPointType.Comma: return ',';
				}
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// 数値表記文字列に対し、小数点の表記を修正します。
		/// </summary>
		/// <param name="formula">数値表記文字列。</param>
		/// <returns>修正後の表記文字列。</returns>
		public string SetRadixPoint(string formula)
		{
			if (Type == RadixPointType.Period) return formula;
			return formula.Replace(".", RadixPointChara.ToString());
		}

		/// <summary>
		/// RadixPointType型からの暗黙的変換を行います。
		/// </summary>
		/// <param name="property">変換対象のプロパティ。</param>
		/// <returns>指定した属性に対応する属性情報。</returns>
		public static implicit operator RadixPointFormatProperty(RadixPointType property)
		{
			return new RadixPointFormatProperty(property);
		}

		/// <summary>
		/// RadixPointType型への暗黙的変換を行います。
		/// </summary>
		/// <param name="property">変換対象のプロパティ。</param>
		/// <returns>属性情報が示す属性。</returns>
		public static implicit operator RadixPointType(RadixPointFormatProperty property)
		{
			return property.Type;
		}
	}
}




