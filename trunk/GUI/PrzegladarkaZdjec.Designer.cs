
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
            this.miniatury1 = new System.Windows.Forms.TabPage();
            this.widokMiniatur1 = new Photo.WidokMiniatur();
            this.zdjecie1 = new System.Windows.Forms.TabPage();
            this.widokZdjecia1 = new Photo.WidokZdjecia();
            this.panele.SuspendLayout();
            this.miniatury1.SuspendLayout();
            this.zdjecie1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.panele.Controls.Add(this.miniatury1);
            this.panele.Controls.Add(this.zdjecie1);
            this.panele.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panele.Location = new System.Drawing.Point(0, 0);
            this.panele.Margin = new System.Windows.Forms.Padding(1);
            this.panele.Name = "tabControl1";
            this.panele.Padding = new System.Drawing.Point(8, 3);
            this.panele.SelectedIndex = 0;
            this.panele.Size = new System.Drawing.Size(743, 457);
            this.panele.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.miniatury1.Controls.Add(this.widokMiniatur1);
            this.miniatury1.Location = new System.Drawing.Point(4, 22);
            this.miniatury1.Margin = new System.Windows.Forms.Padding(1);
            this.miniatury1.Name = "tabPage1";
            this.miniatury1.Padding = new System.Windows.Forms.Padding(1);
            this.miniatury1.Size = new System.Drawing.Size(735, 431);
            this.miniatury1.TabIndex = 0;
            this.miniatury1.Text = "Widok Miniatur";
            this.miniatury1.UseVisualStyleBackColor = true;
            // 
            // widokMiniatur1
            // 
            this.widokMiniatur1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.widokMiniatur1.Location = new System.Drawing.Point(1, 1);
            this.widokMiniatur1.Name = "widokMiniatur1";
            this.widokMiniatur1.Size = new System.Drawing.Size(733, 429);
            this.widokMiniatur1.TabIndex = 0;
            this.widokMiniatur1.UseCompatibleStateImageBehavior = false;
            this.widokMiniatur1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mouseDoubleClick);
            // 
            // tabPage2
            // 
            this.zdjecie1.Controls.Add(this.widokZdjecia1);
            this.zdjecie1.Location = new System.Drawing.Point(4, 22);
            this.zdjecie1.Margin = new System.Windows.Forms.Padding(1);
            this.zdjecie1.Name = "tabPage2";
            this.zdjecie1.Padding = new System.Windows.Forms.Padding(1);
            this.zdjecie1.Size = new System.Drawing.Size(735, 431);
            this.zdjecie1.TabIndex = 1;
            this.zdjecie1.Text = "Widok Zdjecia";
            this.zdjecie1.UseVisualStyleBackColor = true;
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
            this.miniatury1.ResumeLayout(false);
            this.zdjecie1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WidokZdjecia widokZdjecia1;
        private WidokMiniatur widokMiniatur1;
        private TabControl panele;
        private TabPage miniatury1;
        private TabPage zdjecie1;

        public event WybranoZdjecieDelegate WybranoZdjecie;

    }
}
