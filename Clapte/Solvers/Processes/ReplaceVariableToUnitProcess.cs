using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Clapte.Models.Formulas;

namespace GoodSeat.Clapte.Solvers.Processes
{
    /// <summary>
    /// 数式中の変数を、同名の単位に置き換える処理を表します。
    /// </summary>
    public class ReplaceVariableToUnitProcess : Process
    {
        /// <summary>
        /// 動作モードを表します。
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// 明示的に除外された変数を除き、すべての記号を単位として認識します。
            /// </summary>
            AllAutoDetect,
            /// <summary>
            /// 単位表に登録された記号のみを単位として認識します。
            /// </summary>
            OnlyRegisterd,
            /// <summary>
            /// 明示的に単位として指定された記号のみを単位として認識します。
            /// </summary>
            OnlyExplicit
        }

        /// <summary>
        /// 数式中の変数を、同名の単位に置き換える処理を初期化します。
        /// </summary>
        public ReplaceVariableToUnitProcess(Solver owner, Mode mode) : base(owner)
        {
            IgnoreVariableNames = new List<string>();
            ReplaceMode = mode;
        }

        /// <summary>
        /// 置き換え対象から除外する変数名リストを設定もしくは取得します。
        /// </summary>
        public List<string> IgnoreVariableNames { get; set; }

        /// <summary>
        /// 動作モードを設定若しくは取得します。
        /// </summary>
        public Mode ReplaceMode { get; set; }

        /// <summary>
        /// 計算対象となった入力数式を対象として、処理を行います。
        /// </summary>
        /// <param name="input">処理対象の入力数式。</param>
        /// <returns>エラー情報。エラーのない場合、null。</returns>
        public override Error CheckInputFormula(ref Formula input)
        {
            if (ReplaceMode == Mode.OnlyExplicit) return null;

            if (input is Equal) // 方程式扱いの判定
            {
                // 未定義の変数が一つだけなら方程式とみなし、単位には変換しない
                int countVariable = input.GetExistFactors<Variable>().Count();
                if (countVariable == 1) return null;

                var constants = input.GetExistFactors<Constant>();
                int countConstant = constants.Count();
                var units = input.GetExistFactors<Unit>();
                int countUnit = units.Count();

                // 定数が一つだけなら、定数ではなく変数とみなす
                if (countVariable == 0 && countUnit == 0 && countConstant == 1)
                {
                    var constant = constants.First();
                    input = input.Substituted(constant, new Variable(constant.DistinguishedName));
                    return null;
                }

                // 単位が一つだけなら、単位ではなく変数とみなす
                if (countVariable == 0 && countConstant == 0 && countUnit == 1)
                {
                    var unit = units.First();
                    input = input.Substituted(unit, new Variable(unit.ToString()));
                    return null;
                }
            }

            // 変数を単位に変換
            foreach (var variable in input.GetExistFactors<Variable>())
            {
                if (variable.Mark == SolveEquationProcess.PermanentSolveTarget) continue; // ただし、?は常に除外
                if (variable is VariableWithDefine) continue;
                if (IgnoreVariableNames.Contains(variable.Mark)) continue;

                var unit = new Unit(variable.Mark);
                if (ReplaceMode == Mode.OnlyRegisterd && unit.BelongTable == null) continue;

                input = input.Substituted(variable, unit, false);
            }
            foreach (var variableWithDefine in input.GetExistFactors<VariableWithDefine>())
            {
                var def = variableWithDefine.Define;
                foreach (var variable in def.GetExistFactors<Variable>())
                {
                    if (IgnoreVariableNames.Contains(variable.Mark)) continue;

                    var unit = new Unit(variable.Mark);
                    if (ReplaceMode == Mode.OnlyRegisterd && unit.BelongTable == null) continue;

                    def = def.Substituted(variable, unit, false);
                }
                variableWithDefine.Define = def;
            }
            return null;
        }
    }
}
