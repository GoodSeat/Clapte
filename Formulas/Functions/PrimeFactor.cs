using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Functions
{
	/// <summary>
	/// factor関数（素因数分解）を表します。
	/// </summary>
	[Serializable()]
	public class PrimeFactor : Function
	{
		/// <summary>
		/// 素因数分解関数を初期化します。
		/// </summary>
		public PrimeFactor() : base() { }

		/// <summary>
		/// 素因数分解関数を初期化します。
		/// </summary>
		/// <param name="f">対象の数式。</param>
		public PrimeFactor(Formula f) : base(f) { }

		/// <summary>
		/// 引数を指定して、関数を生成します。
		/// </summary>
		/// <param name="args">初期化に用いるか変数の数式。</param>
		/// <returns>初期化された関数。</returns>
		public override Function CreateFunction(params Formula[] args)
		{
			return new PrimeFactor(args[0]);
		}

		public override Formula CalculateFunction()
		{
			if (Argument[0] is Numeric && (Argument[0] as Numeric).IsInteger)
				return PrimeFactorize((int)(Argument[0] as Numeric).Data);
			else
				return this;
		}

		public override int MinimumArgumentQty
		{
			get { return 1; }
		}


		/// <summary>
		/// 素因数分解を実行します。
		/// </summary>
		/// <param name="d">対象の数値。</param>
		/// <param name="containPower">答えに累乗を含める場合true、全て積とする場合falseを指定。</param>
		/// <returns></returns>
		public static Formula PrimeFactorize(double d, bool containPower)
		{
			return PrimeFactorize(d, containPower, d);
		}

		/// <summary>
		/// 素因数分解を実行します。
		/// </summary>
		/// <param name="d">対象の数値。</param>
		/// <param name="containPower">答えに累乗を含める場合true、全て積とする場合falseを指定。</param>
		/// <param name="maxTest">因数として試行する最大数値。</param>
		/// <returns>素因数分解された数式。</returns>
		public static Formula PrimeFactorize(double d, bool containPower, double maxTest)
		{
			Numeric n = new Numeric(d);			
			FormulaAssertionException.Assert(n.IsInteger); // 整数でない数を素因数分解しようとした
			
			d = d / 1;

			List<double> factors = new List<double>();
			if (d < 0)
			{
				factors.Add(-1);
				d *= -1;
			}
			
			double testMax = Math.Min(d, maxTest);
			for (double div = 2; div <= 3; div++)
			{
				if (div * div > d) break;
				while (d % div == 0)
				{
					factors.Add(div);
					d /= div;
				}
			}
			for (double div = 6; div < testMax; div += 6) // 5以上の素数は 6n-1 または 6n+1 と表せる
			{
				double test = 0;
				for (int i = -1; i <= 1; i += 2)
				{
					test = div + i;
					if (test * test > d) break;
					while (d % test ==0)
					{
						factors.Add(test);
						d /= test;
					}				
				}
				if (test * test > d) break;
			}			
			
			if (d != 1) factors.Add(d);			
			if (factors.Count == 1) return factors[0];

            return CreateFactorFormula(factors, containPower);
		}

        /// <summary>
        /// 素因数リストから、素因数分解の数式を構成して取得します。
        /// </summary>
        /// <param name="factors">素因数リスト。</param>
        /// <param name="containPower">累乗を含めるか否か。</param>
        /// <returns>素因数分解の数式。</returns>
        private static Formula CreateFactorFormula(List<double> factors, bool containPower)
        {
			factors.Sort();
            List<Formula> factorFormulas = new List<Formula>();

            if (!containPower)
            {
                factorFormulas.AddRange(factors.Select(d => new Numeric(d)));
            }
            else
            {
                factors.Add(-1d); // 番兵

                int pow = 1;
                Formula current = factors[0];
                for (int i = 1; i < factors.Count; i++)
                {
                    if (factors[i] != current)
                    {
                        if (pow == 1)
                            factorFormulas.Add(current);
                        else
                            factorFormulas.Add(current ^ pow);
                        current = factors[i];
                        pow = 1;
                    }
                    else
                        pow++;
                }
            }

			if (factorFormulas.Count != 1) return new Product(factorFormulas.ToArray());
			return factorFormulas[0];
        }
		
		/// <summary>
		/// 素因数分解を返します
		/// </summary>
		/// <param name="n">対象の整数</param>
		/// <returns></returns>
		public static Formula PrimeFactorize(int n)
		{
			return PrimeFactorize(n, true);
		}

		public override string GetInformation(out List<string> args)
		{
			args = new List<string>(); args.Add("整数");
			return "整数を素因数分解して返します。";
		}
	}
}

