using GoodSeat.Liffom.Formats.Numerics;
using GoodSeat.Liffom.Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GoodSeat.LiffomTestProject
{
    [TestClass]
    public class RadixConvertFormatPropertyTest
    {
        [TestMethod]
        public void Convert0xTest()
        {
            var target = new RadixConvertFormatProperty();
            target.Mode = RadixConvertFormatProperty.RadixConvertMode._0x;

            {
                var n1 = new Numeric("0xABCDEF");

                var t1 = target.Convert(n1);
                var n2 = new Numeric(target.GetPrefix() + t1);
                Assert.AreEqual(n1, n2);
                Assert.AreEqual("abcdef", target.Convert(n1));
            }
            {
                var n1 = new Numeric("0xABCDEF0000000");

                var t1 = target.Convert(n1);
                var n2 = new Numeric(target.GetPrefix() + t1);
                Assert.AreEqual(n1, n2);
                Assert.AreEqual("a.bcdefE+c", target.Convert(n1));
            }
            {
                var n1 = new Numeric("0xabc.de");

                var t1 = target.Convert(n1);
                var n2 = new Numeric(target.GetPrefix() + t1);
                Assert.AreEqual(n1, n2);
                Assert.AreEqual("abc.de", target.Convert(n1));
            }
            {
                var n1 = new Numeric("0x0.00000abcde");

                var t1 = target.Convert(n1);
                var n2 = new Numeric(target.GetPrefix() + t1);
                Assert.AreEqual(n1, n2);
                Assert.AreEqual("a.bcdeE-6", target.Convert(n1));
            }

        }

        [TestMethod]
        public void Convert0bTest()
        {
            var target = new RadixConvertFormatProperty();
            target.Mode = RadixConvertFormatProperty.RadixConvertMode._0b;

            {
                var n1 = (new Numeric("0b10") * -1).Numerate() as Numeric;

                var t1 = target.Convert(n1);
                var n2 = new Numeric(target.GetPrefix() + t1);
                Assert.AreEqual(n1, n2.Numerate() as Numeric);
                Assert.AreEqual("-10", target.Convert(n1));
            }
            {
                var n1 = new Numeric("8.5397096498719");

                var t1 = target.Convert(n1);
                var n2 = new Numeric(target.GetPrefix() + t1);
                Assert.AreEqual(n1, n2);
                Assert.AreEqual("1000.1000101000101010011010010101111110001001000100010101101101000100011111000", target.Convert(n1));
            }

        }
    }
}
