using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace Photo
{
    /// <summary>
    /// Klasa imlementuj�ca UserControl i interjfejs IWyszukiwacz odpowiada za sworzenie kontrolki do wyszukiwanie i za realizacje wyszukania odpowiednich zdj�� w bazie
    /// </summary>
    public partial class WyszukiwarkaControl : UserControl, IWyszukiwacz    
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public WyszukiwarkaControl()
        {
            InitializeComponent();
        }

        public IWyszukiwacz wyszukiwacz_albumow;

        /// <summary>
        /// Metoda realizuj�ca zapytanie sql do bazy i zwracaj�ca liste zdj�c kt�ra zosta�a sworzona w oparciu o wynik zapytania
        /// </summary>
        /// <returns>lista zdj�� kt�ra spe�nia warunki wyszukania</returns>
        private List<Zdjecie> ZwrocWyszukanie()
        {
            Db baza = new Db();
            baza.Polacz();
            List<Zdjecie> lista_zdjec = new List<Zdjecie>();

            try
            {
                string pelna_sciezka;
                DataSet ds;

                /*ds = baza.Select("select sciezka,nazwa_pliku from Zdjecie");

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
                                z.ZweryfikujZdjecie();
                            }
                        }
                    }
                }*/


                string sql = "select sciezka,nazwa_pliku from Zdjecie where ";                                

                foreach (string s in checkedListBox1.CheckedItems)
                {
                    switch (s)
                    {
                        case "po nazwie":
                            sql += " nazwa_pliku like \'%" + textBox1.Text + "%\' or nazwa_pliku like \'" + textBox1.Text + "%\' or nazwa_pliku like \'" + textBox1.Text + "\' or ";
                            break;
                        case "po komentarzu":
                            sql += " komentarz like \'%" + textBox1.Text + "%\' or komentarz like \'" + textBox1.Text + "%\' or komentarz like \'" + textBox1.Text + "\' or ";
                            break;
                        case "po autorze":
                            sql += " autor like \'%" + textBox1.Text + "%\' or autor like \'" + textBox1.Text + "%\' or autor like \'" + textBox1.Text + "\' or ";
                            break;
                    }

                }

                //MessageBox.Show(sql.Substring(0, sql.Length - 4));

                ds = baza.Select(sql.Substring(0, sql.Length - 4));

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
                                //z.ZweryfikujZdjecie();

                                if (z.CzyUstawioneId() == true)
                                {
                                    lista_zdjec.Add(z);
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

            return lista_zdjec;
        }

        /// <summary>
        /// Metoda kt�ra za pomoc� delegat�w informuje kontrolke wy�wietlaj�ca jakie obiekty zdj�� ma wy�wietli�
        /// </summary>
        private void WykonajZapytanie()
        {
            if (textBox1.Text == "" || checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("�eby wyszuka� podaj fraz� i zaznacz po czym chcesz szuka�");
            }
            else
            {
                List<Zdjecie> zdjecia = new List<Zdjecie>();

                if (RozpoczetoWyszukiwanie != null)
                    RozpoczetoWyszukiwanie(null);

                try
                {
                    zdjecia = ZwrocWyszukanie();
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Brak dost�pu do wybranego katalogu.");
                    return;
                }

                if (ZakonczonoWyszukiwanie != null)
                    ZakonczonoWyszukiwanie(zdjecia.ToArray(), new Katalog[0], false);
            }
        }

        /// <summary>
        /// Metoda wykonywana gdy u�ytkownik kliknie przecisk szukaj metoda wywo�uje WykonajZapytanie
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            WykonajZapytanie();            
        }

        #region Wyszukiwacz Members

        /// <summary>
        /// delegat informuj� aplikacje ze wyszukiwanie zdj�� si� zako�czy�o
        /// </summary>
        public event ZakonczonoWyszukiwanieDelegate ZakonczonoWyszukiwanie;
        /// <summary>
        /// delegat informuj� aplikacje ze wyszukiwanie zdj�� si� rozpocze�o
        /// </summary>
        public event RozpoczetoWyszukiwanieDelegate RozpoczetoWyszukiwanie;
        

        /// <summary>
        /// metoda s�u��cza do sk�adania zapytania sql i zwracaj�ca Obiekt Wyszukania
        /// </summary>
        /// <returns>zwraca obiekt Wyszukanie</returns>
        public IWyszukiwanie Wyszukaj()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        /// <summary>
        /// Metoda do mo�liwo�ci klikni�cia enter po wpisaniu frazy dzi�ki czemu mechanizm wyszukiwania jest uruchamiany bez wykorzystywania myszki
        /// </summary>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Keys k = (Keys)e.KeyChar;
            if (k == Keys.Enter)
            {
                WykonajZapytanie();
            }
        }
    }
}
