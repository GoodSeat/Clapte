using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formats;

namespace GoodSeat.Liffom.Deforms
{
	/// <summary>
	/// 数式の変形処理を表します。
	/// </summary>
	public static class Deform
	{
		/// <summary>
		/// この変形処理に基づいて、対象の数式を変形して返します。
		/// </summary>
		/// <param name="token">変形識別トークン。</param>
		/// <param name="target">変形対象の数式</param>
		/// <returns>変形ルール適用の収束した数式</returns>
		public static Formula Apply(DeformToken token, Formula target) { return Apply(token, target, null, null); }

		/// <summary>
		/// この変形処理に基づいて、対象の数式を変形して返します。
		/// </summary>
		/// <param name="token">変形識別トークン。</param>
		/// <param name="target">変形対象の数式。</param>
		/// <param name="history">ルール適用履歴の登録先とする履歴系統。</param>
		/// <param name="isAbortApply">ルールの再帰適用を取りやめる数式判定デリゲート。</param>
		/// <returns>変形ルール適用の収束した数式、もしくはisAbourtApplyデリゲートに適合する数式。</returns>
		public static Formula Apply(DeformToken token, Formula target, DeformHistory history, Formula.IsTargetFormula isAbortApply)
		{
			Format format = null;
			if (target.HasFormat) format = target.Format;

			if (isAbortApply == null) isAbortApply = f => false;
			if (history == null) history = new DeformHistory(target);

			bool ruleApplied = true;
			while (ruleApplied)
			{
				ruleApplied = false;
				Formula.CheckCancelOperation(target); // ユーザーからの操作中止に対応

				foreach (var rule in GetApplyCandidateRules(token, target, history))
				{
					if (history.IsAlreadyAppliedRule(rule)) continue;

					if (rule.TryMatchRule(ref target))
					{
						ruleApplied = true;
						history.Add(new DeformHistoryNode(target, rule)); // 変形履歴地点を登録

						if (isAbortApply(target) || rule.DeformConclude)
						{
							target.LastDeformToken = token;
							return target;
						}
						break;
					}
				}
			} 

			if (history.Era == 1) token.Sort(target);

			target.Format = format;
			target.LastDeformToken = token;
			return target;
		}

		/// <summary>
		/// 変形対象の数式と変形履歴をもとに、適用候補となるルールをすべて返す反復子を取得します。
		/// </summary>
		/// <param name="token">変形識別トークン。</param>
		/// <param name="target">変形対象の数式。</param>
		/// <param name="history">変形履歴。</param>
		/// <returns>適用候補となるルールをすべて返す反復子。</returns>
		private static IEnumerable<Rule> GetApplyCandidateRules(DeformToken token, Formula target, DeformHistory history)
		{
			List<Rule> testRules = new List<Rule>();

			// 構成数式より関連づくルールを取得
			foreach (var consist in GetAllConsistToCheckRule(target))
			{
				foreach (var rule in consist.GetRelatedRulesOf(token, target, history))
				{
					if (testRules.Contains(rule)) continue; // 重複登録はしない
					testRules.Add(rule);
				}
			}

			testRules = token.GetApplyRules(target, history, testRules); // カスタム追加
			Utilities.Sort.StableSort<Rule>(testRules); // 優先度順にルールを並び替え

			for (int i = 0; i < testRules.Count; i++)
			{
				var rule = testRules[i];
	
				// 重複登録を削除
				for (int k = i + 1; k < testRules.Count; k++)
				{
					if (testRules[k].DistinguishedText == rule.DistinguishedText)
						testRules.RemoveAt(k--);
				}

				// 同じルールの連続適用はしない
				if (rule == history.CurrentNode.AppliedRule) testRules.RemoveAt(i--);
				// 抑止ルールなら削除
				else if (history.IsPreventedRule(rule)) testRules.RemoveAt(i--);
			}
			return testRules;
		}


		/// <summary>
		/// この数式の変形処理にあたって候補となるルールを参照すべきすべての数式を返す反復子を取得します。
		/// </summary>
		/// <param name="f">変形対象とする数式。</param>
		/// <returns>候補ルールの列挙に参照すべき数式をすべて返す反復子。</returns>
		private static IEnumerable<Formula> GetAllConsistToCheckRule(Formula f)
		{
			foreach (var child in GetAllConsistToCheckRule(f, 4)) // 4世代までを探査対象とする
				yield return child;
		}

		/// <summary>
		/// この数式の変形処理にあたって候補となるルールを参照すべきすべての数式を返す反復子を取得します。
		/// </summary>
		/// <param name="f">変形対象とする数式。</param>
		/// <param name="era">再帰的に参照すべき数式子要素の世代数</param>
		/// <returns>候補ルールの列挙に参照すべき数式をすべて返す反復子。</returns>
		private static IEnumerable<Formula> GetAllConsistToCheckRule(Formula f, int era)
		{
			yield return f;
			if (era <= 0) yield break;

			foreach (var consist in f)
			{
				foreach (var child in GetAllConsistToCheckRule(consist, era - 1))
					yield return child;
			}
		}

	}
}
