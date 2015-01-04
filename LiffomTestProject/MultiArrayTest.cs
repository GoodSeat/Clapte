using Liffom.Formulas.Array;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LiffomTestProject
{
    
    
    /// <summary>
    ///MultiArrayTest のテスト クラスです。すべての
    ///MultiArrayTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class MultiArrayTest
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
		///GetNextIndex のテスト
		///</summary>
		[TestCategory("配列"), TestMethod()]
		[DeploymentItem("Liffom.dll")]
		public void GetNextIndexTest()
		{
			MultiArray_Accessor target = new MultiArray_Accessor("Test", 3);
			target.SetSize(1, 4);
			target.SetSize(2, 5);
			target.SetSize(3, 3);
			int dimension = 1;
			int[] index = new int[3];
			index[0] = 1;
			index[1] = 1;
			index[2] = 1;
			int[] indexExpected = new int[3];
			indexExpected[0] = 4;
			indexExpected[1] = 5;
			indexExpected[2] = 3;

			bool actual = true;
			int count = 0;
			while (actual)
			{
				count++;
				actual = target.GetNextIndex(dimension, ref index);
			}

			CollectionAssert.AreEqual(indexExpected, index);
			Assert.AreEqual(60, count);
			// Assert.Inconclusive("このテストメソッドの正確性を確認します。");
		}

		/// <summary>
		///GetIndexFromSerialIndex のテスト
		///</summary>
		[TestCategory("配列"), TestMethod()]
		[DeploymentItem("Liffom.dll")]
		public void GetIndexFromSerialIndexTest()
		{
			MultiArray_Accessor target = new MultiArray_Accessor("Test", 3); // TODO: 適切な値に初期化してください
			target.SetSize(1, 4);
			target.SetSize(2, 5);
			target.SetSize(3, 3);
			int serialIndex = 59; // TODO: 適切な値に初期化してください
			ArrayIndex expected = new ArrayIndex(3); // TODO: 適切な値に初期化してください
			expected.SetPosition(1, 3);
			expected.SetPosition(2, 5);
			expected.SetPosition(3, 3);

			ArrayIndex actual;
			actual = target.GetIndexFromSerialIndex(serialIndex);
			Assert.AreEqual(expected, actual);
//			Assert.Inconclusive("このテストメソッドの正確性を確認します。");
		}
	}
}
