using GoodSeat.Liffom.Formats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom.Formats.Numerics;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///FormatTest のテスト クラスです。すべての
    ///FormatTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class FormatTest
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
        ///SetDefaultProperty のテスト
        ///</summary>
		[TestCategory("出力書式"), TestMethod()]
        public void SetDefaultPropertyTest()
        {
            var test = Formula.Parse("456216.31515+4*x");
            string preUnique = test.GetUniqueText();
            Assert.AreEqual("456216.31515+4*x", test.ToString());

			// MEMO:以下のテストは、他の出力系のテストと同時に実行すると悪影響がある
            //FormatProperty property = new SplitFormatProperty(SplitFormatProperty.SplitType.Comma);
            //Format.SetDefaultProperty(property);
            //Assert.AreEqual("456,216.31515+4*x", test.ToString());

            //property = new OperatorFormatProperty(OperatorFormatProperty.OperatorType.Sum);
            //Format.SetDefaultProperty(property);
            //property = new SplitFormatProperty(SplitFormatProperty.SplitType.Space);
            //Format.SetDefaultProperty(property);
            //Assert.AreEqual("456 216.315 15 + 4*x", test.ToString());


            // 識別用文字列には変化のないことを確認
            string postUnique = test.GetUniqueText();
            Assert.AreEqual(preUnique, postUnique);
        }
    }
}
