using GoodSeat.Liffom.Deforms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///DeformTokenTest のテスト クラスです。すべての
    ///DeformTokenTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class DeformTokenTest
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
		/// 数式変形のテストを行います。
		/// </summary>
		/// <param name="token">変形トークン</param>
		/// <param name="target">変形対象数式</param>
		/// <param name="expected">期待される数式</param>
		public static void DeformTest(DeformToken token, string target, string expected)
		{
			DeformTest(token, Formula.Parse(target), Formula.Parse(expected));
		}

		/// <summary>
		/// 数式変形のテストを行います。
		/// </summary>
		/// <param name="token">変形トークン</param>
		/// <param name="target">変形対象数式</param>
		/// <param name="expected">期待される数式</param>
		public static void DeformTest(DeformToken token, Formula target, string expected)
		{
			DeformTest(token, target, Formula.Parse(expected));
		}

		/// <summary>
		/// 数式変形のテストを行います。
		/// </summary>
		/// <param name="token">変形トークン</param>
		/// <param name="target">変形対象数式</param>
		/// <param name="expected">期待される数式</param>
		public static void DeformTest(DeformToken token, Formula target, Formula expected)
		{
			string baseText = target.ToString();
			Console.WriteLine(baseText + "の変形 -------------------------------------------------");

			DeformHistory history;
			Formula actual = target.DeformFormula(token, out history);
#if DEBUG
			Console.WriteLine(history.GetSimpleHistoryText());
#endif
			Console.WriteLine(baseText + "の変形結果 ------------------------------------------");
			Console.WriteLine(" → " + actual.ToString());
			Console.WriteLine("  = " + expected.ToString() + "?");
			Console.WriteLine("===========================================");

			Assert.AreEqual(expected, actual);
		}
	}
}
