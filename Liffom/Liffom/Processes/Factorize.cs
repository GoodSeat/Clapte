// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Extensions;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Processes
{
    /// <summary>
    /// 数式の因数分解処理を表します。
    /// </summary>
    public class Factorize : Process
    {
        /// <summary>
        /// 数式の因数分解処理を初期化します。
        /// </summary>
        public Factorize() : this(true) { }

        /// <summary>
        /// 数式の因数分解処理を初期化します。
        /// </summary>
        /// <param name="decomposeSquareFree">無平方分解処理を行うか。</param>
        public Factorize(bool decomposeSquareFree) 
            : base() 
        {
            DecomposeSquareFree = decomposeSquareFree;
        }


        /// <summary>
        /// 一意のユーザー情報を指定して、複数の同時呼び出しを許可するか否かを取得します。
        /// </summary>
        public override bool IsSupportMultipleConcurrentInvocations { get { return false; } }

        /// <summary>
        /// 処理対象とする数式数の下限値を取得します。
        /// </summary>
        public override int TargetArgumentsMinQty { get { return 1; } }


        /// <summary>
        /// 因数分解の際に、無平方分解を行うか否かを設定もしくは取得します。
        /// </summary>
        public bool DecomposeSquareFree { get; set; }

        /// <summary>
        /// 因数分解対象の数式を設定もしくは取得します。
        /// </summary>
        private Formula f { get; set; }

        /// <summary>
        /// 因数分解処理における主変数を設定もしくは取得します。
        /// </summary>
        private AtomicFormula x { get; set; }

        /// <summary>
        /// 因数分解対象の数式の主変数に対する次数を設定もしくは取得します。
        /// </summary>
        private Numeric n { get; set; }

        /// <summary>
        /// <see cref="n"/>/2以下の最大の整数を設定もしくは取得します。
        /// </summary>
        private int s { get; set; }

        /// <summary>
        /// 適当な整数snのリストを設定もしくは取得します。
        /// </summary>
        private List<int> snList { get; set; }

        /// <summary>
        /// 適当な整数snと、f(sn)の全約数リストを設定もしくは取得します。
        /// </summary>
        private Dictionary<int, List<Formula>> DivisorList { get; set; }

        /// <summary>
        /// f(x)の因数となりうるg(x)を構成するラグランジュ補間式を設定もしくは取得します。
        /// </summary>
        private Formula Interpolation { get; set; }

        /// <summary>
        /// <see cref="Interpolation"/>に用いたg(sn)に対応する変数名マップを設定もしくは取得します。
        /// </summary>
        private Dictionary<int, Variable> InterpolationVariables { get; set; }

        /// <summary>
        /// 指定された数式に対して、処理を実行します。
        /// </summary>
        /// <param name="targets">因数分解対象の和算。</param>
        protected override Formula OnDo(object userState, params Formula[] targets)
        {
            f = targets[0];
            if (!(f is Sum)) return f;

            x = f.RepresentativeVariable();
            if (x == null) return f;

            n = f.Degree(x) as Numeric; // f(x)はn次の多項式
            if (n == null || !n.IsInteger || n <= 0) return f;

            if (DecomposeSquareFree) // 無平方分解を行う
            {
                var fd = DecomposeSquareFreeTerms();
                if (fd != null) return fd;
            }

            s = ((int)n) / 2; // n/2 以下の最大の整数

            ConstituteSnList(); // s+1 個の相異なる整数 s_1, s_2 ... s_s+1
            
            var challenge = ConstituteDivisorsList(); // (*1)
            if (challenge != null) return challenge; // 因数定理による因数分解が成功した場合

            Interpolation = GetLagrangeInterpolation(); // ラグランジュ補間を構成 (*2)

            return ChallengeDivideFactors(); // 試し割をして結果を返す
        }

        /// <summary>
        /// <see cref="f"/>の無平方分解を試みます。無平方分解に成功した場合、内部的に因数分解を行い、因数分解結果を返します。
        /// </summary>
        /// <returns>無平方分解に成功した場合、因数分解の結果。それ以外の場合、null。</returns>
        private Formula DecomposeSquareFreeTerms()
        {
            var sqfr = new SquareFreeDecomposition();
            var fd = sqfr.Do(f);

            if (!(fd is Sum)) // 無平方分解があった
            {
                var factorizeProcesses = new Dictionary<Formula, Process>();
                FactorizeSquareFreeTerms(fd, factorizeProcesses);

                foreach (var process in factorizeProcesses)
                    fd = fd.Substitute(process.Key, process.Value.Wait());

                if (fd is OperatorMultiple) fd = (fd as OperatorMultiple).CreateIntegrated();
                return fd;
            }
            return null;
        }

        /// <summary>
        /// 無平方分解済みの数式の各項について、非同期に因数分解処理を開始します。
        /// </summary>
        /// <param name="fd">無平方分解であることが既知な数式。</param>
        /// <param name="factorizeProcesses">因数分解対象の数式 - 因数分解処理オブジェクトマップ。</param>
        private void FactorizeSquareFreeTerms(Formula fd, Dictionary<Formula, Process> factorizeProcesses)
        {
            if (factorizeProcesses.ContainsKey(fd)) return;

            if (fd is Power)
            {
                Power power = fd as Power;

                var factorizeProcess = new Factorize();
                factorizeProcesses.Add(power.Base.Copy(), factorizeProcess);
                factorizeProcess.DoAsync(power.Base);
            }
            else if (fd is Product)
            {
                foreach (var fc in fd) FactorizeSquareFreeTerms(fc, factorizeProcesses);
            }
            else if (fd is Sum)
            {
                var factorizeProcess = new Factorize();
                factorizeProcesses.Add(fd.Copy(), factorizeProcess);
                factorizeProcess.DoAsync(fd);
            }
        }

        /// <summary>
        /// <see cref="s"/> + 1 個の適当な整数で<see cref="snList"/>を初期化します。
        /// </summary>
        private void ConstituteSnList()
        {
            snList = new List<int>();
            int start = (s + 1) / 2;
            int count = s + 1;
            for (int i = start; i > start - count; i--) snList.Add(i);
        }

        /// <summary>
        /// 指定した整数リストの各整数snにて、<see cref="f"/>(sn)の全約数リストDivisorListを構成します。
        /// </summary>
        /// <param name="si"><see cref="f"/>の<see cref="x"/>に代入する整数リスト。</param>
        /// <returns>通常、null。因数定理による因数分解に成功した場合にのみ、因数分解の結果を返す。</returns>
        private Formula ConstituteDivisorsList()
        {
            var fsList = new Dictionary<int, Formula>();
            DivisorList = new Dictionary<int, List<Formula>>();

            foreach (int sn in snList)
            {
                Formula fs = f.Substituted(x, sn).Simplify();
                fsList.Add(sn, fs);

                if (fs == 0)
                {
                    var f1 = (x - sn).Simplify(); // 因数定理

                    Formula rem;
                    var next = f.Divide(f1, x, out rem, false);
                    FormulaAssertionException.Assert(rem == 0);

                    Factorize factorize = new Factorize(false);
                    var f2 = factorize.Do(next);
                    return new Product(true, f1, f2);
                }
            }

            bool containMinus = false;
            foreach (var pair in fsList)
            {
                var sn = pair.Key;
                var fs = pair.Value;
                    
                DivisorList.Add(sn, GetAllDivisors(fs, containMinus));
                containMinus = true; // 最初の1つ以外は負数を考慮する
            }
            return null;
        }

        /// <summary>
        /// 指定数式で考えられる全約数リストを取得します。
        /// </summary>
        /// <param name="target">約数を探索する対象の数式。</param>
        /// <param name="containMinus">約数に負数を含めるか。</param>
        /// <returns>全約数リスト。</returns>
        /// <example>a(x+1)^2 → 1, -1, a, -a, x+1, -(x+1), (x+1)^2, -(x+1)^2, a(x+1), -a(x+1), a(x+1)^2, -a(x+1)^2</example>
        private List<Formula> GetAllDivisors(Formula target, bool containMinus)
        {
            Factorize factorize = new Factorize(false);
            var factorized = factorize.Do(target);
            var allDecomposedFactors = new List<Formula>(GetDecomposedFactors(factorized));

            // 1を追加
            if (!allDecomposedFactors.Contains(1)) allDecomposedFactors.Add(1);
            // -1を必要に応じて追加、または削除
            if (!allDecomposedFactors.Contains(-1) && containMinus) allDecomposedFactors.Add(-1);
            while (allDecomposedFactors.Contains(-1) && !containMinus) allDecomposedFactors.Remove(-1);

            var allDivisors = new List<Formula>(allDecomposedFactors);

            for (int i = 2; i <= allDecomposedFactors.Count; i++)
                foreach (List<Formula> l in Utilities.Combination<Formula>.GetAllCombination(i, allDecomposedFactors))
                    allDivisors.Add(new Product(l.ToArray()).Combine());

            // 重複するものを削除
            for (int i = 0; i < allDivisors.Count; i++)
                for (int k = i + 1; k < allDivisors.Count; k++)
                    if (allDivisors[i] == allDivisors[k])
                    {
                        allDivisors.RemoveAt(k);
                        k--;
                    }

//            allDivisors.Sort((f1, f2) =>
//            {
//                if (!(f1 is Numeric) && !(f2 is Numeric)) return f1.CompareTo(f2);
//                if (f1 is Numeric && !(f2 is Numeric)) return -1;
//                if (f2 is Numeric && !(f1 is Numeric)) return 1;
//
//                var n1 = f1 as Numeric;
//                var n2 = f2 as Numeric;
//                if (Math.Abs(n1.Data) > Math.Abs(n2.Data)) return 1;
//                else if (Math.Abs(n1.Data) == Math.Abs(n2.Data)) return 0;
//                else return -1;
//            });
            return allDivisors;
        }

        /// <summary>
        /// 指定された因数分解済みの数式の約数リストを取得します。累乗については、指数分だけ約数としてリストに登録します。
        /// </summary>
        /// <param name="factorized">対象とする因数分解済みの数式。</param>
        /// <returns>因数リスト。</returns>
        /// <example>a(x+1)^2 → a, x+1, x+1</example>
        private IEnumerable<Formula> GetDecomposedFactors(Formula factorized)
        {
            if (factorized is Product)
            {
                foreach (var consist in factorized)
                {
                    foreach (var child in GetDecomposedFactors(consist)) yield return child;
                }
            }
            else if (factorized is Power)
            {
                int exponent = 1;

                Power power = factorized as Power;
                if (power.Exponent is Numeric && (power.Exponent as Numeric).IsInteger) exponent = (int)power.Exponent;

                for (int i = 0; i < exponent; i++)
                {
                    foreach (var child in GetDecomposedFactors(power.Base)) yield return child;
                }
            }
            else if (factorized is Numeric)
            {
                Numeric n = factorized as Numeric;
                Formula factor = PrimeFactor.PrimeFactorize(n, false);
                if (factor is Numeric) yield return factor;
                else foreach (var prime in factor) yield return prime;
            }
            else
                yield return factorized;
        }

        /// <summary>
        /// ラグランジュの補間公式を構成し取得します。
        /// </summary>
        /// <returns></returns>
        private Formula GetLagrangeInterpolation()
        {
            Formula gx = 0;
            InterpolationVariables = new Dictionary<int, Variable>();

            foreach (var sn in snList)
            {
                Variable gxn = Polynomial.UnusedVariable("gxn", gx, f);
                InterpolationVariables.Add(sn, gxn);

                Formula term = gxn;
                foreach (var so in snList)
                {
                    if (sn == so) continue;
                    term *= ((x - so) / (sn - so)).Simplify();
                    term = term.Simplify();
                }
                gx += term;
            }
            gx = gx.CollectAbout(x);

            return gx;
        }

        /// <summary>
        /// 補間式を利用して、試し割りによる因数分解を行います。
        /// </summary>
        private Formula ChallengeDivideFactors()
        {
            foreach (var candidateSet in GetValidDivisorSet())
            {
                var candidate = Interpolation.Copy();
                foreach (var pair in candidateSet)
                {
                    int sn = pair.Key;
                    var gxn = pair.Value;
                    Variable v = InterpolationVariables[sn];
                    candidate = candidate.Substitute(v, gxn);
                }
                if (candidate.LeadingCoefficient(x).IsNegative()) candidate *= -1;
                candidate = candidate.Simplify();

                if (candidate == 1 || candidate == -1) continue;
                if (candidate == f) continue;

                Formula rem;
                Formula quo = f.Divide(candidate, x, out rem, false);
                if (rem == 0)
                {
                    Factorize factor1 = new Factorize(false);
                    Factorize factor2 = new Factorize(false);
                    factor1.DoAsync(candidate);
                    factor2.DoAsync(quo);
                    var factorized1 = factor1.Wait();
                    var factorized2 = factor2.Wait();
                    return new Product(true, factorized1, factorized2);
                }
            }
            return f;
        }

        /// <summary>
        /// 補間式g(x)が整数多項式となりうる、snとg(sn)の組み合わせをすべて返す反復子を取得します。
        /// </summary>
        /// <returns>補間式g(x)が整数多項式となりうるsn - g(sn)マップ。</returns>
        private IEnumerable<Dictionary<int, Formula>> GetValidDivisorSet()
        {
            foreach (var valid in GetValidDivisorSet(0, null))
                yield return valid;
        }

        /// <summary>
        /// 補間式g(x)が整数多項式となりうる、snとg(sn)の組み合わせをすべて返す反復子を取得します。
        /// </summary>
        /// <param name="nextIndex">追加するsnListのインデックス。</param>
        /// <param name="midstream">nextIndex - 1までのリストマップ。</param>
        /// <returns>補間式g(x)が整数多項式となりうるsn - g(sn)マップ。</returns>
        private IEnumerable<Dictionary<int, Formula>> GetValidDivisorSet(int nextIndex, Dictionary<int, Formula> midstream)
        {
            if (snList.Count <= nextIndex)
            {
                yield return midstream;
                yield break;
            }

            var sn = snList[nextIndex];
            var gsnList = DivisorList[sn];
            if (midstream == null) midstream = new Dictionary<int, Formula>();

            foreach (var gsn in gsnList)
            {
                bool isValid = true;
                foreach (var pair in midstream)
                {
                    var sm = pair.Key;
                    var gsm = pair.Value;

                    Formula rem;
                    (gsn - gsm).Divide(sn - sm, x, out rem, false);
                    if (!rem.IsZero()) 
                    {
                        isValid = false;
                        break;
                    }
                }
                if (!isValid) continue;

                var nextMidStream = GetCopyOf(midstream);
                nextMidStream.Add(sn, gsn);

                foreach (var valid in GetValidDivisorSet(nextIndex + 1, nextMidStream))
                    yield return valid;
            }
        }

        /// <summary>
        /// 指定した整数-数式マップをコピーして取得します。
        /// </summary>
        private Dictionary<int, Formula> GetCopyOf(Dictionary<int, Formula> target)
        {
            Dictionary<int, Formula> copy = new Dictionary<int, Formula>();

            if (target != null)
                foreach (int key in target.Keys) copy.Add(key, target[key]);

            return copy;
        }


    }
}
