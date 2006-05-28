using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Photo
{
    public partial class ZnajdzPliki : Form
    {
        public string plik;

        //public MyDialogResult mdr;

        public ZnajdzPliki(string s)
        {
            InitializeComponent();
            plik = s;
            Znajdz();
        }

        private void Znajdz()
        {
            textBox1.Text = plik;
            plik = "lala";
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Pomin_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PominAll_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}