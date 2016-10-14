using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSIC1219NESInterpret.InterpretDef;


namespace ANSIC1219NESInterpret
{
    public class TVerrechnungsdatenHistorie : TVerrechnungsdaten
    {
        public List<List<ResultElem>> HistorieList { get; set; }

        public TVerrechnungsdatenHistorie(int index)
            : base(index)
        {
            HistorieList = new List<List<ResultElem>>();
        }

        public override string ToVDEW()
        {
            string result = "";
            result += base.ToVDEW(StandardListe);
            for (int i = 0; i < HistorieList.Count; i++)
            {
                result += base.ToVDEW(HistorieList[i]);
            }
            return result;
        }
    }
}
