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
    /// Klasa tworz¹ca formatke do dodawania tagu do drzewa albumów i do bazy
    /// </summary>
    public partial class Dodaj_Tag : Form
    {
        private ListaAlbumowControl listaAlbumow;

        /// <summary>
        /// Konstruktor 
        /// </summary>
        /// <param name="la">obiekt listy albumów ¿eby wykorzystaæ niektóre jego metody</param>
        public Dodaj_Tag(ListaAlbumowControl la)
        {
            InitializeComponent();
            listaAlbumow = la;
        }

        /// <summary>
        /// Metoda dodaj¹ca nazwe tagu do bazy i do drzewa albumów i aktualizuj¹ca drzewko 
        /// </summary>
        private void DodajTagDoBazy()
        {
            string nazwa;

            if (textBox1.Text != "" && textBox1.Text.Length < 100)
            {
                nazwa = textBox1.Text;

                Db baza = new Db();

                baza.Polacz();

                try
                {
                    baza.Insert_czesci("Tag", "nazwa,album", "\'" + nazwa + "\',0");
                    listaAlbumow.lista_nazw_tagow.Add(nazwa);

                    DataSet ds = baza.Select("select max(id_tagu) from Tag where nazwa=\'" + nazwa + "\' and album=0");

                    foreach (DataTable t in ds.Tables)
                    {
                        foreach (DataRow r in t.Rows)
                        {
                            if (!(r[0] is DBNull))
                            {
                                listaAlbumow.lista_tagow.Add((Int64)r[0]);
                                //MessageBox.Show("nazwa: " + nazwa + " id_tagu: " + r[0]);
                            }
                        }
                    }
                }
                catch (SQLiteException)
                {
                    MessageBox.Show("Taka Nazwa Tag juz instnieje");
                }

                baza.Rozlacz();

                listaAlbumow.odswiez();

                this.Dispose();
            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("Wpisz nazwe tagu"); 
            }
            else
            {
                MessageBox.Show("Za d³uga nazwa");                
            }

        }

        /// <summary>
        /// Metoda wywo³ana gdy u¿ytkownik naciœnie przycisk akceptuj¹cy i wywo³uje metode DodajTagDoBazy
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            DodajTagDoBazy();
        }

        /// <summary>
        /// Metoda wywo³ana gdy u¿ytkownik naciœnie klawisz Enter i wywo³uje metode DodajTagDoBazy
        /// </summary>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Keys k = (Keys)e.KeyChar;
            if (k == Keys.Enter)
            {
                DodajTagDoBazy();
            }
        }
    }
}