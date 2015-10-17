using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 数式の意味的区切りを表現するトークンを表します。
    /// </summary>
    public abstract class PunctuationToken : Token
    {
        /// <summary>
        /// 区切りの定義タイプを表します。
        /// </summary>
        public enum PunctuationType
        {
            /// <summary>
            /// 不定です。
            /// </summary>
            Undefined,
            /// <summary>
            /// 区切り区間の開始を定義します。
            /// </summary>
            Start,
            /// <summary>
            /// 区切り区間の終了を定義します。
            /// </summary>
            End
        }

        /// <summary>
        /// 数式の意味的区切りを表現するトークンを初期化します。
        /// </summary>
        public PunctuationToken(string targetText) 
            : base(targetText) 
        {
            Type = PunctuationType.Undefined;
        }

        /// <summary>
        /// 区切りタイプを設定もしくは取得します。
        /// </summary>
        public PunctuationType Type { get; protected set; }

        /// <summary>
        /// 指定した区切りトークンが、このトークンに正しく対応する区切り対トークンか否かを取得します。
        /// </summary>
        /// <example>
        /// "("のトークンでは、")"のトークンに対してのみtrueを返し、それ以外の場合常にfalseを返します。
        /// </example>
        /// <param name="token">判定対象の対トークン</param>
        /// <returns>対応するか否か</returns>
        public abstract bool IsValidSetPunctuation(PunctuationToken token);

        /// <summary>
        /// 区切りの内部が構文解析されたとき、その評価結果を必要に応じて加工します。
        /// </summary>
        /// <param name="parsed">区切り文字内部の解析結果</param>
        /// <returns>加工結果</returns>
        public virtual Formula OnInnerParsed(Formula parsed) { return parsed; }
    }
}
