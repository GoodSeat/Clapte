using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Sio.Xml.Serialization;
using GoodSeat.Sio.Xml;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Formulas.Constants;

namespace GoodSeat.Clapte.ViewModels
{
	/// <summary>
	/// 単位変換テーブルのビューモデルを表します。
	/// </summary>
	public class UnitConvertTableViewModel : ISerializable
	{
		/// <summary>
		/// 単位変換テーブルのビューモデルを初期化します。
		/// </summary>
		public UnitConvertTableViewModel() { }

		/// <summary>
		/// 単位変換テーブルのビューモデルを初期化します。
		/// </summary>
		/// <param name="target">対象とする単位変換テーブル。</param>
		public UnitConvertTableViewModel(UnitConvertTable target)
		{
			Target = target;
		}

		/// <summary>
		/// 対象の単位変換テーブルを設定もしくは取得します。
		/// </summary>
		public UnitConvertTable Target { get; set; }

		/// <summary>
		/// 指定したXmlElementオブジェクトから、単位変換テーブルを復元します。
		/// </summary>
		/// <param name="xmlElement">復元元のXmlElementオブジェクト。</param>
		public void OnDeserialize(XmlElement xmlElement)
		{
			Target = new UnitConvertTable();
			Target.UnitType = xmlElement.GetAttribute("Type");
			Target.TypeComment = xmlElement.GetAttribute("Comment", "");

			Target.BaseUnit = GetDeserializeUnitConvertRecord(xmlElement["BaseRecord"], true) as BaseUnitConvertRecord;

			foreach (XmlElement record in xmlElement.GetElements("Records\\Record"))
				Target.AddRecord(GetDeserializeUnitConvertRecord(record, false));
		}

		/// <summary>
		/// 対象の単位変換テーブルを、指定したXmlElementオブジェクトに保存します。
		/// </summary>
		/// <param name="xmlElement">保存先とするXmlElementオブジェクト。</param>
		public void OnSerialize(XmlElement xmlElement)
		{
			xmlElement.AddAttribute("Type", Target.UnitType);
			xmlElement.AddAttribute("Comment", Target.TypeComment);

			var baseElement = GetUnitConvertRecordSerialized(Target.BaseUnit, "BaseRecord");
			xmlElement.AddElements(baseElement);

			XmlElement records = new XmlElement("Records");
			xmlElement.AddElements(records);
			foreach (UnitConvertRecord record in Target.GetAllRecords(false))
			{
				records.AddElements(GetUnitConvertRecordSerialized(record, "Record"));
			}
		}

		/// <summary>
		/// 指定XmlElementから単位変換レコードを復元して取得します。
		/// </summary>
		/// <param name="xmlElement">復元対象のXmlElementオブジェクト。</param>
		/// <param name="asBaseRecord">基準レコードとして復元するか否か。</param>
		/// <returns>復元された単位変換レコード。</returns>
		private UnitConvertRecord GetDeserializeUnitConvertRecord(XmlElement xmlElement, bool asBaseRecord)
		{
			var record = new UnitConvertRecord();
			if (asBaseRecord) record = new BaseUnitConvertRecord();

			string unit = xmlElement.GetAttribute("Unit", "");
			if (unit == "")
			{
				record.ConvertUnit = new Unit(xmlElement.GetAttribute("UnitName"), Prefix.GetPrefixFrom(xmlElement.GetAttribute("UnitPrefix", "")));
			}
			else
			{
				var convert = Formula.Parse(unit);
				foreach (var variable in convert.GetExistFactor(f => (f is Variable || f is Constant)))
					convert = convert.Substituted(variable, new Unit(variable.ToString()));
				record.ConvertUnit = convert;
			}

			record.UnitComment = xmlElement.GetAttribute("Comment", "");
			record.ConversionAddition = Formula.Parse(xmlElement.GetAttribute("Add", "0"));
			record.ConversionRatio = Formula.Parse(xmlElement.GetAttribute("Multiple", "1"));

			return record;
		}

		/// <summary>
		/// 指定単位変換レコードをシリアライズして取得します。
		/// </summary>
		/// <param name="record">シリアライズ対象の単位変換レコード。</param>
		/// <param name="name">XmlElementに付与する名称。</param>
		/// <returns>シリアライズされたXmlElementオブジェクト。</returns>
		private XmlElement GetUnitConvertRecordSerialized(UnitConvertRecord record, string name)
		{
			XmlElement xmlElement = new XmlElement(name);
			if (record.ConvertUnit is Unit)
			{
				Unit unit = record.ConvertUnit as Unit;
				xmlElement.AddAttribute("UnitName", unit.UnitName);

				if (unit.Prefix.ToString() != "") xmlElement.AddAttribute("UnitPrefix", unit.Prefix.ToString());
			}
			else
			{
				xmlElement.AddAttribute("Unit", record.ConvertUnit.ToString());
			}
			xmlElement.AddAttribute("Comment", record.UnitComment);

			if (record.ConversionAddition.ToString() != "0") xmlElement.AddAttribute("Add", record.ConversionAddition.ToString());
			if (record.ConversionRatio.ToString() != "1") xmlElement.AddAttribute("Multiple", record.ConversionRatio.ToString());

			return xmlElement;
		}


	}
}
