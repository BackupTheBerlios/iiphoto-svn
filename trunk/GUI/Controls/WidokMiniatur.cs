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
    /// <summary>
    /// Kontrolka dziedziczaca po ListView, do wyswietlania miniatur zdjec.
    /// </summary>
    public class WidokMiniatur : ListView, IOpakowanieZdjec, IKontekst
    {
        private bool Edycja;
        private Bitmap katalog;
        private Bitmap katalog_do_gory;
        private WypelnijMiniaturyThread wypelnijThreadClass;
        private Thread t;
        private Semaphore sem;

        private List<IZdjecie> WyswietloneZdjecia;
        private List<IZdjecie> WszystkieZdjecia;
        private Katalog[] katalogi;
        private List<long> tagi;

        /// <summary>
        /// Pole informujace czy miniatury pochodza z drzewa katalogow,
        /// czy z innego zrodla
        /// </summary>
        public bool MiniaturyZDrzewa;

        /// <summary>
        /// Wyliczenie informujace czy ListViewItem jest zdjeciem czy katalogiem
        /// </summary>
        public enum listViewTag
        {
            katalog,
            zdjecie
        }

        /// <summary>
        /// Konstruktor bezparametryczny
        /// </summary>
        public WidokMiniatur()
        {
            WyswietloneZdjecia = new List<IZdjecie>();
            WszystkieZdjecia = new List<IZdjecie>();
            katalogi = new Katalog[0];
            tagi = new List<long>();
            LargeImageList = new ImageList();
            LargeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            LargeImageList.Tag = "100%";
            LargeImageList.TransparentColor = System.Drawing.Color.Transparent;
            LargeImageList.ImageSize = new Size(Config.RozmiarMiniatury + 2, Config.RozmiarMiniatury + 2);

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

        /// <summary>
        /// Metoda zmieniajaca rozmiar wyswietlanych miniatur
        /// </summary>
        /// <param name="size">Nowy rozmiar</param>
        public void ZmienRozmiarMiniatur(int size)
        {
            LargeImageList.ImageSize = new Size(size + 2, size + 2);
        }

        #region IOpakowanieZdjec Members

        /// <summary>
        /// Propercja zwracajaca zdjecie sposrod wyswietlanych zdjec
        /// </summary>
        /// <param name="numer">Indeks zdjecia do pobrania</param>
        /// <returns>Zwroca obiekt implementujacy interfejs IZdjecie</returns>
        public IZdjecie this[int numer]
        {
            get { return WyswietloneZdjecia[numer]; }
        }

        /// <summary>
        /// Propercja zwracajaca ilosc wyswietlanych elementow
        /// </summary>
        public int Ilosc
        {
            get { return WyswietloneZdjecia.Count + katalogi.Length ; }
        }

        /// <summary>
        /// Propercja zwracajaca ilosc wyswietlanych zdjec
        /// </summary>
        public int IloscZdjec
        {
            get { return WyswietloneZdjecia.Count; }
        }

        /// <summary>
        /// Propercja zwracajaca ilosc wyswietlanych katalogow
        /// </summary>
        public int IloscKatalogow
        {
            get { return katalogi.Length; }
        }

        /// <summary>
        /// Propercja zwracajaca aktualnie wyswietlane katalogi 
        /// </summary>
        public Katalog[] Katalogi
        {
            get { return katalogi; }
        }

        /// <summary>
        /// Metoda dodajaca zdjecie do wyswietlenia
        /// </summary>
        /// <param name="zdjecie">Zdjecie do wyswietlenia</param>
        public void Dodaj(IZdjecie zdjecie)
        {
            if (zdjecie.Miniatura == null)
                return;
            if (!((Zdjecie)zdjecie).FormatPliku.Equals("Jpeg"))
            {
                return;
            }

            zdjecie.ZmodyfikowanoZdjecie += new ZmodyfikowanoZdjecieDelegate(ZmodyfikowanoZdjecie);
            if (CzyWyswietlic(zdjecie))
            {
                ((Zdjecie)zdjecie).indeks = LargeImageList.Images.Count;
                WyswietloneZdjecia.Add(zdjecie);
                WszystkieZdjecia.Add(zdjecie);
                LargeImageList.Images.Add(((Zdjecie)zdjecie).StworzMiniatureDoWidokuMiniatur());
                ListViewItem listViewItem = new ListViewItem(zdjecie.NazwaPliku);
                listViewItem.ImageIndex = LargeImageList.Images.Count - 1;
                listViewItem.Tag = WidokMiniatur.listViewTag.zdjecie;
                this.Items.Add(listViewItem);
            }
            else
            {
                WszystkieZdjecia.Add(zdjecie);
            }
        }

        /*public void OdswiezZdjecie(IZdjecie zdjecie)
        {
            LargeImageList.Images[((Zdjecie)zdjecie).indeks] = ((Zdjecie)zdjecie).StworzMiniatureDoWidokuMiniatur();
            Refresh();
        }*/

        /// <summary>
        /// Metoda usuwajaca zdjecie z kolekcji zdjec do wyswietlenia
        /// </summary>
        /// <param name="zdjecie">Zdjecie do usuniecia</param>
        public void Usun(IZdjecie zdjecie)
        {
            WyswietloneZdjecia.Remove(zdjecie);
            WszystkieZdjecia.Remove(zdjecie);
        }

        /// <summary>
        /// Metoda odswiezajaca widok miniatur
        /// </summary>
        public void Odswiez()
        {
            Wypelnij(WszystkieZdjecia.ToArray(), katalogi, MiniaturyZDrzewa);
        }

        /// <summary>
        /// Metoda czyszczaca obszar wyswietlania
        /// </summary>
        public void Oproznij()
        {
            katalogi = new Katalog[0];
            LargeImageList.Images.Clear();
            this.Items.Clear();
        }

        /// <summary>
        /// Propercja zwracajaca kolekcje wyswietlanych zdjec 
        /// </summary>
        public IZdjecie[] Zdjecia
        {
            get
            {
                return WyswietloneZdjecia.ToArray();
            }
        }

        /// <summary>
        /// Metoda zwracajaca zdjecie, ktorego pole "indeks" jest rowne podanej wartosci
        /// </summary>
        /// <param name="i">Indeks szukanego zdjecia</param>
        /// <returns>Obiekt implementujacy interfejs IZdjecie</returns>
        public IZdjecie ZdjecieZIndeksem(int i)
        {
            foreach (Zdjecie z in WyswietloneZdjecia)
            {
                if (z.indeks == i)
                    return z;
            }
            return null;
        }

        /// <summary>
        /// Propercja zwraca wybrane (zaznaczone) zdjecia 
        /// </summary>
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

        /// <summary>
        /// Metoda rozpoczynajaca tryb szybkiej edycji
        /// </summary>
        public void RozpocznijEdycje()
        {
            Edycja = true;
        }

        /// <summary>
        /// Metoda konczaca tryb szybkiej edycji - wykonujaca wszystkie oczekujace operacje
        /// </summary>
        public void ZakonczEdycje()
        {
            Edycja = false;
            foreach (IZdjecie zdjecie in WyswietloneZdjecia)
            {
                zdjecie.WykonajOperacje();
                zdjecie.UsunWszystkieOperacje();
            }
        }


        /// <summary>
        /// Metoda dodajaca operacje do wykonania na zdjeciu/ach
        /// </summary>
        /// <param name="operacja">Obiekt bedacy poleceniem operacji</param>
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

        /// <summary>
        /// Metoda usuwajaca wszystkie operacje na zdjeciu/ach z wybranych (zaznaczonych) zdjec
        /// </summary>
        public void UsunWszystkieOperacje()
        {
            foreach (IZdjecie zdjecie in WybraneZdjecia)
            {
                zdjecie.UsunWszystkieOperacje();
            }
        }

        /// <summary>
        /// Zdarzenie informujace o wybraniu zdjecia z kolekcji
        /// </summary>
        public event WybranoZdjecieDelegate WybranoZdjecie;

        /// <summary>
        /// Metoda sprawdzajaca czy podane zdjecie spelnia kryteria filtracji tagami
        /// </summary>
        /// <param name="zdjecie">Zdjecie do przefitlrowania</param>
        /// <returns>Wartosc boolowska informujaca czy zdjecie nalezy wyswietlic czy nie</returns>
        public bool CzyWyswietlic(IZdjecie zdjecie)
        {
            if (tagi.Count == 0)
                return true;

            bool wyswietlic = true;
            foreach (long l in tagi) 
            {
                if (!((Zdjecie)zdjecie).Tagi.Contains(l))
                    wyswietlic = false;
            }
            return wyswietlic;
        }

        /// <summary>
        /// Metoda wypelniajaca zdjeciami i katalogami widok miniatur.
        /// </summary>
        /// <param name="zdjecia">Tablica obiektow do wyswietlenia</param>
        /// <param name="katalogi">Tablica katalogow do wyswietlenia</param>
        /// <param name="CzyZDrzewa">Zmienna informujaca czy podane dane pochodza z drzewa katalogow czy nie</param>
        public void Wypelnij(IZdjecie[] zdjecia, Katalog[] katalogi, bool CzyZDrzewa)
        {
            MiniaturyZDrzewa = CzyZDrzewa;
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

        /// <summary>
        /// Metoda ustawiajaca nowe tagi do filtrowania i przeladowujaca kontrolke
        /// </summary>
        /// <param name="t">Nowa lista tagow</param>
        public void DodajTagi(List<long> t)
        {
            tagi = t;
            Wypelnij(WszystkieZdjecia.ToArray(), katalogi, MiniaturyZDrzewa);
        }

        #region IKontekst Members

        /// <summary>
        /// Metoda nieuzywana
        /// </summary>
        public void DodajDoKontekstu(IZdjecie zdjecie)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Metoda nieuzywana
        /// </summary>
        public void UsunZKontekstu(IZdjecie zdjecie)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Metoda zmieniajaca miniature zdjecia, na ktorym wykonano modyfikacje
        /// </summary>
        /// <param name="kontekst">Nieuzywany</param>
        /// <param name="zdjecie">Zdjecie na ktorym wykonano modyfikacje</param>
        /// <param name="rodzaj">Rodzaj modyfikacji</param>
        public void ZmodyfikowanoZdjecie(IKontekst kontekst, IZdjecie zdjecie, RodzajModyfikacjiZdjecia rodzaj)
        {
            int indx = ((Zdjecie)zdjecie).indeks - katalogi.Length;
            if (indx < 0 || indx > WyswietloneZdjecia.Count)
                return;
            IZdjecie z = WyswietloneZdjecia[indx];
            if (z == zdjecie)
            {
                LargeImageList.Images[((Zdjecie)zdjecie).indeks] = ((Zdjecie)zdjecie).StworzMiniatureDoWidokuMiniatur();
                Refresh();
            }
        }

        #endregion

        /// <summary>
        /// Metoda dodajaca miniature katalogu do wyswietlenia
        /// </summary>
        /// <param name="k">Katalog do wyswietlenia</param>
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

        private static bool ThumbnailCallback()
        {
            return true;
        }

        /// <summary>
        /// Metoda zapisujaca na dysku wybrane zdjecie
        /// </summary>
        public void ZapiszPlik()
        {
            Zdjecie z;
            ListViewItem listViewItem = this.FocusedItem;
            if (listViewItem == null || (WidokMiniatur.listViewTag)listViewItem.Tag == WidokMiniatur.listViewTag.katalog)
                return;
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

        /// <summary>
        /// Klasa wykonujaca wypelnianie elementami widoku miniatur, poprzez osobny watek
        /// </summary>
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
            public delegate RodzajDecyzji ZapiszZdjecieDelegate();

            public void ThreadFunc()
            {
                widokMiniatur.sem.WaitOne();
                bool takDlaWszystkich = false;
                foreach (Zdjecie z in widokMiniatur.Zdjecia)
                {
                    if (z.Edytowano == true)
                    {
                        if (takDlaWszystkich)
                        {
                            z.Zapisz();
                        }
                        else
                        {
                            RodzajDecyzji decyzja = (RodzajDecyzji)widokMiniatur.Invoke(new ZapiszZdjecieDelegate(z.ZapisanieNiezapisanych));
                            if (decyzja == RodzajDecyzji.Tak)
                            {
                                z.Zapisz();
                            }
                            else if (decyzja == RodzajDecyzji.TakDlaWszystkich)
                            {
                                takDlaWszystkich = true;
                                z.Zapisz();
                            }
                            else if (decyzja == RodzajDecyzji.NieDlaWszystkich)
                            {
                                break;
                            }
                        }

                    }
                }

                widokMiniatur.Invoke(new OproznijWidokMiniatur(widokMiniatur.Oproznij));
                widokMiniatur.katalogi = katalogi;

                for (int i = 0; i < katalogi.Length; i++)
                {
                    if (stop)
                        break;
                    widokMiniatur.Invoke(new WyswietlKatalog(widokMiniatur.DodajKatalog), katalogi[i]);
                }

                widokMiniatur.WyswietloneZdjecia.Clear();
                widokMiniatur.WszystkieZdjecia.Clear();

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

        /// <summary>
        /// Metoda zapisujaca zmiany na wszystkich wyswietlanych aktualnie zdjeciach, na ktorych zostaly wykonane modyfikacje
        /// </summary>
        internal void ZapiszWszystkiePliki()
        {
            foreach (Zdjecie z in WyswietloneZdjecia)
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

        /// <summary>
        /// Metoda resetujaca i odswiezajaca identyfikatory aktualnie wyswietlonych zdjec
        /// </summary>
        public void ZresetujIds()
        {
            foreach (Zdjecie z in WyswietloneZdjecia)
            {
                z.ResetujId();
            }
        }

        /// <summary>
        /// Metoda resetujaca i odswiezajaca tagi aktualnie wyswietlonych zdjec
        /// </summary>
        public void ZresetujTagi()
        {
            foreach (Zdjecie z in WyswietloneZdjecia)
            {
                z.ResetujTagi();
            }
            Odswiez();
        }
    }
}
