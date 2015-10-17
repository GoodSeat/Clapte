using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Reals;

namespace GoodSeat.Clapte.Solvers
{
    /// <summary>
    /// 区切り数値データの集計クラスを表します。
    /// </summary>
    public class SplitDataCounter
    {
        /// <summary>
        /// 表示対象のデータを表します。
        /// </summary>
        [Flags]
        public enum CountingData
        {
            /// <summary>
            /// 対象なし
            /// </summary>
            None = 0,
            /// <summary>
            /// 数値データの個数
            /// </summary>
            DataCount = 1,
            /// <summary>
            /// 数値データの総和
            /// </summary>
            Sum = 2,
            /// <summary>
            /// 数値データの平均
            /// </summary>
            Average = 4,
            /// <summary>
            /// 数値データの最大値
            /// </summary>
            Maximum = 8,
            /// <summary>
            /// 数値データの最小値
            /// </summary>
            Minimum = 16,
            /// <summary>
            /// 全データ
            /// </summary>
            All = 31
        }

        List<char> _splitCharList = new List<char>();

        /// <summary>
        /// 区切り数値データの集計クラスを初期化します。
        /// </summary>
        public SplitDataCounter()
        {
            CountedData = new Dictionary<CountingData, double>();

            _splitCharList.Add('\n');
            _splitCharList.Add('\r');
            _splitCharList.Add('\t');
            _splitCharList.Add(' ');
            _splitCharList.Add('　');
            _splitCharList.Add(',');
        }

        /// <summary>
        /// 区切り文字リストを取得します。
        /// </summary>
        public List<Char> SplitCharList
        {
            get { return _splitCharList; }
        }

        /// <summary>
        /// 最新の集計結果を取得します。
        /// </summary>
        public Dictionary<CountingData, double> CountedData { get; private set; }

        /// <summary>
        /// 指定文字列から区切り数値データの集計データ作成を試みます。
        /// </summary>
        /// <param name="targetText">対象とする文字列。</param>
        /// <returns>集計に成功したか。</returns>
        public bool CountSplitData(string targetText)
        {
            string[] split = targetText.Split(_splitCharList.ToArray());

            List<ValidReal> valueList = new List<ValidReal>();
            for (int i = 0; i < split.Length; i++)
            {
                if (split[i].Length == 0) continue;

                double value = 0;
                if (!double.TryParse(split[i], out value)) return false;

                valueList.Add(new ValidReal(value));
            }

            int count = valueList.Count;
            if (count < 2) return false;
            double sum = 0;
            double max = double.MinValue;
            double min = double.MaxValue;
            foreach (ValidReal data in valueList)
            {
                sum += data;
                max = Math.Max(max, data);
                min = Math.Min(min, data);
            }
            double average = sum / (double)count;

            CountedData.Clear();
            CountedData.Add(CountingData.DataCount, count);
            CountedData.Add(CountingData.Sum, sum);
            CountedData.Add(CountingData.Average, average);
            CountedData.Add(CountingData.Maximum, max);
            CountedData.Add(CountingData.Minimum, min);
            return true;
        }

    }
}
