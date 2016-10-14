using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ANSIC1219NESInterpret;
using System.IO;
using ANSIC1219NESInterpret.InterpretDef;

namespace ANSIC1219NESInterpretDemo
{
    public partial class Form1 : Form
    {
        private string sourceIDDefinitionFile = @"SourceIDToObis\EchelonSourceIDS.xml";

        public Form1()
        {
            InitializeComponent();
        }

        private void TestVerrechnungsdaten_Click(object sender, EventArgs e)
        {
            Interpreter<DefElemHandler<DefElem>> i = new Interpreter<DefElemHandler<DefElem>>(sourceIDDefinitionFile);
            IVerrechnungsdaten resultTable = i.DoInterpret(File.ReadAllText(@"DemoDaten\Verrechnungsdaten.txt"));
            string erg = resultTable.ToVDEW();
            File.WriteAllText(@"DemoDaten\VerrechnungsdateResult.txt", erg);
        }

        private void TestDemand_Click(object sender, EventArgs e)
        {
            Interpreter<DefElemHandler<DefElem>> i = new Interpreter<DefElemHandler<DefElem>>(sourceIDDefinitionFile);
            string data = File.ReadAllText(@"DemoDaten\Demand.txt");
            IVerrechnungsdaten resultTable = i.DoInterpretDemandHistorie(data);
            string erg = resultTable.ToVDEW();
            File.WriteAllText(@"DemoDaten\DemandResult.txt", erg);
        }

        private void TestSelbstablesung_Click(object sender, EventArgs e)
        {
            Interpreter<DefElemHandler<DefElem>> i = new Interpreter<DefElemHandler<DefElem>>(sourceIDDefinitionFile);
            string data = File.ReadAllText(@"DemoDaten\Selbstabl.txt");
            IVerrechnungsdaten resultTable = i.DoInterpretSelfReadHistorie(data);
            string erg = resultTable.ToVDEW();
            File.WriteAllText(@"DemoDaten\SelbstablResult.txt", erg);
        }

        private void TestLastgang_Click(object sender, EventArgs e)
        {
            Interpreter<DefElemHandler<DefElem>> i = new Interpreter<DefElemHandler<DefElem>>(sourceIDDefinitionFile);
            string data = File.ReadAllText(@"DemoDaten\Lastgang.txt");
            TLastgang resultTable = i.DoInterpretLastgang(1, data);
            string erg = resultTable.ToResultString();
            File.WriteAllText(@"DemoDaten\LastgangResult.txt", erg);
        }

        
    }
}
