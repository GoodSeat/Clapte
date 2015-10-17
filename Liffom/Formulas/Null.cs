using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Liffom.Formulas
{
    /// <summary>
    /// 数式不在を表します。
    /// </summary>
    [Serializable()]
    public class Null : Formula
    {
        static Null s_null = new Null();

        /// <summary>
        /// 数式不在数式の実態を取得します。
        /// </summary>
        public static Null Empty { get { return s_null; } }

        public override string GetText() { return ""; }

        /// <summary>
        /// 数式の一意性評価に用いる文字列で、同じ型の数式同士の一意性を表す文字列を取得します。
        /// </summary>
        /// <returns>数式を一意に区別する文字列。</returns>
        protected override string OnGetUniqueText() { return ""; }
    }
}
