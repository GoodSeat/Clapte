// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Formulas.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Deforms;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///DifferentiateTest のテスト クラスです。すべての
    ///DifferentiateTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class DifferentiateTest
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
        /// diffのCalculateFunction のテスト
        /// </summary>
        [TestCategory("微分"), TestMethod()]
        public void CalculateFunctionTest()
        {
            AddTestCase("3^(2*x)", new Variable("x"), "2*3^(2*x)*ln(3, e)");
            AddTestCase("(x^7 * 3)^-1", new Variable("x"), "-7/3 * 1/x^8");
            AddTestCase("3^-1 * (x^7)^-1", new Variable("x"), "-7 / (3 * x^8)");
            AddTestCase("sin(4*x)^3", new Variable("x"), "12*sin(4*x)^2*cos(4*x)");
            AddTestCase("x*cos(y) - y*cos(x)", Null.Empty, "cos(y)*dx + y*sin(x)*dx - x*sin(y)*dy - cos(x) * dy");
        }

        void AddTestCase(string target, Formula x, string expected)
        {
            DeformToken token = new DeformToken(new SimplifyToken(), new DifferentiateToken());
            Formula targetFormula = new Differentiate(Formula.Parse(target), x);

            DeformTokenTest.DeformTest(token, targetFormula, expected);
        }
    }
}
