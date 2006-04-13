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
                if (this.Size.Width < Image.Width)
                {
                    this.pictureBox1.Width = Image.Width;
                    x = 3;
                }
                else
                {

                    x = (this.Width - Image.Width) / 2;
                    this.pictureBox1.Width = Image.Width;
                }
                if (this.Size.Height < Image.Height)
                {
                    this.pictureBox1.Height = Image.Height;
                    y = 3;
                }
                else
                {
                    y = (this.Height - Image.Height) / 2;
                    this.pictureBox1.Height = Image.Height;
                }
                this.pictureBox1.Location = new Point(x, y);
            }
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
