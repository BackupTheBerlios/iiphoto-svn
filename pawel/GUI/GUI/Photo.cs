using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Photo
{
    public partial class Photo : Form
    {
        private Operacje operacje;

        public Photo()
        {
            InitializeComponent();

            operacje = new Operacje();
            operacje.WczytajWbudowane();
            operacje.WczytajPluginy();
            operacje.WrzucNaToolBar(toolStripOperacje);
            operacje.ZazadanieOperacji += new ZazadanieOperacjiDelegate(listaOpakowanControl.DodajOperacje);

            wyszukiwarkaControl1.wyszukiwacz_albumow = listaAlbumowControl;
            // zdarzenia wyszukiwania
            listaAlbumowControl.ZakonczonoWyszukiwanie +=
                new ZakonczonoWyszukiwanieDelegate(listaOpakowanControl.WynikWyszukiwania);
            wyszukiwarkaControl1.ZakonczonoWyszukiwanie +=
                new ZakonczonoWyszukiwanieDelegate(listaOpakowanControl.WynikWyszukiwania);
            // zaznaczane zdjecia
            listaOpakowanControl.WybranoZdjecie +=
                new WybranoZdjecieDelegate(informacjeControl.Zaladuj);
        }

        private void Alfa_Click(object sender, EventArgs e)
        {
            listaOpakowanControl.RozpocznijEdycje();
        }

        private void Omega_Click(object sender, EventArgs e)
        {
            listaOpakowanControl.ZakonczEdycje();
        }
    }
}