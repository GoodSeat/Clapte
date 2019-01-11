using GoodSeat.Liffom.Reals;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///RealTest のテスト クラスです。すべての
    ///RealTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class RealTest
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
        ///Round のテスト
        ///</summary>
        [TestCategory("数値"), TestMethod()]
        public void RoundTest()
        {
            double d = 0F;
            int decimals = 0;
            double expected = 0F;
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        d = 0.000052138;
                        decimals = 6;
                        expected = 0.000052;
                        break;
                    case 1:
                        d = 0.6;
                        decimals = 0;
                        expected = 1;
                        break;
                    case 2:
                        d = 2359.1235;
                        decimals = -2;
                        expected = 2400;
                        break;
                    case 3:
                        d = 2.3591235E-20;
                        decimals = 22;
                        expected = 2.36E-20;
                        break;
                }
                var dr = new DoubleValueModified(d);

                double actual;
                actual = dr.Round(decimals).ToDouble();
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// DecimalValue のテスト
        /// </summary>
        [TestCategory("数値"), TestMethod()]
        public void DecimalValueTest()
        {
            var test = new DecimalValue(2.345m) as Value;
            var ans = test.GetNapiers() ^ test;
            Assert.AreEqual("10.43327272754891516370603", ans.ToString());
            //               10.4332727275489151637060295131 [keisan.casio.jp]

            test = new DecimalValue(3m);
            ans = test ^ test;
            Assert.AreEqual("27", ans.ToString());

            test = new DecimalValue(3.151351m) ^ new DecimalValue(12.513151m);
            Assert.AreEqual("1728882.4083962592610184375", test.ToString());
            //               1728882.40839625926101843745647 [keisan.casio.jp]

            test = new DecimalValue(DecimalValue.Pi / 6m);
            ans = test.Sin();
            Assert.AreEqual("0.5", ans.ToString());
            //               0.5 [keisan.casio.jp]

            ans = test.Cos();
            Assert.AreEqual("0.86602540378443864676372317", ans.ToString());
            //               0.866025403784438646763723170753 [keisan.casio.jp]

            ans = test.Tan();
            Assert.AreEqual("0.57735026918962576450914878", ans.ToString());
            //               0.577350269189625764509148780502 [keisan.casio.jp]

            ans = new DecimalValue(1.2m).Atan();
            Assert.AreEqual("0.87605805059819342311404752", ans.ToString());
            //               0.876058050598193423114047521128 [keisan.casio.jp]

            ans = new DecimalValue(0.8m).Atan();
            Assert.AreEqual("0.67474094222355266305652097", ans.ToString());
            //               0.67474094222355266305652097361 [keisan.casio.jp]

            ans = new DecimalValue(-1.2m).Atan();
            Assert.AreEqual("-0.87605805059819342311404752", ans.ToString());
            //               -0.876058050598193423114047521128 [keisan.casio.jp]

            ans = new DecimalValue(-0.8m).Atan();
            Assert.AreEqual("-0.67474094222355266305652097", ans.ToString());
            //               -0.67474094222355266305652097361 [keisan.casio.jp]

        }


        /// <summary>
        /// BigDecimalValue のテスト
        /// </summary>
        [TestCategory("数値"), TestMethod()]
        public void BigDecimalValueTest()
        {
            var r1 = new BigDecimalValue(1.23456789012);
            var r2 = new BigDecimalValue(10.23456789018);
            Assert.AreEqual( "11.4691357803", (r1 + r2).ToString());

            r1 = new BigDecimalValue(1.23456789012);
            r2 = new BigDecimalValue(10.23456789012);
            Assert.AreEqual( "12.6352688863953483936144", (r1 * r2).ToString());

            r1 = new BigDecimalValue(10);
            r2 = new BigDecimalValue(0.0025);
            Assert.AreEqual("4000", (r1 / r2).ToString());

            r2 = new BigDecimalValue(3);
            string expect = "3.";
            for (int i = 1; i < BigDecimalValue.MaxDigits; i++) expect += "3";
            Assert.AreEqual(expect, (r1 / r2).ToString());


            r1 = new BigDecimalValue(-1.23456789012);
            r2 = new BigDecimalValue(10.23456789018);
            Assert.AreEqual("9.00000000006", (r1 + r2).ToString());

            r1 = new BigDecimalValue(-1.23456789012);
            r2 = new BigDecimalValue(10.23456789012);
            Assert.AreEqual( "-12.6352688863953483936144", (r1 * r2).ToString());

            r1 = new BigDecimalValue(-10);
            r2 = new BigDecimalValue(0.0025);
            Assert.AreEqual("-4000", (r1 / r2).ToString());

            r2 = new BigDecimalValue(3);
            Assert.AreEqual("-" + expect, (r1 / r2).ToString());
        }

        object _lock = new object();

        /// <summary>
        /// ResetDigits のテスト
        /// </summary>
        [TestCategory("数値"), TestMethod()]
        public void ResetDigitsTest()
        {
            lock (_lock)
            {
                int save = BigDecimalValue.MaxDigits;
                BigDecimalValue.MaxDigits = 4;

                var test = new BigDecimalValue(3.33333333d);
                PrivateObject po = new PrivateObject(test);
                po.Invoke("ResetDigits");

                var r1 = new BigDecimalValue(1d);
                var r3 = new BigDecimalValue(3d);
                var t1 = r1 / r3;
                Assert.AreEqual("0.3333", t1.ToString());
                var t2 = t1 * 3;
                Assert.AreEqual("1", t2.ToString());


                r1 = new BigDecimalValue(-1d);
                t1 = r1 / r3;
                Assert.AreEqual("-0.3333", t1.ToString());
                t2 = t1 * 3;
                Assert.AreEqual("-1", t2.ToString());

                BigDecimalValue.MaxDigits = save;
            }
        }

        /// <summary>
        /// BigDecimalValue のテスト
        /// </summary>
        [TestCategory("数値"), TestMethod()]
        public void BigDecimalValueFunctionTest()
        {
            lock (_lock)
            {
                int save = BigDecimalValue.MaxDigits;
                BigDecimalValue.MaxDigits = 30;

                var result = Value.Exp(new BigDecimalValue(2.345d), 30);
                Assert.AreEqual("10.4332727275489151637060295131", result.ToString());
                //              "10.4332727275489151637060295131" [keisan.casio.jp]

                result = Value.Power(new BigDecimalValue(3), new BigDecimalValue(3), 30);
                Assert.AreEqual(27d, (double)result);

                result = Value.Power((BigDecimalValue)3.151351, (BigDecimalValue)12.513151, 30);
                Assert.AreEqual("1728882.40839625926101843745647", result.ToString());
                //               1728882.40839625926101843745647 [keisan.casio.jp]

                result = Value.Power((BigDecimalValue)0.003151351, (BigDecimalValue)12.513151, 30);
                Assert.AreEqual("4.99243541526789431258604727653E-32", result.ToString());
                //               4.99243541526789431258604727654E-32 [keisan.casio.jp]
                //                                             ~

                result = Value.Ln((BigDecimalValue)851.481, 30);
                Assert.AreEqual("6.74697718628949339448203169576", result.ToString());
                //               6.74697718628949339448203169576 [keisan.casio.jp]

                var pi = result.GetPi();

                result = Value.Sin(pi / 6d as BigDecimalValue, 30);
                Assert.AreEqual(0.5, result);
                //              0.5 [keisan.casio.jp]

                result = Value.Cos(pi / 6d as BigDecimalValue, 30);
                Assert.AreEqual("0.866025403784438646763723170753", result.ToString());
                //              "0.866025403784438646763723170753" [keisan.casio.jp]

                result = Value.Tan(pi / 6d as BigDecimalValue, 30);
                Assert.AreEqual("0.577350269189625764509148780502", result.ToString());
                //              "0.577350269189625764509148780502" [keisan.casio.jp]

                result = Value.Atan(new BigDecimalValue(1.2), 30);
                Assert.AreEqual("0.876058050598193423114047521128", result.ToString());
                //              "0.876058050598193423114047521128" [keisan.casio.jp]

                result = Value.Atan(new BigDecimalValue(0.8), 30);
                Assert.AreEqual("0.67474094222355266305652097361", result.ToString());
                //              "0.67474094222355266305652097361" [keisan.casio.jp]

                result = Value.Atan(new BigDecimalValue(-1.2), 30);
                Assert.AreEqual("-0.876058050598193423114047521128", result.ToString());
                //              "-0.876058050598193423114047521128" [keisan.casio.jp]

                result = Value.Atan(new BigDecimalValue(-0.8), 30);
                Assert.AreEqual("-0.67474094222355266305652097361", result.ToString());
                //              "-0.67474094222355266305652097361" [keisan.casio.jp]

                BigDecimalValue.MaxDigits = save;
            }
        }

        /// <summary>
        /// BigDecimalValue のπの計算テスト
        /// </summary>
        [TestCategory("数値"), TestMethod()]
        public void BigDecimalValuePiTest()
        {
            BigDecimalValue.MaxDigits = 1000;
            var dummy = new BigDecimalValue();
            var pi = dummy.GetPi();
            var textPi = pi.ToString();
            Assert.AreEqual("0199", textPi.Substring(textPi.Length - 4));

            // 以下は時間がかかりすぎるのでコメントアウト
            // var e = dummy.GetNapiers();
            // var texte = e.ToString();
            // Assert.AreEqual("5035", texte.Substring(texte.Length - 4));
        }

        /// <summary>
        /// BigDecimalValue の構文解析テスト
        /// </summary>
        [TestCategory("数値"), TestMethod()]
        public void BigDecimalValueParseTest()
        {
            var test = BigDecimalValue.Parse("178598.235");
            Assert.AreEqual(178598.235d, test.ToDouble());

            test = BigDecimalValue.Parse("17.8598235E+4");
            Assert.AreEqual(178598.235d, test.ToDouble());
        }

        /// <summary>
        /// 非数値や無限大となる数値の扱いに関するテスト。
        /// </summary>
        [TestCategory("数値"), TestMethod()]
        public void InvalidValueTreatmentTest()
        {
            // doubleの動作を正解とする
            {
                double testLn = Math.Log(-2.5);
                Assert.AreEqual(double.NaN, testLn);
                Assert.AreEqual(double.NaN, 5.0 + testLn);

                double testLn2 = Math.Log(0);
                Assert.AreEqual(double.NegativeInfinity, testLn2);
                Assert.AreEqual(double.NegativeInfinity, 5.0 + testLn2);
                Assert.AreEqual(double.PositiveInfinity, testLn2 * -1);

                double asin = Math.Asin(1.2);
                Assert.AreEqual(double.NaN, asin);

                double testInfP = 5d / 0d;
                Assert.AreEqual(double.PositiveInfinity, testInfP);
                Assert.AreEqual(double.PositiveInfinity, testInfP / 30d);

                double testInfM = -5d / 0d;
                Assert.AreEqual(double.NegativeInfinity, testInfM);
                Assert.AreEqual(double.NegativeInfinity, testInfM / 30d);

                Assert.AreEqual(double.NaN, testInfM + testInfP);
                Assert.AreEqual(double.NaN, testInfM / testInfP);

                double maxTest = Math.Max(testLn, testInfP);
                Assert.AreEqual(testLn, maxTest);
            }

            // DoubleValueModified
            {
                DoubleValueModified testLn = Math.Log(-2.5);
                Assert.IsTrue(testLn.IsNaN);
                Assert.IsTrue((5.0 + testLn).IsNaN);

                DoubleValueModified testLn2 = Math.Log(0);
                Assert.IsTrue(testLn2.IsNegativeInfinity);
                Assert.IsTrue((5.0 + testLn2).IsNegativeInfinity);

                DoubleValueModified asin = Math.Asin(1.2);
                Assert.IsTrue(asin.IsNaN);

                DoubleValueModified testInfP = 5d / 0d;
                Assert.IsTrue(testInfP.IsPositiveInfinity);
                Assert.IsTrue((testInfP / 30d).IsPositiveInfinity);

                DoubleValueModified testInfM = -5d / 0d;
                Assert.IsTrue(testInfM.IsNegativeInfinity);
                Assert.IsTrue((testInfM / 30d).IsNegativeInfinity);

                Assert.IsTrue((testInfM + testInfP).IsNaN);
                Assert.IsTrue((testInfM / testInfP).IsNaN);
            }

            // DecimalValue
            {
                DecimalValue testLn = Math.Log(-2.5);
                Assert.IsTrue(testLn.IsNaN);
                Assert.IsTrue((5.0 + testLn).IsNaN);

                DecimalValue testLn2 = Math.Log(0);
                Assert.IsTrue(testLn2.IsNegativeInfinity);
                Assert.IsTrue((5.0 + testLn2).IsNegativeInfinity);

                DecimalValue asin = Math.Asin(1.2);
                Assert.IsTrue(asin.IsNaN);

                DecimalValue testInfP = 5d / 0d;
                Assert.IsTrue(testInfP.IsPositiveInfinity);
                Assert.IsTrue((testInfP / 30d).IsPositiveInfinity);

                DecimalValue testInfM = -5d / 0d;
                Assert.IsTrue(testInfM.IsNegativeInfinity);
                Assert.IsTrue((testInfM / 30d).IsNegativeInfinity);

                Assert.IsTrue((testInfM + testInfP).IsNaN);
                Assert.IsTrue((testInfM / testInfP).IsNaN);
            }

            // BigDecimalValue
            {
                BigDecimalValue testLn = Math.Log(-2.5);
                Assert.IsTrue(testLn.IsNaN);
                Assert.IsTrue((5.0 + testLn).IsNaN);

                BigDecimalValue testLn2 = Math.Log(0);
                Assert.IsTrue(testLn2.IsNegativeInfinity);
                Assert.IsTrue((5.0 + testLn2).IsNegativeInfinity);

                BigDecimalValue asin = Math.Asin(1.2);
                Assert.IsTrue(asin.IsNaN);

                BigDecimalValue testInfP = 5d / 0d;
                Assert.IsTrue(testInfP.IsPositiveInfinity);
                Assert.IsTrue((testInfP / 30d).IsPositiveInfinity);

                BigDecimalValue testInfM = -5d / 0d;
                Assert.IsTrue(testInfM.IsNegativeInfinity);
                Assert.IsTrue((testInfM / 30d).IsNegativeInfinity);

                Assert.IsTrue((testInfM + testInfP).IsNaN);
                Assert.IsTrue((testInfM / testInfP).IsNaN);
            }
        }

    }
}
