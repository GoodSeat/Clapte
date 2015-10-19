using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;

namespace GoodSeat.Liffom.Formulas.Operators.Comparers
{
    /// <summary>
    /// 等式記号による数式のつながりを表します。
    /// </summary>
    [Serializable()]
    public class Equal : Comparer
    {
        /// <summary>
        /// 等号記号を初期化します。
        /// </summary>
        /// <param name="lhs">左辺。</param>
        /// <param name="rhs">右辺。</param>
        public Equal(Formula lhs, Formula rhs)
            : base(lhs, rhs)
        { }

        /// <summary>
        /// 引数を指定して、演算を生成します。
        /// </summary>
        /// <param name="args">初期化に用いるか変数の数式。</param>
        public override Operator CreateOperator(params Formula[] args)
        {
            return new Equal(args[0], args[1]);
        }

        public override string GetText()
        {
            return LeftHandSide.ToString() + "=" + RightHandSide.ToString();
        }
        
        public override Judge GetJudge(DeformToken token)
        {
            Formula left = LeftHandSide.DeformFormula(token);
            Formula right = RightHandSide.DeformFormula(token);
            
            if (left == right) return Judge.True;
            else return Judge.None;
        }
    }
}
