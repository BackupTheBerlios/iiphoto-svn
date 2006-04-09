using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;
using  System.Text;

namespace yapv
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class imageWindow : Form
	{
		private Bitmap image;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel panel1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public imageWindow(string fileName)
		{
			image = new Bitmap(fileName);
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			// TODO: Add any initialization after the InitComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
				image.Dispose();
				pictureBox1.Dispose();
				panel1.Dispose();
			}
			base.Dispose( disposing );
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			pictureBox1.Size = new System.Drawing.Size(image.Width, image.Height);
			pictureBox1.Image = image;
		}

		public Bitmap Image {
			get
			{
				return this.image;
			}
		}
		
		public void rotateImage(int angle) {
			switch(angle)
			{
				case 90: image.RotateFlip( RotateFlipType.Rotate90FlipNone);break;
				case 180: image.RotateFlip( RotateFlipType.Rotate180FlipNone);break;
				case 270: image.RotateFlip( RotateFlipType.Rotate270FlipNone);break;
			}
			this.Invalidate();
		}

		public void flipImage(char c) 
		{
			switch(c)
			{
				case 'h': image.RotateFlip( RotateFlipType.RotateNoneFlipY);break;
				case 'v': image.RotateFlip( RotateFlipType.RotateNoneFlipX);break;
			}
			this.Invalidate();
		}

		public void toGrayScale() {
			Color color;
			for(int i=0;i<image.Size.Height; i++) 
			{
				for(int j=0;j<image.Size.Width;j++)
				{
					color = image.GetPixel(j,i);
					image.SetPixel(j,i, Color.FromArgb((color.R+color.G+color.B)/3,
													   (color.R+color.G+color.B)/3,
													   (color.R+color.G+color.B)/3));
				}
			}
			this.Invalidate();
		}

		public void resize( double x) {
			this.image = new Bitmap(this.image, (int)(image.Width * x), (int)(image.Height * x));			this.Invalidate();
		}

		public void negative() 
		{
			Color color;
			for(int i=0;i<image.Size.Height; i++) 
			{
				for(int j=0;j<image.Size.Width;j++)
				{
					color = image.GetPixel(j,i);
					image.SetPixel(j,i, Color.FromArgb( 255 - color.R, 255 - color.G, 255 - color.B));
				}
			}
			this.Invalidate();
		}

        public void showProperty(string propItem) 
        {
            PropertyItem i;
            try
            {
                string val;
                i = image.GetPropertyItem(int.Parse(propItem));
                switch (i.Type) {
                    case 1: val = Encoding.Unicode.GetString(i.Value); break;
                    case 2: val = Encoding.ASCII.GetString(i.Value); break;
                    default: val = "Value not supported"; break;
                }
                MessageBox.Show(val);
            }
            catch {
                MessageBox.Show("Nie znaleziono takiej Property");
            }  
        }
        public void setProperty(string value)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            PropertyItem propItem = image.PropertyItems[0];
            propItem.Id = PropertyTags.IIPhotoTag;
            propItem.Type = 2;
            propItem.Value = encoding.GetBytes(value);
            propItem.Len = propItem.Value.Length;
            image.SetPropertyItem(propItem);
            MessageBox.Show("Podane Property zostalo ustawione");
        }
		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(200, 168);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(520, 286);
			this.panel1.TabIndex = 1;
			// 
			// imageWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(520, 286);
			this.Controls.Add(this.panel1);
			this.Name = "imageWindow";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion	
	}
}
