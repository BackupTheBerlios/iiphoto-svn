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
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.Usun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OK_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_Button.Location = new System.Drawing.Point(81, 133);
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
            this.Pomin_Button.Location = new System.Drawing.Point(162, 133);
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
            this.PominAll_Button.Location = new System.Drawing.Point(243, 133);
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
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBox1.Location = new System.Drawing.Point(12, 21);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(487, 54);
            this.textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(12, 85);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(361, 20);
            this.textBox2.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button1.Location = new System.Drawing.Point(379, 85);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 20);
            this.button1.TabIndex = 5;
            this.button1.Text = "Wybierz lokalizacjê...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Usun
            // 
            this.Usun.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Usun.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Usun.Location = new System.Drawing.Point(355, 133);
            this.Usun.Name = "Usun";
            this.Usun.Size = new System.Drawing.Size(75, 23);
            this.Usun.TabIndex = 6;
            this.Usun.Text = "Usuñ";
            this.Usun.UseVisualStyleBackColor = true;
            this.Usun.Click += new System.EventHandler(this.Usun_Click);
            // 
            // ZnajdzPliki
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(511, 168);
            this.Controls.Add(this.Usun);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.PominAll_Button);
            this.Controls.Add(this.Pomin_Button);
            this.Controls.Add(this.OK_Button);
            this.Name = "ZnajdzPliki";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ZnajdŸ pliki";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OK_Button;
        private System.Windows.Forms.Button Pomin_Button;
        private System.Windows.Forms.Button PominAll_Button;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button Usun;
    }
}