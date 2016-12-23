using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formats.Numerics;

namespace GoodSeat.Clapte.Solvers.Processes
{
    /// <summary>
    /// 数値の3桁区切りを検知し、出力書式に反映する処理を表します。
    /// </summary>
    public class DetectSplitCommmaProcess : Process
    {
        static DetectSplitCommmaProcess()
        {
            SplitNumericRegex = new Regex(@"[1-9][0-9]{0,2}(\,[0-9]{3})+(\D|$)", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// カンマによる3桁区切りが行われている数値文字列を検査する正規表現を取得します。
        /// </summary>
        public static Regex SplitNumericRegex { get; private set; }

        /// <summary>
        /// 数値の3桁区切りを検知し、出力書式に反映する処理を初期化します。
        /// </summary>
        public DetectSplitCommmaProcess(Solver owner) : base(owner) { }

        /// <summary>
        /// 現在処理中の数式の入力文字列で、3桁区切りのカンマが使用されていたか否かを設定もしくは取得します。
        /// </summary>
        private bool ExistSplitComma { get; set; } 

        /// <summary>
        /// 入力された文字列を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力文字列。</param>
        /// <param name="onlyCheckInput">数式の構文解析のみを目的とし、文字列のチェックのみを行うか否か。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckInputText(ref string input, bool onlyCheckInput) 
        {
            if (!onlyCheckInput) ExistSplitComma = false;

            var matchs = SplitNumericRegex.Matches(input);
            foreach (var match in matchs)
            {
                if (!onlyCheckInput) ExistSplitComma = true;
                input = input.Replace(match.ToString(), match.ToString().Replace(",", ""));
            }

            return base.CheckInputText(ref input, onlyCheckInput);
        }

        /// <summary>
        /// 計算結果として得られた数式を対象として、処理を行います。
        /// </summary>
        /// <param name="output">処理対象の出力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckOutputFormula(ref Formula output)
        {
            if (ExistSplitComma)
            {
                output.Format.SetProperty(new SplitFormatProperty(SplitFormatProperty.SplitType.Comma));
            }

           return base.CheckOutputFormula(ref output);
        }
    }
}
