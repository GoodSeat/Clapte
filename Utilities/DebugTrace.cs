using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace GoodSeat.Sio.Utilities
{
	/// <summary>
	/// デバッグ時のデバッグトレース出力を提供する静的クラスです。
	/// </summary>
	public static class DebugTrace
	{
#if DEBUG
		static List<object> s_supliedIndent = new List<object>();
		static Dictionary<string, Stopwatch> s_mapTextStopWatch = new Dictionary<string, Stopwatch>();
		static Dictionary<int, Stopwatch> s_mapStopWatch = new Dictionary<int, Stopwatch>();
		readonly static int s_maxIndent = 40;
#endif

		/// <summary>
		/// 指定アンカーに時間を関連付けて記録します。
		/// </summary>
		/// <param name="anchor">関連付けるアンカー</param>
		private static void SetAnchor(int anchor)
		{
#if DEBUG
			if (s_mapStopWatch.ContainsKey(anchor))
			{
				s_mapStopWatch[anchor].Stop();
				s_mapStopWatch.Remove(anchor);
			}
			Stopwatch sw = new Stopwatch();
			s_mapStopWatch.Add(anchor, sw);
			sw.Start();
#endif
		}

		/// <summary>
		/// 指定文字列を出力します。
		/// </summary>
		/// <param name="msg">出力文字列</param>
		public static void Write(string msg)
		{
#if DEBUG
			Console.WriteLine(msg);
#endif
		}

		/// <summary>
		/// 指定文字列を出力し、指定アンカーに関連付けて時間計測を開始します。
		/// </summary>
		/// <param name="msg">出力文字列</param>
		/// <param name="anchor">関連付けるアンカー</param>
		public static void Write(string msg, int anchor)
		{
#if DEBUG
			string space = "";
			for (int i = 0; i < Math.Min(anchor, s_maxIndent) - 1; i++) space += "↓";
			Console.WriteLine(space + "[" + anchor.ToString() + "]" + msg);
			SetAnchor(anchor);
#endif
		}

		/// <summary>
		/// 指定文字列と共に、指定アンカーからの経過時間を出力し、アンカーに関連付けて次の計測を開始します。
		/// </summary>
		/// <param name="msg">出力文字列</param>
		/// <param name="anchor">関連付けるアンカー</param>
		/// <param name="from">経過時間の計測基準となるアンカー</param>
		public static void Write(string msg, int anchor, int from)
		{
#if DEBUG
			double tmp;
			Write(msg, anchor, from, out tmp);
#endif
		}

		/// <summary>
		/// 指定文字列と共に、指定アンカーからの経過時間を出力し、アンカーに関連付けて次の計測を開始します。
		/// </summary>
		/// <param name="msg">出力文字列</param>
		/// <param name="anchor">関連付けるアンカー</param>
		/// <param name="from">経過時間の計測基準となるアンカー</param>
		/// <param name="sec">経過時間[sec]</param>
		public static void Write(string msg, int anchor, int from, out double sec)
		{
			sec = 0d;
#if DEBUG
			string space = "";
			for (int i = 0; i < Math.Min(anchor, s_maxIndent) - 1; i++) space += "↓";
			Console.Write(space + "└[" + anchor.ToString() + "]" + msg);

			Stopwatch sw = s_mapStopWatch[from];
			sec = (double)sw.ElapsedTicks / (double)Stopwatch.Frequency;
			Console.WriteLine(string.Format(" {0}から{1}sec", from.ToString(), sec.ToString()));

			SetAnchor(anchor);
#endif
		}

		/// <summary>
		/// 指定アンカーからの経過時間を出力します。
		/// </summary>
		/// <param name="from">経過時間の計測基準となるアンカー</param>
		public static void Write(int from)
		{
#if DEBUG
			double tmp;
			Write(from, out tmp);
#endif
		}

		/// <summary>
		/// 指定アンカーからの経過時間を出力します。
		/// </summary>
		/// <param name="from">経過時間の計測基準となるアンカー</param>
		/// <param name="sec">経過時間[sec]</param>
		public static void Write(int from, out double sec)
		{
			sec = 0d;
#if DEBUG
			string space = "";
			for (int i = 0; i < Math.Min(from, s_maxIndent) - 1; i++) space += "↓";
			Console.Write(space + "└[" + from.ToString() + "]");

			Stopwatch sw = s_mapStopWatch[from];
			sec = (double)sw.ElapsedTicks / (double)Stopwatch.Frequency;
			Console.WriteLine(string.Format(" {0}から{1}sec", from.ToString(), sec.ToString()));
#endif
		}

		/// <summary>
		/// 全ての時間計測を終了します。
		/// </summary>
		public static void StopAll()
		{
#if DEBUG
			foreach (KeyValuePair<int, Stopwatch> pair in s_mapStopWatch)
				pair.Value.Stop();
#endif
		}

	}
}
