using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;

namespace Photo
{
    public class WidokMiniatur : System.Windows.Forms.ListView, IOpakowanieZdjec, IKontekst
    {
        private int defaultImageSize;
        private List<IZdjecie> miniatury = new List<IZdjecie>();
        private double zoom;
        private bool Edycja;
        private Semaphore sem_dodawania = new Semaphore(0, 1);
        private Bitmap katalog;
        private Bitmap katalog_do_gory;

        public enum listViewTag
        {
            katalog,
            zdjecie
        }

        public WidokMiniatur()
        {
            LargeImageList = new ImageList();
            LargeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            LargeImageList.Tag = "100%";
            LargeImageList.TransparentColor = System.Drawing.Color.Transparent;
            defaultImageSize = 120;
            zoom = 1.0;
            LargeImageList.ImageSize = new Size((int)(zoom * this.defaultImageSize), (int)(zoom * this.defaultImageSize));

            sem_dodawania.Release();
            //Activate double buffering

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);

            katalog = Properties.Resources.katalog;
            katalog_do_gory = Properties.Resources.katalog_do_gory;
            Edycja = false;

        }
        public WidokMiniatur(int imgSize)
            : this()
        {
            this.defaultImageSize = imgSize;
            LargeImageList.ImageSize = new Size((int)(zoom * this.defaultImageSize), (int)(zoom * this.defaultImageSize));
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        public void AddImages(List<IZdjecie> images)
        {
            this.miniatury.AddRange(images);
        }

        public void ShowImages()
        {
            Zdjecie z;
            Bitmap newBitmap;
            Graphics MyGraphics;
            Rectangle MyRectan;
            ImageList newImageList = new ImageList();
            ListViewItem listViewItem;

            int maxSize = LargeImageList.ImageSize.Width;
            int scaledH, scaledW, posX, posY;

            LargeImageList.Images.Clear();
            this.Items.Clear();

            for (int i = 0; i < miniatury.Count; i++)
            {
                z = (Zdjecie)miniatury[i];
                newBitmap = new Bitmap(maxSize, maxSize);
                MyGraphics = Graphics.FromImage(newBitmap);

                if (z.Miniatura.Height > z.Miniatura.Width)
                {
                    scaledH = maxSize;
                    scaledW = (int)Math.Round((double)(z.Miniatura.Width * scaledH) / z.Miniatura.Height);
                    posX = (maxSize - scaledW) / 2;
                    posY = 0;
                }
                else
                {
                    scaledW = maxSize;
                    scaledH = (int)Math.Round((double)(z.Miniatura.Height * scaledW) / z.Miniatura.Width);
                    posX = 0;
                    posY = (maxSize - scaledH) / 2;
                }

                MyRectan = new Rectangle(posX, posY, scaledW, scaledH);
                using (Pen p = new Pen(Brushes.LightGray))
                {
                    MyGraphics.DrawRectangle(p, 0, 0, maxSize - 1, maxSize - 1);
                }
                MyGraphics.DrawImage(z.Miniatura, MyRectan);
                newBitmap.Tag = z;
                LargeImageList.Images.Add(newBitmap);
                listViewItem = this.Items.Add(new ListViewItem(z.NazwaPliku));
                listViewItem.ImageIndex = i;
            }

            // Update the ListView control
            this.Refresh();
        }

        #region IOpakowanieZdjec Members

        public IZdjecie this[int numer]
        {
            get { return miniatury[numer]; }
        }

        public int Ilosc
        {
            get { return miniatury.Count; }
        }

        public void Dodaj(IZdjecie zdjecie)
        {
            if (!((Zdjecie)zdjecie).FormatPliku.Equals("Jpeg") && !((Zdjecie)zdjecie).FormatPliku.Equals("Tiff"))
            {
                return;
            }
            sem_dodawania.WaitOne();
            miniatury.Add(zdjecie);
            int maxSize = LargeImageList.ImageSize.Width;
            int scaledH, scaledW, posX, posY;
            Bitmap newBitmap = new Bitmap(maxSize, maxSize);
            Graphics MyGraphics = Graphics.FromImage(newBitmap);
            MyGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            if (zdjecie.Miniatura.Height > zdjecie.Miniatura.Width)
            {
                scaledH = maxSize;
                scaledW = (int)Math.Round(
                    (double)(zdjecie.Miniatura.Width * scaledH) / zdjecie.Miniatura.Height);
                posX = (maxSize - scaledW) / 2;
                posY = 0;
            }
            else
            {
                scaledW = maxSize;
                scaledH = (int)Math.Round(
                    (double)(zdjecie.Miniatura.Height * scaledW) / zdjecie.Miniatura.Width);
                posX = 0;
                posY = (maxSize - scaledH) / 2;
            }

            Rectangle MyRectan = new Rectangle(posX, posY, scaledW, scaledH);
            using (Pen p = new Pen(Brushes.LightGray))
            {
                MyGraphics.DrawRectangle(p, 0, 0, maxSize - 1, maxSize - 1);
            }
            MyGraphics.DrawImage(zdjecie.Miniatura, MyRectan);
            newBitmap.Tag = zdjecie;
            LargeImageList.Images.Add(newBitmap);
            ListViewItem listViewItem = new ListViewItem(zdjecie.NazwaPliku);
            listViewItem.ImageIndex = LargeImageList.Images.Count - 1;
            listViewItem.Tag = WidokMiniatur.listViewTag.zdjecie;
            this.Items.Add(listViewItem);
            sem_dodawania.Release();
        }

        public void Usun(IZdjecie zdjecie)
        {
            miniatury.Remove(zdjecie);
        }

        public void Oproznij()
        {
            miniatury.Clear();
        }

        public IZdjecie[] WybraneZdjecia
        {
            get
            {
                Zdjecie[] zdjecia = new Zdjecie[SelectedItems.Count];
                for (int i = 0; i < SelectedItems.Count; i++)
                {
                    zdjecia[i] = (Zdjecie)this[SelectedItems[i].ImageIndex];
                }
                return zdjecia;
            }
        }

        public void RozpocznijEdycje()
        {
            Edycja = true;
        }

        public void ZakonczEdycje()
        {
            Edycja = false;
            foreach (IZdjecie zdjecie in miniatury)
            {
                zdjecie.WykonajOperacje();
                zdjecie.UsunWszystkieOperacje();
            }
        }

        public void DodajOperacje(PolecenieOperacji operacja)
        {
            if (Edycja == false)
            {
                RozpocznijEdycje();
                foreach (IZdjecie zdjecie in WybraneZdjecia)
                {
                    zdjecie.DodajOperacje(operacja);
                }
                ZakonczEdycje();
            }
            else
            {
                foreach (IZdjecie zdjecie in WybraneZdjecia)
                {
                    zdjecie.DodajOperacje(operacja);
                }
            }
        }

        public void UsunWszystkieOperacje()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public event WybranoZdjecieDelegate WybranoZdjecie;

        public void Wypelnij(IZdjecie[] zdjecia)
        {
            /* Niepotrzebne
             * 
             * sem_dodawania.WaitOne();
            miniatury.Clear();
            miniatury.AddRange(zdjecia);
            foreach (Zdjecie z in zdjecia)
            {
                if (!z.Miniatura.RawFormat.Equals(ImageFormat.Jpeg) && !z.Miniatura.RawFormat.Equals(ImageFormat.Tiff))
                {
                    miniatury.Remove(z);
                }
            }
            ShowImages();
            sem_dodawania.Release();*/
        }

        #endregion

        #region IKontekst Members

        public void DodajDoKontekstu(IZdjecie zdjecie)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UsunZKontekstu(IZdjecie zdjecie)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ZmodyfikowanoZdjecie(IKontekst kontekst, IZdjecie zdjecie, RodzajModyfikacjiZdjecia rodzaj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        internal void DodajKatalogi(Katalog[] katalogi)
        {
            for (int i = 0; i < katalogi.Length; i++)
            {
                if (katalogi[i].CzyDoGory == true)
                    LargeImageList.Images.Add(katalog_do_gory);
                else
                    LargeImageList.Images.Add(katalog);
                ListViewItem listViewItem = new ListViewItem(katalogi[i].Path);
                listViewItem.ImageIndex = LargeImageList.Images.Count - 1;
                this.Items.Add(listViewItem);
            }
        }
    }
}
