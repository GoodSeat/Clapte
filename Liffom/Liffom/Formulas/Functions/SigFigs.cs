// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2024 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Formats.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Formulas.Functions
{
    /// <summary>
    /// sigfigs関数を表します。
    /// </summary>
    [Serializable()]
    public class SigFigs : Function
    {
        /// <summary>
        /// sigfigs関数を初期化します。
        /// </summary>
        public SigFigs() : base() { }

        /// <summary>
        /// sigfigs関数を初期化します。
        /// </summary>
        /// <param name="f">対象の数式。</param>
        public SigFigs(Formula f) : base(f) { }

        /// <summary>
        /// 引数を指定して、関数を生成します。
        /// </summary>
        /// <param name="args">初期化に用いるか変数の数式。</param>
        /// <returns>初期化された関数。</returns>
        public override Function CreateFunction(params Formula[] args)
        {
            return new SigFigs(args[0]);
        }

        public override Formula CalculateFunction()
        {
            var f = Argument[0];
            f.Format.SetProperty(new ConsiderSignificantFiguresFormatProperty(true));
            return f;
        }

        public override int MinimumArgumentQty
        {
            get { return 1; }
        }

        public override string GetInformation(out List<string> args)
        {
            args = new List<string>()
            {
                "数値"
            };
            return "指定数式内における数値を、有効数字を考慮した表記に変換して返します。";
        }
    }
}
