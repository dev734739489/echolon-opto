using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret.InterpretDef
{
    public class ResultElem
    {
        public int SourceID { get; set; }
        public string Obis { get; set; }
        public string Value { get; set; }
        public string Info { get; set; }
        public string Unit { get; set; }

        public ResultElem()
        {
            Info = "";
            Unit="";
        }

        public ResultElem(IDefElem defElem, string ObisPostfix)
            :this()
        {
            SourceID = defElem.SourceID;
            Info = defElem.Info;
            Obis = defElem.OBISOffset + ObisPostfix;
            Unit=defElem.Unit;
        }
        public override string ToString()
        {
            return ToVDEW();
        }
        public string ToVDEW()
        {
            //return Obis + " (" + Value + ") " + Unit +  " [" + SourceID.ToString() + " " + Info + "]" + "\r\n";
            if (Value == "null")
            {
                return Obis + "\r\n";
            }
            else
            {
                return Obis + " (" + Value + ") " + Unit + "\r\n";
            }
        }
    }
}
