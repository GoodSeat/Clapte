using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace GoodSeat.Clapte.Solvers.Processes
{
	/// <summary>
	/// 文字タイプを表します。
	/// </summary>
	public enum CharaType
	{
		/// <summary>自動</summary>
		Auto,
		/// <summary>全角</summary>
		All = VbStrConv.Wide,
		/// <summary>半角</summary>
		Half = VbStrConv.Narrow
	}

	/// <summary>
	/// 出力文字列の文字タイプの判定と変換処理を表します。
	/// </summary>
	public class OutputTextByteFormatProcess : Process
	{
		/// <summary>
		/// 出力文字列の文字タイプの判定と変換処理を初期化します。
		/// </summary>
		/// <param name="owner">処理の保持者となるソルバ。</param>
		public OutputTextByteFormatProcess(Solver owner) : base(owner) { }

		/// <summary>
		/// 出力文字列の文字タイプの判定と変換処理を初期化します。
		/// </summary>
		/// <param name="owner">処理の保持者となるソルバ。</param>
		/// <param name="type">出力の文字タイプ。</param>
		public OutputTextByteFormatProcess(Solver owner, CharaType type)
			: base(owner) 
		{
			OutputCharaType = type;
		}

		/// <summary>
		/// 出力時の文字タイプを設定もしくは取得します。
		/// </summary>
		public CharaType OutputCharaType { get; set; }

		/// <summary>
		/// 現在処理中の計算に対する文字タイプを設定もしくは取得します。
		/// </summary>
		private CharaType CurrentCharaType { get; set; }

		/// <summary>
		/// 対象文字列に、全角と半角のどちらがより多く含まれるかを取得します。
		/// </summary>
		/// <param name="s">判定対象の文字列</param>
		/// <returns>文字列により多く含まれる文字タイプ。</returns>
		private CharaType GetMajorCharaType(string s)
		{
			int baseLength = s.Length;

			string halfed = Strings.StrConv(s, (VbStrConv)CharaType.Half, 0);
			int convertedCount = 0;
			for (int i = 0; i < s.Length; i++) if (halfed[i] != s[i]) convertedCount++;

			if (convertedCount > baseLength / 2) return CharaType.All;
			else return CharaType.Half;
		}

		/// <summary>
		/// 指定文字列を半角文字列、もしくは全角文字列で置換します。
		/// </summary>
		/// <param name="type">変換先の文字タイプ。</param>
		/// <param name="target">変換対象の文字列。</param>
		/// <returns>文字タイプを統一した文字列。</returns>
		private string ReplaceCharaTypeWith(CharaType type, string target)
		{
			return Strings.StrConv(target, (VbStrConv)type, 0);
		}

		public override Error CheckInputText(ref string input)
		{
			if (OutputCharaType == CharaType.Auto)
				CurrentCharaType = GetMajorCharaType(input);
			else
				CurrentCharaType = OutputCharaType;

			return base.CheckInputText(ref input);
		}

		public override Error CheckOutputText(ref string output) 
		{
			output = ReplaceCharaTypeWith(CurrentCharaType, output);
			return base.CheckOutputText(ref output);
		}
	}
}
