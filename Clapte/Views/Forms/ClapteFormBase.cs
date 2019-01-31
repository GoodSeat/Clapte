// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using GoodSeat.Clapte.Views.Components;

namespace GoodSeat.Clapte.Views.Forms
{
    /// <summary>
    /// Clapteで扱うフォームの共通基底クラスを表します。
    /// </summary>
    public partial class ClapteFormBase : Form
    {
        bool _visibleTitle = true;
        bool _sizable = true;
        SolidBrush _titleBrush = new SolidBrush(Color.Black);
        
        /// <summary>
        /// Clapteフォームを初期化します。
        /// </summary>
        public ClapteFormBase()
        {
            InitializeComponent();
        }

        private void ClapteFormBase_Load(object sender, EventArgs e)
        {
            ModifyControlPosition();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            _labelTitle.Text = Text;

            using (Font titleFont = new Font(Font.FontFamily, Font.Size * 1.2f))
            {
                if (_picTitleBar.Image != null) _picTitleBar.Image.Dispose();

                _picTitleBar.Image = new Bitmap(_picTitleBar.Width, _picTitleBar.Height);
                Graphics g = Graphics.FromImage(_picTitleBar.Image);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.DrawString(Text, titleFont, _titleBrush, new PointF(0f, 0f));
                g.Dispose();

                Point offset = new Point();
                Bitmap basePic = _picTitleBar.Image as Bitmap;
                _picTitleBar.Image = AppendDropShadow(basePic, 1, new Point(3, 3), Color.DarkGray, out offset);
                basePic.Dispose();
            }
        }


        /// <summary>
        /// アンカー位置の崩れた各コントロールを修正します。
        /// </summary>
        protected void ModifyControlPosition()
        {
            _frameLeft.Height = (Height - _frameTopLeft.Height - _frameBottomLeft.Height) + 50;
            _frameRight.Height = (Height - _frameTopRight.Height - _frameBottomRight.Width) + 50;
            _frameTop.Width = (Width - _frameTopLeft.Width - _frameTopRight.Width) + 50;
            _frameBottom.Width = (Width - _frameBottomLeft.Width - _frameBottomRight.Width) + 50;

            _frameBottomLeft.Top = Height - _frameBottomLeft.Height;
            _frameBottom.Top = Height - _frameBottom.Height;
            _frameBottomRight.Top = Height - _frameBottomRight.Height;
            _frameTop.Left = _frameTopLeft.Right; 
            _frameRight.Left = Width - _frameRight.Width; 
            _frameTopRight.Left = Width - _frameTopRight.Width;
            _frameBottomRight.Left = Width - _frameBottomRight.Width;
            
            _btnOK.Left = Width - _btnCancel.Width - 3 - _btnOK.Width - 1; _btnOK.Top = Height - _btnOK.Height - 3;
            _btnCancel.Left = Width - _btnCancel.Width - 3; _btnCancel.Top = Height - _btnCancel.Height - 3;

            _btnClose.Left = Width - _btnClose.Width - 4;
            _btnTopMost.Left = _btnClose.Left - _btnTopMost.Width - 1;
            _btnOption.Left = _btnTopMost.Left - _btnOption.Width - 1;

            _picTitleBar.Width = Width - 68;

            _btnTopMost.UnFocusImage = TopMost ? Properties.Resources.Icon_PushPin : Properties.Resources.Icon_PushPin_Unfocus;
        }

        /// <summary>
        /// タスクボタンの配置を縦配置に修正します。
        /// </summary>
        public void SetLayoutVertical()
        {
            _btnTopMost.Left = _btnClose.Left - 2;
            _btnTopMost.Top = _btnClose.Bottom + 2;
            _btnTopMost.BringToFront();

            _btnOption.Left = _btnClose.Left - 2;
            _btnOption.Top = _btnTopMost.Bottom + 2;
            _btnOption.BringToFront();
        }

        /// <summary>
        /// OKが押されたときに呼び出されます。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnOK(EventArgs e) { }

        /// <summary>
        /// キャンセルが押されたときに呼び出されます。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCancel(EventArgs e) { }

