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
            this.thumbnailView1 = new Photo.WidokMiniatur();
            this.imageView1 = new Photo.WidokZdjecia();
            this.SuspendLayout();
            // 
            // thumbnailView1
            // 
            this.thumbnailView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thumbnailView1.Location = new System.Drawing.Point(0, 0);
            this.thumbnailView1.Name = "thumbnailView1";
            this.thumbnailView1.Size = new System.Drawing.Size(743, 457);
            this.thumbnailView1.TabIndex = 0;
            this.thumbnailView1.UseCompatibleStateImageBehavior = false;
            this.thumbnailView1.Visible = false;
            this.thumbnailView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mouseDoubleClick);
            // 
            // imageView1
            // 
            this.imageView1.AutoScroll = true;
            this.imageView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageView1.Location = new System.Drawing.Point(0, 0);
            this.imageView1.Name = "imageView1";
            this.imageView1.Size = new System.Drawing.Size(743, 457);
            this.imageView1.TabIndex = 1;
            this.imageView1.Visible = false;
            // 
            // ImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imageView1);
            this.Controls.Add(this.thumbnailView1);
            this.Name = "ImageViewer";
            this.Size = new System.Drawing.Size(743, 457);
            this.ResumeLayout(false);

        }

        #endregion

        private WidokZdjecia imageView1;
        private WidokMiniatur thumbnailView1;

        public event WybranoZdjecieDelegate WybranoZdjecie;

    }
}
