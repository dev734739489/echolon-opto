using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace ANSIC1219NESInterpret.InterpretDef
{
    public class DefElemHandler<DefElemClass> : IDefElemHandler,IDisposable
        where DefElemClass:IDefElem
    {
        public List<DefElemClass> DefListe { get; set; }
        protected Dictionary<int, DefElemClass> DefDictionary { get; set; }

        public DefElemHandler()
        {
            DefListe = new List<DefElemClass>();
            DefDictionary = new Dictionary<int, DefElemClass>();
        }

        public void Add(DefElemClass elem)
        {
            DefListe.Add(elem);
            DefDictionary.Add(elem.SourceID, elem);
        }

        public IDefElem Get(int sourceID)
        {
            if (!DefDictionary.ContainsKey(sourceID))
                return null;
            return DefDictionary[sourceID];
        }

        public void ReadDef(string datei)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(DefListe.GetType());
                {
                    using (FileStream fs = new FileStream(datei, FileMode.Open))
                    {
                        using (TextReader reader = new StreamReader(fs))
                        {
                            //
                            DefListe = Serializer.Deserialize(reader) as List<DefElemClass>;
                            DefDictionary.Clear();
                            foreach (var elem in DefListe)
                            {
                                DefDictionary.Add(elem.SourceID, elem);
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string h = ex.Message;
                throw ex;
            }
        }

        public void SaveDef(string datei)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(DefListe.GetType());

                using (StreamWriter sw = File.CreateText(datei))
                {
                    Serializer.Serialize(sw, DefListe);
                    sw.Flush();
                    sw.Close();
                }

            }
            catch (Exception ex)
            {
                string h = ex.Message;
                throw ex;
            }
        }

        public void Dispose()
        {
            if (DefListe != null)
            {
                DefListe.Clear();
            }
            DefListe = null;
            if (DefDictionary != null)
            {
                DefDictionary.Clear();
            }
            DefDictionary = null;
        }
    }
}
