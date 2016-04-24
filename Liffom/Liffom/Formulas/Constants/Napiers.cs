using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Constants.Rules;
using GoodSeat.Liffom.Deforms.Rules;

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

        public override Formula Value { get { return Numeric.Zero.Figure.GetNapiers(); } }

        /// <summary>
        /// 指定処理に関連するルールをすべて返す反復子を取得します。
        /// </summary>
        /// <param name="deformToken">変形識別トークン。</param>
        /// <param name="sender">ルール適用対象となる最上位親数式。</param>
        /// <param name="history">変形履歴情報。</param>
        /// <returns>変形に関連するルールを返す反復子。</returns>
        public override IEnumerable<Rule> GetRelatedRulesOf(DeformToken deformToken, Formula sender, DeformHistory history) 
        {
            foreach (var rule in base.GetRelatedRulesOf(deformToken, sender, history)) yield return rule;
        }
    }
}
