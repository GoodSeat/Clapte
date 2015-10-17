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
            List<Formula> consist = new List<Formula>(target.Formulas);

            bool ruleTreated = false;
            for (int i = 0; i < consist.Count; i++)
            {
                if (i != 0 && !target.Satisfy(Operator.OperatorLaw.Associative)) break; // 結合則を満たさない場合

                Formula r1 = consist[i];
                if (r1 == null) break;

                for (int k = i + 1; k < consist.Count; k++)
                {
                    if (k != i + 1 && !target.Satisfy(Operator.OperatorLaw.Commutative)) break; // 交換則を満たさない場合
                    if (k != i + 1 && !target.Satisfy(Operator.OperatorLaw.Associative)) break; // 結合則を満たさない場合

                    Formula r2 = consist[k];
                    if (r2 == null) break;

                    ClearPatternVariable();

                    bool ruleMatch = false;
                    if (r1.PatternMatch(Rule1) && r2.PatternMatch(Rule2, false)) ruleMatch = true;
                    else if (!target.Satisfy(Operator.OperatorLaw.Commutative)) continue; // 交換則を満たさないなら無視
                    else if (Reversible) continue; // 組み合わせを逆にしても結果が変わらないなら無視
                    else ClearPatternVariable();

                    if (!ruleMatch && r2.PatternMatch(Rule1) && r1.PatternMatch(Rule2, false)) ruleMatch = true;
                    if (!ruleMatch) continue;
#if DEBUG
                    foreach (RulePatternVariable r in UsingPatternVariables.Values)
                        FormulaAssertionException.Assert(r.MatchedFormula != null);
#endif
                    Formula rPost = GetRuledFormula();
                    if (rPost == null) continue;
#if DEBUG
                    FormulaAssertionException.Assert(rPost.GetExistFactor<RulePatternVariable>().Count == 0);
#endif                
                    ruleTreated = true;
                    if (rPost.GetType() == target.GetType() && IntegrateAfterRuled)
                    {
                        consist.RemoveAt(k);
                        consist.InsertRange(i + 1, rPost);
                        consist.RemoveAt(i);
                    }
                    else
                    {
                        consist[i] = rPost;
                        consist.RemoveAt(k);
                    }

                    if (RetryAll)
                    {
                        i = -1;
                        break;
                    }
                    else
                    {
                        r1 = consist[i];
                        if (r1 == null) break;
                        k = i;
                    }
                }
            }

            if (ruleTreated) return target.CreateOperator(consist.ToArray()) as OperatorMultiple;
            else return null;
        }


    }
}
