using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Matrices.Rules
{        
    /// <summary>
    /// スカラーと行列の積を規定するルールを表します。
    /// </summary>
    public class ScalarMultiplicationRule : CombinationRule
    {
        static ScalarMultiplicationRule s_entity;

        /// <summary>
        /// スカラーと行列の積を規定するルールの実体を取得します。
        /// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static ScalarMultiplicationRule Entity
        {
            get
            {
                if (s_entity == null)
                {
                    s_entity = new ScalarMultiplicationRule();
                }
                return s_entity;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Product; }

        /// <summary>
        /// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
        /// </summary>
        protected override bool RetryAll { get { return false; } }

        /// <summary>
        /// 指定した数式の組み合わせが、このルールの処理対象となるか否かを取得します。
        /// </summary>
        /// <param name="f1">組み合わせ対象となる前方の数式。</param>
        /// <param name="f2">組み合わせ対象となる後方の数式。</param>
        /// <returns>ルールの処理対象となるか否か。</returns>
        protected override bool IsTargetCouple(Formula f1, Formula f2)
        {
            return !(f1.Contains<Matrix>()) && f2 is Matrix;
        }

        /// <summary>
        /// 指定した数式の組み合わせから、処理後の数式を取得します。
        /// </summary>
        /// <param name="f1">組み合わせ対象となる前方の数式。</param>
        /// <param name="f2">組み合わせ対象となる後方の数式。</param>
        /// <returns>処理後の数式。</returns>
        protected override Formula GetRuledFormula(Formula f1, Formula f2)
        {
            Matrix result = new Matrix(f2 as Matrix);
            for (int r = 1; r <= result.RowSize; r++)
            {
                for (int c = 1; c <= result.ColumnSize; c++)
                {
                    result[r, c] *= f1;
                }
            }
            return result;
        }

        public override string Information
        {
            get { return "スカラーと行列の積を規定するルールです。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("5 * {[1,2], [3,4]}"),
                Formula.Parse("{[1*5, 2*5], [3*5, 4*5]}")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return Entity;
        }

        public override Rule GetClone()
        {
            return Entity;
        }
    }
}
