using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Deforms
{
	/// <summary>
	/// ルールの適用履歴の地点記録を表します。
	/// </summary>
	public class DeformHistoryNode
	{
		bool _changedInChildHistory = false;
		string _cacheFormulaUniqueText = null;
		string _cacheFormulaText = null;

		/// <summary>
		/// ルールの適用履歴の地点記録を初期化します。
		/// </summary>
		/// <param name="target">記録する数式</param>
		public DeformHistoryNode(Formula target) : this(target, null) { }

		/// <summary>
		/// ルールの適用履歴の地点記録を初期化します。
		/// </summary>
		/// <param name="target">記録する数式</param>
		/// <param name="appliedRule">直前に適用されたルール</param>
		public DeformHistoryNode(Formula target, Rule appliedRule)
		{
			Formula = target;
			AppliedRule = appliedRule;
			ChildrenHistories = new List<DeformHistory>();

			SetPreventTargetRules();
		}


		/// <summary>
		/// 所属するルール適用系統を取得します。
		/// </summary>
		public DeformHistory BelongHistory { get; internal set; }

		/// <summary>
		/// この数式の子系統のルール適用履歴リストを取得します。
		/// </summary>
		public List<DeformHistory> ChildrenHistories { get; private set; }

		/// <summary>
		/// 対象とする数式を取得します。
		/// </summary>
		public Formula Formula { get; private set; }

		/// <summary>
		/// 対象とする数式を表すテキストを取得します。
		/// </summary>
		public string FormulaText 
		{
			get
			{
				if (_cacheFormulaText == null) _cacheFormulaText = Formula.ToString();
				return _cacheFormulaText;
			}
		}

		/// <summary>
		/// 対象とする数式を一意に区別するテキストを取得します。
		/// </summary>
		public string FormulaUniqueText 
		{
			get
			{
				if (_cacheFormulaUniqueText == null) _cacheFormulaUniqueText = Formula.GetUniqueText();
				return _cacheFormulaUniqueText;
			}
		}

		/// <summary>
		/// この数式に対して最後に適用されたルールを取得します。
		/// </summary>
		public Rule AppliedRule { get; private set; }

		/// <summary>
		/// この数式に対して最後に実行されたルールにより、今後同系統の数式変形において適用が抑制されるルールリストを取得します。
		/// </summary>
		public List<Rule> PreventTargetRules { get; private set; }

		/// <summary>
		/// この履歴地点の子系統において、数式変化があったか否かを取得します。
		/// </summary>
		public bool ChangedInChildHistory 
		{ 
			get { return _changedInChildHistory; }
			private set { _changedInChildHistory = value; }
		}

		/// <summary>
		/// この履歴地点の子系統において、数式変化があったことを通知します。
		/// </summary>
		/// <param name="appliedRule">適用されたルール</param>
		internal void NotifyChangeInChildHistory(Rule appliedRule)
		{
			if (appliedRule == null) return;
			ChangedInChildHistory = true;

			var removeList = new List<Rule>();
			foreach (var rule in PreventTargetRules)
			{
				if (rule.CompareTo(appliedRule) > 0) removeList.Add(rule); // 適用のあったルールより整理傾向のもののみ抑止を解除
			}
			foreach (var rule in removeList) PreventTargetRules.Remove(rule);

			if (BelongHistory.ParentNode != null) BelongHistory.ParentNode.NotifyChangeInChildHistory(appliedRule);
		}




		/// <summary>
		/// <see cref="AppliedRule"/>を基に、<see cref="PreventTargetRules"/>を初期化します。
		/// </summary>
		private void SetPreventTargetRules()
		{
			PreventTargetRules = new List<Rule>();
			if (AppliedRule == null) return;

			foreach (var rule in AppliedRule.GetReverseRule())
			{
				if (rule.CompareTo(AppliedRule) < 0) // 逆変換ルールが適用ルールよりも優先
				{
					PreventTargetRules.Add(rule);
				}
			}
		}

		/// <summary>
		/// この履歴地点情報に、子系統を追加します。
		/// </summary>
		/// <param name="childHistory"></param>
		public void AddChildHistory(DeformHistory childHistory) { ChildrenHistories.Add(childHistory); }

		public override string ToString()
		{
			string result = FormulaText;
			if (AppliedRule != null) result += "::" + AppliedRule.DistinguishedText;
			return result;
		}

	}
}
