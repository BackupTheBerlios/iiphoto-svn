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
        private FileTree drzewo;
        private PrzegladarkaZdjec przegladarka;
        
        private int opcja = 0; //1 - dodawanie z katalogu  // 2 - dodawanie z miniaturek

        public Dodaj_katalog_do_bazy(string s, FileTree tr)
        {
            InitializeComponent();

            drzewo = tr;            

            this.button1.Enabled = false;
            sciezka = s;

            Wypelnij();
            opcja = 1;            
        }

        public Dodaj_katalog_do_bazy(string s, PrzegladarkaZdjec pr)
        {
            InitializeComponent();
            przegladarka = pr;

            this.button1.Enabled = false;
            //sciezka = s;
            sciezka = s.Substring(0, s.LastIndexOf("\\"));
            if (sciezka.Length == 2)
                sciezka += "\\";

            Wypelnij();
            opcja = 2;

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
            catch (SqlException e)
            {
                MessageBox.Show(e.ToString() + e.Message);
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
            catch (SqlException e)
            {
                MessageBox.Show(e.ToString() + e.Message);
            }
            baza.Rozlacz();

        }

        private void dodaj_do_albumu(List<string> lista)
        {
            string nazwa = (string)this.comboBox1.SelectedItem;
            Int64 id_tagu = -1;
            string nazwa_pliku;

            Db baza = new Db();

            baza.Polacz();

            try
            {
                DataSet dataSet = baza.Select("select id_tagu from Tag where album=1 and nazwa=\'" + nazwa + "\';");

                foreach (DataTable t in dataSet.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        id_tagu = (Int64)r[0];
                    }
                }

                foreach (string n in lista)
                {

                    nazwa_pliku = n.Substring(n.LastIndexOf("\\") + 1, n.Length - n.LastIndexOf("\\") - 1);

                    dataSet = baza.Select("select max(id_zdjecia) from Zdjecie where sciezka=\'" + sciezka + "\' and nazwa_pliku=\'"+nazwa_pliku+"\'"); ;
                    //int i = 0;

                    foreach (DataTable t in dataSet.Tables)
                    {
                        foreach (DataRow r in t.Rows)
                        {
                            //tu maja byc fastinsert
                            baza.Insert("TagZdjecia", r[0] + "," + id_tagu);
                            //MessageBox.Show("i: "+i);
                            //i++;
                        }
                    }
                }

            }
            catch (SqlException e)
            {
                MessageBox.Show(e.ToString() + e.Message);
            }
            baza.Rozlacz();

        }

        private void button1_Click(object sender, EventArgs e)
        {                
            if (opcja == 1)
            {                
                drzewo.d_d_a(sciezka);

                dodaj_do_albumu();
            }
            else if (opcja == 2)
            {
                List<string> lista = przegladarka.dodaj_kolekcje_do_bazy();

                if(lista.Count != 0)
                {                    
                    dodaj_do_albumu(lista);
                }                
            }

            this.Dispose();
        }
    }
}