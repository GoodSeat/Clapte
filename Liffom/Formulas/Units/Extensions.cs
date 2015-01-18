using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Units
{
    public static class Extensions
    {
		/// <summary>
		/// 指定数式が単位として扱われるべき数式(単位扱い数式)か否かを取得します。
		/// </summary>
		/// <param name="f">判定対象の数式。</param>
		/// <returns>単位扱い数式か否か。</returns>
		public static bool IsUnit(this Formula f)
		{
			return IsUnit(f, false);
		}

		/// <summary>
		/// 指定数式が単位として扱われるべき数式(単位扱い数式)か否かを取得します。
		/// </summary>
		/// <param name="f">判定対象の数式。</param>
		/// <param name="alsoOne">1を無次元単位として認めるか。</param>
		/// <returns>単位扱い数式か否か。</returns>
		/// <remarks>
		/// [kg]等の単位、[kg/m^2]等が単位扱い数式となります。
		/// また、1も例外的に無次元単位を表す単位扱い数式となります。
		/// </remarks>
		public static bool IsUnit(this Formula f, bool alsoOne)
		{
			if (f is Unit) return true;
			else if (f is Product)
			{
				bool existIsUnit = false;
				bool exist1 = false;
				foreach (Formula child in f)
				{
					if (child == 1)
					{
						exist1 = true;
						continue; // 1/m2 を許可するため
					}
					if (!IsUnit(child, alsoOne)) return false;
					existIsUnit = true;

					if (exist1 && (!(child is Power) || (child as Power).Exponent >= 0))
						return false;
				}
				return existIsUnit;
			}
			else if (f is Power)
			{
				return IsUnit((f as Power).Base, alsoOne);
			}
			else if (alsoOne && f == 1) return true; // 無次元単位として例外的に許可する

			return false;
		}
		

		/// <summary>
		/// 数式中の単位を無次元化します。
		/// </summary>
		/// <param name="f">対象の数式。</param>
		/// <returns>単位を無次元化した数式。</returns>
		public static Formula ClearUnit(this Formula f)
		{
			int i = 0;
			while (f[i] != null) 
			{
				f[i] = ClearUnit(f[i]);
				i++;
			}
				
			if (f is OperatorMultiple)
			{
				var op = f as OperatorMultiple;
				i = 0;
				while (f[i] != null)
				{
					if (f[i].IsUnit()) f[i] = op.DefaultValue;
					i++;
				}
			}
			return f.Simplify();
		}

    }
}
