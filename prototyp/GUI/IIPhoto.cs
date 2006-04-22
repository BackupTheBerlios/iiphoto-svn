using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Photo
{
    public partial class IIPhoto : Form
    {
        private Operacje operacje;

        public IIPhoto()
        {
            InitializeComponent();

            fileTree1.Fill();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.EnableNotifyMessage, true);

            operacje = new Operacje();
            operacje.WczytajWbudowane();
            operacje.WczytajPluginy();
            operacje.WrzucNaToolBar(toolStripOperacje);
            operacje.ZazadanieOperacji += new ZazadanieOperacjiDelegate(przegladarkaZdjec.DodajOperacje);

            wyszukiwarkaControl1.wyszukiwacz_albumow = listaAlbumowControl;
            // zdarzenia wyszukiwania
            listaAlbumowControl.ZakonczonoWyszukiwanie +=
                new ZakonczonoWyszukiwanieDelegate(przegladarkaZdjec.Wypelnij);
            wyszukiwarkaControl1.ZakonczonoWyszukiwanie +=
                new ZakonczonoWyszukiwanieDelegate(przegladarkaZdjec.Wypelnij);
            fileTree1.ZakonczonoWyszukiwanie +=
                new ZakonczonoWyszukiwanieDelegate(przegladarkaZdjec.Wypelnij);
            // zaznaczane zdjecia
            //listaOpakowanControl.WybranoZdjecie +=
                //new WybranoZdjecieDelegate(informacjeControl.Zaladuj);
        }

        private void Alfa_Click(object sender, EventArgs e)
        {
            przegladarkaZdjec.RozpocznijEdycje();
        }

        private void Omega_Click(object sender, EventArgs e)
        {
            przegladarkaZdjec.ZakonczEdycje();
        }

        private void fileTree1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}