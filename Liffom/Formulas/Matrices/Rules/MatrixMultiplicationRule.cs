using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Matrices.Rules
{        
    /// <summary>
    /// 行列の積を規定するルールです。
    /// </summary>
    public class MatrixMultiplicationRule : CombinationRule
    {
        static MatrixMultiplicationRule s_entity;

        /// <summary>
        /// 行列の積を規定するルールの実体を取得します。
        /// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static MatrixMultiplicationRule Entity
        {
            get
            {
                if (s_entity == null)
                {
                    s_entity = new MatrixMultiplicationRule();
                }
                return s_entity;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is ProductOfMatrix; }

        /// <summary>
        /// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
        /// </summary>
        protected override bool RetryAll { get { return false; } }

        /// <summary>
        /// このルールの変形において、組み合わせが逆転しても結果が変わらないか否かを取得します。
        /// </summary>
        protected override bool Reversible { get { return true; } }

        /// <summary>
        /// 指定した数式の組み合わせが、このルールの処理対象となるか否かを取得します。
        /// </summary>
        /// <param name="f1">組み合わせ対象となる前方の数式。</param>
        /// <param name="f2">組み合わせ対象となる後方の数式。</param>
        /// <returns>ルールの処理対象となるか否か。</returns>
        protected override bool IsTargetCouple(Formula f1, Formula f2)
        {
            var m1 = f1 as Matrix;
            var m2 = f2 as Matrix;
            if (m1 == null || m2 == null) return false;
            return m1.ColumnSize == m2.RowSize;
        }

        /// <summary>
        /// 指定した数式の組み合わせから、処理後の数式を取得します。
        /// </summary>
        /// <param name="f1">組み合わせ対象となる前方の数式。</param>
        /// <param name="f2">組み合わせ対象となる後方の数式。</param>
        /// <returns>処理後の数式。</returns>
        protected override Formula GetRuledFormula(Formula f1, Formula f2)
        {
            var m1 = f1 as Matrix;
            var m2 = f2 as Matrix;

            var result = new Matrix(m1.RowSize, m2.ColumnSize);
            for (int r = 1; r <= result.RowSize; r++)
            {
                for (int c = 1; c <= result.ColumnSize; c++)
                {
                    var sumList = new List<Formula>();
                    for (int i = 1; i <= m1.ColumnSize; i++)
                    {
                        sumList.Add(m1[r, i] * m2[i, c]);
                    }

                    if (sumList.Count == 1) result[r, c] = sumList[0];
                    else result[r, c] = new Sum(sumList.ToArray());
                }
            }
            return result;
        }

        public override string Information
        {
            get { return "行列の積を規定するルールです。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("[a, b] . {[1,2], [3,4]}"),
                Formula.Parse("[a*1 + b*3, a*2 + b*4]")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("{[1,2], [3,4]} . {a, b}"),
                Formula.Parse("{a*1 + b*2, a*3 + b*4}")
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

