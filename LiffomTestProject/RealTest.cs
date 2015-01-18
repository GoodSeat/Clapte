using GoodSeat.Liffom.Reals;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///RealTest のテスト クラスです。すべての
    ///RealTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class RealTest
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
		///Round のテスト
		///</summary>
		[TestCategory("数値"), TestMethod()]
		public void RoundTest()
		{
			double d = 0F;
			int decimals = 0;
			double expected = 0F;
			for (int i = 0; i < 4; i++)
			{
				switch (i)
				{
					case 0:
						d = 0.000052138;
						decimals = 6;
						expected = 0.000052;
						break;
					case 1:
						d = 0.6;
						decimals = 0;
						expected = 1;
						break;
					case 2:
						d = 2359.1235;
						decimals = -2;
						expected = 2400;
						break;
					case 3:
						d = 2.3591235E-20;
						decimals = 22;
						expected = 2.36E-20;
						break;
				}
				double actual;
				actual = Real.Round(d, decimals);
				Assert.AreEqual(expected, actual);
			}
		}
	}
}
