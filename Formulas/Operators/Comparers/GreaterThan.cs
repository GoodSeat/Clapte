using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Deforms;
using Liffom.Formulas.Units;

namespace Liffom.Formulas.Operators.Comparers
{
	/// <summary>
	/// 不等号(より大)による数式の比較を初期化します。
	/// </summary>
	[Serializable()]
	public class GreaterThan : Comparer
	{
		bool _containEqual = false;

		/// <summary>
		/// 包含比較か否かを設定もしくは取得します。
		/// </summary>
		public bool ContainEqual { get { return _containEqual; } set { _containEqual = value; } }

		/// <summary>
		/// 不等号(より大)による数式の比較を初期化します。
		/// </summary>
		/// <param name="lhs">左辺。</param>
		/// <param name="rhs">右辺。</param>
		public GreaterThan(Formula lhs, Formula rhs)
			: base(lhs, rhs)
		{ }

		/// <summary>
		/// 不等号(より大)による数式の比較を初期化します。
		/// </summary>
		/// <param name="lhs">左辺。</param>
		/// <param name="rhs">右辺。</param>
		/// <param name="containEqual">包含比較か否か。</param>
		public GreaterThan(Formula lhs, Formula rhs, bool containEqual)
			: base(lhs, rhs)
		{ ContainEqual = containEqual; }

		/// <summary>
		/// 引数を指定して、演算を生成します。
		/// </summary>
		/// <param name="args">初期化に用いるか変数の数式。</param>
		public override Operator CreateOperator(params Formula[] args)
		{
			return new GreaterThan(args[0], args[1], ContainEqual);
		}

		public override string GetText()
		{
			string mark = ContainEqual ? "≧" : ">";
			return LeftHandSide.ToString() + mark + RightHandSide.ToString();
		}

		public override Judge GetJudge()
		{
			var token = new DeformToken(Formula.SimplifyToken, Formula.CalculateToken, Formula.NumerateToken);
			Formula f = (LeftHandSide - RightHandSide).DeformFormula(token);
			f = f.ClearUnit();

			if (f is Numeric)
			{
				if (ContainEqual)
				{
					if ((f as Numeric).Data >= 0) return Judge.True;
					else return Judge.False;
				}
				else
				{
					if ((f as Numeric).Data > 0) return Judge.True;
					else return Judge.False;
				}
			}
			else
				return Judge.None;
		}
	}
}
