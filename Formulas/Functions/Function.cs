using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Functions.Rules;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Utilities;

namespace GoodSeat.Liffom.Formulas.Functions
{
	/// <summary>
	/// 関数を表します。
	/// </summary>
	[Serializable()]
	public abstract class Function : AtomicFormula
	{
		static List<Reflector<Function>> s_cacheFunctionInformation;

		/// <summary>
		/// 利用可能な全ての関数クラス情報を返す反復子を取得します。
		/// </summary>
		/// <returns>システム内で利用可能な関数クラス情報を返す反復子。</returns>
		/// <remarks>Liffom.dll内、及び同ディレクトリ内のクラスライブラリ中に存在する関数を探索します。</remarks>
		public static IEnumerable<Reflector<Function>> GetEnableFunctionsReflector()
		{
			if (s_cacheFunctionInformation == null)
				s_cacheFunctionInformation = new List<Reflector<Function>>(Reflector<Function>.FindReflectors());

			foreach (var reflector in s_cacheFunctionInformation) yield return reflector;
		}


		// 引数
		Argument _argument = new Argument();

		/// <summary>
		/// 関数を初期化します。
		/// </summary>
		public Function() { AddNull(); }

		/// <summary>
		/// 関数を初期化します。
		/// </summary>
		/// <param name="arg">引数</param>
		public Function(Argument arg) { Argument = arg; }

		/// <summary>
		/// 関数を初期化します。
		/// </summary>
		/// <param name="f">引数リスト</param>
		public Function(params Formula[] f) 
		{
			if (f.Length == 1 && f[0] is Argument)
				Argument = f[0] as Argument;
			else
				Argument = new Argument(f);
		}

		/// <summary>
		/// 可変数の数式を指定して、子数式を有する数式を初期化します。
		/// </summary>
		/// <param name="fs">数式を構成する可変数の子数式。</param>
		/// <returns>初期化された数式。</returns>
		public override Formula CreateFromChildren(params Formula[] fs) 
		{
			if (fs.Length < MinimumArgumentQty) throw new FormulaProcessException(string.Format("関数 {0} に対し、少なすぎる引数が指定されました。", GetType()));
			if (fs.Length > MaximumArgumentQty) throw new FormulaProcessException(string.Format("関数 {0} に対し、多すぎる引数が指定されました。", GetType()));
			return CreateFunction(fs);
		}

		/// <summary>
		/// 引数を指定して、関数を生成します。
		/// </summary>
		/// <param name="args">初期化に用いるか変数の数式。</param>
		/// <returns>初期化された関数。</returns>
		public abstract Function CreateFunction(params Formula[] args);


		/// <summary>
		/// 足りない引数分、Null数式を追加します。
		/// </summary>
		void AddNull()
		{
			// 足りない分Nullを追加
			for (int i = Argument.Count; i < MinimumArgumentQty; i++)
				Argument.Add(new Null());
		}

		#region 関数プロパティ

		/// <summary>
		/// 関数の引数を設定もしくは取得します。
		/// </summary>
		public Argument Argument
		{
			set
			{
				_argument = value;
				AddNull();
			}
			get
			{
				return _argument;
			}
		}

		/// <summary>
		/// この関数として識別する文字列を取得します。
		/// </summary>
		public virtual string DistinguishedName { get { return this.GetType().Name.ToLower(); } }

		/// <summary>
		/// この関数の引数として最低限必要な引数の数を取得します。
		/// </summary>
		public abstract int MinimumArgumentQty { get; }

		/// <summary>
		/// この関数の引数として可能な引数の最大数を取得します。通常は、MinimumArgumentQtyと同様の値を返します。
		/// </summary>
		public virtual int MaximumArgumentQty { get { return MinimumArgumentQty; } }

		/// <summary>
		/// 関数の説明を取得します。
		/// </summary>
		/// <param name="args">引数の説明。</param>
		/// <returns>関数の説明。</returns>
		public abstract string GetInformation(out List<string> args);

		#endregion

		#region 関数処理

		/// <summary>
		/// 関数に設定された引数により、関数を評価します。
		/// </summary>
		/// <returns>関数の評価結果。</returns>
		public abstract Formula CalculateFunction();

		#endregion

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

			if (deformToken.Has<CalculateToken>()) yield return CalculateFunctionRule.Entity;
		}

		public override string GetText() 
		{
			return string.Format("{0}({1})", DistinguishedName, Argument.ToString());
		}

		public override Formula this[int i]
		{
			get { return Argument[i]; }
			set { Argument[i] = value; }
		}

		/// <summary>
		/// 数式の一意性評価に用いる文字列で、同じ型の数式同士の一意性を表す文字列を取得します。
		/// </summary>
		/// <returns>数式を一意に区別する文字列。</returns>
		protected override string OnGetUniqueText()
		{
			var children = new List<string>();
			foreach (var child in this) children.Add(child.GetUniqueText());
			return string.Join(",", children);
		}
		
	}
}
