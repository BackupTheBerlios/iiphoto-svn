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
            SetImageView();
            //SetThumbnailView(); inaczej po uruchomieniu zle dopasowuje zdjecie do rozmiarow - moze ktos wymysli czemu
        }
        public void SetThumbnailView()
        {
            if (thumbnailView1.Visible == false)
            {
                // zamykanie dytora
            }
            thumbnailView1.Visible = true;
            imageView1.Visible = false;
        }
        public void SetImageView()
        {
            imageView1.Visible = true;
            thumbnailView1.Visible = false;
        }
        public WidokMiniatur Thumbnailview
        {
            get
            {
                return thumbnailView1;
            }
        }
        public WidokZdjecia Imageview
        {
            get
            {
                return imageView1;
            }
        }
        IOpakowanieZdjec AktywneOpakowanie
        {
            get
            {
                if (imageView1.Visible == true)
                {
                    return thumbnailView1;
                }
                else
                {
                    return thumbnailView1;
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

        private void mouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem listViewItem = ((WidokMiniatur)sender).FocusedItem;
            Zdjecie z = (Zdjecie)thumbnailView1[listViewItem.ImageIndex];
            this.imageView1.setImage(z);
            this.SetImageView();
            if (WybranoZdjecie != null)
                WybranoZdjecie(z);
        }

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
            if (imageView1.Visible == true)
                SetThumbnailView();
        }

        public void Usun(IZdjecie zdjecie)
        {
            AktywneOpakowanie.Usun(zdjecie);
        }

        public void Oproznij()
        {
            AktywneOpakowanie.Oproznij();
        }

        public IZdjecie[] WybraneZdjecia()
        {
            return AktywneOpakowanie.WybraneZdjecia();
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
    }
}
