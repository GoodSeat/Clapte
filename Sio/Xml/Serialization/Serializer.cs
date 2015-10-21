using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Sio.Xml;

namespace GoodSeat.Sio.Xml.Serialization
{
    /// <summary>
    /// クラスTのシリアライザを表します。
    /// </summary>
    /// <typeparam name="T">シリアライズ対象の型</typeparam>
    public class Serializer<T> where T : class, ISerializable
    {
        /// <summary>
        /// シリアライザを初期化します。
        /// </summary>
        public Serializer() { }

        /// <summary>
        /// オブジェクトがシリアル化される直前に呼び出されます。
        /// </summary>
        public event EventHandler<SerializeEventArgs<T>> ObjectSerializing;

        /// <summary>
        /// オブジェクトがシリアル化された直後に呼び出されます。
        /// </summary>
        public event EventHandler<SerializeEventArgs<T>> ObjectSerialized;

        /// <summary>
        /// オブジェクトが生成された後、状態が復元される直前に呼び出されます。
        /// </summary>
        public event EventHandler<SerializeEventArgs<T>> ObjectDeserializing;

        /// <summary>
        /// オブジェクトが復元された直後に呼び出されます。
        /// </summary>
        public event EventHandler<SerializeEventArgs<T>> ObjectDeserialized;

        /// <summary>
        /// 状態を保存したXmlElementを取得します。
        /// </summary>
        /// <param name="target">保存対象</param>
        /// <param name="name">XmlElementに指定する名称</param>
        /// <returns></returns>
        public virtual XmlElement GetSerializedElementOf(T target, string name)
        {
            XmlElement xmlElement = new XmlElement(name, new XmlAttribute("FullNameOfClass", target.GetType().FullName));

            OnSerializing(target);
            target.OnSerialize(xmlElement);
            OnSerialized(target);

            return xmlElement;
        }

        /// <summary>
        /// 指定したXmlElementから復元されるオブジェクトを取得します。
        /// </summary>
        /// <param name="xmlElement">復元対象のXml要素</param>
        /// <param name="args">復元に使用するコンストラクタの引数</param>
        public virtual T GetDesrializedOf(XmlElement xmlElement, params object[] args)
        {
            string targetName = xmlElement.GetAttribute("FullNameOfClass");
            if (targetName == null) throw new ArgumentException("指定されたXml要素から状態を復元できません。\n\"FullNameOfClass\"属性が見つかりません。");

            Reflector<T> targetReflector = Reflector<T>.GetReflectorOf(targetName);
            if (targetReflector == null) throw new ArgumentException("指定されたXml要素から状態を復元できません。\n" + targetName + "というクラス情報が見つかりません。");

            T loadedObject = targetReflector.CreateInstance(args);

            try
            {
                OnDeserializing(loadedObject);
                loadedObject.OnDeserialize(xmlElement);
                OnDeserialized(loadedObject);
            }
            catch (Exception)
            {
                if (loadedObject is IDisposable) (loadedObject as IDisposable).Dispose();
                throw;
            }
            return loadedObject;
        }

        /// <summary>
        /// ObjectSerializingイベントを呼び出します。
        /// </summary>
        protected virtual void OnSerializing(T target)
        {
            if (ObjectSerializing != null) ObjectSerializing(this, new SerializeEventArgs<T>(target));
        }

        /// <summary>
        /// ObjectSerializedイベントを呼び出します。
        /// </summary>
        protected virtual void OnSerialized(T target)
        {
            if (ObjectSerialized != null) ObjectSerialized(this, new SerializeEventArgs<T>(target));
        }


        /// <summary>
        /// ObjectDeserializingイベントを呼び出します。
        /// </summary>
        protected virtual void OnDeserializing(T target)
        {
            if (ObjectDeserializing != null) ObjectDeserializing(this, new SerializeEventArgs<T>(target));
        }

        /// <summary>
        /// ObjectDeserializedイベントを呼び出します。
        /// </summary>
        protected virtual void OnDeserialized(T target)
        {
            if (ObjectDeserialized != null) ObjectDeserialized(this, new SerializeEventArgs<T>(target));
        }


    }
}
