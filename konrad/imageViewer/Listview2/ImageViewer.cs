using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Listview2
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

        private void mouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem listViewItem = ((ThumbnailView)sender).FocusedItem;
            Bitmap b = (Bitmap)thumbnailView1.Images[listViewItem.ImageIndex];
            this.imageView1.Image = new Bitmap(((ThumbnailTag)b.Tag).Path);
            this.SetImageView();
        }
    }
}
