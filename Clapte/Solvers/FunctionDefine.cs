// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;

namespace GoodSeat.Clapte.Solvers
{
    /// <summary>
    /// 関数の定義を表します。
    /// </summary>
    public class FunctionDefine
    {
        /// <summary>
        /// 関数定義を初期化します。
        /// </summary>
        public FunctionDefine() { }

        /// <summary>
        /// 関数定義を初期化します。
        /// </summary>
        /// <param name="target">定義対象の関数。</param>
        public FunctionDefine(Function target)
        {
            Target = target;
        }

        /// <summary>
        /// 定義対象となる関数を設定もしくは取得します。
        /// </summary>
        public Function Target { get; set; }

        /// <summary>
        /// 定義対象となる関数名を設定もしくは取得します。
        /// </summary>
        public string Name
        {
            get { return Target.DistinguishedName; }
            set
            {
                var userFunction = Target as UserFunction;
                if (userFunction == null) return;

                userFunction.Name = value;
            }
        }

        private string _comment;

        /// <summary>
        /// 定義対象の関数の説明文を設定もしくは取得します。
        /// </summary>
        public string Information 
        {
            get
            {
                var inf = _comment;
                if (!(Target is UserFunction))
                {
                    List<string> arg;
                    inf = Target.GetInformation(out arg);
                }
                return string.IsNullOrEmpty(inf) ? "" : inf;
            }
            set
            {
                _comment = value;
            }
        }

        /// <summary>
        /// 定義対象の関数の引数の説明文リストを設定もしくは取得します。
        /// </summary>
        public List<string> ArgumentInformation
        {
            get
            {
                List<string> arg;
                Target.GetInformation(out arg);

                if (Target is UserFunction)
                {
                    var userFunction = Target as UserFunction;
                    for (int i = arg.Count; i < userFunction.UseVariable.Count; i++)
                    {
                        arg.Add(userFunction.UseVariable[i].ToString());
                    }
                }
                return arg;
            }
            set
            {
                var userFunction = Target as UserFunction;
                if (userFunction == null) return;

                for (int i = 0; i < Math.Min(value.Count, userFunction.UseVariable.Count); i++)
                {
                    Variable v = userFunction.UseVariable[i];
                    if (userFunction.ArgumentInformation.ContainsKey(v))
                        userFunction.ArgumentInformation[v] = value[i];
                    else
                        userFunction.ArgumentInformation.Add(v, value[i]);
                }
            }
        }

        /// <summary>
        /// 関数の値の定義を設定もしくは取得します。
        /// </summary>
        public string Define
        {
            get;
            set;
        }
    }
}
