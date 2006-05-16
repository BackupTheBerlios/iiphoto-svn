using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

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
        }

        /*public void setImage(Zdjecie zdjecie)
        {
            this.zdjecie = zdjecie;
            this.zdjecieZapas = zdjecie;
            this.pictureBoxImage = FitToPage();
            //this.checkImagePosition();
            this.lmStartingPoint = new Point();
            this.selectedRectangle = new Rectangle(0, 0, 0, 0);
            //this.clearRect = false;
        }*/

        public ZdjecieInfo pobierzInfoZdjecia
        {
            get
            {
                return new ZdjecieInfo(zdjecie.PobierzDaneExif(), zdjecie.NazwaPliku, zdjecie.Path, new Size(zdjecie.Duze.Width, zdjecie.Duze.Height), zdjecie.FormatPliku);
            }
        }

        public bool czyZaladowaneZdjecie
        {
            get {
                return zdjecie != null;
                }
        }

        private Bitmap pictureBoxImage
        {
            get
            {
                return (Bitmap)this.pictureBox1.Image;
            }
            set
            {
                this.pictureBox1.Image = value;
            }
        }

        private void checkImagePosition()
        {
            if (this.pictureBoxImage != null)
            {
                if (this.Width > pictureBoxImage.Width)
                {
                    padX = (this.Width - pictureBoxImage.Width) / 2;
                }
                else
                {
                    padX = 0;
                }

                if (this.Height > pictureBoxImage.Height)
                {
                    padY = (this.Height - pictureBoxImage.Height) / 2;
                } else {
                    padY = 0;
                }
                this.pictureBox1.Padding = new Padding(padX, padY, 0, 0);
                this.pictureBox1.Width = pictureBoxImage.Width + padX;
                this.pictureBox1.Height = pictureBoxImage.Height + padY;
            }
        }

        public Bitmap FitToPage()
        {
            /*int scaledH, scaledW;

            if (zdjecie.Duze.Height > zdjecie.Duze.Width)
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
            WidthRatio = zdjecie.Duze.Width / scaledW;
            HeightRatio = zdjecie.Duze.Height / scaledH;
            return newBitmap;*/
            return zdjecie.Duze;
        }

        /*public void Zoom(double zoom) {
            this.pictureBoxImage = new Bitmap(this.rescueBitmap, (int)(rescueBitmap.Width * zoom), (int)(rescueBitmap.Height * zoom));
            this.checkImagePosition();
        }*/

        /*public void Crop()
        {
            if (selectedRectangle.Width != 0 && selectedRectangle.Height != 0)
            {
                this.DrawMyRectangle(selectedRectangle);
                if (selectedRectangle.Width < 0)
                {
                    selectedRectangle.X +=  selectedRectangle.Width;
                    selectedRectangle.Width *= -1;
                }
                if (selectedRectangle.Height < 0)
                {
                    selectedRectangle.Y += selectedRectangle.Height;
                    selectedRectangle.Height *= -1;
                }
                Bitmap cropped = new Bitmap(Math.Abs(selectedRectangle.Width), Math.Abs(selectedRectangle.Height), this.pictureBoxImage.PixelFormat);
                Graphics g = Graphics.FromImage(cropped);
                g.DrawImage(this.pictureBoxImage, new Rectangle(0, 0, cropped.Width, cropped.Height), selectedRectangle, GraphicsUnit.Pixel);
                g.Dispose();
                this.setImage(cropped);
            }
        }*/

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

        private void DrawMyRectangle(Rectangle r)
        {
            this.data = this.pictureBoxImage.LockBits(new Rectangle(0, 0, this.pictureBoxImage.Width, this.pictureBoxImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            DrawMyLine(new Point(r.X, r.Y), new Point(r.X + r.Width, r.Y));
            DrawMyLine(new Point(r.X, r.Y), new Point(r.X, r.Y + r.Height));
            DrawMyLine(new Point(r.X + r.Width, r.Y), new Point(r.X + r.Width, r.Y + r.Height));
            DrawMyLine(new Point(r.X, r.Y + r.Height), new Point(r.X + r.Width, r.Y + r.Height));
            XorPixel(r.X, r.Y, Color.Gray);
            XorPixel(r.X + r.Width, r.Y + r.Height, Color.Gray);
            this.pictureBoxImage.UnlockBits(data);
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
                zdjecie.Zaznaczenie = selectedRectangle;

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
            throw new Exception("The method or operation is not implemented.");
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

        public void Wypelnij(IZdjecie[] zdjecia)
        {
            if (zdjecia.Length != 0)
            {
                this.zdjecie = (Zdjecie)zdjecia[0];
                this.zdjecieZapas = (Zdjecie)zdjecia[0];
                this.pictureBoxImage = FitToPage();
                this.checkImagePosition();
                this.lmStartingPoint = new Point();
                this.selectedRectangle = new Rectangle(0, 0, 0, 0);
                zdjecie.Zaznaczenie = selectedRectangle;
                this.Refresh();
            }
        }

        #endregion
    }
}
