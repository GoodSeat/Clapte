using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Clapte.ViewModels.InputSupports
{
    /// <summary>
    /// 入力補助の候補を表します。
    /// </summary>
    public class InputSupportCandidate : IComparable<InputSupportCandidate>
    {
        /// <summary>
        /// 入力補助の選択候補を初期化します。
        /// </summary>
        /// <param name="title">候補の表示文字列。</param>
        /// <param name="replaceText">実際に使用される文字列。</param>
        /// <param name="information">説明テキスト。</param>
        /// <param name="tag">関連付けるオブジェクト。</param>
        public InputSupportCandidate(string title, string replaceText, string information, object tag)
        {
            Title = title;
            ReplaceText = replaceText;
            Information = information;
            Tag = tag;
        }

        /// <summary>
        /// 候補の表示文字列を設定もしくは取得します。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 実際に使用される文字列を設定もしくは取得します。
        /// </summary>
        public string ReplaceText { get; set; }

        /// <summary>
        /// この候補に対する説明文を設定もしくは取得します。
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// この候補に関連付けられるオブジェクトを設定もしくは取得します。
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 入力候補を表す文字列を取得します。
        /// </summary>
        /// <returns>入力候補を表す文字列。</returns>
        public override string ToString() { return Title; }

        /// <summary>
        /// 指定したobjectが、この入力候補オブジェクトと等しいか否かを判定します。
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト。</param>
        /// <returns>判定結果。</returns>
        public override bool Equals(object obj)
        {
            var other = obj as InputSupportCandidate;
            if (other == null) return false;

            return ReplaceText == other.ReplaceText;
        }

        /// <summary>
        /// このインスタンスのハッシュコードを取得します。
        /// </summary>
        /// <returns>このインスタンスのハッシュコード。</returns>
        public override int GetHashCode() { return Title.GetHashCode(); }

        #region IComparable<InputSupportCandidate> メンバー

        /// <summary>
        /// 現在のオブジェクトを同じ型の別のオブジェクトと比較します。
        /// </summary>
        /// <param name="other">このオブジェクトと比較するオブジェクト。</param>
        /// <returns>
        /// 比較対象オブジェクトの相対順序を示す値。戻り値の意味は次のとおりです。値意味0 より小さい値このオブジェクトが other パラメーターより小さいことを意味します。0このオブジェクトが
        /// other と等しいことを意味します。0 より大きい値このオブジェクトが other よりも大きいことを意味します。
        /// </returns>
        public int CompareTo(InputSupportCandidate other) { return this.Title.CompareTo(other.Title); }

        #endregion
    }
}
