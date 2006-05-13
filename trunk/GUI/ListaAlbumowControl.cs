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
        private ContextMenuStrip Context;

        public ListaAlbumowControl()
        {
            InitializeComponent();
            Context = new ContextMenuStrip();

            Db baza = new Db();

            baza.Polacz();

            try
            {
                //baza.Insert_czesci("Tag", "nazwa,album", "\'miejsca\',1");

                TreeNode albumy = new TreeNode("Albumy");
                TreeNode tagi = new TreeNode("Tagi");

                //baza.Delete("Tag", "album=1");


                DataSet dataSet = baza.Select("select nazwa from Tag where album=1;");                

                foreach (DataTable t in dataSet.Tables)
                {
                    if (t.Rows.Count == 0)
                    {
                        //albumy.Checked = true;
                        //albumy.e
                        //albumy.
                    }

                    foreach (DataRow r in t.Rows)
                    {
                        foreach (DataColumn c in t.Columns)
                        {
                            albumy.Nodes.Add(new TreeNode(""+r[c.ColumnName]));
                        }
                    }
                }

                dataSet = baza.Select("select nazwa from Tag where album=0;");

                foreach (DataTable t in dataSet.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        foreach (DataColumn c in t.Columns)
                        {                            
                            tagi.Nodes.Add(new TreeNode("" + r[c.ColumnName]));
                        }
                    }
                }


                this.treeView1.Nodes.Add(albumy);
                this.treeView1.Nodes.Add(tagi);

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

            baza.Rozlacz();
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

        private void cos()
        {
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

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {

        }
        private void DodajAlbum(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;
            //MessageBox.Show("Dodaje zawartosc katalogu " + mn.ToolTipText + " do kolekcji!");
        }

        private void DodajTag(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;
            //MessageBox.Show("Dodaje zawartosc katalogu " + mn.ToolTipText + " do kolekcji!");
        } 


        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.FullPath.IndexOf("Albumy") == 0)
                {

                    Context.Items.Clear();
                    ToolStripItem toolStripItem = Context.Items.Add("Dodaj Album");
                    // toolStripItem.ToolTipText = ((DirTreeNode)e.Node).Path;
                    toolStripItem.Click += new EventHandler(DodajAlbum);

                    Context.Show(this, new Point(e.X, e.Y));
                }
                else
                {
                    Context.Items.Clear();
                    ToolStripItem toolStripItem = Context.Items.Add("Dodaj Tag");
                    // toolStripItem.ToolTipText = ((DirTreeNode)e.Node).Path;
                    toolStripItem.Click += new EventHandler(DodajTag);

                    Context.Show(this, new Point(e.X, e.Y));
                }
            }

            /*MessageBox.Show("" + e.Button);



            if (e.Button == MouseButtons.Left)
            {
                MessageBox.Show("lewy");
            }
            else if (e.Button == MouseButtons.Right)
            {
                MessageBox.Show("prawy");
            }*/
        }

        
    }
}
