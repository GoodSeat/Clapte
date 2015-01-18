using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GoodSeat.Sio;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Clapte.Views.Forms
{
	/// <summary>
	/// Clapte計算機フォームを表します。
	/// </summary>
	public partial class FormOfCalculator : ClapteFormBase
	{
//		FormulaIntellisence _intellisence;
		FormOfMain _mainForm;

		/*
		static bool s_colorHighlight = true;
		static FormOfCalculator s_calculator;
		static Dictionary<FormulaIntellisence.CharaType, Color> s_colorMap = new Dictionary<FormulaIntellisence.CharaType, Color>();

		static FormOfCalculator()
		{
			s_colorMap.Add(FormulaIntellisence.CharaType.Function, Color.Blue);
			s_colorMap.Add(FormulaIntellisence.CharaType.Operator, Color.OrangeRed);
			s_colorMap.Add(FormulaIntellisence.CharaType.Variable, Color.Violet);
			s_colorMap.Add(FormulaIntellisence.CharaType.Unit, Color.Green);
		}

		/// <summary>
		/// 計算機上の文字色分け機能の有効可否を設定もしくは取得します。
		/// </summary>
		public static bool EnableColorHighlight
		{
			get { return s_colorHighlight; }
			set
			{
				s_colorHighlight = value;
				if (s_calculator != null)
				{
					RichTextBoxFormat format = s_calculator._intellisence.Format as RichTextBoxFormat;
					format.EnableColorHighlight = value;
				}
			}
		}

		/// <summary>
		/// 指定文字区分の色を設定します。
		/// </summary>
		/// <param name="targetType">設定対象の文字区分タイプ</param>
		/// <param name="color">文字色</param>
		public static void SetColorOf(FormulaIntellisence.CharaType targetType, Color color)
		{
			if (s_colorMap.ContainsKey(targetType)) s_colorMap.Remove(targetType);
			s_colorMap.Add(targetType, color);

			if (s_calculator != null) 
			{
				RichTextBoxFormat format = s_calculator._intellisence.Format as RichTextBoxFormat;
				if (format.ColorMap.ContainsKey(targetType)) format.ColorMap.Remove(targetType);
				format.ColorMap.Add(targetType, color);
			}
		}

		/// <summary>
		/// 指定文字区分の文字色を取得します。
		/// </summary>
		/// <param name="targetType">対象の文字区分タイプ</param>
		/// <returns></returns>
		public static Color GetColorOf(FormulaIntellisence.CharaType targetType)
		{
			if (s_colorMap.ContainsKey(targetType)) return s_colorMap[targetType];
			else if (s_calculator != null)
			{
				RichTextBoxFormat format = s_calculator._intellisence.Format as RichTextBoxFormat;
				if (format.ColorMap.ContainsKey(targetType)) return format.ColorMap[targetType];
			}
			return Color.Black;
		}


		// デザイナ用
		private FormOfCalculator()
		{
			InitializeComponent();
		}
		*/

		/// <summary>
		/// Clapte計算機フォームを初期化します。
		/// </summary>
		public FormOfCalculator(FormOfMain mainForm)
		{
			InitializeComponent();

			_mainForm = mainForm;
		}

		private void FormOfCalculator_Load(object sender, EventArgs e) { }
		/*
		{
			SetLayoutVertical();
			ShowOKButton = ShowCancelButton = false;
			s_calculator = this;

			ShowOption = false;

			// インテリセンス初期化
			_intellisence = new FormulaIntellisence(_textBoxInput, this);

			// 書式初期化
			RichTextBoxFormat format = _intellisence.Format as RichTextBoxFormat;
			format.ClickWithCtrl += new RichTextBoxFormat.TermClickedEventHandler(format_ClickWithCtrl);
			format.EnableColorHighlight = EnableColorHighlight;
			foreach (FormulaIntellisence.CharaType type in Enum.GetValues(typeof(FormulaIntellisence.CharaType))) SetColorOf(type, GetColorOf(type));

			// 画面の四隅のうち、最も近いところに表示する
			int minLength = int.MaxValue;
			foreach (Screen s in Screen.AllScreens)
			{
				Rectangle rect = s.WorkingArea;

				if (Math.Abs(rect.Top - Cursor.Position.Y) + Math.Abs(rect.Left - Cursor.Position.X) < minLength)
				{
					minLength = Math.Abs(rect.Top - Cursor.Position.Y) + Math.Abs(rect.Left - Cursor.Position.X);
					this.Top = rect.Top;
					this.Left = rect.Left;
				}
				if (Math.Abs(rect.Top - Cursor.Position.Y) + Math.Abs(rect.Right - Cursor.Position.X) < minLength)
				{
					minLength = Math.Abs(rect.Top - Cursor.Position.Y) + Math.Abs(rect.Right - Cursor.Position.X);
					this.Top = rect.Top;
					this.Left = rect.Right - this.Width;
				}
				if (Math.Abs(rect.Bottom - Cursor.Position.Y) + Math.Abs(rect.Right - Cursor.Position.X) < minLength)
				{
					minLength = Math.Abs(rect.Bottom - Cursor.Position.Y) + Math.Abs(rect.Right - Cursor.Position.X);
					this.Top = rect.Bottom - this.Height;
					this.Left = rect.Right - this.Width;
				}
				if (Math.Abs(rect.Bottom - Cursor.Position.Y) + Math.Abs(rect.Left - Cursor.Position.X) < minLength)
				{
					minLength = Math.Abs(rect.Bottom - Cursor.Position.Y) + Math.Abs(rect.Left - Cursor.Position.X);
					this.Top = rect.Bottom - this.Height;
					this.Left = rect.Left;
				}
			}
			VisibleTitle = false;
		}

		/// <summary>
		/// インテリセンスの書式を取得します。
		/// </summary>
		RichTextBoxFormat Format
		{
			get { return _intellisence.Format as RichTextBoxFormat; }
		}

		*/
		/// <summary>
		/// 計算を実行します。
		/// </summary>
		private void _btnExe_Click(object sender, EventArgs e) { }
		/*
		{
			try
			{
				_textBoxAnswer.Text = "";
				_imgInfo.Visible = false;

				ClapteSolverException errorInfo;
				string answer = _clapteWatcher.Solver.SolveFormula(_textBoxInput.Text, out errorInfo, true);
				_textBoxAnswer.Text = answer;

				if (errorInfo != null)
				{
					if (answer == "") _textBoxAnswer.Text = errorInfo.Message;
					else
					{
						_toolTipBalloon.SetToolTip(_imgInfo, errorInfo.Message);
						_imgInfo.Visible = true;
					}
				}
				_intellisence.Exit(true);
			}
			catch (Exception exc)
			{
				_textBoxAnswer.Text = exc.Message;
			}
		}
		*/

		protected override void OnCancel(EventArgs e)
		{
			this.Close();
		}

		// インテリセンスの構成
		private void _textBoxInput_Enter(object sender, EventArgs e) {}
		//{
		//    _clapteWatcher.Solver.RenewIntellisence(_intellisence, Solver.IntellisenceType.All);
		//}
		private void FormOfCalculator_Enter(object sender, EventArgs e) {}
		//{
		//    _intellisence.ClearTypeCache();
		//    _clapteWatcher.Solver.RenewIntellisence(_intellisence, Solver.IntellisenceType.All);
		//}

		// Enterで計算
		private void _textBoxInput_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) _btnExe_Click(sender, e);
		}

		private void FormOfCalculator_FormClosing(object sender, FormClosingEventArgs e){}
		//{
		//    _intellisence.Dispose();
		//    s_calculator = null;
		//}

		//// 定義ジャンプ
		//void format_ClickWithCtrl(object sender, TermClickedEventArgs e)
		//{
		//    Formula target = e.TargetFormula;
		//    _mainForm.OpenDefine(target);
		//}

		#region コンテキストメニュー

		private void _menuUndo_Click(object sender, EventArgs e)
		{
//			_intellisence.Format.Undo();
		}

		private void _menuRedo_Click(object sender, EventArgs e)
		{
//			_intellisence.Format.Redo();
		}

		private void _menuCut_Click(object sender, EventArgs e)
		{
			_textBoxInput.Cut();
		}

		private void _menuCopy_Click(object sender, EventArgs e)
		{
			_textBoxInput.Copy();
		}

		private void _menuPaste_Click(object sender, EventArgs e)
		{
//			_textBoxInput.Paste(DataFormats.GetFormat(DataFormats.Text));
		}

		private void _menuDelete_Click(object sender, EventArgs e)
		{
			string text = _textBoxInput.Text;
			_textBoxInput.Text = text.Substring(0, _textBoxInput.SelectionStart) + text.Substring(_textBoxInput.SelectionStart + _textBoxInput.SelectionLength);
			_textBoxInput.SelectionLength = 0;			
		}

		private void _menuOpenDefine_Click(object sender, EventArgs e)
		{
//			Format.OpenDefine();
		}

		private void _menuInput_Opening(object sender, CancelEventArgs e)
		{
			//_menuDelete.Enabled = _textBoxInput.SelectionLength != 0;
			//_menuPaste.Enabled = _textBoxInput.CanPaste(DataFormats.GetFormat(DataFormats.Text));
			//_menuUndo.Enabled = _intellisence.Format.CanUndo;
			//_menuRedo.Enabled = _intellisence.Format.CanRedo;
			//_menuOpenDefine.Enabled = Format.EnableOpenDefine;
		}

		#endregion

	}
}
