using Liffom.Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Liffom.Reals;
using Liffom.Formats.Numerics;

namespace LiffomTestProject
{
    
    
    /// <summary>
    ///NumericTest のテスト クラスです。すべての
    ///NumericTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class NumericTest
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
		///GetText のテスト
		///</summary>
        [TestCategory("出力書式"), TestMethod()]
		public void GetTextTest()
		{
			string actual;
			Numeric target = new Numeric("53000.02");

			var digit = new ConsiderDigitFormatProperty(); // 有効数値考慮表記
			var radixPoint = new RadixPointFormatProperty(); // 小数点表記
			var split = new SplitFormatProperty(); // 3桁区切りの表記

			{
				digit = true;
				radixPoint = RadixPointFormatProperty.RadixPointType.Comma;
				split = SplitFormatProperty.SplitType.Comma;
				target.Format.SetProperty(digit);
				target.Format.SetProperty(radixPoint);
				target.Format.SetProperty(split);

				actual = target.GetText();
				Assert.AreEqual("5,300002E+4", actual);
			}{
				digit = true;
				radixPoint = RadixPointFormatProperty.RadixPointType.Period;
				split = SplitFormatProperty.SplitType.Space;
				target.Format.SetProperty(digit);
				target.Format.SetProperty(radixPoint);
				target.Format.SetProperty(split);

				actual = target.GetText();
				Assert.AreEqual("5.300 002E+4", actual);
			}{
				digit = false;
				radixPoint = RadixPointFormatProperty.RadixPointType.Period;
				split = SplitFormatProperty.SplitType.Space;
				target.Format.SetProperty(digit);
				target.Format.SetProperty(radixPoint);
				target.Format.SetProperty(split);

				actual = target.GetText();
				Assert.AreEqual("53 000.02", actual);
			}{
				digit = false;
				radixPoint = RadixPointFormatProperty.RadixPointType.Period;
				split = SplitFormatProperty.SplitType.Comma;
				target.Format.SetProperty(digit);
				target.Format.SetProperty(radixPoint);
				target.Format.SetProperty(split);

				actual = target.GetText();
				Assert.AreEqual("53,000.02", actual);
			}


		}
	}
}
