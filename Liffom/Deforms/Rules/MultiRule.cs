using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Deforms.Rules
{
	/// <summary>
	/// 複数のルールから成るルールを表します。
	/// </summary>
	public abstract class MultiRule : Rule
	{
		/// <summary>
		/// 複数のルールから成るルールを初期化します。
		/// </summary>
		/// <param name="consistRules">構成する可変数のルール</param>
		protected MultiRule(params Rule[] consistRules)
		{
			ConsistRules = new List<Rule>(consistRules);
		}

		/// <summary>
		/// このルールを構成するルールリストを取得します。
		/// </summary>
		protected List<Rule> ConsistRules { get; private set; }

		protected internal override bool IsTargetTypeFormula(Formula target)
		{
			foreach (var rule in ConsistRules)
				if (rule.IsTargetTypeFormula(target)) return true;
			return false;
		}

		protected override Formula OnTryMatchRule(Formula target)
		{
			bool ruleTreated = true;
			int ruleTreatCount = 0;
			Formula result = target;

			while (ruleTreated)
			{
				ruleTreated = false;
				foreach (var rule in ConsistRules)
				{
					if (rule.TryMatchRule(ref result))
					{
						ruleTreated = true;
						ruleTreatCount ++;
						break;
					}
				}
			}

			if (ruleTreatCount == 0) return null;
			else return result;
		}

	}
}
