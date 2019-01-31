// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Formulas.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Deforms;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///ImaginaryTest のテスト クラスです。すべての
    ///ImaginaryTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class ImaginaryTest
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
        ///GetRealAndImaginary のテスト
        ///</summary>
        [TestCategory("数値"), TestMethod()]
        public void GetRealAndImaginaryTest()
        {
            Formula f = Formula.Parse("5+x+i*z");
            Formula RExpected = Formula.Parse("5+x");
            Formula EExpected = Formula.Parse("z");
            Formula R, E; 
            bool result = Imaginary.GetRealAndImaginary(f, out R, out E);

            Assert.AreEqual(false, result);
            Assert.AreEqual(RExpected, R);
            Assert.AreEqual(EExpected, E);
        }

        /// <summary>
        /// 虚数関連の計算 のテスト
        /// </summary>
        [TestCategory("虚数"), TestMethod()]
        public void CalculateImaginaryTest()
        {
            DeformToken token = new DeformToken(Formula.SimplifyToken, Formula.CalculateToken, Formula.NumerateToken);

            DeformTokenTest.DeformTest(token, "sin(i)",             "1.1752011936438*i");
            DeformTokenTest.DeformTest(token, "sin(i  + 1)",        "1.29845758141598+0.634963914784736*i");
            DeformTokenTest.DeformTest(token, "cos(i)",             "1.54308063481524");
            DeformTokenTest.DeformTest(token, "cos(i + 1)",         "0.833730025131149-0.988897705762865*i");
            DeformTokenTest.DeformTest(token, "tan(i)",             "0.761594155955765*i");
            DeformTokenTest.DeformTest(token, "tan(i + 1)",         "0.271752585319512+1.0839233273387*i");
                                                                     
            DeformTokenTest.DeformTest(token, "i^i",                "0.207879576350762");
            DeformTokenTest.DeformTest(token, "ln(i)",              "1.5707963267949*i");
            DeformTokenTest.DeformTest(token, "ln(i+1)",            "0.346573590279972+0.785398163397448*i");
                                                                     
            DeformTokenTest.DeformTest(token, "-(2i)^(2i)",         "-0.00792789471147608-0.0424804804251522*i");
            DeformTokenTest.DeformTest(token, "(-2i)^(2i)",         "4.24532146387435+22.7479427903521*i");
            DeformTokenTest.DeformTest(token, "(-2)^(2i) * i^(2i)", "1.48048893566922E-05+7.93298644094137E-05*i");
                                                                     
            DeformTokenTest.DeformTest(token, "ln(-2 )",            "0.693147180559944+3.14159265358979*i");
            DeformTokenTest.DeformTest(token, "asin(2)",            "1.5707963267949-1.31695789692481*i");
            DeformTokenTest.DeformTest(token, "acos(2)",            "1.31695789692481*i");
            DeformTokenTest.DeformTest(token, "atan(2i)",           "1.5707963267949+0.549306144334054*i");

            DeformTokenTest.DeformTest(token, "sqrt(-8)",           "2.8284271247462*i");
			DeformTokenTest.DeformTest(token, "(-2)^1.5",           "-2.82842712474619*i");
			DeformTokenTest.DeformTest(token, "(2i)^i",             "0.159909056928068+0.13282699942462*i");
//			DeformTokenTest.DeformTest(token, "tan(355/226)",       "-7497258.18532558711"); // 内部数値がdoubleだと精度が足りないため、テストケースから除外
            DeformTokenTest.DeformTest(token, "((1-i)/(1+i))^8",    "1");
			DeformTokenTest.DeformTest(token, "i/(1-i)+(1-i)/i",    "-1.5 - 0.5*i");
			DeformTokenTest.DeformTest(token, "sqrt(-i)",           "0.707106781186547-0.707106781186548*i");
			DeformTokenTest.DeformTest(token, "(1-i)^(1+i)",        "2.80787929726062904-1.31786517290118086*i");
			DeformTokenTest.DeformTest(token, "2^(2i)",             "0.183456974743304+0.983027740411243*i");
			DeformTokenTest.DeformTest(token, "ln(-1)",             "3.14159265358979324*i");
			DeformTokenTest.DeformTest(token, "ln(i^i)",            "-1.57079632679489");
        }
    }
}
