// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using GoodSeat.Clapte.ViewModels.ClapteCommands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.ViewModels;

namespace GoodSeat.ClapteTestProject
{
    
    
    /// <summary>
    ///DefineConstantCommandTest のテスト クラスです。すべての
    ///DefineConstantCommandTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class DefineConstantCommandTest
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
		[DeploymentItem("Clapte.exe")]
		public void PickDefineFromTest()
		{
			DefineConstantCommand target = new DefineConstantCommand(new ClapteCoreViewModel());
			ConstantDefine actual;

			actual = target.PickDefineFrom(@"   test := a + b【テスト変数】 // テスト");
			Assert.AreEqual("test", actual.Name);
			Assert.AreEqual("a + b", actual.Define);
			Assert.AreEqual("テスト変数", actual.Information);

			actual = target.PickDefineFrom(@"   test := a + b ");
			Assert.AreEqual("test", actual.Name);
			Assert.AreEqual("a + b", actual.Define);
			Assert.AreEqual("", actual.Information);

			actual = target.PickDefineFrom(@"   test(a, b) := a + b【テスト変数】 // テスト");
			Assert.AreEqual(null, actual);
		}
	}
}
