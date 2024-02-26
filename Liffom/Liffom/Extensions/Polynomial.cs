// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Reals;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Liffom.Extensions
{
    /// <summary>
    /// 多項式に関する処理を提供します。
    /// </summary>
    public static class Polynomial
    {
        /// <summary>
        /// 数式が0と判定されるべき数式（0[kg/cm2]など）か否かを取得します。
        /// </summary>
        /// <param name="f">判定対象の数式。</param>
        /// <returns>0と判定されるべきか。</returns>
        public static bool IsZero(this Formula f)
        {
            if (f == 0) return true;

            if (f == null) return false;

            bool containZero = false;
            foreach (var consist in f)
            {
                if (consist.IsUnit()) continue;
                else if (consist == 0) containZero = true;
                else return false;
            }
            return containZero;
        }

        /// <summary>
        /// 数式が負数と判定されるべき数式（-aなど）か否かを取得します。
        /// </summary>
        /// <param name="f">判定対象の数式。</param>
        /// <returns>負数と判定されるべきか。</returns>
        public static bool IsNegative(this Formula f)
        {
             if (f is Numeric && f < 0) return true;
             else if (f is Product)
             {
                 foreach (Formula inf in f)
                 {
                    if (inf.IsNegative()) return true;
                 }
             }
             return false;
        }

        /// <summary>
        /// 整数多項式か否かを取得します。
        /// </summary>
        /// <param name="f">判定対象の数式。</param>
        /// <returns>整数多項式か否か。</returns>
        public static bool IsPolynomial(this Formula f)
        {
            Numeric degree = null;

            AtomicFormula x = f.RepresentativeVariable();
            if (x == null) 
                degree = f as Numeric;
            else
                degree = f.Degree(x) as Numeric;

            if (degree == null || !degree.IsInteger) return false;

            return true;
        }

        /// <summary>
        /// 数式中で使用されていない適当な変数を取得します。
        /// </summary>
        /// <param name="f">探索対象の数式。</param>
        /// <returns>数式中で未使用の変数。</returns>
        public static Variable UnusedVariable(this Formula f)
        {
            return UnusedVariable("u", f);
        }

        /// <summary>
        /// 指定した数式中で使用されていない適当な変数を取得します。
        /// </summary>
        /// <param name="fs">探索対象の可変数の数式。</param>
        /// <returns>数式中で未使用の変数。</returns>
        public static Variable UnusedVariable(string baseName, params Formula[] fs)
        {
            Variable candidate = new Variable(baseName);

            int i = 0;
            foreach (var f in fs)
                while (f.Contains(candidate))
                    candidate = new Variable(baseName + (i++).ToString());

            return candidate;
        }

        /// <summary>
        /// 多項式の頭項係数を取得します。
        /// </summary>
        /// <param name="f">対象の多項式。</param>
        /// <param name="x">対象とする主変数。</param>
        /// <returns>頭項係数。</returns>
        /// <exception cref="Liffom.FormulaProcessException">主変数の指数が数値以外である項がある場合に投げられます。</exception>
        /// <example>
        /// <code>
        /// var lc = Formula.Parse("4x^3+(3+a)x^2+5").LeadingCoefficient(x); // lc = 4
        /// </code>
        /// </example>
        public static Formula LeadingCoefficient(this Formula f, AtomicFormula x)
        {
            Numeric maxExponent = f.Degree(x) as Numeric;
            if (maxExponent == null || !maxExponent.IsInteger) throw new FormulaProcessException(string.Format("{0}は整数多項式ではありません。", f));

            return f.CoefficientOf(x, maxExponent);
        }

        /// <summary>
        /// 多項式中における、指定次数の係数を取得します。
        /// </summary>
        /// <param name="f">対象の多項式。</param>
        /// <param name="x">対象とする主変数。</param>
        /// <param name="exponent">対象とする次数</param>
        /// <returns>指定次数の係数。</returns>
        public static Formula CoefficientOf(this Formula f, AtomicFormula x, Formula exponent)
        {
            var f_ = f.CollectAbout(x);

            Formula lc = 0;
            Formula le = 0;
            if (f_ is Sum)
            {
                foreach (var term in f_)
                {
                    Formula c = GetTermCoefficientOf(term, x, out le);
                    if (le != exponent) continue;

                    lc += c;
                }
                lc = lc.Combine();
            }
            else
            {
                Formula c = GetTermCoefficientOf(f_, x, out le);
                if (le == exponent) lc = c;
            }
            return lc;
        }

        /// <summary>
        /// 多項式の次数を取得します。
        /// </summary>
        /// <param name="f">対象の多項式。</param>
        /// <param name="x">対象とする主変数。</param>
        /// <returns>多項式の次数(deg f)。ただし、主変数の指数が数値以外である項が存在する場合、当該指数。</returns>
        /// <example>
        /// <code>
        /// var degF = Formula.Parse("4x^3+(3+a)x^2+5").Degree(x); // degF = 3
        /// </code>
        /// </example>
        public static Formula Degree(this Formula f, AtomicFormula x)
        {
            var f_ = f.CollectAbout(x);

            Numeric maxExponent = null;
            Formula exponent = 0;
            if (f_ is Sum)
            {
                foreach (var term in f_)
                {
                    Formula c = GetTermCoefficientOf(term, x, out exponent);

                    if (c.IsZero()) continue;
                    if (!(exponent is Numeric)) return exponent;
                    if (maxExponent == null || exponent > maxExponent) maxExponent = exponent as Numeric;
                }
            }
            else
            {
                Formula c = GetTermCoefficientOf(f_, x, out exponent);

                if (!(exponent is Numeric)) return exponent;
                maxExponent = exponent as Numeric;
            }
            return maxExponent ?? 0;
        }

        /// <summary>
        /// 指定項における、<see cref="x"/>の係数と指数を取得します。
        /// </summary>
        /// <param name="term">判定対象とする項。</param>
        /// <param name="x">対象とする主変数。</param>
        /// <param name="exponent">主変数の指数。</param>
        /// <returns><see cref="x"/>の指数。</returns>
        private static Formula GetTermCoefficientOf(Formula term, AtomicFormula x, out Formula exponent)
        {
            var a = new RulePatternVariable("a");
            a.AdmitMultiplyOne = true;
            a.CheckTarget = f => (!f.Contains(x));
            var n = new RulePatternVariable("n");
            n.AdmitPowerOne = true;
            Formula rule = a * (x ^ n);

            Formula c = null;
            if (term.PatternMatch(rule))
            {
                c = a.MatchedFormula;
                exponent = n.MatchedFormula;
            }
            else 
            {
                c = term;
                exponent = 0;
            }
            return c;
        }

        /// <summary>
        /// 必要に応じて、多項式を指定変数について整理します。
        /// </summary>
        /// <param name="f">対象数式。</param>
        /// <param name="x">対象とする主変数。</param>
        /// <returns><paramref name="x"/>について整理した数式。</returns>
        public static Formula CollectAbout(this Formula f, AtomicFormula x)
        {
            var collectToken = f.LastDeformToken as CollectToken;
            if (collectToken == null || collectToken.About != x)
            {
                collectToken = new CollectToken(x);
                return f.Copy().DeformFormula(collectToken);
            }

            return f;
        }


        /// <summary>
        /// 整数係数多項式の除算を行い、その商を取得します。
        /// </summary>
        /// <param name="f">被除数。</param>
        /// <param name="g">除数。</param>
        /// <param name="r">剰余。</param>
        public static Formula Divide(this Formula f, Formula g, out Formula r)
        {
            return f.Divide(g, out r, false);
        }

        /// <summary>
        /// 整数係数多項式の除算を行い、その商を取得します。
        /// </summary>
        /// <param name="f">被除数。</param>
        /// <param name="g">除数。</param>
        /// <param name="x">対象とする主変数。</param>
        /// <param name="r">剰余。</param>
        public static Formula Divide(this Formula f, Formula g, AtomicFormula x, out Formula r)
        {
            return Divide(f, g, x, out r, false);
        }


        /// <summary>
        /// 整数係数多項式の除算を行い、その商を取得します。
        /// </summary>
        /// <param name="f">被除数。</param>
        /// <param name="g">除数。</param>
        /// <param name="r">剰余。</param>
        /// <param name="admitFraction">商に分数を許容するか。</param>
        /// <returns>商。</returns>
        public static Formula Divide(this Formula f, Formula g, out Formula r, bool admitFraction)
        {
            AtomicFormula x = RepresentativeVariable(f); // 被除数において、次数が0とならない代表変数
            AtomicFormula y = RepresentativeVariable(g); // 除数において、次数が0とならない代表変数

            if (x == null)
            {
                if (admitFraction)
                {
                    r = 0;
                    return (f / g).Simplify();
                }
                else
                {
                    return DivideWithoutFraction(f, g, out r);
                }
            }
            else
            {
                return f.Divide(g, x, out r, admitFraction);
            }
        }

        /// <summary>
        /// 整数係数多項式の除算を行い、その商を取得します。
        /// </summary>
        /// <param name="f">被除数。</param>
        /// <param name="g">除数。</param>
        /// <param name="x">対象とする主変数。</param>
        /// <param name="r">剰余。</param>
        /// <param name="admitFraction">商に分数を許容するか。</param>
        /// <exception cref="Liffom.FormulaProcessException">被除数、もしくは除数が整数多項式でない場合にスローされます。</exception>
        public static Formula Divide(this Formula f, Formula g, AtomicFormula x, out Formula r, bool admitFraction)
        {
            if (g == 1)
            {
                r = 0;
                return f;
            }
            var f_ = f.CollectAbout(x);
            var g_ = g.CollectAbout(x);

            Numeric degF = f_.Degree(x) as Numeric;
            if (degF == null || !degF.IsInteger) throw new FormulaProcessException(string.Format("{0}は整数多項式ではありません。", f_));
            Numeric degG = g_.Degree(x) as Numeric;
            if (degG == null || !degG.IsInteger) throw new FormulaProcessException(string.Format("{0}は整数多項式ではありません。", g_));

            Formula q = 0;
            r = f_.Copy();
            while (r.Degree(x) >= g_.Degree(x))
            {
                Formula rt;
                Formula qt = r.LeadingCoefficient(x).Divide(g_.LeadingCoefficient(x), out rt, admitFraction);

                Formula t = qt * (x ^ (r.Degree(x) - g_.Degree(x)));
//                t = t.CollectAbout(x);
                t = t.Simplify();
                if (t.IsZero()) break;

                r = (r - t * g_).CollectAbout(x);
//                q = (q + t).CollectAbout(x);
                q = (q + t).Simplify();
            }
            return q;
        }

        /// <summary>
        /// 指定した2数式の両方において、次数が0とならない適当な代表変数を取得します。
        /// </summary>
        /// <param name="f1">探索対象の数式1。</param>
        /// <param name="f2">探索対象の数式2。</param>
        /// <returns>次数が0とならない代表変数。</returns>
        public static AtomicFormula RepresentativeVariable(Formula f1, Formula f2)
        {
            var ignoreList = new List<AtomicFormula>();

            var x = f1.RepresentativeVariable(ignoreList.ToArray());

            while (x != null)
            {
                if (f2.Degree(x) != 0) break;

                ignoreList.Add(x);
                x = f1.RepresentativeVariable(ignoreList.ToArray());
            }
            return x;
        }


        /// <summary>
        /// 指定数式において、次数が0とならない適当な代表変数を取得します。
        /// </summary>
        /// <param name="f">探索対象の数式。</param>
        /// <returns>次数が0とならない代表変数。</returns>
        public static AtomicFormula RepresentativeVariable(this Formula f, params AtomicFormula[] ignores)
        {
            var ignoreList = new List<AtomicFormula>(ignores);
            foreach (var x in f.GetExistFactors<Variable>())
            {
                if (ignoreList.Contains(x)) continue;
                if (f.Degree(x) != 0) return x;
            }

            foreach (var x in f.GetExistFactors<AtomicFormula>())
            {
                if (ignoreList.Contains(x)) continue;
                if (x is Variable) continue;
                if (f.Degree(x) != 0) return x;
            }
            return null;
        }

        /// <summary>
        /// 有効な次数を有する変数がない被除数に対して、指定除数で除した時の商を取得します。
        /// </summary>
        /// <param name="f">被除数。</param>
        /// <param name="g">除数。</param>
        /// <param name="r">剰余。</param>
        /// <returns>商。</returns>
        private static Formula DivideWithoutFraction(Formula f, Formula g, out Formula r)
        {
            var f_ = f.Simplify();
            var g_ = g.Simplify();

            if (f_ is Numeric && g_ is Numeric)
            {
                var nf = f_ as Numeric;
                var ng = g_ as Numeric;
                r = nf.Figure % ng.Figure;
                return ((f_ - r) / g_).Simplify();
            }
            else
            {
                var q = (f_ / g_).Simplify();

                var a = new RulePatternVariable("a");
                a.AdmitMultiplyOne = true;
                var b = new RulePatternVariable("b");
                var rule = a / b;

                if (q.PatternMatch(rule)) // 分母が残るなら割れなかったと判定
                {
                    r = f_;
                    return 0;
                }
                else
                {
                    r = 0;
                    return q;
                }
            }
        }


        /// <summary>
        /// 整数係数多項式の擬除算を行います。
        /// </summary>
        /// <param name="f">被除数。</param>
        /// <param name="g">除数。</param>
        /// <param name="x">対象とする主変数。</param>
        /// <param name="q">擬商。</param>
        /// <param name="r">擬剰余。</param>
        /// <returns>商に対する擬商の倍率。</returns>
        /// <exception cref="Liffom.FormulaProcessException">被除数、もしくは除数が整数多項式でない場合にスローされます。</exception>
        public static Formula PseudoDivide(this Formula f, Formula g, AtomicFormula x, out Formula q, out Formula r)
        {
            Numeric degF = f.Degree(x) as Numeric;
            if (degF == null || !degF.IsInteger) throw new FormulaProcessException(string.Format("{0}は整数多項式ではありません。", f));
            Numeric degG = g.Degree(x) as Numeric;
            if (degG == null || !degG.IsInteger) throw new FormulaProcessException(string.Format("{0}は整数多項式ではありません。", g));

            Formula ratio = (g.LeadingCoefficient(x) ^ (degF - degG + 1)).Simplify();
            f = (f * ratio).CollectAbout(x);
            q = f.Divide(g, x, out r, true);

            return ratio;
        }

        /// <summary>
        /// 数式の原始多項式を取得します。
        /// </summary>
        /// <param name="f">対象の数式。</param>
        /// <param name="x">主変数。</param>
        /// <returns>原始多項式。</returns>
        public static Formula PrimitivePolynomial(this Formula f, AtomicFormula x)
        {
            Formula cont;
            return f.PrimitivePolynomial(x, out cont);
        }

        /// <summary>
        /// 数式の原始多項式を取得します。
        /// </summary>
        /// <param name="f">対象の数式。</param>
        /// <param name="x">主変数。</param>
        /// <param name="cont">容量。</param>
        /// <returns>原始多項式。</returns>
        public static Formula PrimitivePolynomial(this Formula f, AtomicFormula x, out Formula cont)
        {
            cont = 1;
            var f_ = f.CollectAbout(x);

            // 指数 - 係数マップ
            Dictionary<Formula, Formula> coefMap = new Dictionary<Formula, Formula>();
            if (!(f_ is Sum))
            {
                Formula exp;
                Formula coef = GetTermCoefficientOf(f_, x, out exp);
                if (coef != 0) coefMap.Add(exp, coef);
            }
            else
            {
                foreach (var term in f_)
                {
                    Formula exp;
                    Formula coef = GetTermCoefficientOf(term, x, out exp);
                    if (coef.IsZero()) continue;

                    if (coefMap.ContainsKey(exp)) coefMap[exp] += coef;
                    else coefMap.Add(exp, coef);
                }
            }

            // 係数1があるならすでに原始多項式
            if (coefMap.Values.Contains(1)) return f_;

            Formula gcd = null;
            foreach (var pair in coefMap)
            {
                if (gcd == null) gcd = pair.Value;
                else gcd = GCD(gcd, pair.Value);
            }
            if (gcd == null) return f_; // 0など。

            Formula rem, pp;
            if (gcd is Numeric)
            {
                pp = (f_ / gcd).CollectAbout(x);
            }
            else
            {
                pp = f_.Divide(gcd, x, out rem, false);
#if DEBUG
                FormulaAssertionException.Assert(rem.LeadingCoefficient(x).IsZero());
#endif
            }
            cont = gcd;
            return pp.CollectAbout(x);
        }

        /// <summary>
        /// 指定した2数式間の最大公約数を取得します。
        /// </summary>
        /// <param name="f1">対象の数式1。</param>
        /// <param name="f2">対象の数式2。</param>
        /// <returns>2数式の最大公約数。数値間の最大公約数が無理数と判定された場合には、nullとなります。</returns>
        /// <exception cref="Liffom.FormulaProcessException">被除数、もしくは除数が整数多項式でない場合にスローされます。</exception>
        public static Formula GCD(Formula f1, Formula f2)
        {
            if (f1.IsZero()) return f2;
            if (f2.IsZero()) return f1;
            if (f1 is Numeric && f2 is Numeric) return NumericGCD(f1 as Numeric, f2 as Numeric);
            if (f1.IsNumericOnly() && f2.IsNumericOnly()) return NumericGCD(f1.Numerate() as Numeric, f2.Numerate() as Numeric);

            var x = RepresentativeVariable(f1, f2);
            if (x == null) x = f1.RepresentativeVariable();
            if (x == null) x = f2.RepresentativeVariable();
            if (x == null) return 1;

            return GCD(f1, f2, x);
        }

        /// <summary>
        /// 指定した2数式間の最大公約数を取得します。
        /// </summary>
        /// <param name="f1">対象の数式1。</param>
        /// <param name="f2">対象の数式2。</param>
        /// <param name="x">主変数。</param>
        /// <returns>2数式の最大公約数。数値間の最大公約数が無理数と判定された場合には、nullとなります。</returns>
        /// <exception cref="Liffom.FormulaProcessException">被除数、もしくは除数が整数多項式でない場合にスローされます。</exception>
        public static Formula GCD(Formula f1, Formula f2, AtomicFormula x)
        {
            if (f1.IsZero()) return f2;
            if (f2.IsZero()) return f1;
            if (f1 is Numeric && f2 is Numeric) return NumericGCD(f1 as Numeric, f2 as Numeric);
            if (f1.IsNumericOnly() && f2.IsNumericOnly()) return NumericGCD(f1.Numerate() as Numeric, f2.Numerate() as Numeric);

            var deg1 = f1.Degree(x) as Numeric;
            if (deg1 == null || !deg1.IsInteger) throw new FormulaProcessException(string.Format("{0}は整数多項式ではありません。", f1));
            var deg2 = f2.Degree(x) as Numeric;
            if (deg2 == null || !deg2.IsInteger) throw new FormulaProcessException(string.Format("{0}は整数多項式ではありません。", f2));

            int i = 0;
            var f = new List<Formula>();
            Formula cont1, cont2;
            Formula f1pp = f1.PrimitivePolynomial(x, out cont1);
            Formula f2pp = f2.PrimitivePolynomial(x, out cont2);
            Formula gcd = GCD(cont1, cont2);
            if (gcd == null) return null;

            f.Add((deg1 >= deg2) ? f1pp : f2pp);
            f.Add((deg1 >= deg2) ? f2pp : f1pp);

            for (int k = (int)((Value.Max(deg1, deg2)) + 0.1); k >= 0; k--)
            {
                Formula pquo, prem;
                Formula ratio = f[i].PseudoDivide(f[i + 1], x, out pquo, out prem);
                if (prem.IsZero())
                {
                    if (f[i + 1].Degree(x).IsZero()) return gcd;

                    if (f[i + 1].LeadingCoefficient(x).IsNegative()) gcd *= -1;
                    return (f[i + 1] * gcd).Simplify();
                }
                Formula cont;
                f.Add(prem.PrimitivePolynomial(x, out cont));
                i++;
            }
            return gcd;
        }

        /// <summary>
        /// 指定した2数値間の最大公約数を取得します。
        /// </summary>
        /// <param name="n1">対象の数値1。</param>
        /// <param name="n2">対象の数値2。</param>
        /// <returns>2数式の最大公約数。数値間の最大公約数が無理数と判定された場合には、nullとなります。</returns>
        /// <exception cref="Liffom.FormulaProcessException">被除数、もしくは除数が整数多項式でない場合にスローされます。</exception>
        internal static Formula NumericGCD(Numeric n1, Numeric n2)
        {
            if (n1 == 0) return n2;
            if (n2 == 0) return n1;
            if (n1.Figure.IsInfinity) return null;
            if (n2.Figure.IsInfinity) return null;

            Numeric errorRatio = new Numeric(Math.Pow(10, -Numeric.MaxValidDigits) * 5d);

            Numeric nBase = (n1 > n2) ? n1 : n2;
        
            if (n1 < 0) n1 = (-1 * n1).Numerate() as Numeric;
            if (n2 < 0) n2 = (-1 * n2).Numerate() as Numeric;

            int count = 0;            
            Numeric error = new Numeric(0);
            while (Value.Abs(n1) > error && Value.Abs(n2) > error)
            {
                if (n1 > n2)
                {
                    Real baseReal = n1.Figure;
                    n1 = new Numeric(n1.Figure % n2.Figure);
                    error = (n2 * errorRatio).Numerate() as Numeric;

                    if (n1.Figure == baseReal) break;
                }
                else
                {
                    Real baseReal = n2.Figure;
                    n2 = new Numeric(n2.Figure % n1.Figure);
                    error = (n1 * errorRatio).Numerate() as Numeric;

                    if (n2.Figure == baseReal) break;
                }

                if (count++ > 1000)
                {
                    FormulaAssertionException.Assert(false);
                    return null;
                }
                if (n1 == n2) break;
            }

            Numeric gcd = (n1 > n2) ? n1 : n2;
            if (gcd < (nBase * (2d * errorRatio)).Numerate()) return null; // 無理数判定
            return gcd;
        }

        /// <summary>
        /// 指定した2数値間の最小公倍数を取得します。
        /// </summary>
        /// <param name="n1">対象の数値1。</param>
        /// <param name="n2">対象の数値2。</param>
        /// <returns>2数式の最小公倍数。</returns>
        public static Formula LCM(Formula f1, Formula f2)
        {
            Formula gcd = GCD(f1, f2);
            if (gcd == null) return null;

            Formula rem;
            return (f1 * f2.Divide(gcd, out rem)).Simplify();
        }

    }
}
