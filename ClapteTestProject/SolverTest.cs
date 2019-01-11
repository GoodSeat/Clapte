using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoodSeat.Clapte.Solvers;
using System.Collections.Generic;
using GoodSeat.Clapte.Solvers.Processes;
using GoodSeat.Clapte.ViewModels;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Formulas.Rules;
using GoodSeat.Liffom.Parse;
using GoodSeat.Liffom.Formats;
using GoodSeat.Liffom.Formats.Numerics;
using GoodSeat.Liffom.Formats.Powers;

namespace GoodSeat.ClapteTestProject
{
    [TestClass]
    public class SolverTest
    {
        /// <summary>
        /// テストに使用可能な標準的なSolverを初期化して取得します。
        /// </summary>
        /// <param name="mode">計算モード。</param>
        /// <param name="considerDigit">有効数字を考慮するか。</param>
        /// <returns>初期化されたSolve。</returns>
        public static Solver CreateStandardSolver(CalculateMode mode, bool considerDigit)
        {
            var solver = new Solver();

            solver.Parser = CreateFormulaParser();
            solver.ProcessList = CreateProcessList(solver, mode, considerDigit);
            solver.OutputFormat = CreateFormat(considerDigit);

			return solver;
        }

		/// <summary>
		/// Solverの標準的な数式の構文解析器を初期化して取得します。
		/// </summary>
		/// <returns>初期化された数式の構文解析器。</returns>
		static private FormulaParser CreateFormulaParser()
		{
			var parser = new FormulaParser();
			parser.Lexers.Add(new NumericLexer()); // 数値
			parser.Lexers.Add(new ParenthesesLexer()); // 丸括弧
			parser.Lexers.Add(new BracketLexer(true, true)); // 角括弧
			parser.Lexers.Add(new BraceLexer()); // 波括弧
			if (true) parser.Lexers.Add(new AbsolutePunctuationLexer()); // 絶対値記号
			parser.Lexers.Add(new SpaceLexer()); // 空白トークン
			parser.Lexers.Add(new FunctionLexer(parser, true)); // 関数
			parser.Lexers.Add(new ConstantLexer(true)); // 定数
			parser.Lexers.Add(new VariableLexer(parser, false)); // 変数

			if (true) parser.AddOperatorParsers(new AbbreviatedPowerOperatorParser(parser)); // 乗算記号の省略を許可(通常の乗算より優先度を上げる)
			parser.AddOperatorParsers(new PowerOperatorParser()); // 累乗
			parser.AddOperatorParsers(new PowerOfMatrixOperatorParser()); // 行列の累乗
			parser.AddOperatorParsers(new PlusMinusOperatorParser()); // 正負記号
			if (true) parser.AddOperatorParsers(new AbbreviatedProductOperatorParser()); // 積算記号の省略を許可(通常の積算より優先度を上げる)
			parser.AddOperatorParsers(new ProductOfMatrixOperatorParser()); // 行列の乗算
			parser.AddOperatorParsers(new ProductOperatorParser()); // 乗算・除算
			parser.AddOperatorParsers(new SumOperatorParser()); // 和算・減算
			if (true) parser.AddOperatorParsers(new FactorialOperatorParser()); // 階乗記号(!)
			parser.AddOperatorParsers(new ComparerOperatorParser()); // 比較演算子
			parser.AddOperatorParsers(new BooleanOperatorParser()); // 論理演算子
			parser.AddOperatorParsers(new ArgumentOperatorParser()); // 引数演算子
			return parser;
		}


