using GoodSeat.Liffom.Formulas.Matrices;
using GoodSeat.Liffom.Parse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoodSeat.Liffom;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Liffom.Formulas.Operators.Booleans;

namespace GoodSeat.LiffomTestProject
{
    
    
    /// <summary>
    ///FormulaParserTest のテスト クラスです。すべての
    ///FormulaParserTest 単体テストをここに含めます
    ///</summary>
	[TestClass()]
	public class FormulaParserTest
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
		/// FormulaParser.Parse のテスト
		/// </summary>
		[TestCategory("構文解析"), TestMethod()]
		public void ParseTest()
		{
			UnitConvertTableTest.RegistBasicUnitRecord();

			Liffom.Parse.FormulaParser target = CreateTestTarget();

			// 加算と単項演算子のテスト
			AddParseTestCase(target, "-5*x^4 + 3", new Sum(new Product(-5, new Power(new Variable("x"), 4)), 3));
			// 空白トークンのテスト
			AddParseTestCase(target, "3+(5.2 +3)*2/4+3", new Sum(3, new Product((new Sum(5.2, 3)), 2, new Power(4, -1)), 3));
			// 絶対値のテスト
			AddParseTestCase(target, "3|-2^3|+3", new Product(3, new Abs(-1 * new Power(2, 3))) + 3);
			// 累乗のテスト
			AddParseTestCase(target, "-2^2", -1 * new Power(2, 2));
			AddParseTestCase(target, "2^2^3", new Power(new Power(2, 2), 3));
			AddParseTestCase(target, "(2^-3^2)^-1", new Power(new Power(2, -3), 2) ^ -1);
			// 積算記号省略のテスト
			AddParseTestCase(target, "6/2(1+2)", new Product(6, new Power(new Product(2, new Sum(1, 2)), -1)));
			// 乗算記号省略のテスト
			AddParseTestCase(target, "i2", new Power(Imaginary.i, 2));
			// 階乗記号のテスト
			AddParseTestCase(target, "2+4!/2", new Sum(2, new Product(new Factorial(4), new Power(2, -1))));
			AddParseTestCase(target, "2+5!!/3", new Sum(2, new Product(new Factorial(5, 2), new Power(3, -1))));
			// 関数のテスト
			AddParseTestCase(target, "sin(1)+cos(2)", new Sin(new Numeric("1")) + new Cos(new Numeric("2")));
			// 定数と変数のテスト
			AddParseTestCase(target, "sin(x)+cos(e)+ y", new Sum(new Sin(new Variable("x")), new Cos(new Napiers()), new Variable("y")));
			// 単位のテスト
			AddParseTestCase(target, "5[m]+5[cm]", new Product(5, new Unit("m")) + new Product(5, new Unit("cm")));
			// 波括弧のテスト
			AddParseTestCase(target, "{5+5}*2", new Product(new Sum(5, 5), 2));
			// 論理演算子のテスト
			AddParseTestCase(target, "x=2 && y=1", new And(new Equal(new Variable("x"), 2), new Equal(new Variable("y"), 1)));
			// 比較演算子のテスト
			AddParseTestCase(target, "x=2", new Equal(new Variable("x"), 2));
			// 定数のテスト
			AddParseTestCase(target, "x=i", new Equal(new Variable("x"), Imaginary.i));

			// 空テスト
			AddParseTestCase(target, "5 m+5 cm", new Product(5, new Variable("m")) + new Product(5, new Variable("cm")));
			// 単位読込を有効化
			foreach (var lexer in target.Lexers) if (lexer is VariableLexer) (lexer as VariableLexer).ScanAsUnit = true;
			// 単位のテスト
			AddParseTestCase(target, "5 m+5 cm", new Product(5, new Unit("m")) + new Product(5, new Unit("cm")));
			// 単位読込を無効化
			foreach (var lexer in target.Lexers) if (lexer is VariableLexer) (lexer as VariableLexer).ScanAsUnit = false;

			// 行ベクトルのテスト
			AddParseTestCase(target, "[5,2]", new Vector(true, 5, 2));
			// 列ベクトルのテスト
			AddParseTestCase(target, "{5,2}", new Vector(false, 5, 2));
			// マトリクスのテスト
			AddParseTestCase(target, "{[1,2],[3,4]}", new Matrix(new Vector(1, 2), new Vector(3, 4)));

			// 行列の演算のテスト
			AddParseTestCase(target, "{[1,2],[3,4]}.{[1,2],[3,4]}", 
					new ProductOfMatrix(
						new Matrix(new Vector(1, 2), new Vector(3, 4)),
						new Matrix(new Vector(1, 2), new Vector(3, 4))
						));
			AddParseTestCase(target, "A.B", new ProductOfMatrix(new Variable("A"), new Variable("B")));
			AddParseTestCase(target, "{[1,2],[3,4]}^^2", 
					new PowerOfMatrix(new Matrix(new Vector(1, 2), new Vector(3, 4)), 2));
			AddParseTestCase(target, "A^^2", new PowerOfMatrix(new Variable("A"), 2));

			// ユーザー定義関数の自動検知のテスト
			AddParseTestCase(target, "test(a, b)", new Product(new Variable("test"), new Argument(new Variable("a"), new Variable("b"))));
			foreach (var lexer in target.Lexers)
			{
				if (!(lexer is FunctionLexer)) continue;
				(lexer as FunctionLexer).ScanNotDefinedUserFunction = true;
				break;
			}
			var expected = new UserFunction("test");
			expected.Argument = new Argument(new Variable("a"), new Variable("b"));
			AddParseTestCase(target, "test(a, b)", expected);
			
		}

