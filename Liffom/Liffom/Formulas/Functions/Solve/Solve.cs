// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2024 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Liffom.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Formulas.Functions
{
    /// <summary>
    /// solve関数を表します。
    /// </summary>
    [Serializable()]
    public class Solve : Function
    {
        /// <summary>
        /// solve関数を初期化します。
        /// </summary>
        public Solve() : base() { }

        /// <summary>
        /// solve関数を初期化します。
        /// </summary>
        /// <param name="f">対象の方程式。</param>
        /// <param name="x">対象変数。</param>
        public Solve(Formula f, Variable x) : base(f, x) { }

        /// <summary>
        /// 引数を指定して、関数を生成します。
        /// </summary>
        /// <param name="args">初期化に用いる可変数の数式。</param>
        /// <returns>初期化された関数。</returns>
        public override Function CreateFunction(params Formula[] args)
        {
            return new Solve(args[0], args[1] as Variable);
        }

        /// <summary>
        /// この関数の引数として最低限必要な引数の数を取得します。
        /// </summary>
        public override int MinimumArgumentQty => 2;

        /// <summary>
        /// 関数に設定された引数により、関数を評価します。
        /// </summary>
        /// <returns>関数の評価結果。</returns>
        public override Formula CalculateFunction()
        {
            var eq = Argument[0] as Equal;
            var x  = Argument[1] as Variable;
            if (eq == null) throw new FormulaProcessException("第一引数には方程式を指定して下さい。");
            if (x == null) throw new FormulaProcessException("第二引数には求解対象とする変数を指定して下さい。");

            var solve = new SolveAlgebraicEquation();
            solve.AdmitImaginary = true;
            solve.AutoDeleteDenominator = true;
            //solve.CheckSignificantDigits = true;
            var ans = solve.Solve(eq, x);
            return ans;
        }

        /// <summary>
        /// 関数の説明を取得します。
        /// </summary>
        /// <param name="args">引数の説明。</param>
        /// <returns>関数の説明。</returns>
        public override string GetInformation(out List<string> args)
        {
            args = new List<string>()
            {
                "方程式", "求解対象の変数"
            };
            return "解の公式を用いて方程式の解を求めます。";
        }
    }
}
