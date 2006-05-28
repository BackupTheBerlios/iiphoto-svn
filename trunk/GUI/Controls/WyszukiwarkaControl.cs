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
    public partial class WyszukiwarkaControl : UserControl, IWyszukiwacz
    {
        public WyszukiwarkaControl()
        {
            InitializeComponent();
        }

        public IWyszukiwacz wyszukiwacz_albumow;

        private List<Zdjecie> ZwrocWyszukanie()
        {
            Db baza = new Db();
            baza.Polacz();
            List<Zdjecie> lista_zdjec = new List<Zdjecie>();

            try
            {

                string sql = "select sciezka,nazwa_pliku from Zdjecie where ";

                string pelna_sciezka;                

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

                DataSet ds = baza.Select(sql.Substring(0, sql.Length - 4));

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

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "" || checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("¯eby wyszukaæ podaj frazê i zaznacz po czym chcesz szukaæ");
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
                    MessageBox.Show("Brak dostêpu do wybranego katalogu.");
                    return;
                }

                if (ZakonczonoWyszukiwanie != null)
                    ZakonczonoWyszukiwanie(zdjecia.ToArray(), new Katalog[0]);
            }
            /*Wyszukiwanie wynik = new Wyszukiwanie();
            if (checkBox1.Checked)
            {
                wynik.And(wyszukiwacz_albumow.Wyszukaj());
            }
            wynik.And(Wyszukaj());
            if (ZakonczonoWyszukiwanie != null)
            {
                ZakonczonoWyszukiwanie(wynik.PodajWynik(), new Katalog[0]);
            }
             * */
        }

        #region Wyszukiwacz Members

        public event ZakonczonoWyszukiwanieDelegate ZakonczonoWyszukiwanie;
        public event RozpoczetoWyszukiwanieDelegate RozpoczetoWyszukiwanie;
        public event ZnalezionoZdjecieDelegate ZnalezionoZdjecie;

        public IWyszukiwanie Wyszukaj()
        {
            Wyszukiwanie wynik = new Wyszukiwanie();
            wynik.And("..\\..\\img\\wyszuk1.bmp");
            return wynik;
        }

        #endregion
    }
}
