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
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStripOperacje = new System.Windows.Forms.ToolStrip();
            this.Alfa = new System.Windows.Forms.ToolStripButton();
            this.Omega = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.programToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zakonczToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filtryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.przegladarkaZdjec = new Photo.PrzegladarkaZdjec();
            this.informacjeControl1 = new Photo.InformacjeControl();
            this.drzewoOpakowane1 = new Photo.DrzewoOpakowane();
            this.wyszukiwarkaControl1 = new Photo.WyszukiwarkaControl();
            this.listaAlbumowControl = new Photo.ListaAlbumowControl();
            this.statusStrip = new Photo.PasekStanuControl();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStripOperacje.SuspendLayout();
            this.menuStrip1.SuspendLayout();
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
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(951, 620);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(951, 645);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(812, 49);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(1, 549);
            this.splitter2.TabIndex = 7;
            this.splitter2.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(200, 49);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1, 549);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.drzewoOpakowane1);
            this.panel1.Controls.Add(this.wyszukiwarkaControl1);
            this.panel1.Controls.Add(this.listaAlbumowControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 549);
            this.panel1.TabIndex = 4;
            // 
            // toolStripOperacje
            // 
            this.toolStripOperacje.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Alfa,
            this.Omega,
            this.toolStripSeparator1});
            this.toolStripOperacje.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripOperacje.Location = new System.Drawing.Point(0, 24);
            this.toolStripOperacje.Name = "toolStripOperacje";
            this.toolStripOperacje.Size = new System.Drawing.Size(951, 25);
            this.toolStripOperacje.TabIndex = 1;
            this.toolStripOperacje.Text = "toolStrip1";
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
            this.programToolStripMenuItem,
            this.fileToolStripMenuItem,
            this.filtryToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(951, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // programToolStripMenuItem
            // 
            this.programToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zakonczToolStripMenuItem});
            this.programToolStripMenuItem.Name = "programToolStripMenuItem";
            this.programToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.programToolStripMenuItem.Text = "Program";
            // 
            // zakonczToolStripMenuItem
            // 
            this.zakonczToolStripMenuItem.Name = "zakonczToolStripMenuItem";
            this.zakonczToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.zakonczToolStripMenuItem.Text = "Zakoncz";
            this.zakonczToolStripMenuItem.Click += new System.EventHandler(this.zakonczToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(34, 20);
            this.fileToolStripMenuItem.Text = "Plik";
            // 
            // filtryToolStripMenuItem
            // 
            this.filtryToolStripMenuItem.Name = "filtryToolStripMenuItem";
            this.filtryToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.filtryToolStripMenuItem.Text = "Filtry";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.helpToolStripMenuItem.Text = "Pomoc";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // przegladarkaZdjec
            // 
            this.przegladarkaZdjec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.przegladarkaZdjec.Location = new System.Drawing.Point(201, 49);
            this.przegladarkaZdjec.Name = "przegladarkaZdjec";
            this.przegladarkaZdjec.Size = new System.Drawing.Size(611, 549);
            this.przegladarkaZdjec.TabIndex = 8;
            // 
            // informacjeControl1
            // 
            this.informacjeControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.informacjeControl1.Location = new System.Drawing.Point(813, 49);
            this.informacjeControl1.Name = "informacjeControl1";
            this.informacjeControl1.Size = new System.Drawing.Size(138, 549);
            this.informacjeControl1.TabIndex = 6;
            // 
            // drzewoOpakowane1
            // 
            this.drzewoOpakowane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drzewoOpakowane1.Location = new System.Drawing.Point(0, 226);
            this.drzewoOpakowane1.Name = "drzewoOpakowane1";
            this.drzewoOpakowane1.Size = new System.Drawing.Size(200, 323);
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
            this.statusStrip.Location = new System.Drawing.Point(0, 598);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(951, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            // 
            // PhotoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 645);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PhotoForm";
            this.Text = "IIPhoto";
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.toolStripOperacje.ResumeLayout(false);
            this.toolStripOperacje.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Photo.PasekStanuControl statusStrip;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStripOperacje;
        private System.Windows.Forms.ToolStripButton Alfa;
        private System.Windows.Forms.ToolStripButton Omega;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
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
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filtryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem programToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zakonczToolStripMenuItem;

    }
}

