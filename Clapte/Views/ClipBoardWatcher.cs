using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GoodSeat.Clapte.Views
{
	/// <summary>
	/// クリップボードを監視するクラス。
	/// 使用後は必ずDispose()メソッドを呼び出して下さい。
	/// </summary>
	public class ClipBoardWatcher : IDisposable
	{
		ClipBoardWatcherForm _form;

		/// <summary>
		/// クリップボードに内容に変更があると発生します。
		/// </summary>
		public event EventHandler DrawClipBoard;

		/// <summary>
		/// ClipBoardWatcherクラスを初期化して
		/// クリップボードビューアチェインに登録します。
		/// 使用後は必ずDispose()メソッドを呼び出して下さい。
		/// </summary>
		public ClipBoardWatcher()
		{
			_form = new ClipBoardWatcherForm();
			_form.StartWatch(raiseDrawClipBoard);
			Enable = true;
		}

		/// <summary>
		/// クリップボードの監視有無を設定もしくは取得します。
		/// </summary>
		public bool Enable
		{
			get { return _form.Enable; }
			set { _form.Enable = value; }
		}

		private void raiseDrawClipBoard()
		{
			Console.WriteLine("クリップボードに変化がありました。");
			if (DrawClipBoard != null)
			{
				DrawClipBoard(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// ClipBoardWatcherクラスを
		/// クリップボードビューアチェインから削除します。
		/// </summary>
		public void Dispose()
		{
			_form.Dispose();
		}

		private class ClipBoardWatcherForm : Form
		{
			DateTime _lastProcDate;

			[DllImport("user32.dll")]
			private static extern IntPtr SetClipboardViewer(IntPtr hwnd);
			[DllImport("user32.dll")]
			private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
			[DllImport("user32.dll")]
			private static extern bool ChangeClipboardChain(IntPtr hwnd, IntPtr hWndNext);

			const int WM_DRAWCLIPBOARD = 0x0308;
			const int WM_CHANGECBCHAIN = 0x030D;

			IntPtr nextHandle;
			System.Threading.ThreadStart proc;

			public bool Enable { get; set; }

			public void StartWatch(System.Threading.ThreadStart proc)
			{
				this.proc = proc;
				nextHandle = SetClipboardViewer(this.Handle);
				this.WindowState = FormWindowState.Minimized;

				_lastProcDate = DateTime.Now;
			}

			protected override void WndProc(ref Message m)
			{
				if (Enable && m.Msg == WM_DRAWCLIPBOARD)
				{
					Console.WriteLine(this.Handle.ToString() + ", " + nextHandle.ToString());
					SendMessage(nextHandle, m.Msg, m.WParam, m.LParam);

					if (DateTime.Now - _lastProcDate > TimeSpan.FromMilliseconds(100))
					{
						proc();
						_lastProcDate = DateTime.Now;
					}
				}
				else if (Enable && m.Msg == WM_CHANGECBCHAIN)
				{
					if (m.WParam == nextHandle)
					{
						nextHandle = m.LParam;
					}
					else
					{
						SendMessage(nextHandle, m.Msg, m.WParam, m.LParam);
					}
				}
				base.WndProc(ref m);
			}

			protected override void Dispose(bool disposing)
			{
				try
				{
					ChangeClipboardChain(this.Handle, nextHandle);
				}
				catch { }
				base.Dispose(disposing);
			}
		}
	}
}
