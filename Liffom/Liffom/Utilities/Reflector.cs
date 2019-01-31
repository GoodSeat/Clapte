// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace GoodSeat.Liffom.Utilities
{
    /// <summary>
    /// リフレクタ情報を提供します。
    /// </summary>
    /// <typeparam name="T">対象の型</typeparam>
    [Serializable()]
    public class Reflector<T> where T : class
    {
        static List<Reflector<T>> s_loadedReflectors;
        static List<string> s_folders = new List<string>();

        /// <summary>
        /// 指定クラスに関するリフレクタ情報を取得します。
        /// </summary>
        /// <param name="fullName">取得対象クラスのフルネーム</param>
        /// <param name="folderName">取得対象アプリケーションフォルダ名</param>
        /// <returns></returns>
        public static Reflector<T> GetReflectorOf(string fullName, params string[] folderNames)
        {
            if (s_loadedReflectors == null) s_loadedReflectors = new List<Reflector<T>>(FindReflectors());
            foreach (string folderName in folderNames)
            {
                if (!s_folders.Contains(folderName))
                {
                    s_loadedReflectors.AddRange(FindReflectors(folderName));
                    s_folders.Add(folderName);
                }
            }

            foreach (Reflector<T> reflect in s_loadedReflectors)
            {
                if (reflect.ClassName == fullName)
                    return reflect;
            }

            // ジェネリッククラスに対応
            Type type = typeof(T);
            if (type.FullName == fullName) return new Reflector<T>(type);

            return null;
        }

        /// <summary>
        /// 実行可能ファイルと同ディレクトリ内で、有効なクラス情報を取得して返します。
        /// </summary>
        /// <returns></returns>
        public static Reflector<T>[] FindReflectors()
        {
            return FindReflectors("");
        }

        /// <summary>
        /// 指定されたフォルダ内から有効なクラスを探索し、それらを格納したクラス情報を返します
        /// </summary>
        /// <param name="folderName">探索フォルダを表すアプリケーションフォルダ名</param>
        /// <param name="type">クラスの型</param>
        /// <returns></returns>
        public static Reflector<T>[] FindReflectors(string folderName)
        {
            List<Reflector<T>> plugins = new List<Reflector<T>>();

            Type[] types = new Type[] { typeof(T) };
            
            //プラグインフォルダ
            string folder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            folder += "\\" + folderName;

            if (!Directory.Exists(folder)) throw new ApplicationException("プラグインフォルダ\"" + folder + "\"が見つかりませんでした。");

            // *.dll及び*.exeファイルを探索対象とする
            List<string> targetPaths = new List<string>();
            targetPaths.AddRange(Directory.GetFiles(folder, "*.dll"));
            targetPaths.AddRange(Directory.GetFiles(folder, "*.exe"));

            try
            {
                foreach (string path in targetPaths)
                {
                    // アセンブリとして読み込む
                    Assembly asm = Assembly.LoadFrom(path);

                    foreach (Type t in asm.GetTypes())
                    {
                        try
                        {
                            // クラスのフルネームが一致すればOK
                            foreach (Type targetType in types)
                            {
                                if (t.FullName == targetType.FullName && !t.IsAbstract)
                                {
                                    plugins.Add(new Reflector<T>(t));
                                    continue;
                                }
                            }
                            if (t.IsClass && t.IsPublic && !t.IsAbstract && HasCorrectInterface(t, types))
                                plugins.Add(new Reflector<T>(t));
                        }
                        catch
                        { }
                    }
                }
            }
            catch { }

            //コレクションを配列にして返す
            return plugins.ToArray();
        }

        /// <summary>
        /// 指定した型が、指定した名前のインターフェイスを全て実装しているかを返します
        /// </summary>
        /// <param name="t">検証する型</param>
        /// <param name="IName">実装検証する型名</param>
        /// <returns></returns>
        protected static bool HasCorrectInterface(Type t, params string[] IName)
        {
            bool ok = true;

            foreach (string name in IName)
            {
                if (t.GetInterface(name) == null)
                {
                    ok = false;
                    break;
                }
            }

            return ok;
        }

        /// <summary>
        /// 指定した型が、指定した型を全て継承しているかを返します
        /// </summary>
        /// <param name="t">検証する型</param>
        /// <param name="type">継承検証する型</param>
        /// <returns></returns>
        protected static bool HasCorrectInterface(Type t, params Type[] type)
        {
            bool ok = true;
            foreach (Type tp in type)
            {
                if (!t.IsSubclassOf(tp))
                {
                    ok = false;
                    break;
                }
            }
            return ok;
        }


        protected Type _type;

        /// <summary>
        /// リフレクション情報を初期化します。
        /// </summary>
        /// <param name="type">クラスの型</param>
        public Reflector(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// クラスのフルネームを取得します。
        /// </summary>
        public string ClassName { get { return _type.FullName; } }

        /// <summary>
        /// クラスの型を取得します。
        /// </summary>
        public Type Type { get { return _type; } }

        /// <summary>
        /// プラグインクラスのインスタンスを作成します
        /// </summary>
        /// <param name="args">コンストラクタの引数</param>
        /// <returns>プラグインクラスのインスタンス</returns>
        public virtual T CreateInstance(params object[] args)
        {
            try
            {
                //クラス名からインスタンスを作成する
                T obj = Activator.CreateInstance(Type, BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, args, null) as T;
                return obj;
            }
            catch
            {
                return null;
            }
        }

    }
}
