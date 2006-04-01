using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace yapv
{
    public partial class ShowProp : Form
    {
        imageWindow iw;
        public ShowProp(imageWindow iw)
        {
            this.iw = iw;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            iw.showProperty(textBox1.Text);
        }
    }
}