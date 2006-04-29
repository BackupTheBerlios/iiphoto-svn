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

            Tags.Items.Add(new ListViewItem(new string[] { "Brak","informacji"}));
            Exif.Items.Add(new ListViewItem(new string[] { "Brak","informacji"}));
        }

        public void Zaladuj(IZdjecie zdjecie)
        {
            Tags.Items.Clear();
            Exif.Items.Clear();
            if (zdjecie != null)
            {
                fillTags((Zdjecie)zdjecie);
                fillExif((Zdjecie)zdjecie);
            }
            else
            {
                Tags.Items.Add(new ListViewItem(new string[] { "Brak", "informacji" }));
                Exif.Items.Add(new ListViewItem(new string[] { "Brak", "informacji" }));
            }
        }

        private void fillExif(Zdjecie zdjecie)
        {
            Exif.Items.Add(new ListViewItem(new string[] { "Brak", zdjecie.GetProperty(PropertyTags.EquipMake)}));
            Exif.Items.Add(new ListViewItem(new string[] { "Brak", zdjecie.GetProperty(PropertyTags.EquipModel)}));
            Exif.Items.Add(new ListViewItem(new string[] { "Brak", zdjecie.GetProperty(PropertyTags.Orientation)}));
            Exif.Items.Add(new ListViewItem(new string[] { "Brak", zdjecie.GetProperty(PropertyTags.ExifFlash)}));
        }

        private void fillTags(Zdjecie zdjecie)
        {
            Tags.Items.Add(new ListViewItem(new string[] { "Lokalizacja", zdjecie.Path.Substring(0, zdjecie.Path.LastIndexOf('\\') + 1) }));
            Tags.Items.Add(new ListViewItem(new string[] { "Nazwa", zdjecie.NazwaPliku }));
            Tags.Items.Add(new ListViewItem(new string[] { "Szerokoœæ", zdjecie.Duze.Width.ToString() }));
            Tags.Items.Add(new ListViewItem(new string[] { "Wysokoœæ", zdjecie.Duze.Height.ToString() }));
            Tags.Items.Add(new ListViewItem(new string[] { "Format", zdjecie.FormatPliku}));
        }

    }
}
