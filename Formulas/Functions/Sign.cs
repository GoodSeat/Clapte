using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas.Operators;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace GoodSeat.Liffom.Formulas.Functions
{
	/// <summary>
	/// sign関数を表します。
	/// </summary>
	[Serializable()]
	public class Sign : Function
	{
		/// <summary>
		/// sign関数を初期化します。
		/// </summary>
		public Sign() : base() { }

		/// <summary>
		/// sign関数を初期化します。
		/// </summary>
		/// <param name="f">対象の数式。</param>
		public Sign(Formula f) : base(f) { }

		/// <summary>
		/// 引数を指定して、関数を生成します。
		/// </summary>
		/// <param name="args">初期化に用いるか変数の数式。</param>
		/// <returns>初期化された関数。</returns>
		public override Function CreateFunction(params Formula[] args)
		{
			return new Sign(args[0]);
		}

		public override Formula CalculateFunction()
		{
			if (Argument[0] is Numeric)
				return new Numeric(Math.Sign(Argument[0]));
			else
				return this;
		}

		public override int MinimumArgumentQty
		{
			get { return 1; }
		}

		public override string GetInformation(out List<string> args)
		{
			args = new List<string>(); args.Add("数値");
			return "数値の正負を返します。正の数の時は1、0の時は0、負の数の時は-1を返します。";
		}
	}
}

