using Liffom.Formulas.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Liffom.Formulas;

namespace LiffomTestProject
{
    
    
    /// <summary>
    ///UnitConvertTableTest のテスト クラスです。すべての
    ///UnitConvertTableTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class UnitConvertTableTest
	{
		static object s_lockObject = new object();
		/// <summary>
		/// 基本的な単位テーブルを登録します。
		/// </summary>
		public static void RegistBasicUnitRecord()
		{
			lock (s_lockObject)
			{
				if (!ExistTableOf("force"))
				{
					UnitConvertTable forceTable = new UnitConvertTable(new Unit("N"), "Force");
					UnitConvertRecord defineNRecord = new UnitConvertRecord(Formula.Parse("[kg*m/s^2]"), 1.0);
					forceTable.AddRecord(defineNRecord);
					UnitConvertRecord gfRecord = new UnitConvertRecord(new Unit("kgf"), Formula.Parse("9.80665"));
					forceTable.AddRecord(gfRecord);
					UnitConvertTable.AddTable(forceTable);
				}
				if (!ExistTableOf("time"))
				{
					UnitConvertTable timeTable = new UnitConvertTable(new Unit("s"), "Time");
				}
				if (!ExistTableOf("angle"))
				{
					UnitConvertTable angleTable = new UnitConvertTable(new Unit("rad"), "Angle");
					UnitConvertRecord degRecord = new UnitConvertRecord(new Unit("deg"), Formula.Parse("pi/180"));
					angleTable.AddRecord(degRecord);
					UnitConvertRecord noDimentionRecord = new UnitConvertRecord(1, 1);
					angleTable.AddRecord(noDimentionRecord);
					UnitConvertTable.AddTable(angleTable);
				}
				if (!ExistTableOf("length"))
				{
					UnitConvertTable lengthTable = new UnitConvertTable(new Unit("m"), "Length");
					UnitConvertTable.AddTable(lengthTable);
				}
				if (!ExistTableOf("pressure"))
				{
					UnitConvertTable pressureTable = new UnitConvertTable(new Unit("Pa"), "Pressure");
					UnitConvertRecord definePaRecord = new UnitConvertRecord(Formula.Parse("[N/(m^2)]"), 1);
					pressureTable.AddRecord(definePaRecord);
					UnitConvertRecord atmRecord = new UnitConvertRecord(new Unit("atm"), 101325);
					pressureTable.AddRecord(atmRecord);
					UnitConvertTable.AddTable(pressureTable);
				}
				if (!ExistTableOf("volume"))
				{
					UnitConvertTable volumeTable = new UnitConvertTable(Formula.Parse("[m^3]"), "Volume");
					UnitConvertRecord littleRecord = new UnitConvertRecord(new Unit("l"), 0.001);
					volumeTable.AddRecord(littleRecord);
					UnitConvertTable.AddTable(volumeTable);
				}
				if (!ExistTableOf("energy"))
				{
					UnitConvertTable energyTable = new UnitConvertTable(new Unit("J"), "Energy");
					UnitConvertRecord defineJRecord = new UnitConvertRecord(Formula.Parse("[N*m]"), 1);
					energyTable.AddRecord(defineJRecord);
					UnitConvertTable.AddTable(energyTable);
				}
			}
		}

		/// <summary>
		/// 指定した名前の単位変換テーブルが存在するか否かを取得します。
		/// </summary>
		/// <param name="unitType">判定対象のテーブル名。</param>
		/// <returns>判定対象の単位変換テーブルが存在するか。</returns>
		private static bool ExistTableOf(string unitType)
		{
			foreach (var table in UnitConvertTable.ValidTables.Values)
			{
				if (table.UnitType.ToLower() ==unitType) return true;
			}
			return false;
		}


		private TestContext testContextInstance;

		/// <summary>
		///現在のテストの実行についての情報および機能を
		///提供するテスト コンテキストを取得または設定します。
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region 追加のテスト属性
		// 
		//テストを作成するときに、次の追加属性を使用することができます:
		//
		//クラスの最初のテストを実行する前にコードを実行するには、ClassInitialize を使用
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//クラスのすべてのテストを実行した後にコードを実行するには、ClassCleanup を使用
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//各テストを実行する前にコードを実行するには、TestInitialize を使用
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//各テストを実行した後にコードを実行するには、TestCleanup を使用
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///GetRecordOf のテスト
		///</summary>
		[TestCategory("単位変換"), TestMethod()]
		public void GetRecordOfTest()
		{
			UnitConvertTableTest.RegistBasicUnitRecord();

			var target = UnitConvertTable.ValidTables["Force"];

			Formula formula, modify;
			UnitConvertRecord actual;

			formula = Formula.Parse("[gf]");
			actual = target.GetRecordOf(formula, out modify);
			Assert.AreEqual(Formula.Parse("1000^-1"), modify);
			Assert.AreEqual("kgf", actual.ConvertUnit.ToString());

			formula = Formula.Parse("[g*mm*s^-2]");
			actual = target.GetRecordOf(formula, out modify);
			Assert.AreEqual(Formula.Parse("1000000^-1"), modify);
			Assert.AreEqual(Formula.Parse("[kg*m*(s^2)^-1]"), actual.ConvertUnit);
		}

		/// <summary>
		///GetPrefixRemovedUnitFormula のテスト
		///</summary>
		[TestCategory("単位変換"), TestMethod()]
		[DeploymentItem("Liffom.dll")]
		public void GetPrefixRemovedUnitFormulaTest()
		{
			UnitConvertTable_Accessor target = new UnitConvertTable_Accessor();
			Formula unitFormula = Formula.Parse("[kN/cm]");
			Formula modify = null;
			Formula modifyExpected = 100000;
			Formula expected = Formula.Parse("[N/m]");
			Formula actual;
			actual = target.GetPrefixRemovedUnitFormula(unitFormula, out modify);
			Assert.AreEqual(modifyExpected, modify);
			Assert.AreEqual(expected, actual);
		}
	}
}
