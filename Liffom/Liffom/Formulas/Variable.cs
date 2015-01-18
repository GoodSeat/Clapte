using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Constants;

namespace GoodSeat.Liffom.Formulas
{
	/// <summary>
	/// 変数を表します。
	/// </summary>
	/// <remarks>TODO:現在、変数を数値で置き換えるルールなどが存在しない。Replaceメソッドでとりあえず代替してもらうこととする。</remarks>
	[Serializable()]
	public class Variable : AtomicFormula
	{
		string _information;

		/// <summary>
		/// 変数を作成します。
		/// </summary>
		/// <param name="mark">変数記号</param>
		public Variable(string mark) { Mark = mark; }

		/// <summary>
		/// 変数を表す文字列を取得します。
		/// </summary>
		public string Mark { get; private set; }

		/// <summary>
		/// 変数の説明を設定もしくは取得します。
		/// </summary>
		public virtual string Information
		{
			get
			{
				if (_information == null) _information = "";
				return _information;
			}
			set { _information = value; }
		}

		public override string GetText() { return Mark; }

		/// <summary>
		/// 数式の一意性評価に用いる文字列で、同じ型の数式同士の一意性を表す文字列を取得します。
		/// </summary>
		/// <returns>数式を一意に区別する文字列。</returns>
		protected override string OnGetUniqueText() { return Mark; }

		protected override CompareResult IsLargerThan(Formula other)
		{
			if (other is Variable)
			{
				int compare = ToString().CompareTo(other.ToString());

				if (compare > 0) return CompareResult.Larger;
				else if (compare < 0) return CompareResult.Smaller;
				else return CompareResult.Same;
			}
			else
				return base.IsLargerThan(other);
		}
	}
}
