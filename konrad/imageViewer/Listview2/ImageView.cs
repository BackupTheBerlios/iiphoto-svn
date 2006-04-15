using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

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
        }

        public Image Image
        {
            get
            {
                return this.pictureBox1.Image;
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
            Rectangle r = new Rectangle(lmStartingPoint.X, lmStartingPoint.Y, lmEndPoint.X - lmStartingPoint.X, lmEndPoint.Y - lmStartingPoint.Y);
            Bitmap cropped = new Bitmap(r.Width, r.Height, this.Image.PixelFormat);
            Graphics g = Graphics.FromImage(cropped);
            g.DrawImage(this.Image, new Rectangle(0, 0, cropped.Width, cropped.Height), r, GraphicsUnit.Pixel);
            g.Dispose();
            this.openImage(cropped);
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
                selectedRectangle = this.pictureBox1.RectangleToScreen(new Rectangle(lmStartingPoint.X, lmStartingPoint.Y, lmEndPoint.X - lmStartingPoint.X, lmEndPoint.Y - lmStartingPoint.Y));
                ControlPaint.DrawReversibleFrame(selectedRectangle, this.BackColor, FrameStyle.Dashed);
                isDrag = true;
                moving = false;
                this.lmStartingPoint = new Point(e.X, e.Y);
            }
        }

        private void onMouseUp(object sender, MouseEventArgs e)
        {
            isDrag = false;
            if (e.Button == MouseButtons.Left)
            {
                this.lmEndPoint.X = e.X;
                this.lmEndPoint.Y = e.Y;
            }
        }

        private void onMouseMove(object sender, MouseEventArgs e)
        {
            if (isDrag)
            {
                int maxX, maxY;

                // Calculate the endpoint and dimensions for the new 
                // rectangle, again using the PointToScreen method.
                
                /*if (e.X > this.pictureBox1.Width)
                    maxX = this.pictureBox1.Width;
                else
                    maxX = e.X;
                if (e.Y > this.pictureBox1.Height)
                    maxY = this.pictureBox1.Height;
                else
                    maxY = e.Y;*/
                if (moving == true)
                    ControlPaint.DrawReversibleFrame(selectedRectangle, this.BackColor, FrameStyle.Dashed);
                else
                    moving = true;

                selectedRectangle = this.pictureBox1.RectangleToScreen(new Rectangle(lmStartingPoint.X, lmStartingPoint.Y, e.X - lmStartingPoint.X, e.Y - lmStartingPoint.Y));
                //selectedRectangle = new Rectangle(lmStartingPoint.X, lmStartingPoint.Y, e.X - lmStartingPoint.X, e.Y - lmStartingPoint.Y);
                // Draw the new rectangle by calling DrawReversibleFrame
                // again.  
                ControlPaint.DrawReversibleFrame(selectedRectangle, this.BackColor, FrameStyle.Dashed);
            }
        }

        private void onPaint(object sender, PaintEventArgs e)
        {
            //if (isDrag == false)
            //Rectangle r = new Rectangle(lmStartingPoint.X, lmStartingPoint.Y, lmEndPoint.X - lmStartingPoint.X, lmEndPoint.Y - lmStartingPoint.Y);
            //Rectangle r =  this.pictureBox1.RectangleToScreen(new Rectangle(lmStartingPoint.X, lmStartingPoint.Y, lmEndPoint.X - lmStartingPoint.X, lmEndPoint.Y - lmStartingPoint.Y));
            //ControlPaint.DrawReversibleFrame(r, this.BackColor, FrameStyle.Dashed);
            //Console.WriteLine("onpaint" + r.ToString());
        }
    }
}
