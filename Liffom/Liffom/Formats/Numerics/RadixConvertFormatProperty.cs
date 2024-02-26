// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2024 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodSeat.Liffom.Formats.Numerics
{
    /// <summary>
    /// 数値の表記用基数情報を表します。
    /// </summary>
    [Serializable()]
    public class RadixConvertFormatProperty : FormatProperty
    {
        /// <summary>
        /// 基数を表します。
        /// </summary>
        public enum RadixConvertMode
        {
            /// <summary>
            /// 2進数。
            /// </summary>
            _0b = 2, 
            /// <summary>
            /// 8進数。
            /// </summary>
            _0o = 8, 
            /// <summary>
            /// 10進数。
            /// </summary>
            _0d = 10, 
            /// <summary>
            /// 16進数。
            /// </summary>
            _0x = 16, 
        }

        /// <summary>
        /// 数値の表記用基数情報を初期化します。
        /// </summary>
        public RadixConvertFormatProperty() { }

        /// <summary>
        /// 表記用の基数を設定もしくは取得します。
        /// </summary>
        public RadixConvertMode Mode { get; set; } = RadixConvertMode._0d;

        /// <summary>
        /// 設定に基づいて指定の10進数数値を文字列("0x"等の接頭辞は付与しない)に変換します。
        /// </summary>
        /// <param name="n">変換元の数値。</param>
        /// <returns>変換された文字列("0x"等の接頭辞は付与しない)。</returns>
        public string Convert(Numeric n)
        {
            if (Mode == RadixConvertMode._0d) return n.ToString();

            bool isNegative = false;
            var figure = n.Figure;
            if (n < 0)
            {
                isNegative = true;
                figure *= -1;
            }

            var n_ = new Numeric(figure);

            var eps = 0.00001;

            var v2 = new Mod(n_, new Numeric(1.0)).Calculate() as Numeric; // 小数部
            var v1 = (n_ - v2).Numerate() as Numeric; // 整数部

            var b = (int)Mode;

            Func<Numeric, string> toDigit = n1 =>
            {
                if (n1.Figure < 10) return ((int)(n1.Figure.Value.ToDouble() + eps)).ToString();
                if (n1.Figure < 36) return ((char)((int)'a' + (int)(n1.Figure.Value.ToDouble() + eps) - 10)).ToString();
                throw new NotSupportedException();
            };

            var s = "";
            int tn1 = 0;
            while (v1 >= (double)b - eps)
            {
                Formula.CheckCancelOperation(n);

                var d = new Mod(v1, b).Calculate() as Numeric;
                s = toDigit(d) + s;
                v1 = ((v1 - d) / b + eps).Numerate() as Numeric;
                if (s.All(c => c == '0')) ++tn1;
            }
            s = toDigit(v1) + s;

            int m = figure.Value.MaxValidDigits * 10 / b + 1;

            int cnt = s == "0" ? 0 : s.Length;
            int tn0 = 0;
            var s0 = "";
            while (v2 != 0 && cnt <= m)
            {
                Formula.CheckCancelOperation(n);

                var v3 = (v2 * b).Numerate() as Numeric;
                v2 = new Mod(v3, new Numeric(1.0)).Calculate() as Numeric;

                var t = toDigit((v3 - v2).Numerate() as Numeric);
                s0 += t;
                if (cnt == 0 && t == "0") tn0++;
                if (cnt != 0 || t != "0") cnt++;
            }

            if (s == "0" && tn0 >= 4)
            {
                var s_ = s0.Trim('0');
                s = s_[0] + "." + s_.Substring(1) + "E-" + Convert(tn0 + 1);
            }
            else if (string.IsNullOrEmpty(s0) && tn1 > 4)
            {
                s = s[0] + "." + s.Substring(1).TrimEnd('0') + "E+" + Convert(s.Length - 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(s0)) s += "." + s0;
            }

            if (isNegative) s = "-" + s;
            return s;
        }

        /// <summary>
        /// 表記基数に対応する接頭辞文字列を取得します。
        /// </summary>
        /// <returns>接頭辞文字列。</returns>
        public string GetPrefix()
        {
            switch (Mode)
            {
                case RadixConvertMode._0d: return "";
                case RadixConvertMode._0b: return "0b";
                case RadixConvertMode._0o: return "0o";
                case RadixConvertMode._0x: return "0x";
                default: return "0n[" + ((int)Mode).ToString() + "]";
            }
        }

    }
}
