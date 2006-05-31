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
    public partial class WidokZdjecia : UserControl, IOpakowanieZdjec
    {
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

        public bool czyZaladowaneZdjecie
        {
            get 
            {
                return zdjecie != null;
            }
        }

        public void UstawDopasowanieDoEkranu()
        {
            DrawMyRectangle(selectedRectangle);
            zoom = 0.0;
            if (this.zdjecie != null)
            {
                Wypelnij(new IZdjecie[] { this.zdjecie });
            }
        }

        public void RysujXorZaznaczenie()
        {
            this.DrawMyRectangle(selectedRectangle);
        }

        private Color MyGetPixel(int x, int y)
        {
            unsafe
            {
                byte* imgPtr = (byte*)(data.Scan0);
                imgPtr += y * data.Stride + x * 3;
                return Color.FromArgb(*(imgPtr++), *(imgPtr++), *imgPtr);
            }
        }

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

        private void XorPixel(int x, int y, Color color)
        {

            Color srcPixel = this.MyGetPixel(x, y);
            this.MySetPixel(x, y, Color.FromArgb(color.R ^ srcPixel.R, srcPixel.G ^ color.G, srcPixel.B ^ color.B));

        }

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
                if (zoom == 1.0)
                    zdjecie.Zaznaczenie = selectedRectangle;
                else if (zoom == 0.0)
                    zdjecie.Zaznaczenie = new Rectangle((int)(selectedRectangle.X * WidthRatio), (int)(selectedRectangle.Y * WidthRatio), (int)(selectedRectangle.Width * WidthRatio), (int)(selectedRectangle.Height * WidthRatio));
                else
                    zdjecie.Zaznaczenie = new Rectangle((int)(selectedRectangle.X / zoom), (int)(selectedRectangle.Y / zoom), (int)(selectedRectangle.Width / zoom), (int)(selectedRectangle.Height / zoom));

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

        public IZdjecie this[int numer]
        {
            get { return zdjecie; }
        }

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

        public void Dodaj(IZdjecie zdjecie)
        {
            Wypelnij(new IZdjecie[] { zdjecie });
        }

        public void Usun(IZdjecie zdjecie)
        {
            throw new Exception("The method or operation is not implemented.");
        }

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

        public IZdjecie[] WybraneZdjecia
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void RozpocznijEdycje()
        {
            Edycja = true;
        }

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

        public void UsunWszystkieOperacje()
        {
            zdjecie.UsunWszystkieOperacje();
        }

        public event WybranoZdjecieDelegate WybranoZdjecie;

        public void Wypelnij(IZdjecie[] zdjecia, Katalog[] k, bool CzyZDrzewa)
        {
            throw new Exception("This method is not used");
        }

        public event ZmodyfikowanoZdjecieDelegate ZmodyfikowanoZdjecie;

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
                        if (MessageBox.Show("S¹ niezapisane zmiany w zdjêciu " + temp.NazwaPliku + ". Czy zapisaæ?", "Czy zapisaæ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            temp.Zapisz();
                    }
                    temp.Dispose();
                    if (ZmodyfikowanoZdjecie != null)
                        ZmodyfikowanoZdjecie(null, temp, RodzajModyfikacjiZdjecia.Zawartosc);
                }
                this.Refresh();
            }
        }

        #endregion

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

        public void ZapiszPlik()
        {
            try
            {
                zdjecie.Zapisz();
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Wybrana scie¿ka nie istnieje!", "B³¹d!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

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

        internal void Zoom(double z)
        {
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
