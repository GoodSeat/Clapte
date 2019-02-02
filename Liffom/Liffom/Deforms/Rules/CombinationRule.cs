// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Deforms.Rules
{
    /// <summary>
    /// OperatorMultipleの構成項の各組合せについて、ルールの適用による数式変形を試みるルールを表します。
    /// </summary>
    public abstract class CombinationRule : Rule
    {
        /// <summary>
        /// 組合せの適用回数タイプを表します。
        /// </summary>
        public enum ApplyType
        {
            /// <summary>
            /// 適用可能な組み合わせがある限り適用を続行します。
            /// </summary>
            NoLimit,
            /// <summary>
            /// 各項について、1度のみ適用を試みます。
            /// </summary>
            OneTimePerTerm,
            /// <summary>
            /// 何れかの組み合わせで適用があった場合に変形を終了します。
            /// </summary>
            OneTime
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

        /// <summary>
        /// 各組合せについてルールの適用を試みる方法を取得します。
        /// </summary>
        protected virtual ApplyType RuleApplyType { get { return ApplyType.NoLimit; } }

        /// <summary>
        /// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
        /// </summary>
        protected virtual bool RetryAll { get { return true; } }

        /// <summary>
        /// このルールの変形において、組み合わせが逆転しても結果が変わらないか否かを取得します。
        /// </summary>
        protected virtual bool Reversible { get { return false; } }

        /// <summary>
        /// ルールの適用後、演算の統合を実施するか否かを取得します。
        /// </summary>
        protected virtual bool IntegrateAfterRuled { get { return false; } }

        /// <summary>
        /// 指定した数式の組み合わせが、このルールの処理対象となるか否かを取得します。
        /// </summary>
        /// <param name="f1">組み合わせ対象となる前方の数式。</param>
        /// <param name="f2">組み合わせ対象となる後方の数式。</param>
        /// <returns>ルールの処理対象となるか否か。</returns>
        protected abstract bool IsTargetCouple(Formula f1, Formula f2);

        /// <summary>
        /// 指定した数式の組み合わせから、処理後の数式を取得します。
        /// </summary>
        /// <param name="f1">組み合わせ対象となる前方の数式。</param>
        /// <param name="f2">組み合わせ対象となる後方の数式。</param>
        /// <returns>処理後の数式。</returns>
        protected abstract Formula GetRuledFormula(Formula f1, Formula f2);


        /// <summary>
        /// ルールによる変形前に、ソートを行う場合に用いる比較メソッドを取得します。
        /// </summary>
        protected virtual Comparison<Formula> Comparison { get { return null; } }

        /// <summary>
        /// 指定ルールに従って、収束するまで数式の変形を行います。
        /// </summary>
        /// <param name="f">任意の組み合わせの数式と親数式を指定して、変形を試みる関数。</param>
        /// <param name="target">変形対象の数式。</param>
        /// <param name="ruleApplyType">ルール適用の方法を表すApplyType。</param>
        /// <param name="retryAll">ルールの適用があった場合に、再度すべての組み合わせについて適用を試みるか。</param>
        /// <param name="integrateAfterRuled">ルールの適用後、演算の統合を実施するか。</param>
        /// <returns>ルールの適用があった場合、適用後の数式。それ以外の場合、null。</returns>
        internal static OperatorMultiple DeformWith(Func<Formula, Formula, OperatorMultiple, Formula> f,
            OperatorMultiple target, ApplyType ruleApplyType, bool retryAll, bool integrateAfterRuled, Comparison<Formula> comparison)
        {
            List<Formula> consist = new List<Formula>(target.Formulas);
            if (comparison != null) consist.Sort(comparison);

            bool ruleTreated = false;
            for (int i = 0; i < consist.Count - 1; i++)
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

                    Formula rPost = f(r1, r2, target);
                    if (rPost == null) continue;

                    ruleTreated = true;
                    if (rPost.GetType() == target.GetType() && integrateAfterRuled)
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

                    if (ruleApplyType == ApplyType.OneTime || ruleApplyType == ApplyType.OneTimePerTerm) break;
                    if (retryAll)
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
                if (ruleTreated && ruleApplyType == ApplyType.OneTime) break;
            }

            if (ruleTreated) return target.CreateOperator(consist.ToArray()) as OperatorMultiple;
            else return null;
        }

        /// <summary>
        /// 指定ルールに従って、収束するまで数式の変形を行います。
        /// </summary>
        /// <param name="target">変形対象の数式。</param>
        /// <returns>ルールの適用があったか。</returns>
        private OperatorMultiple DeformWithRule(OperatorMultiple target)
        {
            return DeformWith(TryApply, target, RuleApplyType, RetryAll, IntegrateAfterRuled, Comparison);
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
            Formula rPost = null;
            if (IsTargetCouple(r1, r2)) rPost = GetRuledFormula(r1, r2);
            if (rPost != null) return rPost;

            if (!target.Satisfy(Operator.OperatorLaw.Commutative)) return null; // 交換則を満たさないなら無視
            if (Reversible) return null; // 組み合わせを逆にしても結果が変わらないなら無視

            if (IsTargetCouple(r2, r1)) rPost = GetRuledFormula(r2, r1);
            return rPost;
        }

    }
}
