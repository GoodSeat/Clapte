using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Formulas.Functions
{
	/// <summary>
	/// sqrt関数を表します。
	/// </summary>
	[Serializable()]
	public class Sqrt : Root
	{
		/// <summary>
		/// sqrt関数を初期化します。
		/// </summary>
		public Sqrt() : base() { }

		/// <summary>
		/// sqrt関数を初期化します。
		/// </summary>
		/// <param name="f">平方根対象の数式</param>
		public Sqrt(Formula f) : base(f, 2) { }

		public override int MinimumArgumentQty { get { return 1; } }

		public override string GetInformation(out List<string> args)
		{
			args = new List<string>(); args.Add("数値");
			return "数式の平方根を返します。";
		}

		public override Type GetEqualBaseType() { return typeof(Root); }

		public override Formula this[int i]
		{
			get
			{
				if (i == 1) return 2;
				else return base[i];
			}
			set 
            {
                if (i == 0) base[i] = value;
            }
		}
	}
}
