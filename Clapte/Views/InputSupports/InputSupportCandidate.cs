using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Clapte.Views.InputSupports
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

        public override string ToString()
        {
            return Title;
        }

        public override bool Equals(object obj)
        {
            var other = obj as InputSupportCandidate;
            if (other == null) return false;

            return ReplaceText == other.ReplaceText;
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }

        #region IComparable<InputSupportCandidate> メンバー

        public int CompareTo(InputSupportCandidate other)
        {
            return this.Title.CompareTo(other.Title);
        }

        #endregion
    }
}
