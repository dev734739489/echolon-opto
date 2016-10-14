using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret.InterpretDef
{
    public interface IDefElemHandler
    {
        IDefElem Get(int sourceID);
        void ReadDef(string datei);
    }
}
