using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Liffom.Formulas.Operators.Booleans
{
	/// <summary>
	/// 論理演算を表します。
	/// </summary>
	[Serializable()]
	public abstract class Boolean : Comparers.Comparer
	{
		/// <summary>
		/// 論理演算を初期化します。
		/// </summary>
		/// <param name="lhs">左辺</param>
		/// <param name="rhs">右辺</param>
		public Boolean(Formula lhs, Formula rhs)
			: base(lhs, rhs)
		{ }
		
		/// <summary>
		/// 演算が満たす法則を取得します。
		/// </summary>
		public override OperatorLaw Law 
		{ 
			get
			{ 
				return OperatorLaw.Distributive | OperatorLaw.Associative | OperatorLaw.Commutative;
			}
		}

		/// <summary>
		/// 演算の評価優先度を表す数値を取得します。
		/// 原則として、OperatorPriorityの値を返してください。
		/// </summary>
		public override int EvaluatePriority { get { return (int)OperatorPriority.Boolean; } }

	}
}
