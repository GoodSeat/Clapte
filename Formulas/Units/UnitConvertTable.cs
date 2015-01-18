using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using GoodSeat.Liffom.Processes;
using GoodSeat.Liffom.Reals;

namespace GoodSeat.Liffom.Formulas.Units
{
	/// <summary>
	/// 単位変換表を表します。
	/// </summary>
	[Serializable()]
	public class UnitConvertTable
	{
		static Dictionary<string, UnitConvertTable> s_unitTables = new Dictionary<string, UnitConvertTable>();

		/// <summary>
		/// 有効な単位種類-単位変換表マップを取得します。
		/// </summary>
		public static Dictionary<string, UnitConvertTable> ValidTables
		{
			get { return s_unitTables; }
		}

		/// <summary>
		/// 有効な単位種類-単位変換表マップに単位変換表を登録します。
		/// </summary>
		/// <param name="table">登録する単位変換表。</param>
		public static void AddTable(UnitConvertTable table)
		{
			ValidTables.Add(table.UnitType, table);
		} 

		/// <summary>
		/// 有効な単位種類-単位変換表マップから単位変換表を削除します。
		/// </summary>
		/// <param name="table">削除する単位変換表。</param>
		public static void RemoveTable(UnitConvertTable table)
		{
			ValidTables.Remove(table.UnitType);
		}

		string _unitType = "";
		string _typeComment = "";

		BaseUnitConvertRecord _baseUnit;
		List<UnitConvertRecord> _convertUnitList = new List<UnitConvertRecord>();

		
		/// <summary>
		/// 単位変換表を初期化します。
		/// </summary>
		public UnitConvertTable() { }

		/// <summary>
		/// 単位変換表を初期化します。
		/// </summary>
		/// <param name="baseUnit">基準とする単位扱い数式。</param>
		/// <param name="unitType">単位タイプを表す文字列。</param>
		public UnitConvertTable(Formula baseUnit, string unitType)
		{
			if (!baseUnit.IsUnit(true)) throw new ArgumentException(string.Format("基準とする単位数式は、単位扱い数式である必要があります。"));

			UnitType = unitType;
			BaseUnit = new BaseUnitConvertRecord(baseUnit);
		}

		/// <summary>
		/// 単位の種類を表す一意の文字列を設定もしくは取得します。
		/// </summary>
		/// <example>Weight、Length</example>
		public string UnitType
		{
			get { return _unitType; }
			set { _unitType = value; }
		}

		/// <summary>
		/// 単位種類の説明を設定もしくは取得します。
		/// </summary>
		public string TypeComment
		{
			get { return _typeComment; }
			set { _typeComment = value; }
		}

		/// <summary>
		/// 変換の基準となる単位を設定もしくは取得します。
		/// </summary>
		public BaseUnitConvertRecord BaseUnit
		{
			get { return _baseUnit; }
			set 
			{
				if (_baseUnit != null) _baseUnit.BelongTable = null;

				_baseUnit = value;
				if (_baseUnit != null) _baseUnit.BelongTable = this;
			}
		}


		/// <summary>
		/// 単位変換データを追加します。
		/// </summary>
		/// <param name="record">単位変換データ</param>
		public void AddRecord(UnitConvertRecord record) { InsertRecord(_convertUnitList.Count, record); }

		/// <summary>
		/// 単位変換データを追加します。
		/// </summary>
		/// <param name="index">追加先インデックス</param>
		/// <param name="record">単位変換データ</param>
		public void InsertRecord(int index, UnitConvertRecord record)
		{
			// 同単位を対象としたレコードの重複を不許可
			if (UnitConvertTable.ValidTables.ContainsKey(UnitType))
			{
				foreach (UnitConvertTable table in UnitConvertTable.ValidTables.Values)
				{
					foreach (UnitConvertRecord data in table.GetAllRecords(true))
					{
						if (record.ConvertUnit is Unit && data.ConvertUnit is Unit)
						{
							if ((record.ConvertUnit as Unit).UnitName == (data.ConvertUnit as Unit).UnitName)
								throw new FormulaAssertionException("既に" + (data.ConvertUnit as Unit).UnitName + "を対象とした変換データが登録されています（" + table.UnitType + "）");
						}
					}
				}
			}
			// 同テーブル内では組立単位の重複を不許可
			foreach (UnitConvertRecord data in GetAllRecords(true))
			{
				if (data.ConvertUnit == record.ConvertUnit)
					throw new FormulaAssertionException("既に" + record.ConvertUnit.ToString() + "を対象とした変換データが登録されています。");
			}

			if (record.BelongTable != null) record.BelongTable.RemoveRecord(record.ConvertUnit);

			record.BelongTable = this;
			_convertUnitList.Insert(index, record);
		}

