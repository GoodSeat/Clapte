using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;

namespace GoodSeat.Liffom.Formulas.Operators
{
	public abstract partial class Operator
	{
		/// <summary>
		/// 演算のルールパターン一致の確認メソッドを表します。
		/// </summary>
		struct RulePatternCheck
		{
			/// <summary>
			/// ルール数式のチェック対象を表します。
			/// </summary>
			[Flags]
			private enum PatternCheckTargetType
			{
				/// <summary>
				/// 情報がありません。
				/// </summary>
				None = 0,
				/// <summary>
				/// 他種類の子演算
				/// </summary>
				ChildOperator = 1,
				/// <summary>
				/// ルール変数、演算以外の数式
				/// </summary>
				Other = 2,
				/// <summary>
				/// 一致済みの数式指定のあるルール変数
				/// </summary>
				MatchedPatternVariable = 4,
				/// <summary>
				/// 条件指定付きのルール変数
				/// </summary>
				ConditionallyPatternVariable = 8,
				/// <summary>
				/// 自由なルール変数
				/// </summary>
				FreePatternVariable = 16,
				/// <summary>
				/// 無意味な数値の許可されたルール変数
				/// </summary>
				AdmitIgnorePatternVariable = 32
			}
			
			/// <summary>
			/// 演算のルールパターン一致の確認メソッドを初期化します。
			/// </summary>
			/// <param name="rule"></param>
			/// <param name="test"></param>
			public RulePatternCheck(Operator rule, Formula test)
			{
				_baseRuleFormula = rule;
				_baseTestFormula = test;
				_matchedRuleFormulas = new Dictionary<PatternCheckTargetType, Dictionary<Formula, Formula>>();

				_testFormulas = null;
				_ruleFormulas = null;
				_addableRulePatternVariables = new List<RulePatternVariable>();
				_testFormulas = GetIntegratedList(BaseTestFormula);
				_ruleFormulas = GetIntegratedList(BaseRuleFormula);
			}

			Operator _baseRuleFormula;
			Formula _baseTestFormula;
			List<Formula> _testFormulas;
			List<Formula> _ruleFormulas;
			Dictionary<PatternCheckTargetType, Dictionary<Formula, Formula>> _matchedRuleFormulas;

			List<RulePatternVariable> _addableRulePatternVariables;


			/// <summary>
			/// 検証対象のルール数式を設定もしくは取得します。
			/// </summary>
			Operator BaseRuleFormula
			{
				get { return _baseRuleFormula; }
				set { _baseRuleFormula = value; }
			}

			/// <summary>
			/// 検証対象の検証数式を設定もしくは取得します。
			/// </summary>
			public Formula BaseTestFormula
			{
				get { return _baseTestFormula; }
				set { _baseTestFormula = value; }
			}

			/// <summary>
			/// 未割当の検証対象数式リストを取得します。
			/// </summary>
			List<Formula> TestFormulas
			{
				get { return _testFormulas; }
				set { _testFormulas = value; }
			}

			/// <summary>
			/// 未割当の検証数式リストを取得します。
			/// </summary>
			List<Formula> RuleFormulas 
			{
				get { return _ruleFormulas; }
				set { _ruleFormulas = value; }
			}

			/// <summary>
			/// 子数式要素の追加設定可能なルールパターン変数一覧を設定もしくは取得します。
			/// </summary>
			public List<RulePatternVariable> AddableRulePatternVariables
			{
				get { return _addableRulePatternVariables; }
				set { _addableRulePatternVariables = value; }
			}

			/// <summary>
			/// ルールと数式の割り当て結果を表します。
			/// </summary>
			Dictionary<PatternCheckTargetType, Dictionary<Formula, Formula>> MatchedRuleFormulas
			{
				get { return _matchedRuleFormulas; }
				set { _matchedRuleFormulas = value; }
			}


			/// <summary>
			/// 内部の同演算要素を統合した子数式リストを取得します。
			/// </summary>
			/// <param name="f"></param>
			/// <returns></returns>
			List<Formula> GetIntegratedList(Formula f)
			{
				Type baseType = BaseRuleFormula.GetType();
				List<Formula> resultList = new List<Formula>();

				// 同演算は、分解の上、登録する
				if (f is Operator && f.GetType() == baseType)
				{
					foreach (Formula child in f)
					{
						if (f is OperatorMultiple)
							resultList.AddRange(GetIntegratedList(child));
						else
							resultList.Add(child);
					}
				}
				// 一致済み数式が同演算なら、分解の上、ルールに登録する
				else if (f is RulePatternVariable)
				{
					RulePatternVariable r = f as RulePatternVariable;
					Formula matched = r.MatchedFormula;
					if (matched != null && matched.GetType() == baseType)
						resultList.AddRange(GetIntegratedList(matched));
					else
						resultList.Add(f);
				}
				else
					resultList.Add(f);
				return resultList;
			}

			/// <summary>
			/// 指定数式のルール対象種類を取得します。
			/// </summary>
			/// <param name="rule"></param>
			/// <returns></returns>
			PatternCheckTargetType GetTargetTypeOf(Formula rule)
			{
				if (rule is Operator)
				{
					FormulaAssertionException.Assert(rule.GetType() != BaseRuleFormula.GetType());
					return PatternCheckTargetType.ChildOperator;
				}
				else if (rule is RulePatternVariable)
				{
					RulePatternVariable r = rule as RulePatternVariable;
					if (r.MatchedFormula != null) return PatternCheckTargetType.MatchedPatternVariable;

					PatternCheckTargetType result = PatternCheckTargetType.None;
					if (r.CheckTarget != null) result = PatternCheckTargetType.ConditionallyPatternVariable;
					else result = PatternCheckTargetType.FreePatternVariable;

					if (BaseRuleFormula is Sum && r.AdmitAddZero) result |= PatternCheckTargetType.AdmitIgnorePatternVariable;
					if (BaseRuleFormula is Product && r.AdmitMultiplyOne) result |= PatternCheckTargetType.AdmitIgnorePatternVariable;
//					if (BaseRuleFormula is Power && r.AdmitPowerOne) result |= PatternCheckTargetType.AdmitIgnorePatternVariable;
					return result;
				}
				else
					return PatternCheckTargetType.Other;
			}




