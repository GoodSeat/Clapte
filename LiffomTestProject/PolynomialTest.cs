using Liffom.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Liffom;
using Liffom.Formulas;
using Liffom.Formulas.Functions;

namespace LiffomTestProject
{
    
    
    /// <summary>
    ///PolynomialTest のテスト クラスです。すべての
    ///PolynomialTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class PolynomialTest
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
		/// テストケースを追加します。
		/// </summary>
		/// <param name="actual">実際の結果。</param>
		/// <param name="expected">期待される結果。</param>
		private void AddTestCase(Formula actual, string expected) { Assert.AreEqual(Formula.Parse(expected), actual); }

		/// <summary>
		/// テストケースを追加します。
		/// </summary>
		/// <param name="actual">実際の結果。</param>
		/// <param name="expected">期待される結果。</param>
		private void AddTestCase(Formula actual, Formula expected) { Assert.AreEqual(expected, actual); }

		/// <summary>
		///LeadingCoefficient のテスト
		///</summary>
		[TestCategory("多項式"), TestMethod()]
		public void LeadingCoefficientTest()
		{
			Variable x = new Variable("x");
			AddTestCase(Formula.Parse("a*x^4 + 3*b*x^4 + (3+a)*x^3 + c+b").LeadingCoefficient(x), "a+3*b");
		}

		/// <summary>
		///CoefficientOf のテスト
		///</summary>
		[TestCategory("多項式"), TestMethod()]
		public void CoefficientOfTest()
		{
			Variable x = new Variable("x");
			AddTestCase(Formula.Parse("a*x^4 + 3*b*x^4 + (3+a)*x^3 + c+b").CoefficientOf(x, 4), "a+3*b");
			AddTestCase(Formula.Parse("a*x^4 + 3*b*x^4 + (3+a)*x^3 + c+b").CoefficientOf(x, 3), "3+a");
			AddTestCase(Formula.Parse("a*x^4 + 3*b*x^4 + (3+a)*x^3 + c+b").CoefficientOf(x, 2), "0");
			AddTestCase(Formula.Parse("a*x^4 + 3*b*x^4 + (3+a)*x^3 + c+b").CoefficientOf(x, 1), "0");
			AddTestCase(Formula.Parse("a*x^4 + 3*b*x^4 + (3+a)*x^3 + c+b").CoefficientOf(x, 0), "c+b");
		}

		/// <summary>
		///Degree のテスト
		///</summary>
		[TestCategory("多項式"), TestMethod()]
		public void DegreeTest()
		{
			Variable x = new Variable("x");
			AddTestCase(Formula.Parse("a*x^4 + 3*b*x^4 + (3+a)*x^3 + c+b").Degree(x), "4");
			AddTestCase(Formula.Parse("a*x^n + 3*b*x^4 + (3+a)*x^3 + c+b").Degree(x), "n");

			AtomicFormula sinx = new Sin(x);
			AddTestCase(Formula.Parse("a*sin(x)^4 + 3*b*x^4 + (3+a)*sin(x)^3 + c+b").Degree(sinx), "4");
		}

		/// <summary>
		///PseudoDivide のテスト
		///</summary>
		[TestCategory("多項式"), TestMethod()]
		public void PseudoDivideTest()
		{
			var x = new Variable("x");
			PseudoDivideTest("x^8 + x^6 - 3*x^4 + 4*x^3 - 5*x^2 + 4*x -3", // f
									"3*x^6 - 2*x^4 - 5*x^2 + 7*x - 13", // g
									x,
									"27", // ratio
									"9*x^2 + 15",
									"-6*x^4 + 45*x^3 + 57*x^2 + 3*x + 114");
			PseudoDivideTest("4*x+3", // f
									"2", // g
									x,
									"4", // ratio
									"8*x+6",
									"0");
			PseudoDivideTest("5", // f
									"2", // g
									x,
									"2", // ratio
									"5",
									"0");
			//PseudoDivideTest("x^5 - 3*x^4 + 4*x^3 - 5*x^2 + 4*x -3", // f
			//                        "3*x^6 - 2*x^4 - 5*x^2 + 7*x - 13", // g
			//                        x,
			//                        "1", // ratio
			//                        "0",
			//                        "x^5 - 3*x^4 + 4*x^3 - 5*x^2 + 4*x -3");
		}

		private void PseudoDivideTest(string f, string g, AtomicFormula x, string expectedRatio, string expectedQuo, string expectedRem)
		{
			Formula q, r;
			Formula ratio = Formula.Parse(f).PseudoDivide(Formula.Parse(g), x, out q, out r);

			AddTestCase(ratio, expectedRatio);
			AddTestCase(q, expectedQuo);
			AddTestCase(r, expectedRem);
		}

