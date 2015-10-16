using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formats;

namespace GoodSeat.Clapte.Solvers.Processes
{
    /// <summary>
    /// 出力における単位表記タイプを表します。
    /// </summary>
    public enum UnitFormatType
    {
        /// <summary>判定なし。</summary>
        None,
        /// <summary>自動。</summary>
        Auto,
        /// <summary>空白で括る。</summary>
        EncloseWithSpace,
        /// <summary>丸括弧で括る。</summary>
        EncloseWithParentheses,
        /// <summary>角括弧で括る。</summary>
        EncloseWithBrackets
    }

    /// <summary>
    /// 出力書式における単位の表記を設定する処理を表します。
    /// </summary>
    public class SetUnitFormatProcess : Process
    {
        /// <summary>
        /// 出力書式における単位の表記を設定する処理を初期化します。
        /// </summary>
		/// <param name="owner">処理の保持者となるソルバ。</param>
		/// <param name="type">出力の表記タイプ。</param>
        public SetUnitFormatProcess(Solver owner, UnitFormatType type)
            : base(owner)
        {
            OutputType = type;
            CurrentType = UnitFormatType.None;
        }

		/// <summary>
		/// 出力時の表記タイプを設定もしくは取得します。
		/// </summary>
		public UnitFormatType OutputType { get; set; }

		/// <summary>
		/// 現在処理中の計算に対する表記タイプを設定もしくは取得します。
		/// </summary>
        private UnitFormatType CurrentType { get; set; }

        public override Error CheckInputFormula(ref Formula input)
        {
            foreach (var f in input.GetExistFactor(f => f.IsUnit()))
            {
                var prop = f.Format.PropertyOf<Bracket>();

                if (prop.Type == Bracket.Kind.Parentheses) CurrentType = UnitFormatType.EncloseWithParentheses;
                else if (prop.Type == Bracket.Kind.Brackets) CurrentType = UnitFormatType.EncloseWithBrackets;
                else CurrentType = UnitFormatType.EncloseWithSpace;

                break;
            }
            return base.CheckInputFormula(ref input);
        }

        public override Error CheckOutputFormula(ref Formula output)
        {
            var type = OutputType;
            if (type == UnitFormatType.Auto) type = CurrentType;

            if (type != UnitFormatType.None)
            {
                var notargets = new List<Formula>();
                foreach (var f in output.GetExistFactor(f => f.IsUnit()))
                {
                    if (notargets.Contains(f)) continue;

                    if (type == UnitFormatType.EncloseWithSpace)
                        f.Format.SetProperty(new Bracket(Bracket.Kind.Denial));
                    else if (type == UnitFormatType.EncloseWithParentheses)
                        f.Format.SetProperty(Bracket.Parenthese);
                    else if (type == UnitFormatType.EncloseWithBrackets)
                        f.Format.SetProperty(Bracket.SquareBracket);

                    foreach (var child in f.GetExistFactor(p => p.IsUnit())) notargets.Add(child);
                }
            }
            return base.CheckOutputFormula(ref output);
        }
    }
}
