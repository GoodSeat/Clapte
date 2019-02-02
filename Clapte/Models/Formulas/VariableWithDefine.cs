// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Clapte.Models.Formulas
{
    /// <summary>
    /// 定義付きの変数を表します。
    /// </summary>
    [Serializable()]
    public class VariableWithDefine : Variable
    {
        /// <summary>
        /// 定義付きの変数を初期化します。
        /// </summary>
        /// <param name="mark">変数記号。</param>
        /// <param name="define">変数の定義。</param>
        public VariableWithDefine(string mark, Formula define) : base(mark) { Define = define; }

        /// <summary>
        /// この変数に関連付けられた定義を表す数式を取得します。
        /// </summary>
        public Formula Define { get; set; }

        public override IEnumerable<Rule> GetRelatedRulesOf(DeformToken deformToken, Formula sender, DeformHistory history)
        {
            foreach (var r in base.GetRelatedRulesOf(deformToken, sender, history)) yield return r;

            yield return SubstituteDefineForVariableRule.Entity;
        }
    }
}
