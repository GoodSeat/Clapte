using GoodSeat.Liffom.Deforms;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Liffom.Formulas.Operators.Booleans
{
	/// <summary>
	/// 論理和を表します。
	/// </summary>
	[Serializable()]
	public class Or : Boolean
	{
		/// <summary>
		/// 論理和を初期化します。
		/// </summary>
		/// <param name="lhs">左辺</param>
		/// <param name="rhs">右辺</param>
		public Or(Formula lhs, Formula rhs)
			: base(lhs, rhs)
		{ }

		/// <summary>
		/// 分配則の対象となる演算の型を取得します。
		/// </summary>
		/// <example>積算(Product)は和算(Sum)に対して分配則が成り立つため、typeof(Sum)を返します。</example>
		public override Type DistributeTarget() { return typeof(And); }

		/// <summary>
		/// 引数を指定して、演算を生成します。
		/// </summary>
		/// <param name="args">初期化に用いるか変数の数式。</param>
		public override Operator CreateOperator(params Formula[] args)
		{
			return new Or(args[0], args[1]);
		}

		public override string GetText()
		{
			return LeftHandSide.ToString() + "|" + RightHandSide.ToString();
		}
		
		public override Judge GetJudge(DeformToken token)
		{
			Formula f = (LeftHandSide + RightHandSide).DeformFormula(token);
			if (f is Numeric)
			{
				if (f != 0) return Judge.True;
				else return Judge.False;
			}
			else
				return Judge.None;
		}


	}
}
