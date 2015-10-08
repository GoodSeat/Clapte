using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using GoodSeat.Sio.Xml.Serialization;
using GoodSeat.Sio.Xml;

namespace GoodSeat.Clapte.Models.Windows
{
	/// <summary>
	/// ウインドウを識別する情報を表します。
	/// </summary>
	public class WindowIdentifyInfo : ISerializable
	{
        /// <summary>
        /// ウインドウ情報の文字列属性を表します。
        /// </summary>
        public enum PropertyType
        {
            /// <summary>タイトル。</summary>
            TitleBarText,
            /// <summary>製品名。</summary>
            ProductName,
            /// <summary>ファイル名。</summary>
            FileName,
            /// <summary>クラス名。</summary>
            ClassName
        }

        /// <summary>
        /// 文字列の一致判定方法を現します。
        /// </summary>
        public enum MatchType
        {
            /// <summary>部分一致。</summary>
            Contain,
            /// <summary>完全一致。</summary>
            Match
        }

        Dictionary<PropertyType, string> _textMap;
        Dictionary<PropertyType, Regex> _regexMap;
        Dictionary<PropertyType, bool> _useRegexMap;
        Dictionary<PropertyType, bool> _validMap;
        Dictionary<PropertyType, MatchType> _matchMap;

        /// <summary>
        /// ウインドウを識別する情報を初期化します。
        /// </summary>
        public WindowIdentifyInfo()
        {
            _textMap = new Dictionary<PropertyType, string>();
            _regexMap = new Dictionary<PropertyType, Regex>();
            _useRegexMap = new Dictionary<PropertyType, bool>();
            _validMap = new Dictionary<PropertyType, bool>();
            _matchMap = new Dictionary<PropertyType, MatchType>();

            for (PropertyType type = PropertyType.TitleBarText; type <= PropertyType.ClassName; type++)
            {
                _textMap.Add(type, null);
                _regexMap.Add(type, null);
                _useRegexMap.Add(type, false);
                _validMap.Add(type, false);
                _matchMap.Add(type, MatchType.Contain);
            }
        }

        /// <summary>
        /// ウインドウを識別する情報を初期化します。
        /// </summary>
        /// <param name="baseInfo">初期化基準とするウインドウ識別情報。</param>
        public WindowIdentifyInfo(WindowIdentifyInfo baseInfo)
            : this()
        {
            for (PropertyType type = PropertyType.TitleBarText; type <= PropertyType.ClassName; type++)
            {
                _textMap[type] = baseInfo._textMap[type];
                _regexMap[type] = baseInfo._regexMap[type];
                _useRegexMap[type] = baseInfo._useRegexMap[type];
                _validMap[type] = baseInfo._validMap[type];
                _matchMap[type] = baseInfo._matchMap[type];
            }
        }


        /// <summary>
        /// 指定属性の文字列を指定します。
        /// </summary>
        /// <param name="type">指定対象とする属性タイプ。</param>
        /// <param name="text">指定する文字列。</param>
        public void SetTextOf(PropertyType type, string text)
        {
            if (text == _textMap[type]) return;
            _textMap[type] = text;
            _regexMap[type] = null;
        }

        /// <summary>
        /// 指定属性の文字列を取得します。
        /// </summary>
        /// <param name="type">指定対象とする属性タイプ。</param>
        public string GetTextOf(PropertyType type) { return _textMap[type]; }

        /// <summary>
        /// 指定属性の文字列を正規表現として解釈するか否かを指定します。
        /// </summary>
        /// <param name="type">指定対象とする属性タイプ。</param>
        /// <param name="asRegex">正規表現として扱うか否か。</param>
        public void SetUsingRegexIn(PropertyType type, bool asRegex) { _useRegexMap[type] = asRegex; }

        /// <summary>
        /// 指定属性の文字列を正規表現として解釈するか否かを取得します。
        /// </summary>
        /// <param name="type">指定対象とする属性タイプ。</param>
        public bool GetUsingRegexIn(PropertyType type) { return _useRegexMap[type]; }

        /// <summary>
        /// 指定属性を判定対象とするか否かを指定します。
        /// </summary>
        /// <param name="type">指定対象とする属性タイプ。</param>
        /// <param name="asRegex">正規表現として扱うか否か。</param>
        public void SetValidOf(PropertyType type, bool use) { _validMap[type] = use; }

