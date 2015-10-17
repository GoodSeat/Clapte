using GoodSeat.Liffom.Processes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using System.Collections.Generic;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///InstantProcessTest のテスト クラスです。すべての
    ///InstantProcessTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class InstantProcessTest
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
        /// InstantProcessのDo のテスト
        ///</summary>
        [TestCategory("プロセス"), TestMethod()]
        [DeploymentItem("Liffom.dll")]
        public void DoOfInstantProcessTest()
        {
            InstantProcess process = new InstantProcess(f =>
            {
                var x = new Variable("x");
                var solve = new SolveAlgebraicEquation();
                solve.AdmitImaginary = false;
                var result = solve.Solve(f as Equal, x);
                return result;
            });
            process.ProcessCompleted += new ProcessCompletedEventHandler(ProcessTest.ProcessTestProcessCompleted);

            var userStates = new List<object>();
            Console.WriteLine(string.Format("全プロセス開始前 : {0}", DateTime.Now));
            ProcessTest.AddTestCaseAsync(process, "x=1.55689330449006[cm]", userStates, "(x+2[cm])^3 = 45[cm^3]");
            ProcessTest.AddTestCaseAsync(process, "x=(sqrt(43, 2), -sqrt(43, 2))", userStates, "(x^2+2)[cm] = 45[cm]");
            ProcessTest.AddTestCaseAsync(process, "x=(1,2,3)", userStates, "x^3 - 6*x^2 + 11*x - 6 = 0");
            Console.WriteLine(string.Format("全プロセス開始済み : {0}", DateTime.Now));

            ProcessTest.CheckAssertionTestCase(process, userStates);
        }

    }
}
