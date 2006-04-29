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
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStripOperacje = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Alfa = new System.Windows.Forms.ToolStripButton();
            this.Omega = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.przegladarkaZdjec = new Photo.PrzegladarkaZdjec();
            this.informacjeControl1 = new Photo.InformacjeControl();
            this.drzewoOpakowane1 = new Photo.DrzewoOpakowane();
            this.wyszukiwarkaControl1 = new Photo.WyszukiwarkaControl();
            this.listaAlbumowControl = new Photo.ListaAlbumowControl();
            this.statusStrip = new Photo.PasekStanuControl();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStripOperacje.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.przegladarkaZdjec);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitter2);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.informacjeControl1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitter1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.statusStrip);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.toolStripOperacje);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.menuStrip1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1016, 562);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1016, 562);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripOperacje
            // 
            this.toolStripOperacje.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator2,
            this.Alfa,
            this.Omega,
            this.toolStripSeparator1});
            this.toolStripOperacje.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripOperacje.Location = new System.Drawing.Point(0, 24);
            this.toolStripOperacje.Name = "toolStripOperacje";
            this.toolStripOperacje.Size = new System.Drawing.Size(1016, 25);
            this.toolStripOperacje.TabIndex = 1;
            this.toolStripOperacje.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(81, 22);
            this.toolStripButton1.Text = "Widok Miniatur";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // Alfa
            // 
            this.Alfa.Image = ((System.Drawing.Image)(resources.GetObject("Alfa.Image")));
            this.Alfa.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Alfa.Name = "Alfa";
            this.Alfa.Size = new System.Drawing.Size(58, 22);
            this.Alfa.Text = "Edytuj";
            this.Alfa.Click += new System.EventHandler(this.Alfa_Click);
            // 
            // Omega
            // 
            this.Omega.Image = ((System.Drawing.Image)(resources.GetObject("Omega.Image")));
            this.Omega.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Omega.Name = "Omega";
            this.Omega.Size = new System.Drawing.Size(101, 22);
            this.Omega.Text = "Zakoncz Edycje";
            this.Omega.Click += new System.EventHandler(this.Omega_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1016, 24);
            this.menuStrip1.TabIndex = 3;
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
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 491);
            this.panel1.TabIndex = 4;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(200, 49);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1, 491);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(877, 49);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(1, 491);
            this.splitter2.TabIndex = 7;
            this.splitter2.TabStop = false;
            // 
            // przegladarkaZdjec
            // 
            this.przegladarkaZdjec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.przegladarkaZdjec.Location = new System.Drawing.Point(201, 49);
            this.przegladarkaZdjec.Name = "przegladarkaZdjec";
            this.przegladarkaZdjec.Size = new System.Drawing.Size(676, 491);
            this.przegladarkaZdjec.TabIndex = 8;
            // 
            // informacjeControl1
            // 
            this.informacjeControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.informacjeControl1.Location = new System.Drawing.Point(878, 49);
            this.informacjeControl1.Name = "informacjeControl1";
            this.informacjeControl1.Size = new System.Drawing.Size(138, 491);
            this.informacjeControl1.TabIndex = 6;
            // 
            // drzewoOpakowane1
            // 
            this.drzewoOpakowane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drzewoOpakowane1.Location = new System.Drawing.Point(0, 226);
            this.drzewoOpakowane1.Name = "drzewoOpakowane1";
            this.drzewoOpakowane1.Size = new System.Drawing.Size(200, 265);
            this.drzewoOpakowane1.TabIndex = 2;
            // 
            // wyszukiwarkaControl1
            // 
            this.wyszukiwarkaControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.wyszukiwarkaControl1.Location = new System.Drawing.Point(0, 144);
            this.wyszukiwarkaControl1.Name = "wyszukiwarkaControl1";
            this.wyszukiwarkaControl1.Size = new System.Drawing.Size(200, 82);
            this.wyszukiwarkaControl1.TabIndex = 1;
            // 
            // listaAlbumowControl
            // 
            this.listaAlbumowControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.listaAlbumowControl.Location = new System.Drawing.Point(0, 0);
            this.listaAlbumowControl.Name = "listaAlbumowControl";
            this.listaAlbumowControl.Size = new System.Drawing.Size(200, 144);
            this.listaAlbumowControl.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 540);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1016, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // PhotoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 562);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PhotoForm";
            this.Text = "IIPhoto";
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStripOperacje.ResumeLayout(false);
            this.toolStripOperacje.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Photo.PasekStanuControl statusStrip;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStripOperacje;
        private System.Windows.Forms.ToolStripButton Alfa;
        private System.Windows.Forms.ToolStripButton Omega;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private PrzegladarkaZdjec przegladarkaZdjec;
        private System.Windows.Forms.Splitter splitter2;
        private InformacjeControl informacjeControl1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel1;
        private WyszukiwarkaControl wyszukiwarkaControl1;
        private ListaAlbumowControl listaAlbumowControl;
        private DrzewoOpakowane drzewoOpakowane1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;

    }
}

