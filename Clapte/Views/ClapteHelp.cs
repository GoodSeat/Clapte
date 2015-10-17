using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GoodSeat.Clapte.Views
{
    /// <summary>
    /// Clapteの状況依存ヘルプ機能を提供します。
    /// </summary>
    public static class ClapteHelp
    {
        static Dictionary<string, string> s_pathMap = new Dictionary<string, string>();

        static ClapteHelp()
        {
            s_pathMap.Add("一般設定", "03_reference/01_settingDialog/01_general.html");
            s_pathMap.Add("区切り数値集計", "03_reference/01_settingDialog/02_splitDataCount.html");
            s_pathMap.Add("計算機", "03_reference/01_settingDialog/03_calculator.html");
            s_pathMap.Add("方程式", "03_reference/01_settingDialog/04_solveEquation.html");
            s_pathMap.Add("定数", "03_reference/01_settingDialog/05_constant.html");
            s_pathMap.Add("関数", "03_reference/01_settingDialog/06_function.html");
            s_pathMap.Add("単位変換表", "03_reference/01_settingDialog/07_unitTable.html");
            s_pathMap.Add("バージョン情報", "03_reference/01_settingDialog/08_versionInfo.html");
        }
        /// <summary>
        /// Clapteのヘルプファイルパスを取得します。
        /// </summary>
        public static string URL
        {
            get { return Application.StartupPath + "\\ClapteHelp.chm"; }
        }

        /// <summary>
        /// Clapteのヘルプファイルを開きます。
        /// </summary>
        /// <param name="control">ヘルプの親を認識するためのコントロール</param>
        public static void Show(Control control)
        {
            if (!CheckExistFile()) return;

            Help.ShowHelp(control, URL);
        }

        /// <summary>
        /// トピック名を指定してClapteのヘルプファイルを開きます。
        /// </summary>
        /// <param name="control">ヘルプの親を認識するためのコントロール</param>
        /// <param name="topic">トピック名</param>
        public static void Show(Control control, string topic)
        {
            if (!CheckExistFile()) return;

            Help.ShowHelp(control, URL, HelpNavigator.Topic, GetTopicPath(topic));
        }

        /// <summary>
        /// トピック名を指定してClapteのヘルプファイルを開きます。
        /// </summary>
        /// <param name="control">ヘルプの親を認識するためのコントロール</param>
        /// <param name="topic">トピック名</param>
        /// <param name="bookmark">ブックマーク名</param>
        public static void Show(Control control, string topic, string bookmark)
        {
            if (!CheckExistFile()) return;

            Help.ShowHelp(control, URL, HelpNavigator.Topic, GetTopicPath(topic) + "#" + bookmark);
        }

        /// <summary>
        /// ヘルプファイルが存在するか調べ、必要に応じてメッセージを表示します。
        /// </summary>
        /// <returns></returns>
        static bool CheckExistFile()
        {
            if (!System.IO.File.Exists(URL))
            {
                MessageBox.Show("ヘルプファイル \"" + URL + "\" が見つかりません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// トピック名に対応したパスを取得します。該当トピック名が見つからない場合、トップページのパスを返します。
        /// </summary>
        /// <param name="topic">トピック名</param>
        /// <returns></returns>
        public static string GetTopicPath(string topic)
        {
            if (s_pathMap.ContainsKey(topic))
                return s_pathMap[topic];
            else
                return "default.html";
        }
    }
}
