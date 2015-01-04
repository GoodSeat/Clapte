using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Deforms;

namespace GoodSeat.Liffom.Deforms.Rules
{
	/// <summary>
	/// ルールパターン数式に用いる変数を表します。
	/// </summary>
	/// <remark>
	/// ルールパターン数式の子数式に演算等、ルール変数以外の数式がある場合、その数と、当該数式に合致する子数式の数の一致を見る。
	/// ルールパターン数式の子数式に複数のルールパターン変数がある場合、その数の一致をみる。
	/// </remark>
	/// <example>
	/// a^b-1
	///  ○ 1*2^-3 (a=1 b=2), 1*2*3^-1 (a=1*2 b=3), 1*2^2*(3*4)^-1 (a=1*2^2 b=3*4)
	///  × 1*2^-1*3^-1 (bに該当する子数式が1より多く存在)
	/// a*b
	///  ○ 2*3(a=2 b=3), 2*(3+4) (a=2,b=3+4)
	///  × 2*3*4
	/// </example>
	[Serializable()]
	public class RulePatternVariable : Variable
	{
		Formula  _matchedFormula;
		bool _admitPowerOne = false;
		bool _admitMultiplyOne = false;
		bool _admitAddZero = false;
		
		/// <summary>
		/// ルールパターン変数を初期化します。
		/// </summary>
		/// <param name="mark"></param>
		public RulePatternVariable(string mark) : base(mark){}

		/// <summary>
		/// ルールパターン数式中で一致した数式を設定もしくは取得します。
		/// </summary>
		public Formula MatchedFormula
		{ 
			get { return _matchedFormula; }
			set 
			{
				FormulaAssertionException.Assert(IsMatchFormula(value) || IsAdmitValue(value));
				_matchedFormula = value; 
			}
		}

		/// <summary>
		/// 累乗の指数として1を許可するかを設定もしくは取得します。
		/// </summary>
		public bool AdmitPowerOne
		{
			get { return _admitPowerOne; }
			set { _admitPowerOne = value; }
		}

		/// <summary>
		/// 積算として1を許可するかを設定もしくは取得します。
		/// </summary>
		public bool AdmitMultiplyOne
		{
			get { return _admitMultiplyOne; }
			set { _admitMultiplyOne = value; }
		}

		/// <summary>
		/// 和算として0を許可するかを設定もしくは取得します。
		/// </summary>
		public bool AdmitAddZero
		{
			get { return _admitAddZero; }
			set { _admitAddZero = value; }
		}

		/// <summary>
		/// 無意味な数値にヒットしているか否かを取得します。
		/// </summary>
		public bool IsHitAdmitOneOrZero
		{
			get
			{
				if (AdmitPowerOne || AdmitMultiplyOne)
				{
					return (MatchedFormula == 1);
				}
				if (AdmitAddZero)
				{
					return (MatchedFormula == 0);
				}
				return false;
			}
		}


		/// <summary>
		/// ルールパターン数式中の一致数式を削除します。
		/// </summary>
		public void ClearMatchedFormula() { _matchedFormula = null; }


		[NonSerialized()]
		Formula.IsTargetFormula _isTargetFormula;
		
		/// <summary>
		/// 処理対象とする付加条件の判断処理を設定もしくは取得します。
		/// </summary>
		public Formula.IsTargetFormula CheckTarget
		{ 
			get { return _isTargetFormula; }
			set { _isTargetFormula = value; }
		}
		
		/// <summary>
		/// 指定数式が、処理対象とする付加条件を満たすか否かを取得します。
		/// </summary>
		public bool IsMatchFormula(Formula f)
		{
			if (CheckTarget != null)
				return CheckTarget(f);
			return true;
		}

		/// <summary>
		/// 指定数値が無意味な数値と明示した上で設定可能か否かを取得します。
		/// </summary>
		/// <param name="f"></param>
		/// <returns></returns>
		public bool IsAdmitValue(Formula f)
		{
			if (AdmitAddZero && f == 0) return true;
			if (AdmitPowerOne && f == 1) return true;
			if (AdmitMultiplyOne && f == 1) return true;
			return false;
		}

		/// <summary>
		/// 無意味な数値であることを明示して、一致数式を設定します。
		/// </summary>
		/// <param name="f">無意となる数値</param>
		/// <returns></returns>
		public bool SetAdmitValue(Formula f)
		{
			if (!IsAdmitValue(f)) return false;
			MatchedFormula = f;
			return true;
		}

		/// <summary>
		/// 親の演算を指定して、無意数値を自動設定します。
		/// </summary>
		/// <param name="parent">親の演算子</param>
		/// <returns></returns>
		public bool SetAdmitValueAuto(Operator parent)
		{
			if (parent is Sum) return SetAdmitValue(0);
			if (parent is Product) return SetAdmitValue(1);
			if (parent is Power) return SetAdmitValue(1);
			return false;
		}

		/// <summary>
		/// ルールパターン数式に対し、指定数式が合致するか否かを返します。
		/// </summary>
		/// <param name="f">判定対象の数式</param>
		/// <returns>ルールパターン数式に一致するか否か</returns>
		protected override bool OnCheckPatternMatch(Formula f)
		{
			if (MatchedFormula != null)
				return f == MatchedFormula;
			else if (IsMatchFormula(f))
			{
				MatchedFormula = f;
				return true;
			}

			return false;
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

			yield return ReferencePatternMatchedRule.Entity;
		}

	}
}
