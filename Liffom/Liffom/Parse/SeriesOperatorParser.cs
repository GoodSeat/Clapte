// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 数式間のトークンから、演算数式を解析する構文解析器を表します。
    /// </summary>
    public abstract class SeriesOperatorParser : OperatorParser
    {
        /// <summary>
        /// 指定された数式と、それらの結合トークンの組み合わせを、演算に変換して取得します。
        /// </summary>
        /// <param name="initial">最初の数式</param>
        /// <param name="subsequents">可変数の、後続演算トークンと数式の組み合わせ</param>
        /// <returns>解析結果</returns>
        public abstract Formula GetParsedFormula(Formula initial, params KeyValuePair<OperatorToken, Formula>[] subsequents);
    }
}

