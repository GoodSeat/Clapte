// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Liffom.Formulas.Matrices;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Clapte.Solvers.Processes
{
    /// <summary>
    /// 計算結果の数式を篩う処理を表します。
    /// </summary>
    public class SieveResultFormulaProcess : Process
    {
        /// <summary>
        /// 計算結果の数式を篩う処理を初期化します。
        /// </summary>
        /// <param name="owner">処理の保持者となるソルバ。</param>
        /// <param name="onlySingleFactor">単項式のみを許可するか否か。</param>
        public SieveResultFormulaProcess(Solver owner, bool onlySingleFactor)
            : base(owner)
        {
            PermitOnlySingleFactor = onlySingleFactor;
        }

        private string InputTextCache { get; set; }

        private Formula InputFormulaCache { get; set; }

        /// <summary>
        /// 結果として、単項式のみを許可するか否かを設定もしくは取得します。
        /// </summary>
        public bool PermitOnlySingleFactor { get; set; }

        /// <summary>
        /// 入力された文字列を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力文字列。</param>
        /// <param name="onlyCheckInput">数式の構文解析のみを目的とし、文字列のチェックのみを行うか否か。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckInputText(ref string input, bool onlyCheckInput)
        {
            if (!onlyCheckInput) InputTextCache = input;
            return base.CheckInputText(ref input, onlyCheckInput);
        }

        /// <summary>
        /// 計算対象となった入力数式を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckInputFormula(ref Formula input) 
        {
            InputFormulaCache = input;
            return base.CheckInputFormula(ref input);
        }

        /// <summary>
        /// 計算結果として得られた数式を対象として、処理を行います。
        /// </summary>
        /// <param name="output">処理対象の出力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckOutputFormula(ref Formula output)
        {
            if (output == InputFormulaCache)
            {
                return new Error(Error.Level.Abort, "入力と計算後で変化がありません。");
            }
            if (PermitOnlySingleFactor)
            {
                // 許可されるのは、Product、Numeric、Matrix、Equal(最上位のみ)、Argument(Equalのrhsのみ)、Variable(Equalのlhsのみ)
                if (!IsValidAsResult(output, true, false))
                    return new Error(Error.Level.Abort, "計算結果が単項式となりませんでした。");
            }

            return base.CheckOutputFormula(ref output);
        }

        /// <summary>
        /// 計算結果として得られた数式を対象として、処理を行います。
        /// </summary>
        /// <param name="output">処理対象の出力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckOutputText(ref string output)
        {
            if (output == InputTextCache)
            {
                return new Error(Error.Level.Abort, "入力と計算後で変化がありません。");
            }
            return base.CheckOutputText(ref output);
        }

        /// <summary>
        /// 指定数式が単項式のみを許可する場合の解に適合するか否かを取得します。
        /// </summary>
        /// <param name="f">判定対象の数式。</param>
        /// <param name="permitEqual">等価判断式を許可するか。</param>
        /// <param name="permitArgument">引数を許可するか。</param>
        /// <returns>単項式の解として適合するか否か。</returns>
        private bool IsValidAsResult(Formula f, bool permitEqual, bool permitArgument)
        {
            if (f.IsUnit()) return true;
            if (f is Numeric) return true;
            if (f is Constant) return true;
            if (f is Unit) return true;
            if (f is Variable) return true;
            if (f is Product) return f.All(c => IsValidAsResult(c, false, false));
            if (f is Matrix) return f.All(c => c is Null || IsValidAsResult(c, false, false));
            if (f is Argument && permitArgument) return f.All(c => IsValidAsResult(c, false, false));
            if (f is Equal && permitEqual)
            {
                var equal = f as Equal;
                if (!(equal.LeftHandSide is Variable)) return false;
                return IsValidAsResult(equal.RightHandSide, false, true);
            }
            return false;
        }
    }
}
