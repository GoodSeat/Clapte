using GoodSeat.Liffom.Processes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom;
using GoodSeat.Liffom.Formulas;
using System.Collections.Generic;
using GoodSeat.Liffom.Utilities;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///ProcessTest のテスト クラスです。すべての
    ///ProcessTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class ProcessTest
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
        /// Processの非同期テストケースを追加し、関連付けられたユーザー状態を取得します。
        /// </summary>
        /// <param name="process">テスト対象のプロセスオブジェクト。</param>
        /// <param name="expected">期待される結果</param>
        /// <param name="userStates">テストケースのユーザー状態リスト</param>
        /// <param name="args">引数とする可変数の数式</param>
        public static void AddTestCaseAsync(Process process, string expected, List<object> userStates, params string[] args)
        {
            List<Formula> argFormulas = new List<Formula>();
            foreach (var arg in args)
                argFormulas.Add(Formula.Parse(arg));

            process.DoAsync(expected, argFormulas.ToArray());
            userStates.Add(expected);
        }

        /// <summary>
        /// ユーザー状態リストを指定して、テスト結果の正当性を検証します。
        /// </summary>
        /// <param name="process"></param>
        /// <param name="userStates"></param>
        public static void CheckAssertionTestCase(Process process, List<object> userStates)
        {
            foreach (object userState in userStates)
            {
                var result = process.Wait(userState);
                Assert.AreEqual(Formula.Parse(userState.ToString()), result);
            }
        }

        /// <summary>
        /// <see cref="AddTestCaseAsync"/>にて追加されたテストケース検証終了時の情報をコンソール出力します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ProcessTestProcessCompleted(object sender, EAPCompletedEventArgs<Formula> e)
        {
            Console.WriteLine(string.Format("[{0}] {1}::{2}", DateTime.Now, e.UserState, e.Result));
        }
    }
}
