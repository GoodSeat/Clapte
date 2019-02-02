// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Formulas.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///ArcCosTest のテスト クラスです。すべての
    ///ArcCosTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class ArcCosTest
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
        ///CalculateFunction のテスト
        ///</summary>
        [TestCategory("三角関数"), TestCategory("変形"), TestMethod()]
        public void CalculateFunctionTest()
        {
            ArcCos target = new ArcCos(Formula.Parse("1/2"));
            Formula expected = Formula.Parse("1/3*pi");
            Formula actual;
            actual = target.CalculateFunction();
            Assert.AreEqual(expected, actual);
        }
    }
}
