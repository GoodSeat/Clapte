using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Deforms.Rules;

namespace GoodSeat.Liffom.Deforms
{
	/// <summary>
	/// ルールの適用履歴情報の一系統を表します。
	/// </summary>
	public class DeformHistory : IEnumerable<DeformHistoryNode>
	{
		/// <summary>
		/// ルールの適用履歴情報を初期化します。
		/// </summary>
		/// <param name="initialTarget">対象数式の初期状態</param>
		public DeformHistory(Formula initialTarget) : this(new DeformHistoryNode(initialTarget), null) { } 

		/// <summary>
		/// ルールの適用履歴情報を初期化します。
		/// </summary>
		/// <param name="initialNode">ルール適用対象の数式の初期状態を表す履歴地点</param>
		public DeformHistory(DeformHistoryNode initialNode) : this(initialNode, null) { }

		/// <summary>
		/// ルールの適用履歴情報を初期化します。
		/// </summary>
		/// <param name="initialNode">ルール適用対象の数式の初期状態を表す履歴地点</param>
		/// <param name="parentNode">この系統の親となる履歴地点</param>
		public DeformHistory(DeformHistoryNode initialNode, DeformHistoryNode parentNode)
		{
			if (initialNode == null) throw new ArgumentNullException();

			AppliedNodes = new List<DeformHistoryNode>();
			ParentNode = parentNode;
			Add(initialNode);
		}

		/// <summary>
		/// この履歴系統の履歴リストを設定もしくは取得します。
		/// </summary>
		private List<DeformHistoryNode> AppliedNodes { get; set; }

		/// <summary>
		/// この履歴系統の親となる履歴地点を取得します。
		/// </summary>
		public DeformHistoryNode ParentNode { get; private set; }

		/// <summary>
		/// 指定した履歴地点情報をリストに追加します。
		/// </summary>
		/// <param name="node"></param>
		public void Add(DeformHistoryNode node)
		{
			AppliedNodes.Add(node);
			node.BelongHistory = this;

			if (node.AppliedRule != null && ParentNode != null)
				ParentNode.NotifyChangeInChildHistory(node.AppliedRule);
		}

		/// <summary>
		/// この履歴系統の最後の履歴地点情報を取得します。
		/// </summary>
		public DeformHistoryNode CurrentNode { get { return AppliedNodes.Last(); } }

		/// <summary>
		/// 最上位系統を1とした時の、この履歴系統の世代を取得します。
		/// </summary>
		public int Era
		{
			get
			{
				if (ParentNode != null) return ParentNode.BelongHistory.Era + 1;
				return 1;
			}
		}

		/// <summary>
		/// このルール適用履歴上にて、<paramref name="rule"/>が適用抑止されるルールか否かを取得します。
		/// </summary>
		/// <param name="rule">判定対象のルール</param>
		/// <returns>ルールの適用が抑止されるか否か</returns>
		public bool IsPreventedRule(Rule rule)
		{
			var preventRules = new List<Rule>(GetAllPreventTargetRules()); // 抑止ルールリスト
			if (preventRules.Contains(rule)) return true;

			// 後の実行を許されない逆変換ルールが既に適用されていたら、抑止される
			foreach (var reverse in rule.GetReverseRule())
			{
				if (rule.CompareTo(reverse) >= 0) continue; // 試行ルールが、この逆変換より後の実行を許可されている
				foreach (var applied in GetAppliedRules())
				{
					if (applied == reverse) return true;
				}
			}
			return false;
		}

		/// <summary>
		/// この履歴系統と、その直属の親でこれまでに適用されたすべてのルールを返す反復子を取得します。
		/// </summary>
		/// <returns>これまでに適用されたルールを返す反復子。</returns>
		public IEnumerable<Rule> GetAppliedRules()
		{
			// この履歴系統における適用済みルールを参照
			foreach (var node in AppliedNodes)
			{
				if (node.AppliedRule != null) yield return node.AppliedRule;
			}

			// 直属の親系統の適用済みルールを参照
			if (ParentNode != null && ParentNode.AppliedRule != null) yield return ParentNode.AppliedRule;
		}

