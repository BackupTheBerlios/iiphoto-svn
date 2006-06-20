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
    /// Formatka wyswietlajaca informacje dla uzytkownika o uzytych oznaczeniach w programie
    /// </summary>
    public partial class OznaczeniaHelp : Form
    {
        /// <summary>
        /// Konstruktor bezparametryczny
        /// </summary>
        public OznaczeniaHelp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}