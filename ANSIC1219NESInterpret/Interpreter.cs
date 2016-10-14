using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;
using ANSIC1219NESInterpret.InterpretDef;

namespace ANSIC1219NESInterpret
{
    public class Interpreter<DefElemHandlerClass>:IInterpreter2<DefElemHandlerClass>
        where DefElemHandlerClass:IDefElemHandler,new()
    {
        public Dictionary<int, ITable> Tables { get; set; }

        public Interpreter(string sourceIDDefinitionFile)
        {
            DefElemHandler1 = new DefElemHandlerClass();
            string fileName = sourceIDDefinitionFile;  
            if (!File.Exists(sourceIDDefinitionFile))
            {
                throw new Exception(sourceIDDefinitionFile + " not found");
            
            }
            DefElemHandler1.ReadDef(fileName);
            Tables = new Dictionary<int, ITable>();
            Add(new BT1());
            //das ist new Macke
            Tables.Add(3,Tables[1]);
            //Add(new BT3());
            Add(new BT20());
            Add(new BT21());
            Add(new BT22());
            Add(new BT23());
            Add(new BT26());
            Add(new BT61());
            Add(new BT62());
            Add(new BT63());
            //Lastgänge
            Add(new BT64());
            Add(new BT65());
            Add(new BT66());
            Add(new BT67());
          
            Add(new ET3());
            Add(new ET21());
            Add(new ET36());
            Add(new ET41());
            Add(new ET42());
            Add(new ET66());
        }

        protected void Add(ITable table)
        {
            Tables.Add(table.Index,table);
        }


        public ITable Get(int index)
        {
            if (!Tables.ContainsKey(index))
                return null;
            return Tables[index];
        }

        private void ReadTables(JArray tableArray,int indexOffset/* für die ET.. -> 0x80 sonst 00*/)
        {
            foreach (var elem in tableArray)
            {
                int index = elem.Value<int>("TableIndex");
                index += indexOffset;
                if (!Tables.ContainsKey(index))
                {
                    throw new Exception("no Definition for Table:" + index.ToString());
                    //continue;
                }
                string value = elem.Value<string>("TableData");
                if (!string.IsNullOrEmpty(value))
                {
                    byte[] hData = Array.ConvertAll<string, byte>(value.Split('-'), s => Convert.ToByte(s, 16));
                    Tables[index].Rohdaten = hData;
                }
                //array
                JArray rohdatenArray = elem.SelectToken("TableDataArray") as JArray;
                if (rohdatenArray != null && rohdatenArray.Count > 0)
                {
                    {
                        foreach (var item in rohdatenArray)
                        {
                            value = item.Value<string>();
                            byte[] hData = Array.ConvertAll<string, byte>(value.Split('-'), s => Convert.ToByte(s, 16));
                            Tables[index].RohdatenListe.Add(hData);
                        }
                    }
                }

            }
        }

        public IDefElemHandler DefElemHandler {
            get { return DefElemHandler1; }
        }

        public DefElemHandlerClass DefElemHandler1 { get; set; }
        

        public JObject ParseJsonString(string data)
        {
            JObject result = JObject.Parse(data);
            ReadTables(result.SelectToken("BasicTables") as JArray, 0);
            ReadTables(result.SelectToken("ExtendedTables") as JArray, ET.ETOffset);
            return result;
        }

        public IVerrechnungsdaten DoInterpret(string data)
        {
            return DoInterpretVerrechnungsdaten(data);
        }

        public IVerrechnungsdaten DoInterpretVerrechnungsdaten(string data)
        {
            try
            {
                JObject result = ParseJsonString(data);
                Tables[23].Interpret(this);
                return Tables[23] as IVerrechnungsdaten;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IVerrechnungsdaten DoInterpretDemandHistorie(string data)
        {
            try
            {
                JObject result = ParseJsonString(data);
                Tables[41 + ET.ETOffset].Interpret(this);
                return Tables[41 + ET.ETOffset] as IVerrechnungsdaten;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IVerrechnungsdaten DoInterpretSelfReadHistorie(string data)
        {
            try
            {
                JObject result = ParseJsonString(data);
                Tables[26].Interpret(this);
                return Tables[26] as IVerrechnungsdaten;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TLastgang DoInterpretLastgang(int nr, string data)
        {
            try
            {
                JObject result = ParseJsonString(data);
                Tables[64+nr-1].Interpret(this);
                return Tables[64 + nr - 1] as TLastgang;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
