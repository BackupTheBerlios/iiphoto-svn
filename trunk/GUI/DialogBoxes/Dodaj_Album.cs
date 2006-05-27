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
    public partial class Dodaj_Album : Form
    {
        private ListaAlbumowControl listaAlbumow;

        public Dodaj_Album(ListaAlbumowControl la)
        {
            InitializeComponent();
            listaAlbumow = la;
        }

        private void DodajAlbumDoBazy()
        {
            string nazwa;

            if (textBox1.Text != "")
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

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DodajAlbumDoBazy();            
        }

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