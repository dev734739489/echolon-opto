using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSIC1219NESInterpret.InterpretDef;

namespace ANSIC1219NESInterpret
{
    public interface IVerrechnungsdaten
    {
        string ToVDEW();
    }

    public abstract class TVerrechnungsdaten : BTable, IVerrechnungsdaten
    {
        protected UInt16 Size { get; set; }
        protected BT1 bt1 { get; set; }
        protected BT21 bt21 { get; set; }
        protected BT22 bt22 { get; set; }
        protected ET3 et3 { get; set; }

        public List<ResultElem> StandardListe { get; set; }
        protected List<ResultElem> CurrentListe;

        public TVerrechnungsdaten(int index)
            : base(index)
        {
            StandardListe = new List<ResultElem>();
        }

        protected void AddResultElem(ResultElem item)
        {
            CurrentListe.Add(item);
        }

        private void BerechneSize(BT21 bt21)
        {
            UInt16 result = 0;
            //RegisterDataRcd
            //Nbr Demand Resets
            result += bt21.A;
            //Summantion 
            result += (UInt16)(bt21.NumberOfSummation /*array-size*/ * 4 /*element-size*/);
            //DmRcd
            UInt16 hDmcRcdSize = 0;
            //array of STIME_DATE
            hDmcRcdSize += (UInt16)((bt21.EventTimeFlag ? 1 : 0) * bt21.NumberOfOccurrences * 5);
            hDmcRcdSize += (UInt16)((bt21.CumulativeDemand ? 1 : 0) * 4);
            hDmcRcdSize += (UInt16)((bt21.ContinuousCumulativeDemand ? 1 : 0) * 4);
            hDmcRcdSize += (UInt16)(bt21.NumberOfOccurrences * 4);
            result += (UInt16)(bt21.NumberOfDemands * hDmcRcdSize);

            //CoinRcd
            UInt16 hCoinRcdCount = bt21.NumberOfCoincidentValues; //sollte immer 0 sein
            result += (UInt16)(hCoinRcdCount * bt21.NumberOfOccurrences * 4);

            //TierDataBlock
            //Summations
            UInt16 hSummation = (UInt16)(bt21.NumberOfSummation * 4);
            //Demands 
            UInt16 hDemands = (UInt16)(bt21.NumberOfDemands * hDmcRcdSize);
            //Coincidents
            UInt16 hCoincidents = (UInt16)(hCoinRcdCount * bt21.NumberOfOccurrences * 4);
            result += (UInt16)(bt21.NumberOfTiers * (hSummation + hDemands + hCoincidents));
            Size = result;
        }

        private void BaseInterpret()
        {

            bt1 = Interpreter.Get(1) as BT1;
            if (bt1 == null)
                throw new Exception("BT23: BT1 nicht vorhanden");
            bt1.Interpret(Interpreter);

            et3 = Interpreter.Get(0x80 + 3) as ET3;
            if (et3 == null)
                throw new Exception("BT23: ET3 nicht vorhanden");
            et3.Interpret(Interpreter);

            bt21 = Interpreter.Get(21) as BT21;
            if (bt21 == null)
                throw new Exception("BT23: BT21 nicht vorhanden");
            bt21.Interpret(Interpreter);

            bt22 = Interpreter.Get(22) as BT22;
            if (bt22 == null)
                throw new Exception("BT23: BT22 nicht vorhanden");
            bt22.Interpret(Interpreter);
            BerechneSize(bt21);
        }

        protected void InterpretSummations(byte[] data, int offset, string tarif)
        {
            //Summations
            for (int i = 0; i < bt21.NumberOfSummation; i++)
            {
                IDefElem def = Interpreter.DefElemHandler.Get(bt22.SummationSources[i]);
                if (def == null)
                    continue;
                UInt32 value = BitConverter.ToUInt32(data, offset + i * 4);
                double dv = value / def.Divisor;
                AddResultElem(new ResultElem(def, tarif) { Value = dv.ToString().Replace(",", ".") });
            }
        }