        /// <summary>
        /// 画面右上の閉じるボタンが押されたときに呼び出されます。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClose(EventArgs e) { OnCancel(e); }

        /// <summary>
        /// オプションが押されたときに呼び出されます。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnOption(EventArgs e) { }


        /// <summary>
        /// オプションボタンを表示するか否かを設定もしくは取得します。
        /// </summary>
        public bool ShowOption
        {
            get { return _btnOption.Visible; }
            set { _btnOption.Visible = value; }
        }

        /// <summary>
        /// OKボタンを表示するか否かを設定もしくは取得します。
        /// </summary>
        public bool ShowOKButton
        {
            get { return _btnOK.Visible; }
            set { _btnOK.Visible = value; }
        }

        /// <summary>
        /// Cancelボタンを表示するか否かを設定もしくは取得します。
        /// </summary>
        public bool ShowCancelButton
        {
            get { return _btnCancel.Visible; }
            set { _btnCancel.Visible = value; }
        }

        /// <summary>
        /// ウインドウタイトルの可視状態を設定もしくは取得します。
        /// </summary>
        public bool VisibleTitle
        {
            get { return _visibleTitle; }
            set { _visibleTitle = value; _picTitleBar.Visible = value; }
        }

        /// <summary>
        /// ウインドウサイズを変更できるか否かを設定もしくは取得します。
        /// </summary>
        public bool Sizable
        {
            get { return _sizable; }
            set { _sizable = value; }
        }

        private void _btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            OnOK(e);
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            OnCancel(e);
        }

        private void _btnClose_Click(object sender, EventArgs e)
        {
            OnClose(e);
        }

        private void _btnOption_Click(object sender, EventArgs e)
        {
            OnOption(e);
        }

