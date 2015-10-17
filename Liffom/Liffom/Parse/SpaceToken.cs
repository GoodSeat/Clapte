using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 空白文字トークンを表します。
    /// </summary>
    public class SpaceToken : Token
    {
        /// <summary>
        /// 空白文字トークンを初期化します。
        /// </summary>
        /// <param name="targetText"></param>
        public SpaceToken(string targetText) : base(targetText) { }

        /// <summary>
        /// 空白文字トークンをトークン列から削除します。
        /// </summary>
        /// <returns></returns>
        public override bool ModifyTokenRelation()
        {
            Remove();
            return true;
        }
    }
}
