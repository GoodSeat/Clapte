using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Liffom.Formulas
{
    public abstract partial class Formula
    {
        #region IEnumerable メンバー
               
        public IEnumerator<Formula> GetEnumerator()
        {
            int i = 0;
            while (this[i] != null) yield return this[i++];
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        #endregion

        #region IComparable<Formula> メンバー

        /// <summary>
        /// 数式の並び順比較結果を表します。
        /// </summary>
        protected enum CompareResult
        {
            Smaller = -2,
            Same = 0,
            Larger = 2
        }

        /// <summary>
        /// 数式の基本的な並び順を定義する比較結果を取得します。
        /// </summary>
        /// <remarks>
        /// 通常、次の順に従います。
        /// 数値 ＜ その他(文字列長比較順) ＜ 負の累乗 ＜ 単位
        /// </remarks>
        /// <param name="other">比較対象とする数式。</param>
        /// <returns>このインスタンスが先となる場合に負の数値、後となる場合に正の数値、同じとなる場合には0を返します。</returns>
        public int CompareTo(Formula other)
        {
            int compare1 = (int)this.IsLargerThan(other);
            int compare2 = (int)other.IsLargerThan(this);

#if DEBUG
            // 互いに同じ結果の場合、比較ができないのでエラーとする。
            if (other.GetUniqueText() != this.GetUniqueText() && compare1 != 0 && compare1 - compare2 == 0)
                throw new FormulaAssertionException("次の数式間の比較が正しく行えません。\n" + this.ToString() + ", " + other.ToString());
#endif

            if (compare1 != compare2) return compare1 - compare2;
            else return this.GetUniqueText().CompareTo(other.GetUniqueText());
        }

        /// <summary>
        /// 数式の基本的な並び順を定義する比較結果を取得します。
        /// </summary>
        /// <remarks>
        /// 通常、次の順に従います。
        /// 数値 ＜ その他(文字列長比較順) ＜ 負の累乗 ＜ 単位
        /// </remarks>
        /// <param name="other">比較対象とする数式。</param>
        /// <returns>比較結果。</returns>
        protected virtual CompareResult IsLargerThan(Formula other)
        {
            if (!this.IsUnit() &&  other.IsUnit()) return (CompareResult) (- 3);
            if ( this.IsUnit() && !other.IsUnit()) return (CompareResult)3;

            return (CompareResult)Math.Sign(GetUniqueText().Length.CompareTo(other.GetUniqueText().Length));
        }

        #endregion
    }
}
