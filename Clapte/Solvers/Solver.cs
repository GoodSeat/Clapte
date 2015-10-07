using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Parse;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formats;
using GoodSeat.Clapte.Solvers.Processes;
using GoodSeat.Liffom;

namespace GoodSeat.Clapte.Solvers
{
	/// <summary>
	/// 数式の計算処理を表します。
	/// </summary>
	public class Solver
	{
		/// <summary>
		/// 処理の段階を表します。
		/// </summary>
		enum Step
		{
			/// <summary>
			/// 入力文字列の検査段階を表します。
			/// </summary>
			CheckInputText,
			/// <summary>
			/// 入力数式の検査段階を表します。
			/// </summary>
			CheckInputFormula,
			/// <summary>
			/// 入力数式の評価段階を表します。
			/// </summary>
			EvaluateFormula,
			/// <summary>
			/// 出力数式の検査段階を表します。
			/// </summary>
			CheckOutputFormula,
			/// <summary>
			/// 出力文字列の検査段階を表します。
			/// </summary>
			CheckOutputText
		}

		/// <summary>
		/// 数式の計算処理を初期化します。
		/// </summary>
		public Solver()
		{
			ProcessList = new List<Process>();
			AbortLevel = Error.Level.Abort;
            UserConstants = new List<ConstantDefine>();
            UserFunctions = new List<FunctionDefine>();
		}

		/// <summary>
		/// 適用する処理リストを設定もしくは取得します。
		/// </summary>
		public List<Process> ProcessList { get; set; }

		/// <summary>
		/// 計算を中止するエラーレベルを設定もしくは取得します。
		/// </summary>
		public Error.Level AbortLevel { get; set; }

		/// <summary>
		/// 数式の構文解析器を設定もしくは取得します。
		/// </summary>
		public FormulaParser Parser { get; set; }

		/// <summary>
		/// 数式の出力書式を設定もしくは取得します。
		/// </summary>
		public Format OutputFormat { get; set; }

		/// <summary>
		/// Solverで用いる定数定義リストを設定もしくは取得します。
		/// </summary>
		public IList<ConstantDefine> UserConstants { get; set; }

		/// <summary>
		/// Solverで用いる関数定義リストを設定もしくは取得します。
		/// </summary>
		public IList<FunctionDefine> UserFunctions { get; set; }

		/// <summary>
		/// 登録済みのプロセスリストから、指定した型のプロセスを探索して取得します。
		/// </summary>
		/// <returns>指定型の登録済みプロセス。</returns>
		public T GetProcessOf<T>() where T : Process
		{
			foreach (var process in ProcessList)
			{
				if (process is T) return process as T;
			}
			return null;
		}
			 

		/// <summary>
		/// 文字列を指定して、処理を実行します。
		/// </summary>
		/// <param name="input">処理対象の文字列。</param>
		/// <returns>処理結果。</returns>
		public Result Solve(string input)
		{
			string output = input;
			Formula formula = null;
			List<Error> errors = new List<Error>();
			Result result = null;

			try
			{
				result = DoProcess(Step.CheckInputText, ref input, ref formula, errors);
				if (result != null) return result;

				formula = Parser.Parse(input);

				result = DoProcess(Step.CheckInputFormula, ref input, ref formula, errors);
				if (result != null) return result;

				result = DoProcess(Step.EvaluateFormula, ref input, ref formula, errors);
				if (result != null) return result;

				formula.Format = OutputFormat;

				result = DoProcess(Step.CheckOutputFormula, ref input, ref formula, errors);
				if (result != null) return result;

				output = formula.ToString();

				result = DoProcess(Step.CheckOutputText, ref output, ref formula, errors);
				if (result != null) return result;

				return new Result(Result.Level.Success, output, formula, errors.ToArray());
			}
			catch (FormulaParseException e)
			{
				errors.Add(new Error(Error.Level.Error, e.Message));
				return new Result(Result.Level.Error, e.Message, null, errors.ToArray());
			}
			catch (ClapteProcessException e)
			{
				errors.Add(new Error(Error.Level.Error, e.Message));
				return new Result(Result.Level.Error, e.Message, null, errors.ToArray());
			}
			catch (Exception e)
			{
				errors.Add(new Error(Error.Level.Error, e.Message));
				return new Result(Result.Level.Error, e.Message, null, errors.ToArray());
			}
		}

		/// <summary>
		/// 文字列を数式に変換して取得します。
		/// </summary>
		/// <param name="input">変換対象の文字列。</param>
		/// <param name="f">変換された数式。変換に失敗した場合、null。</param>
		/// <returns>数式への変換に成功したか否か。</returns>
		public bool TryParse(string input, out Formula f)
		{
			f = null;
			try
			{
				f = Parse(input);
			}
			catch (FormulaParseException e)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// 文字列を数式に変換して取得します。
		/// </summary>
		/// <param name="input">変換対象の文字列。</param>
		/// <returns>変換された数式。</returns>
		public Formula Parse(string input)
		{
			List<Error> errors = new List<Error>();
			Formula formula = null;
			Result result = null;

			result = DoProcess(Step.CheckInputText, ref input, ref formula, errors);
			if (result != null) throw new FormulaParseException(result.ResultText);

			return Parser.Parse(input);
		}

		/// <summary>
		/// ステップを指定して、処理を実行します。
		/// </summary>
		/// <param name="step">処理段階を表すステップ。</param>
		/// <param name="text">処理中の文字列。</param>
		/// <param name="formula">処理中の数式。</param>
		/// <param name="errors">検知されたエラーリスト。</param>
		/// <returns>計算を中断すべきエラーを含む結果。中断の必要のない場合、null。</returns>
		private Result DoProcess(Step step, ref string text, ref Formula formula, List<Error> errors)
		{
			foreach (var process in ProcessList)
			{
				Error error = null;
				switch (step)
				{
					case Step.CheckInputText: 
						error = process.CheckInputText(ref text);
						break;
					case Step.CheckInputFormula: 
						error = process.CheckInputFormula(ref formula);
						break;
					case Step.EvaluateFormula: 
						error = process.EvaluateFormula(ref formula);
						break;
					case Step.CheckOutputFormula: 
						error = process.CheckOutputFormula(ref formula);
						break;
					case Step.CheckOutputText: 
						error = process.CheckOutputText(ref text);
						break;
					default:
						throw new NotImplementedException();
				}

				if (error == null) continue;

				errors.Add(error);
				if ((int)error.ErrorLevel >= (int)AbortLevel)
				{
					Result.Level level = Result.Level.Error;
					if (error.ErrorLevel == Error.Level.Abort) level = Result.Level.Abort;
					return new Result(level, error.Message, null, errors.ToArray());
				}
			}
			return null;
		}

	}
}
