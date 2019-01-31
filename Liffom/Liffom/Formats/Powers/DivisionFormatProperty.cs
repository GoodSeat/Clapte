// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Formats.Powers
{
    /// <summary>
    /// 負の累乗を 1/x の形式で表示するかの出力書式を表します。
    /// </summary>
    [Serializable()]
    public class DivisionFormatProperty : FormatProperty
    {
        /// <summary>
        /// 負の累乗を 1/x の形式で表示するかの設定を初期化します。
        /// </summary>
        public DivisionFormatProperty() : this(false) { }

        /// <summary>
        /// 負の累乗を 1/x の形式で表示するかの設定を初期化します。
        /// </summary>
        /// <param name="division">1/x の形式で表示するか。</param>
        public DivisionFormatProperty(bool division)
        {
            Division = division;
        }


        /// <summary>
        /// 負の累乗を 1/x の形式で表示するか否かを設定もしくは取得します。
        /// </summary>
        public bool Division { get; set; }

        /// <summary>
        /// DivisionFormatProperty型への暗黙的変換を行います。
        /// </summary>
        /// <param name="property">変換対象のプロパティ。</param>
        /// <returns>指定した属性に対応する属性情報。</returns>
        public static implicit operator DivisionFormatProperty(bool property)
        {
            return new DivisionFormatProperty(property);
        }

        /// <summary>
        /// bool型への暗黙的変換を行います。
        /// </summary>
        /// <param name="property">変換対象のプロパティ。</param>
        /// <returns>負の累乗を 1/x の形式で表示するか否か。</returns>
        public static implicit operator bool(DivisionFormatProperty property)
        {
            return property.Division;
        }
    }
}


