// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Operators.Comparers;
using GoodSeat.Liffom.Reals;

namespace GoodSeat.Liffom.Processes
{
    /// <summary>
    /// ニュートン・ラフソン法による解の算出処理を表します。
    /// </summary>
    public class NewtonMethod : SolveEquation
    {
        /// <summary>
        /// 一意のユーザー情報を指定して、複数の同時呼び出しを許可するか否かを取得します。
        /// </summary>
        public override bool IsSupportMultipleConcurrentInvocations { get { return true; } }

        double _initialSolution = 0d;   // 初期解
        double _errorTolerance = 1E-6;  // 許容誤差
        int _maxTryCount = 500;         // 最大試行回数
        bool _autoInitialSolutinoShift = true; // 与えられた初期解で求解できない場合に、自動で初期解をずらして試行を続行するか

        double _increment = 0.5;        // 再試行時の初期解のずらし基準量
        double _incrementWidth = 0.25;  // 再試行時の初期解ずらし基準の振れ幅

        /// <summary>
        /// 初期解を設定もしくは取得します。
        /// </summary>
        public double InitialSolution
        {
            get { return _initialSolution; }
            set { _initialSolution = value; }
        }

         /// <summary>
         /// 与えられた初期解で求解できない場合に、自動で初期解をずらして試行を続行するか否かを設定若しくは取得します。
         /// </summary>
        public bool AutoInitialSolutionShift
        {
            get { return _autoInitialSolutinoShift; }
            set { _autoInitialSolutinoShift = value; }
        }
        
        /// <summary>
        /// 解の許容誤差を設定もしくは取得します。
        /// </summary>
        public double ErrorTolerance
        {
            get { return _errorTolerance; }
            set { _errorTolerance = value; }
        }

        /// <summary>
        /// 最大試行回数を設定もしくは取得します。
        /// </summary>
        public int MaxTryCount
        {
            get { return _maxTryCount; }
            set { _maxTryCount = value; }
        }

        /// <summary>
        /// 初期解のずらし基準量を設定もしくは取得します。
        /// </summary>
        public double Increment
        {
            get { return _increment; }
            set { _increment = value; }
        }

        /// <summary>
        /// 初期解のずらし基準量からの乱数最大振れ幅を設定もしくは取得します。
        /// </summary>
        public double IncrementWidth
        {
            get { return _incrementWidth; }
            set { _incrementWidth = value; }
        }


        /// <summary>
        /// 指定された数式に対して、処理を実行します。
        /// </summary>
        /// <param name="target">求解対象の等式。</param>
        /// <param name="about">求解対象の変数。</param>
        /// <param name="userState">一意のユーザー状態。</param>
        public override Equal Solve(object userState, Equal target, Variable about)
        {
            return GetSolution(target, about, new Numeric(InitialSolution), userState);
        }

        /// <summary>
        /// ニュートン・ラフソン法により解を求めます。
        /// </summary>
        /// <param name="target">対象方程式。</param>
        /// <param name="x">求める変数。</param>
        /// <param name="initial">解の初期値。</param>
        /// <param name="userState">一意のユーザー状態。</param>
        /// <returns>求められた解。ただし、最大試行回数まで解が見つからなかった場合、null。</returns>
        private Equal GetSolution(Equal target, Variable x, Numeric initial, object userState)
        {
            if (!target.Contains(x))
                throw new FormulaProcessException(string.Format("数式「{0}」に、求解対象の変数{1}が存在しません。", target, x));
#if DEBUG
            DateTime start = DateTime.Now;
            Console.WriteLine("ニュートン法による解の算出開始：" + start.ToLongTimeString());
#endif
            Formula f = (target[0] - target[1]).Numerate().Combine(); // 対象の方程式 f(x) = 0 を解く。
            if (f.Contains(v=>(v is Variable && v != x)))
                throw new FormulaProcessException(string.Format("数式「{0}」に{1}以外の変数が存在するため、ニュートン法で解くことができません。", target, x));

            Numeric solution = GetSolution(f, x, initial, userState);
#if DEBUG
            DateTime end = DateTime.Now;
            Console.WriteLine("ニュートン法による解の算出終了：計算時間：" + (end - start).ToString());
#endif
            if (IsCanceled(userState)) return null;
            return new Equal(x, GetModifiedSolution(f, x, solution, ErrorTolerance));
        }

