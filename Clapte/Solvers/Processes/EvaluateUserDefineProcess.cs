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
using GoodSeat.Liffom;
using GoodSeat.Liffom.Parse;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Clapte.Models.Formulas;

namespace GoodSeat.Clapte.Solvers.Processes
{
    /// <summary>
    /// ユーザー定義の定数・関数をシステム定義まで落とす処理を表します。
    /// </summary>
    public class EvaluateUserDefineProcess : Process
    {
        /// <summary>
        /// ユーザー定義の定数・関数をシステム定義まで落とす処理を初期化します。
        /// </summary>
        /// <param name="owner">処理の保持者となるソルバ。</param>
        public EvaluateUserDefineProcess(Solver owner) 
            : base(owner) 
        {
            UserConstantCache = new Dictionary<Variable, Formula>();
            UserFunctionCache = new Dictionary<UserFunction,Formula>();

            CustomDefineConstants = new List<ConstantDefine>();
            CustomDefineFunctions = new List<FunctionDefine>();
        }

        /// <summary>
        /// ユーザー定義定数の評価数式キャッシュマップを設定もしくは取得します。
        /// </summary>
        Dictionary<Variable, Formula> UserConstantCache { get; set; }

        /// <summary>
        /// ユーザー定義関数の評価数式キャッシュマップを設定もしくは取得します。
        /// </summary>
        Dictionary<UserFunction, Formula> UserFunctionCache { get; set; }

        /// <summary>
        /// 追加で使用可能とする定数定義リストを設定もしくは取得します。
        /// </summary>
        public List<ConstantDefine> CustomDefineConstants { get; set; }

        /// <summary>
        /// 追加で使用可能とする関数定義リストを設定もしくは取得します。
        /// </summary>
        public List<FunctionDefine> CustomDefineFunctions { get; set; }

        /// <summary>
        /// 入力された文字列を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力文字列。</param>
        /// <param name="onlyCheckInput">数式の構文解析のみを目的とし、文字列のチェックのみを行うか否か。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckInputText(ref string input, bool onlyCheckInput)
        {
            var parser = Owner.Parser;
            var funcLexer = parser.GetLexerOf<FunctionLexer>();
            if (funcLexer == null) return null;

            funcLexer.ClearUserFunction();

            foreach (var def in GetAllValidFunctionDefines())
            {
                var userFunc = def.Target as UserFunction;
                if (userFunc == null) continue;
                if (funcLexer.ContainsUserFunction(userFunc)) continue;

                funcLexer.AddUserFunction(userFunc);
            }
            return null;
        }

        /// <summary>
        /// 計算対象となった入力数式を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckInputFormula(ref Formula input) 
        { 
            UserConstantCache.Clear();
            UserFunctionCache.Clear();

            // 変数がただ1つだけある等式なら、方程式とみなす
            if (input is Equal && input.GetExistFactors<Variable>().Count() == 1) return null;

            var callStack = new Stack<Formula>();

            try
            {
                foreach (Variable v in input.GetExistFactors<Variable>())
                {
                    var error = EvaluateConstant(v, callStack);
                    if (error != null) return error;

                    var define = UserConstantCache[v];
                    if (define == null) continue;
                    input = input.Substituted(v, new VariableWithDefine(v.Mark, define));
                }
                foreach (UserFunction v in input.GetExistFactors<UserFunction>())
                {
                    var error = EvaluateFunction(v, callStack);
                    if (error != null) return error;

                    var define = UserFunctionCache[v];
                    if (define == null) continue;
                    v.UseFormula = define;
                }
            }
            catch (ClapteProcessException exc)
            {
                return new Error(Error.Level.Message, exc.Message);
            }
            return null;
        }