        /// <summary>
        /// 指定属性を判定対象とするか否かを取得します。
        /// </summary>
        /// <param name="type">指定対象とする属性タイプ。</param>
        public bool GetValidOf(PropertyType type) { return _validMap[type]; }


        /// <summary>
        /// 指定文字列を検査する正規表現を取得します。
        /// </summary>
        /// <param name="type">指定対象とする属性タイプ。</param>
        /// <returns>検査用の正規表現。</returns>
        Regex GetRegexOf(PropertyType type)
        {
            if (_regexMap[type] != null) return _regexMap[type];

            _regexMap[type] = new Regex(GetTextOf(type));
            return _regexMap[type];
        }

        /// <summary>
        /// 指定属性の判定方法を取得します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void SetMatchTypeOf(PropertyType type, MatchType matchType) { _matchMap[type] = matchType; }

        /// <summary>
        /// 指定属性の判定方法を取得します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public MatchType GetMatchTypeOf(PropertyType type) { return _matchMap[type]; }

        /// <summary>
        /// ウインドウ情報から指定の属性タイプの文字列を取得します。
        /// </summary>
        /// <param name="windowInfo">取得対象とするウインドウ情報。</param>
        /// <param name="type">指定対象とする属性タイプ。</param>
        /// <returns>ウインドウ情報から取得された文字列。</returns>
        string GetTextFromWindowInfoOf(WindowInfo windowInfo, PropertyType type)
        {
            switch (type)
            {
                case PropertyType.TitleBarText: return windowInfo.TitleBarText;
                case PropertyType.ProductName: return windowInfo.ProductName;
                case PropertyType.FileName: return windowInfo.FileName;
                case PropertyType.ClassName: return windowInfo.ClassName;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// 指定のウインドウ情報が、この識別情報に合致するか否かを取得します。
        /// </summary>
        /// <param name="windowInfo">判定対象のウインドウ情報。</param>
        /// <returns>合致するか否か。</returns>
        public bool MatchWith(WindowInfo windowInfo)
        {
            for (PropertyType type = PropertyType.TitleBarText; type <= PropertyType.ClassName; type++)
            {
                if (!GetValidOf(type)) continue;

                string filterText = GetTextOf(type);
                if (string.IsNullOrEmpty(filterText)) continue;

                string testText = GetTextFromWindowInfoOf(windowInfo, type);
                if (GetUsingRegexIn(type))
                {
                    if (GetMatchTypeOf(type) == MatchType.Contain)
                    {
                        if (GetRegexOf(type).IsMatch(testText)) return true;
                    }
                    else
                    {
                        if (GetRegexOf(type).Replace(testText, "") == "") return true;
                    }
                }
                else
                {
                    if (GetMatchTypeOf(type) == MatchType.Contain)
                    {
                        if (testText.Contains(filterText)) return true;
                    }
                    else
                    {
                        if (testText == filterText) return true;
                    }
                }
            }

            return false;
        }


        #region ISerializable メンバー

        public void OnDeserialize(XmlElement xmlElement)
        {
            for (PropertyType type = PropertyType.TitleBarText; type <= PropertyType.ClassName; type++)
            {
                XmlElement elm = xmlElement[type.ToString()];
                SetTextOf(type, elm.GetAttribute("Text", ""));
                SetUsingRegexIn(type, bool.Parse(elm.GetAttribute("AsRegex", "False")));
                SetValidOf(type, bool.Parse(elm.GetAttribute("Valid", "False")));
                SetMatchTypeOf(type, (MatchType)Enum.Parse(typeof(MatchType), elm.GetAttribute("MatchType", "Contain")));
            }
        }

        public void OnSerialize(XmlElement xmlElement)
        {
            for (PropertyType type = PropertyType.TitleBarText; type <= PropertyType.ClassName; type++)
            {
                XmlElement elm = new XmlElement(type.ToString());
                elm.AddAttribute("Text", GetTextOf(type));
                elm.AddAttribute("AsRegex", GetUsingRegexIn(type).ToString());
                elm.AddAttribute("Valid", GetValidOf(type).ToString());
                elm.AddAttribute("MatchType", GetMatchTypeOf(type).ToString());
                xmlElement.AddElements(elm);
            }
        }

        #endregion
    }
}
