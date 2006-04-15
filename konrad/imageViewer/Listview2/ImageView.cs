using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Listview2
{
    public partial class ImageView : UserControl
    {
        public ImageView()
        {
            InitializeComponent();
        }

        public void openImage(Bitmap b)
        {
            this.Image = b;
            this.rescueBitmap = b;
            this.checkImagePosition();
            this.lmStartingPoint = new Point();
            this.lmEndPoint = new Point();
            this.selectedRectangle = new Rectangle(0, 0, 0, 0);
            this.clearRect = false;
        }

        public Bitmap Image
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

        private void checkImagePosition() {
            if (this.Image != null)
            {
                int x, y;
                if (this.Width < Image.Width)
                {
                    x = 0;
                }
                else
                {
                    x = ((this.Width - Image.Width) / 2) ;
                    if (x < 0)
                        MessageBox.Show("test");
                }
                if (this.Height < Image.Height)
                {
                    y = 0;
                }
                else
                {
                    y = ((this.Height - Image.Height) / 2) ;
                    if (y < 0)
                        MessageBox.Show("test");
                }
                this.pictureBox1.Width = Image.Width;
                this.pictureBox1.Height = Image.Height;
                this.pictureBox1.Location = new Point(x+3, y+3);
            }
        }

        public void Zoom(double zoom) {
            this.Image = new Bitmap(this.rescueBitmap, (int)(rescueBitmap.Width * zoom), (int)(rescueBitmap.Height * zoom));
            this.checkImagePosition();
        }

        public void Crop()
        {
            if (selectedRectangle.Width != 0 && selectedRectangle.Height != 0)
            {
                this.DrawMyRectangle(selectedRectangle);

                Bitmap cropped = new Bitmap(selectedRectangle.Width, selectedRectangle.Height, this.Image.PixelFormat);
                Graphics g = Graphics.FromImage(cropped);
                g.DrawImage(this.Image, new Rectangle(0, 0, cropped.Width, cropped.Height), selectedRectangle, GraphicsUnit.Pixel);
                g.Dispose();
                this.openImage(cropped);
            }
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
                *(imgPtr++) = c.B;
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
            this.data = this.Image.LockBits(new Rectangle(0, 0, this.Image.Width, this.Image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            DrawMyLine(new Point(r.X, r.Y), new Point(r.X + r.Width, r.Y));
            DrawMyLine(new Point(r.X, r.Y), new Point(r.X, r.Y + r.Height));
            DrawMyLine(new Point(r.X + r.Width, r.Y), new Point(r.X + r.Width, r.Y + r.Height));
            DrawMyLine(new Point(r.X, r.Y + r.Height), new Point(r.X + r.Width, r.Y + r.Height));
            XorPixel(r.X, r.Y, Color.Gray);
            XorPixel(r.X + r.Width, r.Y + r.Height, Color.Gray);
            this.Image.UnlockBits(data);
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
        }

        private void onMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                /*if (clearRect)
                {
                    this.DrawMyRectangle(selectedRectangle);
                    clearRect = false;
                }*/
                isDrag = true;
                moving = false;
                this.lmStartingPoint = new Point(e.X, e.Y);
                this.Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {
                isDrag = true;
                this.rmStartingPoint = new Point(e.X, e.Y);
            }
        }

        private void onMouseUp(object sender, MouseEventArgs e)
        {
            
            isDrag = false;
            if (e.Button == MouseButtons.Left)
            {
                //this.lmEndPoint = new Point(e.X, e.Y);
                this.Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {
                this.rmEndPoint = new Point(e.X, e.Y);
                this.DrawMyRectangle(new Rectangle(rmStartingPoint.X, rmStartingPoint.Y, rmEndPoint.X - rmStartingPoint.X, rmEndPoint.Y - rmStartingPoint.Y));
                this.Refresh();
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
                        maxX = this.pictureBox1.Width - 1;
                    else if (e.X < 0)
                        maxX = 0;
                    else
                        maxX = e.X;
                    if (e.Y >= this.pictureBox1.Height)
                        maxY = this.pictureBox1.Height - 1;
                    else if (e.Y < 0)
                        maxY = 0;
                    else
                        maxY = e.Y;

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
    }
}
