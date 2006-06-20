using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Photo
{
    /// <summary>
    /// Formatka wyswietlajaca informacje dla uzytkownika o uzytych skrotach klawiaturowych w programie
    /// </summary>
    public partial class SkrotyKlawiaturoweHelp : Form
    {
        /// <summary>
        /// Konstruktor bezparametryczny
        /// </summary>
        public SkrotyKlawiaturoweHelp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}