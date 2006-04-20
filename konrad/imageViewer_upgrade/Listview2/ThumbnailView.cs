using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Photo
{
    public class ThumbnailView : System.Windows.Forms.ListView
    {
        private int defaultImageSize;
        private List<Bitmap> images;

        public ThumbnailView()
        {
            images = new List<Bitmap>();
            //Activate double buffering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            this.defaultImageSize = 120;
        }
        public ThumbnailView(int imgSize)
            : this()
        {
            this.defaultImageSize = imgSize;
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        public void AddImages(List<Bitmap> images) {
            this.images.AddRange(images);
        }

        public List<Bitmap> Images
        {
            get
            {
                return images;
            }
        }

        public void ShowImages(double zoom) {
            Bitmap b, newBitmap;
            Graphics MyGraphics;
            Rectangle MyRectan;
            ImageList newImageList = new ImageList();
            ListViewItem listViewItem;
            newImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            newImageList.Tag = "100%";
            newImageList.TransparentColor = System.Drawing.Color.Transparent;
            newImageList.ImageSize = new Size((int)(zoom * this.defaultImageSize), (int)(zoom * this.defaultImageSize));

            int maxSize = newImageList.ImageSize.Width;
            int scaledH, scaledW, posX, posY;
            this.Items.Clear();
            for (int i = 0; i < images.Count; i++)
            {
                b = images[i];
                newBitmap = new Bitmap(maxSize, maxSize);
                MyGraphics = Graphics.FromImage(newBitmap);

                if (b.Height > b.Width)
                {
                    scaledH = maxSize;
                    scaledW = (int)Math.Round((double)(b.Width * scaledH) / b.Height);
                    posX = (maxSize - scaledW) / 2;
                    posY = 0;
                }
                else
                {
                    scaledW = maxSize;
                    scaledH = (int)Math.Round((double)(b.Height * scaledW) / b.Width);
                    posX = 0;
                    posY = (maxSize - scaledH) / 2;
                }

                MyRectan = new Rectangle(posX, posY, scaledW, scaledH);
                using (Pen p = new Pen(Brushes.LightGray))
                {
                    MyGraphics.DrawRectangle(p, 0, 0, maxSize - 1, maxSize - 1);
                }
                MyGraphics.DrawImage(b, MyRectan);
                newBitmap.Tag = new ThumbnailTag(((ThumbnailTag)b.Tag).Filename);
                newImageList.Images.Add(newBitmap);
                listViewItem = this.Items.Add(new ListViewItem(((ThumbnailTag)b.Tag).Filename));
                listViewItem.ImageIndex = i;
            }

            // Create an new ImageList for the ListView control
            this.LargeImageList = newImageList;

            // Update the ListView control
            this.Refresh();
        }
    }
}