        protected void InterpretDemands(byte[] data, int offset, string tarif)
        {
            for (int i = 0; i < bt21.NumberOfDemands; i++)
            {
                int hOffset = 0;
                if (bt21.EventTimeFlag)
                {
                    //-4 -> DatumUhrzeit
                    IDefElem def = Interpreter.DefElemHandler.Get(-4);
                    for (int j = 0; j < bt21.NumberOfOccurrences; j++)
                    {
                        LTIME_DATE value = new LTIME_DATE(data, offset + i * bt21.DmdRcd + j * 5);
                        if (def == null)
                            continue;
                        ResultElem elem = new ResultElem(def, "0");
                        elem.Obis = def.OBISOffset + j.ToString();
                        elem.Value = value.ToString();
                        AddResultElem(elem);
                    }
                    hOffset += 5 * bt21.NumberOfOccurrences;
                }
                if (bt21.CumulativeDemand)
                {
                    UInt32 value = BitConverter.ToUInt32(data, offset + i * bt21.DmdRcd + hOffset);
                    IDefElem def = Interpreter.DefElemHandler.Get(-100);
                    if (def != null)
                    {
                        //continue;
                        double dv = value / def.Divisor;
                        AddResultElem(new ResultElem(def, "-100") { Value = dv.ToString().Replace(",", ".") });
                    }
                    hOffset += 4;
                }
                if (bt21.ContinuousCumulativeDemand)
                {
                    UInt32 value = BitConverter.ToUInt32(data, offset + i * bt21.DmdRcd + hOffset);
                    IDefElem def = Interpreter.DefElemHandler.Get(-101);
                    if (def != null)
                    {
                        //continue;
                        double dv = value / def.Divisor;
                        AddResultElem(new ResultElem(def, "-101") { Value = dv.ToString().Replace(",", ".") });
                    }
                    hOffset += 4;
                }
                for (int j = 0; j < bt21.NumberOfOccurrences; j++)
                {
                    UInt32 value = BitConverter.ToUInt32(data, offset + i * bt21.DmdRcd + hOffset + j * 4);
                    IDefElem def = Interpreter.DefElemHandler.Get(bt22.DemandSelect[i]);
                    if (def != null)
                    {
                        //continue;
                        ResultElem elem = new ResultElem(def, i.ToString());
                        elem.Obis = def.OBISOffsetDemand + tarif;
                        elem.Value = value.ToString();
                        AddResultElem(elem);
                    }
                    hOffset += 4;
                }
            }
        }

        protected void InterpretBlock(byte[] data, int offset)
        {
            //Summations
            InterpretSummations(data, offset + bt21.A, "0");
            //Demands
            InterpretDemands(data, offset + bt21.B, "0");
            //Coincidents
            InterpretCoincidents(data, offset + bt21.C, "0");
            ////diese sollten gar nicht vorkommen
            for (int i = 0; i < bt21.NumberOfTiers; i++)
            {
                int hOffset = offset + bt21.D + i * bt21.DataBlockLength;
                InterpretSummations(data, hOffset, (i + 1).ToString());
                hOffset += 4 * bt21.NumberOfSummation;
                InterpretDemands(data, hOffset, (i + 1).ToString());
                hOffset += bt21.NumberOfDemands * bt21.DmdRcd;
                InterpretCoincidents(data, hOffset, (i + 1).ToString());
            }
        }

        protected void InterpretCoincidents(byte[] data,int offset, string tarif)
        {
            //diese sollten gar nicht vorkommen
            for (int i = 0; i < bt21.NumberOfCoincidentValues; i++)
            {
            }
        }

        protected void AddStandardItems()
        {
            //-6 -> header
            AddResultElem(new ResultElem(Interpreter.DefElemHandler.Get(-6), bt1.FirmwareNumber) { Value = "null" });
            //-3 -> Firmware
            AddResultElem(new ResultElem(Interpreter.DefElemHandler.Get(-3), "-03") { Value = bt1.FirmwareNumber });
            //-2 -> SerialNummer
            AddResultElem(new ResultElem(Interpreter.DefElemHandler.Get(-2), "") { Value = et3.SerialNr });
            AddResultElem(new ResultElem(Interpreter.DefElemHandler.Get(-10), "-10") { Value = bt22.SummationSourcesToString() });
            AddResultElem(new ResultElem(Interpreter.DefElemHandler.Get(-9), "-09") { Value = bt22.DemandSelectToString() });
        }

        public override bool Interpret(IInterpreter interpreter)
        {
            if(!base.Interpret(interpreter))
                return false;
            StandardListe.Clear();
            BaseInterpret();
            CurrentListe = StandardListe;
            AddStandardItems();
            return true;
        }

        public string ToVDEW(List<ResultElem> item)
        {
            string result = "";
           // item.Sort();
            for (int i = 0; i < item.Count; i++)
            {
                result += item[i].ToVDEW();
            }
            return result;
        }

        public virtual string ToVDEW()
        {
            return ToVDEW(StandardListe);
        }
    }
}
