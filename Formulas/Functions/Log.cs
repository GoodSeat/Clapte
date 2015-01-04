using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using Liffom.Deforms;
using Liffom.Deforms.Rules;
using Liffom.Formulas.Functions.Rules;
using Liffom.Formulas.Operators;

namespace Liffom.Formulas.Functions
{
	/// <summary>
	/// log関数を表します。
	/// </summary>
	[Serializable()]
	public class Log : Function
	{
		/// <summary>
		/// 対数関数を初期化します。
		/// </summary>
		public Log() : base(1d, 10d) { }

		/// <summary>
		/// 対数関数を初期化します。
		/// </summary>
		/// <param name="f">対数の対象。</param>
		public Log(Formula f) : base(f, 10d) { }

		/// <summary>
		/// 対数関数を初期化します。
		/// </summary>
		/// <param name="f">対数の対象。</param>
		/// <param name="b">対数の底。</param>
		public Log(Formula f, Formula b) : base(f, b) { }

		/// <summary>
		/// 引数を指定して、関数を生成します。
		/// </summary>
		/// <param name="args">初期化に用いるか変数の数式。</param>
		/// <returns>初期化された関数。</returns>
		public override Function CreateFunction(params Formula[] args)
		{
			if (args.Length < 2) return new Log(args[0]);
			else return new Log(args[0], args[1]);
		}

		/// <summary>
		/// 対数の底を取得します。
		/// </summary>
		public Formula LogBase
		{
			get
			{
				if (Argument.Count > 1) return Argument[1];
				else return new Numeric(10d);
			}
		}

		public override Formula CalculateFunction()
		{
			Numeric n = Argument[0] as Numeric;
			Numeric newBase = Argument.Count > 1 ? Argument[1] as Numeric : null;

			if (n != null && (Argument.Count == 1 || newBase == null))
				return new Numeric(n.Data.Log(new Numeric(10d)));
			else if (n != null && newBase != null)
				return new Numeric(n.Data.Log(newBase));
			else
				return this;
		}

		public override int MinimumArgumentQty
		{
			get { return 1; }
		}

		public override int MaximumArgumentQty
		{
			get { return 2; }
		}

		public override string GetInformation(out List<string> args)
		{
			args = new List<string>(); args.Add("数値"); args.Add("底（省略可。省略時10）");
			return "指定された数を底とする数値の対数を返します。";
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

			if ((deformToken.Has<CalculateToken>() || deformToken.Has<DifferentiateToken>()) && sender is Differentiate)
			{
				var x = new RulePatternVariable("x");
				var y = new RulePatternVariable("y");
				yield return new InstantPatternRule(new Differentiate(new Log(x, y), x), 1 / (x * new Ln(y)));

				yield return new DifferentiateCompositeFunctionRule(0);
			}
		}
	}
}
