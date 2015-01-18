using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace GoodSeat.Clapte.Solvers.Processes
{
	/// <summary>
	/// 代替可能な文字列に対する文字列置換処理を表します。
	/// </summary>
	public class ReplaceAlternateStringProcess : Process
	{
		static ReplaceAlternateStringProcess()
		{
			IgnoreTextList = new List<string>();
			IgnoreTextList.Add("　");
			IgnoreTextList.Add("\n");
			IgnoreTextList.Add("\r");
			IgnoreTextList.Add("\t");
		}

		/// <summary>
		/// 代替可能な文字列に対する文字列置換処理を初期化します。
		/// </summary>
		/// <param name="owner">処理の保持者となるソルバ。</param>
		public ReplaceAlternateStringProcess(Solver owner) : base(owner) { }

		/// <summary>
		/// 計算時、無視する文字列一覧を設定もしくは取得します。
		/// </summary>
		public static List<string> IgnoreTextList { get; set; }

		/// <summary>
		/// 計算対象となった入力数式を対象として、処理を行います。
		/// </summary>
		/// <param name="input">処理対象の入力数式。</param>
		/// <returns>エラー情報。エラーのない場合、null。</returns>
		public override Error CheckInputText(ref string input) 
		{
			foreach (string s in IgnoreTextList) input = input.Replace(s, " ");

			input = input.Replace("１", "1");
			input = input.Replace("２", "2");
			input = input.Replace("３", "3");
			input = input.Replace("４", "4");
			input = input.Replace("５", "5");
			input = input.Replace("６", "6");
			input = input.Replace("７", "7");
			input = input.Replace("８", "8");
			input = input.Replace("９", "9");
			input = input.Replace("０", "0");
			input = input.Replace("＋", "+");
			input = input.Replace("－", "-");
			input = input.Replace("×", "*");
			input = input.Replace("・", "*");
			input = input.Replace("･", "*");
			input = input.Replace("／", "/");
			input = input.Replace("÷", "/");
			input = input.Replace("＾", "^");
			input = input.Replace("（", "(");
			input = input.Replace("）", ")");
			input = input.Replace("｛", "{");
			input = input.Replace("｝", "}");
			input = input.Replace("［", "[");
			input = input.Replace("］", "]");
			input = input.Replace("＜", "<");
			input = input.Replace("＞", ">");
			input = input.Replace("＝", "=");

			input = input.TrimEnd(' ', '=');
			input = input.TrimStart(' ');

			List<char> listChar = new List<char>();
			for (int i = 0; i < input.Length; i++) listChar.Add(input[i]);

			input = Strings.StrConv(input, VbStrConv.Narrow, 0);

			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == '?' && listChar[i] != '?' && listChar[i] != '？')
				{
					listChar.RemoveAt(i);
					input = input.Substring(0, i) + input.Substring(i + 1);
				}
			}

			return null;
		}
	}
}
