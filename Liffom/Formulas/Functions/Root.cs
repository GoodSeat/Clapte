using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Functions.Rules;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Functions
{
    /// <summary>
    /// root関数を表します。
    /// </summary>
    [Serializable()]
    public class Root : Function
    {
        /// <summary>
        /// root関数を初期化します。
        /// </summary>
        public Root() : base() { } 

        /// <summary>
        /// 平方根を初期化します。
        /// </summary>
        /// <param name="f">平方根の対象とする数式</param>
        public Root(Formula f) : base(f, 2) { }

        /// <summary>
        /// 冪根を作成します。
        /// </summary>
        /// <param name="f">平方根の対象とする数式</param>
        /// <param name="n">冪根の冪数</param>
        public Root(Formula f, Formula n) : base(f, n) { }

        /// <summary>
        /// 引数を指定して、関数を生成します。
        /// </summary>
        /// <param name="args">初期化に用いるか変数の数式。</param>
        /// <returns>初期化された関数。</returns>
        public override Function CreateFunction(params Formula[] args)
        {
            return new Root(args[0], args[1]);
        }

        public override Formula CalculateFunction()
        {
            if (Argument.Count < 2 || this[1] is Null)
                return new Power(this[0], new Numeric(2) ^ -1);
            else
                return new Power(this[0], this[1] ^ -1);
        }

        public override int MinimumArgumentQty { get { return 1; } }

        public override int MaximumArgumentQty { get { return 2; } }

        public override string GetInformation(out List<string> args)
        {
            args = new List<string>(); args.Add("数式"); args.Add("累乗根の冪数（任意。省略時2。）");
            return "数式の平方根、もしくは指定した冪数の冪根を返します。";
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

            if (deformToken.Has<CombineToken>())
            {
                yield return FactorOutInnerRootRule.Entity;

                if (sender is Root) yield return new ComposeInnerRootRule();
                if (sender is Product) yield return new ProductRootRule();
                if (sender is Power) yield return new PowerInnerRootRule();
            }
        }
    }
}

