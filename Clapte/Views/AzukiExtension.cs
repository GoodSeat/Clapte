using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sgry.Azuki.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace GoodSeat.Clapte.Views
{
    /// <summary>
    /// AzukiControlの拡張メソッドを提供します。
    /// </summary>
    public static class AzukiExtension
    {
        static AzukiExtension() { ResetRegex(); }

        static Regex FindPreCursorWordRegex { get; set; }
        static Regex FindCursorWordRegex { get; set; }
        static Regex FindCaretWordRegex { get; set; }

        static bool s_splitWithfollowNumber;

        /// <summary>
        /// 非数値文字列の後方に続く数値文字列を、区切り位置として扱うか否かを設定若しくは取得します。
        /// </summary>
        public static bool SplitWithFollowNumber
        {
            get { return s_splitWithfollowNumber; }
            set
            {
                if (s_splitWithfollowNumber == value) return;
                s_splitWithfollowNumber = value;

                ResetRegex();
            }
        }

        /// <summary>
        /// 現在の設定に基づいて、<see cref="AzukiExtension"/>で用いる正規表現を初期化します。
        /// </summary>
        private static void ResetRegex()
        {
            if (SplitWithFollowNumber)
            {
                FindPreCursorWordRegex = new Regex(@"^(?<pre>.*\b\d*)(?<target>\D\S*?)(\d|\b)");
                FindCursorWordRegex = new Regex(@"^(?<target>\D\S*?)(\d|\b)");
                FindCaretWordRegex = new Regex(@"^(?<pre>.*\b\d*)(?<target>\D\S*)$");
            }
            else
            {
                FindPreCursorWordRegex = new Regex(@"^(?<pre>.*\b\d*)(?<target>\D\S*?)\b");
                FindCursorWordRegex = new Regex(@"^(?<target>\D\S*?)\b");
                FindCaretWordRegex = new Regex(@"^(?<pre>.*\b\d*)(?<target>\D\S*)$");
            }
        }


        /// <summary>
        /// マウスカーソルが指し示す位置の文字インデックスを取得します。
        /// </summary>
        /// <param name="azuki">対象のAzukiControl。</param>
        /// <returns>マウスカーソル位置の文字インデックス。</returns>
        public static int? GetMouseHoverIndex(this AzukiControl azuki)
        {
            Point position = azuki.PointToClient(Cursor.Position);
            int index = azuki.GetIndexFromPosition(position);
            Point checkPosition = azuki.GetPositionFromIndex(index);
            if (Math.Abs(position.X - checkPosition.X) > 20 || Math.Abs(position.Y - checkPosition.Y) > 20) return null;
            return index;
        }


        /// <summary>
        /// マウスカーソルが指し示す位置に存在する文字を取得します。
        /// </summary>
        /// <param name="azuki">対象のAzukiControl。</param>
        /// <returns>マウスカーソル位置の文字。</returns>
        public static char? GetMouseHoverChar(this AzukiControl azuki)
        {
            int? index = GetMouseHoverIndex(azuki);
            if (!index.HasValue) return null;
            return azuki.Document.GetCharAt(index.Value);
        }

        /// <summary>
        /// マウスカーソルが指し示す位置に存在するマークIDを取得します。
        /// </summary>
        /// <param name="azuki">対象のAzukiControl。</param>
        /// <returns>マウスカーソル位置の文字。</returns>
        public static int? GetMouseHoverMarkID(this AzukiControl azuki)
        {
            int? index = GetMouseHoverIndex(azuki);
            if (!index.HasValue) return null;

            var ids = azuki.Document.GetMarkingsAt(index.Value);
            return (ids.Length == 0) ? (int?)null : ids.First();
        }

        /// <summary>
        /// マウスカーソルが指し示す位置に存在する単語を取得します。
        /// </summary>
        /// <param name="azuki">対象のAzukiControl。</param>
        /// <param name="lineIndex">取得された単語の所属行番号。</param>
        /// <param name="postText">見つかった単語と同じ行の後方の文字列。</param>
        /// <returns>マウスカーソル位置の単語。</returns>
        public static string GetMouseHoverWord(this AzukiControl azuki, out int lineIndex, out string postText)
        {
            lineIndex = 0;
            postText = "";

            Point position = azuki.PointToClient(Cursor.Position);
            position.X += 8; // カーソル前の単語を取ってくるので、カーソル位置を本来より少し右側に詐称しないと取得される単語が不自然。
            int index = azuki.GetIndexFromPosition(position);
            Point checkPosition = azuki.GetPositionFromIndex(index);
            if (Math.Abs(position.X - checkPosition.X) > 20 || Math.Abs(position.Y - checkPosition.Y) > 20) return null;

            return azuki.GetWordFromIndex(index, out lineIndex, out postText);
        }

        /// <summary>
        /// キャレット位置に存在する単語を取得します。
        /// </summary>
        /// <param name="azuki">対象のAzukiControl。</param>
        /// <param name="lineIndex">取得された単語の所属行番号。</param>
        /// <param name="postText">見つかった単語と同じ行の後方の文字列。</param>
        /// <returns>キャレット位置の単語。</returns>
        public static string GetCaretWord(this AzukiControl azuki, out int lineIndex, out string postText)
        {
            return azuki.GetWordFromIndex(azuki.CaretIndex, out lineIndex, out postText);
        }

        /// <summary>
        /// 指定インデックス位置に存在する単語を取得します。
        /// </summary>
        /// <param name="azuki">対象のAzukiControl。</param>
        /// <param name="index">インデックス。</param>
        /// <param name="lineIndex">取得された単語の所属行番号。</param>
        /// <param name="postText">見つかった単語と同じ行の後方の文字列。</param>
        /// <returns>指定インデックス位置の単語。</returns>
        public static string GetWordFromIndex(this AzukiControl azuki, int index, out int lineIndex, out string postText)
        {
            postText = "";

            int columnIndex;
            azuki.Document.GetLineColumnIndexFromCharIndex(index, out lineIndex, out columnIndex);

            string lineContent = azuki.Document.GetLineContent(lineIndex);
            string preFindText = lineContent.Substring(0, columnIndex);
            var match = FindPreCursorWordRegex.Match(preFindText);
            if (!match.Success) return null;

            string findText = lineContent.Substring(match.Groups["pre"].Length);
            match = FindCursorWordRegex.Match(findText);
            if (!match.Success) return null;

            string targetText = match.Groups["target"].Value;
            postText = findText.Substring(targetText.Length);

            return targetText;
        }

        /// <summary>
        /// キャレット位置から、前方の単語区切りまでの単語を取得します。
        /// </summary>
        /// <param name="azuki">対象のAzukiControl。</param>
        /// <param name="startIndex">取得された単語の最初の文字位置インデックス。</param>
        public static string GetPreCaretWord(this AzukiControl azuki, out int startIndex)
        {
            int caretRowIndex, caretColumnIndex;
            azuki.Document.GetCaretIndex(out caretRowIndex, out caretColumnIndex);

            return GetPreWordOf(azuki, caretRowIndex, caretColumnIndex, out startIndex);
        }

        /// <summary>
        /// 指定位置から、前方の単語区切りまでの単語を取得します。
        /// </summary>
        /// <param name="azuki">対象のAzukiControl。</param>
        /// <param name="rowIndex">取得対象位置の行番号。</param>
        /// <param name="columnIndex">取得対象位置の列番号。</param>
        /// <param name="startIndex">取得された単語の最初の文字位置インデックス。</param>
        public static string GetPreWordOf(this AzukiControl azuki, int rowIndex, int columnIndex, out int startIndex)
        {
            startIndex = 0;

            string lineContent = azuki.Document.GetLineContent(rowIndex);
            string targetText = lineContent.Substring(0, columnIndex);

            var match = FindCaretWordRegex.Match(targetText);
            if (!match.Success) return null;

            var pre = match.Groups["pre"].Value;
            var target = match.Groups["target"].Value;

            startIndex = azuki.Document.GetCharIndexFromLineColumnIndex(rowIndex, pre.Length);
            return target;
        }

        /// <summary>
        /// キャレット位置が対応する関数名とその引数の順序を取得します。
        /// </summary>
        /// <param name="azuki">対象のAzukiControl。</param>
        /// <param name="startIndex">取得された単語の最初の文字位置インデックス。</param>
        /// <param name="argIndex">0から始まる引数の順序インデックス。</param>
        public static string GetCurrentFunctionName(this AzukiControl azuki, out int startIndex, out int argIndex)
        {
            argIndex = 0;
            startIndex = 0;

            int caretRowIndex, caretColumnIndex;
            azuki.Document.GetCaretIndex(out caretRowIndex, out caretColumnIndex);

            string lineContent = azuki.Document.GetLineContent(caretRowIndex);

            string target = lineContent.Substring(0, caretColumnIndex);
            int count = 0;
            int startBracketIndex = 0;
            for (startBracketIndex = target.Length - 1; startBracketIndex >= 0; startBracketIndex--)
            {
                if (target[startBracketIndex] == ')') count--;
                else if (target[startBracketIndex] == '(') count++;
                else if (count == 0 && target[startBracketIndex] == ',') argIndex++;
                
                if (count == 1) break;
            }
            if (startBracketIndex <= 1) return null;

            string name = GetPreWordOf(azuki, caretRowIndex, startBracketIndex, out startIndex);
            return name;
        }
    }
}
