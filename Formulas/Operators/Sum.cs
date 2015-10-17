using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators.Rules.Sums;
using GoodSeat.Liffom.Utilities;

namespace GoodSeat.Liffom.Formulas.Operators
{
    /// <summary>
    /// 和算を表します。
    /// </summary>
    [Serializable()]
    public class Sum : OperatorMultiple
    {
        /// <summary>
        /// 和算を作成します。
        /// </summary>
        /// <param name="formulas">対象数式</param>
        public Sum(params Formula[] formulas) : this(false, formulas) { }

        /// <summary>
        /// 和算を作成します。
        /// </summary>
        /// <param name="integration">trueを指定すると、構成数式内部のSumを統合します</param>
        /// <param name="formulas">対象数式</param>
        public Sum(bool integration, params Formula[] formulas)
            : base(integration, formulas) { }
        
        /// <summary>
        /// 演算が満たす法則を取得します。
        /// </summary>
        public override OperatorLaw Law 
        { 
            get
            { 
                return OperatorLaw.Associative | OperatorLaw.Commutative;
            }
        }
                
        /// <summary>
        /// 演算の評価優先度を表す数値を取得します。
        /// 原則として、OperatorPriorityの値を返してください。
        /// </summary>
        public override int EvaluatePriority { get { return (int)OperatorPriority.Sum; } }

        /// <summary>
        /// 引数を指定して、演算を生成します。
        /// </summary>
        /// <param name="args">初期化に用いるか変数の数式。</param>
        public override Operator CreateOperator(params Formula[] args)
        {
            return new Sum(args);
        }

        public override Formula DefaultValue { get { return 0; } }

        public override string GetText()
        {
            string result = string.Join("+", this);

//            result = ret.TrimEnd('+').Replace("+-1*", "-").Replace("+-", "-");
            result = result.TrimEnd('+').Replace("+-", "-");
            return result;
        }

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

            if (sender is Power) foreach (var rule in GetPowerRelatedRulesOf(sender as Power, deformToken)) yield return rule;
            if (sender is Product) foreach (var rule in GetProductRelatedRulesOf(sender as Product, deformToken)) yield return rule;
            if (sender is Sum) foreach (var rule in GetSumRelatedRulesOf(sender as Sum, deformToken)) yield return rule;
        }

        public IEnumerable<Rule> GetPowerRelatedRulesOf(Power sender, DeformToken deformToken)
        {
            yield break;
        }

        public IEnumerable<Rule> GetProductRelatedRulesOf(Product sender, DeformToken deformToken)
        {
            yield break;
        }

        public IEnumerable<Rule> GetSumRelatedRulesOf(Sum sender, DeformToken deformToken)
        {
            if (deformToken.Has<CombineToken>())
            {
                yield return new ReduceCommonDenominatorRule(); // 通分 (a/b) + c → (a + c*b) / b
                yield return new ReduceCommonDenominatorRuleLCM(); // 通分 (a/b) + (c/d) → (a*d + b*c) / (b*d)
            }
        }

    }
}
