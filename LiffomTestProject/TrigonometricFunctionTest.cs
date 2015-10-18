using GoodSeat.Liffom.Formulas.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///TrigonometricFunctionTest のテスト クラスです。すべての
    ///TrigonometricFunctionTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class TrigonometricFunctionTest
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


        internal virtual TrigonometricFunction CreateTrigonometricFunction()
        {
            // TODO: 適切な具象クラスをインスタンス化します。
            TrigonometricFunction target = null;
            return target;
        }

        /// <summary>
        ///CalculateFunction のテスト
        ///</summary>
        [TestCategory("三角関数"), TestCategory("変形"), TestMethod()]
        public void CalculateFunctionTest()
        {
            UnitConvertTableTest.RegistBasicUnitRecord();

            DeformToken token = new DeformToken(Formula.SimplifyToken, Formula.CalculateToken);
            DeformTokenTest.DeformTest(token, "cos(5*pi[rad])", "-1");
            DeformTokenTest.DeformTest(token, "sin(13/6*pi[rad])", "1/2");
            DeformTokenTest.DeformTest(token, "cos(5*pi)", "-1");
            DeformTokenTest.DeformTest(token, "sin(390[deg])", "1/2");
            DeformTokenTest.DeformTest(token, "atan(0.5)", "0.463647609000806");
            DeformTokenTest.DeformTest(token, Formula.Parse("tan(pi/2)"), double.PositiveInfinity);
            DeformTokenTest.DeformTest(token, Formula.Parse("tan(-pi/2)"), double.NegativeInfinity);
        }
    }
}
