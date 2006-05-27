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

        private void button1_Click_1(object sender, EventArgs e)
        {
            fileTree1.Delete();
            fileTree1.Fill();  
        }

        public void ZaladujZawartoscKatalogu(Katalog k)
        {
            fileTree1.ZaladujZawartoscKatalogu(k);
        }

        private void fileTree1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.F))
            {
                if (ZabierzFocus != null)
                    ZabierzFocus();
            }
        }

    }
}