		/// <summary>
		/// この履歴系統と、その親の履歴系統でこれまでに適用されたすべてのルールを返す反復子を取得します。
		/// </summary>
		/// <returns>これまでに適用されたルールを返す反復子。</returns>
		public IEnumerable<Rule> GetAllAppliedRules()
		{
			// この履歴系統における適用済みルールを参照
			foreach (var node in AppliedNodes)
			{
				if (node.AppliedRule != null) yield return node.AppliedRule;
			}

			// すべての親系統の適用済みルールを参照
			if (ParentNode != null)
				foreach (var applied in ParentNode.BelongHistory.GetAllAppliedRules()) yield return applied;
		}

		/// <summary>
		/// この履歴系統において、今後適用を抑止するすべてのルールを返す反復子を取得します。
		/// </summary>
		/// <returns></returns>
		private IEnumerable<Rule> GetAllPreventTargetRules()
		{
			// この履歴系統で抑止されるルールを参照
			foreach (var node in AppliedNodes)
				foreach (var prevent in node.PreventTargetRules) 
					yield return prevent;

			// 直属の親の抑止ルールのみ禁止
			if (ParentNode != null) 
				foreach (var prevent in ParentNode.PreventTargetRules)
					yield return prevent;
		}

		/// <summary>
		/// 適用候補のルールと対象数式が、すでに適用済みの変形か否かを取得します。
		/// </summary>
		/// <param name="ApplyRule">適用候補のルール</param>
		/// <returns>同系統の履歴を参照して、同数式に対して同ルールの適用があった場合、trueを返します。</returns>
		public bool IsAlreadyAppliedRule(Rule ApplyRule)
		{
			for (int i = 1; i < AppliedNodes.Count; i++)
			{
				if (AppliedNodes[i].AppliedRule.DistinguishedText != ApplyRule.DistinguishedText) continue;
				if (AppliedNodes[i - 1].FormulaUniqueText == CurrentNode.FormulaUniqueText)
				{
#if DEBUG
					string msg = string.Format("【{0}に対して、{1}を再び適用しようとしました。（→{2}）】",
												CurrentNode.FormulaUniqueText, ApplyRule.DistinguishedText, AppliedNodes[i].FormulaUniqueText);
					Console.WriteLine(msg);
#endif
					return true;
				}
			}
			return false;
		}



		/// <summary>
		/// このルール適用履歴の概要を表す簡易テキストを取得します。
		/// </summary>
		/// <returns></returns>
		public string GetSimpleHistoryText()
		{
			string result = "";
			foreach (var node in this)
			{
				result += GetConsoleOutputIndent() + "------------------------------------\r\n";
				result += GetConsoleOutputIndent() + node.FormulaText;
				if (node.AppliedRule != null) result += string.Format(" :: {0}", node.AppliedRule.DistinguishedText);
				result += "\r\n";

				foreach (var history in node.ChildrenHistories)
					result += history.GetSimpleHistoryText() + "\r\n";
			}
			return result.TrimEnd('\r', '\n');
		}

		private string GetConsoleOutputIndent()
		{
			string indent = "  ";
			for (int i = 1; i < Era; i++) indent += "｜";
			return indent;
		}

		public override string ToString()
		{
			string result = "";
			foreach (DeformHistoryNode node in this)
			{
				result += node.FormulaText + "→";
			}
			return result.TrimEnd('→');
		}

		#region IEnumerable<RuleApplyHistoryNode> メンバー

		public IEnumerator<DeformHistoryNode> GetEnumerator()
		{
			foreach (var node in AppliedNodes) yield return node;
		}

		#endregion

		#region IEnumerable メンバー

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			foreach (var node in AppliedNodes) yield return node;
		}

		#endregion
	}
}
