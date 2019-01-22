using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Processes;

namespace GoodSeat.Clapte.Solvers.Processes
{
    /// <summary>
    /// 目標単位の認識と変換の処理を表します。
    /// </summary>
    public class ConvertToSpecifiedUnitProcess : Process
    {
        /// <summary>
        /// 目標単位の指定文字列を"targetUnit"グループにキャプチャする正規表現を設定もしくは取得します。
        /// </summary>
        Regex TargetUnitRegex { get; set; }

        /// <summary>
        /// 現在の目標単位を設定もしくは取得します。
        /// </summary>
        public Formula CurrentTargetUnit { get; private set; }

        /// <summary>
        /// 単位変換実施後の数式に対して適用する変形の識別トークンを設定もしくは取得します。
        /// </summary>
        public DeformToken ApplyDeformToken { get; set; }

        /// <summary>
        /// 目標単位の認識と変換の処理を初期化します。
        /// </summary>
        /// <param name="owner">処理の保持者となるソルバ。</param>
        public ConvertToSpecifiedUnitProcess(Solver owner) : this(owner, null) { }

        /// <summary>
        /// 目標単位の認識と変換の処理を初期化します。
        /// </summary>
        /// <param name="owner">処理の保持者となるソルバ。</param>
        /// <param name="token">変換時に適用する変形トークン。</param>
        public ConvertToSpecifiedUnitProcess(Solver owner, DeformToken token) : base(owner)
        {
            TargetUnitRegex = new Regex(@"^\s*\[(?<targetUnit>[^[\]]+)\]");
            ApplyDeformToken = token;
        }

        /// <summary>
        /// 入力された文字列を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力文字列。</param>
        /// <param name="onlyCheckInput">数式の構文解析のみを目的とし、文字列のチェックのみを行うか否か。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckInputText(ref string input, bool onlyCheckInput)
        {
            LastConvertHistories = null;

            if (!onlyCheckInput) CurrentTargetUnit = null;

            Match match = TargetUnitRegex.Match(input);
            if (!match.Success) return base.CheckInputText(ref input, onlyCheckInput);
            
            string targetUnit = match.Groups["targetUnit"].Value;
            var parsed = Owner.Parser.Parse(targetUnit);

            if (!(parsed is Argument)) // parsedが引数の場合、マトリクスもしくは行ベクトルの可能性
            {
                var targetUnitFormula = parsed;

                // 変数を同名の単位で置き換え
                foreach (var variable in targetUnitFormula.GetExistFactors<Variable>())
                    targetUnitFormula = targetUnitFormula.Substituted(variable, new Unit(variable.Mark));
                // 定数を同名の単位で置き換え
                foreach (var constant in targetUnitFormula.GetExistFactors<Constant>())
                    targetUnitFormula = targetUnitFormula.Substituted(constant, new Unit(constant.DistinguishedName));

                if (!targetUnitFormula.IsUnit(true)) return new Error(Error.Level.Abort, "目標単位として指定された文字列を、単位として認識できません。");

                input = TargetUnitRegex.Replace(input, "");

                if (!onlyCheckInput) CurrentTargetUnit = targetUnitFormula;
            }

            return base.CheckInputText(ref input, onlyCheckInput);
        }

        /// <summary>
        /// 単位換算で行なった、元の数式に対する数式変形履歴マップを取得します。
        /// </summary>
        public List<DeformHistory> LastConvertHistories { get; private set; }

        /// <summary>
        /// 計算対象となった入力数式を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckOutputFormula(ref Formula output)
        {
            if (CurrentTargetUnit != null)
            {
                ConvertToTargetUnit convert = new ConvertToTargetUnit(true);
                convert.ApplyDeformToken = ApplyDeformToken;

                var result = convert.Do(output, CurrentTargetUnit);
                output = result;

                LastConvertHistories = convert.ConvertHistories;
            }
            return base.CheckOutputFormula(ref output);
        }

    }
}
