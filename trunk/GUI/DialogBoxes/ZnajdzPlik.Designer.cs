namespace Photo
{
    partial class ZnajdzPliki
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
            this.OK_Button = new System.Windows.Forms.Button();
            this.Pomin_Button = new System.Windows.Forms.Button();
            this.PominAll_Button = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OK_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_Button.Location = new System.Drawing.Point(96, 135);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(75, 23);
            this.OK_Button.TabIndex = 0;
            this.OK_Button.Text = "OK";
            this.OK_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // Pomin_Button
            // 
            this.Pomin_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Pomin_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Pomin_Button.Location = new System.Drawing.Point(177, 135);
            this.Pomin_Button.Name = "Pomin_Button";
            this.Pomin_Button.Size = new System.Drawing.Size(75, 23);
            this.Pomin_Button.TabIndex = 1;
            this.Pomin_Button.Text = "Pomiñ";
            this.Pomin_Button.UseVisualStyleBackColor = true;
            this.Pomin_Button.Click += new System.EventHandler(this.Pomin_Button_Click);
            // 
            // PominAll_Button
            // 
            this.PominAll_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PominAll_Button.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.PominAll_Button.Location = new System.Drawing.Point(258, 135);
            this.PominAll_Button.Name = "PominAll_Button";
            this.PominAll_Button.Size = new System.Drawing.Size(106, 23);
            this.PominAll_Button.TabIndex = 2;
            this.PominAll_Button.Text = "Pomiñ wszystkie";
            this.PominAll_Button.UseVisualStyleBackColor = true;
            this.PominAll_Button.Click += new System.EventHandler(this.PominAll_Button_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(12, 39);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(437, 20);
            this.textBox1.TabIndex = 3;
            // 
            // ZnajdzPliki
            // 
            this.AcceptButton = this.OK_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 170);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.PominAll_Button);
            this.Controls.Add(this.Pomin_Button);
            this.Controls.Add(this.OK_Button);
            this.Name = "ZnajdzPliki";
            this.Text = "ZnajdŸ pliki";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OK_Button;
        private System.Windows.Forms.Button Pomin_Button;
        private System.Windows.Forms.Button PominAll_Button;
        private System.Windows.Forms.TextBox textBox1;
    }
}