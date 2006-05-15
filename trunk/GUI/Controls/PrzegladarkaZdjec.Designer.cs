
using System.Windows.Forms;

namespace Photo
{
    partial class PrzegladarkaZdjec
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panele = new System.Windows.Forms.TabControl();
            this.miniatury1Tab = new System.Windows.Forms.TabPage();
            this.widokMiniatur1 = new Photo.WidokMiniatur();
            this.zdjecie1Tab = new System.Windows.Forms.TabPage();
            this.widokZdjecia1 = new Photo.WidokZdjecia();
            this.panele.SuspendLayout();
            this.miniatury1Tab.SuspendLayout();
            this.zdjecie1Tab.SuspendLayout();
            this.SuspendLayout();
            // 
            // panele
            // 
            this.panele.Controls.Add(this.miniatury1Tab);
            this.panele.Controls.Add(this.zdjecie1Tab);
            this.panele.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panele.Location = new System.Drawing.Point(0, 0);
            this.panele.Margin = new System.Windows.Forms.Padding(1);
            this.panele.Name = "panele";
            this.panele.Padding = new System.Drawing.Point(8, 3);
            this.panele.SelectedIndex = 0;
            this.panele.Size = new System.Drawing.Size(743, 457);
            this.panele.TabIndex = 2;
            this.panele.SelectedIndexChanged += new System.EventHandler(this.panele_onSelectedIndexChanged);
            // 
            // miniatury1Tab
            // 
            this.miniatury1Tab.Controls.Add(this.widokMiniatur1);
            this.miniatury1Tab.Location = new System.Drawing.Point(4, 22);
            this.miniatury1Tab.Margin = new System.Windows.Forms.Padding(1);
            this.miniatury1Tab.Name = "miniatury1Tab";
            this.miniatury1Tab.Padding = new System.Windows.Forms.Padding(1);
            this.miniatury1Tab.Size = new System.Drawing.Size(735, 431);
            this.miniatury1Tab.TabIndex = 0;
            this.miniatury1Tab.Text = "Widok Miniatur";
            this.miniatury1Tab.UseVisualStyleBackColor = true;
            // 
            // widokMiniatur1
            // 
            this.widokMiniatur1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.widokMiniatur1.Location = new System.Drawing.Point(1, 1);
            this.widokMiniatur1.Name = "widokMiniatur1";
            this.widokMiniatur1.Size = new System.Drawing.Size(733, 429);
            this.widokMiniatur1.TabIndex = 0;
            this.widokMiniatur1.UseCompatibleStateImageBehavior = false;
            this.widokMiniatur1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.widokMiniatur_DoubleClick);
            this.widokMiniatur1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.widokMiniatur_Click);
            this.widokMiniatur1.SelectedIndexChanged += new System.EventHandler(this.widokMiniatur_selectedIndexChanged);
            this.widokMiniatur1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.widokMiniatur1_keyPress);
            // 
            // zdjecie1Tab
            // 
            this.zdjecie1Tab.Controls.Add(this.widokZdjecia1);
            this.zdjecie1Tab.Location = new System.Drawing.Point(4, 22);
            this.zdjecie1Tab.Margin = new System.Windows.Forms.Padding(1);
            this.zdjecie1Tab.Name = "zdjecie1Tab";
            this.zdjecie1Tab.Padding = new System.Windows.Forms.Padding(1);
            this.zdjecie1Tab.Size = new System.Drawing.Size(735, 431);
            this.zdjecie1Tab.TabIndex = 1;
            this.zdjecie1Tab.Text = "Widok Zdjecia";
            this.zdjecie1Tab.UseVisualStyleBackColor = true;
            // 
            // widokZdjecia1
            // 
            this.widokZdjecia1.AutoScroll = true;
            this.widokZdjecia1.BackColor = System.Drawing.SystemColors.Window;
            this.widokZdjecia1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.widokZdjecia1.ForeColor = System.Drawing.SystemColors.Window;
            this.widokZdjecia1.Location = new System.Drawing.Point(1, 1);
            this.widokZdjecia1.Margin = new System.Windows.Forms.Padding(0);
            this.widokZdjecia1.Name = "widokZdjecia1";
            this.widokZdjecia1.Padding = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.widokZdjecia1.Size = new System.Drawing.Size(733, 429);
            this.widokZdjecia1.TabIndex = 1;
            // 
            // PrzegladarkaZdjec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panele);
            this.Name = "PrzegladarkaZdjec";
            this.Size = new System.Drawing.Size(743, 457);
            this.panele.ResumeLayout(false);
            this.miniatury1Tab.ResumeLayout(false);
            this.zdjecie1Tab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WidokZdjecia widokZdjecia1;
        private WidokMiniatur widokMiniatur1;
        private TabControl panele;
        private TabPage miniatury1Tab;
        private TabPage zdjecie1Tab;
        private ContextMenuStrip Context;

        public event WybranoZdjecieDelegate WybranoZdjecie;
        public event ZaznaczonoZdjecieDelegate ZaznaczonoZdjecie;

    }
}
