using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace GoodSeat.Clapte.Views.Controls
{
	/// <summary>
	/// スライド操作可能な数値入力ボックスを表します。
	/// </summary>
	public partial class NumericSlider : UserControl
	{
		/// <summary>
		/// スライド操作可能な数値入力ボックスを初期化します。
		/// </summary>
		public NumericSlider()
		{
			InitializeComponent();

			_btnSub.Parent = _picBox;
			_btnAdd.Parent = _picBox;

			_dragTimer = new Timer();
			_dragTimer.Interval = 200;

			_tooltip = new ToolTip();
			_tooltip.IsBalloon = true;

			_dragTimer.Tick += new EventHandler(_dragTimer_Tick);

			_picBox.MouseDown += new MouseEventHandler(picBox_MouseDown);
			_picBox.MouseClick += new MouseEventHandler(picBox_MouseClick);
			_picBox.MouseDoubleClick += new MouseEventHandler(picBox_MouseDoubleClick);
			_picBox.MouseUp += new MouseEventHandler(picBox_MouseUp);
			_picBox.MouseEnter += new EventHandler(picBox_MouseEnter);
			_picBox.MouseLeave += new EventHandler(picBox_MouseLeave);
			_picBox.MouseMove += new MouseEventHandler(_picBox_MouseMove);
			EnabledChanged += new EventHandler(NumericSlider_EnabledChanged);

			this.SizeChanged += new EventHandler(RequestRefresh);

			DrawValue();
		}

		Point _preCursor;

		decimal _value = 0;
		decimal _max = 100;
		decimal _min = 0;
		int _precision = 4;
		string _unit = "";
		bool _underBar = false;

		decimal _clickChange = 1;
		decimal _slideChange = 1;
		decimal _sensitivity = 2;

		bool _useToolTip = true;
		bool _nowDragging = false;
		bool _showButton = true;

		SolidBrush _brush;
		SolidBrush _backbarBrush;
		Timer _dragTimer;
		ToolTip _tooltip;

		EventHandler _valueChanged;
		EventHandler _endEdit;

		bool _useBar = true;
		Color _barColor = Color.Lavender;
		bool _visibleCursorOnSlide = false;

		#region プロパティ

		/// <summary>
		/// スライダの値を設定もしくは取得します。
		/// </summary>
		[Browsable(true), Category("動作"), Description("スライダの値を設定もしくは取得します。")]
		public decimal Value
		{
			get { return _value; }
			set
			{
				decimal pre = Value;

				decimal set = Math.Max(_min, value);
				set = Math.Min(_max, set);
				_value = Math.Round(set, _precision);

				if (Value != pre && _valueChanged != null)
					_valueChanged(this, new EventArgs());

				DrawValue();
			}
		}

		/// <summary>
		/// スライダの最大値を設定もしくは取得します。
		/// </summary>
		[Browsable(true), Category("動作"), Description("スライダの最大値を設定もしくは取得します。")]
		public decimal Maximum
		{
			get { return _max; }
			set
			{
				if (value >= Minimum) _max = value;
				if (Value > value) Value = value;
			}
		}

		/// <summary>
		/// スライダの最小値を設定もしくは取得します。
		/// </summary>
		[Browsable(true), Category("動作"), Description("スライダの最小値を設定もしくは取得します。")]
		public decimal Minimum
		{
			get { return _min; }
			set
			{
				if (value <= Maximum) _min = value;
				if (Value < value) Value = value;
			}
		}

		/// <summary>
		/// スライダの値の有効桁数を設定もしくは取得します。
		/// </summary>
		[Browsable(true), Category("動作"), Description("スライダの値の有効桁数を設定もしくは取得します。")]
		public int Precision
		{
			set
			{
				_precision = Math.Max(0, value);
				Value = Value;//有効桁の更新
			}
			get { return _precision; }
		}

		/// <summary>
		/// スライダに表示する単位を設定もしくは取得します。
		/// </summary>
		[Browsable(true), Category("表示"), Description("スライダに表示する単位を設定もしくは取得します。")]
		public string Unit
		{
			set
			{
				_unit = value;
				Value = Value; // 有効桁の更新
			}
			get { return _unit; }
		}

		/// <summary>
		/// 左右だけでなく、上下の動きによっても値を変えられるか否かを設定もしくは取得します。
		/// </summary>
		[Browsable(true), Category("動作"), Description("左右だけでなく、上下の動きによっても値を変えられるか否かを設定もしくは取得します。")]
		public bool EnableUpDown
		{
			get { return (Cursor == Cursors.SizeAll); }
			set
			{
				if (value)
					Cursor = Cursors.SizeAll;
				else
					Cursor = Cursors.SizeWE;
			}
		}

		/// <summary>
		/// スライダの感度（小さいほど感度が良い）を設定もしくは取得します。
		/// </summary>
		[Browsable(true), Category("動作"), Description("スライダの感度（小さいほど感度が良い）を設定もしくは取得します。")]
		public decimal Sensitivity
		{
			get { return _sensitivity; }
			set
			{
				if (value > 0)
					_sensitivity = value;
			}
		}

		/// <summary>
		/// スライダによる単変化の値を設定または取得します
		/// </summary>
		[Browsable(true), Category("動作"), Description("スライダによる単変化の値を設定または取得します")]
		public decimal SlideChange
		{
			get { return _slideChange; }
			set
			{
				if (value > 0)
					_slideChange = value;
			}
		}

		/// <summary>
		/// クリックによる増減操作でいくつの値が変化するかを設定または取得します。
		/// </summary>
		[Browsable(true), Category("動作"), Description("クリックによる増減操作でいくつの値が変化するかを設定または取得します。")]
		public decimal ClickChange
		{
			get { return _clickChange; }
			set
			{
				if (value > 0)
					_clickChange = value;
			}
		}

		/// <summary>
		/// 数値の直接入力時にツールチップを表示するか否かを設定もしくは取得します。
		/// </summary>
		[Browsable(true), Category("動作"),	Description("数値の直接入力時にツールチップを表示するか否かを設定もしくは取得します。")]
		public bool UseToolTip
		{
			get { return _useToolTip; }
			set { _useToolTip = value; }
		}

		/// <summary>
		/// クリックによる増減を表すボタンを表示するか否かを設定もしくは取得します。
		/// </summary>
		[Browsable(true), Category("動作"), Description("クリックによる増減を表すボタンを表示するか否かを設定もしくは取得します。")]
		public bool ShowButton
		{
			get { return _showButton; }
			set { _showButton = value; }
		}

		/// <summary>
		/// 背景バーの表示有無を設定もしくは取得します。
		/// </summary>
		[Browsable(true), Category("表示"), Description("背景バーの表示有無を設定もしくは取得します。")	]
		public bool VisibleBackBar
		{
			get { return _useBar; }
			set 
			{
				_useBar = value;
				DrawValue(); 
			}
		}

		/// <summary>
		/// 背景バーの表示色を設定もしくは取得します。
		/// </summary>
		[Browsable(true), Category("表示"), Description("背景バーの表示色を設定もしくは取得します。")]
		public Color BackBarColor
		{
			get { return _barColor; }
			set
			{ 
				_barColor = value; 
				DrawValue(); 
			}
		}

		/// <summary>
		/// アンダーバーの表示有無を設定もしくは取得します。
		/// </summary>
		[Browsable(true), Category("表示"), Description("アンダーバーの表示有無を設定もしくは取得します。")]
		public bool UnderBar
		{
			get { return _underBar; }
			set
			{
				_underBar = value;
				DrawValue();
			}
		}

		/// <summary>
		/// 背景バー用ブラシを取得します。
		/// </summary>
		SolidBrush BackBarBrush
		{
			get
			{
				Color backColor = Enabled ? BackBarColor : Color.FromArgb(240, 240, 240);
				if (_backbarBrush == null) _backbarBrush = new SolidBrush(backColor);
				if (_backbarBrush.Color != backColor) _backbarBrush.Color = backColor;
				return _backbarBrush;
			}
		}

		/// <summary>
		/// 文字の塗りつぶし用ブラシを取得します。
		/// </summary>
		SolidBrush TextBrush
		{
			get
			{
				Color textColor = Enabled ? ForeColor : Color.LightGray;
				if (_brush == null) _brush = new SolidBrush(textColor);
				if (_brush.Color != textColor) _brush.Color = textColor;
				return _brush;
			}
		}

		/// <summary>
		/// 入力編集中か否かを取得します。
		/// </summary>
		bool NowInputing
		{
			get
			{
				return txtBox_input.Visible;
			}
		}

		#endregion

		#region 操作

		/// <summary>
		/// 再描画を要求します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void RequestRefresh(object sender, EventArgs e)
		{
			DrawValue();
		}

		/// <summary>
		/// 数値を描画します。
		/// </summary>
		void DrawValue()
		{
			// ピクチャボックスを用意
			if (_picBox.Image != null)
				_picBox.Image.Dispose();

			_picBox.Image = new Bitmap(_picBox.Width, _picBox.Height);

			// 描画文字を用意
			GraphicsPath gp = new GraphicsPath();
			gp.AddString(Value.ToString() + " " + Unit, Font.FontFamily, (int)Font.Style, Font.Size, Point.Empty, StringFormat.GenericDefault);

			Matrix m = new Matrix();
			m.Scale(0.9f, 0.9f);
			while (gp.GetBounds().Width > (int)(Width * 0.9))
				gp.Transform(m);

			int top = (int)((this.Height - Font.Size) / 2);
			int left = (int)((this.Width - gp.GetBounds().Width) / 2);
			Matrix t = new Matrix();
			t.Translate(left, top);
			gp.Transform(t);

			Graphics g = Graphics.FromImage(_picBox.Image);

			// 背景バーの描画
			if (VisibleBackBar)
			{
				float width = 0f;
				if (Maximum != Minimum) width = (float)(Width * (Value - Minimum) / (Maximum - Minimum));
				g.FillRectangle(BackBarBrush, 0, 0, width, Height);
			}

			// アンダーバーの描画
			if (UnderBar)
			{
				g.FillRectangle(TextBrush, 0, Height - 1, Width, 1);
			}

			// 数値の描画
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.FillPath(TextBrush, gp);

			g.Dispose();
		}

		#endregion

		#region イベント

		/// <summary>
		/// スライダの値が変化した時に呼び出されます。
		/// </summary>
		[Browsable(true), Description("スライダの値が変化した時に呼び出されます。")]
		public event EventHandler ValueChanged
		{
			add { _valueChanged += value; }
			remove { _valueChanged -= value; }
		}

		/// <summary>
		/// スライダの値の編集が終了した時に呼び出されます。
		/// </summary>
		[Browsable(true), Description("スライダの値の編集が終了した時に呼び出されます。")]
		public event EventHandler EndChange
		{
			add { _endEdit += value; }
			remove { _endEdit -= value; }
		}

		#endregion

		#region イベント対応

		void txtBox_input_Enter(object sender, EventArgs e)
		{
			txtBox_input.Font = new Font(txtBox_input.Font.FontFamily, Font.Size * 0.8f);

			if (_useToolTip)
				_tooltip.Show(Math.Round(Minimum, _precision).ToString() + "～" +
								   Math.Round(Maximum, _precision).ToString(), txtBox_input, new Point(txtBox_input.Width / 3, -50), 2000);
		}

		void txtBox_input_Leave(object sender, EventArgs e)
		{
			if (!txtBox_input.Visible) return;

			_picBox.Visible = true;
			txtBox_input.Visible = false;

			try
			{
				decimal value = decimal.Parse(txtBox_input.Text);
				Value = value;
				_tooltip.Hide(txtBox_input);

				if (_endEdit != null)
					_endEdit(this, e);
			}
			catch
			{ }
		}

		void txtBox_input_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				txtBox_input_Leave(sender, e);
		}

		void _dragTimer_Tick(object sender, EventArgs e)
		{
			_nowDragging = true;
			_dragTimer.Interval = 50;

			if (Math.Abs(_preCursor.X - Cursor.Position.X) >= _sensitivity)
			{
				Value -= (int)((_preCursor.X - Cursor.Position.X) / _sensitivity) * _slideChange;

				if (_visibleCursorOnSlide) _preCursor.X = Cursor.Position.X;
				else Cursor.Position = _preCursor;
			}
			if (EnableUpDown && Math.Abs(_preCursor.Y - Cursor.Position.Y) >= _sensitivity)
			{
				Value -= (int)((_preCursor.Y - Cursor.Position.Y) / _sensitivity) * _slideChange;

				if (_visibleCursorOnSlide) _preCursor.Y = Cursor.Position.Y;
				else Cursor.Position = _preCursor;
			}
		}

		void picBox_MouseDown(object sender, MouseEventArgs e)
		{
			_preCursor = Cursor.Position;
			_dragTimer.Enabled = true;

			if (!_visibleCursorOnSlide) Cursor.Hide();
		}

		void picBox_MouseUp(object sender, MouseEventArgs e)
		{
			if (_nowDragging)
			{
				_nowDragging = false;

				if (_endEdit != null)
					_endEdit(this, e);
			}
			else
			{
				int delta = 0;
				if (_btnSub.Bounds.Contains(_picBox.PointToClient(Cursor.Position))) delta = -1;
				else if (_btnAdd.Bounds.Contains(_picBox.PointToClient(Cursor.Position))) delta = 1;

				if (delta != 0)
				{
					Value += delta * ClickChange;
					_picBox.Refresh();
				}
			}
			_dragTimer.Enabled = false;
			_dragTimer.Interval = 200;

			if (!_visibleCursorOnSlide) Cursor.Show();
		}

		void picBox_MouseClick(object sender, MouseEventArgs e)
		{
			if (_nowDragging) return;

			if (!NowInputing)
			{
				_picBox.Visible = false;

				txtBox_input.Top = (Height - txtBox_input.Height) / 2;
				txtBox_input.Text = _value.ToString();
				txtBox_input.Visible = true;
				txtBox_input.Focus();
				txtBox_input.SelectAll();
			}
		}

		void picBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (!_nowDragging) picBox_MouseUp(sender, e);
		}
		
		void picBox_MouseEnter(object sender, EventArgs e)
		{
			if (_showButton)
			{
				int size = Math.Min(this.Width / 5, this.Height * 2 / 3);
				_btnAdd.Width =  _btnAdd.Height = size;
				_btnAdd.Left = this.Width - _btnAdd.Width - 2;
				_btnAdd.Top = (this.Height - _btnAdd.Height) / 2;

				_btnSub.Width = _btnSub.Height = size;
				_btnSub.Left = 2;
				_btnSub.Top = (this.Height - _btnSub.Height) / 2;

				_btnAdd.Visible = true;
				_btnSub.Visible = true;
			}
		}

		void picBox_MouseLeave(object sender, EventArgs e)
		{
			_btnAdd.Visible = false;
			_btnSub.Visible = false;
		}

		void _picBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (!ShowButton || _nowDragging) return;

			if (_btnAdd.Bounds.Contains(this.PointToClient(Cursor.Position)) ||
				_btnSub.Bounds.Contains(this.PointToClient(Cursor.Position)))
				_picBox.Cursor = _btnSub.Cursor;
			else
				_picBox.Cursor = Cursor;
		}

		private void _btnAdd_Click(object sender, EventArgs e)
		{
			Value += ClickChange;
		}

		private void _btnSub_Click(object sender, EventArgs e)
		{
			Value -= ClickChange;
		}

		void NumericSlider_EnabledChanged(object sender, EventArgs e)
		{
			DrawValue();
		}

		#endregion


	}
}
