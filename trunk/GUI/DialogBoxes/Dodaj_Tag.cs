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
    public partial class Dodaj_Tag : Form
    {
        private ListaAlbumowControl listaAlbumow;

        public Dodaj_Tag(ListaAlbumowControl la)
        {
            InitializeComponent();
            listaAlbumow = la;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nazwa;

            if (textBox1.Text != "")
            {
                nazwa = textBox1.Text;

                Db baza = new Db();

                baza.Polacz();

                try
                {
                    baza.Insert_czesci("Tag", "nazwa,album", "\'" + nazwa + "\',0");

                }
                catch (SQLiteException)
                {
                    MessageBox.Show("Taka Nazwa Tag juz instnieje");
                }

                baza.Rozlacz();

                listaAlbumow.odswiez();

                this.Dispose();             
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Keys k = (Keys)e.KeyChar;
            if (k == Keys.Enter)
            {
                string nazwa;

                if (textBox1.Text != "")
                {
                    nazwa = textBox1.Text;

                    Db baza = new Db();

                    baza.Polacz();

                    try
                    {
                        baza.Insert_czesci("Tag", "nazwa,album", "\'" + nazwa + "\',0");

                    }
                    catch (SQLiteException)
                    {
                        MessageBox.Show("Taka Nazwa Tag juz instnieje");
                    }

                    baza.Rozlacz();

                    listaAlbumow.odswiez();

                    this.Dispose();
                }
            }
        }
    }
}