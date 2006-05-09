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
        }
        public void SetThumbnailView()
        {
            if (panele.SelectedTab == zdjecie1)
            {
                // zamykanie dytora
            }
            panele.SelectedTab = miniatury1;
        }
        public void SetImageView()
        {
            panele.SelectedTab = zdjecie1;
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
                if (panele.SelectedTab == miniatury1)
                {
                    return widokMiniatur1;
                }
                else
                {
                    return widokMiniatur1;
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
            Zdjecie z = (Zdjecie)widokMiniatur1[listViewItem.ImageIndex];
            this.widokZdjecia1.setImage(z);
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
