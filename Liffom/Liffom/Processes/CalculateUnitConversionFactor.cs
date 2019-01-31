// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Liffom.Processes
{
    /// <summary>
    /// 単位間の変換係数算出処理を表します。
    /// </summary>
    public class CalculateUnitConversionFactor : Process
    {
        /// <summary>
        /// 単位間の変換係数算出処理を初期化します。
        /// </summary>
        public CalculateUnitConversionFactor() { }

        /// <summary>
        /// 一意のユーザー情報を指定して、複数の同時呼び出しを許可するか否かを取得します。
        /// </summary>
        public override bool IsSupportMultipleConcurrentInvocations { get { return false; } }

        /// <summary>
        /// 処理対象とする数式数の下限値を取得します。
        /// </summary>
        public override int TargetArgumentsMinQty { get { return 1; } }

        /// <summary>
        /// 処理対象とする数式数の上限値を取得します。
        /// </summary>
        public override int TargetArgumentsMaxQty { get { return 2; } }



        List<Unit> _fromMolecular;
        List<Unit> _fromDenominator;
        List<Unit> _toMolecular;
        List<Unit> _toDenominator;

        Formula _modifyFrom;        // 変換元単位の分母分子間の約分補正係数
        Formula _modifyTo;          // 変換先単位の分母分子間の約分補正係数
        Formula _modifyMolecular;   // 変換元と変換先単位の分子同士の約分補正係数
        Formula _modifyDenominator; // 変換元と変換先単位の分母同士の約分補正係数

        List<Unit> _fromMolecularMinimum;
        List<Unit> _fromDenominatorMinimum;
        List<Unit> _toMolecularMinimum;
        List<Unit> _toDenominatorMinimum;

        Formula _modifyFromMinimum;        // 変換元単位の分母分子間の約分補正係数
        Formula _modifyToMinimum;          // 変換先単位の分母分子間の約分補正係数
        Formula _modifyMolecularMinimum;   // 変換元と変換先単位の分子同士の約分補正係数
        Formula _modifyDenominatorMinimum; // 変換元と変換先単位の分母同士の約分補正係数

        /// <summary>
        /// 変換時の加算値を取得します。変換先の単位扱い数式は含まれません。
        /// </summary>
        public Formula ConversionAddition { get; private set; }

        /// <summary>
        /// 単位変換係数を取得します。
        /// </summary>
        public Formula ConversionFactor { get; private set; }

        /// <summary>
        /// 数式中の単位を目標単位に変換します。
        /// </summary>
        protected override Formula OnDo(object userState, params Formula[] targets)
        {
            Formula from = targets[0]; // 変換元単位
            Formula to = null; // 目標単位
            if (targets.Length > 1) to = targets[1];

            if (from != null && !from.IsUnit(true))
                throw new FormulaProcessException(from.ToString() + "は単位扱い数式ではありません。");
            if (to != null && !to.IsUnit(true))
                throw new FormulaProcessException(to.ToString() + "は単位扱い数式ではありません。");

            InitializeField();

            SetAddition(from, to); // 加算値を算出

            BuildUnitList(from, ref _fromMolecular, ref _fromDenominator);
            BuildUnitList(to,   ref _toMolecular,   ref _toDenominator);

            RenewModifyCoefficients();

            int maxCount = RemainedUnitCount * 2;
            int minCount = maxCount;
            int tryCount = 0;
            while (!ConvertSuccessed && tryCount++ < maxCount)
            {
                if (minCount > RemainedUnitCount)
                {
                    minCount = RemainedUnitCount;
                    RegistMinimumMement();
                }

                if (!TryConvertUnit(ref _modifyFrom, ref _modifyTo)) break; // 変換のしようがない場合、あきらめる

                RenewModifyCoefficients();

                maxCount = Math.Max(maxCount, RemainedUnitCount * 2);
            }

            if (!ConvertSuccessed) ReplicateMinmumMemento();

            Formula molecular   = _modifyMolecular   * _modifyTo   * new Product(_fromMolecular.ToArray())   * new Product(_toDenominator.ToArray()) ;
            Formula denominator = _modifyDenominator * _modifyFrom * new Product(_fromDenominator.ToArray()) * new Product(_toMolecular.ToArray());
            molecular   = molecular.Simplify();
            denominator = denominator.Simplify();
            return (molecular / denominator).Simplify();
        }

        /// <summary>
        /// 各種フィールドを初期化します。
        /// </summary>
        private void InitializeField()
        {
            _fromMolecular   = new List<Unit>();
            _fromDenominator = new List<Unit>();
            _toMolecular     = new List<Unit>();
            _toDenominator   = new List<Unit>();

            _modifyFrom         = 1;
            _modifyTo           = 1;
            _modifyMolecular    = 1;
            _modifyDenominator  = 1;

            ConversionAddition = null;
            ConversionFactor = null;
        }

        /// <summary>
        /// 変換過程において、変換処理の完了していない残単位数を取得します。
        /// </summary>
        private int RemainedUnitCount { get { return _fromDenominator.Count + _fromMolecular.Count + _toDenominator.Count + _toMolecular.Count; } }

        /// <summary>
        /// 未変換の残単位数が最小の状態としてスナップショットを残します。
        /// </summary>
        private void RegistMinimumMement()
        {
            if (_fromMolecularMinimum == null)
            {
                _fromMolecularMinimum   = new List<Unit>();
                _fromDenominatorMinimum = new List<Unit>();
                _toMolecularMinimum     = new List<Unit>();
                _toDenominatorMinimum   = new List<Unit>();
            }
            else
            {
                _fromMolecularMinimum.Clear();
                _fromDenominatorMinimum.Clear();
                _toMolecularMinimum.Clear();
                _toDenominatorMinimum.Clear();
            }
            _fromMolecularMinimum.AddRange  (_fromMolecular   );
            _fromDenominatorMinimum.AddRange(_fromDenominator );
            _toMolecularMinimum.AddRange    (_toMolecular     );
            _toDenominatorMinimum.AddRange  (_toDenominator   );

            _modifyFromMinimum          = _modifyFrom;       
            _modifyToMinimum            = _modifyTo;                     
            _modifyMolecularMinimum     = _modifyMolecular;  
            _modifyDenominatorMinimum   = _modifyDenominator;
        }

        /// <summary>
        /// 未変換の残単位数が最小の状態をスナップショットから復元します。
        /// </summary>
        private void ReplicateMinmumMemento()
        {
            _fromMolecular.Clear();
            _fromDenominator.Clear();
            _toMolecular.Clear();
            _toDenominator.Clear();
            
            _fromMolecular.AddRange(_fromMolecularMinimum   );
            _fromDenominator.AddRange(_fromDenominatorMinimum );
            _toMolecular.AddRange(_toMolecularMinimum     );
            _toDenominator.AddRange(_toDenominatorMinimum   );

            _modifyFrom         = _modifyFromMinimum;       
            _modifyTo           = _modifyToMinimum;                     
            _modifyMolecular    = _modifyMolecularMinimum;  
            _modifyDenominator  = _modifyDenominatorMinimum;
        }


        /// <summary>
        /// 変換加算値を設定します。
        /// </summary>
        private void SetAddition(Formula from, Formula to)
        {
            ConversionAddition = 0;
            if (to == null) return;

            foreach (UnitConvertTable table in UnitConvertTable.ValidTables.Values)
                foreach (UnitConvertRecord record in table.GetAllRecords(true))
                    if (record.ConvertUnit == from)
                    {
                        ConversionAddition = table.GetConvertAddition(from, to);
                        return;
                    }
        }

        /// <summary>
        /// 数値による単位変換に成功したか否かを取得します。
        /// </summary>
        private bool ConvertSuccessed
        {
            get
            {
                if (_fromMolecular == null || _fromDenominator == null || _toMolecular == null || _toDenominator == null) return false;
                return (_fromMolecular.Count == 0 && _fromDenominator.Count == 0 && _toMolecular.Count == 0 && _toDenominator.Count == 0);
            }
        }

        /// <summary>
        /// 各種変換係数を更新します。
        /// </summary>
        private void RenewModifyCoefficients()
        {
            _modifyFrom        /= GetModifyToLowestTerms  (ref _fromMolecular,   ref _fromDenominator); // 変換元の分母分子約分補正係数
            _modifyTo          /= GetModifyToLowestTerms  (ref _toMolecular,     ref _toDenominator);   // 変換先の分母分子約分補正係数
            _modifyMolecular   *= GetModifyBetweenUnitList(ref _fromMolecular,   ref _toMolecular);     // 分子同士の約分補正係数
            _modifyDenominator *= GetModifyBetweenUnitList(ref _fromDenominator, ref _toDenominator);   // 分母同士の約分補正係数
        }
        
        /// <summary>
        /// 指定した単位数式から、分母項と分子項リストを構成します。
        /// </summary>
        /// <param name="target">対象の単位扱い数式</param>
        /// <param name="moleculars">分子項リスト</param>
        /// <param name="denominators">分母項リスト</param>
        private void BuildUnitList(Formula target, ref List<Unit> moleculars, ref List<Unit> denominators)
        {
            if (target == null) return;

            if (target is Unit)
            {
                moleculars.Add(target as Unit);
            }
            else if (target is Product)
            {
                foreach (var consist in target)
                    BuildUnitList(consist, ref moleculars, ref denominators);
            }
            else if (target is Power)
            {
                Power pow = target as Power;
                Numeric exponent = pow.Exponent as Numeric;
                if (exponent == null || !exponent.IsInteger)
                    throw new FormulaProcessException(string.Format("目標数式「{0}」を単位として扱うことが出来ません。", target));

                int count = (int)exponent;
                if (Math.Abs(count) > 20)
                    throw new FormulaProcessException(string.Format("単位「{0}」の累乗指数が過大もしくは過小です。", target), new OverflowException());

                for (int i = 0; i < Math.Abs(count); i++)
                {
                    if (count > 0) BuildUnitList(pow.Base, ref moleculars, ref denominators);
                    else BuildUnitList(pow.Base, ref denominators, ref moleculars);
                }
            }

            moleculars.Sort(new Comparison<Unit>(CompareUnit));
            denominators.Sort(new Comparison<Unit>(CompareUnit));
        }

        /// <summary>
        /// 分母項リスト、分子項リスト構成用の単位比較メソッドです。
        /// </summary>
        /// <param name="target1">比較対象の単位1。</param>
        /// <param name="target2">比較対象の単位2。</param>
        /// <returns>比較結果。</returns>
        private int CompareUnit(Unit target1, Unit target2) { return target1.UnitName.CompareTo(target2.UnitName); }
        
        /// <summary>
        /// 指定分子単位リスト、及び分母単位リストから同単位次元を削除し、その変換係数を取得します。
        /// </summary>
        /// <param name="moleculars">分子リスト。</param>
        /// <param name="denominators">分母リスト。</param>
        /// <returns>変換係数。</returns>
        private Formula GetModifyToLowestTerms(ref List<Unit> moleculars, ref List<Unit> denominators)
        {
            Formula modify = 1;
            for (int i = 0; i < moleculars.Count; i++)
            {
                Unit molecular = moleculars[i];
                for (int k = 0; k < denominators.Count; k++)
                {
                    Unit denominator = denominators[k];

                    Formula modifyCurrent = molecular.GetModifyTo(denominator);
                    if (modifyCurrent == null) continue;

                    modify *= modifyCurrent;
                    moleculars.RemoveAt(i--);
                    denominators.RemoveAt(k);
                    break;
                }
            }
            return modify.Simplify();
        }
        
        /// <summary>
        /// 指定単位リスト間の変換係数を取得します。
        /// </summary>
        /// <param name="from">変換元の単位リスト。</param>
        /// <param name="to">変換先の単位リスト。</param>
        /// <returns>変換係数。</returns>
        private Formula GetModifyBetweenUnitList(ref List<Unit> from, ref List<Unit> to)
        {
            Formula modify = 1;
            for (int i = 0; i < from.Count; i++)
            {
                Unit fromUnit = from[i];
                for (int k = 0; k < to.Count; k++)
                {
                    Unit targetUnit = to[k];

                    Formula modifyCurrent = fromUnit.GetModifyTo(targetUnit);
                    if (modifyCurrent == null) continue;

                    modify *= modifyCurrent;
                    from.RemoveAt(i--);
                    to.RemoveAt(k);
                    break;
                }
            }
            return modify.Simplify();
        }

        /// <summary>
        /// 変換元と変換先の単位間で、単位変換を試みます。
        /// </summary>
        /// <param name="modifyFrom">変換による変換元の補正係数。</param>
        /// <param name="modifyTo">変換による変換先の補正係数。</param>
        /// <returns>単位変換が成功したか</returns>
        private bool TryConvertUnit(ref Formula modifyFrom, ref Formula modifyTo)
        {
            if (TryConvertUnit(ref modifyFrom, ref modifyTo, false)) return true;
            return TryConvertUnit(ref modifyFrom, ref modifyTo, true);
        }

        /// <summary>
        /// 変換元と変換先の単位間で、単位変換を試みます。
        /// </summary>
        /// <param name="modifyFrom">変換による変換元の補正係数。</param>
        /// <param name="modifyTo">変換による変換先の補正係数。</param>
        /// <param name="alsoNoCancel">約分がされない場合を含めて変換を実施するか否か。</param>
        /// <returns>変換に成功したか否か。</returns>
        private bool TryConvertUnit(ref Formula modifyFrom, ref Formula modifyTo, bool alsoNoCancel)
        {
            bool baseIsFrom = (_fromDenominator.Count + _fromMolecular.Count < _toDenominator.Count + _toMolecular.Count);

            if (TryConvertUnit(baseIsFrom, ref modifyFrom, ref modifyTo, alsoNoCancel)) return true;
            return TryConvertUnit(!baseIsFrom, ref modifyFrom, ref modifyTo, alsoNoCancel);
        }

        /// <summary>
        /// 変換元と変換先の単位間で、単位変換を試みます。
        /// </summary>
        /// <param name="baseIsFrom">変換基準を変換元とするか否か。</param>
        /// <param name="modifyFrom">変換による変換元の補正係数。</param>
        /// <param name="modifyTo">変換による変換先の補正係数。</param>
        /// <param name="alsoNoCancel">約分がされない場合を含めて変換を実施するか否か。</param>
        /// <returns>変換に成功したか否か。</returns>
        private bool TryConvertUnit(bool baseIsFrom, ref Formula modifyFrom, ref Formula modifyTo, bool alsoNoCancel)
        {
            List<Unit>[] convertBases = new List<Unit>[2];
            List<Unit>[] convertTo = new List<Unit>[2];
            convertBases[0] =  baseIsFrom ? _fromMolecular   : _toMolecular;
            convertBases[1] =  baseIsFrom ? _fromDenominator : _toDenominator;
            convertTo[0]    = !baseIsFrom ? _fromMolecular   : _toMolecular;
            convertTo[1]    = !baseIsFrom ? _fromDenominator : _toDenominator;
            for (int i = 0; i < 2; i++) // 分子、分母の順
            {
                for (int j = 0; j < convertBases[i].Count; j++)
                {
                    UnitConvertTable table = convertBases[i][j].BelongTable;
                    if (table == null) continue;

                    foreach (UnitConvertRecord record in table.GetAllRecords(true))
                    {
                        if (record.ConvertUnit is Unit) continue;

                        List<Unit> convertMolecular   = new List<Unit>();
                        List<Unit> convertDenominator = new List<Unit>();
                        BuildUnitList(record.ConvertUnit, ref convertMolecular, ref convertDenominator);

                        List<Unit>[] convertUnit = new List<Unit>[2];
                        convertUnit[0] = convertMolecular;
                        convertUnit[1] = convertDenominator;
                        if (!alsoNoCancel && !HasPossibleCanceling(i == 0, convertBases, convertTo, convertUnit)) continue;

                        Formula modify = table.GetConversionRatio(convertBases[i][j], record.ConvertUnit);
                        if ((baseIsFrom && i == 1) || (!baseIsFrom && i == 0)) modifyFrom *= modify;
                        else modifyTo *= modify;

                        convertBases[i].RemoveAt(j);
                        convertBases[i].AddRange(convertMolecular);
                        convertBases[1 - i].AddRange(convertDenominator);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 変換元の構成単位リストと変換を試みる単位の単位構成リストを指定して、約分により単位を減らすことができるか否かを判定して取得します。
        /// </summary>
        /// <param name="isMolecular">対象の単位が分子側の場合、true。分母側の場合false。</param>
        /// <param name="convertBases">変換元の構成単位リスト。インデックス0が分子側、インデックス1が分母側を表す。</param>
        /// <param name="convertTo">変換先の構成単位リスト。インデックス0が分子側、インデックス1が分母側を表す。</param>
        /// <param name="convertUnit">変換を試みる単位の構成単位リスト。インデックス0が分子側、インデックス1が分母側を表す。</param>
        /// <returns></returns>
        private bool HasPossibleCanceling(bool isMolecular, List<Unit>[] convertBases, List<Unit>[] convertTo, List<Unit>[] convertUnit)
        {
            int i = isMolecular ? 0 : 1;

            if (ExistMatchTypeUnit(convertUnit[0], convertTo[i]) ||
                ExistMatchTypeUnit(convertUnit[1], convertTo[1 - i]) ||
                ExistMatchTypeUnit(convertUnit[0], convertBases[1 - i]) ||
                ExistMatchTypeUnit(convertUnit[1], convertBases[i])) return true;
            return false;
        }

        /// <summary>
        /// 指定リスト間で一つでも同次元の単位が存在するか否かを返します。
        /// </summary>
        /// <param name="units1">探索単位リスト1。</param>
        /// <param name="units2">探索単位リスト2。</param>
        /// <returns>同次元の単位が存在するか否か。</returns>
        private bool ExistMatchTypeUnit(List<Unit> units1, List<Unit> units2)
        {
            foreach (Unit unit1 in units1)
            {
                foreach (Unit unit2 in units2)
                {
                    if (unit1.IsEqualTypeTo(unit2)) return true;
                }
            }
            return false;
        }


    }

}
