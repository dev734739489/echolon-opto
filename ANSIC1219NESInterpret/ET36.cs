using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{

    public class ET36Entry
    {
        byte[] Rohdata { get; set; }
        public UInt16 Table { get; set; }
        public UInt16 EntrySize { get; set; }
        public UInt16 MaxEntries { get; set; }
        public UInt16 CurrentEntries { get; set; }

        public ET36Entry()
        {
            Rohdata = new byte[17];
        }

        public ET36Entry(byte[] data, int offset)
            : this()
        {
            for (int l = 0; l < 17; l++)
            {
                Rohdata[l] = data[offset + l];
            }
            Table = BitConverter.ToUInt16(Rohdata, 0);
            EntrySize = BitConverter.ToUInt16(Rohdata, 2);
            MaxEntries = BitConverter.ToUInt16(Rohdata, 4);
            CurrentEntries = BitConverter.ToUInt16(Rohdata, 6);
        }
    }

    public class ET36:ET
    {
        public byte Anzahl { get; set; }
        public List<ET36Entry> Liste { get; set; }

        public ET36()
            : base(36)
        {
            Liste = new List<ET36Entry>();
        }

        public override bool Interpret(IInterpreter interpreter)
        {
            Anzahl = Rohdaten[0];
            for (byte l = 0; l < Anzahl; l++)
            {
                Liste.Add(new ET36Entry(Rohdaten, 1 + 17 * l));
            }
            return true;
        }

    }
}
