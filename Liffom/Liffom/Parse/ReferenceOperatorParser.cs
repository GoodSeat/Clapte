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
using GoodSeat.Liffom.Formulas.Functions;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 参照関数の演算構文解析器を表します。
    /// </summary>
    public class ReferenceOperatorParser : SeriesOperatorParser
    {
        public override IEnumerable<Lexer> GetAllBelongOperatorLexers()
        {
            yield return new ReferenceOperatorLexer(this);
        }

        public override Formula GetParsedFormula(Formula initial, params KeyValuePair<OperatorToken, Formula>[] subsequents)
        {
            Formula result = initial;

            foreach (var pair in subsequents)
            {
                result = new Ref(result, pair.Value);
            }

            return result;
        }
    }
}
