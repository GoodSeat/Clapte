// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2025 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using System;
using System.Collections.Generic;

namespace GoodSeat.Liffom.Formulas.Functions
{
    /// <summary>
    /// 変形除外関数を表します。
    /// </summary>
    [Serializable()]
    public class Lock : Function
    {
        /// <summary>
        /// 変形除外関数を初期化します。
        /// </summary>
        public Lock() : base() { }

        /// <summary>
        /// 参照関数を初期化します。
        /// </summary>
        /// <param name="f">対象数式。</param>
        public Lock(Formula f) : base(f) { }

        public override int MinimumArgumentQty
        {
            get { return 1; }
        }

        public override Formula CalculateFunction()
        {
            return this;
        }

        /// <summary>
        /// 指定処理に関連するルールをすべて返す反復子を取得します。
        /// </summary>
        /// <param name="deformToken">変形識別トークン。</param>
        /// <param name="sender">ルール適用対象となる最上位親数式。</param>
        /// <param name="history">変形履歴情報。</param>
        /// <returns>変形に関連するルールを返す反復子。</returns>
        public override IEnumerable<Rule> GetRelatedRulesOf(DeformToken deformToken, Formula sender, DeformHistory history)
        {
            yield break;
        }

        public override Function CreateFunction(params Formula[] args)
        {
            return new Lock(args[0]);
        }

        public override string GetInformation(out List<string> args)
        {
            args = new List<string>(); args.Add("対象数式");
            return "数式変形対象から除外化します。";
        }

        public override Formula this[int i]
        {
            get { return Argument[0][i]; }
            set { Argument[0][i] = value; }
        }

    }
}
