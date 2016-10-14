using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{
    public class BT21:BTable
    {
        public byte byteNull { get; set; }
        public bool EventTimeFlag { get; set; }
        public bool DemandResetCounter { get; set; }
        public bool CumulativeDemand { get; set; }
        public bool ContinuousCumulativeDemand { get; set; }

        public byte NumberOfSelfReads { get; set; }
        public byte NumberOfSummation { get; set; }
        public byte NumberOfDemands { get; set; }
        public byte NumberOfCoincidentValues { get; set; }
        public byte NumberOfOccurrences { get; set; }
        public byte NumberOfTiers { get; set; }


        public UInt16 DmdRcd { get; set; }
        public UInt16 CoinRcd { get; set; }
        public UInt16 A { get; set; }
        public UInt16 B { get; set; }
        public UInt16 C { get; set; }
        public UInt16 D { get; set; }
        public UInt16 DataBlockLength { get; set; }

        public BT21()
            :base(21)
        {
        }

        public override bool Interpret(IInterpreter interpreter)
        {
            if(!base.Interpret(interpreter))
                return false;
            try
            {
                byteNull = Rohdaten[0];
                EventTimeFlag = HelperFunctions.IsBitSet(byteNull, 1);
                DemandResetCounter = HelperFunctions.IsBitSet(byteNull, 2);
                CumulativeDemand = HelperFunctions.IsBitSet(byteNull, 4);
                ContinuousCumulativeDemand = HelperFunctions.IsBitSet(byteNull, 5);
                NumberOfSelfReads = Rohdaten[2];
                NumberOfSummation = Rohdaten[3];
                NumberOfDemands = Rohdaten[4];
                NumberOfCoincidentValues = Rohdaten[5];
                NumberOfOccurrences = Rohdaten[6];
                NumberOfTiers = Rohdaten[7];
                CoinRcd = 0;
                DmdRcd = (UInt16)((HelperFunctions.IsBitSet(byteNull, 1) ? 1 : 0) * NumberOfOccurrences * 5
                        + (CumulativeDemand ? 1 : 0) * 4
                        + (ContinuousCumulativeDemand ? 1 : 0) * 4
                        + NumberOfOccurrences * 4);
                A = (UInt16)(DemandResetCounter ? 1 : 0);
                B = (UInt16)(A + 4 * NumberOfSummation);
                C = (UInt16)(B + NumberOfDemands * DmdRcd);
                D = (UInt16)(C + NumberOfCoincidentValues * CoinRcd);
                DataBlockLength = (UInt16)(4 * NumberOfSummation + NumberOfDemands * DmdRcd);
                //(UInt16)(D + (i - 1) * dataBlockLength)
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

        public BT21(byte[] data)
            : this()
        {
            Rohdaten = data;
        }
    }
}
