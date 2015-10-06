using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Clapte.Solvers;

namespace GoodSeat.Clapte.Solvers.Processes
{
	/// <summary>
	/// 通常の式に対する計算処理を表します。
	/// </summary>
	public class CalculateFormulaProcess : Process
	{
		/// <summary>
		/// 通常の式に対する計算処理を表します。
		/// </summary>
		/// <param name="owner">処理の保持者となるソルバ。</param>
		/// <param name="maxTime">計算を中止する時間[ms]。</param>
		/// <param name="deformTokens">変形を試みる可変数の変形識別トークン。</param>
		public CalculateFormulaProcess(Solver owner, double maxTime, params DeformToken[] deformTokens) 
			: base(owner) 
		{
			Tokens = new List<DeformToken>(deformTokens);

            MaxTime = maxTime;
		}

        DateTime _calcStartTime;

		/// <summary>
		/// 計算処理に用いる変形識別トークンのリストを取得します。
		/// 計算に変化が生じるまで、インデックス0のトークンから変形処理を試みます。
		/// </summary>
		public List<DeformToken> Tokens { get; private set; }

        /// <summary>
        /// 計算を中止する時間[ms]を設定若しくは取得します。
        /// </summary>
        public double MaxTime { get; set; }

		/// <summary>
		/// 計算対象となった入力数式を対象として、処理を行います。
		/// </summary>
		/// <param name="input">処理対象の入力数式。</param>
		/// <returns>エラー情報。エラーのない場合、null。</returns>
		public override Error EvaluateFormula(ref Formula input) 
		{
            _calcStartTime = DateTime.Now;
            Formula.FormulaProcessing += Formula_FormulaProcessing;

            try
            {
                Formula result = null;
                for (int i = 0; i < Tokens.Count; i++)
                {
                    result = input.DeformFormula(Tokens[i]);
                    if (result != input) break;
                }

                input = result;
                return null;
            }
            finally
            {
                Formula.FormulaProcessing -= Formula_FormulaProcessing;
            }
		}

        void Formula_FormulaProcessing(Formula sender, EventArgs e, ref bool Cancel)
        {
#if DEBUG
            return;
#endif
            if (DateTime.Now - _calcStartTime > TimeSpan.FromMilliseconds(MaxTime))
            {
                throw new Exception("一定時間以上計算が終了しませんでした。計算式が複雑すぎます。");
            }
        }

	}
}

