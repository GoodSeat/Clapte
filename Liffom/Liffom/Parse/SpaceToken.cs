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
        /// <param name="targetText">初期化元となった文字列。</param>
        /// <param name="productOperatorParser">積算記号として認識する場合の構文解析器。積算として認識しない場合はnullを指定。</param>
        public SpaceToken(string targetText, ProductOperatorParser productOperatorParser) : base(targetText) { ProductParser = productOperatorParser; }

        /// <summary>
        /// 空白文字を積算記号として認識する場合に使用する積算の構文解析器を設定若しくは取得します。
        /// </summary>
        public ProductOperatorParser ProductParser { get; set; }

        /// <summary>
        /// 空白文字トークンをトークン列から削除します。
        /// </summary>
        /// <returns></returns>
        public override bool ModifyTokenRelation()
        {
            bool isNotProduct = true;

            if (ProductParser != null)
            {
                isNotProduct = (this == TopToken || this == LastToken
                             || PreviousToken is OperatorToken    || NextToken is OperatorToken
                             || PreviousToken is SpaceToken       || NextToken is SpaceToken
                             );
                isNotProduct |= PreviousToken is PunctuationToken && (PreviousToken as PunctuationToken).Type == PunctuationToken.PunctuationType.Start;
                isNotProduct |= NextToken     is PunctuationToken && (NextToken     as PunctuationToken).Type == PunctuationToken.PunctuationType.End;
            }

            if (!isNotProduct) InsertNext(new OperatorToken("*", ProductParser));

            Remove();
            return true;
        }
    }
}
