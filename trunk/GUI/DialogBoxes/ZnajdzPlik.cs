using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace Photo
{
    /// <summary>
    /// Formatka wyswietlana, jesli zostanie zazadane wyswietlenie zdjecia z kolekcji,
    /// a nie zostanie odnaleziono one na dysku twardym. Formatka umozliwa odnalezienie 
    /// brakujacych zdjec.
    /// </summary>
    public partial class ZnajdzPliki : Form
    {
        private string plikSzukany;
        private string plikOdnaleziony;
        private long id;

        /// <summary>
        /// Propercja zwracajaca odnaleziony plik
        /// </summary>
        public string OdnalezionyPlik
        {
            get
            {
                return plikOdnaleziony;
            }
        }

        /// <summary>
        /// Konstruktor 
        /// </summary>
        /// <param name="kv">Para identyfikatora i lokalizacji szukanego zdjecia</param>
        public ZnajdzPliki(KeyValuePair<long, string> kv)
        {
            InitializeComponent();
            plikSzukany = kv.Value;
            id = kv.Key;
            Znajdz();
        }

        private void Znajdz()
        {
            textBox1.Text = "Plik '" + plikSzukany + "' nie zosta³ odnaleziony na dysku. Proszê podaæ lokalizacjê, w której wykonaæ jego poszukiwanie.";
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            if (!textBox2.Text.Equals("")) {
                List<string> pliki = new List<string>();
                pliki.AddRange(Directory.GetFiles(textBox2.Text, "*.jpg"));
                pliki.AddRange(Directory.GetFiles(textBox2.Text, "*.jpeg"));

                PropertyItem[] items;
                string iiphotoTag;
                foreach (string plik in pliki)
                {
                    items = Zdjecie.PobierzDaneExif(plik);
                    foreach (PropertyItem item in items)
                    {
                        if (item.Id == PropertyTags.IIPhotoTag)
                        {
                            iiphotoTag = PropertyTags.ParseProp(item);
                            if (long.Parse(iiphotoTag) == id)
                            {
                                plikOdnaleziony = plik;
                                this.Close();
                                return;
                            }
                        }
                    }
                }

            }
            MessageBox.Show("Nie znaleziono szukanego pliku w podanej lokalizacji!");
            this.Close();
            return;
        }

        private void Pomin_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PominAll_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        internal bool czyOdnaleziono()
        {
            return (plikOdnaleziony != null);
        }

        private void Usun_Click(object sender, EventArgs e)
        {
            Zdjecie.UsunZAlbumu(id);
        }
    }
}