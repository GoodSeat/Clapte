// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GoodSeat.Clapte.ViewModels.ClapteCommands
{
    /// <summary>
    /// Clapteの実行コマンドを表します。
    /// </summary>
    public abstract class ClapteCommand
    {
        /// <summary>
        /// 行内のコメント扱い文字列を検知する正規表現オブジェクトを設定もしくは取得します。
        /// </summary>
        static Regex CommentRegex { get; set; }

        static ClapteCommand()
        {
            CommentRegex = new Regex(@"(^\#.*|\/\/.*)");
        }

        /// <summary>
        /// 指定文字列からコメント扱い文字を削除して取得します。
        /// </summary>
        /// <param name="text">処理対象のテキスト。</param>
        /// <returns>コメントを取り除いた文字列。</returns>
        static public string RemoveCommentTextFrom(string text)
        {
            var lines = text.Replace("\r", "").Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = CommentRegex.Replace(lines[i], "");
            }
            return string.Join("\r\n", lines);
        }

        /// <summary>
        /// Clapteの実行コマンドを初期化します。
        /// </summary>
        /// <param name="owner">親となるClapteCoreModelViewオブジェクト。</param>
        public ClapteCommand(ClapteCoreViewModel owner)
        {
            Owner = owner;
        }

        /// <summary>
        /// 親となるClapteCoreModelViewオブジェクトを取得します。
        /// </summary>
        public ClapteCoreViewModel Owner { get; private set; }

        /// <summary>
        /// 命令テキストを指定して、コマンドの初期化を試みます。
        /// </summary>
        /// <param name="text">命令テキスト。</param>
        /// <returns>コマンドの初期化結果。初期化に失敗した場合、null。</returns>
        public abstract ClapteCommandResult TryInitializeCommand(string text);

        /// <summary>
        /// コマンドに関連付けられた処理を実行します。
        /// </summary>
        /// <returns>処理の実行結果を表す結果。</returns>
        public abstract ClapteCommandResult DoAction();


    }
}
