// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2024 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Processes;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Formulas.Functions
{
    /// <summary>
    /// solveN関数を表します。
    /// </summary>
    [Serializable()]
    public class SolveN : Function
    {
        /// <summary>
        /// solveN関数を初期化します。
        /// </summary>
        public SolveN() : base() { }

        /// <summary>
        /// solveN関数を初期化します。
        /// </summary>
        /// <param name="f">対象の方程式。</param>
        /// <param name="x">対象変数。</param>
        public SolveN(Formula f, Variable x) : base(f, x) { }

        /// <summary>
        /// 引数を指定して、関数を生成します。
        /// </summary>
        /// <param name="args">初期化に用いる可変数の数式。</param>
        /// <returns>初期化された関数。</returns>
        public override Function CreateFunction(params Formula[] args)
        {
            return new SolveN() { Argument = new Operators.Argument(args) };
        }

        /// <summary>
        /// この関数として識別する文字列を取得します。
        /// </summary>
        public override string DistinguishedName => "solveN";

        /// <summary>
        /// この関数の引数として最低限必要な引数の数を取得します。
        /// </summary>
        public override int MinimumArgumentQty => 2;

        /// <summary>
        /// この関数の引数として可能な引数の最大数を取得します。既定では、MinimumArgumentQtyと同様の値を返します。
        /// </summary>
        public override int MaximumArgumentQty => 6;

        /// <summary>
        /// 関数に設定された引数により、関数を評価します。
        /// </summary>
        /// <returns>関数の評価結果。</returns>
        public override Formula CalculateFunction()
        {
            var eq  = Argument[0] as Equal;
            var x   = Argument[1] as Variable;
            if (Argument[1] is Unit)
            {
                x = new Variable((Argument[1] as Unit).GetText());
                eq = eq.Substitute(Argument[1], x) as Equal;
            }

            if (eq == null) throw new FormulaProcessException("第一引数には方程式を指定して下さい。");
            if (x == null) throw new FormulaProcessException("第二引数には求解対象とする変数を指定して下さい。");

            var solve = new NewtonMethod();

            if (Argument.Count > 2)
            {
                var x1  = Argument[2] as Numeric;
                if (x1 == null) throw new FormulaProcessException("第三引数には初期解を指定して下さい。");
                solve.InitialSolution = x1.Figure;
            }

            if (Argument.Count > 3)
            {
                var eps = Argument[3] as Numeric;
                if (eps == null) throw new FormulaProcessException("第四引数には許容誤差値を指定して下さい。");
                solve.ErrorTolerance = eps;
            }

            if (Argument.Count > 4)
            {
                var count = Argument[4] as Numeric;
                if (count == null || !count.IsInteger) throw new FormulaProcessException("第五引数には最大試行回数を整数で指定して下さい。");
                solve.MaxTryCount =(int)count;
            }

            if (Argument.Count > 5)
            {
                var inc = Argument[5] as Numeric;
                if (inc == null) throw new FormulaProcessException("第六引数には解の振動値を整数で指定して下さい。");
                solve.Increment = inc;
                solve.IncrementWidth = solve.Increment / 2.0;
            }

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
            var solve = new NewtonMethod();
            args = new List<string>()
            {
                "方程式", "求解対象の変数", $"初期解。既定は{solve.InitialSolution}です", $"許容誤差値。既定は{solve.ErrorTolerance}です", $"探索最大回数。既定は{solve.MaxTryCount}です", $"探索時の解の振動幅。既定は{solve.Increment}です"
            };
            return "ニュートン・ラフソン法を用いて方程式の近似解を一つ求めます。";
        }
    }
}
