using GoodSeat.Clapte.Models;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.ViewModels.InputSupports;
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
        /// <param name="inputTextBox">ビューにおける入力テキストボックス。</param>
        /// <param name="inputSupportPositionOffset">入力補助の表示位置の補正量。</param>
        public ClaptePadViewModel(Views.Forms.FormOfClaptePad view, AzukiControl inputTextBox, Point inputSupportPositionOffset)
        {
            InputTextBox = inputTextBox;
            InputTextBox.VScroll += InputTextBox_VScroll;
            InputTextBox.CaretMoved += InputTextBox_CaretMoved;
            InputTextBox.SetKeyBind(Keys.Control | Keys.Enter, i => InputSupport.ShowInputSupport(true));
            InputTextBox.SetKeyBind(Keys.Control | Keys.H, i => ArgumentHelper.ShowArgumentHelp());
            InputTextBox.SetKeyBind(Keys.Alt | Keys.Up, i => MoveUpOrDownSelectedLine(true));
            InputTextBox.SetKeyBind(Keys.Alt | Keys.Down, i => MoveUpOrDownSelectedLine(false));

            Target = view.Target;

            InputSupportEnumerator = new ClaptePadInputSupportEnumerator(Target);
            InputSupport = new InputSupport(InputTextBox, view, InputSupportEnumerator);
            InputSupport.ModifyLocation = inputSupportPositionOffset;

            ArgumentHelper = new FunctionArgumentHelp(InputTextBox, view, InputSupportEnumerator);
            ArgumentHelper.ModifyLocation = inputSupportPositionOffset;

            AdditionalInformations = new List<Tuple<FormulaCellContent.AdditionalInformationType, string>>();

            _greekLetterModable = new GreekLetterModable(
                () => InputTextBox.IsReadOnly
                , b => InputTextBox.IsReadOnly = b
                , t => InputTextBox.Document.Replace(t)
                , InputTextBox);
            InputTextBox.CaretMoved += _greekLetterModable.CaretMoved;
        }

        GreekLetterModable _greekLetterModable;


        #region プロパティ

        /// <summary>
        /// 対象とする入力テキストボックスを設定若しくは取得します。
        /// </summary>
        AzukiControl InputTextBox { get; set; }

        /// <summary>
        /// 入力補助オブジェクトを設定もしくは取得します。
        /// </summary>
        public InputSupport InputSupport { get; set; }

        /// <summary>
        /// 入力補助の候補列挙オブジェクトを設定もしくは取得します。
        /// </summary>
        public ClaptePadInputSupportEnumerator InputSupportEnumerator { get; set; }

        /// <summary>.
        /// 引数ヘルプオブジェクトを設定もしくは取得します。
        /// </summary>.
        public FunctionArgumentHelp ArgumentHelper { get; set; }


        /// <summary>
        /// 入力の際、自動で入力補助を表示するか否かを設定もしくは取得します。
        /// </summary>
        public bool AutoShowInputSupport
        {
            get { return InputSupport.AutoShow; }
            set { InputSupport.AutoShow = value; }
        }

        /// <summary>
        /// キャレット移動時、必要に応じて自動で関数の引数ヘルプを表示するか否かを設定もしくは取得します。
        /// </summary>
        public bool AutoShowArgumentHelp
        {
            get { return ArgumentHelper.AutoShow; }
            set { ArgumentHelper.AutoShow = value; }
        }

        /// <summary>
        /// 説明文も補完対象として使用するか否かを設定もしくは取得します。
        /// </summary>
        public bool InputSupportWithAlsoInfomation
        {
            get { return InputSupportEnumerator.AlsoInfomation; }
            set { InputSupportEnumerator.AlsoInfomation = value; }
        }

        /// <summary>
        /// 対象の数式セルリストビューモデルを取得します。
        /// </summary>
        public FormulaCellListViewModel Target { get; private set; }

        /// <summary>
        /// 現在の入力ボックス各行に関連付けられた付加情報リストを設定もしくは取得します。
        /// </summary>
        public List<Tuple<FormulaCellContent.AdditionalInformationType, string>> AdditionalInformations { get; private set; }

        #endregion


        /// <summary>
        /// 指定の文字列が継続行を表すか否かを判定します。
        /// </summary>
        /// <param name="line">判定対象とする行。</param>
        /// <returns>継続行か否か。</returns>
        public bool IsContinueLine(string line)
        {
            return line.Split('#')[0].Trim().EndsWith(" _");
        }

        /// <summary>
        /// スクロール位置に変更がないように、入力ボックスのテキストを指定文字列に変更します。
        /// </summary>
        /// <param name="text">変更後のテキスト。</param>
        public void SetTextWithoutChangeScroll(string text)
        {
            int visible1stLine = InputTextBox.FirstVisibleLine;
            InputTextBox.Text = text;
            InputTextBox.FirstVisibleLine = visible1stLine;
        }

        /// <summary>
        /// 現在のキャレット上の行で指定されている換算目標単位を取得します。換算目標単位の指定がない場合には、nullを返します。
        /// </summary>
        /// <returns></returns>
        public string DetectTargetUnitOnCaretLine()
        {
            int begin, end;
            InputTextBox.GetSelection(out begin, out end);

            int lineIndex = InputTextBox.Document.GetLineIndexFromCharIndex(begin);

            string text = InputTextBox.Document.GetLineContent(lineIndex);

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
            InputTextBox.GetSelection(out begin, out end);

            int lineIndex = InputTextBox.Document.GetLineIndexFromCharIndex(begin);

            string text = InputTextBox.Document.GetLineContent(lineIndex);

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

            var texts = new List<string>(InputTextBox.Text.Split('\n').Select(s => s.Replace("\r", "")));
            texts[lineIndex] = newText;

            SetTextWithoutChangeScroll(string.Join("\r\n", texts));

            int caretIndex = InputTextBox.Document.GetCharIndexFromLineColumnIndex(lineIndex, 0);
            InputTextBox.SetSelection(caretIndex, caretIndex);
        }

        /// <summary>
        /// 現在のキャレット位置、もしくは選択範囲に基づいて、その定義元にジャンプします。
        /// </summary>
        /// <param name="azuki">対象のAzukiコントロール。</param>
        /// <returns>ジャンプ先とする数式。対象の数式が見つからなかった場合や、既にテキストボックス内の定義にジャンプ済みの場合にはnull。</returns>
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

            var doc = azuki.Document;
            doc.SetCaretIndex(jump, 0);
            azuki.ScrollToCaret();

            var pos = doc.FindNext(targetText, doc.GetLineHeadIndex(jump), doc.GetLineHeadIndex(jump) + doc.GetLineLength(jump, false));
            if (pos != null) doc.SetSelection(pos.Begin, pos.End);

            return null;
        }

        /// <summary>
        /// 選択されているすべての行についてテキストの変換を行います。
        /// </summary>
        /// <param name="convert">元の文字列と、その行が選択行のうちの最終行か否かを受け取り、文字列を変換する処理。</param>
        public void EditSelectedLines(Func<string, bool, string> convert)
        {
            int caretIndex = InputTextBox.CaretIndex;

            int sline, eline;
            InputTextBox.GetSelectedLineIndex(out sline, out eline);

            var texts = new List<string>(InputTextBox.Text.Split('\n').Select(s => s.Replace("\r", "")));
            for (int l = sline; l < eline; ++l) texts[l] = convert(texts[l], false);
            texts[eline] = convert(texts[eline], true);

            SetTextWithoutChangeScroll(string.Join("\r\n", texts));

            caretIndex = Math.Min(caretIndex, InputTextBox.Document.Text.Length - 1);
            InputTextBox.SetSelection(caretIndex, caretIndex);
        }

        /// <summary>
        /// 選択されているすべての行についてテキストの変換を行います。
        /// </summary>
        /// <param name="convert">元の文字列を受け取り、文字列を変換する処理。</param>
        public void EditAllLines(Func<string, string> convert)
        {
            int caretIndex = InputTextBox.CaretIndex;

            var texts = new List<string>(InputTextBox.Text.Split('\n').Select(s => s.Replace("\r", "")).Select(convert));

            SetTextWithoutChangeScroll(string.Join("\r\n", texts));

            caretIndex = Math.Min(caretIndex, InputTextBox.Document.Text.Length - 1);
            InputTextBox.SetSelection(caretIndex, caretIndex);
        }

        /// <summary>
        /// 現在の選択範囲を、指定名の定数/関数で置き換えて、定数/関数の宣言を前の行に挿入します。
        /// </summary>
        /// <param name="name">定数/関数名。</param>
        /// <returns>処理に失敗した場合におけるエラー情報。処理に成功した場合、null。</returns>
        public string InsertDefineWithName(string name)
        {
            var solver = Target.BaseSolver.Target;

            Formula f;
            Predicate<Formula> isOk = f_ => !(f_ is Liffom.Formulas.Operators.Sum)
                                         && !(f_ is Liffom.Formulas.Operators.Power)
                                         && !(f_ is Numeric);
            if (!solver.TryParse(name, out f) || !isOk(f)) return "\"" + name + "\"は定数/関数名として使用できません。";

            string def = InputTextBox.GetSelectedText().Trim();
            if (!solver.TryParse(def, out f)) return "\"" + def + "\"は数式として認識できません。";

            InputTextBox.Document.Replace(name);

            string defLine = name + " = " + def;

            { // 優先解決行の考慮
                int sline, eline;
                InputTextBox.GetSelectedLineIndex(out sline, out eline);

                var l = InputTextBox.Document.GetLineContent(sline);
                var regex = new Regex("^\\s*(\\|\\s*)+");
                var match = regex.Match(l);
                if (match.Success) defLine = match.Value + defLine;
            }

            InsertLine(defLine);
            return null;
        }

        /// <summary>
        /// 継続行を考慮して、選択行の一行上あるいは下に指定行を追加します。
        /// </summary>
        /// <param name="text">追加する文字列。</param>
        /// <param name="insertUp">一行上に挿入する場合はtrue、下に挿入する場合はfalse。</param>
        public void InsertLine(string text, bool insertUp = true)
        {
            var lines = new List<string>(InputTextBox.Text.Split('\n'));

            int sline, eline;
            InputTextBox.GetSelectedLineIndex(out sline, out eline);

            while (sline > 1 && IsContinueLine(lines[sline - 1])) --sline;
            while (eline < lines.Count && IsContinueLine(lines[eline])) ++eline;

            lines.Insert(insertUp ? sline : eline + 1, text);

            int begin, end;
            InputTextBox.Document.GetSelection(out begin, out end);
            SetTextWithoutChangeScroll(string.Join("\n", lines));
            int addSelectionPosition = insertUp ? text.Length + 1 : 0;
            InputTextBox.Document.SetSelection(begin + addSelectionPosition, end + addSelectionPosition);
        }

        /// <summary>
        /// 選択されている全ての行を、継続行を考慮して1行上に移動します。
        /// </summary>
        /// <param name="up">選択行を上げるならtrue、下げるならfalseを指定。</param>
        public void MoveUpOrDownSelectedLine(bool up)
        {
            var lines = new List<string>(InputTextBox.Text.Split('\n'));

            int sline, eline;
            InputTextBox.GetSelectedLineIndex(out sline, out eline);

            while (sline > 1 && IsContinueLine(lines[sline - 1])) --sline;
            while (eline < lines.Count && IsContinueLine(lines[eline])) ++eline;
            if (up && sline <= 0) return;
            if (!up && eline >= lines.Count - 1) return;

            int moveTo;
            if (up)
            {
                moveTo = sline - 1;
                while (moveTo > 1 && IsContinueLine(lines[moveTo - 1])) --moveTo;
            }
            else
            {
                moveTo = eline + 1;
                while (moveTo < lines.Count && IsContinueLine(lines[moveTo])) ++moveTo;
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
            InputTextBox.Document.GetSelection(out bs, out es);
            SetTextWithoutChangeScroll(string.Join("\n", lines));

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
            InputTextBox.Document.SetSelection(bs, es); 
        }

        /// <summary>
        /// 数式の変形関数を受け取り、変形結果をキャレットの次の行に挿入します。
        /// </summary>
        /// <param name="deform">適用する数式の変形処理。</param>
        public void DeformFormula(Func<Formula, Formula> deform)
        {
            int caretIndex = InputTextBox.CaretIndex;
            int begin, end;
            InputTextBox.GetSelection(out begin, out end);
            int lineIndex = InputTextBox.Document.GetLineIndexFromCharIndex(begin);

            string result = "# 数式処理に失敗しました。";
            try
            {
                // MEMO:現状、単位の自動認識はされない([]で囲ったやつだけが単位として認識される)
                var f1 = FormulaOnCaret();
                var f2 = deform(f1);
                f2.Format = Target.BaseSolver.Target.OutputFormat;
                result = f2.ToString();
            }
            catch (Exception e) { result += e.Message; }

            InsertLine(result, false);

            InputTextBox.SetSelection(caretIndex, caretIndex);
        }

        /// <summary>
        /// 現在のキャレット上にある数式を取得します。
        /// </summary>
        /// <returns>キャレット上にある数式。見つからなかった場合、null。</returns>
        public Formula FormulaOnCaret()
        {
            int begin, end;
            InputTextBox.GetSelection(out begin, out end);

            int lineIndex = InputTextBox.Document.GetLineIndexFromCharIndex(begin);
            var line = Target.ElementAt(lineIndex);

            // MEMO:現状、単位の自動認識はされない([]で囲ったやつだけが単位として認識される)
            return Target.BaseSolver.Target.Parse(line.Target.Content.FormulaText);
        }

        /// <summary>
        /// 指定数式変形履歴を表すツリーノードを生成して取得します。
        /// </summary>
        /// <param name="history">対象とする数式変形履歴。</param>
        /// <param name="colorOfRule">ルール名に付与するカラー。</param>
        /// <param name="colorOfError">エラーの説明に付与するカラー。</param>
        /// <param name="contextMenuOfNode">数式のノードに付与するコンテキストメニュー。</param>
        /// <returns>指定数式変形履歴を表すツリーノード。</returns>
        public IEnumerable<TreeNode> CreateTreeNodeOfDeformHistories(DeformHistory history, Color colorOfRule, Color colorOfError, ContextMenuStrip contextMenuOfNode)
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
                    treeNodeApplied.ForeColor = colorOfRule;
                    yield return treeNodeApplied;
                }

                var treeNode = new TreeNode(toString(historyNode.Formula));
                if (historyNode.AppliedRule == null) treeNode.Tag = history;
                else treeNode.Tag = historyNode;

                if (historyNode.Formula is ErrorFormula) treeNode.ForeColor = colorOfError;

                treeNode.ContextMenuStrip = contextMenuOfNode;

                bool anyHasHistory = false;
                foreach (var historyChild in historyNode.ChildrenHistories)
                {
                    if (historyChild.Count() < 2) continue;
                    anyHasHistory = true;

                    var text = "└ " + toString(historyChild.First().Formula) + " → " + toString(historyChild.Last().Formula);

                    var treeNodeChild = new TreeNode(text);
                    treeNodeChild.ForeColor = Color.Gray;
                    foreach (var n in CreateTreeNodeOfDeformHistories(historyChild, colorOfRule, colorOfError, contextMenuOfNode))
                    {
                        treeNodeChild.Nodes.Add(n);
                    }
                    treeNodeChild.Tag = historyChild;
                    treeNodeChild.ContextMenuStrip = contextMenuOfNode;
                    treeNode.Nodes.Add(treeNodeChild);
                }
                if (!anyHasHistory) treeNode.Nodes.Clear();

                yield return treeNode;
            }
        }


        /// <summary>
        /// 現在のマウス位置において、表示すべきヘルプメッセージを取得します。
        /// </summary>
        /// <param name="textBox">判定対象とするテキストボックス。</param>
        /// <returns>表示すべきヘルプメッセージ。ない場合にはnull。</returns>
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
                helpText = AdditionalInformations[lineIndex]?.Item2;
            }
            return helpText;
        }


        #region イベント対応

        private void InputTextBox_VScroll(object sender, EventArgs e)
        {
            ArgumentHelper.Hide();
            InputSupport.EscapeInputSupport();
        }

        private void InputTextBox_CaretMoved(object sender, EventArgs e)
        {
            int line, column;
            InputTextBox.Document.GetCaretIndex(out line, out column);
            InputSupportEnumerator.CurrentCaretLineNumber = line;
        }

        #endregion

    }
}
