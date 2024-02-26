// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2024 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using GoodSeat.Liffom.Formats.Numerics;

namespace GoodSeat.Liffom.Formulas.Functions
{
    /// <summary>
    /// to0n関数を表します。
    /// </summary>
    [Serializable()]
    public class To0n : Function
    {
        /// <summary>
        /// to0n関数を初期化します。
        /// </summary>
        public To0n() : base() { }

        /// <summary>
        /// to0n関数を初期化します。
        /// </summary>
        /// <param name="f">対象の数式。</param>
        /// <param name="forBase">変換先基数。</param>
        public To0n(Formula f, Formula forBase) : base(f, forBase) { }

        /// <summary>
        /// 引数を指定して、関数を生成します。
        /// </summary>
        /// <param name="args">初期化に用いるか変数の数式。</param>
        /// <returns>初期化された関数。</returns>
        public override Function CreateFunction(params Formula[] args)
        {
            return args.Length > 1 ? new To0n(args[0], args[1]) : new To0n(args[0], ForBase);
        }

        protected virtual Formula ForBase { get { return Argument[1]; } }

        public override Formula CalculateFunction()
        {
            //if (!(Argument[0] is Numeric)) return this;
            if (!(ForBase is Numeric)) return this;

            var forBase = ForBase as Numeric;
            if (!forBase.IsInteger)
            {
                throw new FormulaDeformException("進数表記の基数として、2～36の整数以外の数値を指定することはできません。");
            }
            int n = (int)forBase;
            if (n < 2 || 36 < n)
            {
                throw new FormulaDeformException("進数表記の基数として、2～36の整数以外の数値を指定することはできません。");
            }

            var f = Argument[0];
            f.Format.SetProperty(new RadixConvertFormatProperty() { Mode = (RadixConvertFormatProperty.RadixConvertMode)n });
            return f;
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
            args = new List<string>()
            {
                "数式", "基数"
            };
            return "指定数式内における数値の進数表記を変換して返します。";
        }

    }

    /// <summary>
    /// to0b関数を表します。
    /// </summary>
    [Serializable()]
    public class To0b : To0n
    {
        /// <summary>
        /// to0b関数を初期化します。
        /// </summary>
        public To0b() : base() { }

        /// <summary>
        /// to0b関数を初期化します。
        /// </summary>
        /// <param name="f">対象の数式。</param>
        public To0b(Formula f) : base(f, 2) { }

        protected override Formula ForBase { get { return 2; } }

        public override int MaximumArgumentQty
        {
            get { return 1; }
        }
        public override string GetInformation(out List<string> args)
        {
            args = new List<string>() { "数式" };
            return "指定数式内における数値の進数表記を2進数に変換して返します。";
        }
    }

    /// <summary>
    /// to0o関数を表します。
    /// </summary>
    [Serializable()]
    public class To0o : To0n
    {
        /// <summary>
        /// to0o関数を初期化します。
        /// </summary>
        public To0o() : base() { }

        /// <summary>
        /// to0o関数を初期化します。
        /// </summary>
        /// <param name="f">対象の数式。</param>
        public To0o(Formula f) : base(f, 8) { }

        protected override Formula ForBase { get { return 8; } }

        public override int MaximumArgumentQty
        {
            get { return 1; }
        }
        public override string GetInformation(out List<string> args)
        {
            args = new List<string>() { "数式" };
            return "指定数式内における数値の進数表記を8進数に変換して返します。";
        }
    }

    /// <summary>
    /// to0d関数を表します。
    /// </summary>
    [Serializable()]
    public class To0d : To0n
    {
        /// <summary>
        /// to0d関数を初期化します。
        /// </summary>
        public To0d() : base() { }

        /// <summary>
        /// to0d関数を初期化します。
        /// </summary>
        /// <param name="f">対象の数式。</param>
        public To0d(Formula f) : base(f, 10) { }

        protected override Formula ForBase { get { return 10; } }

        public override int MaximumArgumentQty
        {
            get { return 1; }
        }
        public override string GetInformation(out List<string> args)
        {
            args = new List<string>() { "数式" };
            return "指定数式内における数値の進数表記を10進数に変換して返します。";
        }
    }

    /// <summary>
    /// to0x関数を表します。
    /// </summary>
    [Serializable()]
    public class To0x : To0n
    {
        /// <summary>
        /// to0x関数を初期化します。
        /// </summary>
        public To0x() : base() { }

        /// <summary>
        /// to0x関数を初期化します。
        /// </summary>
        /// <param name="f">対象の数式。</param>
        public To0x(Formula f) : base(f, 16) { }

        protected override Formula ForBase { get { return 16; } }

        public override int MaximumArgumentQty
        {
            get { return 1; }
        }
        public override string GetInformation(out List<string> args)
        {
            args = new List<string>() { "数式" };
            return "指定数式内における数値の進数表記を16進数に変換して返します。";
        }
    }

}

