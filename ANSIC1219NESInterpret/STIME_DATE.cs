using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{
    /*
    A structure of 5 – UINT8 fields where:
    byte 0 = 2-digit year
    byte 1 = month
    byte 2 = day
    byte 3 = hour
    byte 4 = minute
     */
    public class STIME_DATE
    {
        public int Year { get; set; }
        public byte Month { get; set; }
        public byte Day { get; set; }
        public byte Hour { get; set; }
        public byte Minute { get; set; }
        public byte[] _Rohdata = null;

        public virtual byte[] Rohdata
        {
            set
            {
                _Rohdata = value;
                Year = 2000 + _Rohdata[0];
                Month = _Rohdata[1];
                Day = _Rohdata[2];
                Hour = _Rohdata[3];
                Minute = _Rohdata[4];
            }
        }

        public STIME_DATE()
        {
            Year = 0;
            Month = 0;
            Day = 0;
            Hour = 0;
            Minute = 0;
        }

        public STIME_DATE(byte[] rohdata,int offset)
            : this()
        {
            byte[] hdata = new byte[6];
            Array.Copy(rohdata, offset, hdata, 0, 5);
            Rohdata = hdata;
        }

        public new string ToString()
        {
            string erg = "";
            erg += string.Format("{0:00}.{1:00}.{2:0000} {3:00}:{4:00}:{5:00}",Day,Month,Year,Hour,Minute,0);
            return erg;
        }

    }
}
