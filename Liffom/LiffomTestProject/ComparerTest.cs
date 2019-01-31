// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Deforms;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///ComparerTest のテスト クラスです。すべての
    ///ComparerTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class ComparerTest
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


        internal virtual Comparer CreateComparer()
        {
            // TODO: 適切な具象クラスをインスタンス化します。
            Comparer target = null;
            return target;
        }

        /// <summary>
        ///GetJudge のテスト
        ///</summary>
        [TestCategory("演算"), TestMethod()]
        public void GetJudgeTest()
        {
            DeformToken token = new DeformToken(Formula.CalculateToken, Formula.NumerateToken, Formula.SimplifyToken);

            Comparer target = Formula.Parse("5*kN < 10*kN") as Comparer;
            Assert.AreEqual(Comparer.Judge.None, target.GetJudge(token)); // 変数は負の数値の可能性があるので判定不可

            target = Formula.Parse("5[kN] < 10[kN]") as Comparer;
            Assert.AreEqual(Comparer.Judge.True, target.GetJudge(token));
        }
    }
}
