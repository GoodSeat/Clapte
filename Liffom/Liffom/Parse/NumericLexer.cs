// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoodSeat.Liffom.Formulas;
using Microsoft.VisualBasic.ApplicationServices;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 数値の字句解析器を表します。
    /// </summary>
    public class NumericLexer : Lexer
    {
        public NumericLexer()
        {
            var ops = new List<string>();
            ops.Add("^([0-9]*(\\.)?[0-9]+|[0-9]+(\\.)?[0-9]*)(e[+-]?[0-9]+)?$");

            if (Use0b)
            {
                var c = "[0-1]";
                ops.Add($"^0b({c}*(\\.)?{c}+|{c}+(\\.)?{c}*)(e[+-]?{c}+)?$");
            }
            if (Use0o)
            {
                var c = "[0-7]";
                ops.Add($"^0o({c}*(\\.)?{c}+|{c}+(\\.)?{c}*)(e[+-]?{c}+)?$");
            }
            if (Use0x)
            {
                var c = "[0-9abcdef]";
                ops.Add($"^0x({c}*(\\.)?{c}+|{c}+(\\.)?{c}*)(e[+-]?{c}+)?$");
            }

            var pattern = string.Join("|", ops);

            // 正負記号は和算演算に任せる
            NumericRegex = new Regex(pattern, RegexOptions.IgnoreCase);
        }

        public bool Use0b { get; set; } = true;
        public bool Use0o { get; set; } = true;
        public bool Use0x { get; set; } = true;

        public Regex NumericRegex { get; private set; }

        public override Lexer.ScanResult Scan(string text)
        {
            if (NumericRegex.IsMatch(text)) return ScanResult.Match;
            if (!NumericRegex.IsMatch(text + "1") &&
                !NumericRegex.IsMatch(text + "+1")) return ScanResult.NeverMatch; // 3E → 3E1、3E+ → 3E+1
            return ScanResult.NoMatch;
        }

        public override IEnumerable<Token> Tokenize(string text)
        {
            FormulaToken result = null;
            try
            {
                result = new FormulaToken(text, new Numeric(text));
            }
            catch (OverflowException e)
            {
                throw new FormulaParseException(string.Format("オーバーフローが発生しました。\"{0}\"は数値として大きすぎるか、もしくは小さすぎます。", text), e);
            }
            yield return result;
        }
    }
}
