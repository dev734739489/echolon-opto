using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{
    public class ET42ProfileInformation
    {
        public static int BaseSize { get { return 14; } }

        public bool ActiveFlag { get; set; }

        public UInt32 BlockSize { get; set; }
        public UInt16 NumberOfBlocks { get; set; }
        public byte NumberOfChannels { get; set; }
        public byte IntervalDuration { get; set; }
        public byte IntervalSize { get; set; }
        public byte Options { get; set; }
        public UInt16 IntervalsPerBlock { get; set; }
        public byte BlockStartHour { get; set; }
        public byte BlockStartMinute { get; set; }
        public byte[] LoadProfileExtendedSources { get; set; }
      
        public ET42ProfileInformation()
        {
            ActiveFlag = false;
        }

        public ET42ProfileInformation(byte[] data, int offset)
            : this()
        {
            ActiveFlag = true;
            BlockSize = BitConverter.ToUInt32(data, offset);
            NumberOfBlocks = BitConverter.ToUInt16(data, offset + 4);
            NumberOfChannels = data[offset + 6];
            IntervalDuration = data[offset + 7];
            IntervalSize = data[offset + 8];
            Options = data[offset + 9];
            IntervalsPerBlock = BitConverter.ToUInt16(data, offset + 10);
            BlockStartHour = data[offset + 12];
            BlockStartMinute = data[offset + 13];
            LoadProfileExtendedSources = new byte[NumberOfChannels];
            offset += 13;
            for (int i = 0; i < NumberOfChannels; i++)
            {
                LoadProfileExtendedSources[i] = data[offset]; offset++;
            }


        }
    }

    public class ET42 : ET
    {
        public byte LoadProfileBitMask { get; set; }
        public byte FSL { get; set; }
        public byte LLS { get; set; }
        public byte NumberOfDemands { get; set; }
        public byte NumberOfCoincidentValues { get; set; }
        public byte NumberOfChannels { get; set; }
        public byte MTTCurrentEntries { get; set; }
        public int AOffset { get; set; }

        public List<ET42ProfileInformation> ProfileInformationList { get; set; }

        public ET42()
            : base(42)
        {
            ProfileInformationList = new List<ET42ProfileInformation>();
        }

        public override bool Interpret(IInterpreter interpreter)
        {
            if (!base.Interpret(interpreter))
                return false;
            BT61 bt61 = Interpreter.Get(61) as BT61;
            LoadProfileBitMask = Rohdaten[37];
            int offset = 23;
            ProfileInformationList.Add(new ET42ProfileInformation(Rohdaten, offset));
            offset = 38;
            //offset bestimmen
            FSL = Rohdaten[2];
            LLS = Rohdaten[3];
            NumberOfDemands = Rohdaten[13];
            NumberOfCoincidentValues = Rohdaten[14];
            NumberOfChannels = Rohdaten[29];
            int MTTCurrentEntriesIndex = FSL + 3 * LLS + NumberOfDemands + NumberOfCoincidentValues + 2 * NumberOfChannels;
            MTTCurrentEntries = Rohdaten[MTTCurrentEntriesIndex];
            AOffset = MTTCurrentEntriesIndex + 1 + (MTTCurrentEntries * 4);
            offset = AOffset + 1;

            for (int i = 0; i < 3; i++)
            {
                if (HelperFunctions.IsBitSet(LoadProfileBitMask, 0))
                {
                    ProfileInformationList.Add(new ET42ProfileInformation(Rohdaten, offset));
                    offset += ET42ProfileInformation.BaseSize + bt61.ConfigListe[i + 1].NumberOfChannels * 2;
                }
                else
                {
                    ProfileInformationList.Add(new ET42ProfileInformation());
                }
            }
            return true;
        }
    }
}
