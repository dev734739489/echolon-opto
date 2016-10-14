using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{
    public class HelperFunctions
    {
        public static ushort ToBigIndian(ushort n)
        {
            bool b = BitConverter.IsLittleEndian;
            return (ushort)((n >> 8) + ((n & 0x00FF) << 8));
        }

        //position von rechts !!
        public static bool IsBitSet(byte b, byte position)
        {
            return (b & (1 << position)) != 0;
        }


        public static byte GetPartValue(byte value, int fromBit, int size)
        {
            byte result = value;
            value <<= fromBit;
            result >>= (8 - fromBit - size);
            return result;

        }

        //public static ushort GetPartValue(ushort value, int fromBit, int size)
        //{
        //    ushort result = value;
        //    value <<= fromBit;
        //    result >>= (8 - fromBit - size);
        //    return result;

        //}

        public static UInt16 GetPartValue(UInt16 value, int fromBit, int size)
        {
            UInt16 result = value;
            value <<= fromBit;
            result >>= (16 - fromBit - size);
            return result;

        }

        //position von links nach rechts
        public static byte SetBit(byte b, byte position, bool value)
        {
            byte bitmuster = 0x80; //das erste bit
            bitmuster >>= position;
            if (value)
            {
                b |= bitmuster;
            }
            else
            {
                int hb = bitmuster;
                bitmuster = (byte)(~hb);
                b &= bitmuster;
            }
            return b;


        }
    }
}
