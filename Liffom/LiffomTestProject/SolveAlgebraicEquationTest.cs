// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Processes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Liffom;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///SolveAlgebraicEquationTest のテスト クラスです。すべての
    ///SolveAlgebraicEquationTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class SolveAlgebraicEquationTest
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
        /// SolveAlgebraicEquationのSolve のテスト
        /// </summary>
        [TestCategory("方程式"), TestMethod()]
        public void SolveAlgebraicTest()
        {
            SolveAlgebraicEquation target = new SolveAlgebraicEquation();
            if (Numeric.InnerRealType == Numeric.RealType.BigDecimal)
            {
                SolveEquationTest.AddTestCase(target, "(x+2[cm])^3 = 45[cm^3]", "x", "x=1.55689330449006280600615462224[cm]");
                SolveEquationTest.AddTestCase(target, "(x^2+2)[cm] = 45[cm]", "x", "x=(sqrt(43, 2), -sqrt(43, 2))");
                SolveEquationTest.AddTestCase(target, "x / 50.75[cm] = 3.4", "x", "x = 172.55[cm]");
                SolveEquationTest.AddTestCase(target, "x^3 - 6*x^2 + 11*x - 6 = 0", "x", "x=(1,2,3)");
                SolveEquationTest.AddTestCase(target, "y^3 - 10*y^2 + 31*y - 30 = 0", "y", "y=(2,3,5)");
                SolveEquationTest.AddTestCase(target, "(7.19 / 26.4) ^ 2 + (20.07 / ?) ^ 2 = 2.2", "?",
                    "?=(696.96*sqrt(2387208.57775164/696.96)*2963.2318^-1, -696.96*sqrt(2387208.57775164/696.96)*2963.2318^-1)");
                SolveEquationTest.AddTestCase(target, "x^100 + 3 = 0", "x", "x=(-3)^(1/100)");
                SolveEquationTest.AddTestCase(target, "x^9 - 6*x^6 + 11*x^3 - 6 = 0", "x", "x=(1^(1/3),2^(1/3),3^(1/3))");
            }
            else
            {
                SolveEquationTest.AddTestCase(target, "(x+2[cm])^3 = 45[cm^3]", "x", "x=1.55689330449006[cm]");
                SolveEquationTest.AddTestCase(target, "(x^2+2)[cm] = 45[cm]", "x", "x=(sqrt(43, 2), -sqrt(43, 2))");
                SolveEquationTest.AddTestCase(target, "x / 50.75[cm] = 3.4", "x", "x = 172.55[cm]");
                SolveEquationTest.AddTestCase(target, "x^3 - 6*x^2 + 11*x - 6 = 0", "x", "x=(1,2,3)");
                SolveEquationTest.AddTestCase(target, "y^3 - 10*y^2 + 31*y - 30 = 0", "y", "y=(2,3,5)");
                SolveEquationTest.AddTestCase(target, "(7.19 / 26.4) ^ 2 + (20.07 / ?) ^ 2 = 2.2", "?",
                    "?=(696.96*sqrt(2387208.57775164/696.96)*2963.2318^-1, -696.96*sqrt(2387208.57775164/696.96)*2963.2318^-1)");
                SolveEquationTest.AddTestCase(target, "x^100 + 3 = 0", "x", "x=(-3)^(1/100)");
                SolveEquationTest.AddTestCase(target, "x^9 - 6*x^6 + 11*x^3 - 6 = 0", "x", "x=(1^(1/3),2^(1/3),3^(1/3))");
                SolveEquationTest.AddTestCase(target, "x^3 + 6*x^2 + 8*x + 3 = 0", "x", "x=(-4.30277563773199,-1,-0.697224362268006)");

                target.AdmitImaginary = true;
                SolveEquationTest.AddTestCase(target, "2 x^3 + 6*x^2 + 8*x + 3 = 0", "x", "x=(-0.576146200930217, -1.21192689953489+1.06524130235333*i, -1.21192689953489-1.06524130235333*i)");
            }
        }

        /// <summary>
        ///GetCollected のテスト
        ///</summary>
        [TestCategory("方程式"), TestMethod()]
        [DeploymentItem("Liffom.dll")]
        public void GetCollectedTest()
        {
            var target = new SolveAlgebraicEquation();
            Equal f = Formula.Parse("(7*x+a)*b + z*x^2 = 50") as Equal;
            Variable about = new Variable("x");

            Formula a1 = Formula.Parse("z*x^2");
            Formula a2 = Formula.Parse("7*b*x");
            Formula a3 = Formula.Parse("a*b");
            Formula lhs = new Sum(true, a1, a2, a3, new Numeric(-50));
            Equal expected = new Equal(lhs, 0);
            Equal actual;
            actual = target.GetCollected(f, about);
            Assert.AreEqual(expected, actual);

            f = Formula.Parse("(x+2[cm])^3 = 45[cm^3]") as Equal;
            Unit cm = new Unit("cm");
            a1 = Formula.Parse("x^3");
            a2 = new Product(true, Formula.Parse("x^2*6"), cm);
            a3 = new Product(true, Formula.Parse("x*12"), new Power(cm, 2));
            Formula a4 = new Numeric(-37) * new Power(cm, 3);
            lhs = new Sum(true, a1, a2, a3, a4);
            expected = new Equal(lhs, 0);
            actual = target.GetCollected(f, about);
            Assert.AreEqual(expected, actual);
        
        }

        /// <summary>
        ///SetCoefficientMap のテスト
        ///</summary>
        [TestCategory("方程式"), TestMethod()]
        [DeploymentItem("Liffom.dll")]
        public void SetCoefficientMapTest()
        {
            var target = new SolveAlgebraicEquation(); // TODO: 適切な値に初期化してください
            Variable about = new Variable("x");

            Equal f = Formula.Parse("(7*x+a)*b + z*x^2 = 50") as Equal;
            f = target.GetCollected(f, about);
            target.SetCoefficientMap(f, about);

            Assert.AreEqual(target.CoefficientMap[2], Formula.Parse("z"));
            Assert.AreEqual(target.CoefficientMap[1], Formula.Parse("7*b"));
            Assert.AreEqual(target.CoefficientMap[0], Formula.Parse("-50+a*b"));

            //

            f = Formula.Parse("a * x^(1/3) + b * x ^ 2 * x^(1/3) = 0") as Equal;
            f = target.GetCollected(f, about);
            target.SetCoefficientMap(f, about);

            Assert.AreEqual(target.CoefficientMap[2], Formula.Parse("b"));
            Assert.AreEqual(target.CoefficientMap[0], Formula.Parse("a"));

            //

            f = Formula.Parse("a * x^2.7 + x^1.7 = 0") as Equal;
            f = target.GetCollected(f, about);
            target.SetCoefficientMap(f, about);
            Assert.AreEqual(target.CoefficientMap[2], Formula.Parse("a"));
            Assert.AreEqual(target.CoefficientMap[1], Formula.Parse("1"));
        }
    }
}
