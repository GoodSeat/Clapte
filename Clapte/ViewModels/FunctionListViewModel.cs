// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Sio.Utilities;
using GoodSeat.Sio.Xml;
using GoodSeat.Sio.Xml.Serialization;

namespace GoodSeat.Clapte.ViewModels
{
    /// <summary>
    /// 関数定義リストを管理するモデルビューを表します。
    /// </summary>
    public class FunctionListViewModel : ISerializable
    {
        /// <summary>
        /// 関数定義リストを管理するモデルビューを初期化します。
        /// </summary>
        public FunctionListViewModel()
        {
            Target = new ObservableList<FunctionDefine>();
        }

        List<FunctionDefine> _systemFunctions;
        ObservableList<FunctionDefine> _target;

        /// <summary>
        /// 設定対象となる関数定義リストを設定もしくは取得します。
        /// </summary>
        public ObservableList<FunctionDefine> Target 
        {
            get { return _target; }
            set
            {
                if (_target != null)
                {
                    _target.ItemAdding -= new EventHandler<ListChangeEventArgs<FunctionDefine>>(_target_ItemAdding);
                    _target.ItemAdded -= new EventHandler<ListChangeEventArgs<FunctionDefine>>(_target_ItemAdded);
                    _target.ItemRemoved -= new EventHandler<ListChangeEventArgs<FunctionDefine>>(_target_ItemRemoved);
                }
                _target = value;
                if (_target != null)
                {
                    _target.ItemAdding += new EventHandler<ListChangeEventArgs<FunctionDefine>>(_target_ItemAdding);
                    _target.ItemAdded += new EventHandler<ListChangeEventArgs<FunctionDefine>>(_target_ItemAdded);
                    _target.ItemRemoved += new EventHandler<ListChangeEventArgs<FunctionDefine>>(_target_ItemRemoved);
                }
            }
        }

        /// <summary>
        /// 定義リストに登録された定義が編集されたことを通知します。
        /// </summary>
        /// <param name="defines">変更のあった可変数の定義。</param>
        public void InformDefineChanged(params FunctionDefine[] defines)
        {
            if (DefineChanged != null)
            {
                var e = new ListChangeEventArgs<FunctionDefine>(defines);
                DefineChanged(this, e);
            }
        }

        void _target_ItemAdding(object sender, ListChangeEventArgs<FunctionDefine> e)
        {
            foreach (var define in e.Items)
                foreach (var item in Target)
                    if (item.Name == define.Name)
                        throw new ArgumentException(string.Format("すでに{0}を対象とした関数定義が存在します。", define.Name));
        }

        void _target_ItemAdded(object sender, ListChangeEventArgs<FunctionDefine> e)
        {
            if (DefineAdded != null) DefineAdded(this, e);
        }

        void _target_ItemRemoved(object sender, ListChangeEventArgs<FunctionDefine> e)
        {
            if (DefineRemoved != null) DefineRemoved(this, e);
        }


        /// <summary>
        /// 関数定義が追加されたときに呼び出されます。
        /// </summary>
        public event EventHandler<ListChangeEventArgs<FunctionDefine>> DefineAdded;

        /// <summary>
        /// 関数定義が削除されたときに呼び出されます。
        /// </summary>
        public event EventHandler<ListChangeEventArgs<FunctionDefine>> DefineRemoved;

        /// <summary>
        /// 関数定義が更新されたときに呼び出されます。
        /// </summary>
        public event EventHandler<ListChangeEventArgs<FunctionDefine>> DefineChanged;


        /// <summary>
        /// システム定義の定数定義をすべて返す反復子を取得します。
        /// </summary>
        /// <returns>システム定義の定数定義をすべて返す反復子。</returns>
        public IEnumerable<FunctionDefine> GetSystemFunctions()
        {
            if (_systemFunctions == null)
            {
                _systemFunctions = new List<FunctionDefine>();
                foreach (var reflector in Function.GetEnableFunctionsReflector())
                {
                    var sample = reflector.CreateInstance();
                    if (sample is UserFunction) continue;

                    var def = new FunctionDefine(sample);
                    _systemFunctions.Add(def);
                }
            }
            foreach (var def in _systemFunctions) yield return def;
        }

        /// <summary>
        /// 指定定義のインデックスを取得します。
        /// </summary>
        /// <param name="target">判定対象の定数定義。</param>
        /// <returns>0から始まるインデックス番号。見つからなかった場合、-1。</returns>
        public int IndexOf(FunctionDefine target)
        {
            for (int i = 0; i < Target.Count; i++)
                if (target.Name == Target[i].Name) return i;

            return -1;
        }

        /// <summary>
        /// 指定インデックスの定数定義を設定もしくは取得します。
        /// </summary>
        /// <param name="index">対象のインデックス番号。</param>
        /// <returns>指定インデックスの定数定義。</returns>
        public FunctionDefine this[int index]
        {
            get { return Target[index]; }
            set 
            {
                Target[index] = value;
                InformDefineChanged(Target[index]); // 変更を通知
            }
        }



        #region ISerializable メンバー

        public void OnDeserialize(XmlElement xmlElement)
        {
            Target.Clear();
            foreach (var element in xmlElement.GetElements("UserFunction"))
            {
                FunctionDefine def = new FunctionDefine();

                UserFunction userFunction = new UserFunction();
                userFunction.Name = element.GetAttribute("Name");
                userFunction.Information = element.GetAttribute("Information");
                def.Information = userFunction.Information;
                def.Target = userFunction;

                def.Define = element["Formula"].GetAttribute("Define");
                foreach (var argument in element.GetElements("Arguments\\Argument"))
                {
                    var variable = new Variable(argument.GetAttribute("Name"));
                    var info = argument.GetAttribute("Information", "");
                    userFunction.ArgumentInformation.Add(variable, info);
                    userFunction.UseVariable.Add(variable);
                }
                Target.Add(def);
            }
        }

        public void OnSerialize(XmlElement xmlElement)
        {
            foreach (var def in Target)
            {
                XmlElement element = new XmlElement("UserFunction");
                element.AddAttribute("Name", def.Name);
                element.AddAttribute("Information", def.Information);

                XmlElement formula = new XmlElement("Formula");
                formula.AddAttribute("Define", def.Define);

                XmlElement arguments = new XmlElement("Arguments");
                var userFunction = def.Target as UserFunction;
                foreach (var variable in userFunction.UseVariable)
                {
                    XmlElement argument = new XmlElement("Argument");
                    argument.AddAttribute("Name", variable.ToString());
                    if (userFunction.ArgumentInformation.ContainsKey(variable))
                        argument.AddAttribute("Information", userFunction.ArgumentInformation[variable]);
                    arguments.AddElements(argument);
                }
                element.AddElements(formula, arguments);

                xmlElement.AddElements(element);
            }
        }

        #endregion
    }
}
