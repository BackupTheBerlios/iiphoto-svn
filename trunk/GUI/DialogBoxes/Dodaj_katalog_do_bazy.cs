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
    public partial class Dodaj_katalog_do_bazy : Form
    {

        private string sciezka;

        public Dodaj_katalog_do_bazy(string s)
        {
            InitializeComponent();

            this.button1.Enabled = false;
            sciezka = s;

            Wypelnij();

            
        }

        private void Wypelnij()
        {
            List<string> lista= new List<string>();

            Db baza = new Db();

            baza.Polacz();

            try
            {
                DataSet dataSet = baza.Select("select nazwa from tag where album=1"); ;

                foreach (DataTable t in dataSet.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        lista.Add((string)r[0]);                        
                    }
                }      
                lista.Sort();

                foreach(string t in lista)
                {
                    this.comboBox1.Items.Add(t);
                }
            }
            catch (SqlException)
            {
            }
            baza.Rozlacz();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("SelectedIndexChanged");
            this.button1.Enabled = true;

        }
        private void dodaj_do_albumu()
        {
            string nazwa = (string)this.comboBox1.SelectedItem;
            Int64 id_tagu = -1;

            Db baza = new Db();

            baza.Polacz();

            try
            {
                DataSet dataSet = baza.Select("select id_tagu from Tag where album=1 and nazwa=\'"+nazwa+"\';"); 

                foreach (DataTable t in dataSet.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        id_tagu = (Int64)r[0];                        
                    }
                }

                dataSet = baza.Select("select id_zdjecia from Zdjecie where sciezka=\'" + sciezka + "\'"); ;

                foreach (DataTable t in dataSet.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        //tu maja byc fastinsert
                        baza.Insert("TagZdjecia", r[0] + "," + id_tagu);
                    }
                }

            }
            catch (SqlException)
            {
            }
            baza.Rozlacz();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dodaj_do_albumu();

            this.Dispose();
        }
    }
}