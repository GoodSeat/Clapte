using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;

namespace GoodSeat.Liffom.Formulas.Constants
{
    /// <summary>
    /// ネイピア数(自然対数の底)eを表します。
    /// </summary>
    [Serializable()]
    public class Napiers : Constant
    {
        /// <summary>
        /// ネイピア数を表します。このフィールドは定数です。
        /// </summary>
        public static readonly Napiers e = new Napiers();

        /// <summary>
        /// ネイピア数(自然対数の底)eを初期化します。
        /// </summary>
        public Napiers() { }

        /// <summary>
        /// この定数として識別する文字列を取得します。
        /// </summary>
        public override string DistinguishedName { get { return "e"; } }

        /// <summary>
        /// 定数の説明を取得します。
        /// </summary>
        public override string Information { get { return "自然対数の底"; } }

        public override Formula Value { get { return Math.E; } }
    }
}
