using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{
    public class BT1:BTable
    {
        public string FirmwareNumber { get; set; }

        public BT1()
            : base(1)
        {
        }

        public override bool Interpret(IInterpreter interpreter)
        {
            if (!base.Interpret(interpreter))
                return false;
            try
            {
                int MajorVersion = 0;
                int MinorVersion = 0;
                int Build = 0;
                //FirmwareData result = new FirmwareData();
                //byte[] data = Extract(SendAndReceiveFrame(new PartialReadFrame(new byte[] { 0x00, 0x01 }, 14, 2)));
                //Info("GetFirmware", data);
                //die ersten vier bits
                byte hData = Rohdaten[14];//data[0];
                hData &= 0xF0;
                hData >>= 4;
                MajorVersion = hData;
                //die letzten vier bits
                hData = Rohdaten[14];
                hData &= 0x0F;
                hData <<= 4;
                //die ersten vier bits
                byte hData2 = Rohdaten[15];
                hData2 &= 0xF0;
                hData2 >>= 4;
                MinorVersion = hData;
                MinorVersion += hData2;
                //die letzten vier bits
                hData = Rohdaten[15];
                hData &= 0x0F;
                Build = hData;

                FirmwareNumber = string.Format("{0}.{1}.{2}", MajorVersion, MinorVersion, Build);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
