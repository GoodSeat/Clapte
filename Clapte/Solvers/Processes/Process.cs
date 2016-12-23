using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Clapte.Solvers;

namespace GoodSeat.Clapte.Solvers.Processes
{
    /// <summary>
    /// Clapteの計算における各種処理を表します。
    /// </summary>
    public abstract class Process
    {
        /// <summary>
        /// 処理を初期化します。
        /// </summary>
        /// <param name="owner">処理の保持者となるソルバ。</param>
        public Process(Solver owner)
        {
            Owner = owner;
        }

        /// <summary>
        /// この処理の保持者となるソルバを設定もしくは取得します。
        /// </summary>
        protected Solver Owner { get; set; }

        /// <summary>
        /// 入力された文字列を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力文字列。</param>
        /// <param name="onlyCheckInput">数式の構文解析のみを目的とし、文字列のチェックのみを行うか否か。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public virtual Error CheckInputText(ref string input, bool onlyCheckInput) { return null; }

        /// <summary>
        /// 計算対象となった入力数式を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public virtual Error CheckInputFormula(ref Formula input) { return null; }

        /// <summary>
        /// 計算対象となった入力数式を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public virtual Error EvaluateFormula(ref Formula input) { return null; }

        /// <summary>
        /// 計算結果として得られた数式を対象として、処理を行います。
        /// </summary>
        /// <param name="output">処理対象の出力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public virtual Error CheckOutputFormula(ref Formula output) { return null; }

        /// <summary>
        /// 計算結果として得られた数式を対象として、処理を行います。
        /// </summary>
        /// <param name="output">処理対象の出力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public virtual Error CheckOutputText(ref string output) { return null; }

    }
}
