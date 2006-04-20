using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Photo
{
    public partial class ImageViewer : UserControl
    {
        public ImageViewer()
        {
            InitializeComponent();
        }
        public void SetThumbnailView()
        {
            thumbnailView1.Visible = true;
            imageView1.Visible = false;
        }
        public void SetImageView()
        {
            imageView1.Visible = true;
            thumbnailView1.Visible = false;
        }
        public ThumbnailView Thumbnailview
        {
            get
            {
                return thumbnailView1;
            }
        }
        public ImageView Imageview
        {
            get
            {
                return imageView1;
            }
        }

        public void Zoom(double zoom)
        {
            if (imageView1.Visible == true)
            {
                imageView1.Zoom(zoom);
            }
            else if (thumbnailView1.Visible == true)
            {
                thumbnailView1.ShowImages(zoom);
            }
        }

        public void Crop()
        {
            if (imageView1.Visible == true)
            {
                imageView1.Crop();
            }
        }

        public void toGrayScale()
        {
            if (imageView1.Visible == true)
            {
                imageView1.toGrayScale();
            }
        }

        private void mouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem listViewItem = ((ThumbnailView)sender).FocusedItem;
            Bitmap b = (Bitmap)thumbnailView1.Images[listViewItem.ImageIndex];
            this.imageView1.setImage(new Bitmap(((ThumbnailTag)b.Tag).Path));
            this.SetImageView();
        }
    }
}
