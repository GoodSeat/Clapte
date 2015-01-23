using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sgry.Azuki.WinForms;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;

namespace GoodSeat.Clapte.Views.InputSupports
{
	/// <summary>
	/// Azukiコントロールを対象とした入力補助機能を表します。
	/// </summary>
	public class InputSupport : IDisposable
	{
		/// <summary>
		/// Azukiコントロールを対象とした入力補助機能を初期化します。
		/// </summary>
		/// <param name="azuki">補助機能を提供する対象となるAzukiコントロール。</param>
		/// <param name="owner">補助候補のリストボックスを表示する先のコントロール。</param>
		/// <param name="enumerator">入力補助の候補列挙オブジェクト。</param>
		public InputSupport(AzukiControl azuki, Control owner, IInputSupportEnumerator enumerator)
		{
			Azuki = azuki;
			Owner = owner;
			CandidateEnumerator = enumerator;

			CandidateListBox = new ListBox();
			CandidateListBox.Visible = false;
			CandidateListBox.Font = Azuki.Font;
			CandidateListBox.SelectedIndexChanged += new EventHandler(CandidateListBox_SelectedIndexChanged);
			Owner.Controls.Add(CandidateListBox);

			ToolTipHelp = new ToolTip();

			azuki.TextChanged += new EventHandler(azuki_TextChanged);
			azuki.FontChanged += new EventHandler(azuki_FontChanged);
			azuki.CaretMoved += new EventHandler(azuki_CaretMoved);
			azuki.KeyDown += new KeyEventHandler(azuki_KeyDown);
            azuki.KeyUp += new KeyEventHandler(azuki_KeyUp);
			CandidateListBox.KeyDown += new KeyEventHandler(azuki_KeyDown);

			State = new InputSupportTypingState(this);

            AutoShow = true;
		}

		InputSupportState _state;
        Keys _nextIgnore = Keys.None;

		/// <summary>
		/// 対象となるAzukiコントロールを設定もしくは取得します。
		/// </summary>
		AzukiControl Azuki { get; set; }

		/// <summary>
		/// 入力補助リストの表示先コントロールを設定もしくは取得します。
		/// </summary>
		Control Owner { get; set; }

		/// <summary>
		/// 入力補助の候補列挙オブジェクトを設定もしくは取得します。
		/// </summary>
		IInputSupportEnumerator CandidateEnumerator { get; set; }

		/// <summary>
		/// 現在の入力補助状態を設定もしくは取得します。
		/// </summary>
		InputSupportState State 
		{
			get
			{
				return _state;
			}
			set
			{
				if (_state == value) return;

                if (value is InputSupportTypingState)
                {
                    ToolTipHelp.Hide(CandidateListBox);
                    Azuki.Document.EndUndo();
                }
                else
                {
                    Azuki.Document.BeginUndo();
                }

				_state = value;
				_state.NotifyStartState();
			}
		}

		/// <summary>
		/// AzukiのOwnerに対する相対位置を設定します。
		/// </summary>
		public Point ModifyLocation { private get; set; }

		/// <summary>
		/// 入力補助の候補をリスト表示するリストボックスを取得します。
		/// </summary>
		internal ListBox CandidateListBox { get; private set; }

        /// <summary>
        /// 入力補助を自動で表示するか否かを設定もしくは取得します。
        /// </summary>
        public bool AutoShow { get; set; }

		/// <summary>
		/// 入力補助候補の説明を表示するツールチップを設定もしくは取得します。
		/// </summary>
		ToolTip ToolTipHelp { get; set; }

		/// <summary>
		/// 現在の候補絞り込み対象テキストの開始インデックスを取得します。
		/// </summary>
		public int CurrentTargetStartIndex { get; private set; }

		/// <summary>
		/// 現在の候補絞り込み対象テキストの文字列長を取得します。
		/// </summary>
		public int CurrentTargetLenth { get; private set; }

		/// <summary>
		/// 現在の候補絞り込み対象テキストを取得します。
		/// </summary>
		public string CurrentTargetText { get; private set; }

		/// <summary>
		/// 現在の候補絞り込み対象の元テキストを取得します。
		/// </summary>
		public string CurrentBaseTargetText { get; private set; }

        /// <summary>
        /// 次の指定キー入力まで、テキストの変更を抑止します。
        /// </summary>
        /// <param name="key">テキスト変更抑止の解除に用いるキー。</param>
        public void IgnoreInputTill(Keys key)
        {
            Azuki.IsReadOnly = true;
            _nextIgnore = key;
        }


