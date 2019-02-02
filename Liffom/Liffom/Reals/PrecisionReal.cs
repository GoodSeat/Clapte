// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Reals
{
    /// <summary>
    /// 有効数値の考慮、及び誤差の自動修正が可能な実数を表します。
    /// </summary>
    [Serializable()]
    public abstract class PrecisionReal : Real
    {
        /// <summary>
        /// 有効数字を考慮した数値を初期化します。
        /// </summary>
        public PrecisionReal() : base() { }

        /// <summary>
        /// 有効数値を無限大として数値を初期化します。
        /// </summary>
        /// <param name="data">初期化実数を表すdouble型実数。</param>
        public PrecisionReal(double data) : base(data) { }


        /// <summary>
        /// 有効桁数を設定もしくは取得します。
        /// </summary>
        public abstract int Precision { get; set; }

        /// <summary>
        /// このインスタンスの有効桁数が無限大と判断されるか否かを取得します。
        /// </summary>
        public abstract bool IsInfinityPrecision { get; }

        /// <summary>
        /// 数値の内部誤差をリセットします。
        /// MEMO:この奇妙なメソッドは、将来的には削除する予定です。
        /// </summary>
        public abstract void ModifyError();

    }
}
