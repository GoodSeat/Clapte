// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace GoodSeat.Clapte.Views.Forms.SettingPanels
{
    /// <summary>
    /// バージョン情報パネルを表します。
    /// </summary>
    public partial class VersionInformationPanel : SettingPanel
    {
        static bool s_isBeta = false;

        /// <summary>
        /// このバージョンがβバージョンか否かを取得します。
        /// </summary>
        public static bool IsBeta { get { return s_isBeta; } }

        /// <summary>
        /// バージョン情報パネルを初期化します。
        /// </summary>
        /// <param name="targetWathcer"></param>
        public VersionInformationPanel()
        {
            InitializeComponent();

            _labelRights.Text = AssemblyCopyright;
            _labelVersion.Text = "ver" + AssemblyVersion;

            if (IsBeta) _labelVersion.Text += " Beta";

            _labelDate.Text = "R" + AssemblyCreateDate;
            _labelDate.Left = _labelVersion.Right + 12;
        }

        #region アセンブリ属性アクセサー

        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string AssemblyVersion
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().GetName().Version.ToString());
            }
        }

        public static string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public static string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public static string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public static string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        public static string AssemblyCreateDate
        {
            get
            {
                return "20190203";
            }
        }

        #endregion

        private void _linkHP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(_linkHP.Text);            
        }

        private void _linkMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:" + _linkMail.Text);
        }
    }
}
