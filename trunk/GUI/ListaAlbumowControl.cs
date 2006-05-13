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
        private TreeNode albumy, tagi;

        public ListaAlbumowControl()
        {
            InitializeComponent();
            Context = new ContextMenuStrip();

            Wypelnij();

            
        }

        private void Usun_wszystkie()
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            treeView1.EndUpdate();
        }

        private void Wypelnij()
        {
            Db baza = new Db();

            baza.Polacz();

            try
            {
                //baza.Insert_czesci("Tag", "nazwa,album", "\'miejsca\',1");

                TreeNode alb = new TreeNode("Albumy");
                TreeNode ta = new TreeNode("Tagi");
                albumy = alb;
                tagi = ta;

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
                            alb.Nodes.Add(new TreeNode("" + r[c.ColumnName]));
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
                            ta.Nodes.Add(new TreeNode("" + r[c.ColumnName]));
                        }
                    }
                }


                this.treeView1.Nodes.Add(alb);
                this.treeView1.Nodes.Add(ta);

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

            baza.Rozlacz();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            odswiez();
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MessageBox.Show("zczytanie z bazy i wyswietlenie zawartosci pozycji " + e.Node.Text);
        }

        public void odswiez()
        {
            bool al = false, t = false;
            
            if (albumy.IsExpanded == true)
            {
                //albumy.Expand();
                al = true;
            }
            if (tagi.IsExpanded == true)
            {
                tagi.Expand();
                t = true;
            }

            Usun_wszystkie();
            Wypelnij();

            if (al)
            {
                albumy.Expand();
            }
            if (t)
            {
                tagi.Expand();
            }
        }

        private void ZarzadzajAlbumem(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;
            //MessageBox.Show(mn.Text);

            //tu chcial bym miec wybor czy dodaje czy usuwam album


            Dodaj_Album da = new Dodaj_Album();
            da.Show();
            //MessageBox.Show("Dodaje zawartosc katalogu " + mn.ToolTipText + " do kolekcji!");
        }

        private void ZarzadzajTagiem(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;
            //MessageBox.Show("Dodaje zawartosc katalogu " + mn.ToolTipText + " do kolekcji!");

            Dodaj_Tag dt = new Dodaj_Tag();
            dt.Show();
        } 


        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.FullPath.IndexOf("Albumy") == 0)
                {

                    Context.Items.Clear();                    
                    
                    ToolStripItem toolStripItem = Context.Items.Add("Dodaj Album");
                    Context.Items.Add("Usun Album");

                    // toolStripItem.ToolTipText = ((DirTreeNode)e.Node).Path;
                    toolStripItem.Click += new EventHandler(ZarzadzajAlbumem);
                    

                    Context.Show(this, new Point(e.X, e.Y));
                }
                else
                {
                    Context.Items.Clear();
                    ToolStripItem toolStripItem = Context.Items.Add("Dodaj Tag");
                    Context.Items.Add("Usun Tag");

                    // toolStripItem.ToolTipText = ((DirTreeNode)e.Node).Path;
                    toolStripItem.Click += new EventHandler(ZarzadzajTagiem);

                    Context.Show(this, new Point(e.X, e.Y));
                }
            }            
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //MessageBox.Show("check");

            if (e.Node.Text == "Albumy")
            {
                if (e.Node.Checked == true)
                {
                    foreach (TreeNode t in e.Node.Nodes)
                    {
                        t.Checked = true;
                    }
                }
                else
                {
                    foreach (TreeNode t in e.Node.Nodes)
                    {
                        t.Checked = false;
                    }
                }
            }
            if (e.Node.Text == "Tagi")
            {             
                if (e.Node.Checked == true)
                {
                    foreach (TreeNode t in e.Node.Nodes)
                    {
                        t.Checked = true;
                    }
                }
                else
                {
                    foreach (TreeNode t in e.Node.Nodes)
                    {
                        t.Checked = false;
                    }
                }
            }
        }        
    }
}
