using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{
    public class ET3 : ET
    {
        public string SerialNr { get; set; }

        public ET3()
            : base(3)
        {
        }

        public override bool Interpret(IInterpreter interpreter)
        {
            if(!base.Interpret(interpreter))
                return false;
            try
            {
                SerialNr = System.Text.Encoding.UTF8.GetString(Rohdaten, 0, 30).Trim();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
