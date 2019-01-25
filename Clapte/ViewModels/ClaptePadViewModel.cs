using GoodSeat.Clapte.Models;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.Views;
using GoodSeat.Clapte.Views.InputSupports;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Units;
using Sgry.Azuki.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GoodSeat.Clapte.ViewModels
{
    /// <summary>
    /// ClaptePadのエディタ機能に係るビューモデルを表します。
    /// </summary>
    public class ClaptePadViewModel
    {

        /// <summary>
        /// ClaptePadのエディタ機能に係るビューモデルを初期化します。
        /// </summary>
        /// <param name="view">ClaptePadのビュー。</param>
        /// <param name="azuki">ビューにおける入力テキストボックス。</param>
        public ClaptePadViewModel(Views.Forms.FormOfClaptePad view, AzukiControl azuki)
        {
            _inputTextBox = azuki;
            _view = view;
        }

        Views.Forms.FormOfClaptePad _view; // TODO!:可能であれば消したい依存
        AzukiControl _inputTextBox;

        ClaptePadInputSupportEnumerator InputSupportEnumerator
        {
            get { return _view.InputSupportEnumerator; }
        }

        /// <summary>
        /// 対象の数式セルリストビューモデルを取得します。
        /// </summary>
        public FormulaCellListViewModel Target { get { return _view.Target; } }


        /// <summary>
        /// 現在のキャレット上の行で指定されている換算目標単位を取得します。換算目標単位の指定がない場合には、nullを返します。
        /// </summary>
        /// <returns></returns>
        public string DetectTargetUnitOnCaretLine()
        {
            int begin, end;
            _inputTextBox.GetSelection(out begin, out end);

            int lineIndex = _inputTextBox.Document.GetLineIndexFromCharIndex(begin);

            string text = _inputTextBox.Document.GetLineContent(lineIndex);

            var targetUnitRegex = new Regex(@"^\s*\[(?<targetUnit>[^[\]]+)\]");

            var match = targetUnitRegex.Match(text);
            return match.Success ? match.Groups["targetUnit"].Value : null;
        }

        /// <summary>
        /// 現在のキャレット上の換算目標単位を指定します。nullや空文字が指定された場合、目標単位を削除します。
        /// </summary>
        /// <param name="targetUnit"></param>
        public void SetTargetUnitOnCaretLine(string targetUnit)
        {
            int begin, end;
            _inputTextBox.GetSelection(out begin, out end);

            int lineIndex = _inputTextBox.Document.GetLineIndexFromCharIndex(begin);

            string text = _inputTextBox.Document.GetLineContent(lineIndex);

            var targetUnitRegex = new Regex(@"^(?<indent>\s*)\[(?<targetUnit>[^[\]]*)\]\s*");
            var match = targetUnitRegex.Match(text);
            var newText = text;
            if (match.Success)
            {
                if (string.IsNullOrWhiteSpace(targetUnit))
                {
                    newText = Regex.Replace(text, targetUnitRegex.ToString(), "${indent}");
                }
                else
                {
                    newText = Regex.Replace(text, targetUnitRegex.ToString(), "${indent}" + "[" + targetUnit + "] ");
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(targetUnit)) newText = "[" + targetUnit + "] " + text;
            }
            if (newText == text) return;

            var texts = new List<string>(_inputTextBox.Text.Split('\n').Select(s => s.Replace("\r", "")));
            texts[lineIndex] = newText;

            int visible1stLine = _inputTextBox.FirstVisibleLine;
            _inputTextBox.Text = string.Join("\r\n", texts);
            _inputTextBox.FirstVisibleLine = visible1stLine;

            int caretIndex = _inputTextBox.Document.GetCharIndexFromLineColumnIndex(lineIndex, 0);
            _inputTextBox.SetSelection(caretIndex, caretIndex);
        }


        /// <summary>
        /// 現在のキャレット位置、もしくは選択範囲に基づいて、その定義元にジャンプします。
        /// </summary>
        /// <param name="azuki">対象のAzukiコントロール。</param>
        public Formula JumpDefine(AzukiControl azuki)
        {
            string targetText = azuki.GetSelectedText();
            string postText;
            int lineIndex;
            string txt = azuki.GetCaretWord(out lineIndex, out postText);
            if (string.IsNullOrEmpty(targetText)) targetText = txt;
            if (targetText == null) return null;

            var helpTarget = InputSupportEnumerator.GetInputSupportCandidateFromText(targetText, lineIndex, postText);
            if (helpTarget == null)
            {
                var target = Target.BaseSolver.Target.Parser.Parse(targetText);
                if (target is Variable) target = new Unit(targetText);

                var unit = target as Unit;
                if (unit != null && unit.UnitType != null) return target;

                return null;
            }

            int jump = -1;
            if (helpTarget.Tag is FunctionDefine)
            {
                var def = helpTarget.Tag as FunctionDefine;

                int line = 0;
                foreach (var cell in Target)
                {
                    if (cell.Target.Content.GetAllDefinedFunctionNames().Select(u => u.Item1).Contains(def.Name)) jump = line;
                    if (line++ >= lineIndex) break;
                }
                if (jump == -1) return def.Target;
            }
            else if (helpTarget.Tag is ConstantDefine)
            {
                var def = helpTarget.Tag as ConstantDefine;

                int line = 0;
                foreach (var cell in Target)
                {
                    if (cell.Target.Content.GetAllDefinedVariableNames().Contains(def.Name)) jump = line;
                    if (line++ >= lineIndex) break;
                }
                if (jump == -1) return def.Target;
            }
            else if (helpTarget.Tag is UnitConvertRecord)
            {
                return (helpTarget.Tag as UnitConvertRecord).ConvertUnit;
            }
            if (jump < 0) return null;

            azuki.Document.SetCaretIndex(jump, 0);
            azuki.ScrollToCaret();
            return null;
        }

        /// <summary>
        /// 選択されているすべての行についてテキストの変換を行います。
        /// </summary>
        /// <param name="convert">元の文字列と、その行が選択行のうちの最終行か否かを受け取り、文字列を変換する処理。</param>
        public void EditSelectedLines(Func<string, bool, string> convert)
        {
            int caretIndex = _inputTextBox.CaretIndex;

            int sline, eline;
            _inputTextBox.GetSelectedLineIndex(out sline, out eline);

            var texts = new List<string>(_inputTextBox.Text.Split('\n').Select(s => s.Replace("\r", "")));
            for (int l = sline; l < eline; ++l) texts[l] = convert(texts[l], false);
            texts[eline] = convert(texts[eline], true);

            int visible1stLine = _inputTextBox.FirstVisibleLine;
            _inputTextBox.Text = string.Join("\r\n", texts);
            _inputTextBox.FirstVisibleLine = visible1stLine;

            caretIndex = Math.Min(caretIndex, _inputTextBox.Document.Text.Length - 1);
            _inputTextBox.SetSelection(caretIndex, caretIndex);
        }

        /// <summary>
        /// 選択されている全ての行を、継続行を考慮して1行上に移動します。
        /// </summary>
        /// <param name="up">選択行を上げるならtrue、下げるならfalseを指定。</param>
        public void MoveUpOrDownSelectedLine(bool up)
        {
            var lines = new List<string>(_inputTextBox.Text.Split('\n'));

            int sline, eline;
            _inputTextBox.GetSelectedLineIndex(out sline, out eline);

            Predicate<string> isContinueLine = l => l.Trim().EndsWith(" _");

            while (sline > 1 && isContinueLine(lines[sline - 1])) --sline;
            while (eline < lines.Count && isContinueLine(lines[eline])) ++eline;
            if (up && sline <= 0) return;
            if (!up && eline >= lines.Count - 1) return;

            int moveTo;
            if (up)
            {
                moveTo = sline - 1;
                while (moveTo > 1 && isContinueLine(lines[moveTo - 1])) --moveTo;
            }
            else
            {
                moveTo = eline + 1;
                while (moveTo < lines.Count && isContinueLine(lines[moveTo])) ++moveTo;
            }

            int countOverChar = 0;
            if (up)
            {
                for (int overLine = moveTo; overLine < sline; overLine++) countOverChar += lines[overLine].Length + 1;
            }
            else
            {
                for (int overLine = moveTo; overLine > eline; overLine--) countOverChar += lines[overLine].Length + 1;
                moveTo -= (eline - sline);
            }

            var moveLines = new List<string>();
            for (int l = eline; l >= sline; --l) moveLines.Add(lines[l]);
            lines.RemoveRange(sline, eline - sline + 1);

            foreach (var l in moveLines) lines.Insert(moveTo, l);

            int bs, es;
            _inputTextBox.Document.GetSelection(out bs, out es);

            int visible1stLine = _inputTextBox.FirstVisibleLine;
            _inputTextBox.Text = string.Join("\n", lines);
            _inputTextBox.FirstVisibleLine = visible1stLine;

            if (up)
            {
                bs -= countOverChar;
                es -= countOverChar;
            }
            else
            {
                bs += countOverChar;
                es += countOverChar;
            }
            _inputTextBox.Document.SetSelection(bs, es); 
        }

        /// <summary>
        /// 数式の変形関数を受け取り、変形結果をキャレットの次の行に挿入します。
        /// </summary>
        /// <param name="deform">適用する数式の変形処理。</param>
        public void DeformFormula(Func<Formula, Formula> deform)
        {
            int caretIndex = _inputTextBox.CaretIndex;
            int begin, end;
            _inputTextBox.GetSelection(out begin, out end);
            int lineIndex = _inputTextBox.Document.GetLineIndexFromCharIndex(begin);

            string result = "# 数式処理に失敗しました。";
            try
            {
                // MEMO:現状、単位の自動認識はされない([]で囲ったやつだけが単位として認識される)
                var f1 = FormulaOnCaret();
                var f2 = deform(f1);
                f2.Format = Target.BaseSolver.Target.OutputFormat;
                result = f2.ToString();
            }
            catch (Exception e)
            {
                result += e.Message;
            }

            var texts = new List<string>(_inputTextBox.Text.Split('\n').Select(s => s.Replace("\r", "")));
            texts.Insert(lineIndex + 1, result);

            int visible1stLine = _inputTextBox.FirstVisibleLine;
            _inputTextBox.Text = string.Join("\r\n", texts);
            _inputTextBox.FirstVisibleLine = visible1stLine;

            _inputTextBox.SetSelection(caretIndex, caretIndex);
        }

        /// <summary>
        /// 現在のキャレット上にある数式を取得します。
        /// </summary>
        /// <returns></returns>
        public Formula FormulaOnCaret()
        {
            int begin, end;
            _inputTextBox.GetSelection(out begin, out end);

            int lineIndex = _inputTextBox.Document.GetLineIndexFromCharIndex(begin);
            var line = Target.ElementAt(lineIndex);

            // MEMO:現状、単位の自動認識はされない([]で囲ったやつだけが単位として認識される)
            return Target.BaseSolver.Target.Parse(line.Target.Content.FormulaText);
        }

        /// <summary>
        /// 指定数式変形履歴を表すツリーノードを生成して取得します。
        /// </summary>
        /// <param name="history">対象とする数式変形履歴。</param>
        /// <returns>指定数式変形履歴を表すツリーノード。</returns>
        public IEnumerable<TreeNode> CreateTreeNodeOfDeformHistories(DeformHistory history)
        {
            var format = Target.BaseSolver.Target.OutputFormat;

            Func<Formula, string> toString = f =>
            {
                f.Format = format;
                return f.ToString();
            };

            foreach (var historyNode in history)
            {
                if (historyNode.AppliedRule != null)
                {
                    var treeNodeApplied = new TreeNode(" ↓ " + historyNode.AppliedRule.Information);
                    treeNodeApplied.Tag = historyNode.AppliedRule;
                    treeNodeApplied.ForeColor = _view.GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Comment);
                    yield return treeNodeApplied;
                }

                var treeNode = new TreeNode(toString(historyNode.Formula));
                if (historyNode.AppliedRule == null) treeNode.Tag = history;
                else treeNode.Tag = historyNode;

                if (historyNode.Formula is ErrorFormula) treeNode.ForeColor = _view.GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Error);

//                treeNode.ContextMenuStrip = _contextMenuHistoryNode; // TODO:どうにかする

                bool anyHasHistory = false;
                foreach (var historyChild in historyNode.ChildrenHistories)
                {
                    if (historyChild.Count() < 2) continue;
                    anyHasHistory = true;

                    var text = "└ " + toString(historyChild.First().Formula) + " → " + toString(historyChild.Last().Formula);

                    var treeNodeChild = new TreeNode(text);
                    treeNodeChild.ForeColor = Color.Gray;
                    foreach (var n in CreateTreeNodeOfDeformHistories(historyChild))
                    {
                        treeNodeChild.Nodes.Add(n);
                    }
                    treeNodeChild.Tag = historyChild;
 //                   treeNodeChild.ContextMenuStrip = _contextMenuHistoryNode; // TODO:どうにかする
                    treeNode.Nodes.Add(treeNodeChild);
                }
                if (!anyHasHistory) treeNode.Nodes.Clear();

                yield return treeNode;
            }
        }


        public string HelpMessageOnMouse(AzukiControl textBox)
        {
            int lineIndex;
            string postText;
            string targetText = textBox.GetMouseHoverWord(out lineIndex, out postText);
            char? targetChar = textBox.GetMouseHoverChar();
            int? markID = textBox.GetMouseHoverMarkID();

            string helpText = null;

            int mouseIndex = textBox.GetMouseHoverIndex().GetValueOrDefault(-1);
            var selectedText = textBox.GetSelectedText();
            int begin, end;
            textBox.GetSelection(out begin, out end);
            bool isOnSelected = mouseIndex > begin && mouseIndex < end;
            if (isOnSelected && selectedText.Length > 1)
            {
                selectedText = Regex.Replace(selectedText, "#[^\n]*", "");
                selectedText = Regex.Replace(selectedText, " _ *\r?\n", "");
                int n = lineIndex;
                var line = Target.ElementAt(n);
                while (line.Target.Content.IsContinuation) line = Target.ElementAt(++n);

                var preCells = line.Target.Content.PreDemandEvaluateFormulaCells.Where(c => !c.Content.IsContinuation).ToArray();
                var cell = new FormulaCell(selectedText, Target.BaseSolver.Target, preCells);
                var content = cell.Content;
                if (!(content is FormulaCellContentComment) && content.GetAllDefinedVariableNames().Count() == 0 && content.GetAllDefinedFunctionNames().Count() == 0 && cell.CanEvaluate)
                {
                    if (!cell.Evaluated) cell.Evaluate(Target.BaseSolver.Target);
                    if (content.ResultLevel.GetValueOrDefault(Result.Level.Error) == Result.Level.Success) helpText = content.FormulaText + " = " + content.ResultText;
                }
            }
            if (helpText == null && targetChar.HasValue && targetText != null && targetText.Contains(targetChar.Value))
            {
                var helpTarget = InputSupportEnumerator.GetInputSupportCandidateFromText(targetText, lineIndex, postText);
                helpText = helpTarget?.Information;
            }
            if (helpText == null && markID.HasValue)
            {
                helpText = _view.AdditionalInformations[lineIndex]?.Item2;
            }
            return helpText;
        }




    }
}
