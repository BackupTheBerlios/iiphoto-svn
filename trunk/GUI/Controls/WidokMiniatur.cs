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
        private List<IZdjecie> photos = new List<IZdjecie>();
        private Katalog[] katalogi;
        private double zoom;
        private bool Edycja;
        private Bitmap katalog;
        private Bitmap katalog_do_gory;
        private wyswietlZdjeciaThread thread;
        private System.Threading.Thread t;

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
            this.photos.AddRange(images);
        }

        #region IOpakowanieZdjec Members

        public IZdjecie this[int numer]
        {
            get { return photos[numer]; }
        }

        public int Ilosc
        {
            get { return photos.Count; }
        }

        public int IloscKatalogow
        {
            get { return katalogi.Length; }
        }

        public Katalog[] Katalogi
        {
            get { return katalogi; }
        }

        public void Dodaj(IZdjecie zdjecie)
        {
            if (zdjecie.Miniatura == null)
                return;
            if (!((Zdjecie)zdjecie).FormatPliku.Equals("Jpeg") && !((Zdjecie)zdjecie).FormatPliku.Equals("Tiff"))
            {
                return;
            }
            photos.Add(zdjecie);
            int maxSize = Config.maxRozmiarMiniatury;
            int posX, posY;
            Bitmap newBitmap = new Bitmap(maxSize, maxSize);
            Graphics MyGraphics = Graphics.FromImage(newBitmap);
            //MyGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            if (zdjecie.Miniatura.Height > zdjecie.Miniatura.Width)
            {
                posX = (maxSize - zdjecie.Miniatura.Width) / 2;
                posY = 0;
            }
            else
            {
                posX = 0;
                posY = (maxSize - zdjecie.Miniatura.Height) / 2;
            }

            Rectangle MyRectan = new Rectangle(posX, posY, zdjecie.Miniatura.Width, zdjecie.Miniatura.Height);
            using (Pen p = new Pen(Brushes.LightGray))
            {
                MyGraphics.DrawRectangle(p, 0, 0, maxSize - 1, maxSize - 1);
            }
            MyGraphics.DrawImage(zdjecie.Miniatura, MyRectan);
            //newBitmap.Tag = zdjecie;
            LargeImageList.Images.Add(newBitmap);
            ListViewItem listViewItem = new ListViewItem(zdjecie.NazwaPliku);
            listViewItem.ImageIndex = LargeImageList.Images.Count - 1;
            listViewItem.Tag = WidokMiniatur.listViewTag.zdjecie;
            this.Items.Add(listViewItem);
        }

        public void Usun(IZdjecie zdjecie)
        {
            photos.Remove(zdjecie);
        }

        public void Oproznij()
        {
            LargeImageList.Images.Clear();
            this.Items.Clear();
            photos.Clear();
        }

        public IZdjecie[] WybraneZdjecia
        {
            get
            {
                List<Zdjecie> zdjecia = new List<Zdjecie>();
                for (int i = 0; i < SelectedItems.Count; i++)
                {
                    if (SelectedItems[i].ImageIndex - katalogi.Length >= 0)
                        zdjecia.Add((Zdjecie)this[SelectedItems[i].ImageIndex - katalogi.Length]);
                }
                return zdjecia.ToArray();
            }
        }

        public void RozpocznijEdycje()
        {
            Edycja = true;
        }

        public void ZakonczEdycje()
        {
            Edycja = false;
            foreach (IZdjecie zdjecie in photos)
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

            if (t != null && t.IsAlive)
            {
                thread.Stop();
                t.Abort();
            }
            thread = new wyswietlZdjeciaThread(this, zdjecia);
            t = new System.Threading.Thread(new System.Threading.ThreadStart(thread.ThreadFunc));
            t.IsBackground = true;
            t.Start();
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

        internal void DodajKatalogi(Katalog[] k) {
        
            Oproznij();
            katalogi = k;
            int maxSize = Config.maxRozmiarMiniatury;
            int posX, posY;
            Bitmap newBitmap;
            Graphics MyGraphics;
            Rectangle MyRectan;
            for (int i = 0; i < katalogi.Length; i++)
            {
                newBitmap = new Bitmap(maxSize, maxSize);
                MyGraphics = Graphics.FromImage(newBitmap);


                posX = (maxSize - katalog.Width) / 2;
                posY = (maxSize - katalog.Height) / 2;

                
                using (Pen p = new Pen(Brushes.LightGray))
                {
                    MyGraphics.DrawRectangle(p, 0, 0, maxSize - 1, maxSize - 1);
                }
                
                ListViewItem listViewItem;
                if (katalogi[i].CzyDoGory == true)
                {
                    MyRectan = new Rectangle(posX, posY, katalog_do_gory.Width, katalog_do_gory.Height);
                    MyGraphics.DrawImage(katalog_do_gory, MyRectan);
                    LargeImageList.Images.Add(newBitmap);
                    listViewItem = new ListViewItem("..");
                }
                else
                {
                    MyRectan = new Rectangle(posX, posY, katalog.Width, katalog.Height);
                    MyGraphics.DrawImage(katalog, MyRectan);
                    LargeImageList.Images.Add(newBitmap);
                    listViewItem = new ListViewItem(katalogi[i].Path.Substring(katalogi[i].Path.LastIndexOf('\\') + 1));
                }
                listViewItem.ImageIndex = LargeImageList.Images.Count - 1;
                listViewItem.Tag = WidokMiniatur.listViewTag.katalog;
                this.Items.Add(listViewItem);
            }
            this.Refresh();
        }

        class wyswietlZdjeciaThread
        {
            WidokMiniatur widokMiniatur;
            IZdjecie[] zdjecia;
            bool stop;

            public wyswietlZdjeciaThread(WidokMiniatur wm, IZdjecie[] z)
            {
                widokMiniatur = wm;
                zdjecia = z;
            }

            public delegate void WyswietlZdjecie(Zdjecie z);

            public void ThreadFunc()
            {
                for (int i = 0; i < zdjecia.Length; i++)
                {
                    if (stop)
                        break;
                    widokMiniatur.Invoke(new WyswietlZdjecie(widokMiniatur.Dodaj),zdjecia[i]);
                }
            }

            internal void Stop()
            {
                stop = true;
            }
        }

    }
}
