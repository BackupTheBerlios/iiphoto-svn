namespace Photo
{
    partial class PhotoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhotoForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.informacjeControl1 = new Photo.InformacjeControl();
            this.przegladarkaZdjec = new Photo.PrzegladarkaZdjec();
            this.drzewoOpakowane1 = new Photo.DrzewoOpakowane();
            this.wyszukiwarkaControl1 = new Photo.WyszukiwarkaControl();
            this.listaAlbumowControl = new Photo.ListaAlbumowControl();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 506);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(986, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(986, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.drzewoOpakowane1);
            this.panel1.Controls.Add(this.wyszukiwarkaControl1);
            this.panel1.Controls.Add(this.listaAlbumowControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 482);
            this.panel1.TabIndex = 2;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(200, 24);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 482);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(705, 24);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 482);
            this.splitter2.TabIndex = 6;
            this.splitter2.TabStop = false;
            // 
            // informacjeControl1
            // 
            this.informacjeControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.informacjeControl1.Location = new System.Drawing.Point(708, 24);
            this.informacjeControl1.Name = "informacjeControl1";
            this.informacjeControl1.Size = new System.Drawing.Size(278, 482);
            this.informacjeControl1.TabIndex = 5;
            // 
            // przegladarkaZdjec
            // 
            this.przegladarkaZdjec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.przegladarkaZdjec.Location = new System.Drawing.Point(203, 24);
            this.przegladarkaZdjec.Name = "przegladarkaZdjec";
            this.przegladarkaZdjec.Size = new System.Drawing.Size(783, 482);
            this.przegladarkaZdjec.TabIndex = 4;
            // 
            // drzewoOpakowane1
            // 
            this.drzewoOpakowane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drzewoOpakowane1.Location = new System.Drawing.Point(0, 167);
            this.drzewoOpakowane1.Name = "drzewoOpakowane1";
            this.drzewoOpakowane1.Size = new System.Drawing.Size(200, 315);
            this.drzewoOpakowane1.TabIndex = 8;
            // 
            // wyszukiwarkaControl1
            // 
            this.wyszukiwarkaControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.wyszukiwarkaControl1.Location = new System.Drawing.Point(0, 85);
            this.wyszukiwarkaControl1.Name = "wyszukiwarkaControl1";
            this.wyszukiwarkaControl1.Size = new System.Drawing.Size(200, 82);
            this.wyszukiwarkaControl1.TabIndex = 7;
            // 
            // listaAlbumowControl
            // 
            this.listaAlbumowControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.listaAlbumowControl.Location = new System.Drawing.Point(0, 0);
            this.listaAlbumowControl.Name = "listaAlbumowControl";
            this.listaAlbumowControl.Size = new System.Drawing.Size(200, 85);
            this.listaAlbumowControl.TabIndex = 7;
            // 
            // PhotoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 528);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.informacjeControl1);
            this.Controls.Add(this.przegladarkaZdjec);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PhotoForm";
            this.Text = "IIPhoto";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private PrzegladarkaZdjec przegladarkaZdjec;
        private WyszukiwarkaControl wyszukiwarkaControl1;
        private ListaAlbumowControl listaAlbumowControl;
        private InformacjeControl informacjeControl1;
        private System.Windows.Forms.Splitter splitter2;
        private DrzewoOpakowane drzewoOpakowane1;

    }
}

