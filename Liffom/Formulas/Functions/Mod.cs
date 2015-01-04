using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Extensions;
using Liffom.Formulas.Operators;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Liffom.Formulas.Functions
{
	/// <summary>
	/// Mod関数（剰余）を表します。
	/// </summary>
	[Serializable()]
	public class Mod : Function
	{
		/// <summary>
		/// 剰余関数を初期化します。
		/// </summary>
		public Mod() : base() { }

		/// <summary>
		/// 剰余関数を初期化します。
		/// </summary>
		/// <param name="molecular">被除数。</param>
		/// <param name="denominator">除数。</param>
		public Mod(Formula molecular, Formula denominator) : base(molecular, denominator) { }

		/// <summary>
		/// 引数を指定して、関数を生成します。
		/// </summary>
		/// <param name="args">初期化に用いるか変数の数式。</param>
		/// <returns>初期化された関数。</returns>
		public override Function CreateFunction(params Formula[] args)
		{
			return new Mod(args[0], args[1]);
		}

		public override Formula CalculateFunction()
		{
			if (Argument[0] != null && Argument[1] != null)
			{
				Formula result;
				Argument[0].Divide(Argument[1], out result);
				return result;
			}
			else
				return this;
		}

		public override int MinimumArgumentQty
		{
			get { return 2; }
		}

		public override string GetInformation(out List<string> args)
		{
			args = new List<string>(); args.Add("分子"); args.Add("分母");
			return "指定した分子分母による除算の剰余を取得します。";
		}
	}
}
