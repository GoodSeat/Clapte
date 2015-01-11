using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Liffom.Formulas.Units.Rules;

namespace GoodSeat.Liffom.Formulas.Units
{
	/// <summary>
	/// 単位を表します。
	/// </summary>
	/// <remarks>
	/// 単位は、積算中、累乗の累乗数以外には存在しない。
	/// 単位項は、積算中以外には存在しない。
	/// </remarks>
	[Serializable()]
	public class Unit : AtomicFormula
	{
		string _unitName;
		Prefix _prefix;

		/// <summary>
		/// 単位を初期化します。
		/// </summary>
		/// <param name="unitName">単位名</param>
		/// <param name="prefix">接頭辞</param>
		public Unit(string unitName, Prefix prefix)
		{
			UnitName = unitName;
			Prefix = prefix;
		}

		/// <summary>
		/// 単位を初期化します。
		/// </summary>
		/// <param name="unitName">単位名。接頭辞は自動判定されます。</param>
		public Unit(string unitName)
		{
			// 何れかの登録単位の単位名に一致するなら接頭辞探索はしない
			foreach (UnitConvertTable table in UnitConvertTable.ValidTables.Values)
			{
				foreach (UnitConvertRecord record in table.GetAllRecords(true))
				{
					if (!(record.ConvertUnit is Unit)) continue;

					Unit unitCheck = record.ConvertUnit as Unit;
					if (unitCheck.UnitName == unitName)
					{
						UnitName = unitName;
						return;
					}
				}
			}

			string hitMark = "";
			foreach (Prefix prefix in Units.Prefix.GetAllPrefix(false))
			{
				if (unitName.StartsWith(prefix.Mark) && prefix.Mark != unitName && prefix.Mark.Length > hitMark.Length &&
					!(unitName[prefix.Mark.Length] >= '0' && unitName[prefix.Mark.Length] <= '9'))
				{
					hitMark = prefix.Mark;
				}
			}
			Prefix = Units.Prefix.GetPrefixFrom(hitMark);
			UnitName = unitName.Substring(Prefix.ToString().Length);
		}

		/// <summary>
		/// 単位接頭辞を含まない単位の表示名称を取得します。
		/// </summary>
		public string UnitName
		{
			private set { _unitName = value; }
			get { return _unitName; }
		}

		/// <summary>
		/// 単位の接頭辞を取得します。
		/// </summary>
		public Prefix Prefix
		{
			get 
			{
				if (_prefix == null) _prefix = new NullPrefix();
				return _prefix; 
			}
			private set { _prefix = value; }
		}



		/// <summary>
		/// 単位の所属するテーブルタイプを取得します。見つからない場合、nullを返します。
		/// </summary>
		public string UnitType
		{
			get
			{
				// 単位変換テーブル更新に伴う単位整合性チェック
				string unitName = Prefix.ToString() + UnitName;
				Unit testUnit = new Unit(unitName);
				UnitName = testUnit.UnitName;
				Prefix = testUnit.Prefix;

				foreach (UnitConvertTable table in UnitConvertTable.ValidTables.Values)
				{
					if (table.GetRecordOf(this) != null)
						return table.UnitType;
				}
				return null;
			}
		}

		/// <summary>
		/// 所属単位変換テーブルを取得します。
		/// </summary>
		public UnitConvertTable BelongTable
		{
			get 
			{
				if (UnitType != null && UnitConvertTable.ValidTables.ContainsKey(UnitType))
					return UnitConvertTable.ValidTables[UnitType];
				else
					return null;
			}
		}

		/// <summary>
		/// 指定単位への変換倍率を取得します。変換できない場合、nullを返します。
		/// </summary>
		/// <param name="targetUnit">変換先の単位。</param>
		/// <returns>変換倍率。変換できなかった場合、null。</returns>
		public Formula GetModifyTo(Unit targetUnit)
		{
			if (!IsEqualTypeTo(targetUnit)) return null;

			if (UnitName == targetUnit.UnitName)
				return Prefix.GetModify(Prefix, targetUnit.Prefix);
			else
				return BelongTable.GetConversionRatio(this, targetUnit);
		}

		/// <summary>
		/// 指定単位と同単位次元か否かを取得します。
		/// </summary>
		/// <param name="unit">比較対象単位</param>
		/// <returns></returns>
		public bool IsEqualTypeTo(Unit unit)
		{
			if (UnitName == unit.UnitName) return true;
			return (UnitType != null && UnitType == unit.UnitType) ;
		}

		public override string GetText() { return Prefix.ToString() + UnitName; }

		/// <summary>
		/// 数式の一意性評価に用いる文字列で、同じ型の数式同士の一意性を表す文字列を取得します。
		/// </summary>
		/// <returns>数式を一意に区別する文字列。</returns>
		protected override string OnGetUniqueText() { return GetText(); }

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

			// 単位系の統一、ただし既に実行されていたら対象外
			if (deformToken.Has<CombineToken>())
			{
				if (history.GetAppliedRules().Count(rule => rule is UniteUnitRule) == 0)
					yield return UniteUnitRule.Entity;
			}

			if (sender is Product) foreach(var rule in  GetProductRelatedRulesOf(sender as Product, deformToken)) yield return rule;
			if (sender is Sum) foreach(var rule in  GetSumRelatedRulesOf(sender as Sum, deformToken)) yield return rule;
		}

		/// <summary>
		/// 接頭辞を削除した単位を生成して取得します。
		/// </summary>
		public Formula RemovePrefix()
		{
			if (Prefix is NullPrefix) return this;

			var nullPrefix = new NullPrefix();
			var modify = Prefix.GetModify(nullPrefix);
			Unit unit = new Unit(UnitName, nullPrefix);

			return modify * unit;
		}

		public IEnumerable<Rule> GetProductRelatedRulesOf(Product sender, DeformToken deformToken)
		{
			if (deformToken.Has<CombineToken>())
			{
				yield return DivideUnitRule.Entity; // 単位の分離 5*a[kN]*b[m] → (5*a*b)*[kN*m]
			}
		}

		public IEnumerable<Rule> GetSumRelatedRulesOf(Sum sender, DeformToken deformToken)
		{
			if (deformToken.Has<CombineToken>())
			{
				yield return new CombineUnitSectionSumRule(); // a[b] + c[b] → (a+c)[b]
			}
		}

	}	
}