		/// <summary>
		///Divide のテスト
		///</summary>
		[TestCategory("多項式"), TestMethod()]
		public void DivideTest()
		{
			DivideTest("4*x+3", "2", "2*x+1", "1", false);
			DivideTest("4*x+3", "2", "(4*x+3)/2", "0", true);

			DivideTest("5", "2", "2", "1", false);
			DivideTest("5", "2", "5/2", "0", true);
		}

		private void DivideTest(string f, string g, string expectedQuo, string expectedRem, bool admitFraction)
		{
			Formula r;
			Formula q = Formula.Parse(f).Divide(Formula.Parse(g), out r, admitFraction);

			AddTestCase(q, expectedQuo);
			AddTestCase(r, expectedRem);
		}

		/// <summary>
		///PrimitivePolynomial のテスト
		///</summary>
		[TestCategory("多項式"), TestMethod()]
		public void PrimitivePolynomialTest()
		{
			Variable x = new Variable("x");
			AddTestCase(Formula.Parse("0").PrimitivePolynomial(x), Formula.Parse("0"));
			AddTestCase(Formula.Parse("6").PrimitivePolynomial(x), Formula.Parse("1"));
			AddTestCase(Formula.Parse("-15*x^4 + 3*x^2 + 9").PrimitivePolynomial(x), Formula.Parse("-5*x^4 + x^2 + 3"));
		}

		/// <summary>
		///NumericGCD のテスト
		///</summary>
		[TestCategory("多項式"), TestMethod()]
		public void NumericGCDTest()
		{
			AddTestCase(Polynomial.NumericGCD(new Numeric(15795d), new Numeric(30375d)), 1215d);
			AddTestCase(Polynomial.NumericGCD(new Numeric(3.5), new Numeric(1.5)), 0.5d);
		}

		/// <summary>
		///GCD のテスト
		///</summary>
		[TestCategory("多項式"), TestMethod()]
		public void GCDTest()
		{
			Console.WriteLine(DateTime.Now);

			AddTestCase(Polynomial.GCD(Formula.Parse("6"),
													 Formula.Parse("8*x")),
							 Formula.Parse("2"));
			Console.WriteLine(DateTime.Now);

			AddTestCase(Polynomial.GCD(Formula.Parse("4*x"),
													 Formula.Parse("8*x")),
							 Formula.Parse("4*x"));
			Console.WriteLine(DateTime.Now);

			AddTestCase(Polynomial.GCD(Formula.Parse("4*x"),
													 Formula.Parse("9*x")),
							 Formula.Parse("x"));
			Console.WriteLine(DateTime.Now);

			AddTestCase(Polynomial.GCD(Formula.Parse("2*x^2 + 4"),
													 Formula.Parse("x^2+2")),
							 Formula.Parse("x^2 + 2"));
			Console.WriteLine(DateTime.Now);

			AddTestCase(Polynomial.GCD(Formula.Parse("2*x^2 - 2"),
													 Formula.Parse("3*x^3 + 3*x^2")),
							 Formula.Parse("x + 1"));
			Console.WriteLine(DateTime.Now);

			AddTestCase(Polynomial.GCD(Formula.Parse("x*sin(x)*y + 2*sin(x)*y + x*y + 2*y + x^2*sin(x) + 2*x*sin(x) + x^2 + 2*x"),
													 Formula.Parse("sin(x)*y + y + 2*sin(x) + 2")),
							 Formula.Parse("sin(x)+1"));
			Console.WriteLine(DateTime.Now);

			AddTestCase(Polynomial.GCD(Formula.Parse("x^8 + x^6 - 3*x^4 - 3*x^3 + 8*x^2 + 2*x - 5"),
			                                         Formula.Parse("3*x^6 + 5*x^4 - 4*x^2 - 9*x + 21")),
			                 1);
			Console.WriteLine(DateTime.Now);
		}

		/// <summary>
		///LCM のテスト
		///</summary>
		[TestCategory("多項式"), TestMethod()]
		public void LCMTest()
		{
			AddTestCase(Polynomial.LCM(Formula.Parse("6"),
													Formula.Parse("3*x")),
							 "6*x");
			AddTestCase(Polynomial.LCM(Formula.Parse("4"),
													Formula.Parse("9*x")),
							 "36*x");
			AddTestCase(Polynomial.LCM(Formula.Parse("2*x^2 + 4"),
													 Formula.Parse("x^2+2")),
							 Formula.Parse("2*x^2 + 4"));
			AddTestCase(Polynomial.LCM(Formula.Parse("2*x^2 - 2"),
													 Formula.Parse("3*x^3 + 3*x^2")),
							 Formula.Parse("6*x^4 - 6*x^2"));
		}
	}
}
