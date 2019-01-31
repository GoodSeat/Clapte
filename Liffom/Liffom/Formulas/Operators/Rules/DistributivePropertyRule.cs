// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Rules;

namespace GoodSeat.Liffom.Formulas.Operators.Rules
{
    /// <summary>
    /// 分配則を規定するルールを表します。
    /// </summary>
    public class DistributivePropertyRule : CombinationRule
    {
        /// <summary>
        /// 分配則を規定するルールを初期化します。
        /// </summary>
        private DistributivePropertyRule() { }

        /// <summary>
        /// 分配則を規定するルールを初期化します。
        /// </summary>
        /// <param name="ownerType">分配則を満たす演算の型。</param>
        /// <param name="targetType">分配則の成立対象となる演算の型。</param>
        public DistributivePropertyRule(Type ownerType, Type targetType)
        {
            OwnerType = ownerType;
            TargetType = targetType;
        }

        private Type OwnerType { get; set; }

        private Type TargetType { get; set; }

        private Operator OwnerFormula { get; set; }

        private Operator TargetFormula { get; set; }

        protected override CombinationRule.ApplyType RuleApplyType
        {
            get { return ApplyType.OneTimePerTerm; }
        }

        protected internal override bool IsTargetTypeFormula(Formula target) 
        {
            OwnerFormula = target as Operator;
            return target.GetType() == OwnerType;
        }

        protected override bool IsTargetCouple(Formula f1, Formula f2) 
        {
            if (OwnerFormula.Satisfy(Operator.OperatorLaw.DistributiveToLeft) && f1.GetType() == TargetType)
                TargetFormula = f1 as Operator;
            else if (OwnerFormula.Satisfy(Operator.OperatorLaw.DistributiveToRight) && f2.GetType() == TargetType) 
                TargetFormula = f2 as Operator;
            else
                return false;

            return true;
        }

        protected override Formula GetRuledFormula(Formula f1, Formula f2)
        {
            List<Formula> list = new List<Formula>();
            if (OwnerFormula.Satisfy(Operator.OperatorLaw.DistributiveToLeft) && f1.GetType() == TargetType)
                foreach (var f in f1) list.Add(OwnerFormula.CreateOperator(f, f2));
            else if (OwnerFormula.Satisfy(Operator.OperatorLaw.DistributiveToRight) && f2.GetType() == TargetType)
                foreach (var f in f2) list.Add(OwnerFormula.CreateOperator(f1, f));
            else
                return null;

            return TargetFormula.CreateOperator(list.ToArray());
        }

        public override string Information { get { return "分配則を適用します。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("a*(x+y)"),
                Formula.Parse("a*x+a*y")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("(x+y)*(x+y)"),
                Formula.Parse("(x+y)*x + (x+y)*y") // この変形時点で積が和となるので終了。さらなる展開は子数式変形でおこなわれる。
                );
        }

        public override Rule GetClone() { return new DistributivePropertyRule(typeof(Product), typeof(Sum)); }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return new DistributivePropertyRule(typeof(Product), typeof(Sum)); }

        /// <summary>
        /// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
        /// </summary>
        protected override bool RetryAll { get { return false; } }

        /// <summary>
        /// このルールの変形において、組み合わせが逆転しても結果が変わらないか否かを取得します。
        /// </summary>
        /// TODO: 名前がややこしすぎる。もっとわかりやすく直観的な名称に。
        protected override bool Reversible { get { return true; } }

        /// <summary>
        /// ルール挙動を一意に決定する識別名を取得します。通常、型のFullNameですが、ルールに設定されたプロパティなどにより挙動が変わる場合には、状態に応じて一意の文字列を返すよう、オーバーライドしてください。
        /// </summary>
        public override string DistinguishedText 
        {
            get
            { 
                if (OwnerType == null || TargetType == null) return base.DistinguishedText;
                return base.DistinguishedText + "::" + OwnerType.ToString() + "::" + TargetType.ToString(); 
            }
        }

    }
}
