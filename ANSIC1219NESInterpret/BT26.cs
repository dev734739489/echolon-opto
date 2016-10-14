using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSIC1219NESInterpret.InterpretDef;

namespace ANSIC1219NESInterpret
{
    /// <summary>
    ///// SelfRead Historie
    /// </summary>
    public class BT26 : TVerrechnungsdatenHistorie
    {
        public BT26()
            : base(26)
        {
        }

        public override bool Interpret(IInterpreter interpreter)
        {
            if (!base.Interpret(interpreter))
                return false;
            try
            {

                for (int i = 0; i < RohdatenListe.Count; i++)
                {
                    List<ResultElem> newDemand = new List<ResultElem>();
                    HistorieList.Add(newDemand);
                    CurrentListe = newDemand;
                    int offset = 0;
                    Rohdaten = RohdatenListe[i];
                    //das ist ein gegencheck
                    int hSize = bt21.D + bt21.NumberOfTiers * bt21.DataBlockLength;
                    if (bt21.NumberOfDemands > 0)
                    {
                        hSize += 2;
                    }
                    hSize += 5 + 1;
                    if (hSize != Rohdaten.Length)
                    {
                        throw new Exception("hSize != Rohdaten.Length");
                    }
                    if (bt21.NumberOfDemands > 0)
                    {
                        UInt16 BIDN = BitConverter.ToUInt16(Rohdaten, offset); offset += 2;
                        AddResultElem(new ResultElem() { Obis = "BIDN", Value = BIDN.ToString() });
                    }
                    LTIME_DATE dateTime = new LTIME_DATE(Rohdaten, offset); offset += 5;
                    AddResultElem(new ResultElem() { Obis = "Datum", Value = dateTime.ToString() });
                    byte Season = Rohdaten[offset]; offset++;
                    AddResultElem(new ResultElem() { Obis = "Season", Value = Season.ToString() });
                    InterpretBlock(Rohdaten, offset);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
