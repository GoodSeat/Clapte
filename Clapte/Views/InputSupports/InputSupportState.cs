using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoodSeat.Clapte.Views.InputSupports
{
	/// <summary>
	/// 入力補助の状態を表します。
	/// </summary>
	public abstract class InputSupportState
	{
		/// <summary>
		/// 入力補助の状態を初期化します。
		/// </summary>
		/// <param name="owner">対象の入力補助。</param>
		public InputSupportState(InputSupport owner)
		{
			Owner = owner;
		}

		/// <summary>
		/// 対象の入力補助を取得します。
		/// </summary>
		public InputSupport Owner { get; private set; }

		/// <summary>
		/// 状態にキーダウンを通知します。
		/// </summary>
		/// <param name="e">キーイベント情報。</param>
		/// <returns>後の状態。</returns>
		public abstract InputSupportState NotifyKeyDown(KeyEventArgs e);

		/// <summary>
		/// 通常の状態に戻る場合の復帰先状態を取得します。
		/// </summary>
		/// <returns>復帰後の状態。</returns>
		public abstract InputSupportState EscapeState();

		/// <summary>
		/// 状態が始まったことを通知します。
		/// </summary>
		public virtual void NotifyStartState() { }
	}
}
