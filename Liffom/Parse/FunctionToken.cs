using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formats;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 関数名トークンを表します。
    /// </summary>
    public class FunctionToken : Token
    {
        /// <summary>
        /// 関数名トークンを初期化します。
        /// </summary>
        /// <param name="targetText">トークン化のもととなった文字列。</param>
        /// <param name="sample">関連付けられる関数。</param>
        public FunctionToken(string targetText, Function sample) 
            : base(targetText) 
        {
            SampleFunction = sample;
        }

        /// <summary>
        /// 対象の関数を設定もしくは取得します。
        /// </summary>
        private Function SampleFunction { get; set; }


        /// <summary>
        /// 直後の数式を引数として、関数を解析した数式トークンに置き換えます。
        /// </summary>
        /// <returns>変更があったか否か。</returns>
        public override bool ModifyTokenRelation()
        {
            var formulaToken = NextToken as FormulaToken;
            if (formulaToken == null) return false;

            var f = formulaToken.ParsedFormula;
            f.Format.RemoveIndividualSettingOf<Bracket>();
            var argument = f as Argument;
            if (argument == null) argument = new Argument(f);

            SampleFunction.Argument = argument;
            var newFunctionToken = new FormulaToken(GetBaseText(formulaToken), SampleFunction);

            InsertPrevious(newFunctionToken);
            NextToken.Remove();
            Remove();
            return true;
        }
    }
}
