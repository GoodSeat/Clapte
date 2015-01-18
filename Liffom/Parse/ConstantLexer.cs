using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Constants;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 定数の字句解析器を表します。
	/// </summary>
	public class ConstantLexer : Lexer
	{
		List<Constant> _constantList;
		List<Constant> _autoRegistedConstantList;

		/// <summary>
		/// 定数の字句解析器を初期化します。
		/// </summary>
		public ConstantLexer() : this(true) { }

		/// <summary>
		/// 定数の字句解析器を初期化します。
		/// </summary>
		/// <param name="autoRegistAssembly">アセンブリ内の定数を自動探索して解析可能な定数として登録するか否か。</param>
		public ConstantLexer(bool autoRegistAssembly)
		{
			_constantList = new List<Constant>();
			AutoRegistAssembly = autoRegistAssembly;
		}

		/// <summary>
		/// アセンブリ内の定数を自動探索して解析可能な定数として登録するか否かを設定もしくは取得します。
		/// </summary>
		public bool AutoRegistAssembly { get; set; }

		/// <summary>
		/// 解析の対象の定数として、定数を登録します。
		/// </summary>
		/// <param name="sample">追加登録する定数。</param>
		public void AddConstant(Constant sample)
		{
			_constantList.Add(sample);
		}

		/// <summary>
		/// 解析の対象の定数から、指定の定数を削除します。
		/// </summary>
		/// <param name="sample">登録から削除する定数。</param>
		public void RemoveConstant(Constant sample)
		{
			_constantList.Remove(sample);
		}


		public override Lexer.ScanResult Scan(string text)
		{
			var hit = GetHitConstant(text, false);
			if (hit != null) return ScanResult.Match;

			hit = GetHitConstant(text, true);
			if (hit != null) return ScanResult.NoMatch;
			return ScanResult.NeverMatch;
		}

		/// <summary>
		/// 指定名に関連付けられる定数を引数なしで初期化して取得します。
		/// </summary>
		/// <param name="name">探索する定数。</param>
		/// <param name="containStartWith">前方一致のみでも一致として判定するか。</param>
		/// <returns>見つかった引数なしの定数。見つからない場合、null。</returns>
		public Constant GetHitConstant(string name, bool containStartWith)
		{
			if (AutoRegistAssembly && _autoRegistedConstantList == null)
			{
				_autoRegistedConstantList = new List<Constant>();
				foreach (Constant sample in Constant.GetEnableConstants())
					_autoRegistedConstantList.Add(sample);
			}

			foreach (var sample in _constantList)
			{
				foreach (var distinguishedName in sample.GetAllDistinguishedNames())
					if (distinguishedName == name || (containStartWith && distinguishedName.StartsWith(name)))
						return sample;
			}
			if (!AutoRegistAssembly) return null;

			foreach (var sample in _autoRegistedConstantList)
			{
				foreach (var distinguishedName in sample.GetAllDistinguishedNames())
					if (distinguishedName == name || (containStartWith && distinguishedName.StartsWith(name)))
						return sample;
			}
			return null;
		}

		public override IEnumerable<Token> Tokenize(string text)
		{
			var constant = GetHitConstant(text, false);
			yield return new FormulaToken(text, constant);
		}
	}
}