		private void AddParseTestCase(Liffom.Parse.FormulaParser target, string actual, Formula expected)
		{
			Assert.AreEqual(expected, target.Parse(actual));
		}

		private FormulaParser CreateTestTarget()
		{
			var target = new FormulaParser();
			target.Lexers.Add(new NumericLexer()); // 数値
			target.Lexers.Add(new ParenthesesLexer()); // 丸括弧
			target.Lexers.Add(new BracketLexer(true, true)); // 角括弧
			target.Lexers.Add(new BraceLexer()); // 波括弧
			target.Lexers.Add(new AbsolutePunctuationLexer()); // 絶対値記号
			target.Lexers.Add(new SpaceLexer()); // 空白トークン
			target.Lexers.Add(new FunctionLexer(target, true)); // 関数
			target.Lexers.Add(new ConstantLexer(true)); // 定数
			target.Lexers.Add(new VariableLexer(target)); // 変数

			target.AddOperatorParsers(new AbbreviatedPowerOperatorParser(target)); // 乗算の記号の省略を許可(通常の乗算より優先度を上げる)
			target.AddOperatorParsers(new PowerOperatorParser()); // 累乗
			target.AddOperatorParsers(new PowerOfMatrixOperatorParser()); // 行列の累乗
			target.AddOperatorParsers(new PlusMinusOperatorParser()); // 正負記号
			target.AddOperatorParsers(new AbbreviatedProductOperatorParser()); // 積算記号の省略を許可(通常の積算より優先度を上げる)
			target.AddOperatorParsers(new ProductOfMatrixOperatorParser()); // 行列の乗算
			target.AddOperatorParsers(new ProductOperatorParser()); // 乗算・除算
			target.AddOperatorParsers(new SumOperatorParser()); // 和算・減算
			target.AddOperatorParsers(new FactorialOperatorParser()); // 階乗記号(!)
			target.AddOperatorParsers(new ComparerOperatorParser()); // 比較演算子
			target.AddOperatorParsers(new BooleanOperatorParser()); // 論理演算子
			target.AddOperatorParsers(new ArgumentOperatorParser()); // 引数演算子
			
			return target;
		}


		/// <summary>
		///AnalyzeLexical のテスト
		///</summary>
		[TestCategory("構文解析"), TestMethod()]
		[DeploymentItem("Liffom.dll")]
		public void AnalyzeLexicalTest()
		{
			Liffom.Parse.FormulaParser_Accessor target = new Liffom.Parse.FormulaParser_Accessor();
			target.Lexers.Add(new NumericLexer());
			target.Lexers.Add(new ParenthesesLexer());
			target.AddOperatorParsers(new ProductOperatorParser(), new SumOperatorParser());

			string text = "(5.0E+2+3)*2/4+3";
			StartToken actual;
			actual = target.AnalyzeLexical(text);

			Token testToken = actual.TopToken;
			Assert.IsTrue(testToken is StartToken);
			testToken = testToken.NextToken;
			Assert.IsTrue(testToken.TargetText == "(" && testToken is PunctuationToken);
			testToken = testToken.NextToken;
			Assert.IsTrue(testToken.TargetText == "5.0E+2" && testToken is FormulaToken);
			testToken = testToken.PreviousToken;
			Assert.IsTrue(testToken.TargetText == "(" && testToken is PunctuationToken);
		}
	}
}
