using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Sio.Xml;
using GoodSeat.Sio.Xml.Serialization;

namespace GoodSeat.Clapte.ViewModels.ClapteCommands
{
    /// <summary>
    /// 区切り数値データ集計コマンドを表します。
    /// </summary>
    public class SplitDataCountCommand : ClapteCommand, ISerializable
    {
        /// <summary>
        /// 区切り数値データ集計コマンドを初期化します。
        /// </summary>
        public SplitDataCountCommand(ClapteCoreViewModel owner)
            : base(owner)
        {
            this.Enable = true;
            this.MaxTextLength = 1000;
            this.CountTargetData = SplitDataCounter.CountingData.All;
            this.CopyTargetData = SplitDataCounter.CountingData.Sum | SplitDataCounter.CountingData.Average;

            Counter = new SplitDataCounter();
        }

        /// <summary>
        /// 区切り数値データ集計コマンドの有効可否を設定もしくは取得します。
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 対象とする数式の最大文字列長を設定もしくは取得します。
        /// </summary>
        public int MaxTextLength { get; set; }


        /// <summary>
        /// 集計対象とするデータを設定もしくは取得します。
        /// </summary>
        public SplitDataCounter.CountingData CountTargetData { get; set; }

        /// <summary>
        /// コピー対象とするデータを設定もしくは取得します。
        /// </summary>
        public SplitDataCounter.CountingData CopyTargetData { get; set; }

        /// <summary>
        /// 区切り数値データの集計オブジェクトを取得します。
        /// </summary>
        public SplitDataCounter Counter { get; private set; }

        /// <summary>
        /// 最後にコピーされたデータ種類を設定もしくは取得します。
        /// </summary>
        SplitDataCounter.CountingData LastCopiedData { get; set; }

        /// <summary>
        /// 集計対象データを表す文字列を取得します。
        /// </summary>
        /// <param name="data">対象のデータ種類。</param>
        /// <returns>データを表す文字列。説明のない場合、null。</returns>
        string TextOf(SplitDataCounter.CountingData data)
        {
            switch (data)
            {
                case SplitDataCounter.CountingData.DataCount: return "数値個数";
                case SplitDataCounter.CountingData.Sum: return "総和";
                case SplitDataCounter.CountingData.Average: return "平均値";
                case SplitDataCounter.CountingData.Maximum: return "最大値";
                case SplitDataCounter.CountingData.Minimum: return "最小値";
                default: return null;
            }
        }

        /// <summary>
        /// 次回コピー対象となるデータ種類を取得します。
        /// </summary>
        /// <returns>次回のコピー対象とするデータ種類。</returns>
        SplitDataCounter.CountingData GetNextCopyData()
        {
            bool hit = false;
            foreach (SplitDataCounter.CountingData data in Enum.GetValues(typeof(SplitDataCounter.CountingData)))
            {
                if (data == LastCopiedData) // このデータの次が対象となるデータ。
                {
                    hit = true;
                    continue;
                }
                if (TextOf(data) == null) continue;

                if (hit && (CopyTargetData & data) == data) return data;
            }
            return SplitDataCounter.CountingData.None;
        }

        public override ClapteCommandResult TryInitializeCommand(string text)
        {
            if (!Enable) return null;
            if (text.Length > MaxTextLength) return null;
            if (!Counter.CountSplitData(text)) return null;
            LastCopiedData = SplitDataCounter.CountingData.None;

            string result = "";

            foreach (var data in Enum.GetValues(typeof(SplitDataCounter.CountingData)))
            {
                var countData = (SplitDataCounter.CountingData)data;
                if ((CountTargetData & countData) == SplitDataCounter.CountingData.None) continue;

                string txt = TextOf(countData);
                if (txt == null) continue;

                result += string.Format("\n{0}：{1}", txt, Counter.CountedData[countData]);
            }
            if (string.IsNullOrEmpty(result)) return null;

            var nextCopyDataText = TextOf(GetNextCopyData());
            if (nextCopyDataText != null) result += string.Format("\n\n操作の続行で、{0}をコピーします。", nextCopyDataText);

            return new ClapteCommandResult("区切り数値集計結果", result.TrimStart('\n'), ToolTipIcon.Info, true);
        }

        public override ClapteCommandResult DoAction()
        {
            var copyData = GetNextCopyData();
            var copyDataText = TextOf(copyData);
            if (copyDataText == null) return null;

            try
            {
                var text = Counter.CountedData[copyData].ToString();
                Clipboard.SetText(text);
                LastCopiedData = copyData;

                var nextCopyDataText = TextOf(GetNextCopyData());
                if (nextCopyDataText != null)
                    return new ClapteCommandResult("集計結果コピー", string.Format("\"{0}\"({1})をコピーしました。\n\n操作の続行で、{2}をコピーします。", text, copyDataText, nextCopyDataText), ToolTipIcon.Info, true);
                else
                    return new ClapteCommandResult("集計結果コピー", string.Format("\"{0}\"({1})をコピーしました。", text, copyDataText), ToolTipIcon.Info, false);
            }
            catch
            {
                return new ClapteCommandResult("結果コピー失敗", "結果のコピーに失敗しました。\nこのメッセージが表示されている間、再試行できます。", ToolTipIcon.Error, true);
            }
        }

        #region ISerializable メンバー

        public void OnDeserialize(XmlElement xmlElement)
        {
            Enable = bool.Parse(xmlElement.GetAttribute("Enable"));
            MaxTextLength = int.Parse(xmlElement.GetAttribute("MaxTextLength"));

            CountTargetData = (SplitDataCounter.CountingData)Enum.Parse(typeof(SplitDataCounter.CountingData), xmlElement.GetAttribute("CountTargetData"));
            CopyTargetData = (SplitDataCounter.CountingData)Enum.Parse(typeof(SplitDataCounter.CountingData), xmlElement.GetAttribute("CopyTargetData"));

            Counter.SplitCharList.Clear();
            foreach (var splitCharElement in xmlElement["SplitChars"].GetElements("SplitChar"))
            {
                Counter.SplitCharList.Add(char.Parse(splitCharElement.GetAttribute("Char")));
            }
        }

        public void OnSerialize(XmlElement xmlElement)
        {
            xmlElement.AddAttribute("Enable", Enable.ToString());
            xmlElement.AddAttribute("MaxTextLength", MaxTextLength.ToString());

            xmlElement.AddAttribute("CountTargetData", CountTargetData.ToString());
            xmlElement.AddAttribute("CopyTargetData", CopyTargetData.ToString());

            XmlElement splitCharsElement = new XmlElement("SplitChars");
            foreach (var splitChar in Counter.SplitCharList)
            {
                XmlElement splitCharElement = new XmlElement("SplitChar");
                splitCharElement.AddAttribute("Char", splitChar.ToString());
                splitCharsElement.AddElements(splitCharElement);
            }
            xmlElement.AddElements(splitCharsElement);
        }

        #endregion
    }
}
