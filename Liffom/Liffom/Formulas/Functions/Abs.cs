using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas.Operators;
using System.Drawing.Drawing2D;
using System.Drawing;

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
            if (Argument[0] is Numeric)
            {
                Numeric n = Argument[0] as Numeric;
                if (n >= 0) return n;
                else return (new Numeric(-1)).Data * n.Data;
            }
            else
                return this;
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
