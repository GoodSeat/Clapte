using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formats.Powers;
using GoodSeat.Liffom.Utilities;

namespace GoodSeat.Liffom.Formulas.Units
{
    /// <summary>
    /// 単位接頭辞を表します。
    /// </summary>
    [Serializable()]
    public abstract class Prefix
    {
        static List<Prefix> s_prefixList;

        /// <summary>
        /// 存在する全ての接頭辞系の既定接頭辞を取得します。
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Prefix> GetAllBasePrefix()
        {
            if (s_prefixList == null)
            {
                s_prefixList = new List<Prefix>();
                foreach (Reflector<Prefix> info in Reflector<Prefix>.FindReflectors())
                {
                    s_prefixList.Add(info.CreateInstance());
                }
            }
            foreach (Prefix prefix in s_prefixList) yield return prefix;
        }

        /// <summary>
        /// 存在する全ての接頭辞を取得します。
        /// </summary>
        /// <param name="containNull">NullPrefixを含めるか否か</param>
        /// <returns></returns>
        public static IEnumerable<Prefix> GetAllPrefix(bool containNull)
        {
            if (containNull) yield return new NullPrefix();

            foreach (Prefix p in GetAllBasePrefix())
                foreach (Prefix prefix in p.GetAllPrefixs())
                    yield return prefix;
        }

        /// <summary>
        /// 指定文字列から生成される接頭辞を取得します。該当がない場合、NullPrefixを返します。
        /// </summary>
        /// <param name="mark">探索文字列</param>
        /// <returns></returns>
        public static Prefix GetPrefixFrom(string mark)
        {
            foreach (Prefix prefix in GetAllPrefix(false))
            {
                if (prefix.Mark == mark) return prefix;
            }
            return new NullPrefix();
        }


        /// <summary>
        /// 指定単位接頭辞間の変換倍率を取得します。
        /// </summary>
        /// <param name="from">変換元となる接頭辞。</param>
        /// <param name="to">変換先となる接頭辞。</param>
        /// <returns>変換倍率。</returns>
        public static Formula GetModify(Prefix from, Prefix to)
        {
            Formula modify = 1;
            if (from is NullPrefix && to is NullPrefix) return modify;
            else if (from is NullPrefix) modify = new Power(to.Base, -to.Power);
            else if (to is NullPrefix) modify = new Power(from.Base, from.Power);
            else if (from.Base == to.Base) modify = new Power(from.Base, from.Power - to.Power);
            else modify = from.Coefficient / to.Coefficient;

            modify = modify.Combine();
            modify.Format.SetProperty(new DivisionFormatProperty(false));
            return modify;
        }

        /// <summary>
        /// 全ての接頭辞表記文字列を返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<Prefix> GetAllPrefixs();

        /// <summary>
        /// 単位接頭辞の表記文字列を取得します。
        /// </summary>
        public abstract string Mark { get; }

        /// <summary>
        /// 接頭辞の呼び名を取得します。
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 単位接頭辞の基数を取得します。
        /// </summary>
        public abstract double Base { get; }

        /// <summary>
        /// 接頭辞の指数値を取得します。
        /// </summary>
        public abstract double Power { get; }

        /// <summary>
        /// 接頭辞による数値の変換率を取得します。
        /// </summary>
        public Formula Coefficient { get { return new Power(Base, Power); } }

        /// <summary>
        /// 指定接頭辞への変換倍率を取得します。
        /// </summary>
        /// <param name="to">変換先となる接頭辞。</param>
        /// <returns>変換倍率。</returns>
        public Formula GetModify(Prefix to) { return GetModify(this, to); }

        public override string ToString() { return Mark; }
    }

    /// <summary>
    /// 接頭辞不在を表します。
    /// </summary>
    [Serializable()]
    public class NullPrefix : Prefix
    {
        public override IEnumerable<Prefix> GetAllPrefixs()
        {
            yield return new NullPrefix();
        }

        public override string Mark
        {
            get { return ""; }
        }

        public override string Name
        {
            get { return ""; }
        }

        public override double Base
        {
            get { return 1.0; }
        }

        public override double Power
        {
            get { return 0.0; }
        }
    }



}
