// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Linq;
using Sgry.Azuki;
using Sgry.Azuki.Highlighter;

namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    /// <summary>
    /// 計算機の設定パネルを表します。
    /// </summary>
    public partial class CalculatorSettingPanel : SettingPanel
    {
        InstalledFontCollection _installedFonts;

        /// <summary>
        /// 計算機の文字区分色設定パネルを初期化します。
        /// </summary>
        /// <param name="target">設定対象のClaptePadオブジェクト。</param>
        public CalculatorSettingPanel(FormOfClaptePad target)
        {
            InitializeComponent();
            Target = target;

            var highlighter = new KeywordHighlighter();
            highlighter.AddKeywordSet(new string[] { "A", "pi" }, target.Highlighter.GetCharClassOf(ClaptePadKeywordHighlighter.SyntaxTarget.Constant));
            highlighter.AddKeywordSet(new string[] { "sin" }, target.Highlighter.GetCharClassOf(ClaptePadKeywordHighlighter.SyntaxTarget.Function));
            highlighter.AddKeywordSet(new string[] { "cm" }, target.Highlighter.GetCharClassOf(ClaptePadKeywordHighlighter.SyntaxTarget.Unit));
            highlighter.AddKeywordSet(new string[] { "+" }, target.Highlighter.GetCharClassOf(ClaptePadKeywordHighlighter.SyntaxTarget.Operator));
            highlighter.AddKeywordSet(new string[] { "$if" }, target.Highlighter.GetCharClassOf(ClaptePadKeywordHighlighter.SyntaxTarget.Control));
            highlighter.AddLineHighlight("#", target.Highlighter.GetCharClassOf(ClaptePadKeywordHighlighter.SyntaxTarget.Comment));
            _textBoxSample.Highlighter = highlighter;
            _textBoxSample.ColorScheme.SelectionBack = Target.ColorScheme.SelectionBack;
            _textBoxSample.ColorScheme.HighlightColor = Target.ColorScheme.HighlightColor;
            _textBoxSample.ColorScheme.MatchedBracketBack = Target.ColorScheme.MatchedBracketBack;
            _textBoxSample.Text = "$if 1 # サンプル\r\n\tA = sin(pi) + 5[cm]";
            _textBoxSample.IsReadOnly = true;

            _installedFonts = new InstalledFontCollection();
            _comboFont.Items.AddRange(_installedFonts.Families.Select(font => font.Name).ToArray());

            DownloadSetting();
        }

        /// <summary>
        /// 設定対象のClaptePadオブジェクトを設定もしくは取得します。
        /// </summary>
        private FormOfClaptePad Target { get; set; }

        /// <summary>
        /// 現在の設定を画面上に反映させます。
        /// </summary>
        void DownloadSetting()
        {
            _comboFont.SelectedItem = Target.FontInfo.ToFont().FontFamily.Name;

            _numFontSize.Value = Target.FontInfo.Size;
            _checkUnderline.Checked = Target.ShowUnderLine;
            _checkVisibleTab.Checked = Target.ShowTabChara;
            _checkVisibleEol.Checked = Target.ShowEolChara;

            _checkAutoShowInputSupport.Checked = Target.EditorViewModel.AutoShowInputSupport;
            _checkInputSupportAlsoInfomation.Checked = Target.EditorViewModel.InputSupportWithAlsoInfomation;
            _checkAutoShowArgumentHint.Checked = Target.EditorViewModel.AutoShowArgumentHelp;

            _picColorConstant.BackColor = Target.GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Constant);
            _picColorFunction.BackColor = Target.GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Function);
            _picColorOperator.BackColor = Target.GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Operator);
            _picColorUnit.BackColor = Target.GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Unit);
            _picColorComment.BackColor = Target.GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Comment);
            _picColorOperation.BackColor = Target.GetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Control);

            RenewSample();
        }

        /// <summary>
        /// 画面上の設定を反映します。
        /// </summary>
        public override void OnDeterminSetting()
        {
            FontInfo fontInfo = new FontInfo(_installedFonts.Families[_comboFont.SelectedIndex].Name, (int)_numFontSize.Value, FontStyle.Regular);
            Target.FontInfo = fontInfo;
            Target.ShowUnderLine = _checkUnderline.Checked;
            Target.ShowTabChara = _checkVisibleTab.Checked;
            Target.ShowEolChara = _checkVisibleEol.Checked;

            Target.EditorViewModel.AutoShowInputSupport = _checkAutoShowInputSupport.Checked;
            Target.EditorViewModel.InputSupportWithAlsoInfomation = _checkInputSupportAlsoInfomation.Checked;
            Target.EditorViewModel.AutoShowArgumentHelp = _checkAutoShowArgumentHint.Checked;

            Target.SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Constant, _picColorConstant.BackColor);
            Target.SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Function, _picColorFunction.BackColor);
            Target.SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Operator, _picColorOperator.BackColor);
            Target.SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Unit, _picColorUnit.BackColor);
            Target.SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Comment, _picColorComment.BackColor);
            Target.SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.Control, _picColorOperation.BackColor);
            Target.SetSyntaxColorOf(ClaptePadKeywordHighlighter.SyntaxTarget.ControlResult, _picColorOperation.BackColor);
        }

        /// <summary>
        /// 画面上の設定に基づいてサンプル表示を更新します。
        /// </summary>
        void RenewSample()
        {
            FontInfo fontInfo = new FontInfo(_installedFonts.Families[_comboFont.SelectedIndex].Name, (int)_numFontSize.Value, FontStyle.Regular);

            _textBoxSample.FontInfo = fontInfo;
            _textBoxSample.HighlightsCurrentLine = _checkUnderline.Checked;
            _textBoxSample.DrawsTab= _checkVisibleTab.Checked ;
            _textBoxSample.DrawsEolCode = _checkVisibleEol.Checked;

            _textBoxSample.ColorScheme.SetColor(Target.Highlighter.GetCharClassOf(ClaptePadKeywordHighlighter.SyntaxTarget.Constant), _picColorConstant.BackColor, Color.White);
            _textBoxSample.ColorScheme.SetColor(Target.Highlighter.GetCharClassOf(ClaptePadKeywordHighlighter.SyntaxTarget.Function), _picColorFunction.BackColor, Color.White);
            _textBoxSample.ColorScheme.SetColor(Target.Highlighter.GetCharClassOf(ClaptePadKeywordHighlighter.SyntaxTarget.Operator), _picColorOperator.BackColor, Color.White);
            _textBoxSample.ColorScheme.SetColor(Target.Highlighter.GetCharClassOf(ClaptePadKeywordHighlighter.SyntaxTarget.Unit), _picColorUnit.BackColor, Color.White);
            _textBoxSample.ColorScheme.SetColor(Target.Highlighter.GetCharClassOf(ClaptePadKeywordHighlighter.SyntaxTarget.Comment), _picColorComment.BackColor, Color.White);
            _textBoxSample.ColorScheme.SetColor(Target.Highlighter.GetCharClassOf(ClaptePadKeywordHighlighter.SyntaxTarget.Control), _picColorOperation.BackColor, Color.White);
        }

        private void ColorSampleClicked(object sender, EventArgs e)
        {
            _colorDialog.Color = (sender as Control).BackColor;
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                (sender as Control).BackColor = _colorDialog.Color;
                SettingChanged(sender, e);
            }
        }

        void SettingChanged(object sender, EventArgs e)
        {
            RenewSample();
        }

    }
}
