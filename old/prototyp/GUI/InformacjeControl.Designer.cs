namespace Photo
{
    partial class InformacjeControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Tags = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Exif = new System.Windows.Forms.ListView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.Tags);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(278, 270);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Podstawowe informacje";
            // 
            // Tags
            // 
            this.Tags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tags.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Tags.Location = new System.Drawing.Point(3, 16);
            this.Tags.MultiSelect = false;
            this.Tags.Name = "Tags";
            this.Tags.Size = new System.Drawing.Size(272, 251);
            this.Tags.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.Tags.TabIndex = 0;
            this.Tags.UseCompatibleStateImageBehavior = false;
            this.Tags.View = System.Windows.Forms.View.Details;
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.Exif);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 270);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 194);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Exif";
            // 
            // Exif
            // 
            this.Exif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Exif.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Exif.Location = new System.Drawing.Point(3, 16);
            this.Exif.Name = "Exif";
            this.Exif.Size = new System.Drawing.Size(272, 175);
            this.Exif.TabIndex = 0;
            this.Exif.UseCompatibleStateImageBehavior = false;
            this.Exif.View = System.Windows.Forms.View.Details;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 267);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(278, 3);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // InformacjeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "InformacjeControl";
            this.Size = new System.Drawing.Size(278, 464);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView Tags;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView Exif;
        private System.Windows.Forms.Splitter splitter1;

    }
}
