using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sgry.Azuki.WinForms;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.Views.InputSupports;
using System.Windows.Forms;
using System.Drawing;

namespace GoodSeat.Clapte.Views
{
    /// <summary>
    /// Azukiコントロールを対象とした、関数の引数ヘルプ機能を表します。
    /// </summary>
    public sealed class FunctionArgumentHelp : IDisposable
    {
        /// <summary>
        /// Azukiコントロールを対象とした、関数の引数ヘルプ機能を初期化します。
        /// </summary>
        /// <param name="azuki">補助機能を提供する対象となるAzukiコントロール。</param>
        /// <param name="owner">補助候補のリストボックスを表示する先のコントロール。</param>
        /// <param name="enumerator">入力補助の候補列挙オブジェクト。</param>
        public FunctionArgumentHelp(AzukiControl azuki, Control owner, IInputSupportEnumerator enumerator)
        {
            Azuki = azuki;
            Owner = owner;
            InputSupportEnumerator = enumerator;

            HelpBox = new RichTextBox();
            HelpBox.Font = Azuki.Font;
            HelpBox.BorderStyle = BorderStyle.None;
            HelpBox.Cursor = Cursors.Default;
            HelpBox.ForeColor = Color.Blue;
            HelpBox.Visible = false;
            HelpBox.Enter += (sender, e) => HelpBox.Visible = false;
            owner.Controls.Add(HelpBox);

            azuki.FontChanged += new EventHandler(azuki_FontChanged);
            azuki.CaretMoved += new EventHandler(azuki_CaretMoved);

            AutoShow = true;
        }

        /// <summary>
        /// 対象となるAzukiコントロールを設定もしくは取得します。
        /// </summary>
        AzukiControl Azuki { get; set; }

        /// <summary>
        /// 入力補助リストの表示先コントロールを設定もしくは取得します。
        /// </summary>
        Control Owner { get; set; }

        /// <summary>
        /// AzukiのOwnerに対する相対位置を設定します。
        /// </summary>
        public Point ModifyLocation { private get; set; }

        /// <summary>
        /// 関数の引数ヘルプを表示するリッチテキストボックスを設定もしくは取得します。
        /// </summary>
        RichTextBox HelpBox { get; set; }

        /// <summary>
        /// 入力補助の候補列挙オブジェクトを設定もしくは取得します。
        /// </summary>
        IInputSupportEnumerator InputSupportEnumerator { get; set; }

        /// <summary>
        /// キャレットの移動時に、自動で表示するか否かを設定もしくは取得します。
        /// </summary>
        public bool AutoShow { get; set; }

        void azuki_FontChanged(object sender, EventArgs e)
        {
            HelpBox.Font = Azuki.Font;
        }

        void azuki_CaretMoved(object sender, EventArgs e)
        {
            if (AutoShow || HelpBox.Visible) ShowArgumentHelp();
        }

        /// <summary>
        /// 現在のキャレット位置に応じて、関数の引数ヘルプを表示します。
        /// </summary>
        public void ShowArgumentHelp()
        {
            HelpBox.Visible = false;

            int startIndex, argIndex;
            string currentFunctionName = Azuki.GetCurrentFunctionName(out startIndex, out argIndex);
            if (currentFunctionName == null) return;

            var list = new List<InputSupportCandidate>(InputSupportEnumerator.GetAllCandidates(currentFunctionName).Where(
                        item => item.ReplaceText == currentFunctionName && item.Tag is FunctionDefine)
                    );
            if (list.Count != 1) return;

            var target = list[0].Tag as FunctionDefine;

            if (target.ArgumentInformation.Count <= argIndex) argIndex = -1;

            int currentStart = 0, currentLength = 0;
            HelpBox.Text = target.Name + "(";
            for (int i = 0; i < target.ArgumentInformation.Count; i++)
            {
                if (i != 0) HelpBox.Text += ", ";
                var text = target.ArgumentInformation[i];
                HelpBox.Text += text;

                if (i == argIndex)
                {
                    currentStart = HelpBox.Text.Length - text.Length;
                    currentLength = text.Length;
                }
            }
            HelpBox.Text += ")";

            HelpBox.SelectionStart = currentStart;
            HelpBox.SelectionLength = currentLength;
            HelpBox.SelectionFont = new Font(HelpBox.Font, (FontStyle.Bold | FontStyle.Underline));
            HelpBox.SelectionLength = 0;

            HelpBox.Visible = true;
            HelpBox.BringToFront();

            SetHelpBoxPosition(startIndex);
        }

        private void SetHelpBoxPosition(int index)
        {
            Point p = Azuki.GetPositionFromIndex(index);
            HelpBox.Location = new Point(p.X + ModifyLocation.X, p.Y + ModifyLocation.Y - (int)HelpBox.Font.Size * 2);
            HelpBox.Size = new Size((int)(HelpBox.PreferredSize.Width * 1.2), (int)(HelpBox.PreferredSize.Height * 1.5));
        }

        public void Dispose()
        {
            Azuki.FontChanged -= new EventHandler(azuki_FontChanged);
            Azuki.CaretMoved -= new EventHandler(azuki_CaretMoved);
        }
    }
}
