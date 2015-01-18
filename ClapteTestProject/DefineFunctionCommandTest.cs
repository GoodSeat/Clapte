using GoodSeat.Clapte.ViewModels.ClapteCommands;
using GoodSeat.Liffom.Formulas.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Clapte.ViewModels;
using GoodSeat.Clapte.Solvers;

namespace GoodSeat.ClapteTestProject
{
    
    
    /// <summary>
    ///DefineFunctionCommandTest のテスト クラスです。すべての
    ///DefineFunctionCommandTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class DefineFunctionCommandTest
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
		///PickDefineFrom のテスト
		///</summary>
		[TestMethod()]
		public void PickDefineFromTest()
		{
			DefineFunctionCommand target = new DefineFunctionCommand(new ClapteCoreViewModel());
			FunctionDefine actual;

			actual = target.PickDefineFrom(@"   test(a, b) := a + b【テスト関数 a=数値1 b = 数値2】 // テスト");
			Assert.AreEqual("test", actual.Name);
			Assert.AreEqual("test(a, b)", (actual.Target as UserFunction).NameForView);
			Assert.AreEqual("a + b", actual.Define);
			Assert.AreEqual("テスト関数", actual.Information);
			var argInfos = actual.ArgumentInformation;
			Assert.AreEqual("数値1", argInfos[0]);
			Assert.AreEqual("数値2", argInfos[1]);

			actual = target.PickDefineFrom(@"   test() := a + b ");
			Assert.AreEqual("test", actual.Name);
			Assert.AreEqual("test()", (actual.Target as UserFunction).NameForView);
			Assert.AreEqual("a + b", actual.Define);
			Assert.AreEqual("", actual.Information);

			actual = target.PickDefineFrom(@"   test := a + b【テスト関数】 // テスト");
			Assert.AreEqual(null, actual);
		}
	}
}
