// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Functions.Rules;
using GoodSeat.Liffom.Formulas.Functions.Trigonometric;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Liffom.Formulas.Functions
{
    /// <summary>
    /// ArcTan関数を表します。
    /// </summary>
    [Serializable()]
    public class ArcTan : Function
    {
        /// <summary>
        /// atan関数を初期化します。
        /// </summary>
        public ArcTan() : base() { }

        /// <summary>
        /// atan関数を初期化します。
        /// </summary>
        /// <param name="f">引数。</param>
        public ArcTan(Formula f) : base(f) { Argument = new Argument(f); }

        /// <summary>
        /// 引数を指定して、関数を生成します。
        /// </summary>
        /// <param name="args">初期化に用いるか変数の数式。</param>
        /// <returns>初期化された関数。</returns>
        public override Function CreateFunction(params Formula[] args)
        {
            return new ArcTan(args[0]);
        }

        public override Formula CalculateFunction()
        {
            var z = Argument[0];
            foreach (Formula rad in TrigonometricFunction.GetTriRads(0, 90))
            {
                Formula checkTan = new Tan(rad);
                if (checkTan.Calculate() == z) return rad;
            }
            foreach (Formula rad in TrigonometricFunction.GetTriRads(270, 360))
            {
                Formula checkTan = new Tan(rad);
                if (checkTan.Calculate() == z) return rad;
            }

            if (z is Numeric)
                return new Numeric((z as Numeric).Figure.Atan());
            else if (Imaginary.IsComplexNumber(z, true))
            {
                var i = Imaginary.i;
                return i / 2 * (new Ln(1 - i * z) - new Ln(1 + i * z));
            }
            else
                return this;
        }

        public override int MinimumArgumentQty { get { return 1; } }

        public override string DistinguishedName { get { return "atan"; } }

        public override string GetInformation(out List<string> args)
        {
            args = new List<string>(); args.Add("数値");
            return "数値のアークタンジェントを返します。";
        }

        /// <summary>
        /// 指定処理に関連するルールをすべて返す反復子を取得します。
        /// </summary>
        /// <param name="deformToken">変形識別トークン。</param>
        /// <param name="sender">ルール適用対象となる最上位親数式。</param>
        /// <param name="history">変形履歴情報。</param>
        /// <returns>変形に関連するルールを返す反復子。</returns>
        public override IEnumerable<Rule> GetRelatedRulesOf(DeformToken deformToken, Formula sender, DeformHistory history) 
        {
            foreach (var rule in base.GetRelatedRulesOf(deformToken, sender, history)) yield return rule;

            if ((deformToken.Has<CalculateToken>() || deformToken.Has<DifferentiateToken>()) && sender is Differentiate)
            {
                var x = new RulePatternVariable("x");
                yield return new InstantPatternRule(new Differentiate(new ArcTan(x), x), 1 / (1 + (x ^ 2)));

                yield return new DifferentiateCompositeFunctionRule(0);
            }
        }
    }
}
