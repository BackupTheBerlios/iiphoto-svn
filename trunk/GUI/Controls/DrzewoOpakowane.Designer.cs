namespace Photo
{
    partial class DrzewoOpakowane
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
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fileTree1 = new Photo.FileTree();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Odœwie¿";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 241);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(166, 23);
            this.panel1.TabIndex = 1;
            // 
            // fileTree1
            // 
            this.fileTree1.BackColor = System.Drawing.Color.Ivory;
            this.fileTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileTree1.ImageIndex = 0;
            this.fileTree1.Location = new System.Drawing.Point(0, 0);
            this.fileTree1.Name = "fileTree1";
            this.fileTree1.SelectedImageIndex = 0;
            this.fileTree1.Size = new System.Drawing.Size(166, 241);
            this.fileTree1.TabIndex = 0;
            this.fileTree1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fileTree1_KeyDown);
            // 
            // DrzewoOpakowane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fileTree1);
            this.Controls.Add(this.panel1);
            this.Name = "DrzewoOpakowane";
            this.Size = new System.Drawing.Size(166, 264);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        public FileTree fileTree1;
        private System.Windows.Forms.Panel panel1;

        public event ZabierzFocusDelegate ZabierzFocus;


    }
}
