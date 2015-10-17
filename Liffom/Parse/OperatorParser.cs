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
    public abstract class OperatorParser
    {
        /// <summary>
        /// この演算構文解析器が対象とする演算記号トークンの字句解析器をすべて返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<Lexer> GetAllBelongOperatorLexers();

        /// <summary>
        /// トークン間の関係を調整します。
        /// </summary>
        /// <param name="targetToken">対象トークン。このトークンはすべてのトークンが対象となります（演算トークンとは限りません）。</param>
        /// <returns>関係の調整操作があったか否か</returns>
        public virtual bool ModifyTokenRelation(Token targetToken) { return false; }

        /// <summary>
        /// 指定演算子トークンを、この演算構文解析器で取り扱うか否かを取得します。
        /// </summary>
        /// <param name="token">判定対象の演算トークン。</param>
        /// <returns>取り扱うか否か。</returns>
        public virtual bool IsParseTarget(OperatorToken token) 
        { 
            return token.BelongOperatorParser == this;
        }
    }
}
