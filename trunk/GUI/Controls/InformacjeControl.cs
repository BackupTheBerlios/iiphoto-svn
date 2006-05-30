using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Photo
{
    public partial class InformacjeControl : UserControl
    {
        public InformacjeControl()
        {
            InitializeComponent();

            Tags.Columns.Add("Nazwa");
            Tags.Columns.Add("Wartoœæ");
            Exif.Columns.Add("Nazwa");
            Exif.Columns.Add("Wartoœæ");
            Tags.Columns[0].Width = 65;
            Exif.Columns[0].Width = 70;
            Tags.Columns[1].Width = 100;
            Exif.Columns[1].Width = 95;
        }

        /*public void Zaladuj(ZdjecieInfo info)
        {
            Wyswietl(info);
        }*/

        public void Zaladuj(Zdjecie zdjecie)
        {
            //ZdjecieInfo info = new ZdjecieInfo(((Zdjecie)zdjecie).PobierzDaneExif(), zdjecie.NazwaPliku, ((Zdjecie)zdjecie).Path, new Size(zdjecie.Rozmiar.Width, zdjecie.Rozmiar.Height), ((Zdjecie)zdjecie).FormatPliku);
            this.zdjecie = zdjecie;
            Wyswietl();
        }

        private void Wyswietl()
        {
            Tags.Items.Clear();
            Exif.Items.Clear();
            if (zdjecie != null)
            {
                fillTags();
                fillExif();
            }
            else
            {
                Tags.Items.Add(new ListViewItem(new string[] { "Brak", "informacji" }));
                Exif.Items.Add(new ListViewItem(new string[] { "Brak", "informacji" }));
            }
        }

        private void fillExif()
        {
            PropertyItem[] propertyItems = zdjecie.PobierzDaneExif();
            Dictionary<int, string> d = PropertyTags.defaultExifIds;
            string propertyValue;

            foreach(PropertyItem pItem in propertyItems) {
                if (d.ContainsKey(pItem.Id)) 
                {
                    propertyValue = PropertyTags.ParseProp(pItem);
                    if (!propertyValue.Equals(""))
                        Exif.Items.Add(new ListViewItem(new string[] { d[pItem.Id], propertyValue }));
                }
            }
        }

        private void fillTags()
        {
            Tags.Items.Add(new ListViewItem(new string[] { "Lokalizacja", zdjecie.Path.Substring(0, zdjecie.Path.LastIndexOf('\\') + 1) }));
            Tags.Items.Add(new ListViewItem(new string[] { "Nazwa", zdjecie.NazwaPliku }));
            Tags.Items.Add(new ListViewItem(new string[] { "Szerokoœæ", zdjecie.Rozmiar.Width.ToString() }));
            Tags.Items.Add(new ListViewItem(new string[] { "Wysokoœæ", zdjecie.Rozmiar.Height.ToString() }));
            Tags.Items.Add(new ListViewItem(new string[] { "Format", zdjecie.FormatPliku }));
            StringBuilder sb;
            List<string> Albumy = zdjecie.ZwrocNazwyAlbumow();
            List<string> Tagi = zdjecie.ZwrocNazwyTagow();
            if (Albumy.Count > 0)
            {
                sb = new StringBuilder();
                int i;
                for (i = 0; i < Albumy.Count - 1; i++)
                {
                    sb.Append(Albumy[i] + ", ");
                }
                sb.Append(Albumy[i]);
                Tags.Items.Add(new ListViewItem(new string[] { "Albumy", sb.ToString() }));
            }
            if (Tagi.Count > 0)
            {
                sb = new StringBuilder();
                int i;
                for (i = 0; i < Tagi.Count - 1; i++)
                {
                    sb.Append(Tagi[i] + ", ");
                }
                sb.Append(Tagi[i]);
                Tags.Items.Add(new ListViewItem(new string[] { "Tagi", sb.ToString() }));
            }
        }

    }
}
