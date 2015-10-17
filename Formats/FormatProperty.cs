using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Formats
{
    /// <summary>
    /// 数式の出力書式を表します。
    /// </summary>
    /// <remarks>
    /// 必ず既定のコンストラクタを実装してください。既定のコンストラクタで初期化される書式設定が、システム既定の書式設定となります。
    /// </remarks>
    [Serializable()]
    public abstract class FormatProperty
    {
        /// <summary>
        /// 数式の出力文字列を修正します。
        /// </summary>
        /// <param name="ownerFormat">この書式情報を保持する書式オブジェクト。</param>
        /// <param name="baseText">変更前の出力文字列。</param>
        internal protected virtual string OnModifyOutputText(Format ownerFormat, string baseText) { return baseText; }

        /// <summary>
        /// 親数式に設定された書式属性が、子数式に継承されるか否かを取得します。
        /// </summary>
        public virtual bool Inheritable { get { return true; } }

    }
}

