using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace Photo
{
    /// <summary>
    /// Kontrolka do wyswietlania zdjecia i jego podstawowej edycji.
    /// </summary>
    public partial class WidokZdjecia : UserControl, IOpakowanieZdjec
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public WidokZdjecia()
        {
            InitializeComponent();
            padX = 0;
            padY = 0;
            Edycja = false;
            WidthRatio = 1.0;
            HeightRatio = 1.0;
            zoom = 1.0;
        }

        /// <summary>
        /// Propercja zwraca wartosc boolowska, czy kontrolka ma aktualnie zaladowane zdjecie
        /// </summary>
        public bool czyZaladowaneZdjecie
        {
            get 
            {
                return zdjecie != null;
            }
        }

        /// <summary>
        /// Metoda ustawia kontrolke w tryb dopasowywania wyswietlanego zdjecia do jej rozmiaru
        /// </summary>
        public void UstawDopasowanieDoEkranu()
        {
            DrawMyRectangle(selectedRectangle);
            zoom = 0.0;
            if (this.zdjecie != null)
            {
                Wypelnij(new IZdjecie[] { this.zdjecie });
            }
        }

        /// <summary>
        /// Metoda rysuje obszar zaznaczenia na zdjeciu
        /// </summary>
        public void RysujXorZaznaczenie()
        {
            this.DrawMyRectangle(selectedRectangle);
        }

        /// <summary>
        /// Metoda zwraca kolor piksela we wskazanym polozeniu
        /// </summary>
        /// <param name="x">Wspolrzedna X-owa</param>
        /// <param name="y">Wspolrzedna Y-kowa</param>
        /// <returns></returns>
        private Color MyGetPixel(int x, int y)
        {
            unsafe
            {
                byte* imgPtr = (byte*)(data.Scan0);
                imgPtr += y * data.Stride + x * 3;
                return Color.FromArgb(*(imgPtr++), *(imgPtr++), *imgPtr);
            }
        }

        /// <summary>
        /// Metoda ustawia kolor podanego piksela
        /// </summary>
        /// <param name="x">Wspolrzedna X-owa</param>
        /// <param name="y">Wspolrzedna Y-kowa</param>
        /// <param name="c">Kolor na ktory ma zostac ustawiony piksel</param>
        private void MySetPixel(int x, int y, Color c)
        {
            unsafe
            {
                byte* imgPtr = (byte*)(data.Scan0);
                imgPtr += y * data.Stride + x * 3;
                *(imgPtr++) = c.R;
                *(imgPtr++) = c.G;
                *(imgPtr) = c.B;
            }
        }

        /// <summary>
        /// Medoda xor'uje podany piksel z podanym kolorem
        /// </summary>
        /// <param name="x">Wspolrzedna X-owa</param>
        /// <param name="y">Wspolrzedna Y-kowa</param>
        /// <param name="color">Kolor z ktorym ma zostac xorowany piksel</param>
        private void XorPixel(int x, int y, Color color)
        {

            Color srcPixel = this.MyGetPixel(x, y);
            this.MySetPixel(x, y, Color.FromArgb(color.R ^ srcPixel.R, srcPixel.G ^ color.G, srcPixel.B ^ color.B));

        }

        /// <summary>
        /// Metoda rysuje linie
        /// </summary>
        /// <param name="srcPoint">Punkt poczatkowy</param>
        /// <param name="destPoint">Punkt docelowy</param>
        private void DrawMyLine(Point srcPoint, Point destPoint)
        {
            int d, delta_A, delta_B, x, y;
            int dx = destPoint.X - srcPoint.X;
            int dy = destPoint.Y - srcPoint.Y;

            int inc_x = (dx >= 0) ? 1 : -1;
            int inc_y = (dy >= 0) ? 1 : -1;

            dx = Math.Abs(dx);
            dy = Math.Abs(dy);

            x = 0; y = 0;

            if (dx >= dy)
            {
                d = 2 * dy - dx;
                delta_A = 2 * dy;
                delta_B = 2 * (dy - dx);

                for (int i = 0; i < dx; i++)
                {
                    XorPixel(srcPoint.X + x, srcPoint.Y + y, Color.Gray);
                    if (d > 0)
                    {
                        d += delta_B;
                        x += inc_x;
                        y += inc_y;
                    }
                    else
                    {
                        d += delta_A;
                        x += inc_x;
                    }
                }

            }
            else
            {
                d = 2 * dx - dy;
                delta_A = 2 * dx;
                delta_B = 2 * (dx - dy);

                for (int i = 0; i < dy; i++)
                {
                    XorPixel(srcPoint.X + y, srcPoint.Y + x, Color.Gray);
                    if (d > 0)
                    {
                        d += delta_B;
                        y += inc_x;
                        x += inc_y;
                    }
                    else
                    {
                        d += delta_A;
                        x += inc_y;
                    }
                }
            }
        }

        /// <summary>
        /// Metoda rysuje kwadrat
        /// </summary>
        /// <param name="r">Kwadrat ktory ma zostac narysowany</param>
        internal void DrawMyRectangle(Rectangle r)
        {
            data = ((Bitmap)pictureBox1.Image).LockBits(new Rectangle(0, 0, this.pictureBox1.Image.Width, this.pictureBox1.Image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            DrawMyLine(new Point(r.X, r.Y), new Point(r.X + r.Width, r.Y));
            DrawMyLine(new Point(r.X, r.Y), new Point(r.X, r.Y + r.Height));
            DrawMyLine(new Point(r.X + r.Width, r.Y), new Point(r.X + r.Width, r.Y + r.Height));
            DrawMyLine(new Point(r.X, r.Y + r.Height), new Point(r.X + r.Width, r.Y + r.Height));
            XorPixel(r.X, r.Y, Color.Gray);
            XorPixel(r.X + r.Width, r.Y + r.Height, Color.Gray);
            ((Bitmap)pictureBox1.Image).UnlockBits(data);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            this.checkImagePosition();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.checkImagePosition();
            Refresh();
        }

        private void onMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrag = true;
                this.lmStartingPoint = new Point(e.X - padX, e.Y - padY);
                if (lmStartingPoint.X < 0)
                    isDrag = false;
                if (lmStartingPoint.Y < 0)
                    isDrag = false;
                this.Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {

            }
        }

        private void onMouseUp(object sender, MouseEventArgs e)
        {

            isDrag = false;
            if (e.Button == MouseButtons.Left)
            {
                //this.zdjecie.Zaznaczenie = new Rectangle((int)(selectedRectangle.X * WidthRatio), (int)(selectedRectangle.Y * HeightRatio), (int)(selectedRectangle.Width * WidthRatio), (int)(selectedRectangle.Height * HeightRatio));
                if (selectedRectangle.Width == 0 || selectedRectangle.Height == 0)
                {
                    zdjecie.Zaznaczenie = new Rectangle(0, 0, 0, 0);
                }
                else
                {

                    if (zoom == 1.0)
                        zdjecie.Zaznaczenie = selectedRectangle;
                    else if (zoom == 0.0)
                        zdjecie.Zaznaczenie = new Rectangle((int)(selectedRectangle.X * WidthRatio), (int)(selectedRectangle.Y * WidthRatio), (int)(selectedRectangle.Width * WidthRatio), (int)(selectedRectangle.Height * WidthRatio));
                    else
                        zdjecie.Zaznaczenie = new Rectangle((int)(selectedRectangle.X / zoom), (int)(selectedRectangle.Y / zoom), (int)(selectedRectangle.Width / zoom), (int)(selectedRectangle.Height / zoom));
                }
                this.Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {

            }
        }

        private void onMouseMove(object sender, MouseEventArgs e)
        {
            if (isDrag)
            {
                if (e.Button == MouseButtons.Left)
                {
                    int maxX, maxY;

                    if (e.X >= this.pictureBox1.Width)
                        maxX = this.pictureBox1.Width - 1 - padX;
                    else if (e.X - padX < 0)
                        maxX = 0;
                    else
                        maxX = e.X - padX;
                    if (e.Y >= this.pictureBox1.Height)
                        maxY = this.pictureBox1.Height - 1 - padY;
                    else if (e.Y - padY < 0)
                        maxY = 0;
                    else
                        maxY = e.Y - padY;

                    //if (moving == true)
                    this.DrawMyRectangle(selectedRectangle);
                    //else
                    //    moving = true;

                    selectedRectangle = new Rectangle(lmStartingPoint.X, lmStartingPoint.Y, maxX - lmStartingPoint.X, maxY - lmStartingPoint.Y);

                    this.DrawMyRectangle(selectedRectangle);
                    this.Refresh();
                }
            }
        }

        #region IOpakowanieZdjec Members

        /// <summary>
        /// Propercja zwracajaca wyswietlane zdjecie
        /// </summary>
        /// <param name="numer">Parametr pomijany</param>
        /// <returns>Zwroca obiekt implementujacy interfejs IZdjecie</returns>
        public IZdjecie this[int numer]
        {
            get { return zdjecie; }
        }

        /// <summary>
        /// Propercja zwracajaca ilosc wyswietlanych elementow - 0 lub 1
        /// </summary>
        public int Ilosc
        {
            get 
            {
                if (zdjecie != null)
                    return 1;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Metoda dodajaca zdjecie do wyswietlenia
        /// </summary>
        /// <param name="zdjecie">Zdjecie do wyswietlenia</param>
        public void Dodaj(IZdjecie zdjecie)
        {
            Wypelnij(new IZdjecie[] { zdjecie });
        }

        /// <summary>
        /// Metoda usuwajaca zdjecie z wyswietlenia
        /// </summary>
        /// <param name="zdjecie">Zdjecie do usuniecia</param>
        public void Usun(IZdjecie zdjecie)
        {
            Oproznij();
        }

        /// <summary>
        /// Metoda czyszczaca obszar wyswietlania
        /// </summary>
        public void Oproznij()
        {
            if (this.zdjecie != null)
            {
                this.zdjecie.Dispose();
                this.zdjecie = null;
            }
            if (this.pictureBox1.Image != null)
            {
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image = null;
            }
        }

        /// <summary>
        /// Propercja zwraca wybrane (zaznaczone) zdjecia 
        /// </summary>
        public IZdjecie[] WybraneZdjecia
        {
            get { return new IZdjecie[] { this.zdjecie }; }
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
            if (this.pictureBox1.Image == null)
                return;
            RysujXorZaznaczenie();
            zdjecie.WykonajOperacje();
            zdjecie.UsunWszystkieOperacje();
            if (pictureBox1.Image.Width == zdjecie.Duze.Width && pictureBox1.Image.Height == zdjecie.Duze.Height)
            {
                //RysujXorZaznaczenie();
            }
            Wypelnij(new Zdjecie[] { zdjecie });
            
        }

        /// <summary>
        /// Metoda dodajaca operacje do wykonania na zdjeciu
        /// </summary>
        /// <param name="operacja">Obiekt bedacy poleceniem operacji</param>
        public void DodajOperacje(PolecenieOperacji operacja)
        {
            if (pictureBox1.Image == null)
                return;
            if (Edycja == false)
            {
                RozpocznijEdycje();
                zdjecie.DodajOperacje(operacja);
                ZakonczEdycje();
            }
            else
                zdjecie.DodajOperacje(operacja);
        }

        /// <summary>
        /// Metoda usuwajaca wszystkie operacje na zdjeciu
        /// </summary>
        public void UsunWszystkieOperacje()
        {
            zdjecie.UsunWszystkieOperacje();
        }

        /// <summary>
        /// Zdarzenie informujace o wybraniu zdjecia
        /// </summary>
        public event WybranoZdjecieDelegate WybranoZdjecie;

        /// <summary>
        /// Metoda niezywana
        /// </summary>
        public void Wypelnij(IZdjecie[] zdjecia, Katalog[] k, bool CzyZDrzewa)
        {
            throw new Exception("This method is not used");
        }

        /// <summary>
        /// Zdarzenie informujace o modyfikacji zdjecia
        /// </summary>
        public event ZmodyfikowanoZdjecieDelegate ZmodyfikowanoZdjecie;

        /// <summary>
        /// Metoda wypelniajaca kontrolke przekazanym zdjeciem
        /// </summary>
        /// <param name="zdjecia">Tablica zdjec</param>
        public void Wypelnij(IZdjecie[] zdjecia)
        {
            if (zdjecia.Length != 0)
            {
                //this.Oproznij();
                Zdjecie temp = this.zdjecie;
                this.zdjecie = (Zdjecie)zdjecia[0];
                this.pictureBox1.Image = ZoomImage();
                this.checkImagePosition();
                this.lmStartingPoint = new Point();
                this.selectedRectangle = new Rectangle(0, 0, 0, 0);
                zdjecie.Zaznaczenie = selectedRectangle;
                if (temp != zdjecie && temp != null)
                {
                    if (temp.Edytowano)
                    {
                        RodzajDecyzji decyzja = temp.ZapisanieNiezapisanych();
                        switch (decyzja)
                        {
                            case RodzajDecyzji.Tak:
                                temp.Zapisz();
                                break;
                            case RodzajDecyzji.Nie:
                                break;
                            case RodzajDecyzji.TakDlaWszystkich:
                                temp.Zapisz();
                                break;
                            case RodzajDecyzji.NieDlaWszystkich:
                                break;
                        }
                        
                    }
                    temp.Dispose();
                    if (ZmodyfikowanoZdjecie != null)
                        ZmodyfikowanoZdjecie(null, temp, RodzajModyfikacjiZdjecia.Zawartosc);
                }
                this.Refresh();
            }
        }

        #endregion

        /// <summary>
        /// Propercja zwracajaca/ustawiajaca zdjecie
        /// </summary>
        public Zdjecie Zdjecie
        {
            get
            {
                return zdjecie;
            }
            set
            {
                zdjecie = value;
            }
        }

        /// <summary>
        /// Metoda zapisujaca zmiany na zdjeciu
        /// </summary>
        public void ZapiszPlik()
        {
            if (czyZaladowaneZdjecie == false)
                return;
            try
            {
                zdjecie.Zapisz();
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Wybrana scie¿ka nie istnieje!", "B³¹d!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Metoda zmieniajaca rozmiar zdjecia w zaleznosci od wybranego zoom'a
        /// </summary>
        /// <returns></returns>
        private Image ZoomImage()
        {
            if (zoom == 0.0)
            {
                return FitToPage();
            }
            else if (zoom == 1.0)
            {
                return zdjecie.Duze;
            } 
            else
            {
                int scaledH, scaledW;

                if (zdjecie.Duze.Height > zdjecie.Duze.Width)
                {
                    scaledH = (int)(zdjecie.Duze.Height * zoom);
                    scaledW = (int)Math.Round((double)(zdjecie.Duze.Width * scaledH) / zdjecie.Duze.Height);
                }
                else
                {
                    scaledW = (int)(zdjecie.Duze.Width * zoom);
                    scaledH = (int)Math.Round((double)(zdjecie.Duze.Height * scaledW) / zdjecie.Duze.Width);
                }

                Bitmap newBitmap = new Bitmap(scaledW, scaledH);
                Graphics MyGraphics = Graphics.FromImage(newBitmap);
                MyGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                Rectangle MyRectan = new Rectangle(0, 0, scaledW, scaledH);
                MyGraphics.DrawImage(zdjecie.Duze, MyRectan);
                MyGraphics.Dispose();
                WidthRatio = zdjecie.Duze.Width / scaledW;
                HeightRatio = zdjecie.Duze.Height / scaledH;
                return newBitmap;
            }
        }

        /// <summary>
        /// Metoda ustawiajaca zdjecie na srodku widoku
        /// </summary>
        private void checkImagePosition()
        {
            if (this.pictureBox1.Image != null)
            {
                if (this.Width > pictureBox1.Image.Width)
                {
                    padX = (this.Width - pictureBox1.Image.Width) / 2;
                }
                else
                {
                    padX = 0;
                }

                if (this.Height > pictureBox1.Image.Height)
                {
                    padY = (this.Height - pictureBox1.Image.Height) / 2;
                }
                else
                {
                    padY = 0;
                }
                this.pictureBox1.Padding = new Padding(padX, padY, 0, 0);
                this.pictureBox1.Width = pictureBox1.Image.Width + padX;
                this.pictureBox1.Height = pictureBox1.Image.Height + padY;
            }
        }

        /// <summary>
        /// Metoda dopasowujaca zdjecie do rozmiarow kontrolki
        /// </summary>
        /// <returns></returns>
        public Bitmap FitToPage()
        {
            int scaledH, scaledW;
            int wDiff, hDiff;

            hDiff = zdjecie.Duze.Height - this.Height;
            wDiff = zdjecie.Duze.Width - this.Width;

            if (hDiff > wDiff)
            {
                scaledH = this.Height;
                scaledW = (int)Math.Round((double)(zdjecie.Duze.Width * scaledH) / zdjecie.Duze.Height);
            }
            else
            {
                scaledW = this.Width;
                scaledH = (int)Math.Round((double)(zdjecie.Duze.Height * scaledW) / zdjecie.Duze.Width);
            }

            Bitmap newBitmap = new Bitmap(scaledW, scaledH);
            Graphics MyGraphics = Graphics.FromImage(newBitmap);
            MyGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            Rectangle MyRectan = new Rectangle(0, 0, scaledW, scaledH);
            MyGraphics.DrawImage(zdjecie.Duze, MyRectan);
            MyGraphics.Dispose();
            WidthRatio = (double)zdjecie.Duze.Width / scaledW;
            HeightRatio = (double)zdjecie.Duze.Height / scaledH;
            return newBitmap;
        }

        /// <summary>
        /// Metoda ustawiajaca zoom
        /// </summary>
        /// <param name="z">Zmienna typu "double", wieksza od 0.</param>
        internal void Zoom(double z)
        {
            if (z <= 0)
                return;
            if (zoom != z)
            {
                if (this.pictureBox1.Image != null)
                    DrawMyRectangle(selectedRectangle);
                zoom = z;
                if (this.zdjecie != null)
                {
                    Wypelnij(new IZdjecie[] { this.zdjecie });
                }
            }
            
        }
    }
}
