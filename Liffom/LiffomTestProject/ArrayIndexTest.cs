// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Formulas.Array;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///ArrayIndexTest のテスト クラスです。すべての
    ///ArrayIndexTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class ArrayIndexTest
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
        ///SetPositionFromSerialPosition のテスト
        ///</summary>
        [TestCategory("配列"), TestMethod()]
        public void SetPositionFromSerialPositionTest()
        {
            ArrayIndex target = new ArrayIndex(3);
            ArrayIndex size = new ArrayIndex(3);
            size.SetPosition(1, 4);
            size.SetPosition(2, 5);
            size.SetPosition(3, 3);
            ArrayIndex expected = new ArrayIndex(3); // TODO: 適切な値に初期化してください
            expected.SetPosition(1, 2);
            expected.SetPosition(2, 5);
            expected.SetPosition(3, 1);

            int serialIndex = 18;
            target.SetPositionFromSerialPosition(serialIndex, size);

            Assert.AreEqual(expected, target);
        }
    }
}
