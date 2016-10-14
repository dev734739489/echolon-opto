using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{
    /*
      LTIME_DATE A structure of 6 – UINT8 fields where:
          byte 0 = 2-digit year (02 = 2002)
          byte 1 = month (01 = January, 02 = February, etc.)
          byte 2 = day
          byte 3 = hour
          byte 4 = minute
          byte 5 = second
       * */
    public class LTIME_DATE : STIME_DATE
    {
        
        public byte Second { get; set; }
        public override byte[] Rohdata
        {
            set
            {
                base.Rohdata = value;
                Second = _Rohdata[5];
            }
        }

        public LTIME_DATE()
            : base()
        {
            Year = 0;
            Month = 0;
            Day = 0;
            Hour = 0;
            Minute = 0;
            Second = 0;
        }

        public new string ToString()
        {
            try
            {
                DateTime date = new DateTime(Year, Month, Day, Hour, Minute, Second);

                return date.ToString();
            }
            catch (Exception ex)
            {
                string ignore = ex.Message;
                return "00.00.00000 00:00:00";
                //return "DateTime failure";
            }
        }

        public LTIME_DATE(byte[] rohdata,int offset)
            : this()
        {
            byte[] hdata = new byte[6];
            Array.Copy(rohdata, offset, hdata, 0, 6);
            Rohdata = hdata;
        }

        public static byte[] GetUTCTimeBuffer()
        {
            byte[] result = new byte[6];
            DateTime dt = DateTime.UtcNow;
            result[0] = (byte)(dt.Year - 2000);
            result[1] = (byte)dt.Month;
            result[2] = (byte)dt.Day;
            result[3] = (byte)dt.Hour;
            result[4] = (byte)dt.Minute;
            result[5] = (byte)dt.Second;
            return result;
        }
    }
}
