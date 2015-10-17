using System;
using System.Collections.Generic;
using System.Text;
using Adaptee = System.Xml;

namespace GoodSeat.Sio.Xml
{
    /// <summary>
    /// Xmlの要素を表します。
    /// </summary>
    public class XmlElement
    {
        string _name = "";
        string _value  = "";

        Dictionary<string, XmlAttribute> _attributes = new Dictionary<string, XmlAttribute>();
        List<XmlElement> _children = new List<XmlElement>();

        /// <summary>
        /// Xml要素を初期化します。
        /// </summary>
        /// <param name="name">要素の名称</param>
        public XmlElement(string name)
        {
            Initialize(name, null);
        }

        /// <summary>
        /// Xml要素を初期化します。
        /// </summary>
        /// <param name="name">要素の名称</param>
        /// <param name="attributes">要素に付加する可変数の属性</param>
        public XmlElement(string name, params XmlAttribute[] attributes)
        {
            Initialize(name, null, attributes);
        }

        /// <summary>
        /// Xml要素を初期化します。
        /// </summary>
        /// <param name="name">要素の名称</param>
        /// <param name="values">要素の値</param>
        public XmlElement(string name, params XmlElement[] values)
        {
            Initialize(name, null);
            Children.AddRange(values);
        }

        /// <summary>
        /// Xml要素を初期化します。
        /// </summary>
        /// <param name="name">要素の名称</param>
        /// <param name="value">要素の値</param>
        /// <param name="attributes">要素に付加する可変数の属性</param>
        public XmlElement(string name, string value, params XmlAttribute[] attributes)
        {
            Initialize(name, value, attributes);
        }

        /// <summary>
        /// 内部初期化作業です。
        /// </summary>
        /// <param name="name">要素の名称</param>
        /// <param name="value">要素の値</param>
        /// <param name="attributes">要素に付加する可変数の属性</param>
        void Initialize(string name, string value, params XmlAttribute[] attributes)
        {
            Name = name;
            if (value != null) Value = value;

            AddAttributes(attributes);
        }

        #region プロパティ

        /// <summary>
        /// 要素名称を設定もしくは取得します。
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }

        /// <summary>
        /// 要素内の内部文字列を設定もしくは取得します。
        /// </summary>
        public string Value
        {
            set 
            {
                SetInnerText(value);
            }
            get
            {
                if (Children.Count == 0) return _value;
                else
                {
                    string value = "";
                    foreach (XmlElement element in Children)
                        value += element.ToString() + "\n";
                    value = value.TrimEnd('\n');
                    return value;
                }
            }
        }

        /// <summary>
        /// 指定文字列をインナー要素として設定します。
        /// </summary>
        /// <param name="text"></param>
        internal void SetInnerText(string text)
        {
            _value = text;
            Children.Clear();
        }

        /// <summary>
        /// 指定名の子要素のうち、最初に見つかった要素を取得します。見つからない場合、nullを返します。
        /// </summary>
        /// <param name="name">要素名</param>
        /// <returns></returns>
        public XmlElement this[string name]
        {
            get { return GetElement(name); }
        }

        /// <summary>
        /// 子エレメントリストを取得します。
        /// </summary>
        protected List<XmlElement> Children
        {
            get { return _children; } private set { _children = value; }
        }
        #endregion

        #region 要素の操作

        /// <summary>
        /// 子XmlElementを追加します。
        /// </summary>
        /// <param name="elements"></param>
        public void AddElements(params XmlElement[] elements)
        {
            if (_value != "")
                throw new InvalidOperationException("要素がオーバーラップします。子の要素には既にデータ（" + Value + "）が含まれるため、エレメントを追加すると整形式XML文書となりません");
            Children.AddRange(elements);
        }

        /// <summary>
        /// 子エレメントを削除します。
        /// </summary>
        /// <param name="elements"></param>
        public void RemoveElements(params XmlElement[] elements)
        {
            foreach (XmlElement element in elements)
                Children.Remove(element);
        }

        /// <summary>
        /// 全ての子エレメントを返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<XmlElement> GetChildren()
        {
            foreach (XmlElement element in Children)
                yield return element;
        }

        /// <summary>
        /// 指定した名称の要素を全て返す反復子を取得します。
        /// </summary>
        /// <param name="name">取得対象とする要素名。\区切りによるXPathの指定も可能です。</param>
        /// <param name="attributes">可変数の属性指定</param>
        /// <returns>条件に一致する要素</returns>
        public IEnumerable<XmlElement> GetElements(string name, params XmlAttribute[] attributes)
        {
            string[] path = name.Split('\\');
            foreach (XmlElement child in Children)
            {
                if (child.Name == path[0])
                {
                    if (path.Length != 1)
                    {
                        string nameChild = "";
                        for (int i = 1; i < path.Length; i++) nameChild += "\\" + path[i];
                        nameChild = nameChild.TrimStart('\\');

                        foreach (XmlElement hit in child.GetElements(nameChild, attributes))
                            yield return hit;
                    }
                    else
                    {
                        bool isHit = true;
                        foreach (XmlAttribute attribute in attributes)
                        {
                            if (child.GetAttribute(attribute.Name) != attribute.Value)
                            {
                                isHit = false;
                                break;
                            }
                        }
                        if (isHit) yield return child;
                    }
                }
            }
        }