        /// <summary>
        /// 指定した変数のユーザー定義を探索し、<see cref="UserConstantCache"/>にその定義を記録します。
        /// </summary>
        /// <param name="v">探索対象の変数。</param>
        /// <param name="callStack">定義の探索元の数式スタック。</param>
        /// <returns>検知されたエラー。正常終了の場合、null。</returns>
        /// <exception cref="ClapteProcessException">ユーザー定義定数の定義に不正がある場合にスローされます。</exception>
        private Error EvaluateConstant(Variable v, Stack<Formula> callStack)
        {
            if (UserConstantCache.ContainsKey(v)) return null;

            if (callStack.Contains(v))
                throw new ClapteProcessException(string.Format("ユーザー定義定数「{0}」の定義が不正です。\n定義に無限循環参照が存在します。", v.Mark));

            callStack.Push(v);
            try
            {
                ConstantDefine target = null;
                foreach (var def in GetAllValidConstantDefines())
                {
                    if (def.Name != v.Mark) continue;
                    target = def;
                    break;
                }

                if (target == null) // ユーザー定義定数に定義がない
                {
                    UserConstantCache.Add(v, null);
                    return null;
                }

                if (target.Define != null)
                {
                    Formula define = Owner.Parse(target.Define);
                    define = SubsutitueUserDefines(define, callStack); // 定義内の別のユーザー定義定数・関数を置き換える
                    UserConstantCache.Add(v, define);
                }
                else
                {
                    UserConstantCache.Add(v, null);
                }
                return null;
            }
            catch (FormulaParseException exc)
            {
                throw new ClapteProcessException(string.Format("ユーザー定義定数「{0}」の定義が不正です。\n{1}", v.Mark, exc.Message));
            }
            finally
            {
                callStack.Pop();
            }
        }

        /// <summary>
        /// 指定した関数のユーザー定義を探索し、<see cref="UserFunctionCache"/>にその定義を記録します。
        /// </summary>
        /// <param name="v">探索対象のユーザー定義関数。</param>
        /// <param name="callStack">定義の探索元の数式スタック。</param>
        /// <returns>検知されたエラー。正常終了の場合、null。</returns>
        /// <exception cref="ClapteProcessException">ユーザー定義関数の定義に不正がある場合にスローされます。</exception>
        private Error EvaluateFunction(UserFunction v, Stack<Formula> callStack)
        {
            if (UserFunctionCache.ContainsKey(v)) return null;

            if (callStack.Contains(v))
                throw new ClapteProcessException(string.Format("ユーザー定義関数「{0}」の定義が不正です。\n定義に無限循環参照が存在します。", v.NameForView));

            callStack.Push(v);
            try
            {
                FunctionDefine target = null;
                foreach (var def in GetAllValidFunctionDefines())
                {
                    if (def.Name != v.DistinguishedName) continue;
                    target = def;
                    break;
                }

                if (target == null) // ユーザー定義関数に定義がない
                {
                    UserFunctionCache.Add(v, null);
                    return null;
                }

                if (target.Define != null)
                {
                    Formula define = Owner.Parse(target.Define);
                    define = SubsutitueUserDefines(define, callStack, v.UseVariable); // 定義内の別のユーザー定義定数・関数を置き換える
                    UserFunctionCache.Add(v, define);
                }
                else
                {
                    UserFunctionCache.Add(v, null);
                }
                return null;
            }
            catch (FormulaParseException exc)
            {
                throw new ClapteProcessException(string.Format("ユーザー定義関数「{0}」の定義が不正です。\n{1}", v.NameForView, exc.Message));
            }
            finally
            {
                callStack.Pop();
            }
        }

        /// <summary>
        /// 指定数式中のユーザー定義定数及び関数を、システム定義のレベルまで置換します。
        /// </summary>
        /// <param name="define">対象の数式。</param>
        /// <param name="callStack">定義の探索元の数式スタック。</param>
        /// <param name="ignoreVariables">対象外とする変数リスト。</param>
        /// <returns>置換された数式。</returns>
        private Formula SubsutitueUserDefines(Formula define, Stack<Formula> callStack, List<Variable> ignoreVariables = null)
        {
            foreach (var inner in define.GetExistFactors<Variable>())
            {
                if (ignoreVariables != null && ignoreVariables.Contains(inner)) continue;
                EvaluateConstant(inner, callStack);
                if (UserConstantCache[inner] != null) define = define.Substituted(inner, UserConstantCache[inner]);
            }
            foreach (var inner in define.GetExistFactors<UserFunction>())
            {
                EvaluateFunction(inner, callStack);
                if (UserFunctionCache[inner] != null) define = define.Substituted(inner, UserFunctionCache[inner]);
            }
            return define;
        }

        /// <summary>
        /// 利用可能なすべての定数定義を返す反復子を取得します。
        /// </summary>
        protected virtual IEnumerable<ConstantDefine> GetAllValidConstantDefines()
        {
            foreach (var def in CustomDefineConstants) yield return def;
            foreach (var def in Owner.UserConstants) yield return def;
        }

        /// <summary>
        /// 利用可能なすべての定数定義を返す反復子を取得します。
        /// </summary>
        protected virtual IEnumerable<FunctionDefine> GetAllValidFunctionDefines()
        {
            foreach (var def in CustomDefineFunctions) yield return def;
            foreach (var def in Owner.UserFunctions) yield return def;
        }

    }
}

