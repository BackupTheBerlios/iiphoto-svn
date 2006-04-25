using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Photo
{
    public partial class PhotoForm : Form
    {
        private Operacje operacje;

        public PhotoForm()
        {
            InitializeComponent();

            /* Inicjalizacja okna g��wnego.
             */
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.EnableNotifyMessage, true);

            /* Inicjalizacja kontrolki DrzewoKatalogow.
             */
            fileTree1.Fill();

            /* Inicjalizacja operacji graficznych.
             */
            operacje = new Operacje();
            operacje.WczytajWbudowane();
            operacje.WczytajPluginy();
            operacje.WrzucNaToolBar(toolStripOperacje);
            operacje.ZazadanieOperacji += new ZazadanieOperacjiDelegate(przegladarkaZdjec.DodajOperacje);

            /* Inicjalizacja wyszukiwarki.
             */
            wyszukiwarkaControl1.wyszukiwacz_albumow = listaAlbumowControl;

            /* Delegowanie RozpoczetoWyszukiwanieDelegate.
             */
            fileTree1.RozpoczetoWyszukiwanie +=
                new RozpoczetoWyszukiwanieDelegate(statusStrip.RozpoczetoWyszukiwanie);

            /* Delegowanie ZnalezionoZdjecieDelegate.
             */
            fileTree1.ZnalezionoZdjecie += 
                new ZnalezionoZdjecieDelegate(przegladarkaZdjec.Dodaj);

            /* Delegowanie ZakonczonoWyszukiwanieDelegate.
             */
            fileTree1.ZakonczonoWyszukiwanie +=
                new ZakonczonoWyszukiwanieDelegate(statusStrip.ZakonczonoWyszukiwanie);
            listaAlbumowControl.ZakonczonoWyszukiwanie +=
                new ZakonczonoWyszukiwanieDelegate(przegladarkaZdjec.Wypelnij);
            wyszukiwarkaControl1.ZakonczonoWyszukiwanie +=
                new ZakonczonoWyszukiwanieDelegate(przegladarkaZdjec.Wypelnij);
            //fileTree1.ZakonczonoWyszukiwanie +=
                //new ZakonczonoWyszukiwanieDelegate(przegladarkaZdjec.Wypelnij);

            /* Delegowanie WybranoZdjecieDelegate
             */
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            przegladarkaZdjec.SetThumbnailView();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            fileTree1.Delete();
            fileTree1.Fill();
            
        }
    }
}