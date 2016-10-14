using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSIC1219NESInterpret
{

    public class ChannelIten
    {
        public byte ExtendedStatus { get; set; }
        public byte SourceID { get; set; }
        public UInt32 Value { get; set; }
    };

    public class LastgangResultItem
    {
        public bool SimpleStatus { get; set; }
        public byte ExtendedAllChannelStatus { get;set;}
        public DateTime Timestamp { get; set; }
        public List<ChannelIten> ChannelList { get; set; }

        public LastgangResultItem()
        {
            ChannelList = new List<ChannelIten>();
        }

    }

    public abstract class TLastgang: BTable
    {
        protected BT61 bt61 { get;set;}
        protected BT63 bt63 { get; set; }
        protected ET42 et42 { get; set; }
        protected abstract int Nr { get; }
        protected DataSetConfigItem CurrentBT61Config { get; set; }
        protected BlockInfoItem CurrentBlockInfoItem { get; set; }
        protected ET42ProfileInformation CurrentProfileInfoItem { get; set; }

        public List<LastgangResultItem> ResultListe { get; set; }

        public TLastgang(int lastgangNr)
            :base(64+lastgangNr)
        {
            ResultListe = new List<LastgangResultItem>();
        }

        protected byte[] CurrentSimpleStatusBuffer = null;

        protected bool CurrentSimpleStatusBufferBitSet(int bitIndex)
        {
            try
            {
                int bufferIndex = bitIndex / 8;
                int byteIndex = bitIndex % 8;
                byte b = CurrentSimpleStatusBuffer[bufferIndex];
                byte checkByte = (byte)(0x01 << byteIndex); // (byte)(0x80 >> byteIndex);
                if ((b & checkByte) == checkByte)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected byte[] CurrentExtendedStatusBuffer = null;

        protected byte GetNibbleFromExtendedStatus(int index)
        {
            try
            {
                int bufferIndex = index / 2;
                int nibbleIndex = index % 2;
                byte b = CurrentExtendedStatusBuffer[bufferIndex];
                //0->height
                if (nibbleIndex == 0)
                {
                    b = (byte)(b << 4 >> 4);
                }
                else
                {
                    b = (byte)(b >> 4);
                }
                return b;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public override bool Interpret(IInterpreter interpreter)
        {
            if (!base.Interpret(interpreter))
                return false;
            try
            {
                bt63 = Interpreter.Get(63) as BT63;
                if (bt63 == null)
                    throw new Exception("TLastgang: BT63 nicht vorhanden");
                bt63.Interpret(Interpreter);

                bt61 = Interpreter.Get(61) as BT61;
                if (bt61 == null)
                    throw new Exception("TLastgang: BT61 nicht vorhanden");
                bt61.Interpret(Interpreter);

                et42=Interpreter.Get(42 + ET.ETOffset) as ET42;
                if (et42 == null)
                    throw new Exception("TLastgang: ET42 nicht vorhanden");
                et42.Interpret(Interpreter);

                CurrentBlockInfoItem = bt63.BlockInfoListe[Nr];
                CurrentBT61Config = bt61.ConfigListe[Nr];
                CurrentProfileInfoItem=et42.ProfileInformationList[Nr];

                //erster check
                if (RohdatenListe.Count != CurrentBlockInfoItem.NumberValidBlocks)
                {
                    throw new Exception("RohdatenListe.Count != CurrentBlockInfoItem.NumberValidBlocks");
                }

                UInt16 BlockDataSize = CurrentBT61Config.Nr7; //das ist die maximale Anzahl bt63.NumberValidBlocks ist die aktuelle Anzahl !!
                UInt16 RecordItemSize = 5; //STIME_Date

                if (bt61.EndReadingSupported)
                {
                    RecordItemSize += (UInt16)(CurrentBT61Config.Nr11 * 4);
                }

                RecordItemSize += (UInt16)((CurrentBT61Config.Nr9 + 7) / 8);

                UInt16 IntervalsItemSize = (UInt16)(((CurrentBT61Config.Nr11 / 2) + 1) + 4 * CurrentBT61Config.Nr11);
                UInt16 IntervallSize = (UInt16)(CurrentBT61Config.Nr9 * IntervalsItemSize);
                RecordItemSize += IntervallSize; //so gross muss jeder Block sein

                //zweiter check
                for (int i = 0; i < RohdatenListe.Count; i++)
                {
                    if (RohdatenListe[i].Length != RecordItemSize)
                    {
                        throw new Exception("RohdatenListe[i].Length != RecordItemSize bei Index:" + i.ToString() );
                    }
                }

                //Auswerten
                for (int i = 0; i < RohdatenListe.Count; i++)
                {
                    int offset = 0;
                    //EndTime
                    STIME_DATE td = new STIME_DATE(RohdatenListe[i], offset); offset += 5;
                    DateTime blockEndTime = new DateTime(td.Year, td.Month, td.Day, td.Hour, td.Minute, 0);
                    Console.WriteLine(td.ToString());
                    //EndReadings: Snapshot of each channel taken at the end of the block
                    if (bt61.EndReadingSupported)
                    {
                        offset += (UInt16)(CurrentBT61Config.Nr11 * 4);
                    }
                    //simple status for this block
                    int simpleStatusLen = ((CurrentBT61Config.Nr9 + 7) / 8);
                    CurrentSimpleStatusBuffer = new byte[simpleStatusLen];
                    Array.Copy(RohdatenListe[i], offset, CurrentSimpleStatusBuffer, 0, simpleStatusLen);
                    offset += simpleStatusLen; // ((CurrentBT61Config.Nr9 + 7) / 8);
                    //Intervalls Nr9->NumberOfIntervals
                    bool simpleStatus = true;
                    for (int j = 0; j < CurrentBT61Config.Nr9; j++)
                    {
                        DateTime currentTime = blockEndTime.AddMinutes((j - CurrentBT61Config.Nr9 + 1) * CurrentProfileInfoItem.IntervalDuration);
                        LastgangResultItem lri = new LastgangResultItem() { Timestamp = currentTime };
                        ResultListe.Add(lri);

                        if (j == CurrentBT61Config.Nr9 - 1)
                        {
                            if (currentTime != blockEndTime)
                            {
                                throw new Exception("da passt was mit den Intervallen nicht");
                            }
                        }
                        simpleStatus = true;
                        lri.SimpleStatus = CurrentSimpleStatusBufferBitSet(j);
                        if (!CurrentSimpleStatusBufferBitSet(j))
                        {
                            //ab hier können eigentlich keine gültigen Einträge mehr kommen
                            simpleStatus = false;
                            //continue;
                        }
                        if (!simpleStatus)
                        {
                            //throw new Exception("das macht keinen Sinn");
                        }
                        //Extended Status
                        //Nr11->NumberOfChannels
                        int extendedStatusLen = ((CurrentBT61Config.Nr11 / 2) + 1);
                        CurrentExtendedStatusBuffer = new byte[extendedStatusLen];
                        Array.Copy(RohdatenListe[i], offset, CurrentExtendedStatusBuffer, 0, extendedStatusLen);
                        offset += extendedStatusLen; // ((CurrentBT61Config.Nr11 / 2) + 1);
                        byte extendedAllChannelStatus = GetNibbleFromExtendedStatus(0);
                        lri.ExtendedAllChannelStatus = extendedAllChannelStatus;
                        //array of 4-byte Records, Nr11->NumberOfChannels
                        for (int v = 0; v < CurrentBT61Config.Nr11; v++)
                        {
                            byte extendedStatus = GetNibbleFromExtendedStatus(v + 1);
                            UInt32 hValue = BitConverter.ToUInt32(RohdatenListe[i], offset); offset += 4;
                            ChannelIten ci = new ChannelIten()
                            {
                                ExtendedStatus = extendedStatus,
                                SourceID = CurrentProfileInfoItem.LoadProfileExtendedSources[v],
                                Value = hValue
                            };
                            lri.ChannelList.Add(ci);
                        }
                    }
                    //End of Intervall
                    if (offset < RohdatenListe[i].Count())
                    {
                        offset += (CurrentBT61Config.Nr11 / 2) + 1;
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ToResultString()
        {
            string erg = "Timestamp;SimpleStatus;ExtendedAllChannelStatus;ChannelNumber;ExtendedStatus;SourceID;Value\r\n";
            for (int i = 0; i < ResultListe.Count; i++)
            {
                string row = ResultListe[i].Timestamp.ToString() + ";" + ResultListe[i].SimpleStatus + ";" + ResultListe[i].ExtendedAllChannelStatus + ";" + ResultListe[i].ChannelList.Count.ToString();
                for(int j=0;j<ResultListe[i].ChannelList.Count;j++)
                {
                    row+=";" + ResultListe[i].ChannelList[j].ExtendedStatus + ";" + ResultListe[i].ChannelList[j].SourceID + ";" + ResultListe[i].ChannelList[j].Value.ToString();
                }
                erg += row + "\r\n";
            }
            return erg;
        }
    }
}
