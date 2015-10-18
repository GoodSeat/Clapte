using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using GoodSeat.Clapte.Views.Forms;

namespace GoodSeat.Clapte
{
	static class Program
	{
		static FormOfMain s_mainForm;
		static System.Threading.Mutex s_mutex;

		[DllImport("User32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
		private static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

		[DllImport("User32.dll", EntryPoint = "SendMessage")]
		private static extern Int32 SendMessage(IntPtr hWnd, uint Msg, Int32 wParam, Int32 lParam);

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
#if !DEBUG
			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(OnThreadException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#endif
			// Mutexクラスの作成
			bool createdNew;
			s_mutex = new System.Threading.Mutex(true, "Clapte", out createdNew);
			if (!createdNew)
			{
				IntPtr hWnd = FindWindow(null, "Clapte_FormOfMain");
                SendMessage(hWnd, FormOfMain.WM_CLAPTE, 0, 0); 
			}
			else
			{
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                s_mainForm = new FormOfMain();
                Application.Run(s_mainForm);

				try
				{
					s_mutex.ReleaseMutex(); // ミューテックスを解放する
				}
				catch { }
			}
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			OnCrash(e.ExceptionObject.ToString());
		}

		static void OnThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			OnCrash(e.Exception.ToString());
		}

		static void OnCrash(string errorInfomation)
		{
			try
			{
				System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + "\\error.txt", false, System.Text.Encoding.Default);
				sw.Write(errorInfomation);
				sw.Close();
				sw.Dispose();

				// クラッシュ直前にデータを保存！！
				string filepath = s_mainForm.SaveCrashData();
				MessageBox.Show("ご迷惑をおかけし、申し訳ありません。\n予期されていないエラーが発生したため、Clapteを終了します。\n\n" +
											"※ 現時点の設定は、" + filepath + "に保存されました。同ファイルの名前を「Setting.xml」に置き換えることでクラッシュ時の設定を復元できます。\n" +
											"※ このエラーの情報は、CrashInfo.txtに保存されます。", "予期されていないエラー",
											MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch { }
			finally { Application.Exit(); }
		}
	}
}