        /// <summary>
        /// 指定した名称の要素のうち、最初に見つかった要素を取得します。見つからない場合、nullを返します。
        /// </summary>
        /// <param name="name">取得対象とする要素名。\区切りによるXPathの指定も可能です。</param>
        /// <param name="attributes">可変数の属性指定</param>
        /// <returns></returns>
        public XmlElement GetElement(string name, params XmlAttribute[] attributes)
        {
            foreach (XmlElement element in GetElements(name, attributes))
                return element;
            return null;
        }

        #endregion

        #region 属性の操作

        /// <summary>
        /// 指定の属性の値を取得します。属性が見つからない場合、nullを返します。
        /// </summary>
        /// <param name="name">取得対象の属性名</param>
        /// <returns></returns>
        public string GetAttribute(string name)
        {
            return GetAttribute(name, null);
        }

        /// <summary>
        /// 指定の属性の値を取得します。属性が見つからない場合、指定デフォルト値を取得します。
        /// </summary>
        /// <param name="name">取得対象の属性名</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns></returns>
        public string GetAttribute(string name, string defaultValue)
        {
            if (_attributes.ContainsKey(name))
                return _attributes[name].Value;
            else
                return defaultValue;
        }

        /// <summary>
        /// 指定属性を追加します。
        /// </summary>
        /// <param name="name">属性名。既存名が指定された場合、上書きされます。</param>
        /// <param name="value">属性の値</param>
        public void AddAttribute(string name, string value)
        {
            if (_attributes.ContainsKey(name))
                _attributes.Remove(name);
            _attributes.Add(name, new XmlAttribute(name, value));
        }

        /// <summary>
        /// 属性を追加します。
        /// </summary>
        /// <param name="attributes">追加する可変数の属性</param>
        public void AddAttributes(params XmlAttribute[] attributes)
        {
            foreach (XmlAttribute attribute in attributes)
            {
                if (_attributes.ContainsKey(attribute.Name))
                    _attributes.Remove(attribute.Name);
                _attributes.Add(attribute.Name, attribute);
            }
        }

        /// <summary>
        /// 指定名の属性を削除します。
        /// </summary>
        /// <param name="names"></param>
        public void RemoveAttributes(params string[] names)
        {
            foreach (string name in names)
            {
                if (_attributes.ContainsKey(name))
                    _attributes.Remove(name);
            }
        }

        #endregion

        /// <summary>
        /// System.Xml.XmlElementに変換します。
        /// </summary>
        /// <returns></returns>
        internal Adaptee.XmlElement ConvertToSystemXmlElement()
        {
            Adaptee.XmlDocument xmlDoc = new Adaptee.XmlDocument();

            Adaptee.XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDec);

            return ConvertToSystemXmlElement(xmlDoc);
        }

        /// <summary>
        /// System.Xml.XmlElementに変換します。
        /// </summary>
        /// <returns></returns>
        internal Adaptee.XmlElement ConvertToSystemXmlElement(Adaptee.XmlDocument xmlDoc)
        {
            Adaptee.XmlElement xmlElement = xmlDoc.CreateElement(Name);

            // 属性の追加
            foreach (string name in _attributes.Keys)
            {
                Adaptee.XmlAttribute attribute = xmlDoc.CreateAttribute(name);
                attribute.Value = _attributes[name].Value;
                xmlElement.Attributes.Append(attribute);
            }

            if (_value != "")
            {                
                xmlElement.AppendChild(xmlDoc.CreateTextNode(_value));
            }
            else
            {
                foreach (XmlElement element in Children)
                {
                    Adaptee.XmlElement childElement = element.ConvertToSystemXmlElement(xmlDoc);
                    xmlElement.AppendChild(childElement);
                }
            }
            return xmlElement;
        }

        /// <summary>
        /// このXml要素を表すXml形式の文字列を取得します。
        /// </summary>
        /// <returns></returns>
        public string GetXmlText()
        {
            Adaptee.XmlDocument xmlDoc = new Adaptee.XmlDocument();

            Adaptee.XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDec);

            Adaptee.XmlElement xml = ConvertToSystemXmlElement(xmlDoc);

            return XmlFile.TidyUp(xml.OuterXml);
        }

        public override string ToString()
        {
            return GetXmlText();
        }

    }
}
