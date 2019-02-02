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
    /// トークン列の末尾を表すトークンを表します。
    /// </summary>
    public class EndToken : PunctuationToken
    {
        /// <summary>
        /// トークン列の末尾を表すトークンを初期化します。
        /// </summary>
        public EndToken()
            : base("")
        {
            Type = PunctuationType.End;
        }

        public override bool IsValidSetPunctuation(PunctuationToken token)
        {
            return token is StartToken;
        }
    }
}
