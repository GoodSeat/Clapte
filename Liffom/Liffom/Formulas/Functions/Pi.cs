using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Comparers;

namespace GoodSeat.Liffom.Formulas.Functions
{
	/// <summary>
	/// Pi関数（総積 Π）を表します。
	/// </summary>
	[Serializable()]
	public class Pi : Function
	{
		/// <summary>
		/// 総積関数を初期化します。
		/// </summary>
		public Pi() : base() { }

		/// <summary>
		/// 総積関数を初期化します。
		/// </summary>
		/// <param name="formula">総和対象の数式</param>
		public Pi(Formula formula) : base(formula) { }

		/// <summary>
		/// 総積関数を初期化します。
		/// </summary>
		/// <param name="n">変化対象の変数</param>
		/// <param name="start">変数の開始値</param>
		/// <param name="end">変数の終了値</param>
		/// <param name="formula">総和対象の数式</param>
		public Pi(Variable n, Formula start, Formula end, Formula formula)
			: base(formula, new Equal(n, start), end)
		{ }

		/// <summary>
		/// 引数を指定して、関数を生成します。
		/// </summary>
		/// <param name="args">初期化に用いるか変数の数式。</param>
		/// <returns>初期化された関数。</returns>
		public override Function CreateFunction(params Formula[] args)
		{
			var result = new Pi();
			result.Argument = new Argument(args);
			return result;
		}

		public override Formula CalculateFunction()
		{
			var token = new DeformToken(Formula.CalculateToken, Formula.SimplifyToken);
			Equal start = Argument[1].DeformFormula(token) as Equal;
			Numeric end = Argument[2].DeformFormula(token) as Numeric;
			NotEqual none = null;
			if (Argument.Count > 3) none = (Argument[3].DeformFormula(token)) as NotEqual;

			if (start != null && start.RightHandSide is Numeric && ((Numeric)start.RightHandSide).IsInteger // 初期条件が等式、左辺が整数である
				&& end != null && end.IsInteger) // 終了条件が整数である
			{
				List<Formula> multList = new List<Formula>();
				List<Formula> notList = new List<Formula>();

				if (Argument.Count > 3 && none != null && none.LeftHandSide == start.LeftHandSide) // 禁止条件がある
				{
					if (none.RightHandSide is Argument)
					{
						foreach (Formula fnot in none.RightHandSide)
						{
							if (fnot.Calculate() is Numeric)
								notList.Add(fnot);
							else // 禁止条件に変数が混在するため、計算できない
								return this;
						}
					}
					else
					{
						if (none.RightHandSide.Calculate() is Numeric)
							notList.Add(none.RightHandSide);
						else // 禁止条件に変数が混在するため、計算できない
							return this;
					}
				}

				for (int i = (int)((Numeric)start.RightHandSide).Data; i <= (int)end.Data; i++)
				{
					if (notList.Contains(i)) continue; // 禁止条件にある
					else
						multList.Add(Argument[0].Substituted(start.LeftHandSide, (double)i));
				}

				if (multList.Count == 0)
					return 1;
				else
				{
					Product m = new Product(multList.ToArray());
					return m.Calculate();
				}
			}
			else
				return this;
		}

		public override int MinimumArgumentQty
		{
			get { return 3; }
		}

		public override int MaximumArgumentQty
		{
			get
			{
				return 4; // 禁止条件
			}
		}

		public override string GetInformation(out List<string> args)
		{
			args = new List<string>(); args.Add("数式"); args.Add("変数 = 初期値"); args.Add("終了値"); args.Add("変数 ≠ 除外値列挙（任意）");
			return "総積を返します。";
		}
	}
}
