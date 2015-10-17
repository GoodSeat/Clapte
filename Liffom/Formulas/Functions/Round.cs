using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas.Operators;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace GoodSeat.Liffom.Formulas.Functions
{
    /// <summary>
    /// Round関数（桁丸め）を表します。
    /// </summary>
    [Serializable()]
    public class Round : Function
    {
        /// <summary>
        /// Round関数を初期化します。
        /// </summary>
        public Round() : base() { }

        /// <summary>
        /// Round関数を初期化します。
        /// </summary>
        /// <param name="f">丸め対象とする対象数式</param>
        public Round(Formula f) : base(f) { }

        /// <summary>
        /// Round関数を初期化します。
        /// </summary>
        /// <param name="f">丸め対象とする対象数式</param>
        /// <param name="b">丸め小数部の桁数</param>
        public Round(Formula f, Formula b) : base(f, b) { }

        /// <summary>
        /// 引数を指定して、関数を生成します。
        /// </summary>
        /// <param name="args">初期化に用いるか変数の数式。</param>
        /// <returns>初期化された関数。</returns>
        public override Function CreateFunction(params Formula[] args)
        {
            if (args.Length < 2) return new Round(args[0]);
            else return new Round(args[0], args[1]);
        }

        public override Formula CalculateFunction()
        {
            Numeric n = Argument[0] as Numeric;
            Numeric round = (Argument.Count > 1) ? Argument[1] as Numeric : null;

            if (n != null && (Argument.Count == 1 || round == null))
                return new Numeric(n.Data.Round(0));
            else if (n != null && round != null && round.IsInteger)
                return new Numeric(n.Data.Round((int)round.Data.Data));
            else
                return this;
        }

        public override int MinimumArgumentQty
        {
            get { return 1; }
        }

        public override int MaximumArgumentQty
        {
            get { return 2; }
        }

        public override string GetInformation(out List<string> args)
        {
            args = new List<string>(); args.Add("数値"); args.Add("小数部の桁数（省略可。省略時0）");
            return "指定した小数部の桁数に丸めます。";
        }
    }
}

