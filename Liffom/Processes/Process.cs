using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Utilities;

namespace GoodSeat.Liffom.Processes
{
    /// <summary>
    /// 非同期処理をサポートする数式処理を表します。
    /// </summary>
    public abstract class Process : EAP<Formula, Formula>
    {
    }
}
