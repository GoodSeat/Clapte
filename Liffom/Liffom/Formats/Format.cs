// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Utilities;

namespace GoodSeat.Liffom.Formats
{
    /// <summary>
    /// 数式を文字列に変換する際の書式情報を表します。
    /// </summary>
    [Serializable()]
    public class Format
    {
        static object s_lockDefaultObject = new object();

        /// <summary>
        /// 書式属性と、その書式属性の既定設定のマップを設定もしくは取得します。
        /// </summary>
        static Dictionary<Type, FormatProperty> Default { get; set; }

        static Format()
        {
            Default = new Dictionary<Type, FormatProperty>();
        }

        /// <summary>
        /// 全ての数式で常に書式を確認すべき書式属性として、型を登録します。
        /// </summary>
        /// <remarks>
        /// ここに登録された書式属性については、個々の書式情報に設定がない場合にも常に確認され、
        /// 個々の設定がない場合にはシステム既定の設定が適用されます。
        /// 例えば数式括弧の付加など、すべての数式で必ず確認すべき書式属性について、登録してください。
        /// </remarks>
        public static void RegistPermanentApply<T>() where T : FormatProperty
        {
            var type = typeof(T);

            // システムの既定設定に登録
            lock (s_lockDefaultObject)
            {
                if (!Default.ContainsKey(type))
                {
                    var reflector = new Reflector<T>(type);
                    var defaultProperty = reflector.CreateInstance();
                    Default.Add(type, defaultProperty);
                }
            }
        }

        /// <summary>
        /// ユーザー指定による書式の既定設定を設定します。
        /// </summary>
        /// <param name="property">設定する書式属性。</param>
        public static void SetDefaultProperty(FormatProperty property)
        {
            var type = property.GetType();
            if (Default.ContainsKey(type)) Default.Remove(type);
            Default.Add(type, property);
        }

        /// <summary>
        /// 指定書式属性の既定設定を取得します。
        /// </summary>
        /// <param name="type">取得対象の書式属性。</param>
        private static FormatProperty DefaultPropertyOf(Type type)
        {
            // システムの既定設定（既定コンストラクタで初期化した状態）
            lock (s_lockDefaultObject)
            {
                if (!Default.ContainsKey(type))
                {
                    var reflector = new Reflector<FormatProperty>(type);
                    var defaultProperty = reflector.CreateInstance();
                    Default.Add(type, defaultProperty);
                }
            }
            return Default[type] as FormatProperty;
        }

        /// <summary>
        /// 指定書式属性の既定設定を取得します。
        /// </summary>
        private static T DefaultPropertyOf<T>() where T : FormatProperty
        {
            var type = typeof(T);
            return DefaultPropertyOf(type) as T;
        }

        /// <summary>
        /// 数式を文字列に変換する際の書式情報を初期化します。
        /// </summary>
        public Format() 
        {
            IndividualSetting = new Dictionary<Type, FormatProperty>();
        }


        [NonSerialized()]
        Formula _target;

        [NonSerialized()]
        Format _parent;

        /// <summary>
        /// この書式情報に設定された個別の書式設定マップを設定もしくは取得します。
        /// </summary>
        Dictionary<Type, FormatProperty> IndividualSetting { get; set; }

        /// <summary>
        /// この書式情報の対象となる数式を取得します。
        /// </summary>
        public Formula Target 
        {
            get { return _target; }
            internal set { _target = value; }
        }

        /// <summary>
        /// この書式情報の親となる書式情報を取得します。
        /// </summary>
        public Format Parent 
        {
            get { return _parent; }
            internal set { _parent = value; }
        }



        /// <summary>
        /// 指定した書式属性に関して、個別の設定が含まれるか否かを取得します。
        /// </summary>
        public bool HasIndividualSettingOf<T>() where T : FormatProperty
        {
            var type = typeof(T);
            return IndividualSetting.ContainsKey(type);
        }

        /// <summary>
        /// 指定書式属性を設定します。
        /// </summary>
        /// <param name="property">設定する書式属性。</param>
        public virtual void SetProperty(FormatProperty property)
        {
            var type = property.GetType();
            if (IndividualSetting.ContainsKey(type)) IndividualSetting.Remove(type);

            IndividualSetting.Add(type, property);
        }

        /// <summary>
        /// 指定した書式属性の個別設定を削除します。
        /// </summary>
        public void RemoveIndividualSettingOf<T>() where T : FormatProperty
        {
            var type = typeof(T);
            if (IndividualSetting.ContainsKey(type)) IndividualSetting.Remove(type);
        }

        /// <summary>
        /// 指定書式属性の設定を取得します。
        /// </summary>
        public T PropertyOf<T>() where T : FormatProperty
        {
            var type = typeof(T);
            var def = DefaultPropertyOf<T>();

            if (HasIndividualSettingOf<T>()) return IndividualSetting[type] as T;
            else if (def.Inheritable && Parent != null) return Parent.PropertyOf<T>() as T;
            else return def;
        }


        /// <summary>
        /// 書式情報に基づいて、数式の出力文字列を修正します。
        /// </summary>
        /// <param name="baseText">変更前の出力文字列。</param>
        /// <returns>書式修正後の文字列。</returns>
        public string ModifyOutputText(string baseText)
        {
            foreach (var setting in GetAllValidSetting())
                baseText = setting.OnModifyOutputText(this, baseText);
            return baseText;
        }

        /// <summary>
        /// この書式情報において、有効となるすべての書式属性を返す反復子を取得します。
        /// </summary>
        public IEnumerable<FormatProperty> GetAllValidSetting()
        {
            List<Type> already = new List<Type>();

            foreach (var setting in GetAllExistSetting(false))
            {
                var type = setting.GetType();
                if (already.Contains(type)) continue;
                already.Add(type);

                yield return setting;
            }
        }

        /// <summary>
        /// この書式情報、及び親の書式情報において、設定の存在するすべての書式属性を返す反復子を取得します。
        /// </summary>
        /// <param name="searchInherit">継承された書式情報を探すか否か。</param>
        /// <returns>すべての書式属性を返す反復子。</returns>
        private IEnumerable<FormatProperty> GetAllExistSetting(bool searchInherit)
        {
            foreach (var setting in IndividualSetting.Values)
            {
                if (searchInherit && !setting.Inheritable) continue;
                yield return setting;
            }

            if (Parent != null)
            {
                foreach (var setting in Parent.GetAllExistSetting(true))
                    yield return setting;
            }
            else
            {
                foreach (var property in Default.Values)
                    yield return property;
            }
        }


    }
}
