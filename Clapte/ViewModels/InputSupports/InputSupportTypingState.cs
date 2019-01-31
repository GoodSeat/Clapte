// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoodSeat.Clapte.ViewModels.InputSupports
{
    /// <summary>
    /// 入力補助における文字列の入力状態を表します。
    /// </summary>
    public class InputSupportTypingState : InputSupportState
    {
        /// <summary>
        /// 入力補助における文字列の入力状態を表します。
        /// </summary>
        /// <param name="owner">対象の入力補助。</param>
        public InputSupportTypingState(InputSupport owner)
            : base(owner)
        {
            SelectState = new InputSupportSelectState(owner, this);
        }

        /// <summary>
        /// 候補選択時に移行すべき入力候補の選択状態を設定もしくは取得します。
        /// </summary>
        InputSupportSelectState SelectState { get; set; }


        /// <summary>
        /// 状態にキーダウンを通知します。
        /// </summary>
        /// <param name="e">キーイベント情報。</param>
        /// <returns>後の状態。</returns>
        public override InputSupportState NotifyKeyDown(KeyEventArgs e)
        {
            if (!Owner.CandidateListBox.Visible) return this;

            if (e.KeyCode == Keys.Down)
            {
                Owner.CandidateListBox.SelectedIndex = 0;
                e.Handled = true;
                return SelectState;
            }
            if (e.KeyCode == Keys.Up)
            {
                Owner.CandidateListBox.SelectedIndex = Owner.CandidateListBox.Items.Count - 1;
                e.Handled = true;
                return SelectState;
            }
            if (e.KeyCode == Keys.Escape)
            {
                Owner.EscapeInputSupport();
            }
            return this;
        }

        /// <summary>
        /// 通常の状態に戻る場合の復帰先状態を取得します。
        /// </summary>
        /// <returns>復帰後の状態。</returns>
        public override InputSupportState EscapeState() { return this; }
    }
}
