using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Clapte.Solvers;

namespace GoodSeat.Clapte.ViewModels.ClapteCommands
{
    /// <summary>
    /// ユーザー定義関数の削除コマンドを表します。
    /// </summary>
    public class DeleteFunctionCommand : ClapteCommand
    {
        /// <summary>
        /// 関数の定義コマンド文字列を検知する正規表現オブジェクトを設定もしくは取得します。
        /// </summary>
        static Regex DefineRegex { get; set; }

        static DeleteFunctionCommand()
        {
            DefineRegex = new Regex(@"^\s*delete\s*\-\>\s*(?<name>[^( ]+)");
        }

        /// <summary>
        /// ユーザー定義関数の削除コマンドを表します。
        /// </summary>
        /// <param name="owner">親となるClapteCoreModelViewオブジェクト。</param>
        public DeleteFunctionCommand(ClapteCoreViewModel owner)
            : base(owner) 
        {
        }

        /// <summary>
        /// 削除候補となるユーザー定義関数の定義オブジェクトを設定もしくは取得します。
        /// </summary>
        private FunctionDefine Target { get; set; }

        /// <summary>
        /// 削除対象となるユーザー定義関数リストを取得します。
        /// </summary>
        private IList<FunctionDefine> TargetList { get { return Owner.Solver.Target.UserFunctions; } }


        public override ClapteCommandResult TryInitializeCommand(string text)
        {
            string targetText = text.Replace("\n","").Replace("\r","");

            var match = DefineRegex.Match(text);
            if (!match.Success) return null;

            var name = match.Groups["name"].Value;
            Target = null;
            foreach (var exist in TargetList)
            {
                if (exist.Name == name)
                {
                    Target = exist;
                    break;
                }
            }
            if (Target == null) return null;

            var message = string.Format("操作の続行により、ユーザー定義関数 {0} を削除します。", name);
            return new ClapteCommandResult("ユーザー定義関数の削除", message, ToolTipIcon.Warning, true);
        }

        public override ClapteCommandResult DoAction()
        {
            if (Target == null) return null;

            bool deleted = false;
            for (int i = 0; i < TargetList.Count; i++)
            {
                if (TargetList[i].Name == Target.Name)
                {
                    TargetList.RemoveAt(i);
                    deleted = true;
                    break;
                }
            }
            string title = "ユーザー定義関数の削除";
            string message = string.Format("ユーザー定義関数 {0} を削除しました。", Target.Name);
            Target = null;

            if (deleted)
                return new ClapteCommandResult(title, message, ToolTipIcon.Info, false);
            else
                return null;
        }

    }
}


