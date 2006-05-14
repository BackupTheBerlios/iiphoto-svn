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
        public Dodaj_Album()
        {
            InitializeComponent();
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
                    baza.Insert_czesci("Tag", "nazwa,album", "\'" + nazwa + "\',1");

                }
                catch (SQLiteException)
                {
                    MessageBox.Show("Taka Nazwa Albumu juz instnieje");
                }

                baza.Rozlacz();

                this.Dispose();

                

                

                //ListaAlbumowControl.dodaj_a();
            }
        }
    }
}