namespace ANSIC1219NESInterpretDemo
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.TestVerrechnungsdaten = new System.Windows.Forms.Button();
            this.TestDemand = new System.Windows.Forms.Button();
            this.TestSelbstablesung = new System.Windows.Forms.Button();
            this.TestLastgang = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TestVerrechnungsdaten
            // 
            this.TestVerrechnungsdaten.Location = new System.Drawing.Point(40, 12);
            this.TestVerrechnungsdaten.Name = "TestVerrechnungsdaten";
            this.TestVerrechnungsdaten.Size = new System.Drawing.Size(194, 44);
            this.TestVerrechnungsdaten.TabIndex = 3;
            this.TestVerrechnungsdaten.Text = "Test Verrechnungsdaten";
            this.TestVerrechnungsdaten.UseVisualStyleBackColor = true;
            this.TestVerrechnungsdaten.Click += new System.EventHandler(this.TestVerrechnungsdaten_Click);
            // 
            // TestDemand
            // 
            this.TestDemand.Location = new System.Drawing.Point(40, 62);
            this.TestDemand.Name = "TestDemand";
            this.TestDemand.Size = new System.Drawing.Size(194, 44);
            this.TestDemand.TabIndex = 4;
            this.TestDemand.Text = "Test Demand";
            this.TestDemand.UseVisualStyleBackColor = true;
            this.TestDemand.Click += new System.EventHandler(this.TestDemand_Click);
            // 
            // TestSelbstablesung
            // 
            this.TestSelbstablesung.Location = new System.Drawing.Point(40, 112);
            this.TestSelbstablesung.Name = "TestSelbstablesung";
            this.TestSelbstablesung.Size = new System.Drawing.Size(194, 44);
            this.TestSelbstablesung.TabIndex = 5;
            this.TestSelbstablesung.Text = "Test Selbstablesung";
            this.TestSelbstablesung.UseVisualStyleBackColor = true;
            this.TestSelbstablesung.Click += new System.EventHandler(this.TestSelbstablesung_Click);
            // 
            // TestLastgang
            // 
            this.TestLastgang.Location = new System.Drawing.Point(40, 162);
            this.TestLastgang.Name = "TestLastgang";
            this.TestLastgang.Size = new System.Drawing.Size(194, 44);
            this.TestLastgang.TabIndex = 6;
            this.TestLastgang.Text = "TestLastgang";
            this.TestLastgang.UseVisualStyleBackColor = true;
            this.TestLastgang.Click += new System.EventHandler(this.TestLastgang_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.TestLastgang);
            this.Controls.Add(this.TestSelbstablesung);
            this.Controls.Add(this.TestDemand);
            this.Controls.Add(this.TestVerrechnungsdaten);
            this.Name = "Form1";
            this.Text = "ANSIC1219NESInterpretDemo";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button TestVerrechnungsdaten;
        private System.Windows.Forms.Button TestDemand;
        private System.Windows.Forms.Button TestSelbstablesung;
        private System.Windows.Forms.Button TestLastgang;
    }
}

