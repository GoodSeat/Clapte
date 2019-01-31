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
using GoodSeat.Liffom.Formats.Powers;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 乗算の演算構文解析器を表します。
    /// </summary>
    public class PowerOperatorParser : SeriesOperatorParser
    {
        /// <summary>
        /// 乗算の演算構文解析器を初期化します。
        /// </summary>
        public PowerOperatorParser() : this(true) { }

        /// <summary>
        /// 乗算の演算構文解析器を初期化します。
        /// </summary>
        /// <param name="proguressiveParse">前進解析か否か</param>
        public PowerOperatorParser(bool proguressiveParse) { ProgressiveParse = proguressiveParse; }

        /// <summary>
        /// 連続する乗算を、前方から解決するか否かを設定もしくは取得します。
        /// </summary>
        /// <remarks>
        /// <para>
        /// 例えば2^2^3のような数式に対し、
        /// <list type="bullet">
        /// <item><see cref="ProguressiveParse"/>==trueの場合 → ((2^2)^3) </item>
        /// <item><see cref="ProguressiveParse"/>==falseの場合 → (2^(2^3))</item>
        /// といった違いがあります。
        /// </list>
        /// </remarks>
        /// </para>
        public bool ProgressiveParse { get; set; }

        public override IEnumerable<Lexer> GetAllBelongOperatorLexers()
        {
            yield return new PowerOperatorLexer(this);
        }

        public override Formula GetParsedFormula(Formula initial, params KeyValuePair<OperatorToken, Formula>[] subsequents)
        {
            Formula result;

            if (ProgressiveParse)
            {
                result = initial;
                foreach (var pair in subsequents)
                {
                    result = result ^ pair.Value;
                }
            }
            else
            {
                result = subsequents[subsequents.Length - 1].Value;
                for (int i = subsequents.Length - 2; i >= 0; i--)
                {
                    result = subsequents[i].Value ^ result;
                }
                result = initial ^ result;
            }

            result.Format.SetProperty(new DivisionFormatProperty(false)); // 1/x 形式とはしない
            return result;
        }
    }
}
