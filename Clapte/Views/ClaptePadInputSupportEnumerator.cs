using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.ViewModels;

namespace GoodSeat.Clapte.Views.InputSupports
{
    /// <summary>
    /// ClaptePadにおける入力補助候補の列挙クラスを表します。
    /// </summary>
    public class ClaptePadInputSupportEnumerator : IInputSupportEnumerator
    {
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
            for (int i = CurrentCaretLineNumber - 1; i >= 0; i--)
            {
                if (Target[i] == null) continue;
                var cell = Target[i].Target;

                List<ConstantDefine> list = new List<ConstantDefine>(cell.Content.GetAllConstantDefines().Where(def => def.Name.StartsWith(startsWith)));
                if (cell.CommentText.Contains(startsWith)) list = new List<ConstantDefine>(cell.Content.GetAllConstantDefines());

                foreach (var def in list)
                {
                    def.Information = cell.CacheText; // cell.CommentText.TrimStart(' ', '#');
                    yield return def;
                }
            }

            foreach (var def in Target.ConstantList.Target.Where(def => def.Name.StartsWith(startsWith) || def.Information.Contains(startsWith)))
                yield return def;
            foreach (var def in Target.ConstantList.GetSystemConstants().Where(def => def.Name.StartsWith(startsWith) || def.Information.Contains(startsWith)))
                yield return def;
        }

        private IEnumerable<FunctionDefine> GetAllFunctionDefines(string startsWith)
        {
            for (int i = CurrentCaretLineNumber - 1; i >= 0; i--)
            {
                if (Target[i] == null) continue;
                var cell = Target[i].Target;

                List<FunctionDefine> list = new List<FunctionDefine>(cell.Content.GetAllFunctionDefines().Where(def => def.Name.StartsWith(startsWith)));
                if (cell.CommentText.Contains(startsWith)) list = new List<FunctionDefine>(cell.Content.GetAllFunctionDefines());

                foreach (var def in list)
                {
                    def.Information = cell.CacheText; // cell.CommentText.TrimStart(' ', '#');
                    yield return def;
                }
            }

            foreach (var def in Target.FunctionList.Target.Where(def => def.Name.StartsWith(startsWith) || def.Information.Contains(startsWith)))
                yield return def;
            foreach (var def in Target.FunctionList.GetSystemFunctions().Where(def => def.Name.StartsWith(startsWith) || def.Information.Contains(startsWith)))
                yield return def;
        }

        #region IInputSupportEnumerator メンバー

        /// <summary>
        /// 指定文字列で引き当てられるすべての候補を返す反復子を取得します。
        /// </summary>
        /// <param name="startWith">引き当ての基となる文字列。</param>
        public IEnumerable<InputSupportCandidate> GetAllCandidates(string startWith)
        {
            var candidates = new List<InputSupportCandidate>();

            foreach (var def in GetAllConstantDefines(startWith))
            {
                string title = def.Name + "：定数";
                var candidate = new InputSupportCandidate(title, def.Name, def.Information, def);

                if (candidates.Contains(candidate)) continue;
                candidates.Add(candidate);
            }
            foreach (var def in GetAllFunctionDefines(startWith))
            {
                string title = def.Name + "：関数";
                var candidate = new InputSupportCandidate(title, def.Name, def.Information, def);

                if (candidates.Contains(candidate)) continue;
                candidates.Add(candidate);
            }
            candidates.Sort();

            foreach (var candidate in candidates) yield return candidate;
        }

        #endregion
    }
}
