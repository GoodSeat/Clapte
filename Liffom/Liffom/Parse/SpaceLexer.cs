// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 空白文字の字句解析器を表します。
    /// </summary>
    public class SpaceLexer : Lexer
    {
        /// <summary>
        /// 空白文字の字句解析器を表します。
        /// </summary>
        /// <param name="productOperatorParser">積算記号として認識する場合の構文解析器。積算として認識しない場合はnullを指定。</param>
        public SpaceLexer(ProductOperatorParser productOperatorParser = null)
        {
            ProductParser = productOperatorParser;
        }

        /// <summary>
        /// 空白文字を積算記号として認識する場合に使用する積算の構文解析器を設定若しくは取得します。
        /// </summary>
        public ProductOperatorParser ProductParser { get; set; }

        /// <summary>
        /// 指定文字が空白トークンとしてトークン化可能か否かを判断します。
        /// </summary>
        /// <param name="text">判定対象の文字列</param>
        /// <returns>判定結果</returns>
        public override Lexer.ScanResult Scan(string text)
        {
            if (text == " ") return ScanResult.Match;
            else return ScanResult.NeverMatch;
        }

        /// <summary>
        /// 空白文字を指定して、空白トークンを初期化して取得します。
        /// </summary>
        /// <param name="text">トークン化対象文字列</param>
        /// <returns>空白トークン</returns>
        public override IEnumerable<Token> Tokenize(string text)
        {
            yield return new SpaceToken(text, ProductParser);
        }
    }
}
