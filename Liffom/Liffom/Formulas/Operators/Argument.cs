// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formats;

namespace GoodSeat.Liffom.Formulas.Operators
{
    /// <summary>
    /// 関数の作成などに用いる引数を表します。
    /// </summary>
    [Serializable()]
    public class Argument : OperatorMultiple
    {
        /// <summary>
        /// 引数を作成します。
        /// </summary>
        /// <param name="formulas">引数を構成する可変数の数式。</param>
        public Argument(params Formula[] formulas) : base(formulas) { }
        
        /// <summary>
        /// 演算が満たす法則を取得します。
        /// </summary>
        public override OperatorLaw Law { get { return OperatorLaw.None; } }
                
        /// <summary>
        /// 演算の評価優先度を表す数値を取得します。
        /// 原則として、OperatorPriorityの値を返してください。
        /// </summary>
        public override int EvaluatePriority { get { return (int)OperatorPriority.Argument; } }

        /// <summary>
        /// 引数を指定して、演算を生成します。
        /// </summary>
        /// <param name="args">初期化に用いる可変数の数式。</param>
        public override Operator CreateOperator(params Formula[] args)
        {
            return new Argument(args);
        }

        /// <summary>
        /// 指定した数式を、引数の構成数式に追加します。
        /// </summary>
        /// <param name="formula">追加数式。</param>
        public void Add(Formula formula)
        {
            Formulas.Add(formula);
        }

        /// <summary>
        /// 引数の構成数式リストを取得します。
        /// </summary>
        public new List<Formula> Formulas
        {
            get { return base.Formulas; }
        }

        public override string GetText()
        {
            return string.Join(", ", this);
        }

        public override Formula DefaultValue
        {
            get { return new Null(); }
        }
    }
}
