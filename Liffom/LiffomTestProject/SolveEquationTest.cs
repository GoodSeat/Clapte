using GoodSeat.Liffom.Processes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using GoodSeat.Liffom;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators.Comparers;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///SolveEquationTest のテスト クラスです。すべての
    ///SolveEquationTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class SolveEquationTest
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
        ///GetModifiedSolution のテスト
        ///</summary>
        [TestCategory("方程式"), TestMethod()]
        [DeploymentItem("Liffom.dll")]
        public void GetModifiedSolutionTest()
        {
            Formula f = Formula.Parse("3*x^2 - 45*x");
            Variable x = new Variable("x");
            Formula solution = 14.9998d;
            double error = 0.001d;
            Formula expected = 15d;
            Formula actual;
            actual = SolveEquation_Accessor.GetModifiedSolution(f, x, solution, error);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetRoundSolution のテスト
        ///</summary>
        [TestCategory("方程式"), TestMethod()]
        [DeploymentItem("Liffom.dll")]
        public void GetRoundSolutionTest()
        {
            Formula f = Formula.Parse("3*x^2 - 45*x");
            Variable x = new Variable("x");
            Formula solution = 14.9998d;
            double error = 0.001d;
            Formula expected = 15d;
            Formula actual;
            actual = SolveEquation_Accessor.GetRoundSolution(f, x, solution, error);
            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        ///GetInitialPrecision のテスト
        ///</summary>
        [TestCategory("方程式"), TestMethod()]
        [DeploymentItem("Liffom.dll")]
        public void GetInitialPrecisionTest()
        {
            Formula f = Formula.Parse("log(x, 10) - 1.636");
            Variable x = new Variable("x");
            Formula solution = 43.251383103501d;
            int initialValidDigit = 0;
            int initialPrecision = 0;

            int initialValidDigitExpected = -3;
            int initialPrecisionExpected = 4;
            bool expected = true;
            bool actual;
            actual = SolveEquation_Accessor.GetInitialPrecision(f, x, solution, out initialValidDigit, out initialPrecision);
            Assert.AreEqual(initialValidDigitExpected, initialValidDigit);
            Assert.AreEqual(initialPrecisionExpected, initialPrecision);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetPrecisionModifiedSolution のテスト
        ///</summary>
        [TestCategory("方程式"), TestMethod()]
        [DeploymentItem("Liffom.dll")]
        public void GetPrecisionModifiedSolutionTest()
        {
            Formula f = Formula.Parse("log(x,10) - 1.636");
            Variable x = new Variable("x");
            Formula solution = 43.251383103501d;
            int initialValidDigit = -3;

            Numeric actual;
            actual = SolveEquation_Accessor.GetPrecisionModifiedSolution(f, x, solution, initialValidDigit) as Numeric;
            Assert.AreEqual(3, actual.Precision);
        }



        /// <summary>
        /// SolveEquationのテストケースを追加し、テスト結果の検証を行います。
        /// </summary>
        /// <param name="solve">テスト対象のSolveEquationオブジェクト。</param>
        /// <param name="target">テスト対象の方程式</param>
        /// <param name="x">解を求める変数</param>
        /// <param name="expected">期待される解</param>
        public static void AddTestCase(SolveEquation solve, string target, string x, string expected)
        {
            Formula targetFormula = Formula.Parse(target);
            Formula actual = solve.Solve(targetFormula as Equal, new Variable(x));

            Assert.AreEqual(Formula.Parse(expected), actual);
        }

        /// <summary>
        /// SolveEquationの非同期テストケースを追加し、関連付けられたユーザー状態を取得します。
        /// </summary>
        /// <param name="solve">テスト対象のSolveEquationオブジェクト。</param>
        /// <param name="target">テスト対象の方程式</param>
        /// <param name="x">解を求める変数</param>
        /// <param name="expected">期待される解</param>
        /// <param name="userStates">テストケースのユーザー状態リスト</param>
        public static void AddTestCaseAsync(SolveEquation solve, string target, string x, string expected, List<object> userStates)
        {
            Formula targetFormula = Formula.Parse(target);
            solve.DoAsync(expected, targetFormula, new Variable(x));
            userStates.Add(expected);
        }

        /// <summary>
        /// ユーザー状態リストを指定して、テスト結果の正当性を検証します。
        /// </summary>
        /// <param name="solve"></param>
        /// <param name="userStates"></param>
        public static void CheckAssertionTestCase(SolveEquation solve, List<object> userStates)
        {
            foreach (object userState in userStates)
            {
                var result = solve.Wait(userState);
                Assert.AreEqual(Formula.Parse(userState.ToString()), result);
            }
        }

        /// <summary>
        /// <see cref="AddTestCaseAsync"/>にて追加されたテストケース検証終了時の情報をコンソール出力します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void SolveEquationTestProcessCompleted(object sender, ProcessCompletedEventArgs e)
        {
            Console.WriteLine(string.Format("[{0}] {1}::{2}", DateTime.Now, e.UserState, e.Result));
        }

    }
}
