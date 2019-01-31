// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Constants.Rules;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Utilities;

namespace GoodSeat.Liffom.Formulas.Constants
{
    /// <summary>
    /// 定数を表します。
    /// </summary>
    [Serializable()]
    public abstract class Constant : AtomicFormula
    {
        static List<Constant> s_constantList;

        /// <summary>
        /// 利用可能なすべての定数を返す反復を取得します。
        /// </summary>
        /// <returns>システム内で利用可能な定数を返す反復子。</returns>
        /// <remarks>Liffom.dll内、及び同ディレクトリ内のクラスライブラリ中に存在する定数を探索します。</remarks>
        public static IEnumerable<Constant> GetEnableConstants()
        {
            if (s_constantList == null)
            {
                s_constantList = new List<Constant>();
                foreach (Reflector<Constant> constInfo in Reflector<Constant>.FindReflectors())
                {
                    Constant c = constInfo.CreateInstance();
                    s_constantList.Add(c);
                }
            }

            foreach (var constant in s_constantList) yield return constant;
        }

        /// <summary>
        /// この定数として識別する文字列を取得します。
        /// </summary>
        public abstract string DistinguishedName { get;  }

        /// <summary>
        /// この定数として読み込む文字列を全て返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetAllDistinguishedNames() { yield return DistinguishedName; }

        /// <summary>
        /// 定数の説明を取得します。
        /// </summary>
        public abstract string Information { get; }
        
        /// <summary>
        /// 指定処理に関連するルールをすべて返す反復子を取得します。
        /// </summary>
        /// <param name="deformToken">変形識別トークン。</param>
        /// <param name="sender">ルール適用対象となる最上位親数式。</param>
        /// <param name="history">変形履歴情報。</param>
        /// <returns>変形に関連するルールを返す反復子。</returns>
        public override IEnumerable<Rule> GetRelatedRulesOf(DeformToken deformToken, Formula sender, DeformHistory history) 
        {
            foreach (var rule in base.GetRelatedRulesOf(deformToken, sender, history)) yield return rule;

            if (deformToken.Has<NumerateToken>()) yield return ConstantNumerateRule.Entity;
        }

        /// <summary>
        /// 定数値の表す数値を取得します。
        /// </summary>
        public abstract Formula Value { get; }

        public override string GetText() { return DistinguishedName; }

        /// <summary>
        /// 数式の一意性評価に用いる文字列で、同じ型の数式同士の一意性を表す文字列を取得します。
        /// </summary>
        /// <returns>数式を一意に区別する文字列。</returns>
        protected override string OnGetUniqueText() { return ""; }

        protected override CompareResult IsLargerThan(Formula other)
        {
            if (other is Constant)
            {
                int compare = ToString().CompareTo(other.ToString());

                if (compare > 0) return CompareResult.Larger;
                else if (compare < 0) return CompareResult.Smaller;
                else return CompareResult.Same;
            }
            else
                return base.IsLargerThan(other);
        }
    }


}