        private void _btnTopMost_Click(object sender, EventArgs e)
        {
            TopMost = !TopMost;

            _btnTopMost.UnFocusImage = TopMost ? Properties.Resources.Icon_PushPin : Properties.Resources.Icon_PushPin_Unfocus;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;

            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST && m.Result.ToInt32() == (int)ChameleonPictureBox.WindowsHitPosition.Client)
            {
                int cornerHandleLength = 8;
                int sideHandleLength = 4;
                int heightCaption = 23;

                Point cursorPosition = PointToClient(Cursor.Position);
                if (cursorPosition.Y <= heightCaption)
                    m.Result = (IntPtr)ChameleonPictureBox.WindowsHitPosition.Caption;

                if (!Sizable) return;
                else if (cursorPosition.X <= cornerHandleLength && cursorPosition.Y <= cornerHandleLength)
                    m.Result = (IntPtr)ChameleonPictureBox.WindowsHitPosition.TopLeft;
                else if (cursorPosition.X <= cornerHandleLength && cursorPosition.Y >= Height - cornerHandleLength)
                    m.Result = (IntPtr)ChameleonPictureBox.WindowsHitPosition.BottomLeft;
                else if (cursorPosition.X >= Width - cornerHandleLength && cursorPosition.Y <= cornerHandleLength)
                    m.Result = (IntPtr)ChameleonPictureBox.WindowsHitPosition.TopRight;
                else if (cursorPosition.X >= Width - cornerHandleLength && cursorPosition.Y >= Height - cornerHandleLength)
                    m.Result = (IntPtr)ChameleonPictureBox.WindowsHitPosition.BottomRight;
                else if (cursorPosition.X <= sideHandleLength)
                    m.Result = (IntPtr)ChameleonPictureBox.WindowsHitPosition.Left;
                else if (cursorPosition.X >= Width - sideHandleLength)
                    m.Result = (IntPtr)ChameleonPictureBox.WindowsHitPosition.Right;
                else if (cursorPosition.Y <= sideHandleLength)
                    m.Result = (IntPtr)ChameleonPictureBox.WindowsHitPosition.Top;
                else if (cursorPosition.Y >= Height - sideHandleLength)
                    m.Result = (IntPtr)ChameleonPictureBox.WindowsHitPosition.Bottom;
            }
        }

        bool OnCursor(Control control)
        {
            Rectangle rect = new Rectangle(control.Location, control.Size);
            Point test = PointToClient(Cursor.Position);
            return rect.Contains(test);
        }

        /// <summary>
        /// ドロップシャドウを加えた新しいビットマップを作成します。
        /// </summary>
        /// <param name="srcBmp">対象のビットマップ</param>
        /// <param name="blur">ぼかし</param>
        /// <param name="distance">影の距離</param>
        /// <param name="shadowColor">影の色</param>
        /// <param name="baseOffset">描画先オフセット値の格納先</param>
        /// <returns>ドロップシャドウが加えられた新しいビットマップ</returns>
        private static Bitmap AppendDropShadow(Bitmap srcBmp, int blur, Point distance, Color shadowColor, out Point baseOffset)
        {
            baseOffset = new Point(0, 0);
            if (srcBmp == null || blur < 0)            return null;

            // ドロップシャドウを含めた新しいサイズを算出
            Rectangle srcRect = new Rectangle(0, 0, srcBmp.Width, srcBmp.Height);
            Rectangle shadowRect = srcRect;
            shadowRect.Offset(distance.X, distance.Y);
            Rectangle shadowBlurRect = shadowRect;
            shadowBlurRect.Inflate(blur, blur);
            Rectangle destRect = Rectangle.Union(srcRect, shadowBlurRect);
            baseOffset.X = destRect.X - srcRect.X;
            baseOffset.Y = destRect.Y - srcRect.Y;

            Bitmap destBmp = new Bitmap(destRect.Width, destRect.Height, PixelFormat.Format32bppArgb);

            // 影部分をレンダリングする
            BitmapData destBmpData = destBmp.LockBits(new Rectangle(0, 0, destRect.Width, destRect.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            BitmapData srcBmpData = srcBmp.LockBits(srcRect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* destLine = (byte*)destBmpData.Scan0 + (shadowBlurRect.Y - destRect.Y) * destBmpData.Stride + (shadowBlurRect.X - destRect.X) * 4;
                byte* srcBeginLine = (byte*)srcBmpData.Scan0 + (- blur - blur) * srcBmpData.Stride + (- blur - blur) * 4;

                int destWidth = shadowBlurRect.Width;
                int destHeight = shadowBlurRect.Height;

                int srcWidth = srcBmp.Width;
                int srcHeight = srcBmp.Height;

                int div = (1 + blur + blur) * (1 + blur + blur);

                byte r = shadowColor.R;
                byte g = shadowColor.G;
                byte b = shadowColor.B;
                float alpha = 100.0f / 255.0f; // shadow<span class="hilite">Co</span>lor.A / 255.0f;

                int destStride = destBmpData.Stride;
                int srcStride = srcBmpData.Stride;

                for (int destY = 0; destY < destHeight; destY++, destLine += destStride, srcBeginLine += srcStride)
                {
                    byte* dest = destLine;
                    byte* srcBegin = srcBeginLine;
                    for (int destX = 0; destX < destWidth; destX++, dest += 4, srcBegin += 4)
                    {
                        // α値をぼかす
                        int total = 0;
                        byte* srcLine = srcBegin;
                        for (int srcY = destY - blur - blur; srcY <= destY; srcY++, srcLine += srcStride)
                        {
                            if (srcY >= 0 && srcY < srcHeight)
                            {
                                byte* src = srcLine;
                                for (int srcX = destX - blur - blur; srcX <= destX; srcX++, src += 4)
                                {
                                    if (srcX >= 0 && srcX < srcWidth)
                                    {
                                        total += src[3];
                                    }
                                }
                            }
                        }

                        dest[0] = b;
                        dest[1] = g;
                        dest[2] = r;
                        dest[3] = (byte)((total / div) * alpha);
                    }
                }
            }

            srcBmp.UnlockBits(srcBmpData);
            destBmp.UnlockBits(destBmpData);

            // 元の画像を重ねる
            using (Graphics g = Graphics.FromImage(destBmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.DrawImage(srcBmp, srcRect.X - destRect.X, srcRect.Y - destRect.Y, srcBmp.Width, srcBmp.Height);
            }

            return destBmp;
        }


    }
}
