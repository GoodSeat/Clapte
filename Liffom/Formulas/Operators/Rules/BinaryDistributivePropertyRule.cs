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
    public class BinaryDistributivePropertyRule : Rule
    {
        /// <summary>
        /// 分配則を規定するルールを初期化します。
        /// </summary>
        private BinaryDistributivePropertyRule() { }

        /// <summary>
        /// 分配則を規定するルールを初期化します。
        /// </summary>
        /// <param name="ownerType">分配則を満たす演算の型。</param>
        /// <param name="targetType">分配則の成立対象となる演算の型。</param>
        public BinaryDistributivePropertyRule(Type ownerType, Type targetType)
        {
            OwnerType = ownerType;
            TargetType = targetType;
        }

        private Type OwnerType { get; set; }

        private Type TargetType { get; set; }

        private Operator OwnerFormula { get; set; }

        private Operator TargetFormula { get; set; }

        protected internal override bool IsTargetTypeFormula(Formula target) 
        {
            if (!(target is OperatorBinary)) return false;

            OwnerFormula = target as Operator;
            return target.GetType() == OwnerType;
        }

        protected override Formula OnTryMatchRule(Formula target)
        {
            List<Formula> list = new List<Formula>();

            OperatorBinary binary = target as OperatorBinary;
            var f1 = binary.Forword;
            var f2 = binary.Backword;

            if (OwnerFormula.Satisfy(Operator.OperatorLaw.DistributiveToLeft) && f1.GetType() == TargetType)
            {
                TargetFormula = f1 as Operator;
                foreach (var f in f1) list.Add(OwnerFormula.CreateOperator(f, f2));
            }
            else if (OwnerFormula.Satisfy(Operator.OperatorLaw.DistributiveToRight) && f2.GetType() == TargetType)
            {
                TargetFormula = f2 as Operator;
                foreach (var f in f2) list.Add(OwnerFormula.CreateOperator(f1, f));
            }
            else
                return null;

            return TargetFormula.CreateOperator(list.ToArray());
        }

        public override string Information { get { return "分配則を規定するルールです。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("(a * b)^x"),
                Formula.Parse("a^x * b^x")
                );
        }

        public override Rule GetClone() { return new BinaryDistributivePropertyRule(typeof(Power), typeof(Product)); }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return new BinaryDistributivePropertyRule(typeof(Power), typeof(Product)); }

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

