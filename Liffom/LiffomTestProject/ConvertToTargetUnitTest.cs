// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Processes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Formulas.Units.Rules;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///ConvertToTargetUnitTest のテスト クラスです。すべての
    ///ConvertToTargetUnitTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class ConvertToTargetUnitTest
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
        ///OnDo のテスト
        ///</summary>
        [TestCategory("単位変換"), TestMethod()]
        [DeploymentItem("Liffom.dll")]
        public void OnDoTest()
        {
            UnitConvertTableTest.RegistBasicUnitRecord();

            ConvertToTargetUnit target = new ConvertToTargetUnit(true);
            {
                var test = Formula.Parse("5[kgf] + 5[N]");
                var targetUnit = new Unit("kN");
                var actual = target.Do(test, targetUnit).Numerate();

                var expected = Formula.Parse("0.04903325[kN] + 0.005[kN]");

                Assert.AreEqual(expected, actual);
            }
            {
                var test = Formula.Parse("-17.78[degC]"); // ファーレンハイトの寒剤(wikipediaより)

                Assert.AreEqual(Formula.Parse("255.37[K]"), target.Do(test, new Unit("K")).Numerate());
                Assert.AreEqual(Formula.Parse("-17.78[degC]"), target.Do(test, new Unit("degC")).Numerate());
                Assert.AreEqual(Formula.Parse("-0.004[degF]"), target.Do(test, new Unit("degF")).Numerate());
                Assert.AreEqual(Formula.Parse("459.666[degR]"), target.Do(test, new Unit("degR")).Numerate());
                Assert.AreEqual(Formula.Parse("176.67[degD]"), target.Do(test, new Unit("degD")).Numerate());
                Assert.AreEqual(Formula.Parse("-5.8674[degN]"), target.Do(test, new Unit("degN")).Numerate());
                Assert.AreEqual(Formula.Parse("-14.224[degRe]"), target.Do(test, new Unit("degRe")).Numerate());
                Assert.AreEqual(Formula.Parse("-1.8345[degRo]"), target.Do(test, new Unit("degRo")).Numerate());
            }
            {
                var test = Formula.Parse("10440[degR]"); // 太陽の表面温度

                Assert.AreEqual(Formula.Parse("5800[K]"), target.Do(test, new Unit("K")).Numerate());
                Assert.AreEqual(Formula.Parse("5526.85[degC]"), target.Do(test, new Unit("degC")).Numerate());
                Assert.AreEqual(Formula.Parse("9980.33[degF]"), target.Do(test, new Unit("degF")).Numerate());
                Assert.AreEqual(Formula.Parse("10440[degR]"), target.Do(test, new Unit("degR")).Numerate());
                Assert.AreEqual(Formula.Parse("-8140.275[degD]"), target.Do(test, new Unit("degD")).Numerate());
                Assert.AreEqual(Formula.Parse("1823.8605[degN]"), target.Do(test, new Unit("degN")).Numerate());
                Assert.AreEqual(Formula.Parse("4421.48[degRe]"), target.Do(test, new Unit("degRe")).Numerate());
                Assert.AreEqual(Formula.Parse("2909.09625[degRo]"), target.Do(test, new Unit("degRo")).Numerate());
            }
            {
                // 加算式などの場合、換算加算値を考慮して変換してしまうと意味が変わってしまう可能性があるため、考慮せずに変換する
                var test = Formula.Parse("0[degC] + 0[K]");

                Assert.AreEqual(Formula.Parse("0[degC] + 0[degC]"), target.Do(test, new Unit("degC")).Numerate());
            }
            {
                var test = Formula.Parse("1[kn] * 1[h] + 1[m]"); // 1kn = 1852m/h なので、1853mになるはず
                Assert.AreEqual("(1853/1852)[h･kn]", test.Simplify().ToString());
            }
            {
                var test = Formula.Parse("1[m] +1 [kn] * 1[h]"); // 1kn = 1852m/h なので、1853mになるはず
                Assert.AreEqual("1853[m]", test.Simplify().ToString());
            }
        }
    }
}
