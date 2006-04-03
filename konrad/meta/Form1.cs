using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace yapv
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.MenuItem menuItem21;
        private System.Windows.Forms.MenuItem menuItem22;
        private MenuItem menuItem23;
        private MenuItem menuItem24;
        private IContainer components;


		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();


			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.menuItem18 = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.menuItem20 = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.menuItem22 = new System.Windows.Forms.MenuItem();
            this.menuItem23 = new System.Windows.Forms.MenuItem();
            this.menuItem24 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem3});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem5,
            this.menuItem10,
            this.menuItem9,
            this.menuItem6,
            this.menuItem4});
            this.menuItem1.Text = "File";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 0;
            this.menuItem5.Text = "Open";
            this.menuItem5.Click += new System.EventHandler(this.openFile);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 1;
            this.menuItem10.Text = "Save";
            this.menuItem10.Click += new System.EventHandler(this.saveAs);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 2;
            this.menuItem9.Text = "Save as..";
            this.menuItem9.Click += new System.EventHandler(this.saveAs);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 3;
            this.menuItem6.Text = "-";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 4;
            this.menuItem4.Text = "Exit";
            this.menuItem4.Click += new System.EventHandler(this.closeApp);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem11,
            this.menuItem16,
            this.menuItem19,
            this.menuItem15,
            this.menuItem22,
            this.menuItem23,
            this.menuItem24});
            this.menuItem2.Text = "Image";
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 0;
            this.menuItem11.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem12,
            this.menuItem13,
            this.menuItem14});
            this.menuItem11.Text = "Rotate";
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 0;
            this.menuItem12.Text = "90o CW";
            this.menuItem12.Click += new System.EventHandler(this.menuItem12_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 1;
            this.menuItem13.Text = "180o CW";
            this.menuItem13.Click += new System.EventHandler(this.menuItem13_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 2;
            this.menuItem14.Text = "270o CW";
            this.menuItem14.Click += new System.EventHandler(this.menuItem14_Click);
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 1;
            this.menuItem16.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem17,
            this.menuItem18});
            this.menuItem16.Text = "Flip";
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 0;
            this.menuItem17.Text = "Horizontaly";
            this.menuItem17.Click += new System.EventHandler(this.menuItem17_Click);
            // 
            // menuItem18
            // 
            this.menuItem18.Index = 1;
            this.menuItem18.Text = "Verticaly";
            this.menuItem18.Click += new System.EventHandler(this.menuItem18_Click);
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 2;
            this.menuItem19.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem20,
            this.menuItem21});
            this.menuItem19.Text = "Resize";
            // 
            // menuItem20
            // 
            this.menuItem20.Index = 0;
            this.menuItem20.Text = "x 2";
            this.menuItem20.Click += new System.EventHandler(this.bigger2);
            // 
            // menuItem21
            // 
            this.menuItem21.Index = 1;
            this.menuItem21.Text = "/ 2";
            this.menuItem21.Click += new System.EventHandler(this.smaller2);
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 3;
            this.menuItem15.Text = "GrayScale";
            this.menuItem15.Click += new System.EventHandler(this.menuItem15_Click);
            // 
            // menuItem22
            // 
            this.menuItem22.Index = 4;
            this.menuItem22.Text = "Negative";
            this.menuItem22.Click += new System.EventHandler(this.negative);
            // 
            // menuItem23
            // 
            this.menuItem23.Index = 5;
            this.menuItem23.Text = "Set Property";
            this.menuItem23.Click += new System.EventHandler(this.menuItem23_Click);
            // 
            // menuItem24
            // 
            this.menuItem24.Index = 6;
            this.menuItem24.Text = "Show Property";
            this.menuItem24.Click += new System.EventHandler(this.menuItem24_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem7,
            this.menuItem8});
            this.menuItem3.Text = "Help";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 0;
            this.menuItem7.Text = "Help";
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 1;
            this.menuItem8.Text = "About";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(960, 606);
            this.IsMdiContainer = true;
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "Yet Another Picture Viewer";
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void closeApp(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void openFile(object sender, System.EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JPEG Images (*.jpg,*.jpeg)|*.jpg;*.jpeg|Gif Images (*.gif)|*.gif|Bitmaps (*.bmp)|*.bmp|Png Images (*.png)|*.png|Tiff Images(*.tif)|*.tif";
			if (ofd.ShowDialog() == DialogResult.OK) 
			{
				imageWindow imageW = new imageWindow(ofd.FileName);
				imageW.MdiParent = this;
				imageW.Text = ofd.FileName;
				imageW.Show();
			}

		}

		private void saveAs(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null) 
			{
				SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "JPEG Images (*.jpg,*.jpeg)|*.jpg;*.jpeg|Gif Images (*.gif)|*.gif|Bitmaps (*.bmp)|*.bmp|Png Images (*.png)|*.png|Tiff Images(*.tif)|*.tif";
				imageWindow activeImage = ((imageWindow)this.ActiveMdiChild);
				/*			Bitmap i = activeImage.Image;
							i.Save("c:\test.jpg",ImageFormat.Jpeg);*/
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					string strImgName = sfd.FileName;
					if (strImgName.EndsWith("jpg"))
						activeImage.Image.Save(strImgName,ImageFormat.Jpeg);
					else if (strImgName.EndsWith("gif"))
						activeImage.Image.Save(strImgName,ImageFormat.Gif);
					else if (strImgName.EndsWith("bmp"))
						activeImage.Image.Save(strImgName,ImageFormat.Bmp);
                    else if (strImgName.EndsWith("png"))
                        activeImage.Image.Save(strImgName,ImageFormat.Png);
                    else if (strImgName.EndsWith("tif"))
                        activeImage.Image.Save(strImgName, ImageFormat.Tiff);
				}
			}
		}

		private void menuItem14_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
				((imageWindow)this.ActiveMdiChild).rotateImage(270);
		}

		private void menuItem13_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
				((imageWindow)this.ActiveMdiChild).rotateImage(180);
		}

		private void menuItem12_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
				((imageWindow)this.ActiveMdiChild).rotateImage(90);
		}

		private void menuItem15_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
				((imageWindow)this.ActiveMdiChild).toGrayScale();
		}

		private void menuItem17_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
				((imageWindow)this.ActiveMdiChild).flipImage('h');
		}

		private void menuItem18_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
				((imageWindow)this.ActiveMdiChild).flipImage('v');
		}

		private void bigger2(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
				((imageWindow)this.ActiveMdiChild).resize(2);
		}

		private void smaller2(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
				((imageWindow)this.ActiveMdiChild).resize(0.5);
		}

		private void negative(object sender, System.EventArgs e)
		{
			if (this.ActiveMdiChild != null)
				((imageWindow)this.ActiveMdiChild).negative();
		}

        private void menuItem23_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                SetProp sp = new SetProp((imageWindow)this.ActiveMdiChild);
                sp.Show();
            }
        }

        private void menuItem24_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                ShowProp sp = new ShowProp((imageWindow)this.ActiveMdiChild);
                sp.Show();
            }
        }
	}
}
