using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{
    public abstract class BTable : ITable
    {
        //protected bool InterpretFlag=false;
        protected IInterpreter Interpreter=null;

        public int Index { get; set; }
        public byte[] Rohdaten { get; set; }
        public List<byte[]> RohdatenListe { get; set; }

        //public abstract void Interpret(IInterpreter interpreter);
        public virtual bool Interpret(IInterpreter interpreter)
        {
            if(Interpreter!=null)
            {
                return false; 
            }
            Interpreter=interpreter;
            return true;

        }

        public BTable(int index)
        {
            Index = index;
            RohdatenListe = new List<byte[]>();
        }

        public void Dispose()
        {
            Rohdaten = null;
            RohdatenListe.Clear();
            Interpreter = null;
        }
    }
}
