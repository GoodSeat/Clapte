// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2020 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas.Operators;
using System.Drawing.Drawing2D;
using System.Drawing;
using GoodSeat.Liffom.Reals;

namespace GoodSeat.Liffom.Formulas.Functions
{
    /// <summary>
    /// size関数を表します。
    /// </summary>
    [Serializable()]
    public class Size : Function
    {
        /// <summary>
        /// size関数を初期化します。
        /// </summary>
        public Size() : base() { }

        /// <summary>
        /// size関数を初期化します。
        /// </summary>
        /// <param name="f">対象の数式。</param>
        public Size(Formula f) : base(f) { }

        /// <summary>
        /// 引数を指定して、関数を生成します。
        /// </summary>
        /// <param name="args">初期化に用いるか変数の数式。</param>
        /// <returns>初期化された関数。</returns>
        public override Function CreateFunction(params Formula[] args)
        {
            return new Size(args[0]);
        }

        public override Formula CalculateFunction()
        {
            int result = 0;
            if (Argument[0] == null) return result;

            int i = 0;
            while (Argument[0][i] != null)
            {
                if (Argument[0][i++] is Null) continue;
                ++result;
            }
            return result;
        }

        public override int MinimumArgumentQty
        {
            get { return 1; }
        }

        public override string GetInformation(out List<string> args)
        {
            args = new List<string>(); args.Add("数式");
            return "マトリクス、ベクトル等の数式を構成する要素数を返します。";
        }

    }
}

