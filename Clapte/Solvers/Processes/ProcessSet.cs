using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Clapte.Solvers.Processes
{
    /// <summary>
    /// 複数のプロセスから成るプロセスを表します。
    /// </summary>
    public class ProcessSet : Process
    {
        /// <summary>
        /// 複数のプロセスから成るプロセスを初期化します。
        /// </summary>
        /// <param name="owner">処理の保持者となるソルバ。</param>
        /// <param name="processes">プロセスを構成するか変数のプロセス。</param>
        public ProcessSet(Solver owner, params Process[] processes)
            : base(owner)
        {
            ConsistProcesses = new List<Process>(processes);
        }

        /// <summary>
        /// プロセスを構成するプロセスリストを取得します。
        /// </summary>
        public List<Process> ConsistProcesses { get; private set; }



        /// <summary>
        /// 入力された文字列を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力文字列。</param>
        /// <param name="onlyCheckInput">数式の構文解析のみを目的とし、文字列のチェックのみを行うか否か。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckInputText(ref string input, bool onlyCheckInput)
        {
            Error result = null;
            int minLevel = int.MaxValue;
            foreach (var process in ConsistProcesses)
            {
                var thisResult = process.CheckInputText(ref input, onlyCheckInput);

                int level = 0;
                if (thisResult != null) level = (int)thisResult.ErrorLevel;
                
                if (level < minLevel)
                {
                    minLevel = level;
                    result = thisResult;
                }
            }
            return result;
        }

        /// <summary>
        /// 計算対象となった入力数式を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckInputFormula(ref Formula input)
        {
            Error result = null;
            int minLevel = int.MaxValue;
            foreach (var process in ConsistProcesses)
            {
                var thisResult = process.CheckInputFormula(ref input);
                
                int level = 0;
                if (thisResult != null) level = (int)thisResult.ErrorLevel;
                
                if (level < minLevel)
                {
                    minLevel = level;
                    result = thisResult;
                }
            }
            return result;
        }

        /// <summary>
        /// 計算対象となった入力数式を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error EvaluateFormula(ref Formula input)
        {
            Error result = null;
            int minLevel = int.MaxValue;
            foreach (var process in ConsistProcesses)
            {
                var thisResult = process.EvaluateFormula(ref input);
                
                int level = 0;
                if (thisResult != null) level = (int)thisResult.ErrorLevel;
                
                if (level < minLevel)
                {
                    minLevel = level;
                    result = thisResult;
                }
            }
            return result;
        }

        /// <summary>
        /// 計算結果として得られた数式を対象として、処理を行います。
        /// </summary>
        /// <param name="output">処理対象の出力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckOutputFormula(ref Formula output)
        {
            Error result = null;
            int minLevel = int.MaxValue;
            foreach (var process in ConsistProcesses)
            {
                var thisResult = process.CheckOutputFormula(ref output);
                
                int level = 0;
                if (thisResult != null) level = (int)thisResult.ErrorLevel;
                
                if (level < minLevel)
                {
                    minLevel = level;
                    result = thisResult;
                }
            }
            return result;
        }

        /// <summary>
        /// 計算結果として得られた数式を対象として、処理を行います。
        /// </summary>
        /// <param name="output">処理対象の出力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckOutputText(ref string output)
        {
            Error result = null;
            int minLevel = int.MaxValue;
            foreach (var process in ConsistProcesses)
            {
                var thisResult = process.CheckOutputText(ref output);
                
                int level = 0;
                if (thisResult != null) level = (int)thisResult.ErrorLevel;
                
                if (level < minLevel)
                {
                    minLevel = level;
                    result = thisResult;
                }
            }
            return result;
        }
    }
}
