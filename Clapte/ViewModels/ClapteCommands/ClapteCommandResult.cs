using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoodSeat.Clapte.ViewModels.ClapteCommands
{
    /// <summary>
    /// Clapteの実行結果を表します。
    /// </summary>
    public class ClapteCommandResult
    {
        /// <summary>
        /// Clapteの実行結果を表します。
        /// </summary>
        /// <param name="title">メッセージタイトル。</param>
        /// <param name="message">結果を表すメッセージ。</param>
        /// <param name="icon">結果を表すアイコン。</param>
        /// <param name="enableNext">続けて操作を実行可能か否か。</param>
        public ClapteCommandResult(string title, string message, ToolTipIcon icon, bool enableNext)
        {
            Title = title;
            Message = message;
            Icon = icon;
            EnableNextAction = enableNext;
        }

        /// <summary>
        /// メッセージのタイトルを設定もしくは取得します。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 結果を表すメッセージを設定もしくは取得します。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 結果を表すアイコンを設定もしくは取得します。
        /// </summary>
        public ToolTipIcon Icon { get; set; }

        /// <summary>
        /// 続けて操作を実行できるか否かを設定若しくは取得します。
        /// </summary>
        public bool EnableNextAction { get; set; }
    }
}
