using GoodSeat.Liffom.Processes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom;
using System.Collections.Generic;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///SquareFreeDecompositionTest のテスト クラスです。すべての
    ///SquareFreeDecompositionTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class SquareFreeDecompositionTest
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
		///OnDo のテスト
		///</summary>
		[TestCategory("微分"), TestCategory("多項式"), TestCategory("プロセス"), TestMethod()]
		[DeploymentItem("Liffom.dll")]
		public void OnDoTest()
		{
			SquareFreeDecomposition process = new SquareFreeDecomposition();
			var userStates = new List<object>();

			ProcessTest.AddTestCaseAsync(process, "3*(x+2)*(x^2-1)^3", userStates,
				"3*x^7 + 6*x^6 - 9*x^5 - 18*x^4 + 9*x^3 + 18*x^2 - 3*x - 6");

			ProcessTest.CheckAssertionTestCase(process, userStates);
		}
	}
}
