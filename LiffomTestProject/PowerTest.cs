using Liffom.Formats.Powers;
using Liffom.Formulas.Operators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Liffom;
using Liffom.Deforms;
using Liffom.Formulas;

namespace LiffomTestProject
{
    
    
    /// <summary>
    ///PowerTest のテスト クラスです。すべての
    ///PowerTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class PowerTest
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
		/// 累乗のCombine のテスト
		/// </summary>
		[TestCategory("変形"), TestMethod()]
		public void CombineOfPowerTest()
		{
			var token = new SimplifyToken();
			DeformTokenTest.DeformTest(token, "(5/2)^(3/2)", "5/4*sqrt(10)");
			DeformTokenTest.DeformTest(token, "((5/3)^(1/2))^(1/3)", "root(15, 6)*root(9, 3)/3");
			DeformTokenTest.DeformTest(token, "(3^(5/3))^-1", "root(3, 3)/9");
			DeformTokenTest.DeformTest(token, "1/(2^(1/2)+3^(1/2)+5^(1/2))", "(2*3^(1/2)+3*2^(1/2)-30^(1/2))/12");
			DeformTokenTest.DeformTest(token, "1/(2^(1/2)+3^(1/2)+5^(1/2)+6^(1/2))", "(6-6*2^(1/2)+5*3^(1/2)+3*5^(1/2)-2*6^(1/2)-3*10^(1/2)+2*15^(1/2)-30^(1/2))/6");
		}


		/// <summary>
		///GetText のテスト
		///</summary>
        [TestCategory("出力書式"), TestMethod()]
		public void GetTextTest()
		{
			Power target = null;
			var division = new DivisionFormatProperty();

			{
				target = Formula.Parse("(1+2)^-4") as Power;

				division = false;
				target.Format.SetProperty(division);
				Assert.AreEqual("(1+2)^-4", target.ToString());

				division = true;
				target.Format.SetProperty(division);
				Assert.AreEqual("1/(1+2)^4", target.ToString());
			}{
				target = Formula.Parse("(1+2)^(-4*a)") as Power;

				division = false;
				target.Format.SetProperty(division);
				Assert.AreEqual("(1+2)^(-4*a)", target.ToString());

				division = true;
				target.Format.SetProperty(division);
				Assert.AreEqual("1/(1+2)^(4*a)", target.ToString());
			}

		}
	}
}
