using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Clapte.Solvers.Processes
{
    /// <summary>
    /// 計算対象とする文字列、及び数式を篩う処理を表します。
    /// </summary>
    public class SieveTargetInputProcess : Process
    {
        static string[] s_ops = { "+", "-", "*", "/", "^", "＋", "―", "－", "／", "×", "÷" };

        /// <summary>
        /// 計算対象とする文字列、及び数式を篩う処理を初期化します。
        /// </summary>
        /// <param name="owner">処理の保持者となるソルバ。</param>
        /// <param name="maxInputTextLength">数式として認識する最大文字列長。</param>
        /// <param name="owner">変数・単位名として認識する最大文字列長。</param>
        public SieveTargetInputProcess(Solver owner, int maxInputTextLength, int maxVariableTextLength)
            : base(owner) 
        {
            MaxInputTextLength = maxInputTextLength;
            MaxVariableTextLength = maxVariableTextLength;
        }

        /// <summary>
        /// 計算対象とする数式の最大文字列長を設定もしくは取得します。
        /// </summary>
        public int MaxInputTextLength { get; set; }

        /// <summary>
        /// 計算対象とする最大の変数・単位文字列長を設定もしくは取得します。
        /// </summary>
        public int MaxVariableTextLength { get; set; }

        /// <summary>
        /// 入力された文字列を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力文字列。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckInputText(ref string input)
        {
            if (input.Length > MaxInputTextLength)
                return new Error(Error.Level.Abort, "計算対象とする数式の最大文字列長を超過します。");

            bool isInvalid = true;
            input.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).Aggregate((s1, s2) =>  {
                if (!s_ops.Any(op => s1.EndsWith(op)) && !s_ops.Any(op => s2.StartsWith(op))) isInvalid = false;
                return s2;
            });
            if (!isInvalid)
                return new Error(Error.Level.Abort, "無効な位置の改行を含む計算式です。");
            
            return base.CheckInputText(ref input);
        }

        /// <summary>
        /// 計算対象となった入力数式を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckInputFormula(ref Formula input)
        {
            foreach (var check in input.GetExistFactors<Variable>())
            {
                if (check.Mark.Length > MaxVariableTextLength)
                    return new Error(Error.Level.Abort, "数式中に存在する変数名が、許容最大文字列長を超過します。");
            }
            foreach (var check in input.GetExistFactors<Unit>())
            {
                if (check.UnitName.Length > MaxVariableTextLength)
                    return new Error(Error.Level.Abort, "数式中に存在する単位名が、許容最大文字列長を超過します。");
            }

            return base.CheckInputFormula(ref input);
        }
    }
}
