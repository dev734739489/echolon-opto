using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret.InterpretDef
{
    public interface IDefElem
    {
        string Info { get; set; }
        int SourceID { get; set; }
        string OBISOffset { get; set; }
        string OBISOffsetSelfRead { get; set; }
        string OBISOffsetDemand { get; set; }
        string Unit { get; set; }
        string UnitSelfRead { get; set; }
        string UnitDemand { get; set; }
        double Divisor { get; set; }
    }
}
