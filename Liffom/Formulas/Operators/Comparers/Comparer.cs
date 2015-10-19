using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators.Rules;

namespace GoodSeat.Liffom.Formulas.Operators.Comparers
{
    /// <summary>
    /// 比較演算を表します。
    /// </summary>
    [Serializable()]
    public abstract class Comparer : OperatorBinary
    {
        /// <summary>
        /// 真偽値を表します。
        /// </summary>
        public enum Judge
        {
            /// <summary>判定不可</summary>
            None,
            /// <summary>真</summary>
            True,
            /// <summary>偽</summary>
            False
        }

        /// <summary>
        /// 比較演算子を初期化します。
        /// </summary>
        /// <param name="lhs">左辺。</param>
        /// <param name="rhs">右辺。</param>
        public Comparer(Formula lhs, Formula rhs)
            : base(lhs, rhs) { }
        
        /// <summary>
        /// 演算が満たす法則を取得します。
        /// </summary>
        public override OperatorLaw Law { get { return OperatorLaw.None; } }

        /// <summary>
        /// 演算の評価優先度を表す数値を取得します。
        /// 原則として、OperatorPriorityの値を返してください。
        /// </summary>
        public override int EvaluatePriority { get { return (int)OperatorPriority.Compare; } }

        /// <summary>
        /// 左項の数式を取得もしくは設定します。
        /// </summary>
        public Formula LeftHandSide
        {
            get { return Forword; }
            set { Forword = value; }
        }

        /// <summary>
        /// 右項の数式を取得もしくは設定します。
        /// </summary>
        public Formula RightHandSide
        {
            get { return Backword; }
            set { Backword = value; }
        }

        /// <summary>
        /// 比較結果を真偽値として取得します。
        /// </summary>
        /// <param name="token">比較結果取得に際して用いる変形識別子。</param>
        /// <returns></returns>
        public abstract Judge GetJudge(DeformToken token);


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

            if (deformToken.Has<NumerateToken>() || deformToken.Has<CalculateToken>()) yield return CalculateComparerRule.Entity;
        }

    }


}
