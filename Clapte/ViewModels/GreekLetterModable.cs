using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoodSeat.Clapte.ViewModels
{
    /// <summary>
    /// テキストを入力するコントロールに対して、ギリシャ文字の入力モード機能を付与します。
    /// </summary>
    public class GreekLetterModable
    {
        /// <summary>
        /// テキストを入力するコントロールに対して、ギリシャ文字の入力モード機能を付与します。
        /// </summary>
        /// <param name="isReadOnly">対象のコントロールを読み取り専用か否かを取得する関数。</param>
        /// <param name="setReadOnly">対象のコントロールを読み取り専用とするか否かを設定するアクション。</param>
        /// <param name="insertText">対象のコントロールに指定文字を挿入するアクション。</param>
        /// <param name="target">対象のコントロール。</param>
        public GreekLetterModable(Func<bool> isReadOnly, Action<bool> setReadOnly, Action<string> insertText, Control target)
        {
            IsReadOnly = isReadOnly;
            SetReadOnly = setReadOnly;
            InsertText = insertText;

            target.KeyUp += KeyUp;

            InitializeGreekLettersMap();
        }

        /// <summary>
        /// テキストを入力するコントロールに対して、ギリシャ文字の入力モード機能を付与します。
        /// </summary>
        /// <param name="target">対象のテキストボックス。</param>
        public GreekLetterModable(TextBox target)
        {
            IsReadOnly = () => target.ReadOnly;
            SetReadOnly = b => target.ReadOnly = b;
            InsertText = t => {
                string textOrg = target.Text;
                int selectionStart = target.SelectionStart;
                int selectionLength = target.SelectionLength;
                target.Text = textOrg.Substring(0, selectionStart) + t + textOrg.Substring(selectionStart + selectionLength);
                target.SelectionStart = selectionStart + 1;
            };
            target.KeyUp += KeyUp;

            InitializeGreekLettersMap();
        }

        /// <summary>
        /// 対象のコントロールを読み取り専用か否かを取得する関数を設定もしくは取得します。
        /// </summary>
        Func<bool> IsReadOnly { get; set; }

        /// <summary>
        /// 対象のコントロールを読み取り専用とするか否かを設定するアクションを設定もしくは取得します。
        /// </summary>
        Action<bool> SetReadOnly { get; set; }

        /// <summary>
        /// 対象のコントロールに指定文字を挿入するアクションを設定もしくは取得します。
        /// </summary>
        Action<string> InsertText { get; set; }

        /// <summary>
        /// アルファベットから対応するギリシャ文字を取得する対応マップを初期化します。
        /// </summary>
        Dictionary<Keys, Tuple<string, string>> MapAlphabetToGreek { get; set; }

        bool _greekLettersMode;
        bool _ignoreKeyinGreekLettersMode;

        /// <summary>
        /// ギリシャ文字入力モードか否かを設定若しくは取得します。
        /// </summary>
        bool GreekLettersMode
        {
            get { return _greekLettersMode; }
            set
            {
                _greekLettersMode = value;
                _ignoreKeyinGreekLettersMode = value;

                SetReadOnly(value);
            }
        }

        /// <summary>
        /// アルファベットから対応するギリシャ文字を取得する対応マップを初期化します。
        /// </summary>
        private void InitializeGreekLettersMap()
        {
            MapAlphabetToGreek = new Dictionary<Keys, Tuple<string, string>>();
            MapAlphabetToGreek.Add(Keys.A, Tuple.Create("Α", "α"));
            MapAlphabetToGreek.Add(Keys.B, Tuple.Create("Β", "β"));
            MapAlphabetToGreek.Add(Keys.C, Tuple.Create("Χ", "χ"));
            MapAlphabetToGreek.Add(Keys.D, Tuple.Create("Δ", "δ"));
            MapAlphabetToGreek.Add(Keys.E, Tuple.Create("Ε", "ε"));
            MapAlphabetToGreek.Add(Keys.F, Tuple.Create("Φ", "φ"));
            MapAlphabetToGreek.Add(Keys.G, Tuple.Create("Γ", "γ"));
            MapAlphabetToGreek.Add(Keys.H, Tuple.Create("Η", "η"));
            MapAlphabetToGreek.Add(Keys.I, Tuple.Create("Ι", "ι"));
            MapAlphabetToGreek.Add(Keys.J, Tuple.Create("ϑ", "ϕ"));
            MapAlphabetToGreek.Add(Keys.K, Tuple.Create("Κ", "κ"));
            MapAlphabetToGreek.Add(Keys.L, Tuple.Create("Λ", "λ"));
            MapAlphabetToGreek.Add(Keys.M, Tuple.Create("Μ", "μ"));
            MapAlphabetToGreek.Add(Keys.N, Tuple.Create("Ν", "ν"));
            MapAlphabetToGreek.Add(Keys.O, Tuple.Create("Ο", "ο"));
            MapAlphabetToGreek.Add(Keys.P, Tuple.Create("Π", "π"));
            MapAlphabetToGreek.Add(Keys.Q, Tuple.Create("Θ", "θ"));
            MapAlphabetToGreek.Add(Keys.R, Tuple.Create("Ρ", "ρ"));
            MapAlphabetToGreek.Add(Keys.S, Tuple.Create("Σ", "σ"));
            MapAlphabetToGreek.Add(Keys.T, Tuple.Create("Τ", "τ"));
            MapAlphabetToGreek.Add(Keys.U, Tuple.Create("Υ", "υ"));
            MapAlphabetToGreek.Add(Keys.V, Tuple.Create("ς", "ϖ"));
            MapAlphabetToGreek.Add(Keys.W, Tuple.Create("Ω", "ω"));
            MapAlphabetToGreek.Add(Keys.X, Tuple.Create("Ξ", "ξ"));
            MapAlphabetToGreek.Add(Keys.Y, Tuple.Create("Ψ", "ψ"));
            MapAlphabetToGreek.Add(Keys.Z, Tuple.Create("Ζ", "ζ"));
        }

        /// <summary>
        /// ギリシャ文字入力モードに移行します。
        /// </summary>
        private void EnterGreekLettersMode() { GreekLettersMode = true; }

        /// <summary>
        /// キャレット移動時にギリシャ文字入力モードを解除する場合、対象コントロールのイベントにメソッドを追加します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CaretMoved(object sender, EventArgs e)
        {
            if (GreekLettersMode) GreekLettersMode = false;
        }

        /// <summary>
        /// キーが離された時の動作を実行します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.G && e.Control)
            {
                EnterGreekLettersMode();
                return;
            }
            else if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey)
            {
                return;
            }
            else if (GreekLettersMode && MapAlphabetToGreek.ContainsKey(e.KeyCode))
            {
                var value = MapAlphabetToGreek[e.KeyCode];
                InsertText(e.Shift ? value.Item1 : value.Item2);
            }
            else
            {
                GreekLettersMode = false;
            }
        }

    }
}
