using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formats;
using GoodSeat.Liffom.Formulas.Operators.Rules;

namespace GoodSeat.Liffom.Formulas.Operators
{
	/// <summary>
	/// 演算を表します。
	/// </summary>
	[Serializable()]
	public abstract partial class Operator : Formula
	{
		/// <summary>
		/// 演算の優先順を表します。値が小さいほど優先的に評価されます。
		/// </summary>
		public enum OperatorPriority
		{
			/// <summary>
			/// 累乗(10)
			/// </summary>
			Power = 10,
			/// <summary>
			/// 乗算(20)
			/// </summary>
			Product = 20,
			/// <summary>
			/// 加算(30)
			/// </summary>
			Sum = 30,
			/// <summary>
			/// 比較(40)
			/// </summary>
			Compare = 40,
			/// <summary>
			/// 論理(50)
			/// </summary>
			Boolean = 50,
			/// <summary>
			/// 引数(60)
			/// </summary>
			Argument = 60,
		}

		/// <summary>
		/// 演算が満たす法則を表します。
		/// </summary>
		[Flags()]
		public enum OperatorLaw
		{
			/// <summary>
			/// 満たす法則はありません。
			/// </summary>
			None = 0,
			/// <summary>
			/// 交換則を満たします。
			/// </summary>
			Commutative = 7,
			/// <summary>
			/// 結合則を満たします。
			/// </summary>
			Associative = 8,
			/// <summary>
			/// 分配則を満たします。
			/// 分配則を満たす演算では、<see cref="DistributeTarget"/>メソッドをオーバーライドする必要があります。
			/// </summary>
			Distributive = 16,
			/// <summary>
			/// 右分配則を満たします。
			/// 分配則を満たす演算では、<see cref="DistributeTarget"/>メソッドをオーバーライドする必要があります。
			/// </summary>
			DistributiveToRight = 17,
			/// <summary>
			/// 左分配則を満たします。
			/// 分配則を満たす演算では、<see cref="DistributeTarget"/>メソッドをオーバーライドする必要があります。
			/// </summary>
			DistributiveToLeft = 18,
			/// <summary>
			/// 分配則(左、右ともに)を満たします。
			/// 分配則を満たす演算では、<see cref="DistributeTarget"/>メソッドをオーバーライドする必要があります。
			/// </summary>
			DistributiveBoth = 19,
		}


		/// <summary>
		/// 演算を初期化します。
		/// </summary>
		/// <param name="formulas">演算を構成する可変数の数式。</param>
		public Operator(params Formula[] formulas) { }

		/// <summary>
		/// 分配則の対象となる演算の型を取得します。
		/// </summary>
		/// <example>積算(Product)は和算(Sum)に対して分配則が成り立つため、typeof(Sum)を返します。</example>
		public virtual Type DistributeTarget() 
		{
			throw new NotImplementedException("分配則(OperatorLaw.Distributive)を満たす演算では、DistributeTargetメソッドを実装する必要があります。");
		}
				
		/// <summary>
		/// 演算が満たす法則を取得します。
		/// </summary>
		public abstract OperatorLaw Law { get; }

		/// <summary>
		/// 演算が指定の法則を満たすか否かを取得します。
		/// </summary>
		/// <param name="law">判定対象の法則。</param>
		/// <returns>法則を満たすか否か。</returns>
		public bool Satisfy(OperatorLaw law) { return (law & Law) == law; }

		/// <summary>
		/// 演算の評価優先度を表す数値を取得します。
		/// 原則として、OperatorPriorityの値を返してください。
		/// </summary>
		public abstract int EvaluatePriority { get; }

		/// <summary>
		/// 引数を指定して、演算を生成します。
		/// </summary>
		/// <param name="args">初期化に用いるか変数の数式。</param>
		/// <returns>初期化された演算。</returns>
		public abstract Operator CreateOperator(params Formula[] args);

		/// <summary>
		/// 可変数の数式を指定して、子数式を有する数式を初期化します。
		/// </summary>
		/// <param name="fs">数式を構成する可変数の子数式。</param>
		/// <returns>初期化された数式。</returns>
		public override Formula CreateFromChildren(params Formula[] fs) { return CreateOperator(fs); }


		/// <summary>
		/// 演算の中において、指定数式が直接の子数式となる場合に括弧が必要か否かを取得します。
		/// </summary>
		/// <param name="f">対象数式。</param>
		/// <returns>括弧の付加が必要か否か。</returns>
		public bool NeedBracket(Formula f)
		{
			if (!(f is Operator)) return false;

			int other = (f as Operator).EvaluatePriority;
			int one = EvaluatePriority;
			return one <= other;
		}

		/// <summary>
		/// ルールパターン数式に対し、指定数式が合致するか否かを返します。
		/// </summary>
		/// <param name="f">判定対象の数式。</param>
		/// <returns>ルールパターン数式に一致するか否か。</returns>
		protected override bool OnCheckPatternMatch(Formula f)
		{
			RulePatternCheck checker = new RulePatternCheck(this, f);

			if (Satisfy(OperatorLaw.Commutative))
				return checker.CheckPatternMatchCombination();
			else
				return checker.CheckPatternMatchPermutation();
		}

		/// <summary>
		/// 数式の一意性評価に用いる文字列で、同じ型の数式同士の一意性を表す文字列を取得します。
		/// </summary>
		/// <returns>数式を一意に区別する文字列。</returns>
		protected override string OnGetUniqueText()
		{
			var children = new List<string>();
			foreach (var child in this) children.Add(child.GetUniqueText());
			return string.Join(",", children);
		}


	}

}
