using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Photo
{
    public partial class WyszukiwarkaControl : UserControl, IWyszukiwacz
    {
        public WyszukiwarkaControl()
        {
            InitializeComponent();
        }

        public IWyszukiwacz wyszukiwacz_albumow;

        private void button1_Click(object sender, EventArgs e)
        {
            Wyszukiwanie wynik = new Wyszukiwanie();
            if (checkBox1.Checked)
            {
                wynik.And(wyszukiwacz_albumow.Wyszukaj());
            }
            wynik.And(Wyszukaj());
            if (ZakonczonoWyszukiwanie != null)
            {
                ZakonczonoWyszukiwanie(wynik.PodajWynik());
            }
        }

        #region Wyszukiwacz Members

        public event ZakonczonoWyszukiwanieDelegate ZakonczonoWyszukiwanie;
        public event RozpoczetoWyszukiwanieDelegate RozpoczetoWyszukiwanie;
        public event ZnalezionoZdjecieDelegate ZnalezionoZdjecie;

        public IWyszukiwanie Wyszukaj()
        {
            Wyszukiwanie wynik = new Wyszukiwanie();
            wynik.And("..\\..\\img\\wyszuk1.bmp");
            return wynik;
        }

        #endregion
    }
}
