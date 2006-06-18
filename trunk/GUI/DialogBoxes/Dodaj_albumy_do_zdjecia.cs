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
    /// Klasa tworz�ca formatke dodaj�c� albumy do zdj��
    /// </summary>
    public partial class Dodaj_albumy_do_zdjecia : Form
    {                
        private List<Zdjecie> lista_zdjec;
        private int opcja = 0;//1 - dodawanie z katalogu  // 2 - dodawanie z miniaturek        
        private string sciezka;
        private FileTree drzewo;
        private PrzegladarkaZdjec przegladarka;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="z">zdjecie dla kt�rego ma formatka by� wywo�ana</param>
        public Dodaj_albumy_do_zdjecia(Zdjecie z)
        {
            InitializeComponent();
            //zdjecie = z;
            //Wypelnij();
            //opcja = 1;
        }
        /// <summary>
        /// Konstruktor wykorzystywaby gdy wywo�ujemy formatke dodawania z widoku miniatur
        /// </summary>
        /// <param name="lis_z">lista zdj�� dla kt�rych maj� by� dodane albumy</param>
        /// <param name="pr">obiekt przegl�darki zdj�� potrzebny do wywo�ania metod z tego obiektu</param>
        /// <param name="s">scie�ka zdj�cia/zdj�� dla kt�rego/kt�rych wykonuje si� dodawanie do albumu</param>
        public Dodaj_albumy_do_zdjecia(List<Zdjecie> lis_z, PrzegladarkaZdjec pr, string s)
        {
            InitializeComponent();
            lista_zdjec = lis_z;
            przegladarka = pr;
            if (lista_zdjec.Count == 1 && lista_zdjec[0].CzyUstawioneId() == true)
            {                
                Wypelnij(lista_zdjec[0]);
            }
            else
            {
                Wypelnij_dla_kilku();
            }

            sciezka = s.Substring(0, s.LastIndexOf("\\"));
            if (sciezka.Length == 2)
                sciezka += "\\";
            opcja = 2;
        }
        /// <summary>
        /// Konstruktor wykorzystywaby gdy wywo�ujemy formatke dodawania z drzewa katalog�w
        /// </summary>
        /// <param name="lis_z">lista zdj�� dla kt�rych maj� by� dodane albumy</param>
        /// <param name="tr">obiekt drzewa katalog�w potrzebny do wywo�ania metod z tego obiektu</param>
        /// <param name="s">scie�ka zdj�cia/zdj�� dla kt�rego/kt�rych wykonuje si� dodawanie do albumu</param>
        public Dodaj_albumy_do_zdjecia(List<Zdjecie> lis_z, FileTree tr, string s)
        {
            InitializeComponent();
            lista_zdjec = lis_z;
            drzewo = tr;
            if (lista_zdjec.Count == 1)
            {
                lista_zdjec[0].WypelnijListeTagow();
                Wypelnij(lista_zdjec[0]);
            }
            else
            {
                Wypelnij_dla_kilku();
            }
            sciezka = s;
            opcja = 1;
        }

        /// <summary>
        /// Metoda wype�niaj�ca formatke albumami ustawionymi dla zdj�cia
        /// </summary>
        /// <param name="zdjecie">zdj�cia dla kt�rego wype�niamy formatke</param>
        private void Wypelnij(Zdjecie zdjecie)
        {            
            Db baza = new Db();
            baza.Polacz();
            try
            {                
                bool czy_znaleziony = false;                

                DataSet ds = baza.Select("select id_tagu, nazwa from Tag where album = 1");
                DataSet zczytanie_z_bazy_almumow;

                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        if (!(r[0] is DBNull))
                        {                            
                            zczytanie_z_bazy_almumow = baza.Select("select id_tagu from TagZdjecia where id_zdjecia=" + zdjecie.Id + " and id_tagu in (select id_tagu from Tag where album=1)");

                            foreach (DataTable t2 in zczytanie_z_bazy_almumow.Tables)
                            {
                                foreach (DataRow r2 in t2.Rows)
                                {
                                    if (!(r2[0] is DBNull))
                                    {                                        
                                        if ((Int64)r2[0] == (Int64)r[0])
                                        {                                            
                                            this.checkedListBox1.Items.Add((string)r[1], true);
                                            czy_znaleziony = true;
                                            break;
                                        }
                                    }
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
        
        /// <summary>
        /// Metoda wype�niaj�ca formatke dla kilku zdj�� nie jeste�my w stanie w jednej formatce wy�wietli� album�w dla ka�dego zdj�cia wiec wy�wietla sie liste album� lecz �aden z nich nie jest zaznaczony gdy u�ytkownik zaznaczy albumy zostana one dodane do ka�dego zdj�cia z listy zdj��
        /// </summary>
        private void Wypelnij_dla_kilku()
        {
            Db baza = new Db();
            baza.Polacz();
            try
            {   
                DataSet ds = baza.Select("select id_tagu, nazwa from Tag where album = 1");

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

        /// <summary>
        /// Metoda dodaj�ca albumy do zdj�� i dodaj�ca odpowiednie wpisy do bazy
        /// </summary>
        /// <param name="list">lista zdj�� dla kt�rych metoda ma by� wywo�ana</param>
        private void DodajDoAlbumu(List<Zdjecie> list)
        {
            Db baza = new Db();
            baza.Polacz();

            try
            {
                foreach (Zdjecie zdjecie in lista_zdjec)
                {
                    if (zdjecie.CzyUstawioneId() == true)
                    {                        
                        if (lista_zdjec.Count == 1)
                        {                            
                            baza.Delete("TagZdjecia", "id_zdjecia=" + zdjecie.Id + " and id_tagu in (select id_tagu from Tag where album = 1)");                         
                        }

                        foreach (string ob in this.checkedListBox1.CheckedItems)
                        {
                            DataSet ds = baza.Select("select id_tagu from Tag where nazwa=\'" + ob + "\' and album = 1");

                            foreach (DataTable t in ds.Tables)
                            {
                                foreach (DataRow r in t.Rows)
                                {
                                    if (!(r[0] is DBNull))
                                    {
                                        baza.Insert("TagZdjecia", zdjecie.Id + "," + r[0]);                                       
                                    }
                                }
                            }
                        }                       
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("blad sql " + ex.Message);
            }

            baza.Rozlacz();            
        }

        /// <summary>
        /// Metoda wywa�ana gdy u�ytkownik kliknie przycisk Zatwierd�. Uruchamia metody dodawania do albumu
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            if (opcja == 1)
            {
                drzewo.d_d_a(sciezka);
                DodajDoAlbumu(lista_zdjec);
            }
            else if (opcja == 2)
            {
                przegladarka.dodaj_kolekcje_do_bazy(lista_zdjec);
                DodajDoAlbumu(lista_zdjec);
            }            
            
            this.Dispose();
        }
    }
}