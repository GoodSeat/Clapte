using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GoodSeat.Sio.Xml;
using GoodSeat.Sio.Xml.Serialization;
using GoodSeat.Liffom;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Clapte.Views.Forms.SettingPanels;
using GoodSeat.Clapte.ViewModels;

namespace GoodSeat.Clapte.Views.Forms
{
	/// <summary>
	/// Clapteの常駐メインフォームを表します。
	/// </summary>
	public partial class FormOfMain : Form
	{
		object _lockClipboard = new object();
		ClipBoardWatcher _clipBoradWatcher;
		HotkeyManager _hotkeyManager;

		FormOfSetting _formOfSetting;
        FormOfClaptePad _formOfClaptePad;

        FormulaCellListViewModel _claptePad;

		/// <summary>
		/// Clapteの常駐メインフォームを初期化します。
		/// </summary>
		public FormOfMain()
		{
			InitializeComponent();
			this.Disposed += new EventHandler(FormOfMain_Disposed);

			_hotkeyManager = new HotkeyManager(Handle);
			_hotkeyManager.HotkeyPressed += new EventHandler(_hotkeyManager_HotkeyPressed);

			_clipBoradWatcher = new ClipBoardWatcher();
			_clipBoradWatcher.DrawClipBoard += new EventHandler(_clipBoradWatcher_DrawClipBoard);

			TaskTrayIcon.Text = "Clapte " + VersionInformationPanel.AssemblyVersion;
			if (VersionInformationPanel.IsBeta) TaskTrayIcon.Text += " Beta";
		}

		/// <summary>
		/// モデルビューを初期化します。
		/// </summary>
		void InitializeModelView()
		{
			this.ActionWithBalloonClick = true;

			UserFunctions = new FunctionListViewModel();
			UserConstants = new ConstantListViewModel();

			ClapteCore = new ClapteCoreViewModel();
			ClapteCore.MessageUpdate += new EventHandler(ClapteCore_MessageUpdate);

			ClapteCore.Solver.Target.UserConstants = UserConstants.Target;
			ClapteCore.Solver.Target.UserFunctions = UserFunctions.Target;
			ClapteCore.Solver.SettingUpdated += new EventHandler(Solver_SettingUpdated);

            _claptePad = new FormulaCellListViewModel(ClapteCore.Solver, UserConstants, UserFunctions);
			_formOfClaptePad = new FormOfClaptePad(_claptePad);
		}

		#region プロパティ

		/// <summary>
		/// ClaptePadフォームのビューを取得します。
		/// </summary>
		public FormOfClaptePad ClaptePadView { get { return _formOfClaptePad; } }

		/// <summary>
		/// Clapteのタスクトレイアイコンを取得します。
		/// </summary>
		NotifyIcon TaskTrayIcon { get { return _notifyIconClapte; } }

		/// <summary>
		/// バルーンチップのクリックでアクションを実行するか否かを設定もしくは取得します。
		/// </summary>
		public bool ActionWithBalloonClick { get; set; }

		/// <summary>
		/// ホットキー管理オブジェクトを取得します。
		/// </summary>
		public HotkeyManager HotkeyManager { get { return _hotkeyManager; } }

		/// <summary>
		/// Clapteコマンドの実行管理モデルビューを取得します。
		/// </summary>
        public ClapteCoreViewModel ClapteCore { get; private set; }

		/// <summary>
		/// ユーザー定義の定数リストモデルビューを取得します。
		/// </summary>
        public ConstantListViewModel UserConstants { get; private set; }

		/// <summary>
		/// ユーザー定義の関数リストモデルビューを取得します。
		/// </summary>
        public FunctionListViewModel UserFunctions { get; private set; }

		#endregion

		#region 初期化と終了処理

		private void FormOfMain_Load(object sender, EventArgs e)
		{
			InitializeModelView();

			LoadSetting("Setting.xml");

			this.WindowState = FormWindowState.Minimized;

#if DEBUG
			_menuCalculator_Click(sender, e);

			ToolStripMenuItem testMenu = new ToolStripMenuItem("テストケース作成(&T)");
			_menuClapte.Items.Add(testMenu);
//			testMenu.Click += (testSender, testE) => { ClapteTest.CreateTestCase(_clapteWatcher.Solver); };

			ToolStripMenuItem testFormuMenu = new ToolStripMenuItem("テストフォーム起動(&F)");
			_menuClapte.Items.Add(testFormuMenu);
			testFormuMenu.Click += (testSender, testE) =>
				{
					var form = new GoodSeat.Liffom.Utilities.TestForm();
					form.Show();
				};
#endif
		}

