using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Utilities;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators.Rules.Products;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Formats;

namespace GoodSeat.Liffom.Formulas.Operators
{
    /// <summary>
    /// 積算を表します。
    /// </summary>
    [Serializable()]
    public class Product : OperatorMultiple
    {
        /// <summary>
        /// 積算を作成します。
        /// </summary>
        /// <param name="formulas">掛ける数式</param>
        public Product(params Formula[] formulas) : this(false, formulas) { }

        /// <summary>
        /// 積算を作成します。
        /// </summary>
        /// <param name="integration">trueを指定すると、構成数式内部のProductを統合します</param>
        /// <param name="formulas">掛ける数式</param>
        public Product(bool integration, params Formula[] formulas) : base(integration, formulas) { }
        
        /// <summary>
        /// 演算が満たす法則を取得します。
        /// </summary>
        public override OperatorLaw Law 
        { 
            get
            { 
                return OperatorLaw.Distributive | OperatorLaw.Associative | OperatorLaw.Commutative;
            }
        }

        /// <summary>
        /// 分配則の対象となる演算の型を取得します。
        /// </summary>
        /// <example>積算(Product)は和算(Sum)に対して分配則が成り立つため、typeof(Sum)を返します。</example>
        public override Type DistributeTarget() { return typeof(Sum); }
                
        /// <summary>
        /// 演算の評価優先度を表す数値を取得します。
        /// 原則として、OperatorPriorityの値を返してください。
        /// </summary>
        public override int EvaluatePriority { get { return (int)OperatorPriority.Product; } }

        /// <summary>
        /// 引数を指定して、演算を生成します。
        /// </summary>
        /// <param name="args">初期化に用いるか変数の数式。</param>
        public override Operator CreateOperator(params Formula[] args)
        {
            return new Product(args);
        }

        protected override void OnSort() { Formulas.Sort(); }

        public override Formula DefaultValue { get { return 1; } }

        public override string GetText()
        {
            string result = "";
            bool initial = true;
            foreach (Formula f in Formulas)
            {
                if (f.IsUnit())
                {
                    if (!this.IsUnit())
                        result = result.TrimEnd('*') + f.ToString() + "*";
                    else
                        result += f.ToString() + "･";
                }
                else
                {
                    if (f is Numeric && result.EndsWith("-")) result += "1*";
                    result += f.ToString() + "*";
                }

                if (initial && f == -1) result = result.Replace("-1*", "-");
                initial = false;
            }
            return result.TrimEnd('*').TrimEnd('･').Replace("*1/", "/").Replace("･1/", "/");
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

            if (sender is Product) foreach(var rule in GetProductRelatedRulesOf(sender as Product, deformToken)) yield return rule;
        }

        public IEnumerable<Rule> GetProductRelatedRulesOf(Product sender, DeformToken deformToken)
        {
            if (deformToken.Has<CombineToken>())
            {
                yield return new CombineProductToPowerRule(); // ((a^b)^d) * ((a^c)^e) → a^(bd + ce)
                yield return new CombineToLowestTermsRule(); // a^b / (c * a^d) → a^(b-d) / c
                yield return new ReduceFractionsToLowestTermsRule(); // (a*b + b) / (a + 1) → b
            }
        }
        
    }
}
