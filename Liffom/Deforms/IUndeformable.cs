using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Deforms
{
    /// <summary>
    /// 変形対象とならない数式を表します。
    /// </summary>
    public interface IUndeformable
    {
        /// <summary>
        /// 数式がいかなるルールに対しても変形適用の対象外であるか否かを取得します。
        /// </summary>
        /// <remarks>
        /// 例えば、数値でも 1.5 -> 3/2 の変形対象となる可能性があります。この判定には十分注意してください。
        /// この判定は、単に変形速度改善のためにのみ用いられます。特に速度向上が必要でない場合は、実装不要です。
        /// </remarks>
        /// <returns>常に変形対象外となるか否か。</returns>
        bool IsUndeformable();
    }
}