		/// <summary>
		/// 指定単位への変換データを削除します。
		/// </summary>
		/// <param name="unit"></param>
		public void RemoveRecord(Formula unit)
		{
			for (int i = 0; i < _convertUnitList.Count; i++)
			{
				if (_convertUnitList[i].ConvertUnit == unit)
				{
					_convertUnitList[i].BelongTable = null;
					_convertUnitList.RemoveAt(i);
					return;
				}
			}
		}

		/// <summary>
		/// 全ての変換レコードを返す反復子を取得します。
		/// </summary>
		/// <param name="containBase">基準単位レコードを含むか</param>
		/// <returns></returns>
		public IEnumerable<UnitConvertRecord> GetAllRecords(bool containBase)
		{
			if (containBase && BaseUnit != null) yield return BaseUnit;

			foreach (UnitConvertRecord data in _convertUnitList)
				yield return data;
		}

		/// <summary>
		/// 指定単位数式のレコードの登録インデックスを取得します。見つからない場合、-1を返します。
		/// </summary>
		/// <param name="formula"></param>
		/// <returns></returns>
		public int GetIndexOf(Formula formula)
		{
			for (int i = 0; i < _convertUnitList.Count; i++)
			{
				UnitConvertRecord record = _convertUnitList[i];
				if (record.ConvertUnit == formula) return i;
			}
			return -1;
		}

		/// <summary>
		/// 登録された変換レコード数を取得します。
		/// </summary>
		public int CountRecord { get { return _convertUnitList.Count; } }


		/// <summary>
		/// 指定した変換単位に関連付けられた単位変換データを取得します。見つからない場合、nullを返します。
		/// </summary>
		/// <param name="formula">目標単位数式</param>
		/// <returns></returns>
		public UnitConvertRecord GetRecordOf(Formula formula)
		{
			Formula modifyDummy;
			return GetRecordOf(formula, out modifyDummy);
		}