        /// <summary>
        /// ニュートン・ラフソン法による解の算出処理を実行します。
        /// </summary>
        /// <param name="f">f(x)=0における、f(x)。</param>
        /// <param name="x">f(x)=0における、x。</param>
        /// <param name="initial">解の初期値。</param>
        /// <param name="userState">一意のユーザー状態。</param>
        /// <returns>求められた解。</returns>
        private Numeric GetSolution(Formula f, Variable x, Numeric initial, object userState)
        {
            var token = new DeformToken(Formula.SimplifyToken, Formula.CalculateToken, Formula.NumerateToken);

            Real solution = initial.Figure;
            Formula fd = new Differentiate(f, x); // f'

            fd = fd.DeformFormula(token);

            // 微分結果の確認
            if (fd.Contains<Differentiate>()) throw new FormulaProcessException(f.ToString() + "を、微分できませんでした。");

            // 初期解の代入
            Random random = new Random(0);
            int tryCount = 0;
            do
            {
                try
                {
                    Numeric initialTest = fd.Substituted(x, solution).DeformFormula(token) as Numeric;
                    if (initialTest == null) throw new FormulaProcessException(f.ToString() + "を、微分できませんでした。");
                    if (!fd.Contains(x) && initialTest == 0) throw new FormulaProcessException(f.ToString() + "は解を持ちません。");
                }
                catch (Exception e) // 0除算などが発生したら、初期解をずらして再試行
                {
                    if (!AutoInitialSolutionShift || ++tryCount > 5) throw e;
                    solution = GetRandomShift(solution, random);
                    continue;
                }
            } while (false);

            // 導関数が0になるなら初期値の設定をやり直す。
            while (fd.Substituted(x, solution).DeformFormula(token) == 0)
            {
                if (!AutoInitialSolutionShift) throw new FormulaProcessException(f.ToString() + "の導関数に初期解\"" + solution.ToString() + "\"を代入した結果が0になります。");
                solution = GetRandomShift(solution, random);
            }

            // 無限ループ検知用の途中解リスト
            List<double> solList = new List<double>();

            Real lastSolution = null;
            Real fdSubstituted = null;
            tryCount = 0;
            do
            {
                if (IsCanceled(userState)) return null;
                if (tryCount++ > MaxTryCount) throw new FormulaProcessException(MaxTryCount + "回の試行回数内では、指定誤差値以内に解が収束しませんでした。");
                Formula.CheckCancelOperation(f);

                lastSolution = solution;

                Real fSubstituted = (f.Substituted(x, solution).DeformFormula(token) as Numeric).Figure;
                if (fdSubstituted == null) fdSubstituted = (fd.Substituted(x, solution).DeformFormula(token) as Numeric).Figure;
                solution = solution - fSubstituted / fdSubstituted;

                // 非数値や無限大ならリセット
                if (solution == null || solution.IsNaN || solution.IsInfinity) 
                    solution = GetRandomShift(0d, random);

                // 無限ループを検知、もしくは導関数が0となったら、解を適当にずらす
                fdSubstituted = (fd.Substituted(x, solution).DeformFormula(token) as Numeric).Figure;
                while (solList.Contains(solution) ||  fdSubstituted == 0) 
                {
                    solution = GetRandomShift(solution, random);
                    fdSubstituted = (fd.Substituted(x, solution).DeformFormula(token) as Numeric).Figure;
                }

                solList.Add(solution);
#if DEBUG
                Console.WriteLine(string.Format("{0}回目の試行:x={1}:{2}+{3}", tryCount, solution, DateTime.Now.Second, DateTime.Now.Millisecond / 1000d));
#endif
            }
            while (Math.Abs(lastSolution - solution) >= ErrorTolerance);

            return solution;
        }


        /// <summary>
        /// 指定数値を適当にシフトした数値を取得します。
        /// </summary>
        /// <param name="baseNumeric">規準数値。</param>
        /// <param name="random">乱数生成器。</param>
        /// <returns>ずらした数値。</returns>
        private Numeric GetRandomShift(Numeric baseNumeric, Random random)
        {
            return new Numeric(baseNumeric.Figure + Increment + (random.NextDouble() - 0.5) * IncrementWidth * 2);
        }

    }
}
