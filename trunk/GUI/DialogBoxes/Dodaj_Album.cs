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
    /// <summary>
    /// Klasa tworz�ca formatka do dodawania albumu do drzewa album�w
    /// </summary>
    public partial class Dodaj_Album : Form
    {
        private ListaAlbumowControl listaAlbumow;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Dodaj_Album(ListaAlbumowControl la)
        {
            InitializeComponent();
            listaAlbumow = la;
        }

        /// <summary>
        /// Metoda dodaj�ca nazwe albumu do bazy
        /// </summary>
        private void DodajAlbumDoBazy()
        {
            string nazwa;

            if (textBox1.Text != "" && textBox1.Text.Length < 100)
            {
                nazwa = textBox1.Text;

                Db baza = new Db();

                baza.Polacz();

                try
                {
                    baza.Insert_czesci("Tag", "nazwa,album", "\'" + nazwa + "\',1");

                    listaAlbumow.lista_nazw_albumow.Add(nazwa);

                    DataSet ds = baza.Select("select max(id_tagu) from Tag where nazwa=\'" + nazwa + "\' and album=1");

                    foreach (DataTable t in ds.Tables)
                    {
                        foreach (DataRow r in t.Rows)
                        {
                            if (!(r[0] is DBNull))
                            {
                                listaAlbumow.lista_albumow.Add((Int64)r[0]);
                            }
                        }
                    }

                }
                catch (SQLiteException)
                {
                    MessageBox.Show("Taka Nazwa Albumu juz instnieje");
                }

                baza.Rozlacz();

                listaAlbumow.odswiez();

                this.Dispose();
            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("Wpisz nazwe albumu");                
            }
            else
            {
                MessageBox.Show("Za d�uga nazwa");
            }

        }

        /// <summary>
        /// Metoda wykonywana po klikni�ciu przycisku zatwierdzaj�cego("OK" lub nacisniecie "ENTER"). Uruchamia metody dodawania albumu
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            DodajAlbumDoBazy();            
        }

        /// <summary>
        /// Metoda wywo�uje metode DodajAlbumDoBazy gdy u�ytkownik naci�nie Enter. Uruchamia metody dodawania albumu
        /// </summary>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Keys k = (Keys)e.KeyChar;
            if (k == Keys.Enter)
            {
                DodajAlbumDoBazy();
            }
        }
    }
}