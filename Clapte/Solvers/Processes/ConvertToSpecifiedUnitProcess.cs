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

		public override Error CheckInputText(ref string input)
		{
			CurrentTargetUnit = null;

			Match match = TargetUnitRegex.Match(input);
			if (!match.Success) return base.CheckInputText(ref input);
			
			string targetUnit = match.Groups["targetUnit"].Value;
			var parsed = Owner.Parser.Parse(targetUnit);

			if (!(parsed is Argument)) // parsedが引数の場合、マトリクスもしくは行ベクトルの可能性
			{
				CurrentTargetUnit = parsed;

				// 変数を同名の単位で置き換え
				foreach (var variable in CurrentTargetUnit.GetExistFactor<Variable>())
					CurrentTargetUnit = CurrentTargetUnit.Substituted(variable, new Unit(variable.Mark));
				// 定数を同名の単位で置き換え
				foreach (var constant in CurrentTargetUnit.GetExistFactor<Constant>())
					CurrentTargetUnit = CurrentTargetUnit.Substituted(constant, new Unit(constant.DistinguishedName));

				if (!CurrentTargetUnit.IsUnit(true)) return new Error(Error.Level.Abort, "目標単位として指定された文字列を、単位として認識できません。");

				input = TargetUnitRegex.Replace(input, "");
			}

			return base.CheckInputText(ref input);
		}

		public override Error CheckOutputFormula(ref Formula output)
		{
			if (CurrentTargetUnit != null)
			{
				ConvertToTargetUnit convert = new ConvertToTargetUnit();
                convert.ApplyDeformToken = ApplyDeformToken;

				var result = convert.Do(output, CurrentTargetUnit);
                output = result;
			}
			return base.CheckOutputFormula(ref output);
		}

	}
}
