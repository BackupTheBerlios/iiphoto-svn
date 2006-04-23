using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Photo
{
    public class WidokMiniatur : System.Windows.Forms.ListView, IOpakowanieZdjec, IKontekst
    {
        private int defaultImageSize;
        private List<IZdjecie> miniatury;
        private double zoom;
        private bool Edycja;

        public WidokMiniatur()
        {
            miniatury = new List<IZdjecie>();
            //Activate double buffering

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.defaultImageSize = 120;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            zoom = 1.0;
        }
        public WidokMiniatur(int imgSize)
            : this()
        {
            this.defaultImageSize = imgSize;
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

            newImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            newImageList.Tag = "100%";
            newImageList.TransparentColor = System.Drawing.Color.Transparent;
            newImageList.ImageSize = new Size((int)(zoom * this.defaultImageSize), (int)(zoom * this.defaultImageSize));

            int maxSize = newImageList.ImageSize.Width;
            int scaledH, scaledW, posX, posY;
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
                newImageList.Images.Add(newBitmap);
                listViewItem = this.Items.Add(new ListViewItem(z.Filename));
                listViewItem.ImageIndex = i;
            }

            // Create an new ImageList for the ListView control
            this.LargeImageList = newImageList;

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
            miniatury.Add(zdjecie);
        }

        public void Usun(IZdjecie zdjecie)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Oproznij()
        {
            miniatury.Clear();
        }

        public IZdjecie[] WybraneZdjecia()
        {
            throw new Exception("The method or operation is not implemented.");
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
                foreach (IZdjecie zdjecie in miniatury)
                {
                    zdjecie.DodajOperacje(operacja);
                }
                ZakonczEdycje();
            }
            else
            {
                foreach (IZdjecie zdjecie in miniatury)
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
    }
}
