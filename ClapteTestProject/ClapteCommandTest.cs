// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using GoodSeat.Clapte.ViewModels.ClapteCommands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GoodSeat.ClapteTestProject
{
    
    
    /// <summary>
    ///ClapteCommandTest のテスト クラスです。すべての
    ///ClapteCommandTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class ClapteCommandTest
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
		///RemoveCommentTextFrom のテスト
		///</summary>
		[TestMethod()]
		public void RemoveCommentTextFromTest()
		{
			string text = "Line 1 This Line Contains No Comment.\r\nLine 2 This Line Contains Comment. // Yes, This is Comment\r\n#Line 3 This Line is All Comment.";
			string expected = "Line 1 This Line Contains No Comment.\r\nLine 2 This Line Contains Comment. \r\n";
			string actual;
			actual = ClapteCommand.RemoveCommentTextFrom(text);
			Assert.AreEqual(expected, actual);

			text = "Line 1 This Line Contains No Comment.\r\nLine 2 This Line Contains Comment. // Yes, This is Comment\r\n#Line 3 This Line is All Comment.\r\nLine 4";
			expected = "Line 1 This Line Contains No Comment.\r\nLine 2 This Line Contains Comment. \r\n\r\nLine 4";
			actual = ClapteCommand.RemoveCommentTextFrom(text);
			Assert.AreEqual(expected, actual);
		}
	}
}
