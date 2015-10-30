using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Deforms.Rules
{
    /// <summary>
    /// OperatorMultipleの構成項の各組合せについて、数式パターンルールの適用による数式変形を試みるルールを表します。
    /// </summary>
    public abstract class CombinationPatternRule : PatternRule
    {
        Formula _rule1;
        Formula _rule2;

        /// <summary>
        /// 対象となる前方ルール数式を取得します。
        /// </summary>
        private Formula Rule1
        {
            get
            {
                if (_rule1 == null) GetRulePatternFormula(out _rule1, out _rule2);
                return _rule1;
            }
        }

        /// <summary>
        /// 対象となる後方ルール数式を取得します。
        /// </summary>
        private Formula Rule2
        {
            get
            {
                if (_rule2 == null) GetRulePatternFormula(out _rule1, out _rule2);
                return _rule2;
            }
        }

        /// <summary>
        /// 処理ルールに従って数式に対する変換処理を試みます。
        /// </summary>
        /// <param name="target">処理対象の数式。</param>
        /// <returns>変形後の数式。変形のない場合、null。</returns>
        protected override Formula OnTryMatchRule(Formula target)
        {
            var targetOperator = target as OperatorMultiple;
            if (targetOperator == null) return null;
            OperatorMultiple result = DeformWithRule(targetOperator);

            if (result == null) return null;

            if (result.Count == 0) return result.DefaultValue; // すべて消えたらデフォルト値となる。
            if (result.Count == 1) return result[0]; // 演算を構成する項が1つなら、演算である必要はない。

            return result;
        }

        protected override Formula GetRulePatternFormula()
        {
            throw new FormulaRuleException("CombinationPatternRuleでは、GetRulePatternFormulaメソッドの呼び出しは想定されていません。");
        }

        /// <summary>
        /// 各組合せについてルールの適用を試みる方法を取得します。
        /// </summary>
        protected virtual CombinationRule.ApplyType RuleApplyType { get { return CombinationRule.ApplyType.NoLimit; } }

        /// <summary>
        /// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
        /// </summary>
        protected virtual bool RetryAll { get { return true; } }

        /// <summary>
        /// このルールの変形において、組み合わせが逆転しても結果が変わらないか否かを取得します。
        /// </summary>
        protected virtual bool Reversible { get { return false; } }
                
        /// <summary>
        /// 処理対象を表すルールパターン数式を取得します。
        /// </summary>
        protected abstract void GetRulePatternFormula(out Formula formula1, out Formula formula2);

        /// <summary>
        /// ルールの適用後、演算の統合を実施するか否かを取得します。
        /// </summary>
        protected virtual bool IntegrateAfterRuled { get { return false; } }

        /// <summary>
        /// 処理対象を表すルール数式をリセットします。ルール独自のプロパティが変化して、対象となる数式に変更があった場合には、本メソッドを呼び出して下さい。
        /// </summary>
        protected override void ResetRulePatternFormula() { _rule1 = null; _rule2 = null; }

        /// <summary>
        /// 指定ルールに従って、収束するまで数式の変形を行います。
        /// </summary>
        /// <param name="target">変形対象の数式。</param>
        /// <returns>ルールの適用があったか。</returns>
        private OperatorMultiple DeformWithRule(OperatorMultiple target)
        {
            return CombinationRule.DeformWith(TryApply, target, RuleApplyType, RetryAll, IntegrateAfterRuled);
        }

        /// <summary>
        /// 指定した数式の組み合わせに対して、ルールの適用を試みます。
        /// </summary>
        /// <param name="r1">前方の数式。</param>
        /// <param name="r2">後方の数式。</param>
        /// <param name="target">親数式となる演算。</param>
        /// <returns>ルールの適用に成功した場合は適用後の数式。それ以外の場合、null。</returns>
        private Formula TryApply(Formula r1, Formula r2, OperatorMultiple target)
        {
            ClearPatternVariable();

            if (!r1.PatternMatch(Rule1) || !r2.PatternMatch(Rule2, false))
            {
                if (!target.Satisfy(Operator.OperatorLaw.Commutative)) return null; // 交換則を満たさないなら無視
                if (Reversible) return null; // 組み合わせを逆にしても結果が変わらないなら無視

                ClearPatternVariable();

                if (!r2.PatternMatch(Rule1) || !r1.PatternMatch(Rule2, false)) return null;
            }
#if DEBUG
            foreach (RulePatternVariable r in UsingPatternVariables.Values)
                FormulaAssertionException.Assert(r.MatchedFormula != null);
#endif
            return GetRuledFormula();
        }

    }
}
