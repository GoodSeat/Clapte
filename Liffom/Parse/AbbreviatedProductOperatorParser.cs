using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 省略された積算記号の構文解析器を表します。
    /// </summary>
    public class AbbreviatedProductOperatorParser : SeriesOperatorParser
    {
        public override IEnumerable<Lexer> GetAllBelongOperatorLexers()
        {
            yield break;
        }

        public override Formula GetParsedFormula(Formula initial, params KeyValuePair<OperatorToken, Formula>[] subsequents)
        {
            var productList = new List<Formula>();
            productList.Add(initial);
            foreach (var pair in subsequents)
            {
                if (!(pair.Key is AbbreviatedProductOperatorToken)) throw new FormulaParseException("想定していない積算記号です。");
                productList.Add(pair.Value);
            }
            return new Product(productList.ToArray());
        }

        public override bool ModifyTokenRelation(Token targetToken)
        {
            if (targetToken is FormulaToken && targetToken.NextToken is FormulaToken)
            {
                targetToken.InsertNext(new AbbreviatedProductOperatorToken(this));
                return true;
            }
            return false;
        }
    }
}
