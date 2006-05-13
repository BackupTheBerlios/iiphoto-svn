using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace Photo
{
    public partial class ListaAlbumowControl : UserControl, IWyszukiwacz
    {
        public ListaAlbumowControl()
        {
            InitializeComponent();           

           
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            IZdjecie[] zdjecia = Wyszukaj().PodajWynik();
            if (ZakonczonoWyszukiwanie != null)
                ZakonczonoWyszukiwanie(zdjecia);
        }

        //protected override ons

        #region IWyszukiwacz Members

        public event ZakonczonoWyszukiwanieDelegate ZakonczonoWyszukiwanie;
        public event RozpoczetoWyszukiwanieDelegate RozpoczetoWyszukiwanie;
        public event ZnalezionoZdjecieDelegate ZnalezionoZdjecie;

        public IWyszukiwanie Wyszukaj()
        {
            Wyszukiwanie wynik = new Wyszukiwanie();
            /*if (listBox1.SelectedIndex == 0)
                wynik.And("..\\..\\img\\album1.bmp");
            if (listBox1.SelectedIndex == 1)
                wynik.And("..\\..\\img\\album2.bmp");
            if (listBox1.SelectedIndex == 2)*/
                wynik.And("..\\..\\img\\album3.bmp");
            return wynik;
            
        }

        #endregion

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("cos");
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(""+e.Button);

            

            if (e.Button == MouseButtons.Left)
            {
                MessageBox.Show("lewy");
            }
            else if (e.Button == MouseButtons.Right)
            {
                MessageBox.Show("prawy");
            }

            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_MouseCaptureChanged(object sender, EventArgs e)
        {
           
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
