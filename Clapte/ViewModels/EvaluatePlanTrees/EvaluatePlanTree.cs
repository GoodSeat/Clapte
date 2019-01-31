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

namespace GoodSeat.Clapte.ViewModels.EvaluatePlanTrees
{
    /// <summary>
    /// 数式セルの非同期評価順構成ツリーを表します。
    /// </summary>
    public class EvaluatePlanTree
    {
        /// <summary>
        /// 数式セルの非同期評価順構成ツリーを初期化します。
        /// </summary>
        /// <param name="target">評価対象となる数式セルリスト。</param>
        /// <param name="postCells">この数式セルより後に配置されている数式セルリスト。</param>
        public EvaluatePlanTree(FormulaCellViewModel target, params FormulaCellViewModel[] postCells)
        {
            TargetFormulaCell = target;
            Children = new List<EvaluatePlanTree>();
        }

        public FormulaCellViewModel TargetFormulaCell { get; set; }

        /// <summary>
        /// この数式評価にあたって、先に評価する必要のある直近の親評価ツリーを取得します。
        /// </summary>
        private EvaluatePlanTree Parent { get; set; }

        /// <summary>
        /// その数式評価にあたって、この評価ツリーを先に評価する必要のある評価ツリーリストを取得します。
        /// </summary>
        public List<EvaluatePlanTree> Children { get; private set; }

        private void CreateTree(params FormulaCellViewModel[] postCells)
        {
            foreach (var cell in postCells)
            {
                foreach (var variable in cell.Target.GetAllReferenceVariableNames())
                {
                }
            }

        }



    }
}

