using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Formats.Numerics
{
    /// <summary>
    /// 数値の区切り表記情報を表します。
    /// </summary>
    [Serializable()]
    public class SplitFormatProperty : FormatProperty
    {
        /// <summary>
        /// 区切り文字のタイプを表します。
        /// </summary>
        public enum SplitType
        {
            /// <summary>
            /// 数値の区切りを行いません。
            /// </summary>
            None,
            /// <summary>
            /// 空白文字で、3桁ごとに区切ります。ただし、4桁の数字を区切るためには挿入されません。
            /// </summary>
            Space,
            /// <summary>
            /// カンマで、3桁ごとに区切ります。
            /// </summary>
            /// <remarks>
            /// 国際度量衡総会決議(CGPM)では、カンマ、ピリオドは小数点にのみ用いるものとし、3桁ごとの区切り文字としての使用は禁じています。
            /// </remarks>
            Comma,
            /// <summary>
            /// ピリオドで、3桁ごとに区切ります。
            /// </summary>
            /// <remarks>
            /// 国際度量衡総会決議(CGPM)では、カンマ、ピリオドは小数点にのみ用いるものとし、3桁ごとの区切り文字としての使用は禁じています。
            /// </remarks>
            Period,
        }


        /// <summary>
        /// 数値の区切り表記情報を初期化します。
        /// </summary>
        public SplitFormatProperty() : this(SplitType.None) { }

        /// <summary>
        /// 数値の区切り表記情報を初期化します。
        /// </summary>
        /// <param name="type">区切り文字のタイプ。</param>
        public SplitFormatProperty(SplitType type)
        {
            Type = type;
        }

        /// <summary>
        /// 区切り文字のタイプを設定もしくは取得します。
        /// </summary>
        public SplitType Type { get; set; }

        /// <summary>
        /// 区切り文字として用いるテキストを取得します。
        /// </summary>
        public string SplitText
        {
            get
            {
                switch (Type)
                {
                    case SplitType.None: return "";
                    case SplitType.Space: return " ";
                    case SplitType.Comma: return ",";
                    case SplitType.Period: return ".";
                }
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 4桁の場合にも区切りを適用するか否かを取得します。
        /// </summary>
        private bool SplitAlsoFour
        {
            get
            {
                return Type != SplitType.Space;
            }
        }

        /// <summary>
        /// 小数部についても区切りを適用するか否かを取得します。
        /// </summary>
        private bool SplitAlsoDecimal
        {
            get
            {
                return Type == SplitType.Space;
            }
        }


        /// <summary>
        /// 数値表記文字列に対し、3桁ごとに区切る文字列を挿入します。
        /// </summary>
        /// <param name="formula">数値表記文字列。</param>
        /// <param name="format">数式の書式情報。</param>
        /// <returns>区切った文字列。</returns>
        public string SetSplit(string formula, Format format)
        {
            if (Type == SplitType.None) return formula;

            // 小数点と区切り文字が一致する場合、区切り処理は行わない。
            var radixPoint = format.PropertyOf<RadixPointFormatProperty>();
            if (radixPoint.Type == RadixPointFormatProperty.RadixPointType.Comma &&
                Type == SplitType.Comma) return formula;
            if (radixPoint.Type == RadixPointFormatProperty.RadixPointType.Period &&
                Type == SplitType.Period) return formula;

            char radix = radixPoint.RadixPointChara;
            string[] splitExp = formula.Split('e', 'E');
            string[] splitPeriod = splitExp[0].Split(radix);

            int count = splitPeriod[0].Length;
            if (SplitAlsoFour || count  > 4)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 1; i <= count; i++)
                {
                    sb.Insert(0, splitPeriod[0][count - i]);
                    if (i % 3 == 0 && i != count) sb.Insert(0, SplitText);
                }
                splitPeriod[0] = sb.ToString();
            }
            
            if (SplitAlsoDecimal && splitPeriod.Length > 1) // 小数部について
            {
                count = splitPeriod[1].Length;
                if (SplitAlsoFour || count  > 4)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 1; i <= count; i++)
                    {
                        sb.Append(splitPeriod[1][i - 1]);
                        if (i % 3 == 0 && i != count) sb.Append(SplitText);
                    }
                    splitPeriod[1] = sb.ToString();
                }
            }

            string result = splitPeriod[0];
            if (splitPeriod.Length > 1) result += radix + splitPeriod[1];
            if (splitExp.Length > 1) result += (formula.Contains("E") ? "E" : "e") + splitExp[1];
            return result;
        }

        /// <summary>
        /// SplitType型からの暗黙的変換を行います。
        /// </summary>
        /// <param name="property">変換対象のプロパティ。</param>
        /// <returns>指定した属性に対応する属性情報。</returns>
        public static implicit operator SplitFormatProperty(SplitType property)
        {
            return new SplitFormatProperty(property);
        }

        /// <summary>
        /// SplitType型への暗黙的変換を行います。
        /// </summary>
        /// <param name="property">変換対象のプロパティ。</param>
        /// <returns>属性情報が示す属性。</returns>
        public static implicit operator SplitType(SplitFormatProperty property)
        {
            return property.Type;
        }
    }
}