		private void FormOfMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			SaveSetting("Setting.xml");

            ClaptePadView.Close();
			TaskTrayIcon.Visible = false;
		}

		void FormOfMain_Disposed(object sender, EventArgs e)
		{
			_clipBoradWatcher.Dispose();
			_hotkeyManager.Dispose();
		}

		#endregion

		#region Viewの操作

		/// <summary>
		/// 現在の生成済みコマンドに関連付けられたアクションを実行します。
		/// </summary>
		private void InformAction()
		{
			_clipBoradWatcher.DrawClipBoard -= new EventHandler(_clipBoradWatcher_DrawClipBoard);
			try
			{
				ClapteCore.InformAction();
			}
			catch (Exception e)
			{
				TaskTrayIcon.ShowBalloonTip(ClapteCore.LimitTime * 1000,
						"処理失敗",
						"処理に失敗しました。\nこのメッセージが出ている間、再試行できます。",
						ToolTipIcon.Error);
			}
			finally
			{
				_clipBoradWatcher.DrawClipBoard += new EventHandler(_clipBoradWatcher_DrawClipBoard);
			}
		}

		// バルーンチップのクリック
		private void _notifyIconClapte_BalloonTipClicked(object sender, EventArgs e)
		{
			if (ActionWithBalloonClick) InformAction();
		}

		// ホットキー通知時
		void _hotkeyManager_HotkeyPressed(object sender, EventArgs e)
		{
			InformAction();
		}

		// クリップボード変化時
		void _clipBoradWatcher_DrawClipBoard(object sender, EventArgs e)
		{
			if (!_menuEnable.Checked) return;

			lock (_lockClipboard)
			{
				_clipBoradWatcher.DrawClipBoard -= new EventHandler(_clipBoradWatcher_DrawClipBoard);
				_clipBoradWatcher.Enable = false;
                try
                {
                    if (Clipboard.ContainsText())
                    {
                        ClapteCore.InformTextCommand(Clipboard.GetText());
                    }
                }
                catch (Exception exc)
                {
#if DEBUG
                    TaskTrayIcon.ShowBalloonTip(ClapteCore.LimitTime * 1000, "予期しない例外", exc.Message, ToolTipIcon.Error);
#endif
                }
				finally
				{
					_clipBoradWatcher.Enable = true;
					_clipBoradWatcher.DrawClipBoard += new EventHandler(_clipBoradWatcher_DrawClipBoard);
				}
			}
		}

		// クリップボード監視切り替え
		private void _menuEnable_Click(object sender, EventArgs e)
		{
			if (_menuEnable.Checked) _clipBoradWatcher.DrawClipBoard -= new EventHandler(_clipBoradWatcher_DrawClipBoard);
			else _clipBoradWatcher.DrawClipBoard += new EventHandler(_clipBoradWatcher_DrawClipBoard);

			_menuEnable.Checked = !_menuEnable.Checked;
		}

		// 設定画面を開く
		private void _menuSetting_Click(object sender, EventArgs e)
		{
			if (_formOfSetting == null || _formOfSetting.IsDisposed)
			{
			    _formOfSetting = new FormOfSetting(this);
			    _formOfSetting.Show();
			}
			else
			{
			    _formOfSetting.Focus();
			}
		}

		// 計算機を開く
		private void _menuCalculator_Click(object sender, EventArgs e)
		{
			_formOfClaptePad.Show();
		}
		private void _notifyIconClapte_MouseDoubleClick(object sender, MouseEventArgs e) { _menuCalculator_Click(sender, e); }
		private void _notifyIconClapte_Click(object sender, EventArgs e)
		{
			if (_formOfClaptePad != null && !_formOfClaptePad.IsDisposed)
			{
				_formOfClaptePad.Focus();
			}
		}

		// ヘルプ
		private void _menuHelp_Click(object sender, EventArgs e) { ClapteHelp.Show(this); }

