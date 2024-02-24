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
    }
}
