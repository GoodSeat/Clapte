using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;

namespace GoodSeat.Liffom.Formulas.Constants
{
    /// <summary>
    /// 円周率を表します。
    /// </summary>
    [Serializable()]
    public class Pi : Constant
    {
        /// <summary>
        /// 円周率を表します。このフィールドは定数です。
        /// </summary>
        public static readonly Pi pi = new Pi();

        /// <summary>
        /// 円周率を初期化します。
        /// </summary>
        public Pi() { }

        /// <summary>
        /// この定数として識別する文字列を取得します。
        /// </summary>
        public override string DistinguishedName { get { return "pi"; } }

        /// <summary>
        /// この定数として読み込む文字列を全て返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetAllDistinguishedNames()
        {
            yield return DistinguishedName;
            yield return "π";
        }

        /// <summary>
        /// 定数の説明を取得します。
        /// </summary>
        public override string Information { get { return "円周率"; } }

        public override Formula Value { get { return Numeric.Zero.Figure.GetPi(); } }
    }
}
