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
	/// 要素が可変数の演算を表します。
	/// </summary>
	[Serializable()]
	public abstract class OperatorMultiple : Operator
	{
		List<Formula> _formulas; // 構成数式リスト

		/// <summary>
		/// 可変数演算を初期化します。
		/// </summary>
		/// <param name="formulas">可変数の構成数式。</param>
		public OperatorMultiple(params Formula[] formulas) : this(false, formulas) { }

		/// <summary>
		/// 可変数演算を初期化します。
		/// </summary>
		/// <param name="integration">trueを指定すると、同種の演算を統合します。</param>
		/// <param name="formulas">可変数の構成数式。</param>
		public OperatorMultiple(bool integration, params Formula[] formulas)
		{
			Formulas = new List<Formula>();

			if (formulas.Length == 0)
				Formulas.Add(DefaultValue);
			else
				Formulas.AddRange(formulas);

			if (integration) Integrate();
		}

		/// <summary>
		/// 演算を構成する数式の一覧を取得します。
		/// </summary>
		internal protected List<Formula> Formulas
		{
			get { return _formulas; }
			private set { _formulas = value; }
		}

		/// <summary>
		/// 演算を構成する数式数を取得します。
		/// </summary>
		public int Count { get { return Formulas.Count; } }

		/// <summary>
		/// 演算中のデフォルト値を取得します。この値は、全ての構成数式が削除された場合に、代替の数式として使用されます。
		/// </summary>
		public abstract Formula DefaultValue { get; }

		/// <summary>
		/// 構成要素内の同要素を、親の子要素として統合します。
		/// </summary>
		/// <returns>統合操作があったか否か。</returns>
		private bool Integrate()
		{
			bool result = false;

			for (int i = 0; i < Formulas.Count; i++)
			{
				if (Formulas[i].GetType() == GetType())
				{
					result = true;
					int insertIndex = 0;
					foreach (Formula m in Formulas[i])
						Formulas.Insert(i + insertIndex++, m);
					Formulas.RemoveAt(i + insertIndex);
					i--;
				}
			}
			return result;
		}

		/// <summary>
		/// 構成要素内の同要素を、親の子要素として統合した演算を初期化して取得します。
		/// </summary>
		/// <returns>統合操作があった場合には、初期化された演算数式。統合操作がなかった場合には、null。</returns>
		public OperatorMultiple CreateIntegrated()
		{
			var list = new List<Formula>(EnumerateIntegrateChildren());
			if (list.Count != Count) return CreateOperator(list.ToArray()) as OperatorMultiple;

			return null;
		}

		/// <summary>
		/// 構成要素内の同じ型の要素を分解した上で、順次返す反復子を取得します。
		/// </summary>
		/// <returns>子要素の反復子。ただし、同じ型の要素はその内部の子要素を返す。</returns>
		private IEnumerable<Formula> EnumerateIntegrateChildren()
		{
			for (int i = 0; i < Formulas.Count; i++)
			{
				if (Formulas[i].GetType() == GetType())
				{
					var same = Formulas[i] as OperatorMultiple;
					foreach (var child in same.EnumerateIntegrateChildren())
						yield return child;
				}
				else
					yield return Formulas[i];
			}
		}

		/// <summary>
		/// 構成数式を設定もしくは取得します。nullの設定により要素を削除します。
		/// </summary>
		/// <param name="i">設定/取得先のインデックス番号。</param>
		/// <returns>指定インデックスに対応する数式。</returns>
		public override Formula this[int i]
		{
			get
			{
				if (i < Formulas.Count) return Formulas[i];
				else return null;
			}
			set
			{
				base[i] = value;

				if (Formulas.Count <= i) Formulas.Add(DefaultValue);
				Formulas[i] = value;				

				for (int k = 0; k < Formulas.Count; k++)
				{
					if (Formulas[k] == null)
						Formulas.RemoveAt(k--);
				}
				if (Formulas.Count == 0) Formulas.Add(DefaultValue);
			}
		}

		/// <summary>
		/// 数式を置き換えます。
		/// </summary>
		/// <param name="oldValue">置換対象とする数式。</param>
		/// <param name="newValue">置換後の数式。</param>
		/// <returns>置換操作後の数式。</returns>
		protected override Formula OnSubstitute(Formula oldValue, Formula newValue)
		{
			if ((Law & OperatorLaw.Commutative) != OperatorLaw.Commutative) return base.OnSubstitute(oldValue, newValue);
			if (oldValue.GetType() != this.GetType()) return base.OnSubstitute(oldValue, newValue);
		
			OperatorMultiple old = oldValue as OperatorMultiple; // 置き換えられる数式

			bool needReplace = true;
			List<int> removeID = new List<int>();
			for (int i = 0; i < old.Formulas.Count; i++)
			{
				for (int k = 0; k < Formulas.Count; k++)
				{
					if (removeID.Contains(k)) continue;

					if (this[k] == old[i]) // oldのi番目要素がある
					{
						removeID.Add(k);
						break;
					}
					else if (k == Formulas.Count - 1) // oldのi番目要素に相当する要素がない → これは置き換えられない
					{
						needReplace = false;
						break;
					}
				}
				if (!needReplace) break;
			}

			if (needReplace)
			{
				// 全ての項について該当が見つかったら、それらを取り除いて、新しい数式を掛け合わせて返す
				removeID.Sort();
				for (int i = removeID.Count - 1; i >= 0; i--) Formulas.RemoveAt(i);

				if (newValue.GetType() == this.GetType()) Formulas.AddRange(newValue);
				else Formulas.Add(newValue);

				if (Formulas.Count == 1) return this[0];
			}

			return base.OnSubstitute(oldValue, newValue);
		}

		protected override void OnSort() 
		{
			if ((Law & OperatorLaw.Commutative) == OperatorLaw.Commutative) 
				Utilities.Sort.StableSort<Formula>(Formulas);
		}

		/// <summary>
		/// 指定したComparisonデリゲートを使用して、構成項を並び替えます。
		/// </summary>
		/// <param name="comparison">並び替え順を規定するComparison。</param>
		public void Sort(Comparison<Formula> comparison) { Utilities.Sort.StableSort<Formula>(Formulas, comparison); }

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

			if (object.ReferenceEquals(sender, this)) yield return MultipleOperatorIntegrateRule.Entity;

			// 分配則
			if (object.ReferenceEquals(sender, this) && deformToken.Has<ExpandToken>() && Satisfy(OperatorLaw.Distributive))
				yield return new DistributivePropertyRule(GetType(), DistributeTarget());
		}

	}
}
