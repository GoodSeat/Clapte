// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Formulas
{
    /// <summary>
    /// 計算の続行が不可能となるエラーを含む数式を表します。
    /// </summary>
    [Serializable()]
    public class ErrorFormula : Formula
    {
        /// <summary>
        /// 計算の続行が不可能となるエラーを含む数式を初期化します。
        /// </summary>
        /// <param name="e">エラーの原因となる例外。</param>
        public ErrorFormula(Exception e)
        {
            CauseExceptionMessage = e.Message;
        }

        /// <summary>
        /// エラーの原因となる例外を取得します。
        /// </summary>
        public string CauseExceptionMessage { get; private set; }

        public override string GetText()
        {
            return "{" + CauseExceptionMessage + "}";
        }

        protected override string OnGetUniqueText()
        {
            return GetText();
        }
    }
}
