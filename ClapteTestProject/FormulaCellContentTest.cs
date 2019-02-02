// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoodSeat.Clapte.Models;
using GoodSeat.Clapte.Solvers;
using GoodSeat.Clapte.ViewModels;

namespace GoodSeat.ClapteTestProject
{
    /// <summary>
    /// ClaptePadの評価テストクラスです。
    /// </summary>
    [TestClass]
    public class FormulaCellContentTest
    {
		/// <summary>
		/// ClaptePadの評価テスト
		/// </summary>
		[TestCategory("ClaptePad"), TestMethod()]
        public void EvaluateTest()
        {
            // 小数モード、有効数字を考慮しない
            var mode = CalculateMode.Decimal;
            Solver solver = SolverTest.CreateStandardSolver(mode, false);
            solver.AbortLevel = Error.Level.Error;

            var tests = new List<KeyValuePair<string, string>>();
            tests.Add(new KeyValuePair<string, string>("[m] 42.195[km] = x",            "x = 42195[m]"));
            tests.Add(new KeyValuePair<string, string>("[m] x = 42.195km",              "x = 42195 m "));
            tests.Add(new KeyValuePair<string, string>("[N/m]24kN/m^3 * 150mm * 350mm", "1260 N/m "));
            tests.Add(new KeyValuePair<string, string>("[N/m^2]  1 kN/cm^2",            "10000000 N/(m^2) "));
            tests.Add(new KeyValuePair<string, string>("[N/m^2] 10 kN/cm^2",            "100000000 N/(m^2) "));
            tests.Add(new KeyValuePair<string, string>("0.7(N/mm^2) + 7kN/cm^2",         "70.7(N/(mm^2))"));
            tests.Add(new KeyValuePair<string, string>("x = 7",                         "x = 7"));
            tests.Add(new KeyValuePair<string, string>("x * 3",                         "21"));
            tests.Add(new KeyValuePair<string, string>("x * 3 =",                        "x * 3 = 21"));
            tests.Add(new KeyValuePair<string, string>("x = 5",                         "x = 5"));
            tests.Add(new KeyValuePair<string, string>("x * 3",                         "15"));
            Check(solver, tests);

            // 分数モード、有効数字を考慮しない
            mode = CalculateMode.Fraction;
            solver = SolverTest.CreateStandardSolver(mode, false);
            solver.AbortLevel = Error.Level.Error;

            tests = new List<KeyValuePair<string, string>>();
            tests.Add(new KeyValuePair<string, string>("104.1kN*cm / 247cm^3",          "(1041/2470) kN/(cm^2) "));
            tests.Add(new KeyValuePair<string, string>("Fc = 18",                       "Fc = 18"));
            tests.Add(new KeyValuePair<string, string>("uLS = 1.5",                     "uLS = 3/2"));
            tests.Add(new KeyValuePair<string, string>("min(a, b) = (a < b) * a + (a >= b) * b", "min(a, b) = a*(a < b) + b*(a≧b)"));
            tests.Add(new KeyValuePair<string, string>("min(1/15 * Fc, 0.9 + 2/75 * Fc) * uLS", "9/5"));
            Check(solver, tests);


            // 小数モード、有効数字を考慮する
            mode = CalculateMode.Decimal;
            solver = SolverTest.CreateStandardSolver(mode, true);
            solver.AbortLevel = Error.Level.Error;

            tests = new List<KeyValuePair<string, string>>();
            tests.Add(new KeyValuePair<string, string>("log(?,10) = 1.636",             "? = 4.33E+1"));
            tests.Add(new KeyValuePair<string, string>("sin(2.14)",                     "8.4E-1"));
            tests.Add(new KeyValuePair<string, string>("sin(0.79rad)",                  "7.1E-1"));
            tests.Add(new KeyValuePair<string, string>("x^2 + 2x = 120",                "x = -12, 10"));
            tests.Add(new KeyValuePair<string, string>("x^2 + 2.x = 120",               "x = -1.2E+1, 1.0E+1"));
            Check(solver, tests);


            // 分数モード、有効数字を考慮する
            mode = CalculateMode.Fraction;
            solver = SolverTest.CreateStandardSolver(mode, true);
            solver.AbortLevel = Error.Level.Error;

            tests = new List<KeyValuePair<string, string>>();
            tests.Add(new KeyValuePair<string, string>("5.324/100",                 "1.331E+3/25000"));
            tests.Add(new KeyValuePair<string, string>("9.5cm",                     "(1.9E+1/2) cm "));
            Check(solver, tests);
        }


        private void Check(Solver solver, List<KeyValuePair<string, string>> targets)
        {
            List<FormulaCell> list = new List<FormulaCell>();

            foreach (var pair in targets)
            {
                FormulaCell cell = new FormulaCell(pair.Key, solver, list.ToArray());
                list.Add(cell);

                cell.Evaluate(solver);
                var res = cell.Content.ResultText;
				Assert.AreEqual(pair.Value, res);
            }
        }
    }
}
