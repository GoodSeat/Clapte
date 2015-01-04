using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liffom.Utilities
{
	public static class Sort
	{
		/// <summary>
		/// 指定したリストに対して、安定なソートを実行します。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		public static void StableSort<T>(List<T> list) where T : IComparable<T>
		{
			StableSort<T>(list, (one, another) => one.CompareTo(another));
		}

		/// <summary>
		/// 指定したリストに対して、安定なソートを実行します。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="comparison"></param>
		public static void StableSort<T>(List<T> list, Comparison<T> comparison)
		{
			for (int i = 0; i < list.Count - 1; i++)
			{
				T least = list[i];
				int swapIndex = i;
				for (int k = i + 1; k < list.Count; k++)
				{
					if (comparison(least, list[k]) > 0)
					{
						least = list[k];
						swapIndex = k;
					}
				}
				list.RemoveAt(swapIndex);
				list.Insert(i, least);
			}
		}

	}
}
