using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Clapte.ViewModels.InputSupports
{
    /// <summary>
    /// 入力補助候補の列挙のインターフェイスを表します。
    /// </summary>
    public interface IInputSupportEnumerator
    {
        /// <summary>
        /// 指定文字列で引き当てられるすべての候補を返す反復子を取得します。
        /// </summary>
        /// <param name="startWith">引き当ての基となる文字列。</param>
        IEnumerable<InputSupportCandidate> GetAllCandidates(string startWith);
    }
}
