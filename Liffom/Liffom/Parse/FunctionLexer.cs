// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Utilities;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 関数の字句解析器を表します。
    /// </summary>
    public class FunctionLexer : Lexer
    {
        static List<string> s_neverMatchStrings;

        static FunctionLexer()
        {
            s_neverMatchStrings = new List<string>();
            s_neverMatchStrings.Add(" ");
            s_neverMatchStrings.Add("　");
            s_neverMatchStrings.Add("\t");
            s_neverMatchStrings.Add("\r");
            s_neverMatchStrings.Add("\n");
        }

        Dictionary<string, Reflector<Function>> _functionMap;
        Dictionary<string, Reflector<Function>> _autoRegistedFunctionMap;
        Dictionary<string, UserFunction> _userFunctionMap;

        /// <summary>
        /// 関数の字句解析器を初期化します。
        /// </summary>
        /// <param name="belongParser">所属する数式構文解析器。</param>
        public FunctionLexer(FormulaParser belongParser) : this(belongParser, false) { }

        /// <summary>
        /// 関数の字句解析器を初期化します。
        /// </summary>
        /// <param name="belongParser">所属する数式構文解析器。</param>
        /// <param name="autoRegistAssembly">アセンブリ内の関数を自動探索して解析可能な関数として登録するか否か。</param>
        public FunctionLexer(FormulaParser belongParser, bool autoRegistAssembly)
        {
            _functionMap = new Dictionary<string, Reflector<Function>>();
            _userFunctionMap = new Dictionary<string, UserFunction>();
            BelongParser = belongParser;
            AutoRegistAssembly = autoRegistAssembly;
        }

        /// <summary>
        /// 所属する構文解析器を設定もしくは取得します。
        /// </summary>
        private FormulaParser BelongParser { get; set; }

        /// <summary>
        /// アセンブリ内の関数を自動探索して解析可能な関数として登録するか否かを設定もしくは取得します。
        /// </summary>
        public bool AutoRegistAssembly { get; set; }

        /// <summary>
        /// 未登録のユーザー定義関数を字句解析結果として認めるかを設定もしくは取得します。
        /// </summary>
        public bool ScanNotDefinedUserFunction { get; set; }

        /// <summary>
        /// 解析可能な関数として、関数の型を登録します。
        /// </summary>
        /// <param name="functionType">追加登録する関数の型。</param>
        public void AddFunction(Type functionType)
        {
            Reflector<Function> addReflector = new Reflector<Function>(functionType);
            var sample = addReflector.CreateInstance();
            if (sample == null) throw new FormulaParseException(string.Format("{0} は有効な関数の型ではありません。", functionType));

            var name = sample.DistinguishedName;
            if (_functionMap.ContainsKey(name)) throw new FormulaParseException(string.Format("{0} はすでに関数名として登録されています。", name));

            _functionMap.Add(name, addReflector);
        }

        /// <summary>
        /// 解析可能な関数として、ユーザー定義関数を追加します。
        /// </summary>
        /// <param name="userFunction">追加するユーザー定義関数。</param>
        public void AddUserFunction(UserFunction userFunction)
        {
            _userFunctionMap.Add(userFunction.Name, userFunction);
        }

        /// <summary>
        /// 指定ユーザー関数と同名の定義が既に登録されているか否かを取得します。
        /// </summary>
        /// <param name="userFunction">判定対象のユーザー定義関数。</param>
        /// <returns>すでに定義が登録されているかい否か。</returns>
        public bool ContainsUserFunction(UserFunction userFunction)
        {
            return _userFunctionMap.ContainsKey(userFunction.Name);
        }

        /// <summary>
        /// 解析可能な関数から、指定したユーザー定義関数を削除します。
        /// </summary>
        /// <param name="userFunction">削除するユーザー定義関数。</param>
        public void RemoveUserFunction(UserFunction userFunction)
        {
            _userFunctionMap.Remove(userFunction.Name);
        }

        /// <summary>
        /// 解析可能な関数から、すべてのユーザー定義関数を削除します。
        /// </summary>
        public void ClearUserFunction() { _userFunctionMap.Clear(); }

        /// <summary>
        /// 解析の対象の関数から、指定の関数の型を削除します。
        /// </summary>
        /// <param name="functionType">登録から削除する関数の型。</param>
        public void RemoveFunction(Type functionType)
        {
            string target = null;
            foreach (var pair in _functionMap)
            {
                if (pair.Value.Type == functionType)
                {
                    target = pair.Key;
                    break;
                }
            }
            if (target != null) _functionMap.Remove(target);
        }

        /// <summary>
        /// 指定した文字列の末尾に、関数名として使用不可能な文字列が含まれるかを判定して取得します。
        /// </summary>
        /// <param name="text">判定対象の文字列。</param>
        /// <returns>関数名として使用可能か。</returns>
        private bool IsPossibleAsFunctionName(string text)
        {
            if (text[0] >= '0' && text[0] <= '9') return false;
            if (text[0] == '(') return false;

            foreach (var s in s_neverMatchStrings)
                if (text.Contains(s)) return false;

            // 最後の一文字単独で検査
            string lastChara = text.Substring(text.Length - 1);
            if (lastChara == "(") return true;

            foreach (var lexer in BelongParser.AllValidLexers)
            {
                if (lexer is FunctionLexer || lexer is VariableLexer) continue;

                var testResult = lexer.Scan(lastChara);
                if (testResult == ScanResult.NeverMatch) continue;

                // 演算記号に使用される文字が含まれていたら変数にならない
                if (lexer is OperatorLexer) return false;

                if (testResult != ScanResult.Match) continue;
                
                // 括弧に一致する文字が含まれていたら変数にならない
                foreach (Token token in lexer.Tokenize(lastChara))
                    if (token is PunctuationToken || token is OperatorToken)
                        return false;
            }
            return true;
        }

        public override Lexer.ScanResult Scan(string text)
        {
            if (!IsPossibleAsFunctionName(text)) return ScanResult.NeverMatch;

            if (text.Contains("("))
            {
                if (!text.EndsWith("(")) return ScanResult.NeverMatch;

                string target = text.Substring(0, text.Length - 1);

                if (ExistHitFunction(target, false)) return ScanResult.Match;
                if (ScanNotDefinedUserFunction) return ScanResult.Match;
                return ScanResult.NoMatch;
            }
            else
            {
                if (ExistHitFunction(text, true)) return ScanResult.NoMatch;
                if (ScanNotDefinedUserFunction) return ScanResult.NoMatch;
                return ScanResult.NeverMatch;
            }
        }

        /// <summary>
        /// 指定名に関連付けられる関数が存在するか否かを取得します。
        /// </summary>
        /// <param name="name">探索する関数名。</param>
        /// <param name="containStartWith">前方一致のみでも一致として判定するか。</param>
        /// <returns>一致する関数が見つかったか否か。</returns>
        public bool ExistHitFunction(string name, bool containStartWith)
        {
            foreach (var userFunction in _userFunctionMap.Keys)
            {
                if (userFunction == name || (containStartWith && userFunction.StartsWith(name))) return true;
            }

            InitializeSystemFunctionMap();

            foreach (var registName in _functionMap.Keys)
            {
                if (registName == name || (containStartWith && registName.StartsWith(name))) return true;
            }
            if (!AutoRegistAssembly) return false;

            foreach (var registName in _autoRegistedFunctionMap.Keys)
            {
                if (registName == name || (containStartWith && registName.StartsWith(name))) return true;
            }
            return false;
        }

        /// <summary>
        /// 指定名に関連付けられる関数を引数なしで初期化して取得します。
        /// </summary>
        /// <param name="name">探索する関数名。</param>
        /// <param name="containStartWith">前方一致のみでも一致として判定するか。</param>
        /// <returns>見つかった引数なしの関数。見つからない場合、null。</returns>
        public Function GetHitFunction(string name, bool containStartWith)
        {
            foreach (var userFunction in _userFunctionMap.Keys)
            {
                if (userFunction == name || (containStartWith && userFunction.StartsWith(name)))
                    return _userFunctionMap[userFunction].Copy() as Function;
            }

            InitializeSystemFunctionMap();

            foreach (var registName in _functionMap.Keys)
            {
                if (registName == name || (containStartWith && registName.StartsWith(name)))
                    return _functionMap[registName].CreateInstance();
            }
            if (!AutoRegistAssembly) return null;

            foreach (var registName in _autoRegistedFunctionMap.Keys)
            {
                if (registName == name || (containStartWith && registName.StartsWith(name)))
                    return _autoRegistedFunctionMap[registName].CreateInstance();
            }
            return null;
        }

        /// <summary>
        /// 必要に応じて、システム関数マップを初期化します。
        /// </summary>
        void InitializeSystemFunctionMap()
        {
            if (AutoRegistAssembly && _autoRegistedFunctionMap == null)
            {
                _autoRegistedFunctionMap = new Dictionary<string, Reflector<Function>>();
                foreach (Reflector<Function> reflector in Function.GetEnableFunctionsReflector())
                {
                    // ユーザー定義関数は除く
                    if (reflector.ClassName == typeof(UserFunction).FullName) continue;
                    Function sample = reflector.CreateInstance();
                    _autoRegistedFunctionMap.Add(sample.DistinguishedName, reflector);
                }
            }
        }


        public override IEnumerable<Token> Tokenize(string text)
        {
            string target = text.Substring(0, text.Length - 1);
            var sample = GetHitFunction(target, false);
            if (sample == null)
            {
                sample = new UserFunction(target);
                (sample as UserFunction).NoCheckArgumentQty = true;
            }

            yield return new FunctionToken(target, sample);

            yield return new ParenthesesToken("(");
        }
    }
}
