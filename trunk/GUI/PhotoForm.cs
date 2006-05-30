using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SqlClient;

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

            drzewoOpakowane1.fileTree1.ZmienionoZrodlo +=
                new ZmienionoZrodloDelegate(statusStrip.ZmienionoZrodlo);

            listaAlbumowControl.ZmienionoZrodlo +=
                new ZmienionoZrodloDelegate(statusStrip.ZmienionoZrodlo);

            /* Delegowanie ZnalezionoZdjecieDelegate.
             */
            /*drzewoOpakowane1.fileTree1.ZnalezionoZdjecie += 
                new ZnalezionoZdjecieDelegate(przegladarkaZdjec.Dodaj);*/

            /* Delegowanie ZakonczonoWyszukiwanieDelegate.
             */
            drzewoOpakowane1.fileTree1.ZakonczonoWyszukiwanie +=
                new ZakonczonoWyszukiwanieDelegate(statusStrip.ZakonczonoWyszukiwanie);

            drzewoOpakowane1.fileTree1.ZakonczonoWyszukiwanie +=
                new ZakonczonoWyszukiwanieDelegate(przegladarkaZdjec.Wypelnij);

            /* Delegowanie RozpoczetoWyszukiwanieDelegate.
             */
            listaAlbumowControl.RozpoczetoWyszukiwanie +=            
                new RozpoczetoWyszukiwanieDelegate(statusStrip.RozpoczetoWyszukiwanie);

            /* Delegowanie ZnalezionoZdjecieDelegate.
             */
            listaAlbumowControl.ZakonczonoWyszukiwanie +=            
                new ZakonczonoWyszukiwanieDelegate(przegladarkaZdjec.Wypelnij);

            /* Delegowanie ZakonczonoWyszukiwanieDelegate.
             */
            listaAlbumowControl.ZakonczonoWyszukiwanie +=            
                new ZakonczonoWyszukiwanieDelegate(statusStrip.ZakonczonoWyszukiwanie);

            //listaAlbumowControl.ZakonczonoWyszukiwanie +=
              //  new ZakonczonoWyszukiwanieDelegate(przegladarkaZdjec.Wypelnij);
            wyszukiwarkaControl1.ZakonczonoWyszukiwanie +=
                new ZakonczonoWyszukiwanieDelegate(przegladarkaZdjec.Wypelnij);
            //fileTree1.ZakonczonoWyszukiwanie +=
                //new ZakonczonoWyszukiwanieDelegate(przegladarkaZdjec.Wypelnij);

            /* Delegowanie WybranoZdjecieDelegate
             */
            przegladarkaZdjec.WybranoZdjecie +=
                new WybranoZdjecieDelegate(informacjeControl1.Zaladuj);

            przegladarkaZdjec.WybranoKatalog +=
                new WybranoKatalogDelegate(drzewoOpakowane1.ZaladujZawartoscKatalogu);
            //listaOpakowanControl.WybranoZdjecie +=
            //new WybranoZdjecieDelegate(informacjeControl.Zaladuj);

            /* Delegowanie ZaznaczonoZdjecieDelegate
             */
            przegladarkaZdjec.ZaznaczonoZdjecie +=
                new ZaznaczonoZdjecieDelegate(informacjeControl1.Zaladuj);

            przegladarkaZdjec.WlaczonoWidokZdjecia +=
                new WlaczonoWidokZdjeciaDelegate(this.OnWlaczonoWidokZdjecia);

            przegladarkaZdjec.WylaczonoWidokZdjecia +=
                new WylaczonoWidokZdjeciaDelegate(this.OnWylaczonoWidokZdjecia);

            przegladarkaZdjec.ZabierzFocus +=
                new ZabierzFocusDelegate(drzewoOpakowane1.Focus);

            drzewoOpakowane1.ZabierzFocus +=
                new ZabierzFocusDelegate(przegladarkaZdjec.WezFocus);

            listaAlbumowControl.ZmienionoTagi +=
                new ZmieninoTagiDelegate(przegladarkaZdjec.Thumbnailview.DodajTagi);

            drzewoOpakowane1.fileTree1.ZmienionoIds +=
                new ZmienionoIdsDelegate(przegladarkaZdjec.Thumbnailview.ZresetujIds);

            drzewoOpakowane1.fileTree1.ZmienionoTagi +=
                new ZmienionoTagiDelegate(przegladarkaZdjec.Thumbnailview.ZresetujTagi);

            przegladarkaZdjec.Imageview.ZmodyfikowanoZdjecie +=
                new ZmodyfikowanoZdjecieDelegate(przegladarkaZdjec.Thumbnailview.ZmodyfikowanoZdjecie);
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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("-= O programie IIPhoto =-\nProgramiœci:\nDawid Jasiñski\nKonrad Garlikowski");
        }

        private void zakonczToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            przegladarkaZdjec.Thumbnailview.ZapiszWszystkiePliki();
        }

        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZapiszPlik();
        }

        private void ZapiszPlik()
        {
            if (przegladarkaZdjec.AktywneOpakowanie == przegladarkaZdjec.Imageview)
            {
                przegladarkaZdjec.Imageview.ZapiszPlik();
            }
            else if (przegladarkaZdjec.AktywneOpakowanie == przegladarkaZdjec.Thumbnailview)
            {
                przegladarkaZdjec.Thumbnailview.ZapiszPlik();
            }
        }



        private void OnWlaczonoWidokZdjecia()
        {
            toolStripComboBox2.Visible = false;
            toolStripComboBox1.Visible = true;
        }

        private void OnWylaczonoWidokZdjecia()
        {
            toolStripComboBox1.Visible = false;
            toolStripComboBox2.Visible = true;
        }

        private void skrótyKlawiaturoweToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SkrotyKlawiaturoweHelp skroty = new SkrotyKlawiaturoweHelp();
            skroty.Show();
        }

        //toolStripComboBox1
        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            double zoom = 1.0;
            switch (((ToolStripComboBox)sender).Text)
            {
                case "10%": zoom = 0.1; break;
                case "25%": zoom = 0.25; break;
                case "50%": zoom = 0.5; break;
                case "75%": zoom = 0.75; break;
                case "100%": zoom = 1.0; break;
                case "150%": zoom = 1.5; break;
                case "Do ekranu": przegladarkaZdjec.Imageview.UstawDopasowanieDoEkranu(); przegladarkaZdjec.Imageview.Focus(); return;
            }
            przegladarkaZdjec.Imageview.Zoom(zoom);
            przegladarkaZdjec.Imageview.Focus();
        }
        
        //toolStripComboBox2
        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int size;
            switch (((ToolStripComboBox)sender).Text)
            {
                case "Ma³e": size = 1; break;
                case "Œrednie": size = 2; break;
                case "Du¿e": size = 3; break;
                default: size = 2; break;
            }
            if (size != Config.RozmiarMiniatury)
            {
                Config.ZmienRozmiarMiniatury(size);
                przegladarkaZdjec.ZmienionoRozmiarMiniatury();
            }
            przegladarkaZdjec.Thumbnailview.Focus();
        }

        private void toolStripComboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void toolStripComboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void wycofajZmianyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            przegladarkaZdjec.Imageview.Zdjecie.DisposeDuze();
            przegladarkaZdjec.Imageview.Wypelnij(new IZdjecie[] { przegladarkaZdjec.Imageview.Zdjecie});
        }

        private void zapiszButton1_Click(object sender, EventArgs e)
        {
            if (przegladarkaZdjec.AktywneOpakowanie == przegladarkaZdjec.Imageview)
            {
                przegladarkaZdjec.Imageview.ZapiszPlik();
            }
            else if (przegladarkaZdjec.AktywneOpakowanie == przegladarkaZdjec.Thumbnailview)
            {
                przegladarkaZdjec.Thumbnailview.ZapiszPlik();
            }
        }

        private void aktualizacjaBazyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Db baza = new Db();

            baza.Polacz();

            try
            {
                DataSet ds;
                string pelna_sciezka;

                ds = baza.Select("select sciezka,nazwa_pliku from Zdjecie");

                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        if (!(r[0] is DBNull))
                        {
                            pelna_sciezka = r[0] + "\\" + r[1];

                            if (System.IO.File.Exists(pelna_sciezka) == true)
                            {
                                Zdjecie z = new Zdjecie(pelna_sciezka);
                                //tutaj musi byc wukorzystana funkcja do zczytania pol i do update bazy                                
                                z.ZweryfikujZdjecie();
                                if (z.CzyUstawioneId() == true)
                                {
                                    z.AktualizujBaze();
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("blad bazy: " + ex.Message);
            }
        }

        private void oznaczeniaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OznaczeniaHelp oznaczenia = new OznaczeniaHelp();
            oznaczenia.Show();
        }
    }
}