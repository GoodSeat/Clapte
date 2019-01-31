// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Liffom.Formulas.Functions
{
    /// <summary>
    /// abs(絶対値)関数を表します。
    /// </summary>
    [Serializable()]
    public class Abs : Function
    {
        /// <summary>
        /// abs(絶対値)関数を初期化します。
        /// </summary>
        public Abs() : base() { }

        /// <summary>
        /// abs(絶対値)関数を初期化します。
        /// </summary>
        /// <param name="arg">対象の数式</param>
        public Abs(Formula arg) : base(arg) { }

        /// <summary>
        /// 引数を指定して、関数を生成します。
        /// </summary>
        /// <param name="args">初期化に用いるか変数の数式。</param>
        /// <returns>初期化された関数。</returns>
        public override Function CreateFunction(params Formula[] args)
        {
            return new Abs(args[0]);
        }

        public override Formula CalculateFunction()
        {
            var arg = Argument[0];

            if (arg is Numeric)
            {
                Numeric n = arg as Numeric;
                if (n >= 0) return n;
                else return (new Numeric(-1)).Figure * n.Figure;
            }
            else if (arg is Product && arg.Count(f => f.IsUnit(true)) != 0)
            {
                List<Formula> fs      = new List<Formula>(arg.Where(f => !f.IsUnit(true)));
                List<Formula> fs_unit = new List<Formula>(arg.Where(f =>  f.IsUnit(true)));
                var nonUnit = new Abs(new Product(fs.ToArray())).CalculateFunction();
                fs_unit.Insert(0, nonUnit);

                return new Product(fs_unit.ToArray());
            }
            else
            {
                return this;
            }
        }

        public override int MinimumArgumentQty
        {
            get { return 1; }
        }

        public override string GetInformation(out List<string> args)
        {
            args = new List<string>(); args.Add("数値");
            return "数値から正負符号を除いた絶対値を返します。";
        }
    }
}
