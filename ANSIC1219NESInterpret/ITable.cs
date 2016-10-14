using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{
    public interface  ITable:IDisposable
    {
        int Index { get; set; }
        byte[] Rohdaten { get; set; }
        List<byte[]> RohdatenListe { get; set; }
        bool Interpret(IInterpreter interpreter);
    }
}
