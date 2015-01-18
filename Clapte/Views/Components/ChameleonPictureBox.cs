using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace GoodSeat.Clapte.Views.Components
{
	/// <summary>
	/// ヒット判定を設定できるピクチャーボックスを表します。
	/// </summary>
	public partial class ChameleonPictureBox : PictureBox
	{
		/// <summary>
		/// ウインドウのヒットテストの結果を表します。
		/// </summary>
		public enum WindowsHitPosition
		{
			/// <summary> エラー </summary>
			Error = -2,
			/// <summary> 透明箇所 </summary>
			Transparent = -1,
			/// <summary> 特定不可 </summary>
			Nowhere = 0,
			/// <summary> クライアント領域 </summary>
			Client = 1,
			/// <summary> キャプション領域 </summary>
			Caption = 2,
			/// <summary> システムメニュー </summary>
			SystemMenu = 3,
			/// <summary> サイズ変更領域 </summary>
			SizeBar = 4,
			/// <summary> メニュー </summary>
			Menu = 5,
			/// <summary> 水平スクロール </summary>
			HorizontalScroll = 6,
			/// <summary> 垂直スクロール </summary>
			VerticalScroll = 7,
			/// <summary> 最小化ボタン </summary>
			MinimizeButton = 8,
			/// <summary> 最大化ボタン </summary>
			MaximizeButton = 9,
			/// <summary> 領域枠左 </summary>
			Left = 10,
			/// <summary> 領域枠右 </summary>
			Right = 11,
			/// <summary> 領域枠上 </summary>
			Top = 12,
			/// <summary> 領域枠左上 </summary>
			TopLeft = 13,
			/// <summary> 領域枠右上 </summary>
			TopRight = 14,
			/// <summary> 領域枠下 </summary>
			Bottom = 15,
			/// <summary> 領域枠左下 </summary>
			BottomLeft = 16,
			/// <summary> 領域枠右下 </summary>
			BottomRight = 17,
			/// <summary> 領域枠 </summary>
			Border = 18,
			/// <summary> オブジェクト </summary>
			Object = 19,
			/// <summary> 終了ボタン </summary>
			Close = 20,
			/// <summary> ヘルプ </summary>
			Help = 21
		}


		WindowsHitPosition _hitSet = WindowsHitPosition.Client;

		/// <summary>
		/// ヒット判定の設定可能なピクチャーボックスを初期化します。
		/// </summary>
		public ChameleonPictureBox()
		{
			InitializeComponent();
		}

		/// <summary>
		/// ヒット拒否可能なピクチャーボックスを初期化します。
		/// </summary>
		/// <param name="container"></param>
		public ChameleonPictureBox(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		/// <summary>
		/// ヒット時のヒット判定を設定もしくは取得します。
		/// </summary>
		public WindowsHitPosition HitPosition
		{
			get { return _hitSet; }
			set { _hitSet = value; }
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			const int WM_NCHITTEST = 0x84;

			//マウスポインタがクライアント領域内にあるか
			if ((m.Msg == WM_NCHITTEST) &&
				(m.Result.ToInt32() == (int)WindowsHitPosition.Client))
			{
				m.Result = (IntPtr)_hitSet;
			}
		}
	}
}
