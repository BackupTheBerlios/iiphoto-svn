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
        private string plikSzukany;
        public string plikOdnaleziony;
        private long id;

        public ZnajdzPliki(KeyValuePair<long, string> kv)
        {
            InitializeComponent();
            plikSzukany = kv.Value;
            id = kv.Key;
            Znajdz();
        }

        private void Znajdz()
        {
            textBox1.Text = plikSzukany;
            plikOdnaleziony = "lala";
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