using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formats.Numerics;

namespace LiffomTestProject
{
    [TestClass]
    public class SignificantFiguresTest
    {
        /// <summary>
        /// 加算における有効数字の取り扱いに関するテスト。
        /// </summary>
        [TestCategory("有効数字"), TestMethod()]
        public void SignificantFigureOfSumTest()
        {
            var format = new ConsiderSignificantFiguresFormatProperty(true);
            {
                var n1 = Formula.Parse("6.72");
                Assert.AreEqual(3, (n1 as Numeric).SignificantDigits);

                var n2 = Formula.Parse("0.542");
                Assert.AreEqual(3, (n2 as Numeric).SignificantDigits);

                var n3 = Formula.Parse("10.3");
                Assert.AreEqual(3, (n3 as Numeric).SignificantDigits);

                var nsum = (n1 + n2 + n3).Numerate();
                Assert.AreEqual(3, (nsum as Numeric).SignificantDigits);

                Assert.AreEqual("17.562", nsum.ToString());
                nsum.Format.SetProperty(format);
                Assert.AreEqual("1.76E+1", nsum.ToString());
            }
            { // 1.00 + 0.2 = 1.2
                var n1 = Formula.Parse("1.00");
                var n2 = Formula.Parse("0.2");
                Assert.AreEqual(2, ((n1 + n2).Numerate() as Numeric).SignificantDigits);
            }
            { // 4.25 - 4.20 = 5E-2
                var n1 = Formula.Parse("4.25");
                var n2 = Formula.Parse("4.20");
                Assert.AreEqual(1, ((n1 - n2).Numerate() as Numeric).SignificantDigits);
            }
        }

        /// <summary>
        /// 乗算における有効数字の取り扱いに関するテスト。
        /// </summary>
        [TestCategory("有効数字"), TestMethod()]
        public void SignificantFigureOfProductTest()
        {
            var format = new ConsiderSignificantFiguresFormatProperty(true);

            {
                var n1 = Formula.Parse("13.57");
                var n2 = Formula.Parse("4.56");
                var result = (n1 * n2).Numerate();

                Assert.AreEqual(3, (result as Numeric).SignificantDigits);

                Assert.AreEqual("61.8792", result.ToString());
                result.Format.SetProperty(format);
                Assert.AreEqual("6.19E+1", result.ToString());
            }
        }

        /// <summary>
        /// 除算における有効数字の取り扱いに関するテスト。
        /// </summary>
        [TestCategory("有効数字"), TestMethod()]
        public void SignificantFigureOfDivideTest()
        {
            var format = new ConsiderSignificantFiguresFormatProperty(true);

            {
                var n1 = Formula.Parse("13.57425");
                var n2 = Formula.Parse("4.50");
                var result = (n1 / n2).Numerate();

                Assert.AreEqual(3, (result as Numeric).SignificantDigits);

                Assert.AreEqual("3.0165", result.ToString());
                result.Format.SetProperty(format);
                Assert.AreEqual("3.02", result.ToString());
            }
        }


    }
}
