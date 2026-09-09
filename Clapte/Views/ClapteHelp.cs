// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            s_pathMap.Add("設定画面/一般設定", "02-01_一般設定");
            s_pathMap.Add("設定画面/計算の詳細", "02-02_計算の詳細");
            s_pathMap.Add("設定画面/計算機", "02-03_計算機");
            s_pathMap.Add("設定画面/監視対象外", "02-04_監視対象外");
            s_pathMap.Add("設定画面/区切り数値集計", "02-05_区切り数値集計");
            s_pathMap.Add("設定画面/定数", "02-06_定数");
            s_pathMap.Add("設定画面/関数", "02-07_関数");
            s_pathMap.Add("設定画面/単位換算表", "02-08_単位換算表");
            s_pathMap.Add("設定画面/バージョン情報", "02-09_バージョン情報");

            s_pathMap.Add("計算機", "03_計算機");
        }
        /// <summary>
        /// Clapteのヘルプファイルパスを取得します。
        /// </summary>
        public static string BaseURL
        {
            get { return "https://github.com/GoodSeat/Clapte/wiki/"; }
        }

        /// <summary>
        /// Clapteのヘルプファイルを開きます。
        /// </summary>
        /// <param name="control">ヘルプの親を認識するためのコントロール</param>
        public static void Show(Control control)
        {
            Process.Start(BaseURL + GetTopicPath("Top"));
        }

        /// <summary>
        /// トピック名を指定してClapteのヘルプファイルを開きます。
        /// </summary>
        /// <param name="control">ヘルプの親を認識するためのコントロール</param>
        /// <param name="topic">トピック名</param>
        public static void Show(Control control, string topic)
        {
            Process.Start(BaseURL + GetTopicPath(topic));
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
                return "";
        }
    }
}
