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
                isDrag = true;
                this.lmStartingPoint = ((Control)sender).PointToScreen(new Point(e.X, e.Y));
                Graphics.FromImage(this.Image).FillEllipse(Brushes.Green, e.X, e.Y, 3, 3);
            }
            Console.WriteLine("mouseDown");
        }

        private void onMouseUp(object sender, MouseEventArgs e)
        {
            isDrag = false;
            if (e.Button == MouseButtons.Left)
            {
                //ControlPaint.DrawReversibleFrame(selectedRectangle, this.BackColor, FrameStyle.Dashed);
            }
        }

        private void onMouseMove(object sender, MouseEventArgs e)
        {
            if (isDrag)
            {
                ControlPaint.DrawReversibleFrame(selectedRectangle , this.BackColor, FrameStyle.Dashed);

                // Calculate the endpoint and dimensions for the new 
                // rectangle, again using the PointToScreen method.
                Point endPoint = ((Control)sender).PointToScreen(new Point(e.X, e.Y));
                int width = endPoint.X - lmStartingPoint.X;
                int height = endPoint.Y - lmStartingPoint.Y;
                selectedRectangle = new Rectangle(lmStartingPoint.X, lmStartingPoint.Y, width, height);
                //selectedRectangle = new Rectangle(lmStartingPoint.X, lmStartingPoint.Y, e.X - lmStartingPoint.X, e.Y - lmStartingPoint.Y);
                Graphics.FromImage(this.Image).FillEllipse(Brushes.Blue, e.X, e.Y, 3, 3);
                // Draw the new rectangle by calling DrawReversibleFrame
                // again.  
                ControlPaint.DrawReversibleFrame(selectedRectangle, this.BackColor, FrameStyle.Dashed);
            }
        }
    }
}
