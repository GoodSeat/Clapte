using Liffom.Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Liffom.Formulas.Operators;
using Liffom.Formats;
using Liffom.Formats.Numerics;
using Liffom.Formulas.Operators.Comparers;

namespace LiffomTestProject
{
    
    
    /// <summary>
    ///FormulaTest のテスト クラスです。すべての
    ///FormulaTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class FormulaTest
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
		///ToString のテスト
		///</summary>
        [TestCategory("出力書式"), TestMethod()]
		public void ToStringTest()
		{
			var f1 = new Sum(new Variable("x"), new Variable("y"));
			var f2 = new Product(new Variable("x"), new Variable("y"));

			Assert.AreEqual("(x+y)*(x*y)", (f1 * f2).ToString());
			Assert.AreEqual("(x+y)+x*y", (f1 + f2).ToString());

            // 書式の継承のテスト
            var f3 = Formula.Parse("5.253 + 358589.35");

            var format = new Format();
            format.SetProperty(new RadixPointFormatProperty(RadixPointFormatProperty.RadixPointType.Period));
            format.SetProperty(new SplitFormatProperty(SplitFormatProperty.SplitType.Space));
            f3.Format = format;
            Assert.AreEqual("5.253+358 589.35", f3.ToString());

            format.SetProperty(new RadixPointFormatProperty(RadixPointFormatProperty.RadixPointType.Comma));
            format.SetProperty(new SplitFormatProperty(SplitFormatProperty.SplitType.Period));
            f3.Format = format;
            Assert.AreEqual("5,253+358.589,35", f3.ToString());

            var f4 = Formula.Parse("5x/2^-1");
            Assert.AreEqual("(5*x)/(2^-1)", f4.ToString());

            OperatorFormatProperty operatorFormat = new OperatorFormatProperty(OperatorFormatProperty.OperatorType.Product | OperatorFormatProperty.OperatorType.Power | OperatorFormatProperty.OperatorType.Sum);
            f4.Format.SetProperty(operatorFormat);
            Assert.AreEqual("(5 * x) / (2 ^ -1)", f4.ToString());


			Formula f5 = Formula.Parse("5 * a[m] * b[kN]");
			f5 = f5.Simplify();
            Assert.AreEqual("(5*a*b)[m･kN]", f5.ToString());
		}

        /// <summary>
        ///Substituted のテスト
        ///</summary>
        [TestMethod()]
        public void SubstitutedTest()
        {
            Formula target = Formula.Parse("E = m*c^2");

            Variable m = new Variable("m");
            Variable c = new Variable("c");

            Formula actual;
            actual = target.Substituted(
                new Equal(m, Formula.Parse("5[kg]")),
                new Equal(c, Formula.Parse("299792458[m/s]"))
                );

            Assert.AreEqual(Formula.Parse("E = 5[kg] * (299792458[m/s])^2"), actual);
        }
	}
}
