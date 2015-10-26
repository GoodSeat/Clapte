using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.ViewModels;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Clapte.Views.InputSupports
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
            /// 考えられる全てを候補とします。
            /// </summary>
            All = Constant | Function | UnitAllPrefix
        }


        /// <summary>
        /// ClaptePadにおける入力補助候補の列挙クラスを初期化します。
        /// </summary>
        /// <param name="target">入力補助の提供元情報となるFormulaCellListViewModel。</param>
        public ClaptePadInputSupportEnumerator(FormulaCellListViewModel target)
        {
            Target = target;
        }

        /// <summary>
        /// 候補列挙の際に参照すべき最大行番号を設定もしくは取得します。
        /// </summary>
        public int CurrentCaretLineNumber { get; set; }

        /// <summary>
        /// 入力補助の提供元情報となるFormulaCellListViewModelを設定もしくは取得します。
        /// </summary>
        FormulaCellListViewModel Target { get; set; }

        private IEnumerable<ConstantDefine> GetAllConstantDefines(string startsWith)
        {
            for (int i = CurrentCaretLineNumber; i >= 0; i--)
            {
                if (Target[i] == null) continue;
                var cell = Target[i].Target;

                var list = new List<ConstantDefine>(cell.Content.GetAllConstantDefines().Where(def =>
                            def != null && def.Name.StartsWith(startsWith)));
                if (cell.CommentText.Contains(startsWith)) list = new List<ConstantDefine>(cell.Content.GetAllConstantDefines());

                foreach (var def in list)
                {
                    if (def == null) continue;
                    def.Information = cell.CacheText; // cell.CommentText.TrimStart(' ', '#');
                    yield return def;
                }
            }

            foreach (var def in Target.ConstantList.Target.Where(def =>
                        def != null && (def.Name.StartsWith(startsWith) || def.Information.Contains(startsWith))))
                yield return def;
            foreach (var def in Target.ConstantList.GetSystemConstants().Where(def =>
                        def != null && (def.Name.StartsWith(startsWith) || def.Information.Contains(startsWith))))
                yield return def;
        }

        private IEnumerable<FunctionDefine> GetAllFunctionDefines(string startsWith)
        {
            for (int i = CurrentCaretLineNumber - 1; i >= 0; i--)
            {
                if (Target[i] == null) continue;
                var cell = Target[i].Target;

                List<FunctionDefine> list = new List<FunctionDefine>(cell.Content.GetAllFunctionDefines().Where(def =>
                            def != null && def.Name.StartsWith(startsWith)));
                if (cell.CommentText.Contains(startsWith)) list = new List<FunctionDefine>(cell.Content.GetAllFunctionDefines());

                foreach (var def in list)
                {
                    if (def == null) continue;
                    def.Information = cell.CacheText; // cell.CommentText.TrimStart(' ', '#');
                    yield return def;
                }
            }

            foreach (var def in Target.FunctionList.Target.Where(def =>
                        def != null && (def.Name.StartsWith(startsWith) || def.Information.Contains(startsWith))))
                yield return def;
            foreach (var def in Target.FunctionList.GetSystemFunctions().Where(def =>
                        def != null && (def.Name.StartsWith(startsWith) || def.Information.Contains(startsWith))))
                yield return def;
        }

        private IEnumerable<Tuple<String, Unit, UnitConvertRecord>> GetAllUnitDefines(string startsWith, CandidateType targetType)
        {
            bool bAllPrefix = ((targetType & CandidateType.UnitAllPrefix) == CandidateType.UnitAllPrefix);

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

                        var test = prefix.Mark + unitName;
                        if (!test.StartsWith(startsWith)) continue;

                        var helpText = ReplacePrefixNameFrom(def.UnitComment, prefix);
                        var ratio = "";
                        if (basePrefix.Name != prefix.Name) ratio = "×" + prefix.Base.ToString() + "E" + (prefix.Power - basePrefix.Power).ToString();

                        helpText += " (" + def.ConversionRatio + "[" + baseDef + "]" + ratio + ")";

                        var sample = new Unit(unitName, prefix);
                        yield return Tuple.Create(helpText, sample, def);
                    }
                }
            }
        }

        private string ReplacePrefixNameFrom(string text, Prefix postPrefix)
        {
            bool replaced = false;
            foreach (var prefix in Prefix.GetAllPrefix(false))
            {
                if (!text.Contains(prefix.Name)) continue;
                if (prefix == postPrefix) return text;
                if (string.IsNullOrEmpty(prefix.Name)) continue;

                text = text.Replace(prefix.Name, postPrefix.Name);
                replaced = true;
                break;
            }
            if (!replaced) text = postPrefix.Name + text;
            return text;
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

            candidates.Sort();
            foreach (var candidate in candidates) yield return candidate;
        }

        #endregion
    }
}
