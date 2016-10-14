using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{
    public class BlockInfoItem
    {
        public UInt16 NumberValidBlocks { get; set; }
        public UInt16 LastBlock { get; set; }

        public BlockInfoItem()
        {
        }

        public BlockInfoItem(byte[] data, int offset)
        {
            NumberValidBlocks = BitConverter.ToUInt16(data, offset);
            LastBlock = BitConverter.ToUInt16(data, offset + 2);
        }
    }

    public class BT63 : BTable
    {
        public List<BlockInfoItem> BlockInfoListe { get; set; }

        public BT63()
            : base(63)
        {
            BlockInfoListe = new List<BlockInfoItem>();
        }

        public override bool Interpret(IInterpreter interpreter)
        {
            if (!base.Interpret(interpreter))
                return false;
            BlockInfoListe.Add(new BlockInfoItem(Rohdaten, 1));
            BlockInfoListe.Add(new BlockInfoItem(Rohdaten, 1 + 4));
            BlockInfoListe.Add(new BlockInfoItem(Rohdaten, 1 + 4 + 4));
            BlockInfoListe.Add(new BlockInfoItem(Rohdaten, 1 + 4 + 4 + 4));
            return true;
        }

    }
}
