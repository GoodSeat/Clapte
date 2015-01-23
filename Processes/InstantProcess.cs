using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Processes
{
	/// <summary>
	/// 一時的な使用を目的とした数式処理クラスを表します。
	/// </summary>
	public class InstantProcess : Process
	{
		/// <summary>
		/// 一つの数式を受け取り、処理結果の数式を返すメソッドを表します。
		/// </summary>
		/// <param name="target">処理対象の数式</param>
		/// <returns>処理結果の数式</returns>
		public delegate Formula InstantProcessHandler(Formula target);

		InstantProcessHandler _handler;

		/// <summary>
		/// 一時的な使用を目的とした数式処理を初期化します。
		/// </summary>
		/// <param name="handler">処理メソッド</param>
		public InstantProcess(InstantProcessHandler handler) { _handler = handler; }

		public override int TargetFormulasMinQty
		{
			get { return 1; }
		}

		protected override Formula OnDo(object userState, params Formula[] targets)
		{
			return _handler(targets[0]);
		}

		public override bool IsSupportMultipleConcurrentInvocations
		{
			get { return true; }
		}
	}
}