        /// <summary>
        /// Solverの標準的なプロセスリストを生成して取得します。
        /// </summary>
		/// <param name="solver">設定対象となるソルバ。</param>
		/// <param name="mode">計算モード。</param>
        /// <param name="considerDigits">有効数字を考慮するか否か。</param>
        /// <returns></returns>
        static private List<Process> CreateProcessList(Solver solver, CalculateMode mode, bool considerDigits)
        {
			var result = new List<Process>();

            int maxInputTextLength = 1000;
            int maxVariableTextLength = 100;

			// 文字列長チェック
			result.Add(new SieveTargetInputProcess(solver, maxInputTextLength, maxVariableTextLength));

			// 出力文字列の半角/全角判定、変換
			result.Add(new OutputTextByteFormatProcess(solver, CharaType.Auto));

			// 全角文字を相当する文字列に置換
			result.Add(new ReplaceAlternateStringProcess(solver));

			// 数値の3桁区切りカンマの認識と出力書式への反映
			result.Add(new DetectSplitCommmaProcess(solver));
			
			// ユーザー定義定数・関数の置き換え
			result.Add(new EvaluateUserDefineProcess(solver));
			
			// 変数を単位で置き換え
			result.Add(new ReplaceVariableToUnitProcess(solver, ReplaceVariableToUnitProcess.Mode.AllAutoDetect));

			// 計算処理
			result.Add(CreateCalculateProcess(solver, mode, considerDigits));

            // 単位表記の調整
            result.Add(new SetUnitFormatProcess(solver, UnitFormatType.Auto));

			// 計算結果の抽出処理
			result.Add(new SieveResultFormulaProcess(solver, false));

            return result;
        }

		/// <summary>
		/// 設定に基づいて数式の計算処理を初期化して取得します。
		/// </summary>
		/// <param name="solver">設定対象となるソルバ。</param>
		/// <param name="mode">計算モード。</param>
        /// <param name="considerDigits">有効数字を考慮するか否か。</param>
		/// <returns>初期化された数式の計算処理。</returns>
		static private Process CreateCalculateProcess(Solver solver, CalculateMode mode, bool considerDigits)
		{
			var list = new List<Clapte.Solvers.Processes.Process>();

			// 目標単位の認識と変換
            if (mode == CalculateMode.Fraction) list.Add(new ConvertToSpecifiedUnitProcess(solver, Formula.CombineToken));
            else list.Add(new ConvertToSpecifiedUnitProcess(solver, Formula.NumerateToken));

			// 等式に対する方程式の求解
			var solveList = new List<GoodSeat.Liffom.Processes.SolveEquation>();
			solveList.Add(new GoodSeat.Liffom.Processes.SolveAlgebraicEquation());
			solveList.Add(new GoodSeat.Liffom.Processes.NewtonMethod());
			solveList.Add(new GoodSeat.Liffom.Processes.BrentMethod());

			if (solveList.Count != 0) list.Add(new SolveEquationProcess(solver, 10000, solveList.ToArray()));

            // 有効数字考慮設定
            solveList.ForEach(s => s.CheckSignificantDigits = considerDigits);

			// 計算処理
			List<DeformToken> tokenList = new List<DeformToken>();
			if (mode == CalculateMode.Fraction)
			{
				var token = new DeformToken(new SimplifyToken(), new CalculateToken());
				token.AdditionalTryRules.Add(ConvertToFractionRule.Entity);
				tokenList.Add(token);
			}
			else
			{
				var token = new DeformToken(new SimplifyToken(), new NumerateToken(), new CalculateToken());
				tokenList.Add(token);
			}
			list.Add(new CalculateFormulaProcess(solver, 10000, tokenList.ToArray()));

			return new ProcessSet(solver, list.ToArray());
		}

        /// <summary>
        /// Solverの標準的な出力書式を初期化して取得します。
        /// </summary>
        /// <param name="considerDigits">有効数字を考慮するか否か。</param>
        /// <returns></returns>
		static private Format CreateFormat(bool considerDigits)
		{
			Format format = new Format();

			// 有効数値の考慮
            var considerDigit = new ConsiderSignificantFiguresFormatProperty(considerDigits);
			format.SetProperty(considerDigit);

            // x^-nは、1/x^n形式で表示
            var divisionFormat = new DivisionFormatProperty(true);
            format.SetProperty(divisionFormat);

			return format;
		}

    }
}
