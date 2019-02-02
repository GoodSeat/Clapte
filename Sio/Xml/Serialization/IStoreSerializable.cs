// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Sio.Xml.Serialization
{
    /// <summary>
    /// オブジェクトの参照関係を含めた状態のシリアライズをサポートします。
    /// </summary>
    public interface IStoreSerializable
    {
        /// <summary>
        /// オブジェクトの状態を指定Xmlオブジェクトに保存します。
        /// </summary>
        /// <param name="elem">保存先Xmlオブジェクト。</param>
        /// <param name="store">保存管理ストア。</param>
        void OnSerialize(XmlElement elem, SerializeStore store);

        /// <summary>
        /// オブジェクトの状態を指定Xmlオブジェクトから復元します。
        /// </summary>
        /// <param name="elem">復元元Xmlオブジェクト。</param>
        /// <param name="store">復元管理ストア。</param>
        void OnDeserialize(XmlElement elem, SerializeStore store);
    }
}
