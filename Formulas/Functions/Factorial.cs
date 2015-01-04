using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Formulas.Functions
{
	/// <summary>
	/// 階乗を表します。
	/// </summary>
	[Serializable()]
	public class Factorial : Function
	{
		/// <summary>
		/// 階乗を返す関数を初期化します。
		/// </summary>
		public Factorial() : base(0, 1) { }

		/// <summary>
		/// 階乗を返す関数を初期化します。
		/// </summary>
		/// <param name="arg">対象の数式</param>
		public Factorial(Formula arg) : base(arg, 1) { }

		/// <summary>
		/// 階乗を返す関数を初期化します。
		/// </summary>
		/// <param name="arg">対象の数式</param>
		/// <param name="multiple">多重階乗数</param>
		public Factorial(Formula arg, Formula multiple) : base(arg, multiple) { }

		/// <summary>
		/// 引数を指定して、関数を生成します。
		/// </summary>
		/// <param name="args">初期化に用いるか変数の数式。</param>
		/// <returns>初期化された関数。</returns>
		public override Function CreateFunction(params Formula[] args)
		{
			if (args.Length < 2) return new Factorial(args[0]);
			else return new Factorial(args[0], args[1]);
		}

		public override int MinimumArgumentQty
		{
			get { return 1; }
		}

		public override int MaximumArgumentQty
		{
			get { return 2; }
		}

		public override string GetInformation(out List<string> args)
		{
			args = new List<string>();
			args.Add("対象の自然数");
			args.Add("多重階乗数。既定は1です。");
			return "引数の階乗を返します。";
		}

		public override Formula CalculateFunction()
		{
			Numeric n = Argument[0] as Numeric;
			if (n == null || !n.IsInteger || n.Data < 0) return this; // 対象が自然数でない
			Numeric m = Argument[1] as Numeric;
			if (m != null && (!m.IsInteger || m.Data < 0)) return this; // 階乗数が自然数でない

			if (n.Data == 0) return 1;

			double result = n;
			double multiple = 1;
			if (m != null) multiple = m;

			for (int i = (int)(n - m + 0.01); i > 0; i -= (int)(m + 0.01)) result *= i;

			return result;
		}
	}
}
