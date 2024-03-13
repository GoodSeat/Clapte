// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2024 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.Solvers.Processes;
using GoodSeat.Liffom.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Clapte.Models
{
    /// <summary>
    /// 評価有無の制御等を行うための数式セルの内容を表します。
    /// </summary>
    public class FormulaCellContentOperation : FormulaCellContent
    {
        /// <summary>
        /// 制御区分を表します。
        /// </summary>
        public enum OperationType
        {
            IF, ELSEIF, ELSE, ENDIF
        }

        /// <summary>
        /// 方程式の求解による変数の定義を表す数式セルの計算/定義内容を初期化します。
        /// </summary>
        /// <param name="formulaText">初期化対象のテキスト。</param>
        /// <param name="f">対象の数式。</param>
        /// <param name="previous">前方に宣言されている可変数の数式セル。</param>
        protected internal FormulaCellContentOperation(string formulaText, Formula f, FormulaCell[] previous) : base(formulaText, f, f, previous, null, null)
        {
        }

        /// <summary>
        /// 関連する制御セルを取得します。
        /// </summary>
        /// <param name="previous">前方の数式セルリスト。</param>
        /// <returns>関連する制御セルリスト。</returns>
        public static List<FormulaCell> GetRelateOperations(bool searchUpLv, params FormulaCell[] previous)
        {
            int dlv = searchUpLv ? 1 : 0;
            int lv = 0;
            var lstRelated = new List<FormulaCell>();
            foreach (var cell in previous.Reverse())
            {
                var op = cell.Content as FormulaCellContentOperation;
                if (op == null) continue;

                if      (op.Type == OperationType.IF   ) --lv;
                else if (op.Type == OperationType.ENDIF) ++lv;

                if ((op.Type == OperationType.IF && lv < 0 - dlv)
                 || (op.Type != OperationType.IF && lv <= 0 - dlv))
                {
                    lstRelated.Insert(0, cell);
                }
                if (lv < 0 - dlv) break;
            }
            return lstRelated;
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
            var lstRelated = GetRelateOperations(false, previous);

            Func<int, OperationType, FormulaCellContentOperation> init = (n, t) =>
            {
                Formula f;
                if (!solver.TryParse(formulaText.Substring(n), out f)) return null;

                return new FormulaCellContentOperation(formulaText.Substring(n), f, previous)
                {
                    Type = t,
                    RelatedContents = lstRelated
                };
            };

            if (formulaText.ToUpper().StartsWith("$IF "))
            {
                lstRelated.Clear();
                return init(4, OperationType.IF);
            }
            else if (formulaText.ToUpper().StartsWith("$ELIF "))
            {
                return init(6, OperationType.ELSEIF);
            }
            else if (formulaText.ToUpper().StartsWith("$ELSEIF "))
            {
                return init(8, OperationType.ELSEIF);
            }
            else if (formulaText.ToUpper().StartsWith("$ELSE"))
            {
                return new FormulaCellContentOperation("", null, previous)
                {
                    Type = OperationType.ELSE,
                    RelatedContents = lstRelated
                };
            }
            else if (formulaText.ToUpper().StartsWith("$ENDIF"))
            {
                return new FormulaCellContentOperation(formulaText, null, previous)
                {
                    Type = OperationType.ENDIF,
                    RelatedContents = lstRelated
                };
            }
            else
            {
                return null;
            }
        }

        List<FormulaCell> RelatedContents { get; set; } = new List<FormulaCell>();

        protected override List<FormulaCell> GetAdditionalDependCells() { return RelatedContents; }  

        protected override Result OnEvaluate(Solver solver)
        {
            if (!IsEvaluateTarget) return base.OnEvaluate(solver);

            if (Type == OperationType.ENDIF)
            {
                Condition = false;
                return new Result(Result.Level.Success, "", null);
            }
            else if (RelatedContents.Any(c => (c.Content as FormulaCellContentOperation).Condition))
            {
                Condition = false;
                return new Result(Result.Level.Success, "$FALSE", null);
            }
            else if (Type == OperationType.ELSE)
            {
                Condition = true;
                return new Result(Result.Level.Success, "$TRUE", null);
            }

            Result result = null;

            // 方程式として検知されないようにする
            var proc = solver.GetProcessOf<EvaluateUserDefineProcess>();
            if (proc != null) proc.ForceSubstitute = true;
            try
            {
                result = base.OnEvaluate(solver);
            }
            finally
            {
                if (proc != null) proc.ForceSubstitute = false;
            }

            if (result.ResultLevel == Result.Level.Success)
            {
                if (result.ResultFormula is Numeric)
                {
                    Condition = (result.ResultFormula != 0);
                    return new Result(Result.Level.Success, "$" + Condition.ToString().ToUpper() + " (" + result.ResultText + ")", null);
                }
                else
                {
                    Condition = false;
                    return new Result(Result.Level.Success, "$FALSE (結果が数値となりませんでした：" + result.ResultText + ")", null);
                }
            }
            else
            {
                Condition = false;
                return new Result(Result.Level.Success, "$FALSE (" + result.ResultText + ")", null);
            }
        }

        /// <summary>
        /// 制御セルの制御区分を取得します。
        /// </summary>
        public OperationType Type { get; private set; }

        /// <summary>
        /// この制御セルが有効となるか否かを取得します。
        /// </summary>
        public bool Condition { get; private set; }

        /// <summary>
        /// この数式セルの項目が、評価対象とすべきか否かを取得します。
        /// </summary>
        //public override bool IsEvaluateTarget
        //{
        //    get { return true; }
        //}

    }
}
