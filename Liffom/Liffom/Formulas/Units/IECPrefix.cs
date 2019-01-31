// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Liffom.Formulas.Units
{
    /// <summary>
    /// IEC規格の接頭辞を表します。
    /// </summary>
    [Serializable()]
    public class IECPrefix : Prefix
    {
        /// <summary>
        /// IEC規格における接頭辞を表します。
        /// </summary>
        public enum Marks
        {
            /// <summary> ヨビ 2^80 </summary>
            Yi = 80,
            /// <summary> ゼビ 2^70 </summary>
            Zi = 70,
            /// <summary> エクスビ 2^60 </summary>
            Ei = 60,
            /// <summary> ペビ 2^50 </summary>
            Pi = 50,
            /// <summary> テビ 2^40 </summary>
            Ti = 40,
            /// <summary> ギビ 2^30 </summary>
            Gi = 30,
            /// <summary> メビ 2^20 </summary>
            Mi = 20,
            /// <summary> キビ 2^10 </summary>
            Ki = 10,
            /// <summary> なし 2^0 </summary>
            none = 0
        }
        
        Marks _mark = Marks.none;
        
        /// <summary>
        /// IEC接頭辞を初期化します。
        /// </summary>
        public IECPrefix()
            : this(Marks.none)
        { }

        /// <summary>
        /// IEC接頭辞を初期化します。
        /// </summary>
        /// <param name="mark">接頭辞タイプ</param>
        public IECPrefix(Marks mark)
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
                case Marks.Yi: return "ヨビ";
                case Marks.Zi: return "ゼビ";
                case Marks.Ei: return "エクスビ";
                case Marks.Pi: return "ペビ";
                case Marks.Ti: return "テビ";
                case Marks.Gi: return "ギビ";
                case Marks.Mi: return "メビ";
                case Marks.Ki: return "キビ";
                case Marks.none: return "";
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// IEC規格に属する全ての接頭辞を返す反復子を取得します。
        /// </summary>
        /// <returns>全ての接頭辞を返す反復子</returns>
        public override IEnumerable<Prefix> GetAllPrefixs()
        {
            foreach (Marks mark in Enum.GetValues(typeof(Marks)))
            {
                if (mark == Marks.none) continue;
                yield return new IECPrefix(mark);
            }
        }

        public override string Mark
        {
            get { return _mark.ToString(); }
        }

        public override string Name
        {
            get { return GetPrefixText(_mark); }
        }

        public override double Base
        {
            get { return 2.0; }
        }

        public override double Power
        {
            get { return (int)_mark; }
        }
    }
}
