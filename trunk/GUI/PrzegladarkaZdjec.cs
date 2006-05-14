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
    public partial class PrzegladarkaZdjec : UserControl, IOpakowanieZdjec
    {
        public PrzegladarkaZdjec()
        {
            InitializeComponent();
            Context = new ContextMenuStrip();
        }
        public void SetThumbnailView()
        {
            if (panele.SelectedTab == zdjecie1Tab)
            {
                // zamykanie dytora
            }
            panele.SelectedTab = miniatury1Tab;

        }
        public void SetImageView()
        {
            panele.SelectedTab = zdjecie1Tab;
        }

        public WidokMiniatur Thumbnailview
        {
            get
            {
                return widokMiniatur1;
            }
        }
        public WidokZdjecia Imageview
        {
            get
            {
                return widokZdjecia1;
            }
        }
        IOpakowanieZdjec AktywneOpakowanie
        {
            get
            {
                if (panele.SelectedTab == miniatury1Tab)
                {
                    return widokMiniatur1;
                }
                else
                {
                    return widokZdjecia1;
                }
            }
        }

        /*public void Zoom(double zoom)
        {
            if (imageView1.Visible == true)
            {
                imageView1.Zoom(zoom);
            }
            else if (thumbnailView1.Visible == true)
            {
                thumbnailView1.ShowImages(zoom);
            }
        }*/

        /*public void Crop()
        {
            if (imageView1.Visible == true)
            {
                imageView1.Crop();
            }
        }*/

        /*public void toGrayScale()
        {
            if (imageView1.Visible == true)
            {
                imageView1.toGrayScale();
            }
        }*/

        #region IOpakowanieZdjec Members

        public IZdjecie this[int numer]
        {
            get { return AktywneOpakowanie[numer]; }
        }

        public int Ilosc
        {
            get { return AktywneOpakowanie.Ilosc; }
        }

        public void Dodaj(IZdjecie zdjecie)
        {
            AktywneOpakowanie.Dodaj(zdjecie);
            /*if (widokZdjecia1.Visible == true)
                SetThumbnailView();*/
        }

        public void Usun(IZdjecie zdjecie)
        {
            AktywneOpakowanie.Usun(zdjecie);
        }

        public void Oproznij()
        {
            AktywneOpakowanie.Oproznij();
        }

        public IZdjecie[] WybraneZdjecia
        {
            get
            {
                return AktywneOpakowanie.WybraneZdjecia;
            }
        }

        public void RozpocznijEdycje()
        {
            AktywneOpakowanie.RozpocznijEdycje();
        }

        public void ZakonczEdycje()
        {
            AktywneOpakowanie.ZakonczEdycje();
        }

        public void DodajOperacje(PolecenieOperacji operacja)
        {
            AktywneOpakowanie.DodajOperacje(operacja);
        }

        public void UsunWszystkieOperacje()
        {
            AktywneOpakowanie.UsunWszystkieOperacje();
        }

        public void Wypelnij(IZdjecie[] zdjecia)
        {
            this.SetThumbnailView();
            AktywneOpakowanie.Wypelnij(zdjecia);
        }

        #endregion

        public void DodajKatalogi(Katalog[] katalogi)
        {
            widokMiniatur1.DodajKatalogi(katalogi);
        }

        private void widokMiniatur_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Context.Items.Clear();

                ToolStripItem toolStripItem = Context.Items.Add("Dodaj zaznaczenie do kolekcji");
                toolStripItem.Click += new EventHandler(DodajZaznaczenieDoKolekcji);

                Context.Show(this.widokMiniatur1, new Point(e.X, e.Y));      
            }
        }

        private void DodajZaznaczenieDoKolekcji(object sender, EventArgs e)
        {
            string temp = "";
            Zdjecie[] zdjecia = (Zdjecie[])widokMiniatur1.WybraneZdjecia;
            if (zdjecia.Length != 0)
            {
                for (int i = 0; i < zdjecia.Length; i++)
                {
                    temp += zdjecia[i].Path + " ";
                }
                MessageBox.Show("Dodaje zdjecia: " + temp + " do kolekcji!");
            }
        }

        private void widokMiniatur_selectedIndexChanged(object sender, EventArgs e)
        {
            Zdjecie[] zdjecia = (Zdjecie[])((WidokMiniatur)sender).WybraneZdjecia;
            if (zdjecia.Length == 1)
            {
                ZdjecieInfo info = new ZdjecieInfo(Zdjecie.PobierzDaneExif(zdjecia[0].Path), zdjecia[0].NazwaPliku, zdjecia[0].Path, new Size(zdjecia[0].Duze.Width, zdjecia[0].Duze.Height), zdjecia[0].FormatPliku);
                if (ZaznaczonoZdjecie != null)
                    ZaznaczonoZdjecie(info);
            }
        }

        private void panele_onSelectedIndexChanged(object sender, EventArgs e)
        {
            if (((TabControl)sender).SelectedTab == zdjecie1Tab && widokZdjecia1.czyZaladowaneZdjecie)
            {
                ZdjecieInfo info = widokZdjecia1.pobierzInfoZdjecia;
                if (ZaznaczonoZdjecie != null)
                    ZaznaczonoZdjecie(info);
            }
        }

        private void wybranoItem(ListViewItem listViewItem)
        {
            if ((WidokMiniatur.listViewTag)listViewItem.Tag == WidokMiniatur.listViewTag.zdjecie)
            {
                Zdjecie[] z = new Zdjecie[] { (Zdjecie)widokMiniatur1[listViewItem.ImageIndex] };
                this.widokZdjecia1.Wypelnij(z);
                this.SetImageView();
                if (WybranoZdjecie != null)
                    WybranoZdjecie(z[0]);
            }
            else
            {
                //wejdz do katalogu
            }
        }

        private void widokMiniatur_DoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem listViewItem = ((WidokMiniatur)sender).FocusedItem;
            wybranoItem(listViewItem);
        }

        private void widokMiniatur1_keyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ListViewItem listViewItem = ((WidokMiniatur)sender).FocusedItem;
                wybranoItem(listViewItem);
            }
        }
    }
}


