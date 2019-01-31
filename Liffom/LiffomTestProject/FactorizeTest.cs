// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Processes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom;
using GoodSeat.Liffom.Formulas;
using System.Collections.Generic;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///FactorizeTest のテスト クラスです。すべての
    ///FactorizeTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class FactorizeTest
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
        ///GetAllDivisors のテスト
        ///</summary>
        [TestCategory("因数分解"), TestMethod()]
        [DeploymentItem("Liffom.dll")]
        public void GetAllDivisorsTest()
        {
            Factorize_Accessor target = new Factorize_Accessor();
            Formula f = Formula.Parse("a*(x+1)^2");
            bool containMinus = true;
            List<Formula> expected = new List<Formula>();
            expected.Add(Formula.Parse("1"));
            expected.Add(Formula.Parse("-1"));
            expected.Add(Formula.Parse("a"));
            expected.Add(Formula.Parse("-a"));
            expected.Add(Formula.Parse("x+1"));
            expected.Add(Formula.Parse("-(x+1)"));
            expected.Add(Formula.Parse("(x+1)^2"));
            expected.Add(Formula.Parse("-((x+1)^2)"));
            expected.Add(Formula.Parse("a*(x+1)"));
            expected.Add(Formula.Parse("-1*a*(x+1)"));
            expected.Add(Formula.Parse("a*(x+1)^2"));
            expected.Add(Formula.Parse("-1*a*(x+1)^2"));

            List<Formula> actual = target.GetAllDivisors(f, containMinus);

            CollectionAssert.AreEquivalent(expected, actual);
        }

        /// <summary>
        ///GetDecomposedFactors のテスト
        ///</summary>
        [TestCategory("因数分解"), TestMethod()]
        [DeploymentItem("Liffom.dll")]
        public void GetDecomposedFactorsTest()
        {
            Factorize_Accessor target = new Factorize_Accessor();
            Formula f = Formula.Parse("a*(x+1)^2");
            List<Formula> expected = new List<Formula>();
            expected.Add(Formula.Parse("a"));
            expected.Add(Formula.Parse("x+1"));
            expected.Add(Formula.Parse("x+1"));

            List<Formula> actual = new List<Formula>(target.GetDecomposedFactors(f));

            CollectionAssert.AreEquivalent(expected, actual);
        }

        /// <summary>
        ///OnDo のテスト
        ///</summary>
        [TestCategory("因数分解"), TestCategory("プロセス"), TestMethod()]
        [DeploymentItem("Liffom.dll")]
        public void OnDoTest()
        {
            AddFactorizeTestCase("2*x^2*y+3*x*y+y", "(x+1)*(2*x+1)*y");
            AddFactorizeTestCase("x^4+x^2+1", "(x^2+x+1)(x^2-x+1)");
            AddFactorizeTestCase("a^3+b^3", "(a+b)(a^2-a*b+b^2)");
            AddFactorizeTestCase("x^4-6*x^2+8", "(x^2-2)(x+2)(x-2)");
            AddFactorizeTestCase("6*y*z^2+3*x*z^2+6*y^3*z+3*x*y^2*z+2*x*y*z+x^2*z+2*x*y^3+x^2*y^2", "(x+2*y)*(3*z+x)*(y^2+z)");
//            AddFactorizeTestCase("x^18-1", "(x-1)*(x+1)*(x^2-x+1)*(x^2+x+1)*(x^6-x^3+1)*(x^6+x^3+1)");
        }

        void AddFactorizeTestCase(string target, string expected)
        {
            Factorize_Accessor factorizer = new Factorize_Accessor();
            Formula actual = factorizer.OnDo(null, Formula.Parse(target));
            Assert.AreEqual(Formula.Parse(expected), actual);
        }
    }
}
