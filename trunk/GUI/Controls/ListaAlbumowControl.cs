using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;
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


                DataSet dataSet = baza.Select("select nazwa from Tag where album=1 order by nazwa asc;");

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

                dataSet = baza.Select("select nazwa from Tag where album=0 order by nazwa asc;");

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
           /* Wyszukiwanie wynik = new Wyszukiwanie();
            List<Zdjecie> lista = new List<Zdjecie>();
            //Zdjecie zd1 = new Zdjecie(
            lista.Add(new Zdjecie("c:\\IM000271.jpg"));
            lista.Add(new Zdjecie("c:\\IM000271t.jpg"));
                /*if (listBox1.SelectedIndex == 0)
                wynik.And("..\\..\\img\\album1.bmp");
            if (listBox1.SelectedIndex == 1)
                wynik.And("..\\..\\img\\album2.bmp");
            if (listBox1.SelectedIndex == 2)*/
             //   wynik.And("..\\..\\img\\album3.bmp");
               // return (IZdjecie[])lista.ToArray;
                //wynik;

            throw new Exception("The method or operation is not implemented.");
            
        }

        #endregion


        /*
         if (RozpoczetoWyszukiwanie != null)
                RozpoczetoWyszukiwanie(null);

            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync(e.Node);
         */

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (ZnalezionoZdjecie != null)
                ZnalezionoZdjecie((Zdjecie)e.UserState);
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ZakonczonoWyszukiwanie != null)
            {
                ZakonczonoWyszukiwanie((IZdjecie[])e.Result);
            }

            this.Refresh();
        }

        void bw_DoWork(object sender, DoWorkEventArgs args)
        {
            TreeNode Node = (TreeNode)args.Argument;
            BackgroundWorker bw = sender as BackgroundWorker;
            args.Result = PokazPlikiZAlbumu(bw, Node);
        }



        private Zdjecie[] PokazPlikiZAlbumu(BackgroundWorker bw, TreeNode Node)
        {
            Db baza = new Db();

            List<string> pliki = new List<string>();
            List<Zdjecie> lista = new List<Zdjecie>();

            
            pliki.AddRange(Directory.GetFiles("c:\\", "*.jpg"));

            Zdjecie z;

            foreach (string zd in pliki)
            {
                z = new Zdjecie(zd);
                //MessageBox.Show(z.Path);
                bw.ReportProgress(0, z);
                lista.Add(z);
                //MessageBox.Show(zd);
            }

            
            //lista.Add(new Zdjecie("c:\\IM000271.jpg"));
            //lista.Add(new Zdjecie("c:\\IM000271t.jpg"));

            //MessageBox.Show("c:\\IM000271.jpg");

            baza.Polacz();

            try
            {

            }
            catch (SqlException)
            {

            }
            baza.Rozlacz();

            return lista.ToArray();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (RozpoczetoWyszukiwanie != null)
                RozpoczetoWyszukiwanie(null);

            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync(e.Node);

            //MessageBox.Show("zczytanie z bazy i wyswietlenie zawartosci pozycji " + e.Node.Text);
        }

        public void odswiez()
        {
            bool al = false, t = false;
            
            if (albumy.IsExpanded == true)
            {                
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

        private void DodajAlbum(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;
            
            Dodaj_Album da = new Dodaj_Album(this);
            da.Show();            
        }

        private void UsunAlbum(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;          

            Db baza = new Db();

            baza.Polacz();

            try
            {
                baza.Delete("Tag", "nazwa=\'" + mn.ToolTipText + "\' and album=1");
            }
            catch (SqlException)
            {

            }
            baza.Rozlacz();

            odswiez();
        }

        private void DodajTag(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;            

            Dodaj_Tag dt = new Dodaj_Tag(this);
            dt.Show();
        }

        private void UsunTag(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;            

            Db baza = new Db();

            baza.Polacz();

            try
            {
                baza.Delete("Tag", "nazwa=\'" + mn.ToolTipText + "\' and album=0");
            }
            catch (SqlException)
            {

            }
            baza.Rozlacz();

            odswiez();
        }


        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.FullPath.IndexOf("Albumy") == 0)
                {

                    Context.Items.Clear();                    
                    
                    ToolStripItem toolStripItem = Context.Items.Add("Dodaj Album");
                    toolStripItem.Click += new EventHandler(DodajAlbum);
                    toolStripItem = Context.Items.Add("Usun Album");
                    toolStripItem.ToolTipText = e.Node.Text;
                    toolStripItem.Click += new EventHandler(UsunAlbum);
                   
                    Context.Show(this, new Point(e.X, e.Y));
                }
                else
                {
                    Context.Items.Clear();
                    ToolStripItem toolStripItem = Context.Items.Add("Dodaj Tag");
                    toolStripItem.Click += new EventHandler(DodajTag);
                    toolStripItem = Context.Items.Add("Usun Tag");
                    toolStripItem.ToolTipText = e.Node.Text;                    
                    toolStripItem.Click += new EventHandler(UsunTag);

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
