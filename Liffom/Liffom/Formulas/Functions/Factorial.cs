// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Reals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Formulas.Functions
{
    /// <summary>
    /// 階乗を表します。
    /// </summary>
    [Serializable()]
    public class Factorial : Function
    {
        /// <summary>
        /// 階乗を返す関数を初期化します。
        /// </summary>
        public Factorial() : base(0, 1) { }

        /// <summary>
        /// 階乗を返す関数を初期化します。
        /// </summary>
        /// <param name="arg">対象の数式</param>
        public Factorial(Formula arg) : base(arg, 1) { }

        /// <summary>
        /// 階乗を返す関数を初期化します。
        /// </summary>
        /// <param name="arg">対象の数式</param>
        /// <param name="multiple">多重階乗数</param>
        public Factorial(Formula arg, Formula multiple) : base(arg, multiple) { }

        /// <summary>
        /// 引数を指定して、関数を生成します。
        /// </summary>
        /// <param name="args">初期化に用いるか変数の数式。</param>
        /// <returns>初期化された関数。</returns>
        public override Function CreateFunction(params Formula[] args)
        {
            if (args.Length < 2) return new Factorial(args[0]);
            else return new Factorial(args[0], args[1]);
        }

        /// <summary>
        /// この関数の引数として最低限必要な引数の数を取得します。
        /// </summary>
        public override int MinimumArgumentQty
        {
            get { return 1; }
        }

        /// <summary>
        /// この関数の引数として可能な引数の最大数を取得します。
        /// </summary>
        public override int MaximumArgumentQty
        {
            get { return 2; }
        }

        /// <summary>
        /// 関数の説明を取得します。
        /// </summary>
        /// <param name="args">引数の説明。</param>
        /// <returns>関数の説明。</returns>
        public override string GetInformation(out List<string> args)
        {
            args = new List<string>();
            args.Add("対象の自然数");
            args.Add("多重階乗数。既定は1です。");
            return "引数の階乗を返します。";
        }

        /// <summary>
        /// 関数に設定された引数により、関数を評価します。
        /// </summary>
        /// <returns>関数の評価結果。</returns>
        public override Formula CalculateFunction()
        {
            Numeric n = Argument[0] as Numeric;
            if (n == null || !n.IsInteger || n.Figure < 0) return this; // 対象が自然数でない
            Numeric m = Argument[1] as Numeric;
            if (m != null && (!m.IsInteger || m.Figure < 0)) return this; // 階乗数が自然数でない

            if (n.Figure == 0) return 1;

            Value result = n;
            Value multiple = result.CreateFrom(1);
            if (m != null) multiple = m;

            for (Value i = (n - multiple); i > 0; i -= multiple) result *= i;

            return result;
        }
    }
}
