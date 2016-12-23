using GoodSeat.Clapte.Solvers.Processes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.ClapteTestProject
{
    
    
    /// <summary>
    ///DetectSplitCommmaProcessTest のテスト クラスです。すべての
    ///DetectSplitCommmaProcessTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class DetectSplitCommmaProcessTest
	{


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
		///CheckInputText のテスト
		///</summary>
		[TestMethod()]
		public void CheckInputTextTest()
		{
			DetectSplitCommmaProcess target = new DetectSplitCommmaProcess(null);
			{
				string input = "1,352,012.2525 + 1,235.5";
				string inputExpected = "1352012.2525 + 1235.5";
				Error expected = null;
				Error actual = target.CheckInputText(ref input, false);
				Assert.AreEqual(inputExpected, input);
				Assert.AreEqual(expected, actual);
			}
			{
				string input = "1,52,012.2525 + 1,235.5";
				string inputExpected = "1,52012.2525 + 1235.5";
				Error expected = null;
				Error actual = target.CheckInputText(ref input, false);
				Assert.AreEqual(inputExpected, input);
				Assert.AreEqual(expected, actual);
			}
			{
				string input = "1,352,012";
				string inputExpected = "1352012";
				Error expected = null;
				Error actual = target.CheckInputText(ref input, false);
				Assert.AreEqual(inputExpected, input);
				Assert.AreEqual(expected, actual);
			}
		}

		/// <summary>
		///CheckOutputFormula のテスト
		///</summary>
		[TestMethod()]
		public void CheckOutputFormulaTest()
		{
			DetectSplitCommmaProcess target = new DetectSplitCommmaProcess(null);
			{
				string input = "1,352,012.2525 + 1,235.5";
				target.CheckInputText(ref input, false);

				Formula output = new Numeric(1352012.2525);
				Error actual = target.CheckOutputFormula(ref output);
				Assert.AreEqual("1,352,012.2525", output.ToString());
			}
			{
				string input = "1352012.2525 + 1235.5";
				target.CheckInputText(ref input, false);

				Formula output = new Numeric(1352012.2525);
				Error actual = target.CheckOutputFormula(ref output);
				Assert.AreEqual("1352012.2525", output.ToString());
			}
		}
	}
}
