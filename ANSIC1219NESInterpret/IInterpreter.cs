using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSIC1219NESInterpret.InterpretDef;

namespace ANSIC1219NESInterpret
{
    public interface IInterpreter
    {
        IDefElemHandler DefElemHandler { get; }
        ITable Get(int index);
    }

    public interface IInterpreter2<DefElemHandlerClass> : IInterpreter
        where DefElemHandlerClass:IDefElemHandler,new()
    {

        IVerrechnungsdaten DoInterpret(string data);
        IVerrechnungsdaten DoInterpretVerrechnungsdaten(string data);
        IVerrechnungsdaten DoInterpretDemandHistorie(string data);
        IVerrechnungsdaten DoInterpretSelfReadHistorie(string data);
        TLastgang DoInterpretLastgang(int nr,string data);
        DefElemHandlerClass DefElemHandler1 { get; set; }
    }
}
