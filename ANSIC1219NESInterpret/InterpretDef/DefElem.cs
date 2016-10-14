using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret.InterpretDef
{
    public class DefElem : IDefElem
    {
        public string Info{ get;set;}
        public int SourceID { get; set; }
        public string OBISOffset { get; set; }
        public string OBISOffsetSelfRead { get; set; }
        public string OBISOffsetDemand { get; set; }
        public string Unit { get; set; }
        public string UnitSelfRead { get; set; }
        public string UnitDemand { get; set; }
        public double Divisor { get; set; }

        public DefElem()
        {
            Divisor = 1;
            //OBISOffsetSelfRead = "";
        }

    }
}
