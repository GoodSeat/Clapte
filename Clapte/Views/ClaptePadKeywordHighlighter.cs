// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sgry.Azuki;
using Sgry.Azuki.Highlighter;
using GoodSeat.Clapte.ViewModels;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Clapte.Models;

namespace GoodSeat.Clapte.Views
{
    /// <summary>
    /// ClaptePadにおけるHighlighterを表します。
    /// </summary>
    public class ClaptePadKeywordHighlighter : KeywordHighlighter
    {
        /// <summary>
        /// ClaptePadにおけるシンタックスハイライトの対象タイプを表します。
        /// </summary>
        public enum SyntaxTarget
        {
            /// <summary>定数を表します。</summary>
            Constant = CharClass.Keyword,
            /// <summary>関数を表します。</summary>
            Function = CharClass.Function,
            /// <summary>単位を表します。</summary>
            Unit = CharClass.Keyword2,
            /// <summary>演算記号を表します。</summary>
            Operator = CharClass.Macro,
            /// <summary>コメントを表します。</summary>
            Comment = CharClass.Comment,
            /// <summary>コメントを表します。</summary>
            Condition = CharClass.DocComment,
            /// <summary>エラーを表します。</summary>
            Error = CharClass.RemovedLine
        }

        /// <summary>
        /// ClaptePadにおけるHighlighterを初期化します。
        /// </summary>
        public ClaptePadKeywordHighlighter(FormulaCellListViewModel target)
        {
            Target = target;
            AddLineHighlight("#", GetCharClassOf(SyntaxTarget.Comment));
            AddLineHighlight("~~~", GetCharClassOf(SyntaxTarget.Condition));
            AddLineHighlight("!!!", GetCharClassOf(SyntaxTarget.Error));
        }

        /// <summary>
        /// 対象のFormulaCellListViewModelオブジェクト。
        /// </summary>
        FormulaCellListViewModel Target { get; set; }

        /// <summary>
        /// 指定対象に関連付けられたシンタックスタイプを取得します。
        /// </summary>
        /// <param name="target">対象の対象タイプ。</param>
        /// <returns>関連付けられたシンタックスタイプ。</returns>
        public CharClass GetCharClassOf(SyntaxTarget target) { return (CharClass)target; }

        /// <summary>
        /// Highlighterの設定を更新します。
        /// </summary>
        public void Renew(string targetText)
        {
            ClearKeywords();
            ClearRegex();

            List<string> constantNames = new List<string>();
            constantNames.AddRange(GetAllConstantsDefinedInCell().Select(def => def.Name).Where(name => !constantNames.Contains(name)));
            constantNames.AddRange(Target.ConstantList.Target.Select(def => def.Name).Where(name => !constantNames.Contains(name)));
            constantNames.AddRange(Target.ConstantList.GetSystemConstants().Select(def => def.Name).Where(name => !constantNames.Contains(name)));
            constantNames.Add(FormulaCellContent.NameOfInputVariable);
            constantNames.Add(FormulaCellContent.NameOfInputRevVariable);
            constantNames.Add(FormulaCellContent.NameOfAnswerVariable);
            constantNames.Add(FormulaCellContent.NameOfAnswerRevVariable);
            constantNames.Sort();
            AddKeywordSet(constantNames.ToArray(), GetCharClassOf(SyntaxTarget.Constant));

            List<string> functionNames = new List<string>();
            functionNames.AddRange(GetAllFunctionDefinedInCell().Select(def => def.Name).Where(name => !functionNames.Contains(name)));
            functionNames.AddRange(Target.FunctionList.Target.Select(def => def.Name).Where(name => !functionNames.Contains(name)));
            functionNames.AddRange(Target.FunctionList.GetSystemFunctions().Select(def => def.Name).Where(name => !functionNames.Contains(name)));
            functionNames.Sort();
            AddKeywordSet(functionNames.ToArray(), GetCharClassOf(SyntaxTarget.Function));

            List<string> operatorNames = new List<string>(){
                "+", "-", "*", "/", "＋", "ー", "×", "÷", "=", "!=", "<", ">", "<=", "≦", ">=", "≧", "!", ",", "@", 
            };
            operatorNames.Sort();
            AddKeywordSet(operatorNames.ToArray(), GetCharClassOf(SyntaxTarget.Operator));

            List<string> unitNames = new List<string>();
            foreach (var table in UnitConvertTable.ValidTables)
            {
                foreach (var name in table.Value.GetAllRecords(true).Where(record => record.ConvertUnit is Unit).Select(record => (record.ConvertUnit as Unit).UnitName.ToString()))
                {
                    var addName = name;
                    if (!unitNames.Contains(addName) && !constantNames.Contains(addName) && targetText.Contains(addName)) unitNames.Add(addName);

                    foreach (var prefix in Prefix.GetAllPrefix(false))
                    {
                        addName = prefix.Mark + name;
                        if (!unitNames.Contains(addName) && !constantNames.Contains(addName) && targetText.Contains(addName)) unitNames.Add(addName);
                    }
                }
            }
            unitNames.Sort();

//            AddKeywordSet(unitNames.ToArray(), GetCharClassOf(SyntaxTarget.Unit));
            foreach (var name in unitNames)
            {
                AddRegex(@"\b(?=\d*)" + name + @"\b", GetCharClassOf(SyntaxTarget.Unit));
            }
        }

        /// <summary>
        /// 数式セルで定義されたすべての定数定義を返す反復子を取得します。
        /// </summary>
        /// <returns>数式セルで定義された定数定義を返す反復子。</returns>
        IEnumerable<ConstantDefine> GetAllConstantsDefinedInCell()
        {
            foreach (var cell in Target)
                foreach (var def in cell.Target.Content.GetAllConstantDefines())
                    if (def != null) yield return def;
        }

        /// <summary>
        /// 数式セルで定義されたすべての関数定義を返す反復子を取得します。
        /// </summary>
        /// <returns>数式セルで定義された関数定義を返す反復子。</returns>
        IEnumerable<FunctionDefine> GetAllFunctionDefinedInCell()
        {
            foreach (var cell in Target)
                foreach (var def in cell.Target.Content.GetAllFunctionDefines())
                    if (def != null) yield return def;
        }
    }
}
