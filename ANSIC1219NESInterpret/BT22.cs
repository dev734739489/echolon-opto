using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{
    public class BT22:BTable
    {
        public List<byte> SummationSources { get; set; }
        public List<byte> DemandSelect { get; set; }
        public List<bool> MinMaxFlags { get; set; }
        public List<byte> CoincidentSelect { get; set; }
        public List<byte> CoincidentDemandAssociated { get; set; }

        public BT22()
            :base(22)
        {
            SummationSources = new List<byte>();
            DemandSelect = new List<byte>();
            MinMaxFlags = new List<bool>();
            CoincidentSelect = new List<byte>();
            CoincidentDemandAssociated = new List<byte>();
        }

        public string SummationSourcesToString()
        {
            return BitConverter.ToString(SummationSources.ToArray());
        }

        public string DemandSelectToString()
        {
            return BitConverter.ToString(DemandSelect.ToArray());
        }

        public override bool Interpret(IInterpreter interpreter)
        {
            if(!base.Interpret(interpreter))
                return false;
            try
            {
                BT21 bt21 = interpreter.Get(21) as BT21;
                if (bt21 == null)
                    throw new Exception("BT22: BT21 nicht vorhanden");
                bt21.Interpret(interpreter);
                int offset = 0;
                for (int i = 0; i < bt21.NumberOfSummation; i++)
                {
                    SummationSources.Add(Rohdaten[offset+i]);
                }
                offset=bt21.NumberOfSummation;
                for (int i = 0; i < bt21.NumberOfDemands; i++)
                {
                    DemandSelect.Add(Rohdaten[offset+i]);
                }
                offset = bt21.NumberOfSummation + bt21.NumberOfDemands;
                int MinMaxSize = (offset + 7) / 8;
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
