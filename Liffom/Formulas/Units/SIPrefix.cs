using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Liffom.Formulas.Units
{
    /// <summary>
    /// SI接頭辞を表します。
    /// </summary>
    [Serializable()]
    public class SIPrefix : Prefix
    {
        /// <summary>
        /// 国際単位系における接頭辞を表します。
        /// </summary>
        public enum Marks
        {
            /// <summary> ヨタ 10^24 </summary>
            Y = 24,
            /// <summary> ゼタ 10^21 </summary>
            Z = 21,
            /// <summary> エクサ 10^18 </summary>
            E = 18,
            /// <summary> ペタ 10^15 </summary>
            P = 15,
            /// <summary> テラ 10^12 </summary>
            T = 12,
            /// <summary> ギガ 10^9 </summary>
            G = 9,
            /// <summary> メガ 10^6 </summary>
            M = 6,
            /// <summary> キロ 10^3 </summary>
            k = 3,
            /// <summary> ヘクト 10^2 </summary>
            h = 2,
            /// <summary> デカ 10^1 </summary>
            da = 1,
            /// <summary> なし 10^0 </summary>
            none = 0,
            /// <summary> デシ 10^-1 </summary>
            d = -1,
            /// <summary> センチ 10^-2 </summary>
            c = -2,
            /// <summary> ミリ 10^-3 </summary>
            m = -3,
            /// <summary> マイクロ(μ) 10^-6 </summary>
            micro = -6,
            /// <summary> ナノ 10^-9 </summary>
            n = -9,
            /// <summary> ピコ 10^-12 </summary>
            p = -12,
            /// <summary> フェムト 10^-15 </summary>
            f = -15,
            /// <summary> アト 10^-18 </summary>
            a = -18,
            /// <summary> ゼプト 10^-21 </summary>
            z = -21,
            /// <summary> ヨクト 10^-24 </summary>
            y = -24
        }

        Marks _mark = Marks.none;

        /// <summary>
        /// SI接頭辞を初期化します。
        /// </summary>
        public SIPrefix()
            : this(Marks.none)
        { }

        /// <summary>
        /// SI接頭辞を初期化します。
        /// </summary>
        /// <param name="mark">接頭辞タイプ</param>
        public SIPrefix(Marks mark)
        {
            _mark = mark;
        }

        /// <summary>
        /// 指定接頭辞の日本語発音を取得します。
        /// </summary>
        /// <param name="mark">接頭辞タイプ</param>
        /// <returns>接頭辞の日本語読み</returns>
        public static string GetPrefixText(Marks mark)
        {
            switch (mark)
            {
                case Marks.Y: return "ヨタ";
                case Marks.Z: return "ゼタ";
                case Marks.E: return "エクサ";
                case Marks.P: return "ペタ";
                case Marks.T: return "テラ";
                case Marks.G: return "ギガ";
                case Marks.M: return "メガ";
                case Marks.k: return "キロ";
                case Marks.h: return "ヘクト";
                case Marks.da: return "デカ";
                case Marks.none: return "";
                case Marks.d: return "デシ";
                case Marks.c: return "センチ";
                case Marks.m: return "ミリ";
                case Marks.micro: return "マイクロ";
                case Marks.n: return "ナノ";
                case Marks.p: return "ピコ";
                case Marks.f: return "フェムト";
                case Marks.a: return "アト";
                case Marks.z: return "ゼプト";
                case Marks.y: return "ヨクト";
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// SI規格に属する全ての接頭辞を返す反復子を取得します。
        /// </summary>
        /// <returns>全ての接頭辞を返す反復子</returns>
        public override IEnumerable<Prefix> GetAllPrefixs()
        {
            foreach (Marks mark in Enum.GetValues(typeof(Marks)))
            {
                if (mark == Marks.none) continue;
                yield return new SIPrefix(mark);
            }
        }

        /// <summary>
        /// 単位接頭辞の表記文字列を取得します。
        /// </summary>
        public override string Mark
        {
            get { return _mark.ToString(); }
        }

        /// <summary>
        /// 接頭辞の呼び名を取得します。
        /// </summary>
        public override string Name
        {
            get { return GetPrefixText(_mark); }
        }

        /// <summary>
        /// 単位接頭辞の基数を取得します。
        /// </summary>
        public override double Base
        {
            get { return 10.0; }
        }

        /// <summary>
        /// 接頭辞の指数値を取得します。
        /// </summary>
        public override double Power
        {
            get { return (int)_mark; }
        }
    }
}
