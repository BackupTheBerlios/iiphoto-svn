using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace yapv
{
    public partial class SetProp : Form
    {
        imageWindow iw;
        public SetProp(imageWindow iw)
        {
            this.iw = iw;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            iw.setProperty(textBox1.Text);
        }
    }
}