		/// <summary>
		/// 指定した変換単位に関連付けられた単位変換データを取得します。見つからない場合、nullを返します。
		/// </summary>
		/// <param name="formula">目標単位数式</param>
		/// <param name="modify">当該単位テーブルに対する目標単位の補正倍率</param>
		/// <example>cmでmの単位変換データを取得した場合、modify = 10^2</example>
		/// <returns></returns>
		public UnitConvertRecord GetRecordOf(Formula formula, out Formula modify)
		{
			modify = 1;
			if (!(formula is Unit))
			{
				Formula baseModify;
				Formula baseUnit = GetPrefixRemovedUnitFormula(formula, out baseModify);

				foreach (UnitConvertRecord data in GetAllRecords(true)) // 単位接頭辞変換だけで変換可能なレコードを探す
				{
					if (data.ConvertUnit is Unit) continue;

					Formula toModify;
					Formula toUnit = GetPrefixRemovedUnitFormula(data.ConvertUnit, out toModify);
					if (baseUnit != toUnit) continue;

					modify = (baseModify / toModify).Simplify();
					return data;
				}
			}
			else
			{
				Unit unit = formula as Unit;
				foreach (UnitConvertRecord data in GetAllRecords(true))
				{
					if (!(data.ConvertUnit is Unit)) continue;
					if (unit.UnitName == (data.ConvertUnit as Unit).UnitName)
					{
						modify = unit.GetModifyTo(data.ConvertUnit as Unit);
						return data;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// 指定単位扱い数式中に存在する単位接頭辞をすべて削除して取得します。
		/// </summary>
		/// <param name="unitFormula">対象の単位扱い数式。</param>
		/// <param name="modify">単位接頭辞の削除のための変換係数。</param>
		/// <returns>単位接頭辞をすべて削除した単位扱い数式。</returns>
		private Formula GetPrefixRemovedUnitFormula(Formula unitFormula, out Formula modify)
		{
			if (!unitFormula.IsUnit(true)) throw new FormulaAssertionException("対象の数式を単位扱い数式として扱えません。");

			Formula result = unitFormula.Copy();
			modify = unitFormula.Copy();

			var nullPrefix = new NullPrefix();
			foreach (var unit in unitFormula.GetExistFactor<Unit>())
			{
				Unit noPrefixUnit = new Unit(unit.UnitName, nullPrefix);
				result = result.Substitute(unit, noPrefixUnit);

				var prefixRemoved = unit.RemovePrefix();
				modify = modify.Substitute(unit, prefixRemoved);
			}
			foreach (var unit in modify.GetExistFactor<Unit>())
				modify = modify.Substitute(unit, 1);
			modify = modify.Simplify();

			return result.Simplify();
		}

		/// <summary>
		/// 単位変換表の変換倍率を設定して変換表を補正します。
		/// </summary>
		/// <param name="from">変換元の単位（補正対象）</param>
		/// <param name="to">変換先の単位</param>
		/// <param name="ratio">変換倍率</param>
		/// <example>(g, t, 10^-6) （1[g]は10^-6[t]）</example>
		public void SetConversionRatio(Formula from, Formula to, Formula ratio)
		{
			if (ratio is Null) throw new FormulaAssertionException("変換率に空白数式を指定することはできません。");
			if (ratio == 0) throw new FormulaAssertionException("変換率に0を指定することはできません。");

			Formula currentConvert = GetConversionRatio(from, to);
			if (currentConvert == null) return;
			foreach (Numeric n in currentConvert.GetExistFactor<Numeric>()) n.Precision = 100; // 設定後の有効桁数に合わせるため、無限を設定

			UnitConvertRecord fromData = GetRecordOf(from);
			UnitConvertRecord toData = GetRecordOf(to);

			if (from == BaseUnit.ConvertUnit && toData.ConvertUnit == to) // 対象が基準単位そのものならば、誤差が生じないよう、設定値をそのまま反映する
			{
				toData.ConversionRatio = (1 / ratio).Combine();
			}
			else if (to == BaseUnit.ConvertUnit && fromData.ConvertUnit == from) // 対象が基準単位そのものならば、誤差が生じないよう、設定値をそのまま反映する
			{
				fromData.ConversionRatio = ratio.Combine();
			}
			else if (fromData is BaseUnitConvertRecord) // 変換元が基準の場合は例外的に変換先を直す
			{
				Formula modif = currentConvert / ratio;
				Formula preConversionRatio = toData.ConversionRatio;
				foreach (Numeric n in preConversionRatio.GetExistFactor<Numeric>()) n.Precision = 100; // 設定後の有効桁数に合わせるため、無限を設定
				toData.ConversionRatio = (modif * preConversionRatio).Combine();
			}
			else
			{
				Formula modif = ratio / currentConvert;
				Formula preConversionRatio = fromData.ConversionRatio;
				foreach (Numeric n in preConversionRatio.GetExistFactor<Numeric>()) n.Precision = 100; // 設定後の有効桁数に合わせるため、無限を設定
				fromData.ConversionRatio = (modif * preConversionRatio).Combine();
			}
		}

		/// <summary>
		/// 指定単位間の変換倍率を取得します。変換できない場合、nullを返します。
		/// </summary>
		/// <param name="from">変換元単位</param>
		/// <param name="to">変換後単位</param>
		/// <returns></returns>
		/// <example>(g, t) → 10^-6 （1[g]は10^-6[t]）</example>
		public Formula GetConversionRatio(Formula from, Formula to)
		{
			if (!from.IsUnit(true) || !to.IsUnit(true)) throw new FormulaAssertionException("変換元、変換先の数式のいずれかが単位として認識できません。");

			Formula fromModify;
			UnitConvertRecord fromData = GetRecordOf(from, out fromModify);
			Formula toModify;
			UnitConvertRecord toData = GetRecordOf(to, out toModify);
			if (fromData == null || toData == null) return null;

			Formula fromConvert = fromData.ConversionRatio.Copy() * fromModify;
			Formula toConvert = toData.ConversionRatio.Copy() * toModify;

			Formula convert = fromConvert / toConvert;
			Formula result = convert.Combine();
			foreach (Numeric n in result.GetExistFactor<Numeric>())
				if (n.Precision > ValidReal.MaxPrecision) n.Precision = 100;

			if (from == to)
			{
#if DEBUG
//				if (!(result is Numeric) || Math.Round(result, Math.Min((result as Numeric).Precision + 1, ValidReal.MaxPrecision)) != 1) throw new FormulaAssertionException("同単位間の変換倍率が1となりませんでした。");
#endif
				return 1;
			}
			else return result;
		}

		/// <summary>
		/// 単位変換表の単位変換加算値（変換後単位）を設定して変換表を補正します。
		/// </summary>
		/// <param name="from">変換元の単位（補正対象）</param>
		/// <param name="to">変換先の単位</param>
		/// <param name="addition">加算値（変換後単位値）</param>
		/// <param name="modifyFrom">変換元を修正する場合true, 変換先を修正する場合false</param>
		public void SetConvertAddition(Formula from, Formula to, Formula addition, bool modifyFrom)
		{
			if (addition is Null) throw new FormulaAssertionException("加算値に空白数式を指定することはできません。");

			UnitConvertRecord fromData = GetRecordOf(from);
			UnitConvertRecord toData = GetRecordOf(to);

			Formula convertToBase = GetConversionRatio(to, BaseUnit.ConvertUnit);
			foreach (Numeric n in convertToBase.GetExistFactor<Numeric>()) n.Precision = 100; // 設定後の有効桁数に合わせるため、無限を設定
			Formula delta = addition * convertToBase; // 基準単位による設定加算値

			Formula fromConvertAdd = fromData.ConversionAddition;
			Formula toConvertAdd = toData.ConversionAddition;
			foreach (Numeric n in fromConvertAdd.GetExistFactor<Numeric>()) n.Precision = 100; // 設定後の有効桁数に合わせるため、無限を設定
			foreach (Numeric n in toConvertAdd.GetExistFactor<Numeric>()) n.Precision = 100; // 設定後の有効桁数に合わせるため、無限を設定

			if ((!(toData is BaseUnitConvertRecord) && !modifyFrom) || fromData is BaseUnitConvertRecord)
				toData.ConversionAddition = (fromConvertAdd - delta).Combine();
			else
				fromData.ConversionAddition = (toConvertAdd + delta).Combine();
		}

		/// <summary>
		/// 指定単位間の変換の際の加算値（変換後の単位値）を取得します。
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public Formula GetConvertAddition(Formula from, Formula to)
		{
			if (!from.IsUnit(true) || !to.IsUnit(true)) throw new FormulaAssertionException("変換元、変換先の数式のいずれかが単位として認識できません。");

			Formula fromModify;
			UnitConvertRecord fromData = GetRecordOf(from, out fromModify);
			Formula toModify;
			UnitConvertRecord toData = GetRecordOf(to, out toModify);
			if (fromData == null || toData == null) return null;

			Formula fromConvert = fromData.ConversionAddition * fromModify; // 基準単位
			Formula toConvert = toData.ConversionAddition * toModify; // 基準単位
			Formula addition = (fromConvert - toConvert) / toData.ConversionRatio; // 変換後単位
			Formula result = addition.Combine();
			foreach (Numeric n in result.GetExistFactor<Numeric>()) if (n.Precision > ValidReal.MaxPrecision) n.Precision = 100;

			if (from == to)
			{
#if DEBUG
				if (!(result is Numeric) || Math.Round(result, Math.Min((result as Numeric).Precision + 1, ValidReal.MaxPrecision)) != 0) throw new FormulaAssertionException("同単位間の変換加算が0となりませんでした。");
#endif
				return 0;
			}
			else return result;
		}

		public override string ToString()
		{
			return "UnitConvertTable:" + UnitType + "|" + TypeComment;
		}

	}
}
