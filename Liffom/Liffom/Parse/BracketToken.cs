// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formats;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Matrices;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Formulas.Constants;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 角括弧トークンを表します。
    /// </summary>
    public class BracketToken : PunctuationToken
    {
        /// <summary>
        /// 角括弧トークンを初期化します。
        /// </summary>
        /// <param name="targetText">トークン元文字列。</param>
        public BracketToken(string targetText) : this(targetText, false, true) { }

        /// <summary>
        /// 角括弧トークンを初期化します。
        /// </summary>
        /// <param name="targetText">トークン元文字列。</param>
        /// <param name="evaluateAsUnit">括弧内の変数を単位として解析するか否か。</param>
        /// <param name="evaluateAsRowVector">括弧内のカンマ区切り数式を行ベクトルとして解析するか否か。</param>
        public BracketToken(string targetText, bool evaluateAsUnit, bool evaluateAsRowVector)
            : base(targetText)
        {
            if (targetText == "[") Type = PunctuationType.Start;
            else if (targetText == "]") Type = PunctuationType.End;
            else throw new FormulaParseException();

            EvaluateAsUnit = evaluateAsUnit;
            EvaluateAsRowVector = evaluateAsRowVector;
        }

        /// <summary>
        /// 角括弧内部の変数を単位として解析するか否かを設定もしくは取得します。
        /// </summary>
        public bool EvaluateAsUnit { get; set; }

        /// <summary>
        /// 角括弧内部のカンマ区切りの数式を、行ベクトルとして解析するか否かを設定もしくは取得します。
        /// </summary>
        public bool EvaluateAsRowVector { get; set; }

        public override bool IsValidSetPunctuation(PunctuationToken token)
        {
            if (!(token is BracketToken)) return false;

            if (Type == PunctuationType.Start) return token.TargetText == "]";
            else if (Type == PunctuationType.End) return token.TargetText == "[";
            else return false;
        }

        /// <summary>
        /// 区切りの内部が構文解析されたとき、その評価結果を必要に応じて加工します。
        /// </summary>
        /// <param name="parsed">区切り文字内部の解析結果</param>
        /// <returns>加工結果</returns>
        public override Formula OnInnerParsed(Formula parsed) 
        {
            // 行ベクトルへの変換
            if (EvaluateAsRowVector && parsed is Argument)
            {
                var arg = parsed as Argument;
                List<Formula> args = new List<Formula>(parsed);
                return new Vector(true, args.ToArray());
            }

            if (EvaluateAsUnit)
            {
                // 変数を単位で置き換え
                foreach (var variable in parsed.GetExistFactors<Variable>())
                {
                    Unit unit = new Unit(variable.Mark);
                    parsed = parsed.Substitute(variable, unit);
                }
                foreach (var constant in parsed.GetExistFactors<Constant>())
                {
                    Unit unit = new Unit(constant.DistinguishedName);
                    parsed = parsed.Substitute(constant, unit);
                }
            }
            parsed.Format.SetProperty(Bracket.SquareBracket);
            return parsed;
        }
    }
}
