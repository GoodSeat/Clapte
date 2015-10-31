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
            var result = DecimalValue.Exp(2.345m, 28);
            //              12 34567890123456789012345678
            Assert.AreEqual(10.433272727548915163706029514m, result);
            //              10.4332727275489151637060295131 [keisan.casio.jp]
            //                                           ~~

            result = DecimalValue.Power(3m, 3m, 28);
            Assert.AreEqual(27m, result);

            result = DecimalValue.Power(3.151351m, 12.513151m, 28);
            Assert.AreEqual(1728882.4083962592610184374537m, result);
            //              1728882.40839625926101843745647 [keisan.casio.jp]
            //                                          ~~

            result = DecimalValue.Sin(DecimalValue.Pi / 6m, 28);
            Assert.AreEqual(0.4999999999999999999999999995m, result);
            //              0.5 [keisan.casio.jp]

            result = DecimalValue.Cos(DecimalValue.Pi / 6m, 28);
            Assert.AreEqual(0.8660254037844386467637231709m, result);
            //              0.866025403784438646763723170753 [keisan.casio.jp]
            //                                           ~~~

            result = DecimalValue.Tan(DecimalValue.Pi / 6m, 28);
            Assert.AreEqual(0.5773502691896257645091487798m, result);
            //              0.577350269189625764509148780502 [keisan.casio.jp]
            //                                         ~~~

            result = DecimalValue.Atan(1.2m, 28);
            Assert.AreEqual(0.8760580505981934231140475196m, result);
            //              0.876058050598193423114047521128 [keisan.casio.jp]
            //                                         ~~~
            result = DecimalValue.Atan(0.8m, 28);
            Assert.AreEqual(0.6747409422235526630565209738m, result);
            //              0.67474094222355266305652097361 [keisan.casio.jp]
            //                                         ~~~
            result = DecimalValue.Atan(-1.2m, 28);
            Assert.AreEqual(-0.8760580505981934231140475196m, result);
            //              -0.876058050598193423114047521128 [keisan.casio.jp]
            //                                         ~~~
            result = DecimalValue.Atan(-0.8m, 28);
            Assert.AreEqual(-0.6747409422235526630565209738m, result);
            //              -0.67474094222355266305652097361 [keisan.casio.jp]
            //                                         ~~~
        }
    }
}
