using GoodSeat.Liffom.Formulas.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///ImaginaryTest のテスト クラスです。すべての
    ///ImaginaryTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class ImaginaryTest
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
        ///GetRealAndImaginary のテスト
        ///</summary>
        [TestCategory("数値"), TestMethod()]
        public void GetRealAndImaginaryTest()
        {
            Formula f = Formula.Parse("5+x+i*z");
            Formula RExpected = Formula.Parse("5+x");
            Formula EExpected = Formula.Parse("z");
            Formula R, E; 
            Imaginary.GetRealAndImaginary(f, out R, out E);
            Assert.AreEqual(RExpected, R);
            Assert.AreEqual(EExpected, E);
        }
    }
}
