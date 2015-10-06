using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Deforms
{
	/// <summary>
	/// 数式変形の種別を識別するトークンを表します。
	/// </summary>
	[Serializable()]
	public class DeformToken
	{
		[NonSerialized()]
		List<Rule> _additionalTryRules;

		/// <summary>
		/// 数式変形の種別を識別するトークンを初期化します。
		/// </summary>
		public DeformToken(params DeformToken[] children)
		{
            EraMaximum = 50;

			Children = new List<DeformToken>(children);
			AdditionalTryRules = new List<Rule>();
		}

        /// <summary>
        /// 数式変形中に許容する最大履歴深さを設定若しくは取得します。
        /// </summary>
        public int EraMaximum { get; set; }

		/// <summary>
		/// 変形種別トークンの子変形種別トークンリストを取得します。
		/// </summary>
		public List<DeformToken> Children { get; private set; }

		/// <summary>
		/// 常に変形を試行する変形ルールリストを取得します。
		/// </summary>
		public List<Rule> AdditionalTryRules 
		{
			get { return _additionalTryRules; }
			private set { _additionalTryRules = value; }
		}

		/// <summary>
		/// この変形識別トークン、もしくはその子孫の変形識別トークンに、指定変形識別トークンが存在するか否かを取得します。
		/// </summary>
		/// <param name="token">探索対象の変形識別トークン。</param>
		public bool Has(DeformToken token)
		{
			return Has(token.GetType());
		}

		/// <summary>
		/// この変形識別トークン、もしくはその子孫の変形識別トークンに、指定変形識別トークンが存在するか否かを取得します。
		/// </summary>
		public bool Has<T> () where T : DeformToken
		{
			return Has(typeof(T));
		}

		/// <summary>
		/// この変形識別トークン、もしくはその子孫の変形識別トークンに、指定変形識別トークンが存在するか否かを取得します。
		/// </summary>
		/// <param name="tokenType">探索対象の変形識別トークンの型。</param>
		public bool Has(Type tokenType)
		{
			if (tokenType == GetType()) return true;
			foreach (var child in Children)
				if (child.Has(tokenType)) return true;

			return false;
		}

		/// <summary>
		/// 指定数式の変形にあたって、適用するルールリストを取得します。既定では、変形対象とする数式、及びその構成数式から取得されるルールで構成されます。
		/// </summary>
		/// <param name="target">変形対象の数式。</param>
		/// <param name="history">変形履歴情報。</param>
		/// <param name="originalRules">変形対象とする数式、及びその構成数式から取得されるルールリスト。</param>
		/// <returns>最終的な適用候補とするルールリスト。</returns>
		public virtual List<Rule> GetApplyRules(Formula target, DeformHistory history, List<Rule> originalRules)
		{
			foreach (var rule in AdditionalTryRules) originalRules.Add(rule);

			return originalRules;
		}

		/// <summary>
		/// 指定された数式をソートします。
		/// </summary>
		/// <param name="f">ソート対象の数式。</param>
		public virtual void Sort(Formula f) { f.Sort(); }

	}
}
