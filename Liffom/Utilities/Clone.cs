using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GoodSeat.Liffom.Utilities
{    
    /// <summary>
    /// オブジェクトのクローン生成処理を提供します。
    /// </summary>
    public static class Clone
    {
        /// <summary>
        /// 指定オブジェクトのディープクローンコピーを取得します。
        /// </summary>
        /// <param name="target">コピー対象の数式。</param>
        /// <returns>ディープコピーされた数式。</returns>
        public static T GetClone<T>(T target) where T : class
        {
            object clone = null;
            using (MemoryStream stream = new MemoryStream())
            {
                //対象オブジェクトをシリアライズ
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, target);
                stream.Position = 0;

                //シリアライズデータをデシリアライズ
                clone = formatter.Deserialize(stream);
            }
            return clone as T;
        }
    }

    
}

