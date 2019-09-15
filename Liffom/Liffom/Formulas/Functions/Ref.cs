// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Formulas.Operators;
using System;
using System.Collections.Generic;

namespace GoodSeat.Liffom.Formulas.Functions
{
    /// <summary>
    /// 参照関数を表します。
    /// </summary>
    [Serializable()]
    public class Ref : Function
    {
        /// <summary>
        /// 参照関数を初期化します。
        /// </summary>
        public Ref() : base() { }

        /// <summary>
        /// 参照関数を初期化します。
        /// </summary>
        /// <param name="f">対象数式。</param>
        /// <param name="index">参照インデックス。</param>
        public Ref(Formula f, Formula index) : base(f, index) { }

        public override int MinimumArgumentQty
        {
            get { return 2; }
        }

        public override Formula CalculateFunction()
        {
            var target = Argument[0];
            var index = Argument[1];

            if (target is Matrices.Matrix && index is Argument)
            {
                var arg = index as Argument;
                if (arg.Count != 2) return this;

                var r = arg[0] as Numeric;
                var c = arg[1] as Numeric;
                if (r == null || !r.IsInteger) return this;
                if (c == null || !c.IsInteger) return this;

                var m = target as Matrices.Matrix;

                var res = m[(int)r, (int)c];
                if (res == null) throw new IndexOutOfRangeException();

                return res;
            }
            else
            {
                var n = index as Numeric;
                if (n == null || !n.IsInteger) return this;

                var res = target[(int)n];
                if (res == null) throw new IndexOutOfRangeException();

                return res;
            }
        }

        public override Function CreateFunction(params Formula[] args)
        {
            return new Ref(args[0], args[1]);
        }

        public override string GetInformation(out List<string> args)
        {
            args = new List<string>(); args.Add("対象数式"); args.Add("参照先インデックス");
            return "インデックスを指定して、数式を構成する要素を参照します。";
        }
    }
}
