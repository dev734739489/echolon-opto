using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSIC1219NESInterpret.InterpretDef;


namespace ANSIC1219NESInterpret
{
    /// <summary>
    /// aktuelle Verrechnungsdaten
    /// </summary>
    public class BT23 : TVerrechnungsdaten
    {
        
        public BT23()
            : base(23)
        {
        }

      
        public override bool Interpret(IInterpreter interpreter)
        {
            if (!base.Interpret(interpreter))
                return false;
            try
            {
                if (bt21.DemandResetCounter)
                {
                    //-1 -> DemandResetCounter
                    CurrentListe.Add(new ResultElem(interpreter.DefElemHandler.Get(-1), "") { Value = Rohdaten[0].ToString() });
                }
                //das ist ein gegencheck
                int hSize = bt21.D + bt21.NumberOfTiers * bt21.DataBlockLength;
                if (hSize != Rohdaten.Length)
                {
                    throw new Exception("hSize != Rohdaten.Length");
                }
                InterpretBlock(Rohdaten, 0);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override  string ToVDEW()
        {
            return base.ToVDEW(StandardListe);
        }
    }
}
