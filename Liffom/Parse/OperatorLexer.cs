using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 演算記号の字句解析器を表します。
    /// </summary>
    public abstract class OperatorLexer : Lexer
    {
        /// <summary>
        /// 演算記号の字句解析器を初期化します。
        /// </summary>
        /// <param name="belongOperatorParser">所属する演算構文解析器</param>
        public OperatorLexer(OperatorParser belongOperatorParser)
        {
            BelongOperatorParser = belongOperatorParser;
        }

        /// <summary>
        /// 所属する演算子構文解析器を取得します。
        /// </summary>
        public OperatorParser BelongOperatorParser { get; private set; }

        /// <summary>
        /// 演算トークンを生成して取得します。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public override IEnumerable<Token> Tokenize(string text)
        {
            yield return new OperatorToken(text, BelongOperatorParser);
        }
    }
}
