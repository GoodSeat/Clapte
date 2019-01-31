// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Clapte.Models
{
    /// <summary>
    /// 継続行を意味する数式セルの内容を表します。
    /// </summary>
    public class FormulaCellContentContinuation : FormulaCellContent
    {
        /// <summary>
        /// 継続行を意味する数式セルの内容を初期化します。
        /// </summary>
        /// <param name="formulaText">継続するテキスト。</param>
        public FormulaCellContentContinuation(string formulaText) : base(formulaText, null, null, null)
        {
            ResultText = "";
        }

        /// <summary>
        /// 指定文字列から、数式セルの内容を初期化して取得します。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        /// <param name="solver">数式の構文解析に用いるソルバ。</param>
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        /// <returns>初期化された数式セル内容オブジェクト。</returns>
        protected override FormulaCellContent CreateFrom(string formulaText, Solver solver, params FormulaCell[] previous)
        {
            return new FormulaCellContentContinuation(formulaText.Trim().TrimEnd('_'));
        }

        /// <summary>
        /// 指定文字列から、評価にあたって計算処理の不要な数式セルの内容の初期化を試みます。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        /// <param name="solver">数式の構文解析に用いるソルバ。</param>
        /// <param name="previous">前方に宣言されているか変数の数式セル。</param>
        /// <returns>初期化された数式セル内容オブジェクト。生成対象とならなかった場合には、null。</returns>
        protected override FormulaCellContent TryCreateNoCalculateTarget(string formulaText, Solver solver, params FormulaCell[] previous)
        {
            if (formulaText.Trim().EndsWith(" _")) return CreateFrom(formulaText, solver, previous);
            return null;
        }

        /// <summary>
        /// この数式セルが実際の評価を行わず、後続の行に評価を移譲するか否かを取得します。
        /// </summary>
        public override bool IsContinuation { get { return true; } }


    }
}
