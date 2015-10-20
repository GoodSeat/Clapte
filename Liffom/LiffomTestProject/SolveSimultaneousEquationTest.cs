using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoodSeat.Liffom.Processes;
using GoodSeat.Liffom.Formulas;

namespace LiffomTestProject
{
    [TestClass]
    public class SolveSimultaneousEquationTest
    {
        /// <summary>
        /// Solve のテスト
        /// </summary>
        [TestCategory("方程式"), TestMethod()]
        [DeploymentItem("Liffom.dll")]
        public void SolveSimultaneousTest()
        {
            var solver = new SolveSimultaneousEquation();

            SolveSimultaneousTest(solver, "x=7/2, y= -1/2",
                "x+y=3", "x-y=4",
                "x,y");
            SolveSimultaneousTest(solver, "x=7/3, y=(-5-3*z)/3",
                "2*x+y+z=3", "x-y-z=4",
                "x,y,z");

            SolveSimultaneousTest(solver, "x= -1*(y-4)*2^-1",
                "2*x+y=4", "4*x+2*y=5",
                "x,y");

            SolveSimultaneousTest(solver, "x=59/669, y=-126/223, z=-1840/669",
                "x+y/3-2*z/5=1", "x-2*y/7-z=3", "3*x/2+3*y-3*z/4=1/2",
                "x,y,z");

            SolveSimultaneousTest(solver, "a = g/3, T = 4*g*m/3",
                "T - m*g = m*a", "2*m*g - T = 2*m*a",
                "a,T");

            SolveSimultaneousTest(solver,"x= -1*(y-4)*2^-1",
                "x-2+(y-3)^2=36", "x^2+y^2=36",
                "x,y");
        }

        public void SolveSimultaneousTest(SolveSimultaneousEquation solver, string expected, params string[] targets)
        {
            var result = solver.Do(targets.Select(f => Formula.Parse(f)).ToArray());
            Assert.AreEqual(Formula.Parse(expected), result);
        }

    }
}
