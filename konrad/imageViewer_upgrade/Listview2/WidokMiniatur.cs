using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Photo
{
    public class WidokMiniatur : System.Windows.Forms.ListView, IOpakowanieZdjec, IKontekst
    {
        private int defaultImageSize;
        private List<Miniatura> miniatury;

        public WidokMiniatur()
        {
            miniatury = new List<Miniatura>();
            //Activate double buffering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            this.defaultImageSize = 120;
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

        public void AddImages(List<Miniatura> images)
        {
            this.miniatury.AddRange(images);
        }

        public List<Zdjecie> Images
        {
            get
            {
                List<Zdjecie> zdjecia = new List<Zdjecie>();
                foreach (Miniatura m in miniatury)
                {
                    zdjecia.Add(new Zdjecie(m.tag.Path));
                }
                return zdjecia;
            }
        }

        public void ShowImages(double zoom)
        {
            Miniatura z;
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
                z = miniatury[i];
                newBitmap = new Bitmap(maxSize, maxSize);
                MyGraphics = Graphics.FromImage(newBitmap);

                if (z.Bitmap.Height > z.Bitmap.Width)
                {
                    scaledH = maxSize;
                    scaledW = (int)Math.Round((double)(z.Bitmap.Width * scaledH) / z.Bitmap.Height);
                    posX = (maxSize - scaledW) / 2;
                    posY = 0;
                }
                else
                {
                    scaledW = maxSize;
                    scaledH = (int)Math.Round((double)(z.Bitmap.Height * scaledW) / z.Bitmap.Width);
                    posX = 0;
                    posY = (maxSize - scaledH) / 2;
                }

                MyRectan = new Rectangle(posX, posY, scaledW, scaledH);
                using (Pen p = new Pen(Brushes.LightGray))
                {
                    MyGraphics.DrawRectangle(p, 0, 0, maxSize - 1, maxSize - 1);
                }
                MyGraphics.DrawImage(z.Bitmap, MyRectan);
                newBitmap.Tag = new MiniaturaTag(z.tag.Filename);
                newImageList.Images.Add(newBitmap);
                listViewItem = this.Items.Add(new ListViewItem(z.tag.Filename));
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
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int Ilosc
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void Dodaj(IZdjecie zdjecie)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Usun(IZdjecie zdjecie)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Oproznij()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IZdjecie[] WybraneZdjecia()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RozpocznijEdycje()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ZakonczEdycje()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DodajOperacje(PolecenieOperacji operacja)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UsunWszystkieOperacje()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public event WybranoZdjecieDelegate WybranoZdjecie;

        public void Wypelnij(IZdjecie[] zdjecia)
        {
            throw new Exception("The method or operation is not implemented.");
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
