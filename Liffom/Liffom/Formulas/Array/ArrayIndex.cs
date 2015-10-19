using System;
using System.Collections.Generic;
using System.Text;

namespace GoodSeat.Liffom.Formulas.Array
{
    /// <summary>
    /// 配列内のインデックスを表します。
    /// </summary>
    [Serializable()]
    public class ArrayIndex
    {
        /// <summary>
        /// 配列の次元数を指定して、配列内インデックスを初期化します。
        /// </summary>
        /// <param name="size">次元数</param>
        public ArrayIndex(int size)
        {
            DimensionSize = size;
            Position = new Formula[size];
        }

        Formula[] Position { get; set; }

        /// <summary>
        /// 次元数を取得します。
        /// </summary>
        public int DimensionSize { get; private set; }

        /// <summary>
        /// 指定した次元のインデックスを設定します。
        /// </summary>
        /// <param name="dimension">設定する次元</param>
        /// <param name="position">設定インデックス</param>
        public void SetPosition(int dimension, Formula position)
        {
            Position[dimension - 1] = position;
        }

        /// <summary>
        /// 指定した次元のインデックスを取得します。
        /// </summary>
        /// <param name="dimension">取得する次元</param>
        /// <returns>指定次元のインデックス。設定のない場合、null。</returns>
        public Formula GetPosition(int dimension)
        {
            return Position[dimension - 1];
        }

        /// <summary>
        /// 直列インデックスからインデックスを指定します。
        /// </summary>
        /// <param name="serialIndex">直列インデックス</param>
        /// <param name="size">配列サイズを規定する配列インデックス</param>
        public void SetPositionFromSerialPosition(int serialIndex, ArrayIndex size)
        {
            int nextIndex = 0;

            for (int i = DimensionSize; i >= 1; i--)
            {
                int thisSerial = 1;
                for (int k = 1; k < i; k++) thisSerial *= (int)size.GetPosition(k);

                int index = (serialIndex - 1 - nextIndex) / thisSerial + 1;
                this[i] = index; 

                nextIndex += thisSerial * (index - 1);
            }
        }

        /// <summary>
        /// 指定次元のインデックスを設定もしくは取得します。
        /// </summary>
        /// <param name="dimension">対象とする次元</param>
        /// <returns>指定次元のインテックス。設定のない場合、null。</returns>
        public Formula this[int dimension]
        {
            get { return GetPosition(dimension); }
            set { SetPosition(dimension, value); }
        }

        public override bool Equals(object obj)
        {
            var other = obj as ArrayIndex;
            if (other == null) return false;
            if (DimensionSize != other.DimensionSize) return false;
            for (int i = 1; i <= DimensionSize; i++)
            {
                if (this[i] != other[i]) return false;
            }
            return true;
        }

        public override int GetHashCode() { return ToString().GetHashCode(); }


        public override string ToString()
        {
            string result = "[";
            for (int i = 1; i <= DimensionSize; i++)
                result += this[i].ToString() + ", ";
            return result.TrimEnd(',', ' ') + "]";
        }

    }
}
