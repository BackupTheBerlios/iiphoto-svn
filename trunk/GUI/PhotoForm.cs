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

            /* Inicjalizacja okna g³ównego.
             */
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.EnableNotifyMessage, true);

            /* Inicjalizacja kontrolki DrzewoKatalogow.
             */

            drzewoOpakowane1.fileTree1.Fill();

            //fileTree1.Fill();

            /* Inicjalizacja operacji graficznych.
             */
            operacje = new Operacje();
            operacje.WczytajWbudowane();
            operacje.WczytajPluginy();
            operacje.WrzucDoGui(toolStripOperacje, filtryToolStripMenuItem);
            operacje.ZazadanieOperacji += new ZazadanieOperacjiDelegate(przegladarkaZdjec.DodajOperacje);

            /* Inicjalizacja wyszukiwarki.
             */
            wyszukiwarkaControl1.wyszukiwacz_albumow = listaAlbumowControl;

            /* Delegowanie RozpoczetoWyszukiwanieDelegate.
             */
            drzewoOpakowane1.fileTree1.RozpoczetoWyszukiwanie +=
                new RozpoczetoWyszukiwanieDelegate(statusStrip.RozpoczetoWyszukiwanie);

            /* Delegowanie ZnalezionoZdjecieDelegate.
             */
            drzewoOpakowane1.fileTree1.ZnalezionoZdjecie += 
                new ZnalezionoZdjecieDelegate(przegladarkaZdjec.Dodaj);

            /* Delegowanie ZakonczonoWyszukiwanieDelegate.
             */
            drzewoOpakowane1.fileTree1.ZakonczonoWyszukiwanie +=
                new ZakonczonoWyszukiwanieDelegate(statusStrip.ZakonczonoWyszukiwanie);
            listaAlbumowControl.ZakonczonoWyszukiwanie +=
                new ZakonczonoWyszukiwanieDelegate(przegladarkaZdjec.Wypelnij);
            wyszukiwarkaControl1.ZakonczonoWyszukiwanie +=
                new ZakonczonoWyszukiwanieDelegate(przegladarkaZdjec.Wypelnij);
            //fileTree1.ZakonczonoWyszukiwanie +=
                //new ZakonczonoWyszukiwanieDelegate(przegladarkaZdjec.Wypelnij);

            /* Delegowanie WybranoZdjecieDelegate
             */
            przegladarkaZdjec.WybranoZdjecie +=
                new WybranoZdjecieDelegate(informacjeControl1.Zaladuj);
             /*+=
    new WybranoZdjecieDelegate(informacjeControl1.Zaladuj);*/
            //listaOpakowanControl.WybranoZdjecie +=
            //new WybranoZdjecieDelegate(informacjeControl.Zaladuj);

            /* Delegowanie ZaznaczonoZdjecieDelegate
             */
            przegladarkaZdjec.ZaznaczonoZdjecie +=
                new ZaznaczonoZdjecieDelegate(informacjeControl1.Zaladuj);

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

            //fileTree1.Delete();
            //fileTree1.Fill();
            
        }

        private void przegladarkaZdjec_Resize(object sender, EventArgs e)
        {
            //drzewoOpakowane1.fileTree1.Refresh();// = false;
        }

        private void splitContainer2_Panel1_Resize(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("About IIPhoto");
        }

        private void zakonczToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}