		// 終了
		private void _menuExit_Click(object sender, EventArgs e) { this.Close(); }

		bool _topMostCalculator = false;

		private void _menuClapte_Opening(object sender, CancelEventArgs e)
		{
			if (_formOfClaptePad != null && !_formOfClaptePad.IsDisposed)
			{
				_topMostCalculator = _formOfClaptePad.TopMost;
				_formOfClaptePad.TopMost = false;
			}
		}

		private void _menuClapte_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (_formOfClaptePad != null && !_formOfClaptePad.IsDisposed)
				_formOfClaptePad.TopMost = _topMostCalculator;
		}

		// 有効数値考慮切替
		private void _menuConsiderValid_Click(object sender, EventArgs e)
		{
			ClapteCore.Solver.ConsiderValidDigit = !(sender as ToolStripMenuItem).Checked;
            ClapteCore.Solver.UpdateSetting();
		}

		// 計算モード切替
		private void _menuMode_Click(object sender, EventArgs e)
		{
			if ((sender as ToolStripMenuItem).Checked)
				ClapteCore.Solver.Mode = CalculateMode.Decimal;
			else
				ClapteCore.Solver.Mode = CalculateMode.Fraction;

            ClapteCore.Solver.UpdateSetting();
		}

        //// 積算記号省略
        //private void _menuOmitMultiple_Click(object sender, EventArgs e)
        //{
        //    ClapteCore.Solver.PermitOmitProductMark =  !(sender as ToolStripMenuItem).Checked;
        //    ClapteCore.Solver.UpdateSetting();
        //}

		// 累乗記号省略
		private void _menuOmitPower_Click(object sender, EventArgs e)
		{
			ClapteCore.Solver.PermitOmitPowerMark = !(sender as ToolStripMenuItem).Checked;
            ClapteCore.Solver.UpdateSetting();
		}

		#endregion

		#region ViewModelからのメッセージ通知

		// ClapteCoreからのメッセージ更新通知時
		void ClapteCore_MessageUpdate(object sender, EventArgs e)
		{
			var result = ClapteCore.Message;
			TaskTrayIcon.ShowBalloonTip(ClapteCore.LimitTime * 1000, result.Title, result.Message, result.Icon);
		}

		// Solverの設定更新時
		void Solver_SettingUpdated(object sender, EventArgs e)
		{
			_menuConsiderValid.Checked = ClapteCore.Solver.ConsiderValidDigit;
            _menuOmitPower.Checked = ClapteCore.Solver.PermitOmitPowerMark;
			_menuMode.Checked = (ClapteCore.Solver.Mode == CalculateMode.Fraction);
		}

		#endregion

		#region 操作

		/// <summary>
		/// 指定数式の定義を開きます。
		/// </summary>
		/// <param name="target">対象の数式。</param>
		public void OpenDefine(Formula target)
		{
			_menuSetting_Click(this, EventArgs.Empty);
			_formOfSetting.OpenDefine(target);
		}

		/// <summary>
		/// クラッシュ時のデータを保存し、保存先のパスを返します。
		/// </summary>
		/// <returns>クラッシュ時の設定が保存されたファイル名。</returns>
		public string SaveCrashData()
		{
			DateTime now = DateTime.Now;
			string date = string.Format("{0}.{1}.{2}_{3}.{4}.{5}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
			string filename = "SettingCrashOn" + date + ".xml";
			SaveSetting(filename);
			return filename;
		}

		/// <summary>
		/// Clapteの設定を表すXmlElementオブジェクトを生成して取得します。
		/// </summary>
		/// <returns>Clapteの設定を表すXmlElementオブジェクト。</returns>
		public XmlElement CreateSerializeXmlElement()
		{
			XmlElement clapteSettingElement = new XmlElement("ClapteSetting");
			clapteSettingElement.AddAttribute("Version", VersionInformationPanel.AssemblyVersion);

			// 計算機の設定の保存
			XmlElement claptePadElement = new XmlElement("ClaptePad");
			ClaptePadView.OnSerialize(claptePadElement);
			clapteSettingElement.AddElements(claptePadElement);

			// ユーザー定義定数の保存
			XmlElement userConstantsElement = new XmlElement("UserConstants");
			UserConstants.OnSerialize(userConstantsElement);
			clapteSettingElement.AddElements(userConstantsElement);

			// ユーザー定義関数の保存
			XmlElement userFunctionsElement = new XmlElement("UserFunctions");
			UserFunctions.OnSerialize(userFunctionsElement);
			clapteSettingElement.AddElements(userFunctionsElement);

			// 単位テーブルの保存
			XmlElement unitTablesElement = new XmlElement("UnitTables");
			foreach (var table in UnitConvertTable.ValidTables.Values)
			{
				var tableViewModel = new UnitConvertTableViewModel();
				tableViewModel.Target = table;

				XmlElement unitTableElement = new XmlElement("UnitTable");
				tableViewModel.OnSerialize(unitTableElement);
				unitTablesElement.AddElements(unitTableElement);
			}
			clapteSettingElement.AddElements(unitTablesElement);

			// ClapteCoreViewModelオブジェクトの保存
			XmlElement clapteCoreElement = new XmlElement("ClapteCore");
			ClapteCore.OnSerialize(clapteCoreElement);
			clapteSettingElement.AddElements(clapteCoreElement);

			return clapteSettingElement;
		}

		/// <summary>
		/// Clapteの設定を表すXmlElementオブジェクトから、設定を復元します。
		/// </summary>
		/// <param name="clapteSettingElement">Clapteの設定を表すXmlElementオブジェクト。</param>
		public void RestoreFromXmlElement(XmlElement clapteSettingElement)
		{
			// 計算機の設定の復元
			if (clapteSettingElement.GetElement("ClaptePad") != null)
				ClaptePadView.OnDeserialize(clapteSettingElement["ClaptePad"]);

			// ユーザー定義定数の復元
			UserConstants.OnDeserialize(clapteSettingElement["UserConstants"]);

			// ユーザー定義関数の復元
			UserFunctions.OnDeserialize(clapteSettingElement["UserFunctions"]);

			// 単位テーブルの復元
			UnitConvertTable.ValidTables.Clear();
			foreach (var unitTableElement in clapteSettingElement["UnitTables"].GetElements("UnitTable"))
			{
				var tableViewModel = new UnitConvertTableViewModel();
				tableViewModel.OnDeserialize(unitTableElement);
				UnitConvertTable.AddTable(tableViewModel.Target);
			}

			// ClapteCoreViewModelオブジェクトの復元
			ClapteCore.OnDeserialize(clapteSettingElement["ClapteCore"]);
		}

		#endregion

		#region 内部処理

		/// <summary>
		/// 設定を外部ファイルに保存します。
		/// </summary>
		/// <param name="fileName">保存先ファイル名。</param>
		private void SaveSetting(string fileName)
		{
			try
			{
				XmlFile file = new XmlFile(fileName);
				file.Element = CreateSerializeXmlElement();
				file.Save();
			}
			catch (Exception e)
			{
#if DEBUG
				MessageBox.Show(e.Message);
#endif
			}
		}

		/// <summary>
		/// 外部ファイルから設定を読み込みます。
		/// </summary>
		/// <param name="fileName">読込対象のファイル名。</param>
		private void LoadSetting(string fileName)
		{
			try
			{
                XmlFile file = new XmlFile(fileName);
                if (file.ExistTargetFile)
                {
                    file.Load();
					RestoreFromXmlElement(file.Element);
                }

//                XmlFile updateFile = new XmlFile("Updates.xml");
//                if (updateFile.ExistTargetFile)
//                {
//                    if (_clapteWatcher == null) _clapteWatcher = new ClapteWatcher(this);
//                    updateFile.Load();
////					_clapteWatcher.Solver.UpdateDeserialize(updateFile.Element);
//                }
			}
			catch (Exception e)
			{
				TaskTrayIcon.ShowBalloonTip(10000, "読込エラー", "設定の読み込みに失敗しました。設定を確認してください。\n\nエラーの説明:\n" + e.Message, ToolTipIcon.Error);
			}
		}

		#endregion

		/// <summary>
		/// Windowsメッセージを処理します。
		/// </summary>
		protected override void WndProc(ref Message message)
		{
			base.WndProc(ref message);

			if (_hotkeyManager != null) _hotkeyManager.OnWndProc(message);
		}

	}
}
