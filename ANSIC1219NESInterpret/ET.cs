using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{
    public abstract class ET : BTable
    {
       public const int ETOffset=0x80;

        public ET(int index)
           : base(ETOffset + index)
        {
        }
       
    }
}
