using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Sio.Xml.Serialization
{
    /// <summary>
    /// オブジェクト参照を含むオブジェクトのシリアライズ管理を表します。
    /// </summary>
    public sealed class SerializeStore
    {
        /// <summary>
        /// オブジェクト参照を含むオブジェクトのシリアライズ管理を初期化します。
        /// </summary>
        public SerializeStore()
        {
            Store = new Dictionary<Type, Dictionary<string, IStoreSerializable>>();
            KeyMap = new Dictionary<IStoreSerializable, string>();
        }

        /// <summary>
        /// 内部のオブジェクトストア情報を設定もしくは取得します。
        /// </summary>
        Dictionary<Type, Dictionary<string, IStoreSerializable>> Store { get; set; }

        /// <summary>
        /// 内部のストア済みオブジェクトに対応するキーのマップを設定もしくは取得します。
        /// </summary>
        Dictionary<IStoreSerializable, string> KeyMap { get; set; }

        /// <summary>
        /// 指定オブジェクトをシリアライズしたXmlElementオブジェクトを取得します。
        /// </summary>
        /// <param name="obj">シリアライズ対象のオブジェクト。</param>
        /// <param name="name">シリアライズされたXmlElementにつける名前。</param>
        /// <returns>シリアライズされたXmlElementオブジェクト。</returns>
        public XmlElement GetSerialized(IStoreSerializable obj, string name)
        {
            var elem = new XmlElement(name);
            if (obj == null) return elem;

            var type = obj.GetType();
            elem.AddAttribute("FullNameOfClass", type.FullName);

            var key = KeyOf(obj);
            if (!ContainsKey(type, key))
            {
                obj.OnSerialize(elem, this);
                elem.AddAttribute("_StoreBody_", key);
                RegistKey(type, key, obj);
            }
            else
            {
                elem.AddAttribute("_StoreKey_", key);
            }
            return elem;
        }

        /// <summary>
        /// 指定XmlElementからオブジェクトを復元して取得します。
        /// </summary>
        /// <param name="elem">デシリアライズ対象のXmlElementオブジェクト。</param>
        /// <param name="args">オブジェクトの初期化に用いる引数。</param>
        /// <returns>復元されたオブジェクト。</returns>
        public T GetDeserialized<T>(XmlElement elem, params object[] args)
            where T : class, IStoreSerializable
        {
            var key = elem.GetAttribute("_StoreKey_");
            var body = elem.GetAttribute("_StoreBody_");

            var className = elem.GetAttribute("FullNameOfClass");
            if (className == null) return null;

            Reflector<T> targetReflector = Reflector<T>.GetReflectorOf(className);
            if (targetReflector == null) throw new ArgumentException("指定されたXml要素から状態を復元できません。\n" + className + "というクラス情報が見つかりません。");

            var type = targetReflector.Type;

            if (key != null)
            {
                IStoreSerializable obj;
                if (ContainsKey(type, key, out obj)) return obj as T;
                
                T loadedObject = targetReflector.CreateInstance(args);
                RegistKey(type, key, loadedObject);
                return loadedObject;
            }
            else if (body != null)
            {
                T res = null;

                IStoreSerializable obj;
                if (ContainsKey(type, body, out obj)) res = obj as T;
                else
                {
                    res = targetReflector.CreateInstance(args);
                    RegistKey(type, body, res);
                }

                res.OnDeserialize(elem, this);
                return res;
            }
            else
                throw new InvalidOperationException("このXmlオブジェクトからは状態を復元できません。");
        }


        /// <summary>
        /// 指定オブジェクトが既にストア済みか否かを取得します。
        /// </summary>
        /// <param name="obj">判定対象のオブジェクト。</param>
        /// <returns>既にストア済みか否か。</returns>
        private bool ContainsStore(IStoreSerializable obj)
        {
            Type type = obj.GetType();
            if (!Store.ContainsKey(type)) return false;

            var innerStore = Store[type];
            return innerStore.ContainsValue(obj);
        }

        /// <summary>
        /// 指定オブジェクトのストアキーを取得します。まだストアされていない場合、新規にキーを生成・登録してキーを返します。
        /// </summary>
        /// <param name="obj">判定対象のオブジェクト。</param>
        /// <returns>ストアキー。</returns>
        private string KeyOf(IStoreSerializable obj)
        {
            if (KeyMap.ContainsKey(obj)) return KeyMap[obj];

            Type type = obj.GetType();
            int count = 0;
            if (Store.ContainsKey(type))
            {
                var innerStore = Store[type];
                count = innerStore.Count;
            }
            string key = type.Name + count.ToString();
            KeyMap.Add(obj, key);
            return key;
        }

        /// <summary>
        /// 指定タイプの指定キーにオブジェクトがストア済みか否かを取得します。
        /// </summary>
        /// <param name="type">対象オブジェクトの型。</param>
        /// <param name="key">取得対象オブジェクトのストアキー。</param>
        /// <param name="obj">取得されたオブジェクト。</param>
        /// <returns>ストア済みだったか否か。</returns>
        private bool ContainsKey(Type type, string key, out IStoreSerializable obj)
        {
            obj = null;
            if (!Store.ContainsKey(type)) return false;
            var innerStore = Store[type];

            if (innerStore.ContainsKey(key)) obj = innerStore[key];
            return obj != null; 
        }

        /// <summary>
        /// 指定タイプの指定キーにオブジェクトがストア済みか否かを取得します。
        /// </summary>
        /// <param name="type">対象オブジェクトの型。</param>
        /// <param name="key">取得対象オブジェクトのストアキー。</param>
        /// <returns>ストア済みだったか否か。</returns>
        private bool ContainsKey(Type type, string key)
        {
            IStoreSerializable dummy;
            return ContainsKey(type, key, out dummy);
        }

        /// <summary>
        /// 指定タイプ、指定キーに関連付けてオブジェクトをストアに登録します。
        /// </summary>
        /// <param name="type">対象オブジェクトの型。</param>
        /// <param name="key">対象オブジェクトのストアキー。</param>
        /// <param name="obj">ストア対象のオブジェクト。</param>
        private void RegistKey(Type type, string key, IStoreSerializable obj)
        {
            if (ContainsKey(type, key)) throw new InvalidOperationException();

            Dictionary<string, IStoreSerializable> innerStore = null;
            if (Store.ContainsKey(type)) innerStore = Store[type];
            else
            {
                innerStore = new Dictionary<string, IStoreSerializable>();
                Store.Add(type, innerStore);
            }

            innerStore.Add(key, obj);
        }



    }
}
