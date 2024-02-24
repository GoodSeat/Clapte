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
using System.Collections.Generic;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///CalculateUnitConversionFactorTest のテスト クラスです。すべての
    ///CalculateUnitConversionFactorTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class CalculateUnitConversionFactorTest
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
        ///BuildUnitList のテスト
        ///</summary>
        [TestCategory("単位変換"), TestMethod()]
        [DeploymentItem("Liffom.dll")]
        public void BuildUnitListTest()
        {
            var target = new CalculateUnitConversionFactor();
            Formula unitFormula = Formula.Parse("[kN*g*m/(s*h)]");
            List<Unit> moleculars = new List<Unit>();
            List<Unit> denominators = new List<Unit>();
            target.BuildUnitList(unitFormula, ref moleculars, ref denominators);

            List<Unit> molecularsExpected = new List<Unit>();
            molecularsExpected.Add(new Unit("kN"));
            molecularsExpected.Add(new Unit("g"));
            molecularsExpected.Add(new Unit("m"));
            List<Unit> denominatorsExpected = new List<Unit>();
            denominatorsExpected.Add(new Unit("s"));
            denominatorsExpected.Add(new Unit("h"));

            CollectionAssert.AreEquivalent(molecularsExpected, moleculars);
            CollectionAssert.AreEquivalent(denominatorsExpected, denominators);
        }

        /// <summary>
        ///OnDo のテスト
        ///</summary>
        [TestCategory("単位変換"), TestCategory("プロセス"), TestMethod()]
        [DeploymentItem("Liffom.dll")]
        public void OnDoTest()
        {
            UnitConvertTableTest.RegistBasicUnitRecord();

            AddTestCase("[m]", "[cm]", "100");
            AddTestCase("[atm*l/J]", "101.325");
            AddTestCase("[atm*l*mol/J]", "101.325[mol]");
            AddTestCase("[atm*l]", "[J]", "101.325");
            AddTestCase("[rad]", "1");
            AddTestCase("[kg]", "[kg*cm/m]", "100");
            AddTestCase("[kn*h]", "[m]", "1852");
        }

        private void AddTestCase(string from, string to, string expectedConversionFactor)
        {
            CalculateUnitConversionFactor target = new CalculateUnitConversionFactor();
            var fromUnit = Formula.Parse(from);
            var toUnit = Formula.Parse(to);
            var expected = Formula.Parse(expectedConversionFactor);
            var actual = target.Do(fromUnit, toUnit);
            Assert.AreEqual(expected, actual);
        }

        private void AddTestCase(string from, string expectedConversionFactor)
        {
            CalculateUnitConversionFactor target = new CalculateUnitConversionFactor();
            var fromUnit = Formula.Parse(from);
            var expected = Formula.Parse(expectedConversionFactor);
            var actual = target.Do(fromUnit);
            Assert.AreEqual(expected, actual);
        }




    }
}
