using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Sio.Xml.Serialization
{
    /// <summary>
    /// シリアル化に関するイベントデータを格納するクラスです。
    /// </summary>
    public class SerializeEventArgs<T> : EventArgs
    {
        T _target;

        /// <summary>
        /// シリアル化に関するイベントデータを初期化します。
        /// </summary>
        /// <param name="target"></param>
        public SerializeEventArgs(T target)
        {
            _target = target;
        }

        /// <summary>
        /// シリアル化対象のオブジェクトを取得します。
        /// </summary>
        public T TargetObject
        {
            get { return _target; }
        }

    }
}
