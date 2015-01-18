using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Sio.Xml;

namespace GoodSeat.Sio.Xml.Serialization
{
	/// <summary>
	/// シリアライズ可能なクラスのインターフェイスを表します。
	/// </summary>
	public interface ISerializable
	{
		/// <summary>
		/// 指定したXmlElementから状態を復元します。
		/// </summary>
		/// <param name="xmlElement">復元元Xml要素</param>
		void OnDeserialize(XmlElement xmlElement);

		/// <summary>
		/// 状態をXmlElementに保存します。
		/// </summary>
		/// <param name="xmlElement">保存先のXmlElement</param>
		void OnSerialize(XmlElement xmlElement);
	}
}
