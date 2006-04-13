using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Listview2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            imageViewer1.SetThumbnailView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            imageViewer1.SetThumbnailView();
            List<Bitmap> images = new List<Bitmap>();
            Bitmap tempBitmap;

            for (int i = 1; i < 11; i++)
            {
                tempBitmap = new Bitmap(@"img\\small_img" + i.ToString() + ".jpg");
                tempBitmap.Tag = new ThumbnailTag(@"img\\img" + i.ToString() + ".jpg");
                images.Add(tempBitmap);
            }
            imageViewer1.Thumbnailview.AddImages(images);
            imageViewer1.Thumbnailview.ShowImages(1.0);
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "50%": imageViewer1.Thumbnailview.ShowImages(0.5); break;
                case "75%": imageViewer1.Thumbnailview.ShowImages(0.75); break;
                case "100%": imageViewer1.Thumbnailview.ShowImages(1.0); break;
                case "150%": imageViewer1.Thumbnailview.ShowImages(1.5); break;
                default: break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            imageViewer1.SetThumbnailView();
        }
    }
}