using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoodSeat.Clapte.Views.InputSupports
{
    /// <summary>
    /// 入力補助の選択候補から選択中の状態を表します。
    /// </summary>
    public class InputSupportSelectState : InputSupportState
    {
        /// <summary>
        /// 入力補助の候補からの選択状態を初期化します。
        /// </summary>
        /// <param name="owner">対象の入力補助。</param>
        /// <param name="baseState">テキストの入力状態。</param>
        public InputSupportSelectState(InputSupport owner, InputSupportTypingState baseState) 
            : base(owner) 
        {
            BaseState = baseState;
        }

        /// <summary>
        /// 通常状態を表すテキストの入力状態を設定もしくは取得します。
        /// </summary>
        InputSupportTypingState BaseState { get; set; }

        /// <summary>
        /// 状態が始まったことを通知します。
        /// </summary>
        public override void NotifyStartState() { Preview(); }

        /// <summary>
        /// 現在の選択状態に基づいて、テキスト表示をプレビューします。
        /// </summary>
        public void Preview()
        {
            var item = Owner.CandidateListBox.SelectedItem as InputSupportCandidate;
            Owner.ReplaceCurrentTargetWith(item.ReplaceText);
        }

        /// <summary>
        /// 状態にキーダウンを通知します。
        /// </summary>
        /// <param name="e">キーイベント情報。</param>
        /// <returns>後の状態。</returns>
        public override InputSupportState NotifyKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;

                if (Owner.CandidateListBox.SelectedIndex == Owner.CandidateListBox.Items.Count - 1)
                {
                    Owner.ReplaceCurrentTargetWith(Owner.CurrentBaseTargetText);
                    Owner.CandidateListBox.SelectedItem = null;
                    return BaseState;
                }
                else
                {
                    Owner.CandidateListBox.SelectedIndex++;
                    Preview();
                    return this;
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;

                if (Owner.CandidateListBox.SelectedIndex == 0)
                {
                    Owner.ReplaceCurrentTargetWith(Owner.CurrentBaseTargetText);
                    Owner.CandidateListBox.SelectedItem = null;
                    return BaseState;
                }
                else
                {
                    Owner.CandidateListBox.SelectedIndex--;
                    Preview();
                    return this;
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                Owner.ReplaceCurrentTargetWith(Owner.CurrentBaseTargetText);
                Owner.CandidateListBox.SelectedItem = null;
                return BaseState;
            }
            if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu ||
                e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                return this;
            }
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                Preview();
                Owner.IgnoreInputTill(e.KeyCode); // このキーは候補確定のための入力と判定し、テキスト入力では無視する。
                Owner.EscapeInputSupport();
                return BaseState;
            }

            Preview();
            return BaseState;
        }

        /// <summary>
        /// 通常の状態に戻る場合の復帰先状態を取得します。
        /// </summary>
        /// <returns>復帰後の状態。</returns>
        public override InputSupportState EscapeState() { return BaseState; }

    }
}
