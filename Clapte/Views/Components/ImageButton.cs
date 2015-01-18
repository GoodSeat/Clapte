using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GoodSeat.Clapte.Views.Components
{
	/// <summary>
	/// イメージによるボタンを表します。
	/// </summary>
	public partial class ImageButton : PictureBox
	{
		/// <summary>
		/// イメージボタンを初期化します。
		/// </summary>
		public ImageButton()
		{
			InitializeComponent();
			InitializeEvent();
		}

		/// <summary>
		/// イメージボタンを初期化します。
		/// </summary>
		/// <param name="container"></param>
		public ImageButton(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
			InitializeEvent();
		}

		private void InitializeEvent()
		{
			this.MouseEnter += new EventHandler(ImageButton_MouseEnter);
			this.MouseLeave += new EventHandler(ImageButton_MouseLeave);
			this.MouseDown += new MouseEventHandler(ImageButton_MouseDown);
			this.MouseUp += new MouseEventHandler(ImageButton_MouseUp);

			Cursor = Cursors.Hand;
			SizeMode = PictureBoxSizeMode.Zoom;
		}

		Image _focusImage;
		Image _unFocusImage;

		bool _downed = false;
		int _move = 1;

		/// <summary>
		/// マウスがボタン領域内にある時のイメージを設定もしくは取得します。
		/// </summary>
		public Image FocusImage
		{
			get { return _focusImage; }
			set { _focusImage = value; }
		}

		/// <summary>
		/// ボタン押下時の移動量を設定もしくは取得します。
		/// </summary>
		public int DownMove
		{
			get { return _move; }
			set { _move = value; }
		}

		/// <summary>
		/// マウスがボタン領域外にあるときのイメージを設定もしくは取得します。
		/// </summary>
		public Image UnFocusImage
		{
			get { return _unFocusImage; }
			set { _unFocusImage = value; Image = value; }
		}

		void ImageButton_MouseEnter(object sender, EventArgs e)
		{
			Image = FocusImage;
		}

		void ImageButton_MouseLeave(object sender, EventArgs e)
		{
			Image = UnFocusImage;
		}

		void ImageButton_MouseDown(object sender, MouseEventArgs e)
		{
			if (_downed) return;

			this.Top += DownMove;
			_downed = true;
		}

		void ImageButton_MouseUp(object sender, MouseEventArgs e)
		{
			if (!_downed) return;

			this.Top -= DownMove;
			_downed = false;
		}

	}
}
