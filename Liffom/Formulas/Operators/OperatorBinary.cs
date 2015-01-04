using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Deforms;
using Liffom.Deforms.Rules;
using Liffom.Formats;
using Liffom.Formulas.Operators.Rules;

namespace Liffom.Formulas.Operators
{
	/// <summary>
	/// 2要素から成る演算を表します。
	/// </summary>
	[Serializable()]
	public abstract class OperatorBinary : Operator
	{
		/// <summary>
		/// 前方の数式を設定もしくは取得します。
		/// </summary>
		public Formula Forword { get; set; }

		/// <summary>
		/// 後方の数式を設定もしくは取得します。
		/// </summary>
		public Formula Backword { get; set; }

		/// <summary>
		/// 2要素から成る演算を初期化します。
		/// </summary>
		/// <param name="forword">前方の構成数式。</param>
		/// <param name="backword">後方の構成数式。</param>
		public OperatorBinary(Formula forword, Formula backword) 
			: base(forword, backword)
		{
			Forword = forword;
			Backword = backword;
		}

		/// <summary>
		/// 構成数式を設定もしくは取得します。nullの設定により要素を削除します。
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public override Formula this[int i]
		{
			get
			{
				if (i == 0) return Forword;
				if (i == 1) return Backword;
				return null;
			}
			set
			{
				base[i] = value;
				if (i == 0) Forword = value;
				if (i == 1) Backword = value;
			}
		}

		protected override void OnSort() 
		{
			if (Satisfy(OperatorLaw.Commutative))
			{
				if (Backword.CompareTo(Forword) < 0)
				{
					var tmp = Forword;
					Forword = Backword;
					Backword = Forword;
				}
			}
		}

		/// <summary>
		/// 指定処理に関連するルールをすべて返す反復子を取得します。
		/// </summary>
		/// <param name="deformToken">変形識別トークン。</param>
		/// <param name="sender">ルール適用対象となる最上位親数式。</param>
		/// <param name="history">変形履歴情報。</param>
		/// <returns>変形に関連するルールを返す反復子。</returns>
		public override IEnumerable<Rule> GetRelatedRulesOf(DeformToken deformToken, Formula sender, DeformHistory history) 
		{
			foreach (var rule in base.GetRelatedRulesOf(deformToken, sender, history)) yield return rule;

			// 分配則
			if (object.ReferenceEquals(sender, this) && deformToken.Has<ExpandToken>() && Satisfy(OperatorLaw.Distributive))
				yield return new BinaryDistributivePropertyRule(GetType(), DistributeTarget());
		}

	}
}
