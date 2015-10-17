using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Adaptee = System.Xml;

namespace GoodSeat.Sio.Xml
{
	/// <summary>
	/// Xml形式ファイルの操作を表します。
	/// </summary>
	public class XmlFile
	{
		/// <summary>
		/// Xml要素の文字列形式を、等価なXmlElementリストに変換します。
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static List<XmlElement> Parse(string s)
		{
			List<XmlElement> result = new List<XmlElement>();
			try
			{
				if (!s.ToLower().Contains("xml version="))
				{
					s = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><InnerParse>" + s + "</InnerParse>";
				}
				Adaptee.XmlDocument doc = new Adaptee.XmlDocument();
				doc.LoadXml(s);

				if (doc.DocumentElement.InnerText != doc.DocumentElement.InnerXml)
				{
					Adaptee.XmlNodeList children = doc.DocumentElement.ChildNodes;
					foreach (Adaptee.XmlNode node in children)
					{
						result.Add(GetXmlElement(node));
					}
				}
			}
			catch
			{ }

			return result;
		}

		/// <summary>
		/// 指定したSystem.Xml.XmlNodeを、XmlElementに変換して取得します。
		/// </summary>
		/// <param name="node">変換対象のノード</param>
		/// <returns></returns>
		static XmlElement GetXmlElement(Adaptee.XmlNode node)
		{
			// 属性を取得
			Adaptee.XmlAttributeCollection attributes = node.Attributes;
			List<XmlAttribute> attributeList = new List<XmlAttribute>();
			if (attributes != null)
			{
				foreach (Adaptee.XmlAttribute attribute in attributes)
					attributeList.Add(new XmlAttribute(attribute.Name, attribute.Value));
			}

			Adaptee.XmlNodeList children = node.ChildNodes;
			XmlElement result = new XmlElement(node.Name, attributeList.ToArray());
			if (node.InnerText == node.InnerXml || children.Count == 0)
			{
				result.SetInnerText(node.InnerText);
			}
			else
			{
				foreach (Adaptee.XmlNode child in children)
					result.AddElements(GetXmlElement(child));
			}
			return result;
		}

		/// <summary>
		/// 指定した文字列に対し、等価なXmlElementリストへの変換を試みます。
		/// </summary>
		/// <param name="s">試行対象の文字列</param>
		/// <param name="result">成功した場合、変換結果</param>
		/// <returns></returns>
		public static bool TryParse(string s, out List<XmlElement> result)
		{
			bool suc = false;
			result = null;
			try
			{
				result = Parse(s);
				suc = (result.Count != 0);
			}
			catch { suc = false; }
			return suc;
		}

		/// <summary>
		/// 指定文字列をXml形式として整形して取得します。
		/// </summary>
		/// <param name="s">整形対象の文字列</param>
		/// <returns></returns>
		public static string TidyUp(string s)
		{
			StringBuilder sb = new StringBuilder(s);
			int tabCount = 1;
			bool inExit = false;

			for (int i = 2; i < sb.Length - 1; i++)
			{
				if (sb[i] == '<' && sb[i + 1] == '/' && sb[i - 1] != '>')
				{
					tabCount--;
				}

				if (sb[i - 1] == '>') // && sb[i - 2] != '?')
				{
					if (sb[i - 2] == '?') tabCount--;
					if (sb[i - 2] == '/') tabCount--;

					if (sb[i] == '<' && sb[i + 1] == '/')
					{
						sb.Insert(i, "\r\n".ToCharArray());
						i++;

						if (!inExit) tabCount--;
						for (int t = 0; t < tabCount; t++) sb.Insert(i + 1 + t, '\t');

						inExit = true;
					}
					else if (sb[i] == '<')
					{
						sb.Insert(i, "\r\n".ToCharArray());
						i++;

						if (inExit) tabCount++;
						for (int t = 0; t < tabCount; t++) sb.Insert(i + 1 + t, '\t');
						tabCount++;

						inExit = false;
					}
				}
			}

			return sb.ToString();
		}


		string _path;
		XmlElement _element;

		/// <summary>
		/// Xml形式のファイル操作を初期化します。
		/// </summary>
		/// <param name="filePath">操作対象のファイルパス</param>
		public XmlFile(string filePath)
		{
			_path = filePath;
		}

		/// <summary>
		/// Xml形式のファイル操作を初期化します。
		/// </summary>
		/// <param name="filePath">操作対象のファイルパス</param>
		/// <param name="targetElement">保存対象のXmlElement</param>
		public XmlFile(string filePath, XmlElement targetElement)
		{
			_path = filePath;
			_element = targetElement;
		}

		/// <summary>
		/// 対象のXmlElementを設定もしくは取得します。
		/// </summary>
		public XmlElement Element
		{
			get { return _element; }
			set { _element = value; }
		}

		/// <summary>
		/// 操作対象のファイル名を設定もしくは取得します。
		/// </summary>
		public string FilePath
		{
			get { return _path; }
			set { _path = value; }
		}

		/// <summary>
		/// ファイルからXmlElementを読み込みます。
		/// </summary>
		public void Load()
		{
			Adaptee.XmlDocument openDoc = new Adaptee.XmlDocument();
			openDoc.Load(_path);

			_element = GetXmlElement(openDoc.DocumentElement);			
		}

		/// <summary>
		/// XmlElementをファイルに書き込みます。
		/// </summary>
		public void Save()
		{
			if (_element == null) throw new InvalidOperationException("保存対象のXmlElementが指定されていません。");
			
			try
			{
				Adaptee.XmlDocument xmlDoc = new Adaptee.XmlDocument();

				Adaptee.XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
				xmlDoc.AppendChild(xmlDec);

				xmlDoc.AppendChild(_element.ConvertToSystemXmlElement(xmlDoc));

                var writer = new StreamWriter(_path, false, Encoding.Default);
                xmlDoc.Save(writer);
			}
			catch { }
		}

		/// <summary>
		/// 対象のファイルが存在するか否かを取得します。
		/// </summary>
		public bool ExistTargetFile
		{
			get { return File.Exists(FilePath); }
		}
	}
}
