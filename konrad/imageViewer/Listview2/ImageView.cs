using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
    }
}
