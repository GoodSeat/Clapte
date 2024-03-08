// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Clapte.Models;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Clapte.ViewModels.InputSupports
{
    /// <summary>
    /// ClaptePadにおける入力補助候補の列挙クラスを表します。
    /// </summary>
    public class ClaptePadInputSupportEnumerator : IInputSupportEnumerator
    {
        /// <summary>
        /// 候補種類を表します。
        /// </summary>
        public enum CandidateType
        {
            /// <summary>
            /// 定数を候補とします。
            /// </summary>
            Constant = 1,
            /// <summary>
            /// 関数を候補とします。
            /// </summary>
            Function = 2,
            /// <summary>
            /// 単位接頭辞を含まない単位を候補とします。
            /// </summary>
            Unit = 4,
            /// <summary>
            /// 全ての単位接頭辞を含む単位を候補とします。
            /// </summary>
            UnitAllPrefix = 12,
            /// <summary>
            /// 制御キーワードを候補とします。
            /// </summary>
            ControlOperator = 16,
            /// <summary>
            /// 考えられる全てを候補とします。
            /// </summary>
            All = Constant | Function | UnitAllPrefix | ControlOperator
        }


        /// <summary>
        /// ClaptePadにおける入力補助候補の列挙クラスを初期化します。
        /// </summary>
        /// <param name="target">入力補助の提供元情報となるFormulaCellListViewModel。</param>
        public ClaptePadInputSupportEnumerator(FormulaCellListViewModel target)
        {
            Target = target;
            AlsoInfomation = true;
        }

        /// <summary>
        /// 候補列挙の際に参照すべき最大行番号を設定もしくは取得します。
        /// </summary>
        public int CurrentCaretLineNumber { get; set; }

        /// <summary>
        /// 説明文も補完対象として使用するか否かを設定もしくは取得します。
        /// </summary>
        public bool AlsoInfomation { get; set; }

        /// <summary>
        /// 入力補助の提供元情報となるFormulaCellListViewModelを設定もしくは取得します。
        /// </summary>
        FormulaCellListViewModel Target { get; set; }

        private IEnumerable<ConstantDefine> GetAllConstantDefines(string startsWith)
        {
            List<string> considerdNames = new List<string>();
            for (int i = CurrentCaretLineNumber; i >= 0; i--)
            {
                if (Target[i] == null) continue;
                var cell = Target[i].Target;
                if (!cell.Content.IsEvaluateTarget) continue;

                foreach (var def in cell.Content.GetAllConstantDefines().Where(d => d != null && !considerdNames.Contains(d.Name)))
                {
                    considerdNames.Add(def.Name);
                    if (!def.Name.StartsWith(startsWith))
                    {
                        if (!AlsoInfomation || !cell.CommentText.Contains(startsWith)) continue;
                    }

                    def.Information = cell.CacheText; // cell.CommentText.TrimStart(' ', '#');
                    yield return def;
                }
            }

            Func<ConstantDefine, bool> isTarget = def => def != null && def.Name.StartsWith(startsWith) && !considerdNames.Contains(def.Name);
            if (AlsoInfomation) isTarget = def => def != null && !considerdNames.Contains(def.Name) && (def.Name.StartsWith(startsWith) || def.Information.Contains(startsWith));

            foreach (var def in Target.ConstantList.Target.Where(isTarget)) { yield return def; considerdNames.Add(def.Name); }
            foreach (var def in Target.ConstantList.GetSystemConstants().Where(isTarget)) { yield return def; considerdNames.Add(def.Name); }
        }

        private IEnumerable<FunctionDefine> GetAllFunctionDefines(string startsWith)
        {
            List<string> considerdNames = new List<string>();
            for (int i = CurrentCaretLineNumber - 1; i >= 0; i--)
            {
                if (Target[i] == null) continue;
                var cell = Target[i].Target;
                if (!cell.Content.IsEvaluateTarget) continue;

                foreach (var def in cell.Content.GetAllFunctionDefines().Where(d => d != null && !considerdNames.Contains(d.Name)))
                {
                    considerdNames.Add(def.Name);
                    if (!def.Name.StartsWith(startsWith))
                    {
                        if (!AlsoInfomation || !cell.CommentText.Contains(startsWith)) continue;
                    }

                    def.Information = cell.CacheText; // cell.CommentText.TrimStart(' ', '#');
                    yield return def;
                }
            }
            
            Func<FunctionDefine, bool> isTarget = def => def != null && def.Name.StartsWith(startsWith) && !considerdNames.Contains(def.Name);
            if (AlsoInfomation) isTarget = def => def != null && !considerdNames.Contains(def.Name) && (def.Name.StartsWith(startsWith) || def.Information.Contains(startsWith));

            foreach (var def in Target.FunctionList.Target.Where(isTarget)) { yield return def; considerdNames.Add(def.Name); }
            foreach (var def in Target.FunctionList.GetSystemFunctions().Where(isTarget)) { yield return def; considerdNames.Add(def.Name); }
        }

        private IEnumerable<Tuple<String, Unit, UnitConvertRecord>> GetAllUnitDefines(string startsWith, CandidateType targetType)
        {
            bool bAllPrefix = ((targetType & CandidateType.UnitAllPrefix) == CandidateType.UnitAllPrefix);
            if (startsWith.Length < 2) bAllPrefix = false; // 1文字以下ではプレフィックスは考慮しない

            var unitNames = new List<string>();
            foreach (var table in UnitConvertTable.ValidTables.Values)
            {
                foreach (var def in table.GetAllRecords(true))
                {
                    if (!(def.ConvertUnit is Unit)) continue;
                    Unit u = def.ConvertUnit as Unit;
                    unitNames.Add(u.UnitName);
                }
            }

            foreach (var table in UnitConvertTable.ValidTables.Values)
            {
                var baseDef = table.BaseUnit.ConvertUnit.ToString();

                foreach (var def in table.GetAllRecords(true))
                {
                    if (!(def.ConvertUnit is Unit)) continue;

                    Unit u = def.ConvertUnit as Unit;
                    var unitName = u.UnitName;
                    var basePrefix = u.Prefix;

                    foreach (var prefix in Prefix.GetAllPrefix(true))
                    {
                        if (prefix.Mark != "" && !bAllPrefix) continue;
                        if (prefix.Mark.Length >= startsWith.Length && prefix.Mark.StartsWith(startsWith)) continue;

                        bool considerInformation = (prefix.Mark == "" || (startsWith.StartsWith(prefix.Name) && startsWith != prefix.Name));

                        bool hit = (prefix.Mark + unitName).StartsWith(startsWith)
                                || (AlsoInfomation && considerInformation && (prefix.Name + def.UnitComment).StartsWith(startsWith));
                        if (!hit) continue;

                        var helpText = ReplacePrefixNameFrom(def.UnitComment, u.Prefix, prefix);
                        var ratio = "";
                        if (basePrefix.Name != prefix.Name) ratio = "×" + prefix.Base.ToString() + "E" + (prefix.Power - basePrefix.Power).ToString();

                        helpText += " (" + def.ConversionRatio + "[" + baseDef + "]" + ratio + ")";

                        var sample = new Unit(unitName, prefix);
                        if (prefix.Mark != "" && unitNames.Contains(sample.ToString())) continue; // min はミリインチではなく、分

                        yield return Tuple.Create(helpText, sample, def);
                    }
                }
            }
        }

        private string ReplacePrefixNameFrom(string text, Prefix orgPrefix, Prefix postPrefix)
        {
            if (orgPrefix == postPrefix)
                return text;
            else if (string.IsNullOrEmpty(orgPrefix.Name) || !text.Contains(orgPrefix.Name))
                return postPrefix.Name + text;
            else
                return text.Replace(orgPrefix.Name, postPrefix.Name);
        }

        #region IInputSupportEnumerator メンバー

        /// <summary>
        /// 指定文字列で引き当てられるすべての候補を返す反復子を取得します。
        /// </summary>
        /// <param name="startWith">引き当ての基となる文字列。</param>
        public IEnumerable<InputSupportCandidate> GetAllCandidates(string startWith)
        {
            return GetAllCandidates(startWith, CandidateType.All);
        }

        /// <summary>
        /// 指定文字列で引き当てられるすべての候補を返す反復子を取得します。
        /// </summary>
        /// <param name="startWith">引き当ての基となる文字列。</param>
        /// <param name="targetType">引き当て対象とするタイプ。</param>
        public IEnumerable<InputSupportCandidate> GetAllCandidates(string startWith, CandidateType targetType)
        {
            var candidates = new List<InputSupportCandidate>();

            if ((targetType & CandidateType.Constant) == CandidateType.Constant)
            {
                foreach (var def in GetAllConstantDefines(startWith))
                {
                    if (def == null) continue;

                    string title = def.Name + "：定数";
                    var candidate = new InputSupportCandidate(title, def.Name, def.Information, def);

                    if (candidates.Contains(candidate)) continue;
                    candidates.Add(candidate);
                }
                { // システム定義の変数
                    var lstTmp = new List<InputSupportCandidate>{
                          new InputSupportCandidate(FormulaCellContent.NameOfCurrentLineVariable + "：変数", FormulaCellContent.NameOfCurrentLineVariable, "行番号", null)
                        , new InputSupportCandidate(FormulaCellContent.NameOfInputVariable + "：変数", FormulaCellContent.NameOfInputVariable, "入力数式を1行目から順に格納した行ベクトル", null)
                        , new InputSupportCandidate(FormulaCellContent.NameOfInputRevVariable + "：変数", FormulaCellContent.NameOfInputRevVariable, "入力数式を一つ上の行から逆順に格納した行ベクトル", null)
                        , new InputSupportCandidate(FormulaCellContent.NameOfAnswerVariable + "：変数", FormulaCellContent.NameOfAnswerVariable, "結果数式を1行目から順に格納した行ベクトル", null)
                        , new InputSupportCandidate(FormulaCellContent.NameOfAnswerRevVariable + "：変数", FormulaCellContent.NameOfAnswerRevVariable, "結果数式を一つ上の行から逆順に格納した行ベクトル", null)
                    };
                    foreach (var inf in lstTmp)
                    {
                        if (inf.Title.StartsWith(startWith)) candidates.Add(inf);
                        else if (AlsoInfomation && inf.Information.Contains(startWith)) candidates.Add(inf);
                    }
                }
            }

            if ((targetType & CandidateType.Function) == CandidateType.Function)
            {
                foreach (var def in GetAllFunctionDefines(startWith))
                {
                    if (def == null) continue;

                    string title = def.Name + "：関数";
                    var candidate = new InputSupportCandidate(title, def.Name + "(", def.Information, def);

                    if (candidates.Contains(candidate)) continue;
                    candidates.Add(candidate);
                }
            }

            if ((targetType & CandidateType.Unit) == CandidateType.Unit)
            {
                foreach (var def in GetAllUnitDefines(startWith, targetType))
                {
                    string title = def.Item2.ToString() + "：単位";
                    var candidate = new InputSupportCandidate(title, def.Item2.ToString(), def.Item1, def.Item3);

                    if (candidates.Contains(candidate)) continue;
                    candidates.Add(candidate);
                }
            }

            if ((targetType & CandidateType.ControlOperator) == CandidateType.ControlOperator)
            {
                { // システム定義の変数
                    var lstTmp = new List<InputSupportCandidate>{
                          new InputSupportCandidate("$if：制御"   , "$if"   , "if:評価結果がtrue(0以外の数値)となる場合のみ以降の評価を行います。", null)
                        , new InputSupportCandidate("$elif：制御" , "$elif" , "elif:上方のif/elifの評価結果がいずれもfalseで、かつ評価結果が0以外の数値となる場合のみ以降の評価を行います。", null)
                        , new InputSupportCandidate("$else：制御" , "$else" , "else:上方のif/elifの評価結果がいずれもfalseとなる場合のみ以降の評価を行います。", null)
                        , new InputSupportCandidate("$endif：制御", "$endif", "endif:if/elif/elseの括りの終わりを表します。", null)
                    };
                    foreach (var inf in lstTmp)
                    {
                        if (inf.Title.StartsWith(startWith)) candidates.Add(inf);
                        else if (AlsoInfomation && inf.Information.Contains(startWith)) candidates.Add(inf);
                    }
                }
            }

            candidates.Sort();
            foreach (var candidate in candidates) yield return candidate;
        }

        #endregion

        /// <summary>
        /// 指定文字列から引き当てられる最初の入力補助候補を取得します。
        /// </summary>
        /// <param name="targetText">引き当てに用いる文字列。</param>
        /// <param name="lineIndex">対象とする行番号。</param>
        /// <param name="postText">対象単語と同じ行の後方の文字列。</param>
        /// <returns>引き当てられる入力補助候補。</returns>
        public InputSupportCandidate GetInputSupportCandidateFromText(string targetText, int lineIndex, string postText)
        {
            int saveLine = CurrentCaretLineNumber;
            try
            {
                CurrentCaretLineNumber = lineIndex;
                var list = new List<InputSupportCandidate>(GetAllCandidates(targetText).Where(def => def != null && def.ReplaceText.TrimEnd('(') == targetText));
                if (list.Count == 0) return null;

                var helpTarget = list[0];
                if (list.Count > 1)
                {
                    if (postText.StartsWith("("))
                        helpTarget = list.Find(c => c.Tag is FunctionDefine);
                    else
                        helpTarget = list.Find(c => !(c.Tag is FunctionDefine));
                }
                return helpTarget;
            }
            finally
            {
                CurrentCaretLineNumber = saveLine;
            }
        }
    }
}
