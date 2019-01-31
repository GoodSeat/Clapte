// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Rules;

namespace GoodSeat.Liffom.Deforms
{
    /// <summary>
    /// 子数式に対して、変形トークンに基づく変形を再帰的に適用するルールを表します。
    /// </summary>
    public class DeformChildrenRule : Rule
    {
        private DeformChildrenRule() { }

        /// <summary>
        /// 子数式に対して、変形トークンに基づく変形を再帰的に適用するルールを初期化します。
        /// </summary>
        /// <param name="deformToken">子に適用する変形トークン</param>
        /// <param name="parentHistoryNode">この数式の変形履歴系統の親となる履歴地点情報</param>
        public DeformChildrenRule(DeformToken deformToken, DeformHistoryNode parentHistoryNode) 
        {
            CauseDeformToken = deformToken;
            ParentHistoryNode = parentHistoryNode;
        }

        /// <summary>
        /// 再帰的変形を行う変形処理を設定もしくは取得します。
        /// </summary>
        private DeformToken CauseDeformToken { get; set; }

        /// <summary>
        /// この数式の変形履歴系統の親となる履歴地点情報を設定もしくは取得します。
        /// </summary>
        private DeformHistoryNode ParentHistoryNode { get; set; }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target[0] != null; }

        protected override Formula OnTryMatchRule(Formula target)
        {
            Formula.IsTargetFormula isAbortApply = null;
            if (target is OperatorMultiple) isAbortApply = f => f.GetEqualBaseType() == target.GetEqualBaseType(); // a+(a*(a+b)) → a+(aa+ab) となったら、統合処理の必要があるため、ルールの再帰適用を中止する。

            bool applied = false;
            var deformed = new List<Formula>();

            int i = 0;
            while (target[i] != null)
            {
                // 子の数式変形履歴を登録
                var initialNode = new DeformHistoryNode(target[i]);
                var childHistory = new DeformHistory(initialNode, ParentHistoryNode);
                if (ParentHistoryNode != null) ParentHistoryNode.AddChildHistory(childHistory);

                // もともと同じ形式の子数式なら、統合処理のための処理中止は行わない (3*3^a*2^a)*[kN*m] ⇒ (3*(3*2)^a)*[kN*m] となっても処理を続行
                Formula.IsTargetFormula isAbort = null;
                if (target[i].GetEqualBaseType() != target.GetEqualBaseType()) isAbort = isAbortApply;

                deformed.Add(Deform.Apply(CauseDeformToken, target[i], childHistory, isAbort));

                if (!applied && childHistory.CurrentNode != initialNode)
                {
                    if (initialNode.FormulaUniqueText != childHistory.CurrentNode.FormulaUniqueText)
                        applied = true; // 初期履歴地点から変更があったら、何かしらのルール適用があったと判定。
                }
                i++;
            }

            if (applied) return target.CreateFromChildren(deformed.ToArray());
            return null;
        }

        public override string Information
        {
            get { return "数式を構成する各部分に対して、再帰的に変形を行います。"; }
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            // 子数式を変形するまでもなく消せるものは消す
            yield return typeof(RemoveInvalidOneOnProductRule);
            yield return typeof(DeleteZeroOnSumRule);
            yield return typeof(DeleteZeroOnProductRule);
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("a+(a*(a+b))"),
                Formula.Parse("a+(a*a+a*b)")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return new DeformChildrenRule(new SimplifyToken(), null); }

        public override Rule GetClone() { return this; }
    }
}
