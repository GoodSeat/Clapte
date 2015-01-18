using GoodSeat.Liffom.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom.Formulas.Matrices;
using GoodSeat.Liffom.Formulas;
using System.Collections.Generic;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///MatrixAlgebraTest のテスト クラスです。すべての
    ///MatrixAlgebraTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class MatrixAlgebraTest
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
		/// Determinant のテスト
		/// </summary>
		[TestCategory("マトリクス"), TestMethod()]
		public void DeterminantTest()
		{
			// 行列式
			Matrix m = Formula.Parse("{[-1, 2, 1], [0, 1, 3], [1, 2, -1]}") as Matrix;
			Assert.AreEqual(12, m.Determinant());

			m = Formula.Parse("{[1+a, 1, 1], [1, 1+a, 1], [1, 1, 1+a]}") as Matrix;
			Assert.AreEqual(Formula.Parse("a^3 + 3*a^2"), m.Determinant());
		}

		/// <summary>
		/// Cofactor のテスト
		/// </summary>
		[TestCategory("マトリクス"), TestMethod()]
		public void CofactorTest()
		{
			// 余因子
			Matrix m = Formula.Parse("{[1+a, 1, 1], [1, 1+a, 1], [1, 1, 1+a]}") as Matrix;
			Assert.AreEqual(Formula.Parse("a^2 + 2a"), m.Cofactor(1, 1));
			Assert.AreEqual(Formula.Parse("-a"), m.Cofactor(1, 2));
			Assert.AreEqual(Formula.Parse("-a"), m.Cofactor(1, 3));
			Assert.AreEqual(Formula.Parse("-a"), m.Cofactor(2, 1));
			Assert.AreEqual(Formula.Parse("a^2 + 2a"), m.Cofactor(2, 2));
		}

		/// <summary>
		///LUdecomposition のテスト
		///</summary>
		[TestCategory("マトリクス"), TestMethod()]
		public void LUdecompositionTest()
		{
			Matrix A = Formula.Parse("{[1, -1, 2], [2, 3, 1], [-1, 4, 4]}") as Matrix;
			List<int> swap = null;
			int toggle = 0;
			Matrix expected = Formula.Parse("{[1, -1, 2], [2, 5, -3], [-1, 3/5, 39/5]}") as Matrix;
			Matrix actual;
			actual = MatrixAlgebra.DecomposeLU(A, out swap, out toggle);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///SolveVector のテスト
		///</summary>
		[TestCategory("マトリクス"), TestMethod()]
		public void SolveVectorTest()
		{
			// 3x + 2y +  z = 10
			//  x +  y +  z = 6
			//  x + 2y + 2z = 11

			Matrix A = Formula.Parse("{[3, 2, 1], [1, 1, 1], [1, 2, 2]}") as Matrix;
			Vector B = Formula.Parse("{10, 6, 11}") as Vector;
			Vector expected = Formula.Parse("{1, 2, 3}") as Vector;
			Vector actual = MatrixAlgebra.SolveVector(A, B);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///Inverse のテスト
		///</summary>
		[TestCategory("マトリクス"), TestMethod()]
		public void InverseTest()
		{
			Matrix m = Formula.Parse("{[1, 2], [3, 4]}") as Matrix;
			Matrix expected = Formula.Parse("{[-2, 1], [3/2, -1/2]}") as Matrix;
			Matrix actual = MatrixAlgebra.Inverse(m);
			Assert.AreEqual(expected, actual);
		}

	}
}
