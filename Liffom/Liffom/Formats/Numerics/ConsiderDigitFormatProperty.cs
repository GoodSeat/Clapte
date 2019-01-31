// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Formats.Numerics
{
    /// <summary>
    /// 数値の有効数字考慮の表記情報を表します。
    /// </summary>
    [Serializable()]
    public class ConsiderSignificantFiguresFormatProperty : FormatProperty
    {
        /// <summary>
        /// 数値の有効数字考慮表記情報を初期化します。
        /// </summary>
        public ConsiderSignificantFiguresFormatProperty() : this(false) { }

        /// <summary>
        /// 数値の有効数字考慮表記情報を初期化します。
        /// </summary>
        /// <param name="consider">有効数字を考慮した表記とするか否か。</param>
        public ConsiderSignificantFiguresFormatProperty(bool consider)
        {
            Consider = consider;
        }

        /// <summary>
        /// 有効数字を考慮した表記とするか否かを設定もしくは取得します。
        /// </summary>
        public bool Consider { get; set; }

        /// <summary>
        /// bool型からの暗黙的変換を行います。
        /// </summary>
        /// <param name="property">変換対象のプロパティ。</param>
        /// <returns>有効数字考慮の表記とするか否か。</returns>
        public static implicit operator ConsiderSignificantFiguresFormatProperty(bool property)
        {
            return new ConsiderSignificantFiguresFormatProperty(property);
        }

        /// <summary>
        /// bool型への暗黙的変換を行います。
        /// </summary>
        /// <param name="property">変換対象のプロパティ。</param>
        /// <returns>有効数字考慮の表記とするか否か。</returns>
        public static implicit operator bool(ConsiderSignificantFiguresFormatProperty property)
        {
            return property.Consider;
        }
    }
}





