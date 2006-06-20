namespace Photo.DialogBoxes
{
    partial class CzyZapisac
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
            this.Yes_Button = new System.Windows.Forms.Button();
            this.YesForAll = new System.Windows.Forms.Button();
            this.No_Button = new System.Windows.Forms.Button();
            this.NoForAll = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Yes_Button
            // 
            this.Yes_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Yes_Button.Location = new System.Drawing.Point(15, 94);
            this.Yes_Button.Name = "Yes_Button";
            this.Yes_Button.Size = new System.Drawing.Size(75, 23);
            this.Yes_Button.TabIndex = 1;
            this.Yes_Button.Text = "Tak";
            this.Yes_Button.UseVisualStyleBackColor = true;
            this.Yes_Button.Click += new System.EventHandler(this.Yes_Button_Click);
            // 
            // YesForAll
            // 
            this.YesForAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.YesForAll.Location = new System.Drawing.Point(96, 94);
            this.YesForAll.Name = "YesForAll";
            this.YesForAll.Size = new System.Drawing.Size(121, 23);
            this.YesForAll.TabIndex = 2;
            this.YesForAll.Text = "Tak dla wszyskich";
            this.YesForAll.UseVisualStyleBackColor = true;
            this.YesForAll.Click += new System.EventHandler(this.YesForAll_Click);
            // 
            // No_Button
            // 
            this.No_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.No_Button.Location = new System.Drawing.Point(223, 94);
            this.No_Button.Name = "No_Button";
            this.No_Button.Size = new System.Drawing.Size(75, 23);
            this.No_Button.TabIndex = 3;
            this.No_Button.Text = "Nie";
            this.No_Button.UseVisualStyleBackColor = true;
            this.No_Button.Click += new System.EventHandler(this.No_Button_Click);
            // 
            // NoForAll
            // 
            this.NoForAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NoForAll.Location = new System.Drawing.Point(304, 94);
            this.NoForAll.Name = "NoForAll";
            this.NoForAll.Size = new System.Drawing.Size(111, 23);
            this.NoForAll.TabIndex = 4;
            this.NoForAll.Text = "Nie dla wszystkich";
            this.NoForAll.UseVisualStyleBackColor = true;
            this.NoForAll.Click += new System.EventHandler(this.NoForAll_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBox1.Location = new System.Drawing.Point(15, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(400, 76);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CzyZapisac
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 129);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.NoForAll);
            this.Controls.Add(this.No_Button);
            this.Controls.Add(this.YesForAll);
            this.Controls.Add(this.Yes_Button);
            this.Name = "CzyZapisac";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CzyZapisac";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Yes_Button;
        private System.Windows.Forms.Button YesForAll;
        private System.Windows.Forms.Button No_Button;
        private System.Windows.Forms.Button NoForAll;
        private System.Windows.Forms.TextBox textBox1;
    }
}