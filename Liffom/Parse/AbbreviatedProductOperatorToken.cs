using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 自動挿入された、省略積算記号トークンを表します。
    /// </summary>
    public class AbbreviatedProductOperatorToken : OperatorToken
    {
        /// <summary>
        /// 自動挿入された省略記号乗算記号トークンを初期化します。
        /// </summary>
        /// <param name="belongOperatorParser">所属演算構文解析器</param>
        public AbbreviatedProductOperatorToken(AbbreviatedProductOperatorParser belongOperatorParser)
            : base("･", belongOperatorParser)
        { }
    }
}
