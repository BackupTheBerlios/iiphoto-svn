namespace Photo
{
    partial class Photo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Photo));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStripOperacje = new System.Windows.Forms.ToolStrip();
            this.Alfa = new System.Windows.Forms.ToolStripButton();
            this.Omega = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.wyszukiwarkaControl1 = new WyszukiwarkaControl();
            this.listaAlbumowControl = new ListaAlbumowControl();
            this.informacjeControl = new InformacjeControl();
            this.listaOpakowanControl = new ListaOpakowanControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStripOperacje.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.listaOpakowanControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(637, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(637, 308);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(637, 355);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripOperacje);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.splitContainer1.Panel2.Controls.Add(this.informacjeControl);
            this.splitContainer1.Size = new System.Drawing.Size(637, 308);
            this.splitContainer1.SplitterDistance = 535;
            this.splitContainer1.TabIndex = 2;
            // 
            // toolStripOperacje
            // 
            this.toolStripOperacje.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripOperacje.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Alfa,
            this.Omega,
            this.toolStripSeparator1});
            this.toolStripOperacje.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripOperacje.Location = new System.Drawing.Point(3, 0);
            this.toolStripOperacje.Name = "toolStripOperacje";
            this.toolStripOperacje.Size = new System.Drawing.Size(175, 25);
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
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.splitContainer2.Panel1.Controls.Add(this.wyszukiwarkaControl1);
            this.splitContainer2.Panel1.Controls.Add(this.listaAlbumowControl);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.splitContainer2.Panel2.Controls.Add(this.listaOpakowanControl);
            this.splitContainer2.Size = new System.Drawing.Size(535, 308);
            this.splitContainer2.SplitterDistance = 210;
            this.splitContainer2.TabIndex = 0;
            // 
            // wyszukiwarkaControl1
            // 
            this.wyszukiwarkaControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.wyszukiwarkaControl1.Location = new System.Drawing.Point(0, 144);
            this.wyszukiwarkaControl1.Name = "wyszukiwarkaControl1";
            this.wyszukiwarkaControl1.Size = new System.Drawing.Size(210, 82);
            this.wyszukiwarkaControl1.TabIndex = 1;
            // 
            // listaAlbumowControl
            // 
            this.listaAlbumowControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.listaAlbumowControl.Location = new System.Drawing.Point(0, 0);
            this.listaAlbumowControl.Name = "listaAlbumowControl";
            this.listaAlbumowControl.Size = new System.Drawing.Size(210, 144);
            this.listaAlbumowControl.TabIndex = 0;
            // 
            // informacjeControl
            // 
            this.informacjeControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.informacjeControl.Location = new System.Drawing.Point(0, 0);
            this.informacjeControl.Name = "informacjeControl";
            this.informacjeControl.Size = new System.Drawing.Size(98, 192);
            this.informacjeControl.TabIndex = 0;
            // 
            // listaOpakowanControl1
            // 
            this.listaOpakowanControl.Controls.Add(this.tabPage1);
            this.listaOpakowanControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listaOpakowanControl.Location = new System.Drawing.Point(0, 0);
            this.listaOpakowanControl.Name = "listaOpakowanControl1";
            this.listaOpakowanControl.SelectedIndex = 0;
            this.listaOpakowanControl.Size = new System.Drawing.Size(321, 308);
            this.listaOpakowanControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(313, 282);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Wynik wyszukiwania";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Photo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 355);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "Photo";
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.toolStripOperacje.ResumeLayout(false);
            this.toolStripOperacje.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.listaOpakowanControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStripOperacje;
        private System.Windows.Forms.ToolStripButton Alfa;
        private System.Windows.Forms.ToolStripButton Omega;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private WyszukiwarkaControl wyszukiwarkaControl1;
        private ListaAlbumowControl listaAlbumowControl;
        private InformacjeControl informacjeControl;
        private ListaOpakowanControl listaOpakowanControl;
        private System.Windows.Forms.TabPage tabPage1;
    }
}

