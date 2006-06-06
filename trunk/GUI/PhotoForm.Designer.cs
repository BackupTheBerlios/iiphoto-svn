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
            this.przegladarkaZdjec = new Photo.PrzegladarkaZdjec();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.informacjeControl1 = new Photo.InformacjeControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.drzewoOpakowane1 = new Photo.DrzewoOpakowane();
            this.wyszukiwarkaControl1 = new Photo.WyszukiwarkaControl();
            this.listaAlbumowControl = new Photo.ListaAlbumowControl();
            this.statusStrip = new Photo.PasekStanuControl();
            this.toolStripOperacje = new System.Windows.Forms.ToolStrip();
            this.zapiszButton1 = new System.Windows.Forms.ToolStripButton();
            this.zapiszAllButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Alfa = new System.Windows.Forms.ToolStripButton();
            this.Omega = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripComboBox2 = new System.Windows.Forms.ToolStripComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.programToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aktualizacjaBazyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zakonczToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zapiszToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.edycjaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wycofajZmianyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filtryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skrótyKlawiaturoweToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oznaczeniaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1024, 635);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1024, 660);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // przegladarkaZdjec
            // 
            this.przegladarkaZdjec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.przegladarkaZdjec.Location = new System.Drawing.Point(201, 49);
            this.przegladarkaZdjec.Name = "przegladarkaZdjec";
            this.przegladarkaZdjec.Size = new System.Drawing.Size(646, 563);
            this.przegladarkaZdjec.TabIndex = 8;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(847, 49);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(1, 563);
            this.splitter2.TabIndex = 7;
            this.splitter2.TabStop = false;
            // 
            // informacjeControl1
            // 
            this.informacjeControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.informacjeControl1.Location = new System.Drawing.Point(848, 49);
            this.informacjeControl1.Name = "informacjeControl1";
            this.informacjeControl1.Size = new System.Drawing.Size(176, 563);
            this.informacjeControl1.TabIndex = 6;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(200, 49);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1, 563);
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
            this.panel1.Size = new System.Drawing.Size(200, 563);
            this.panel1.TabIndex = 4;
            // 
            // drzewoOpakowane1
            // 
            this.drzewoOpakowane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drzewoOpakowane1.Location = new System.Drawing.Point(0, 234);
            this.drzewoOpakowane1.Name = "drzewoOpakowane1";
            this.drzewoOpakowane1.Size = new System.Drawing.Size(200, 329);
            this.drzewoOpakowane1.TabIndex = 2;
            // 
            // wyszukiwarkaControl1
            // 
            this.wyszukiwarkaControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.wyszukiwarkaControl1.Location = new System.Drawing.Point(0, 148);
            this.wyszukiwarkaControl1.Name = "wyszukiwarkaControl1";
            this.wyszukiwarkaControl1.Size = new System.Drawing.Size(200, 86);
            this.wyszukiwarkaControl1.TabIndex = 1;
            // 
            // listaAlbumowControl
            // 
            this.listaAlbumowControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.listaAlbumowControl.Location = new System.Drawing.Point(0, 0);
            this.listaAlbumowControl.Name = "listaAlbumowControl";
            this.listaAlbumowControl.Size = new System.Drawing.Size(200, 148);
            this.listaAlbumowControl.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 612);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1024, 23);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripOperacje
            // 
            this.toolStripOperacje.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zapiszButton1,
            this.zapiszAllButton,
            this.toolStripSeparator2,
            this.Alfa,
            this.Omega,
            this.toolStripSeparator1,
            this.toolStripComboBox1,
            this.toolStripComboBox2});
            this.toolStripOperacje.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripOperacje.Location = new System.Drawing.Point(0, 24);
            this.toolStripOperacje.Name = "toolStripOperacje";
            this.toolStripOperacje.Size = new System.Drawing.Size(1024, 25);
            this.toolStripOperacje.TabIndex = 1;
            this.toolStripOperacje.Text = "toolStrip1";
            // 
            // zapiszButton1
            // 
            this.zapiszButton1.Image = ((System.Drawing.Image)(resources.GetObject("zapiszButton1.Image")));
            this.zapiszButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zapiszButton1.Name = "zapiszButton1";
            this.zapiszButton1.Size = new System.Drawing.Size(57, 22);
            this.zapiszButton1.Text = "&Zapisz";
            this.zapiszButton1.Click += new System.EventHandler(this.zapiszButton1_Click);
            // 
            // zapiszAllButton
            // 
            this.zapiszAllButton.Image = ((System.Drawing.Image)(resources.GetObject("zapiszAllButton.Image")));
            this.zapiszAllButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zapiszAllButton.Name = "zapiszAllButton";
            this.zapiszAllButton.Size = new System.Drawing.Size(106, 22);
            this.zapiszAllButton.Text = "&Zapisz wszystkie";
            this.zapiszAllButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // Alfa
            // 
            this.Alfa.Image = global::Photo.Properties.Resources.edit_image;
            this.Alfa.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Alfa.Name = "Alfa";
            this.Alfa.Size = new System.Drawing.Size(113, 22);
            this.Alfa.Text = "Rozpocznij edycjê";
            this.Alfa.Click += new System.EventHandler(this.Alfa_Click);
            // 
            // Omega
            // 
            this.Omega.Image = global::Photo.Properties.Resources.edit_image_end;
            this.Omega.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Omega.Name = "Omega";
            this.Omega.Size = new System.Drawing.Size(101, 22);
            this.Omega.Text = "Zakoñcz edycjê";
            this.Omega.Click += new System.EventHandler(this.Omega_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "10%",
            "25%",
            "50%",
            "75%",
            "100%",
            "150%",
            "Do ekranu"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(80, 25);
            this.toolStripComboBox1.Text = "100%";
            this.toolStripComboBox1.Visible = false;
            this.toolStripComboBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripComboBox1_KeyPress);
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            // 
            // toolStripComboBox2
            // 
            this.toolStripComboBox2.Items.AddRange(new object[] {
            "Ma³e",
            "Œrednie",
            "Du¿e"});
            this.toolStripComboBox2.Name = "toolStripComboBox2";
            this.toolStripComboBox2.Size = new System.Drawing.Size(80, 25);
            this.toolStripComboBox2.Text = "Œrednie";
            this.toolStripComboBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripComboBox2_KeyPress);
            this.toolStripComboBox2.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox2_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programToolStripMenuItem,
            this.fileToolStripMenuItem,
            this.edycjaToolStripMenuItem,
            this.filtryToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1024, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // programToolStripMenuItem
            // 
            this.programToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aktualizacjaBazyToolStripMenuItem,
            this.zakonczToolStripMenuItem});
            this.programToolStripMenuItem.Name = "programToolStripMenuItem";
            this.programToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.programToolStripMenuItem.Text = "Program";
            // 
            // aktualizacjaBazyToolStripMenuItem
            // 
            this.aktualizacjaBazyToolStripMenuItem.Name = "aktualizacjaBazyToolStripMenuItem";
            this.aktualizacjaBazyToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.aktualizacjaBazyToolStripMenuItem.Text = "Aktualizacja Bazy";
            this.aktualizacjaBazyToolStripMenuItem.Click += new System.EventHandler(this.aktualizacjaBazyToolStripMenuItem_Click);
            // 
            // zakonczToolStripMenuItem
            // 
            this.zakonczToolStripMenuItem.Name = "zakonczToolStripMenuItem";
            this.zakonczToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.zakonczToolStripMenuItem.Text = "Zakoncz";
            this.zakonczToolStripMenuItem.Click += new System.EventHandler(this.zakonczToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zapiszToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(34, 20);
            this.fileToolStripMenuItem.Text = "Plik";
            // 
            // zapiszToolStripMenuItem
            // 
            this.zapiszToolStripMenuItem.Name = "zapiszToolStripMenuItem";
            this.zapiszToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.zapiszToolStripMenuItem.Text = "Zapisz";
            this.zapiszToolStripMenuItem.Click += new System.EventHandler(this.zapiszToolStripMenuItem_Click);
            // 
            // edycjaToolStripMenuItem
            // 
            this.edycjaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wycofajZmianyToolStripMenuItem});
            this.edycjaToolStripMenuItem.Name = "edycjaToolStripMenuItem";
            this.edycjaToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.edycjaToolStripMenuItem.Text = "Edycja";
            // 
            // wycofajZmianyToolStripMenuItem
            // 
            this.wycofajZmianyToolStripMenuItem.Name = "wycofajZmianyToolStripMenuItem";
            this.wycofajZmianyToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.wycofajZmianyToolStripMenuItem.Text = "Wycofaj zmiany";
            this.wycofajZmianyToolStripMenuItem.Click += new System.EventHandler(this.wycofajZmianyToolStripMenuItem_Click);
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
            this.skrótyKlawiaturoweToolStripMenuItem,
            this.oznaczeniaToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.helpToolStripMenuItem.Text = "Pomoc";
            // 
            // skrótyKlawiaturoweToolStripMenuItem
            // 
            this.skrótyKlawiaturoweToolStripMenuItem.Name = "skrótyKlawiaturoweToolStripMenuItem";
            this.skrótyKlawiaturoweToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.skrótyKlawiaturoweToolStripMenuItem.Text = "Skróty klawiaturowe";
            this.skrótyKlawiaturoweToolStripMenuItem.Click += new System.EventHandler(this.skrótyKlawiaturoweToolStripMenuItem_Click);
            // 
            // oznaczeniaToolStripMenuItem
            // 
            this.oznaczeniaToolStripMenuItem.Name = "oznaczeniaToolStripMenuItem";
            this.oznaczeniaToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.oznaczeniaToolStripMenuItem.Text = "Oznaczenia";
            this.oznaczeniaToolStripMenuItem.Click += new System.EventHandler(this.oznaczeniaToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.aboutToolStripMenuItem.Text = "O programie...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // PhotoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 660);
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
        private System.Windows.Forms.ToolStripButton zapiszAllButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem zapiszToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripMenuItem skrótyKlawiaturoweToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox2;
        private System.Windows.Forms.ToolStripButton zapiszButton1;
        private System.Windows.Forms.ToolStripMenuItem edycjaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wycofajZmianyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aktualizacjaBazyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oznaczeniaToolStripMenuItem;

    }
}