		/// <summary>
		/// 現在の状況に応じて、入力補助を表示します。
		/// </summary>
		public void ShowInputSupport()
		{
			if (!(State is InputSupportTypingState)) return;

			int startIndex;
			var target = Azuki.GetPreCaretWord(out startIndex);
			if (target == null)
			{
				EscapeInputSupport();
				return;
			}

			CurrentTargetStartIndex = startIndex;
			CurrentTargetLenth = target.Length;
			CurrentTargetText = target;
			CurrentBaseTargetText = target;

			CandidateListBox.Items.Clear();
			foreach (var item in CandidateEnumerator.GetAllCandidates(target)) CandidateListBox.Items.Add(item);

			if (CandidateListBox.Items.Count == 0)
			{
				EscapeInputSupport();
				return;
			}

			SetCandidateListBoxSize(startIndex);
		}

		/// <summary>
		/// 入力補助の候補リスト表示を解除します。
		/// </summary>
		public void EscapeInputSupport()
		{
			State = State.EscapeState();
			CandidateListBox.Visible = false;
		}

		/// <summary>
		/// 現在の入力補助対象を、指定文字列で置き換えます。
		/// </summary>
		/// <param name="newText">置換後の新しい文字列。</param>
		public void ReplaceCurrentTargetWith(string newText)
		{
            if (!CandidateListBox.Visible) throw new NotSupportedException("入力補助候補のリストボックスが表示されていない状態では、このメソッドはサポートされていません。");

			int start = CurrentTargetStartIndex;
			int end = CurrentTargetStartIndex + CurrentTargetLenth;

			CurrentTargetLenth = newText.Length;
			CurrentTargetText = newText;

			Azuki.Document.Replace(newText, start, end);
		}

		/// <summary>
		/// 現在のキャレット位置を指定して、補助候補のリストボックスの表示状態を初期化します。
		/// </summary>
        /// <param name="position">補助対象単語の先頭文字のキャレットインデックス。</param>
		private void SetCandidateListBoxSize(int startIndex)
		{
            Point position = Azuki.GetPositionFromIndex(startIndex);

			CandidateListBox.Left = position.X + Azuki.Left + ModifyLocation.X;
			CandidateListBox.Top = position.Y + Azuki.Top + ModifyLocation.Y + Azuki.View.LineHeight + Azuki.View.LinePadding;
			CandidateListBox.Size = CandidateListBox.PreferredSize;

			if (CandidateListBox.Bottom > Owner.Height) CandidateListBox.Height += (Owner.Height - CandidateListBox.Bottom - 2);
			CandidateListBox.BringToFront();

			CandidateListBox.Visible = true;
		}

		#region イベント対応

        int _lastTextLength = 0;

		void azuki_TextChanged(object sender, EventArgs e)
		{
            int currentTextLength = Azuki.TextLength;

            if (CandidateListBox.Visible) ShowInputSupport();
            else if (AutoShow && currentTextLength > _lastTextLength) ShowInputSupport();

            _lastTextLength = currentTextLength;
		}

		void azuki_CaretMoved(object sender, EventArgs e)
		{
//			if (!(State is InputSupportTypingState)) return;
			if (Azuki.CaretIndex < CurrentTargetStartIndex ||
				Azuki.CaretIndex > CurrentTargetStartIndex + CurrentTargetLenth + 1) EscapeInputSupport();
		}

		void azuki_FontChanged(object sender, EventArgs e)
		{
			CandidateListBox.Font = Azuki.Font;
		}

		void azuki_KeyDown(object sender, KeyEventArgs e)
		{
			State = State.NotifyKeyDown(e);
		}

        void azuki_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == _nextIgnore)
            {
                Azuki.IsReadOnly = false;
                _nextIgnore = Keys.None;
            }
        }

		void CandidateListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			ToolTipHelp.Hide(CandidateListBox);

			var item = CandidateListBox.SelectedItem as InputSupportCandidate;
			if (item == null) return;

			var rect = CandidateListBox.GetItemRectangle(CandidateListBox.SelectedIndex);
			rect.Offset(rect.Width, 0);

			ToolTipHelp.Show(item.Information, CandidateListBox, rect.Location, 100000);
		}
		
		#endregion

		#region IDisposable メンバー

		public void Dispose()
		{
			Azuki.TextChanged -= new EventHandler(azuki_TextChanged);
			Azuki.FontChanged -= new EventHandler(azuki_FontChanged);
			Azuki.CaretMoved -= new EventHandler(azuki_CaretMoved);
			Azuki.KeyDown -= new KeyEventHandler(azuki_KeyDown);
			Azuki.KeyUp -= new KeyEventHandler(azuki_KeyUp);
		}

		#endregion
	}
}
