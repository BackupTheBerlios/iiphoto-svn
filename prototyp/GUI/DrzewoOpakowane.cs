using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Photo
{
    public partial class DrzewoOpakowane : UserControl
    {
        public DrzewoOpakowane()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fileTree1.Delete();
            fileTree1.Fill();            
        }
    }
}
