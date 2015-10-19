using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Liffom.Utilities
{
    /// <summary>
    /// コンビネーションの取得に関する操作を提供します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Combination<T> where T : IComparable<T>
    {
        /// <summary>
        /// f個の中からn個を選ぶ、すべてのコンビネーション（fCn）の全リストを取得します
        /// </summary>
        /// <param name="n"></param>
        /// <param name="factors"></param>
        /// <returns></returns>
        public static List<List<T>> GetAllCombination(int n, params T[] factors)
        {
            //順番を並び替えないと重複が多数出ちゃう
            List<T> toSort = new List<T>(factors);

            return GetAllCombinationFromSortedList(n, toSort);
        }

        /// <summary>
        /// f個の中からn個を選ぶ、すべてのコンビネーション（fCn）の全リストを取得します
        /// </summary>
        /// <param name="n"></param>
        /// <param name="factors"></param>
        /// <returns></returns>
        public static List<List<T>> GetAllCombination(int n, List<T> factors)
        {
            //順番を並び替えないと重複が多数出ちゃう
            factors.Sort();

            return GetAllCombinationFromSortedList(n, factors);
        }

        /// <summary>
        /// f個の中からn個を選ぶ、すべてのコンビネーション（fCn）の全リストを取得します
        /// </summary>
        /// <param name="n"></param>
        /// <param name="factors"></param>
        /// <returns></returns>
        static List<List<T>> GetAllCombinationFromSortedList(int n, List<T> factors)
        {
            List<List<T>> ret = new List<List<T>>();
            List<T> already = new List<T>();

            if (n == 1)
            {
                foreach (T f in factors)
                {
                    if (already.Contains(f)) continue;//これはすでに選んだ
                    already.Add(f);

                    List<T> ad = new List<T>();
                    ad.Add(f);
                    ret.Add(ad);
                }
            }
            else
            {
                for (int i = 0; i < factors.Count; i++)
                {
                    if (already.Contains(factors[i])) continue;
                    already.Add(factors[i]);

                    List<T> next = new List<T>();//今選んだやつより下のやつのみ渡せばよい
                    for (int k = i + 1; k < factors.Count; k++)
                        next.Add(factors[k]);

                    List<List<T>> nextAd = GetAllCombination(n - 1, next.ToArray());

                    foreach (List<T> flist in nextAd)
                    {
                        List<T> ad = new List<T>();
                        ad.Add(factors[i]);
                        ad.AddRange(flist.ToArray());
                        ret.Add(ad);
                    }
                }
            }

            return ret;
        }

    }
}
