using GoodSeat.Liffom.Formulas.Operators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Formulas.Rules;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///SumTest のテスト クラスです。すべての
    ///SumTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class SumTest
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
        /// SumのSimplify のテスト
        /// </summary>
        [TestCategory("変形"), TestMethod()]
        public void SimplifyOfSumTest()
        {
            UnitConvertTableTest.RegistBasicUnitRecord();

            var token = Formula.SimplifyToken;

            DeformTokenTest.DeformTest(token, "2*x + 3/2", "(4*x+3)/2");
            DeformTokenTest.DeformTest(token, "5[N] + 5[kgf]", "54.03325[N]");
            DeformTokenTest.DeformTest(token, "2[kg] * 5[m/s^2] + 5[kgf]", "59.03325[kg*m/s^2]");

            DeformTokenTest.DeformTest(token, "(b+a*b)/(1+a)", "b");
            DeformTokenTest.DeformTest(token, "a+a/(1+a)", "(a^2 + 2*a)/(1 + a)");
            DeformTokenTest.DeformTest(token, "(1+a)*b/(1+a)", "b");

            // 数値の分数化のテスト
            token = new SimplifyToken();
            token.AdditionalTryRules.Add(ConvertToFractionRule.Entity);
            DeformTokenTest.DeformTest(token, "(5.5+25/15)/73.7+12/15", "9919/11055");

            token = new DeformToken(new SimplifyToken(), new CalculateToken());
            DeformTokenTest.DeformTest(token, "(7.19 / 26.4) ^ 2 + (20.07 / ?) ^ 2 = 2.2",
                "(70184725776 + 12924025*?^2) / (174240000*?^2) = 2.2");
        }
    }
}
