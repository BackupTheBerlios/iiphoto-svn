using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Photo
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
            List<Miniatura> images = new List<Miniatura>();
            Miniatura tempMini;

            for (int i = 1; i < 11; i++)
            {
                tempMini = new Miniatura(@"img\\small_img" + i.ToString() + ".jpg");
                tempMini.tag = new MiniaturaTag(@"img\\img" + i.ToString() + ".jpg");
                images.Add(tempMini);
            }
            imageViewer1.Thumbnailview.AddImages(images);
            imageViewer1.Thumbnailview.ShowImages(1.0);
            
        }

        /*private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "50%": imageViewer1.Zoom(0.5); break;
                case "75%": imageViewer1.Zoom(0.75); break;
                case "100%": imageViewer1.Zoom(1.0); break;
                case "150%": imageViewer1.Zoom(1.5); break;
                default: break;
            }
        }*/

        private void button2_Click(object sender, EventArgs e)
        {
            imageViewer1.SetThumbnailView();
        }

        /*private void button3_Click(object sender, EventArgs e)
        {
            imageViewer1.Crop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            imageViewer1.toGrayScale();
        }*/
    }
}