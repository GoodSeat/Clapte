using GoodSeat.Liffom.Deforms.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using GoodSeat.Liffom;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///RuleTest のテスト クラスです。すべての
    ///RuleTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class RuleTest
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


        internal virtual Rule CreateRule()
        {
            // TODO: 適切な具象クラスをインスタンス化します。
            Rule target = null;
            return target;
        }

        /// <summary>
        ///SelfCheckTest のテスト
        ///</summary>
        [TestCategory("ルール"), TestMethod()]
        public void SelfCheckTestTest()
        {
            UnitConvertTableTest.RegistBasicUnitRecord();

            bool checkOK = true;
            var allRuleReflectors = Rule.GetAllRuleReflectors();

            int countAll = 0;
            int countNG = 0;
            List<string> msgs = new List<string>();

            foreach (var ruleReflector in allRuleReflectors)
            {
                countAll++;

                var rule = ruleReflector.CreateInstance();
                Console.WriteLine(string.Format("-- {0}に対するセルフチェック", rule));
                if (rule == null) throw new FormulaRuleException(string.Format("{0}に既定のコンストラクタが定義されていません。", ruleReflector.ClassName));

                bool isOK = true;
                foreach (var sample in rule.GetAllPatternSample())
                {
                    foreach (var exception in sample.SelfCheckTest())
                    {
                        msgs.Add(rule.ToString());
                        msgs.Add("    " + exception.Message);

                        checkOK = false;
                        isOK = false;
                    }
                }
                if (!isOK) countNG++;
            }

            Console.WriteLine();
            Console.WriteLine(string.Format(" 全テストルール数:{0}  失敗したルール数:{1}", countAll, countNG));
            foreach (var msg in msgs) Console.WriteLine(msg);

            Assert.IsTrue(checkOK);
        }
    }
}
