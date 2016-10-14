using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{
    public class DataSetConfigItem
    {
        public UInt16 NumberOfBlocks { get; set; }
        public UInt16 NumberOfIntervals { get; set; }
        public byte NumberOfChannels { get; set; }
        public byte MaxIntervalTime { get; set; }

        public UInt16 Nr7 { get { return NumberOfBlocks; } }
        public UInt16 Nr9 { get { return NumberOfIntervals; } }
        public byte Nr11 { get { return NumberOfChannels; } }

        public DataSetConfigItem()
        {
        }

        public DataSetConfigItem(byte[] data, int offset)
            : this()
        {
            NumberOfBlocks = BitConverter.ToUInt16(data, offset);
            NumberOfIntervals = BitConverter.ToUInt16(data, offset + 2);
            NumberOfChannels = data[offset + 4];
            MaxIntervalTime = data[offset + 5];
        }
    }

    public class BT61 : BTable
    {
        public bool EndReadingSupported { get; set; }
        public List<DataSetConfigItem> ConfigListe { get; set; }

        public BT61()
            : base(61)
        {
            ConfigListe = new List<DataSetConfigItem>();
        }

        public override bool Interpret(IInterpreter interpreter)
        {
            if (!base.Interpret(interpreter))
                return false;
            byte ViertesByte = Rohdaten[4];
            EndReadingSupported = HelperFunctions.IsBitSet(ViertesByte, 4);
            ConfigListe.Add(new DataSetConfigItem(Rohdaten, 7));
            ConfigListe.Add(new DataSetConfigItem(Rohdaten, 7 + 6));
            ConfigListe.Add(new DataSetConfigItem(Rohdaten, 7 + 6 + 6));
            ConfigListe.Add(new DataSetConfigItem(Rohdaten, 7 + 6 + 6 + 6));
            return true;
        }

    }
}
