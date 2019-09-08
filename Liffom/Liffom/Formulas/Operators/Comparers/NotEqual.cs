// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Liffom.Formulas.Operators.Comparers
{
    /// <summary>
    /// 非等式記号による数式のつながりを表します。
    /// </summary>
    [Serializable()]
    public class NotEqual : Comparer
    {
        /// <summary>
        /// 非等式記号による数式のつながりを初期化します。
        /// </summary>
        /// <param name="lhs">左辺。</param>
        /// <param name="rhs">右辺。</param>
        public NotEqual(Formula lhs, Formula rhs)
            : base(lhs, rhs)
        { }

        /// <summary>
        /// 引数を指定して、演算を生成します。
        /// </summary>
        /// <param name="args">初期化に用いるか変数の数式。</param>
        public override Operator CreateOperator(params Formula[] args)
        {
            return new NotEqual(args[0], args[1]);
        }

        public override string GetText()
        {
            return LeftHandSide.ToString() + "≠" + RightHandSide.ToString();
        }

        public override Judge GetJudge(DeformToken token)
        {
            Formula f = (LeftHandSide - RightHandSide).DeformFormula(token);
            f = f.ClearUnit();

            if (f is Numeric)
            {
                if ((f as Numeric).Figure == 0) return Judge.False;
                else return Judge.True;
            }
            else
                return Judge.None;
        }
    }
}
