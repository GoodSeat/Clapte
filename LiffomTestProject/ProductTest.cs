using Liffom.Formulas.Operators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Liffom;
using Liffom.Deforms;

namespace LiffomTestProject
{
    
    
    /// <summary>
    ///ProductTest のテスト クラスです。すべての
    ///ProductTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class ProductTest
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
		/// ProductのCombine のテスト
		/// </summary>
		[TestCategory("変形"), TestMethod()]
		public void CombineOfProductTest()
		{
			var token = new CombineToken();
			DeformTokenTest.DeformTest(token, "5[kN] * 100[cm]", "500[kN*cm]");
			DeformTokenTest.DeformTest(token, "5*a[km/h] * (b[h])", "(5*a*b)[km]");
			DeformTokenTest.DeformTest(token, "5[kN] * 1[cm] * 1[m]", "500[kN*cm^2]");
		}
	}
}
