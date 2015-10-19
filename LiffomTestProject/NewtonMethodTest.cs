using GoodSeat.Liffom.Processes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Liffom;
using System.Collections.Generic;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///NewtonMethodTest のテスト クラスです。すべての
    ///NewtonMethodTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class NewtonMethodTest
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
        /// ニュートン・ラフソン法のSolve のテスト
        /// </summary>
        [TestCategory("方程式"), TestMethod()]
        public void NewtonMethodSolveTest()
        {
            var solve = new NewtonMethod();
            solve.ProcessCompleted += SolveEquationTest.SolveEquationTestProcessCompleted;

            var userStates = new List<object>();
            Console.WriteLine(string.Format("全プロセス開始前 : {0}", DateTime.Now));
            SolveEquationTest.AddTestCaseAsync(solve, "x^9 + 7*x = 589", "x", "x=2.02588389785461", userStates);
            SolveEquationTest.AddTestCaseAsync(solve, "(x+3)*(x-1)^2 = 0", "x", "x=1", userStates);
            SolveEquationTest.AddTestCaseAsync(solve, "x*sin(x)=0.2", "x", "x=0.455052636794154", userStates);
            Console.WriteLine(string.Format("全プロセス開始済み : {0}", DateTime.Now));

            SolveEquationTest.CheckAssertionTestCase(solve, userStates);
        }

    }
}
