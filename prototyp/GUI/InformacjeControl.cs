using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Photo
{
    public partial class InformacjeControl : UserControl
    {
        public InformacjeControl()
        {
            InitializeComponent();

            Tags.Columns.Add("Nazwa");
            Tags.Columns.Add("Wartosc");

            Tags.Items.Add(new ListViewItem(new string[] { "Format", "JPG" }));
            Tags.Items.Add(new ListViewItem(new string[] { "Rozdzielczosc", "800x600" }));
        }

        public void Zaladuj(IZdjecie zdjecie)
        {
            if (zdjecie != null)
            {
                //MiniaturaControl mini = (MiniaturaControl)zdjecie;
                //listBox1.Items.Add(mini.ImageLocation);
            }
            else
            {
                //listBox1.Items.Add("null");
            }
        }
    }
}
