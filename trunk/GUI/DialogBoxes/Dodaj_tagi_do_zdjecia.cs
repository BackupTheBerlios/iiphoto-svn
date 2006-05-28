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
    public partial class Dodaj_tagi_do_zdjecia : Form
    {
        private PrzegladarkaZdjec przegladarka;
        //private Zdjecie zdjecie;
        private List<Zdjecie> lista_zdjec;
        private int opcja = 0;

        public event ZmienionoTagiDelegate ZmienionoTagi;

        public Dodaj_tagi_do_zdjecia(PrzegladarkaZdjec p)
        {
            InitializeComponent();
            przegladarka = p;
            //Wypelnij();
        }
        public Dodaj_tagi_do_zdjecia(Zdjecie z)
        {
            InitializeComponent();
            //zdjecie = z;
            //Wypelnij();
            //opcja = 1;
        }
        public Dodaj_tagi_do_zdjecia(List<Zdjecie> lis_z)
        {
            InitializeComponent();
            lista_zdjec = lis_z;
            if (lista_zdjec.Count == 1)
            {
                lista_zdjec[0].WypelnijListeTagow();
                Wypelnij(lista_zdjec[0]);
            }
            else
            {
                Wypelnij_dla_kilku();
            }
            //opcja = 2;
        }

        private void Wypelnij(Zdjecie zdjecie)
        {            
            Db baza = new Db();
            baza.Polacz();
            try
            {
                List<Int64> lista;
                bool czy_znaleziony = false;                

                DataSet ds = baza.Select("select id_tagu, nazwa from Tag where album = 0");

                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        if (!(r[0] is DBNull))
                        {
                            lista = zdjecie.ZwrocListeTagow;
                            //MessageBox.Show("lista.count: " + lista.Count);
                            foreach (Int64 tag in lista)
                            {                                
                                if (tag == (Int64)r[0])
                                {                                    
                                    this.checkedListBox1.Items.Add((string)r[1], true);
                                    czy_znaleziony = true;
                                    break;
                                }
                            }
                            if (czy_znaleziony == false)
                            {
                                this.checkedListBox1.Items.Add((string)r[1], false);
                            }
                            czy_znaleziony = false;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("blad bazy: " + ex.Message);
            }
            baza.Rozlacz();        
        }

        //wypelnia kontrolke tagami ale zadne nie jest zaznaczone bierz sie i ustawia sie tago dla wszystkich zaznaczonych zdjec
        private void Wypelnij_dla_kilku()
        {
            Db baza = new Db();
            baza.Polacz();
            try
            {   
                DataSet ds = baza.Select("select id_tagu, nazwa from Tag where album = 0");

                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        if (!(r[0] is DBNull))
                        {                            
                            this.checkedListBox1.Items.Add((string)r[1], false);                            
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("blad bazy: " + ex.Message);
            }
            baza.Rozlacz();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Db baza = new Db();
            baza.Polacz();
            List<Int64> listaTagowDoUstawienia;// = new List<Int64>();

            if (lista_zdjec.Count == 1)
                opcja = 1;
            
            try
            {
                foreach (Zdjecie zdjecie in lista_zdjec)
                {
                    if (zdjecie.CzyUstawioneId() == true)
                    {
                        //MessageBox.Show("dodaje tag");
                        if (opcja == 1)
                        {                            
                            //usuniecie wszystkich tagow zeby dodac nowe
                            baza.Delete("TagZdjecia", "id_zdjecia=" + zdjecie.Id + " and id_tagu in (select id_tagu from Tag where album = 0)");
                            listaTagowDoUstawienia = new List<Int64>();
                        }
                        else
                        {
                            /*zdjecie.WypelnijListeTagow();
                            if (zdjecie.CzyUstawioneId() == true)
                                MessageBox.Show("tak: "+zdjecie.Id);
                            else
                                MessageBox.Show("nie");
                             */
                            listaTagowDoUstawienia = zdjecie.Tagi;
                        }

                        foreach (string ob in this.checkedListBox1.CheckedItems)
                        {
                            DataSet ds = baza.Select("select id_tagu from Tag where nazwa=\'" + ob + "\' and album = 0");

                            foreach (DataTable t in ds.Tables)
                            {
                                foreach (DataRow r in t.Rows)
                                {
                                    if (!(r[0] is DBNull))
                                    {
                                        baza.Insert("TagZdjecia", zdjecie.Id + "," + r[0]);
                                        listaTagowDoUstawienia.Add((Int64)r[0]);
                                        //MessageBox.Show("TagZdjecia" + zdjecie.Id + "," + r[0]);
                                    }
                                }
                            }
                        }
                        zdjecie.Tagi = listaTagowDoUstawienia;
                        //zdjecie.WypelnijListeTagow();
                        //zdjecie.Tagi = listaTagowDoUstawienia;
                        //MessageBox.Show("zd.t.cou: " + zdjecie.Tagi.Count);
                    }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("blad sql " + ex.Message);
            }
            
            baza.Rozlacz();

            this.Dispose();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void checkedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }
    }
}