			/// <summary>
			/// 組み合わせ条件のもと、ルールパターン数式と検討対象数式の一致を調べます。
			/// </summary>
			/// <returns></returns>
			public bool CheckPatternMatchCombination()
			{
				List<PatternCheckTargetType> checkOrder = new List<PatternCheckTargetType>();
				checkOrder.Add(PatternCheckTargetType.ChildOperator);
				checkOrder.Add(PatternCheckTargetType.Other);
				checkOrder.Add(PatternCheckTargetType.MatchedPatternVariable);
				checkOrder.Add(PatternCheckTargetType.ConditionallyPatternVariable);
				checkOrder.Add(PatternCheckTargetType.ConditionallyPatternVariable | PatternCheckTargetType.AdmitIgnorePatternVariable);
				checkOrder.Add(PatternCheckTargetType.FreePatternVariable);
				checkOrder.Add(PatternCheckTargetType.FreePatternVariable | PatternCheckTargetType.AdmitIgnorePatternVariable);

				foreach (PatternCheckTargetType checkType in checkOrder)
				{
					MatchedRuleFormulas.Add(checkType, new Dictionary<Formula, Formula>());

					for (int r = 0; r < RuleFormulas.Count; r++)
					{
						Formula rule = RuleFormulas[r];
						if ((GetTargetTypeOf(rule) & checkType) != checkType) continue;

						if ((checkType & PatternCheckTargetType.MatchedPatternVariable) != PatternCheckTargetType.MatchedPatternVariable &&
							rule is RulePatternVariable)
							AddableRulePatternVariables.Add(rule as RulePatternVariable);

						bool matched = false;
						for (int t = 0; t < TestFormulas.Count; t++)
						{
							Formula test = TestFormulas[t];
//							if ((GetTargetTypeOf(test) & checkType) != checkType) continue;

							if (test.PatternMatch(rule, false))
							{
								TestFormulas.RemoveAt(t);
								RuleFormulas.RemoveAt(r);
								MatchedRuleFormulas[checkType].Add(rule, test);
								matched = true;
								r--;
								break;
							}
						}

						if (checkType == PatternCheckTargetType.ChildOperator ||
							checkType == PatternCheckTargetType.Other )
						{
							if (!matched) return false;
						}
						if (checkType == PatternCheckTargetType.MatchedPatternVariable)
						{
							var rv = rule as RulePatternVariable;
							if (!matched && !rv.IsHitAdmitOneOrZero) return false; 
						}

					}
				}

				if (!AssinSurplusRule()) return false;
				if (!AddSurplusTestToRule()) return false;
				return true;
			}

			/// <summary>
			/// 未割当の検証数式に対し、追加可能なルールへの割り当てを試みます。
			/// </summary>
			/// <returns></returns>
			private bool AddSurplusTestToRule()
			{
				List<PatternCheckTargetType> checkOrder = new List<PatternCheckTargetType>();
				checkOrder.Add(PatternCheckTargetType.ConditionallyPatternVariable);
				checkOrder.Add(PatternCheckTargetType.ConditionallyPatternVariable | PatternCheckTargetType.AdmitIgnorePatternVariable);
				checkOrder.Add(PatternCheckTargetType.FreePatternVariable);
				checkOrder.Add(PatternCheckTargetType.FreePatternVariable | PatternCheckTargetType.AdmitIgnorePatternVariable);

				foreach (Formula test in TestFormulas)
				{
					bool matched = false;
					foreach (PatternCheckTargetType checkType in checkOrder)
					{
						RulePatternVariable rule = null;
						Formula newTest = null;

						foreach (KeyValuePair<Formula, Formula> set in MatchedRuleFormulas[checkType])
						{
							rule = set.Key as RulePatternVariable;
							newTest = BaseRuleFormula.CreateOperator(set.Value, test);

							Formula sav = rule.MatchedFormula;
							rule.ClearMatchedFormula();
							if (newTest.PatternMatch(rule, true))
							{
								matched = true;
								break;
							}
							else
								rule.MatchedFormula = sav;
						}

						if (matched)
						{
							MatchedRuleFormulas[checkType].Remove(rule);
							MatchedRuleFormulas[checkType].Add(rule, newTest);
							break;
						}
					}
					if (!matched) return false;
				}
				return true;
			}

			/// <summary>
			/// 未割当のルール数式に無効数値の割り当てを試みます。
			/// </summary>
			/// <returns></returns>
			private bool AssinSurplusRule()
			{
				// 無意の 1 or 0 の設定を試みる
				foreach (Formula rule in RuleFormulas)
				{
					RulePatternVariable r = rule as RulePatternVariable;
					if (r == null) return false;
					if (!r.SetAdmitValueAuto(BaseRuleFormula)) return false;
				}
				return true;
			}

			/// <summary>
			/// 順列条件のもと、ルールパターン数式と検討対象数式の一致を調べます。
			/// </summary>
			/// <returns></returns>
			public bool CheckPatternMatchPermutation()
			{
				if (TestFormulas.Count != RuleFormulas.Count) return false;
				for (int r = 0; r < RuleFormulas.Count; r++)
				{
					if (!TestFormulas[r].PatternMatch(RuleFormulas[r], false))
						return false;
				}
				return true;
			}
		}
	}
}
