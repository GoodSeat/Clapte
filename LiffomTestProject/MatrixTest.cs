using Liffom.Formulas.Matrices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Liffom.Formulas;

namespace LiffomTestProject
{
    
    
    /// <summary>
    ///MatrixTest のテスト クラスです。すべての
    ///MatrixTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class MatrixTest
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
		/// Transposed のテスト
		/// </summary>
		[TestCategory("マトリクス"), TestMethod()]
		public void TransposedTest()
		{
			Matrix target = Formula.Parse("{[1,2], [3,4], [5,6]}") as Matrix;
			Matrix expected = Formula.Parse("{[1,3,5], [2,4,6]}") as Matrix;
			Matrix actual;
			actual = target.Transposed();
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// Simplify のテスト
		/// </summary>
		[TestCategory("マトリクス"), TestMethod()]
		public void SimplifyTest()
		{
			var token = Formula.SimplifyToken;

			var A = Formula.Parse("{[1,2], [3,4]}") as Matrix;
			var B = Formula.Parse("{[3,a], [1,c]}") as Matrix;
			var a = new Variable("A");
			var b = new Variable("B");
			DeformTokenTest.DeformTest(token, Formula.Parse("A.A").Substitute(a, A), "{[7, 10], [15, 22]}");
			DeformTokenTest.DeformTest(token, Formula.Parse("A^^2.A^^3").Substitute(a, A), "{[1069, 1558], [2337, 3406]}");
			DeformTokenTest.DeformTest(token,
					Formula.Parse("(A+B).A").Substitute(a, A).Substitute(b, B),
					"{[3a + 10, 4a + 16], [3c + 16, 4c + 24]}");
		}

		/// <summary>
		/// IsDiagonal のテスト
		/// </summary>
		[TestCategory("マトリクス"), TestMethod()]
		public void IsDiagonalTest()
		{
			Matrix target = Formula.Parse("{[1, 0, 0], [0, a, 0], [0, 0, b]}") as Matrix;
			Assert.IsTrue(target.IsDiagonal);

			target = Formula.Parse("{[1, 0, 0], [0, a, 0], [0, 1, b]}") as Matrix;
			Assert.IsFalse(target.IsDiagonal);
		}

		/// <summary>
		/// IsLowerTriangular のテスト
		/// </summary>
		[TestCategory("マトリクス"), TestMethod()]
		public void IsLowerTriangularTest()
		{
			Matrix target = Formula.Parse("{[1, 0, 0], [0, a, 0], [0, 2, b]}") as Matrix;
			Assert.IsTrue(target.IsLowerTriangular);

			target = Formula.Parse("{[1, 0, 0], [0, a, 1], [0, 0, b]}") as Matrix;
			Assert.IsFalse(target.IsLowerTriangular);
		}

		/// <summary>
		///IsSquareMatrix のテスト
		///</summary>
		[TestCategory("マトリクス"), TestMethod()]
		public void IsSquareMatrixTest()
		{
			Matrix target = Formula.Parse("{[1, 0, 0], [0, a, 0], [0, 2, b]}") as Matrix;
			Assert.IsTrue(target.IsSquareMatrix);

			target = Formula.Parse("{[1, 0, 0], [0, a, 1]}") as Matrix;
			Assert.IsFalse(target.IsSquareMatrix);
		}

		/// <summary>
		///IsUpperTriangular のテスト
		///</summary>
		[TestCategory("マトリクス"), TestMethod()]
		public void IsUpperTriangularTest()
		{
			Matrix target = Formula.Parse("{[1, 0, 0], [0, a, 0], [0, 2, b]}") as Matrix;
			Assert.IsFalse(target.IsUpperTriangular);

			target = Formula.Parse("{[1, 0, 0], [0, a, 1], [0, 0, b]}") as Matrix;
			Assert.IsTrue(target.IsUpperTriangular);
		}

		/// <summary>
		///IsSymmetric のテスト
		///</summary>
		[TestCategory("マトリクス"), TestMethod()]
		public void IsSymmetricTest()
		{
			Matrix target = Formula.Parse("{[1, 0, 0], [0, a, 2], [0, 2, b]}") as Matrix;
			Assert.IsTrue(target.IsSymmetric);

			target = Formula.Parse("{[1, 0, 0], [0, a, 1], [0, 0, b]}") as Matrix;
			Assert.IsFalse(target.IsSymmetric);
		}
	}
}
