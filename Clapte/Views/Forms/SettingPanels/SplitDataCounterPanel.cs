using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GoodSeat.Clapte.ViewModels.ClapteCommands;
using GoodSeat.Clapte.Solvers;

namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
	/// <summary>
	/// 区切り数値認識設定パネルを表します。
	/// </summary>
	public partial class SplitDataCounterPanel : SettingPanel
	{
		/// <summary>
		/// 区切り数値認識設定パネルを初期化します。
		/// </summary>
		/// <param name="target">設定対象の区切り数値の集計コマンド。</param>
		public SplitDataCounterPanel(SplitDataCountCommand target)
		{
			InitializeComponent();

			_checkEnable.CheckedChanged += (sender, e) => _groupSetting.Enabled = _checkEnable.Checked;

			Target = target;
			DownloadSetting();
		}

		/// <summary>
		/// 設定対象の区切り数値の集計コマンドを設定もしくは取得します。
		/// </summary>
		SplitDataCountCommand Target { get; set; }

		/// <summary>
		/// 現在の設定をコントロールの表示に反映します。
		/// </summary>
		void DownloadSetting()
		{
			_checkEnable.Checked = Target.Enable;
			_numMaxLength.Value = Target.MaxTextLength;
			foreach (char c in Target.Counter.SplitCharList)
				_checkListSplit.SetItemChecked(GetIndexOf(c), true);
			foreach (SplitDataCounter.CountingData data in Enum.GetValues(typeof(SplitDataCounter.CountingData)))
			{
				if (data == SplitDataCounter.CountingData.None) continue;
				if (data == SplitDataCounter.CountingData.All) continue;
				if ((Target.CountTargetData & data) == data)
				{
					int index = 0;
					int target = (int)data;
					while (target != 1)
					{
						index++;
						target /= 2;
					}
					_checkListData.SetItemChecked(index, true);
				}
			}
		}

		public override void  OnDeterminSetting()
		{
			Target.Enable = _checkEnable.Checked;
			Target.MaxTextLength = (int)_numMaxLength.Value;

			Target.Counter.SplitCharList.Clear();
			foreach (int index in _checkListSplit.CheckedIndices)
			{
				if (index == 0)
				{
					Target.Counter.SplitCharList.Add('\r');
					Target.Counter.SplitCharList.Add('\n');
				}
				else if (index == 1) Target.Counter.SplitCharList.Add(' ');
				else if (index == 2) Target.Counter.SplitCharList.Add('　');
				else if (index == 3) Target.Counter.SplitCharList.Add('\t');
				else if (index == 4) Target.Counter.SplitCharList.Add(',');
			}

			int targetData = 0;
			foreach (int index in _checkListData.CheckedIndices) targetData += (int)Math.Pow(2, index);
			Target.CountTargetData = (SplitDataCounter.CountingData)targetData;
		}

		int GetIndexOf(char c)
		{
			if (c == '\r' || c == '\n') return 0;
			else if (c == ' ') return 1;
			else if (c == '　') return 2;
			else if (c == '\t') return 3;
			else if (c == ',') return 4;
			else throw new NotImplementedException( c.ToString() + "は想定されていない区切り文字です。");
		}
	}
}
