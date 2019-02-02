// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Functions.Rules;
using GoodSeat.Liffom.Formulas.Constants;

namespace GoodSeat.Liffom.Formulas.Functions
{
    /// <summary>
    /// 微分を表します。
    /// </summary>
    [Serializable()]
    public class Differentiate : Function
    {
        static DeformToken s_diffSimplifyToken;
        static DeformToken s_diffToken;

        static Differentiate() 
        {
            s_diffSimplifyToken = new DeformToken(Formula.SimplifyToken, new DifferentiateToken());
            s_diffToken = new DifferentiateToken();
        }


        /// <summary>
        /// 微分関数を初期化します。
        /// </summary>
        public Differentiate() : base() { }

        /// <summary>
        /// 全微分関数を初期化します。
        /// </summary>
        /// <param name="f">微分対象の数式。</param>
        public Differentiate(Formula f) : base(f) { }

        /// <summary>
        /// 微分関数を初期化します。
        /// </summary>
        /// <param name="f">微分対象の数式。</param>
        /// <param name="v">微分対象の変数。</param>
        public Differentiate(Formula f, Formula v)
            : base(new Argument(f, v))
        { }

        /// <summary>
        /// 引数を指定して、関数を生成します。
        /// </summary>
        /// <param name="args">初期化に用いるか変数の数式。</param>
        /// <returns>初期化された関数。</returns>
        public override Function CreateFunction(params Formula[] args)
        {
            return new Differentiate(new Argument(args));
        }

        /// <summary>
        /// 微分対象の式を設定もしくは取得します。
        /// </summary>
        public Formula TargetFormula
        {
            get { return Argument[0]; }
            set { Argument[0] = value; }
        }

        /// <summary>
        /// 微分変数を設定もしくは取得します。全微分の場合、Nullが取得されます。
        /// </summary>
        public Formula Variable
        {
            get { return Argument[1]; }
            set { Argument[1] = value; }
        }

        /// <summary>
        /// 微分関数を解決して数式を簡単化します。
        /// </summary>
        public Formula SimplifyDifferentiate()
        {
            return DeformFormula(s_diffSimplifyToken);
        }

        public override int MinimumArgumentQty
        {
            get { return 2; } // 0:式, 1:変数
        }

        public override string DistinguishedName { get { return "diff"; } }

        public override Formula CalculateFunction() 
        {
            return DeformFormula(s_diffToken);
        }

        public override string GetInformation(out List<string> args)
        {
            args = new List<string>(); args.Add("数式"); args.Add("微分変数");
            return "数式を微分して返します。";
        }

        /// <summary>
        /// 指定処理に関連するルールをすべて返す反復子を取得します。
        /// </summary>
        /// <param name="deformToken">変形処理。</param>
        /// <param name="sender">ルール適用対象となる最上位親数式。</param>
        /// <param name="history">変形履歴情報。</param>
        /// <returns>変形に関連するルールを返す反復子。</returns>
        public override IEnumerable<Rule> GetRelatedRulesOf(DeformToken deformToken, Formula sender, DeformHistory history) 
        {
            foreach (var rule in base.GetRelatedRulesOf(deformToken, sender, history)) yield return rule;

            if (!object.ReferenceEquals(this, sender)) yield break;

            if (deformToken.Has<DifferentiateToken>())
            {
                yield return AtomicDifferentiateRule.Entity;
            }
            if (deformToken.Has<DifferentiateToken>() || deformToken.Has<CalculateToken>() || deformToken.Has<ExpandToken>())
            {
                if (TargetFormula is Sum) yield return ExpandDifferentiateSumRule.Entity;
                else if (TargetFormula is Product) yield return ExpandDifferentiateProductRule.Entity;
                else if (TargetFormula is Power) yield return ExpandDifferentiatePowerRule.Entity;
            }
            if (deformToken.Has<DifferentiateToken>() || deformToken.Has<CalculateToken>())
            {
                if (TargetFormula is Power)
                    yield return CalculateDifferentiatePowerRule.Entity;
            }
        }
    }

    /// <summary>
    /// 微分関数を解決する変形トークンの特性を表します。
    /// </summary>
    [Serializable()]
    public class DifferentiateToken : DeformToken { }

}
