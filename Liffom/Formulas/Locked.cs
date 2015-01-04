using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Liffom.Formulas
{
	/// <summary>
	/// 編集対象とならない固定数式を初期化します。
	/// </summary>
	[Serializable()]
	public class Locked : Formula
	{
		/// <summary>
		/// 指定数式内の全てのロックを解除した数式を取得します。
		/// </summary>
		/// <param name="formula"></param>
		/// <returns></returns>
		public static Formula UnLocked(Formula formula)
		{
			Formula f = formula.Copy();
			return UnLockInner(f);
		}

		static Formula UnLockInner(Formula formula)
		{
			if (formula is Locked)
				return UnLockInner((formula as Locked).Formula);
			else
			{
				int i = 0;
				while (formula[i] != null)
				{
					formula[i] = UnLockInner(formula[i]);
					i++;
				}
				return formula;
			}
		}


		Formula _formula;

		/// <summary>
		/// 編集対象とならない固定数式を初期化します。
		/// </summary>
		/// <param name="f"></param>
		public Locked(Formula f)
		{
			_formula = f;
		}

		/// <summary>
		/// 保護対象とする数式を設定もしくは取得します。
		/// </summary>
		public Formula Formula
		{
			get { return _formula; }
			set { _formula = value; }
		}

		protected override Formula.CompareResult IsLargerThan(Formula other)
		{
			return (Formula.CompareResult)_formula.CompareTo(other);
		}

		public override string GetText()
		{
			return Formula.GetText();
		}

		/// <summary>
		/// 数式の一意性評価に用いる文字列で、同じ型の数式同士の一意性を表す文字列を取得します。
		/// </summary>
		/// <returns>数式を一意に区別する文字列。</returns>
		protected override string OnGetUniqueText() { return Formula.GetUniqueText(); }
	}
}
