using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Extensions;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Processes
{
	/// <summary>
	/// 数式の無平方分解処理を表します。
	/// </summary>
	public class SquareFreeDecomposition : Process
	{
		/// <summary>
		/// 一意のユーザー情報を指定して、複数の同時呼び出しを許可するか否かを取得します。
		/// </summary>
		public override bool IsSupportMultipleConcurrentInvocations { get { return true; } }

		/// <summary>
		/// 処理対象とする数式数の下限値を取得します。
		/// </summary>
		public override int TargetFormulasMinQty { get { return 1; } }

		/// <summary>
		/// 指定された数式に対して、処理を実行します。
		/// </summary>
		/// <param name="targets">無平方分解対象の数式。</param>
		protected override Formula OnDo(object userState, params Formula[] targets)
		{
			Formula rem;
			Formula cont; // 容量

			var x = targets[0].RepresentativeVariable();
			var f = targets[0].PrimitivePolynomial(x, out cont);
			var fd = (new Differentiate(f, x)).SimplifyDifferentiate();
			if (fd == 0) return f;

			var g = Polynomial.GCD(f, fd);
			var v = f.Divide(g, x, out rem, false);
			FormulaAssertionException.Assert(rem == 0);
			var w = fd.Divide(g, x, out rem, false);
			FormulaAssertionException.Assert(rem == 0);

			List<Formula> sqfr = new List<Formula>();
			int i = 0;
			while (v != 1)
			{
				var vd = (new Differentiate(v, x)).SimplifyDifferentiate();
				var wvd = (w - vd).CollectAbout(x);
				if (wvd == 0)
				{
					sqfr.Add(v);
					break;
				}

				sqfr.Add(Polynomial.GCD(v, wvd));

				v = v.Divide(sqfr[i], x, out rem, false);
				FormulaAssertionException.Assert(rem == 0);
				w = wvd.Divide(sqfr[i], x, out rem, false);
				FormulaAssertionException.Assert(rem == 0);
				i++;
			}
			return GetSquareFreeDecomposed(f, cont, sqfr);
		}

		/// <summary>
		/// 因子リストから、無平方分解後の数式を構成して取得します。
		/// </summary>
		/// <param name="f">元の数式。</param>
		/// <param name="cont">元の数式の容量。</param>
		/// <param name="sqfr">要素iを、i+1次の因子とした因子リスト。</param>
		/// <returns>無平方分解された数式。</returns>
		private Formula GetSquareFreeDecomposed(Formula f, Formula cont, List<Formula> sqfr)
		{
			List<Formula> sqfrList = new List<Formula>();
			if (cont != 1) sqfrList.Add(cont);

			for (int i = 0; i < sqfr.Count; i++)
			{
				if (sqfr[i] == 1) continue;

				if (i == 0) 
					sqfrList.Add(sqfr[i]);
				else
					sqfrList.Add(sqfr[i] ^ (i + 1));
			}

			if (sqfrList.Count == 0) return f;
			if (sqfrList.Count == 1) return sqfrList[0];
			return new Product(true, sqfrList.ToArray());
		}

	}
}
