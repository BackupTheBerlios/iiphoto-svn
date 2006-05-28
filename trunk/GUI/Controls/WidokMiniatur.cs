using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Photo
{
    public class WidokMiniatur : System.Windows.Forms.ListView, IOpakowanieZdjec, IKontekst
    {
        private int defaultImageSize;
        private bool Edycja;
        private Bitmap katalog;
        private Bitmap katalog_do_gory;
        private WypelnijMiniaturyThread wypelnijThreadClass;
        private Thread t;
        private Semaphore sem;

        private List<IZdjecie> Photos;
        private List<IZdjecie> NiePrzefiltrowanePhotos;
        private Katalog[] katalogi;
        private List<long> tagi;

        public enum listViewTag
        {
            katalog,
            zdjecie
        }

        public WidokMiniatur()
        {
            Photos = new List<IZdjecie>();
            NiePrzefiltrowanePhotos = new List<IZdjecie>();
            katalogi = new Katalog[0];
            tagi = new List<long>();
            LargeImageList = new ImageList();
            LargeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            LargeImageList.Tag = "100%";
            LargeImageList.TransparentColor = System.Drawing.Color.Transparent;
            defaultImageSize = Config.RozmiarMiniatury;
            LargeImageList.ImageSize = new Size(Config.RozmiarMiniatury, Config.RozmiarMiniatury);

            //Activate double buffering

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);

            katalog = Properties.Resources.katalog;
            katalog_do_gory = Properties.Resources.katalog_do_gory;
            Edycja = false;
            
            sem = new Semaphore(0, 1);
            sem.Release();
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        public void ZmienRozmiarMiniatur(int size)
        {
            defaultImageSize = size;
        }

        public void AddImages(List<IZdjecie> images)
        {
            this.Photos.AddRange(images);
        }

        #region IOpakowanieZdjec Members

        public IZdjecie this[int numer]
        {
            get { return Photos[numer]; }
        }

        public int Ilosc
        {
            get { return Photos.Count + katalogi.Length ; }
        }

        public int IloscZdjec
        {
            get { return Photos.Count; }
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
            if (!((Zdjecie)zdjecie).FormatPliku.Equals("Jpeg"))
            {
                return;
            }


            zdjecie.ZmodyfikowanoZdjecie += new ZmodyfikowanoZdjecieDelegate(zdjecie_ZmodyfikowanoZdjecie);
            if (CzyWyswietlic(zdjecie))
            {
                ((Zdjecie)zdjecie).indeks = LargeImageList.Images.Count;
                Photos.Add(zdjecie);
                NiePrzefiltrowanePhotos.Add(zdjecie);
                LargeImageList.Images.Add(((Zdjecie)zdjecie).StworzMiniatureDoWidokuMiniatur());
                ListViewItem listViewItem = new ListViewItem(zdjecie.NazwaPliku);
                listViewItem.ImageIndex = LargeImageList.Images.Count - 1;
                listViewItem.Tag = WidokMiniatur.listViewTag.zdjecie;
                this.Items.Add(listViewItem);
            }
            else
            {
                NiePrzefiltrowanePhotos.Add(zdjecie);
            }
        }

        void zdjecie_ZmodyfikowanoZdjecie(IKontekst kontekst, IZdjecie zdjecie, RodzajModyfikacjiZdjecia rodzaj)
        {
            LargeImageList.Images[((Zdjecie)zdjecie).indeks] = ((Zdjecie)zdjecie).StworzMiniatureDoWidokuMiniatur();
            Refresh();
        }

        public void Usun(IZdjecie zdjecie)
        {
            Photos.Remove(zdjecie);
        }

        public void Oproznij()
        {
            katalogi = new Katalog[0];
            LargeImageList.Images.Clear();
            this.Items.Clear();
        }

        public IZdjecie[] Zdjecia
        {
            get
            {
                return Photos.ToArray();
            }
        }

        public IZdjecie ZdjecieZIndeksem(int i)
        {
            foreach (Zdjecie z in Photos)
            {
                if (z.indeks == i)
                    return z;
            }
            return null;
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
            foreach (IZdjecie zdjecie in Photos)
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

        public bool CzyWyswietlic(IZdjecie zdjecie)
        {
            if (tagi.Count == 0)
                return true;

            return ((Zdjecie)zdjecie).Tagi.Exists(new Predicate<long>(CzyWyswietlicPredicate));
        }

        private bool CzyWyswietlicPredicate(long l)
        {
            return tagi.Contains(l);
        }

        public void Wypelnij(IZdjecie[] zdjecia, Katalog[] katalogi)
        {
            if (t != null && t.IsAlive)
            {
                if (wypelnijThreadClass != null)
                {
                    wypelnijThreadClass.Stop();
                }
            }
            wypelnijThreadClass = new WypelnijMiniaturyThread(this, zdjecia, katalogi);
            t = new System.Threading.Thread(new System.Threading.ThreadStart(wypelnijThreadClass.ThreadFunc));
            t.IsBackground = true;
            t.Start();
        }

        #endregion

        public void DodajTagi(List<long> t)
        {
            tagi = t;
            Wypelnij(NiePrzefiltrowanePhotos.ToArray(), katalogi);
        }

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

        public void DodajKatalog(Katalog k)
        {
            string podpis;
            int maxSize = Config.RozmiarMiniatury;
            int pos;
            Bitmap newBitmap, tempDir;
            Graphics MyGraphics;
            Rectangle MyRectan;
            int scaledD;
            ListViewItem listViewItem;

            newBitmap = new Bitmap(maxSize, maxSize);
            MyGraphics = Graphics.FromImage(newBitmap);

            scaledD = Config.RozmiarMiniatury / 2;

            using (Pen p = new Pen(Brushes.LightGray))
            {
                MyGraphics.DrawRectangle(p, 0, 0, maxSize - 1, maxSize - 1);
            }

            if (k.CzyDoGory == true)
            {
                if (katalog_do_gory.Width > scaledD)
                {
                    tempDir = (Bitmap)katalog_do_gory.GetThumbnailImage(scaledD, scaledD, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), System.IntPtr.Zero);
                    pos = (maxSize - scaledD) / 2;
                }
                else
                {
                    tempDir = katalog_do_gory;
                    scaledD = katalog_do_gory.Width;
                }
                listViewItem = new ListViewItem("..");
            }
            else
            {
                if (katalog.Width > scaledD)
                {
                    tempDir = (Bitmap)katalog.GetThumbnailImage(scaledD, scaledD, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), System.IntPtr.Zero);
                }
                else
                {
                    tempDir = katalog;
                    scaledD = katalog.Width;
                }
                podpis = k.Path.Substring(k.Path.LastIndexOf('\\') + 1);
                if (podpis.Equals(""))
                    podpis = k.Path;
                listViewItem = new ListViewItem(podpis);
            }
            pos = (maxSize - scaledD) / 2;
            MyRectan = new Rectangle(pos, pos, scaledD, scaledD);
            MyGraphics.DrawImage(tempDir, MyRectan);
            LargeImageList.Images.Add(newBitmap);
            listViewItem.ImageIndex = LargeImageList.Images.Count - 1;
            listViewItem.Tag = WidokMiniatur.listViewTag.katalog;
            this.Items.Add(listViewItem);
            this.Refresh();
        }

        public static bool ThumbnailCallback()
        {
            return true;
        }

        public void ZapiszPlik()
        {
            Zdjecie z;
            ListViewItem listViewItem = this.FocusedItem;
            z = (Zdjecie)this[listViewItem.ImageIndex - katalogi.Length];
            try
            {
                z.Zapisz();
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Wybrana scie¿ka nie istnieje!", "B³¹d!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        } 

        internal class WypelnijMiniaturyThread
        {
            WidokMiniatur widokMiniatur;
            IZdjecie[] zdjecia;
            Katalog[] katalogi;
            bool stop;

            public WypelnijMiniaturyThread(WidokMiniatur wm, IZdjecie[] z, Katalog[] k)
            {
                widokMiniatur = wm;
                zdjecia = z;
                katalogi = k;
                stop = false;
            }

            public delegate void WyswietlZdjecie(Zdjecie z);
            public delegate void WyswietlKatalog(Katalog k);
            public delegate void OproznijWidokMiniatur();

            public void ThreadFunc()
            {
                widokMiniatur.sem.WaitOne();

                widokMiniatur.Invoke(new OproznijWidokMiniatur(widokMiniatur.Oproznij));
                widokMiniatur.katalogi = katalogi;

                for (int i = 0; i < katalogi.Length; i++)
                {
                    if (stop)
                        break;
                    widokMiniatur.Invoke(new WyswietlKatalog(widokMiniatur.DodajKatalog), katalogi[i]);
                }

                widokMiniatur.Photos.Clear();
                widokMiniatur.NiePrzefiltrowanePhotos.Clear();

                for (int i = 0; i < zdjecia.Length; i++)
                {
                    if (stop)
                        break;
                    widokMiniatur.Invoke(new WyswietlZdjecie(widokMiniatur.Dodaj),zdjecia[i]);
                }
                widokMiniatur.sem.Release();
            }

            internal void Stop()
            {
                stop = true;
            }
        }

        internal void ZapiszWszystkiePliki()
        {
            foreach (Zdjecie z in Photos)
            {
                try
                {
                    z.Zapisz();
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Wybrana scie¿ka nie istnieje!", "B³¹d!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public void ZresetujIds()
        {
            foreach (Zdjecie z in Photos)
            {
                z.ResetujId();
            }
        }

        public void ZresetujTagi()
        {
            foreach (Zdjecie z in Photos)
            {
                z.ResetujTagi();
            }
        }
    }
}
