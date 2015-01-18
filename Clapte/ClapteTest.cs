using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Sio.Test;
using GoodSeat.Sio.Xml;
using GoodSeat.Clapte.Solvers;

namespace GoodSeat.Clapte
{
	/// <summary>
	/// Clapteの自己テストを行うクラスメソッドを提供します。
	/// </summary>
	internal class ClapteTest
	{
		/// <summary>
		/// 指定ソルバのテストケースの作成を行います。
		/// </summary>
		/// <param name="targetSolver">テスト対象とする計算部</param>
		public static void CreateTestCase(Solver targetSolver)
		{
			/*
			Tester<Solver, string, string> tester =
				new Tester<Solver, string, string>(targetSolver, (target, input) =>
				{
					Clapte.Core.ClapteSolverException exc;
					return target.SolveFormula(input, out exc, true);
				});

			TestInput<Solver, string, string> testInput = new TestInput<Solver, string, string>();
			testInput.InputDeserializing = DoTestCommand.InputDeserialize;
			testInput.InputSerializing = DoTestCommand.InputSerialize;

			Dictionary<string, string[]> valueTestCase = new Dictionary<string, string[]>();
			{

			}
			Dictionary<string, string[]> attributeTestCase = testInput.AttributeTestCase;
			{
				attributeTestCase.Add("ConsiderDigit", new string[] { "True", "False" });
				attributeTestCase.Add("UnitBracket", new string[] { "Auto", "None", "Small", "Large"});
			}
			List<string> inputs = testInput.Inputs;
			{
				inputs.Add("83*(3+8/3)");
				inputs.Add("1,200円/時間×6.2時間");
				inputs.Add("5.00[kgf] * 100.0[mm]");
				inputs.Add("[N*m] 5.00[kgf] * 100.0[mm]");
				inputs.Add("[g] 11.3*10^3[kg/m^3] * 0.240[mm^3]");
				inputs.Add("[kN/m] 24N/cm^2 * 60mm");
				inputs.Add("[h] 42.195km / 3.2[m/s]");
				inputs.Add("[kN･cm/m] 24N/cm^2 * 60mm * 2m");

				inputs.Add("[Gy] 8.002N * 5.200cm / 2.200E-2[t]");
				inputs.Add("[kN] 3531.6[t*mm/(h*s)] + 0.01[kgf] + 1[N]");
				inputs.Add("1.545g + 55mg");
				inputs.Add("4.25 - 4.20");
				inputs.Add("4.23 * 0.38");
				inputs.Add("5.00 km / 3.2 h");
				inputs.Add("[mm^2] (4.23[m]) ^ 2");
				inputs.Add("1.6[t*mm/(s*s)] + 1[N]");
				inputs.Add("5kg + 500g");
				inputs.Add("500g + 5kg");
				inputs.Add("[J/degF] 10[J/degC]");
				inputs.Add("[J/K] 10[J/degC]");
				inputs.Add("[degF] 100[degC]");
				inputs.Add("[K] 100[degC]");
				inputs.Add("5E10+5E9");
				inputs.Add("99.96 - 99.87");

				inputs.Add("sin(45deg)-1/2^0.5");
				inputs.Add("100-100.1+0.1");
				inputs.Add("101-100.9");
				inputs.Add("0.09-(0.1-0.01)");
				inputs.Add("(1.26683-((1.26683-0.73316)*2.474/7.422)/2)*22.03");
				inputs.Add("(5+2)cm^2*2");
				inputs.Add("6/2(1+2)");
                inputs.Add("1.5 * 1.96(kN/m)");
                inputs.Add("5sin(π/3)cos(π/3)");

				inputs.Add("?^3 - 6?^2 + 11? - 6 = 0");
				inputs.Add("[kN/m] ? * (1.15m)^2 = 560 kN*m");
				inputs.Add("3?^2 - 45? = 0");
				inputs.Add("[mm] (? + 2cm)^3 = 45cm^3");
				inputs.Add("?^8 = 64");
				inputs.Add("? * sin(?) = 0.2");
				inputs.Add("abs(?^3) = 43");
			}

			XmlElement inputCaseElement = new XmlElement("TestCase");
			testInput.OnSerialize(inputCaseElement);

			XmlFile inputCaseFile = new XmlFile(DoTestCommand.TestCaseFilePath, inputCaseElement);
			inputCaseFile.Save();
			*/
		}

		/// <summary>
		/// 指定ソルバのテストを実施します。
		/// </summary>
		/// <param name="targetSolver">テスト対象とする計算部</param>
		public static void CheckTestOf(Solver targetSolver)
		{
			/*
			Tester<Solver, string, string> tester =
				new Tester<Solver, string, string>(targetSolver, (target, input) =>
				{
					Clapte.Core.ClapteSolverException exc;
					return target.SolveFormula(input, out exc, true);
				});

			tester.TestComparingWith((element) => element.GetElement("Input").Value, new TestOutput<Solver, string, string>("SolverTestCase.xml"));
			*/
		}
	}
}
