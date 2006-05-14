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
        }

        public void Zaladuj(ZdjecieInfo info)
        {
            Wyswietl(info);
        }

        public void Zaladuj(IZdjecie zdjecie)
        {
            ZdjecieInfo info = new ZdjecieInfo(((Zdjecie)zdjecie).PobierzDaneExif(), zdjecie.NazwaPliku, ((Zdjecie)zdjecie).Path, new Size(zdjecie.Duze.Width, zdjecie.Duze.Height), ((Zdjecie)zdjecie).FormatPliku);
            Wyswietl(info);
        }

        private void Wyswietl(ZdjecieInfo info)
        {
            Tags.Items.Clear();
            Exif.Items.Clear();
            if (info != null)
            {
                fillTags(info);
                fillExif(info.propertyItems);
            }
            else
            {
                Tags.Items.Add(new ListViewItem(new string[] { "Brak", "informacji" }));
                Exif.Items.Add(new ListViewItem(new string[] { "Brak", "informacji" }));
            }
        }

        private void fillExif(PropertyItem[] propertyItems)
        {
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

        private void fillTags(ZdjecieInfo info)
        {
            Tags.Items.Add(new ListViewItem(new string[] { "Lokalizacja", info.Sciezka.Substring(0, info.Sciezka.LastIndexOf('\\') + 1) }));
            Tags.Items.Add(new ListViewItem(new string[] { "Nazwa", info.NazwaPliku }));
            Tags.Items.Add(new ListViewItem(new string[] { "Szerokoœæ", info.Rozmiar.Width.ToString() }));
            Tags.Items.Add(new ListViewItem(new string[] { "Wysokoœæ", info.Rozmiar.Height.ToString() }));
            Tags.Items.Add(new ListViewItem(new string[] { "Format", info.Format}));
        }

    }
}
