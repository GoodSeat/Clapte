using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Sio.Xml;
using GoodSeat.Sio.Xml.Serialization;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Parse;

namespace GoodSeat.Clapte.ViewModels.ClapteCommands
{
    /// <summary>
    /// ユーザー定義関数の追加コマンドを表します。
    /// </summary>
    public class DefineFunctionCommand : ClapteCommand
    {
        /// <summary>
        /// 関数の定義コマンド文字列を検知する正規表現オブジェクトを設定もしくは取得します。
        /// </summary>
        static Regex DefineRegex { get; set; }

        static DefineFunctionCommand()
        {
            DefineRegex = new Regex(@"^\s*(?<name>[^( ]+)\((?<argument>[^)]*)\)\s*\:\=\s*(?<define>[^【]+)(【(?<comment>.*)】)?");
        }

        /// <summary>
        /// 関数定義テキストのマッチ結果から引数に使用する変数リストを取得します。
        /// </summary>
        /// <param name="match">"argument"グループに、括弧を除く引数定義部分を含む関数定義テキストのマッチ結果。</param>
        /// <param name="parser">数式の構文解析器。</param>
        /// <returns>変数リスト。関数定義として使用できない場合、null。</returns>
        static public List<Variable> GetUseVariableList(Match match, FormulaParser parser)
        {
            var useVariables = new List<Variable>();
            if (string.IsNullOrEmpty(match.Groups["argument"].Value)) return useVariables;
            
            string argText = match.Groups["argument"].Value;

            Formula args;
            if (!parser.TryParse(argText, out args)) return null;

            if (args is Argument)
            {
                foreach (var f in args)
                {
                    if (f is Variable) useVariables.Add(f as  Variable);
                    else if (f is Unit) useVariables.Add(new Variable(f.ToString()));
                    else return null;
                }
            }
            else
            {
                if (args is Variable) useVariables.Add(args as  Variable);
                else if (args is Unit) useVariables.Add(new Variable(args.ToString()));
                else return null;
            }
            return useVariables;
        }


        /// <summary>
        /// ユーザー定義関数の追加コマンドを表します。
        /// </summary>
        /// <param name="owner">親となるClapteCoreModelViewオブジェクト。</param>
        public DefineFunctionCommand(ClapteCoreViewModel owner)
            : base(owner) 
        {
        }

        /// <summary>
        /// 追加候補となるユーザー定義関数の定義オブジェクトを設定もしくは取得します。
        /// </summary>
        private FunctionDefine Target { get; set; }

        /// <summary>
        /// <see cref="Target"/>と同名ユーザー定義関数の定義オブジェクトを設定もしくは取得します。
        /// </summary>
        private FunctionDefine AlreadyExist { get; set; }

        /// <summary>
        /// 追加対象となるユーザー定義関数リストを取得します。
        /// </summary>
        private IList<FunctionDefine> TargetList { get { return Owner.Solver.Target.UserFunctions; } }

        /// <summary>
        /// 数式の構文解析器を取得します。
        /// </summary>
        private FormulaParser Parser { get { return Owner.Solver.Target.Parser; } }

        /// <summary>
        /// 指定した文字列から関数定義を取り出します。
        /// </summary>
        /// <param name="targetText">判定対象の文字列。</param>
        /// <returns>取り出された関数定義。取り出せない場合にはnull。</returns>
        public FunctionDefine PickDefineFrom(string targetText)
        {
            var funcLexer = Parser.GetLexerOf<FunctionLexer>();
            try
            {
                funcLexer.ScanNotDefinedUserFunction = true;

                var match = DefineRegex.Match(targetText);
                if (!match.Success) return null;

                string name = match.Groups["name"].Value;
                Formula f;
                if (!Parser.TryParse(name, out f)) return null;
                if (!(f is Variable) && !(f is Unit)) return null;

                var userFunction = new UserFunction(name);

                var useVariables = GetUseVariableList(match, Parser);
                if (useVariables == null) return null;
                userFunction.UseVariable = useVariables;

                string defText = match.Groups["define"].Value.Trim(); 
                Formula define;
                if (!Parser.TryParse(defText, out define)) return null;

                List<string> argsComments;
                string comment = GetComment(match, userFunction, out argsComments);

                var functionDefine = new FunctionDefine(userFunction);
                functionDefine.Define = defText;
                functionDefine.Information = comment.Trim();
                if (argsComments != null) functionDefine.ArgumentInformation = argsComments;
                return functionDefine;
            }
            finally
            {
                funcLexer.ScanNotDefinedUserFunction = false;
            }
        }


        /// <summary>
        /// 関数定義テキストのマッチ結果から関数の説明テキストを取得します。
        /// </summary>
        /// <param name="match">関数定義テキストのマッチ結果。</param>
        /// <param name="userFunction">定義対象となるユーザー定義関数。</param>
        /// <param name="argsComments">引数変数の説明リスト。</param>
        /// <returns>取り出された説明テキスト。取り出しに失敗した場合、""。</returns>
        private string GetComment(Match match, UserFunction userFunction, out List<string> argsComments)
        {
            string comment = "";
            argsComments = null;

            if (!string.IsNullOrEmpty(match.Groups["comment"].Value))
            {
                string commentAll = match.Groups["comment"].Value.TrimEnd(' ');

                argsComments = new List<string>();
                foreach (var arg in userFunction.UseVariable)
                {
                    Regex argCommentRegex = new Regex(string.Format(@"{0}\s*\=\s*(?<comment>[^ ]*)", arg.ToString()));

                    string argComment = "";
                    var argCommentMatch = argCommentRegex.Match(commentAll);
                    if (argCommentMatch.Success)
                    {
                        argComment = argCommentMatch.Groups["comment"].Value;
                        commentAll = argCommentRegex.Replace(commentAll, "");
                    }
                    argsComments.Add(argComment);
                }
                comment = commentAll;
            }
            return comment;
        }

        public override ClapteCommandResult TryInitializeCommand(string text)
        {
            string targetText = text.Replace("\n","").Replace("\r","");

            Target = PickDefineFrom(targetText);
            if (Target == null) return null;

            AlreadyExist = null;
            foreach (var exist in TargetList)
            {
                if (exist.Name == Target.Name)
                {
                    AlreadyExist = exist;
                    break;
                }
            }

            string message = string.Format("操作の続行により、ユーザー定義関数 {0} を追加します。", Target.Name);
            if (AlreadyExist != null)
            {
                var userFunction = AlreadyExist.Target as UserFunction;
                message = string.Format("操作の続行により、ユーザー定義関数 {0} の定義を更新します。\n※ 現在の定義 {0}={1}【{2}】",
                                                userFunction.NameForView, AlreadyExist.Define, AlreadyExist.Information);
            }
            return new ClapteCommandResult("ユーザー定義関数の定義", message, ToolTipIcon.Info, true);
        }

        public override ClapteCommandResult DoAction()
        {
            if (Target == null) return null;
            if (AlreadyExist != null) TargetList.Remove(AlreadyExist);
            TargetList.Add(Target);

            string title = "ユーザー定義関数の定義";
            string message = string.Format("ユーザー定義関数 {0} を{1}しました。", Target.Name, 
                    (AlreadyExist == null) ? "追加" : "更新");

            Target = null;
            return new ClapteCommandResult(title, message, ToolTipIcon.Info, false);
        }

    }
}
