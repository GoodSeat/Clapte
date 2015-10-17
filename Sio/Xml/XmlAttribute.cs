using System;
using System.Collections.Generic;
using System.Text;
using Adaptee = System.Xml;

namespace GoodSeat.Sio.Xml
{
    /// <summary>
    /// XmlElementの属性を表します。
    /// </summary>
    public class XmlAttribute
    {
        string _attributeName = "";
        string _attributeValue = "";

        /// <summary>
        /// XmlElementの属性を初期化します。
        /// </summary>
        /// <param name="name">属性の名称</param>
        /// <param name="value">属性の値</param>
        public XmlAttribute(string name, string value)
        {
            _attributeName = name;
            _attributeValue = value;
        }

        /// <summary>
        /// 属性の名称を設定もしくは取得します。
        /// </summary>
        public string Name { get { return _attributeName; } set { _attributeName = value; } }

        /// <summary>
        /// 属性の値を設定もしくは取得します。
        /// </summary>
        public string Value { get { return _attributeValue; } set { _attributeValue = value; } }

    }
}
