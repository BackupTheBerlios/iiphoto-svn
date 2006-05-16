using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace Photo
{
    public class FileTree : TreeView, IWyszukiwacz
    {        
        private const int Dysk = 0;
        private const int FOLDER = 1;
        private const int Dyskietka = 2;
        private const int Cdrom = 3;
        private const int Cdrom_z = 4;
        private const int Dyskietka_z = 5;

        private ContextMenuStrip Context;

        public FileTree()
        {
            GenerateImage();
            this.BackColor = Color.Beige;
            Context = new ContextMenuStrip();
        }   

        public void GenerateImage()
        {
            ImageList list = new ImageList();
            list.Images.Add(Properties.Resources.Dysk);
            list.Images.Add(Properties.Resources.folder);
            list.Images.Add(Properties.Resources.Dyskietka);
            list.Images.Add(Properties.Resources.Cdrom);
            list.Images.Add(Properties.Resources.Cdrom_z);
            list.Images.Add(Properties.Resources.Dyskietka_z);
            ImageList = list;
        }

        public void Fill()
        {
            BeginUpdate();
            string[] drives = Directory.GetLogicalDrives();

            //MessageBox.Show(Config.katalogAplikacji);

            //DirTreeNode kat_Aplikacji = new DirTreeNode(Config.katalogAplikacji);
            //kat_Aplikacji.Text = "Dane Aplikacji";
            //Nodes.Add(kat_Aplikacji);
            DirTreeNode kat_MDokumenty = new DirTreeNode(Config.katalogMojeDokumenty);
            kat_MDokumenty.Text = "Moje Dokumenty";
            Nodes.Add(kat_MDokumenty);

            DirTreeNode kat_MObrazy = new DirTreeNode(Config.katalogMojeObrazy);
            kat_MObrazy.Text = "Moje Obrazy";
            Nodes.Add(kat_MObrazy);

            DirTreeNode kat_Pulpit = new DirTreeNode(Config.katalogPulpit);
            kat_Pulpit.Text = "Pulpit";
            Nodes.Add(kat_Pulpit);

            foreach (string tempDrive in drives)
            {
                DirTreeNode dn = new DirTreeNode(tempDrive);
                Nodes.Add(dn);              

                if (tempDrive.IndexOf("C:\\") != -1)
                {                    
                    this.SelectedNode = dn;
                    this.Select(true, true);
                }                
            }

            EndUpdate();

            BeforeExpand += new TreeViewCancelEventHandler(prepare);
            AfterCollapse += new TreeViewEventHandler(clear);
        }

        public void Open(string path)
        {
            TreeNodeCollection nodes = Nodes;
            DirTreeNode subnode = null;
            int i, n;

            path = path.ToLower();
            Nodes.Clear();

            MessageBox.Show("open");

            nodes = Nodes;
            while (nodes != null)
            {
                n = nodes.Count;
                for (i = 0; i < n; i++)
                {
                    subnode = (DirTreeNode)nodes[i];
                    if (path == subnode.Path)
                    {
                        subnode.Expand();                        
                        return;
                    }
                    if (path.StartsWith(subnode.Path))
                    {
                        subnode.Expand();                        
                        break;
                    }
                }
                if (i == n)
                    return;
                nodes = subnode.Nodes;
            }
        }
        void prepare(object sender, TreeViewCancelEventArgs e)
        {
            BeginUpdate();
            DirTreeNode tn = (DirTreeNode)e.Node;
            try
            {
                tn.populate(tn);
                EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - Odmowa dostêpu", e.Node.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //e.Node.                
                e.Node.Remove();                
                Nodes.Insert(tn.Index, tn);
                                
                EndUpdate();
            }
        }
        void clear(object sender, TreeViewEventArgs e)
        {
            BeginUpdate();
            DirTreeNode tn = (DirTreeNode)e.Node;
            tn.setLeaf(true);
            EndUpdate();
        }

        public void Delete()
        {
            BeginUpdate();
            //Nodes.RemoveAt(0);
            Nodes.Clear();
            EndUpdate();
        }

        

        public class DirTreeNode : TreeNode
        {
            string path;
            int type;
            public virtual string Path { get { return path; } }

            public DirTreeNode(string s)
                : base(s)
            {
                if (s.CompareTo("A:\\") == 0 || s.CompareTo("B:\\") == 0)
                {                    
                    Text = s + " [Floppy]";
                    ImageIndex = Dyskietka;
                    SelectedImageIndex = Dyskietka;                    
                }
                else
                {
                    Text = s + " [" + GetDriveName(s) + "]";
                    //dn = new DirTreeNode(tempDrive, tempDrive + " [" + GetDriveName(tempDrive) + "]");
                    if (GetDriveName(s) != "" && getDriveType(s) == Cdrom)
                    {
                        ImageIndex = Cdrom_z;
                        SelectedImageIndex = Cdrom_z;
                    }
                    else
                    {
                        ImageIndex = getDriveType(s);
                        SelectedImageIndex = getDriveType(s);
                    }
                }

                path = s.ToLower();
                setLeaf(true);                
            }
            public DirTreeNode(string s, string label)
                : this(s)
            {
                Text = label;
            }
            public DirTreeNode(string s, int aType)
                : base(new FileInfo(s).Name)
            {
                path = s.ToLower();
                type = aType;
                ImageIndex = type;
                SelectedImageIndex = type;
                try
                {
                    string[] n = Directory.GetDirectories(path);

                    if (type == FOLDER && n.Length != 0)
                        setLeaf(true);
                }
                catch (Exception)
                {
                }
            }

            
            [DllImport("kernel32.dll")]
            public static extern long GetDriveType(string driveLetter);
            [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            extern static bool GetVolumeInformation(
                                    string RootPathName,
                                    StringBuilder VolumeNameBuffer,
                                    int VolumeNameSize,
                                    out uint VolumeSerialNumber,
                                    out uint MaximumComponentLength,
                                    out uint FileSystemFlags,
                                    StringBuilder FileSystemNameBuffer,
                                    int nFileSystemNameSize);



            public int getDriveType(string drive)
            {
                if ((GetDriveType(drive) & 5) == 5) return Cdrom;//cd
                if ((GetDriveType(drive) & 3) == 3) return Dysk;//fixed
                if ((GetDriveType(drive) & 2) == 2) return Dysk;//removable
                if ((GetDriveType(drive) & 4) == 4) return Dyskietka;//remote disk
                if ((GetDriveType(drive) & 6) == 6) return Dyskietka;//ram disk
                return 0;
            }



            public static string GetDriveName(string drive)
            {
                //receives volume name of drive
                StringBuilder volname = new StringBuilder(256);
                //receives serial number of drive,not in case of network drive(win95/98)
                uint sn;
                uint maxcomplen;//receives maximum component length
                uint sysflags;//receives file system flags
                StringBuilder sysname = new StringBuilder(256);//receives the file system name
                bool retval;//return value

                retval = GetVolumeInformation(drive, volname, 256, out sn, out maxcomplen,
                                              out sysflags, sysname, 256);

                if (retval == true) return volname.ToString();
                else return "";
            }

            public static string Etykieta(string drive)
            {
                return GetDriveName(drive);
            }

            internal void populate(DirTreeNode tn)
            {
                DirTreeNode dnn = new DirTreeNode(tn.Text);
                int gdzie = tn.Index;
                    
                    //(DirTreeNode)tn;

                ArrayList folder = new ArrayList();

                //string

                //MessageBox.Show(Path);

                string[] files = Directory.GetDirectories(Path);
                Array.Sort(files);

                /*if (files.Length == 0 && type == Dysk)
                {

                    //return;    
                }*/

                for (int i = 0; i < files.Length; i++)
                {
                    folder.Add(new DirTreeNode(files[i], FOLDER));
                }

                Nodes.Clear();
                foreach (DirTreeNode dtn in folder)
                {
                    Nodes.Add(dtn);
                }
            }

            bool isLeaf = true;
            internal void setLeaf(bool b)
            {
                Nodes.Clear();
                isLeaf = b;
                if (IsExpanded)
                    return;
                if (!isLeaf)
                    return;
                Nodes.Add(new TreeNode());
            }

        }

        #region IWyszukiwacz Members

        public IWyszukiwanie Wyszukaj()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public event ZakonczonoWyszukiwanieDelegate ZakonczonoWyszukiwanie;
        public event ZnalezionoZdjecieDelegate ZnalezionoZdjecie;
        public event RozpoczetoWyszukiwanieDelegate RozpoczetoWyszukiwanie;

        #endregion


        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            base.OnBeforeSelect(e);
            //MessageBox.Show("numer: " + e.Node.SelectedImageIndex);
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);

            if (RozpoczetoWyszukiwanie != null)
                RozpoczetoWyszukiwanie(null);

            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync(e.Node);

            //DirTreeNode dn = new DirTreeNode("napis");            

            string etykieta;

            if (e.Node.Text.IndexOf("A:\\") == 0 && e.Node.Text.LastIndexOf("\\") < 4)
            {
                etykieta = DirTreeNode.Etykieta(e.Node.Text.Substring(0, 3));
                if (etykieta != "")
                {
                    e.Node.Text = "A:\\" + " [" + etykieta + "]";
                    e.Node.ImageIndex = Dyskietka_z;
                    e.Node.SelectedImageIndex = Dyskietka_z;
                }
                else
                {
                    e.Node.Text = "A:\\" + " [Floppy]";
                    e.Node.ImageIndex = Dyskietka;
                    e.Node.SelectedImageIndex = Dyskietka;
                }
            }
            else if (e.Node.Text.IndexOf("B:\\") == 0 && e.Node.Text.LastIndexOf("\\") < 4)
            {
                etykieta = DirTreeNode.Etykieta(e.Node.Text.Substring(0, 3));
                if (etykieta != "")
                {
                    e.Node.Text = "B:\\" + " [" + etykieta + "]";
                    e.Node.ImageIndex = Dyskietka_z;
                    e.Node.SelectedImageIndex = Dyskietka_z;
                }
                else
                {
                    e.Node.Text = "B:\\" + " [Floppy]";
                    e.Node.ImageIndex = Dyskietka;
                    e.Node.SelectedImageIndex = Dyskietka;
                }
            }
        }

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
            DirTreeNode Node = (DirTreeNode)args.Argument;
            BackgroundWorker bw = sender as BackgroundWorker;
            args.Result = ZnajdzPlikiWKatalogu(bw, Node);
        }

        private void d_d_a(string sciezka)
        {
            

            List<Zdjecie> zdjecia = new List<Zdjecie>();
            List<string> pliki = new List<string>();
            List<string> pliki_przefiltrowane = new List<string>();

            

            try
            {
                pliki.AddRange(Directory.GetFiles(sciezka, "*.jpg"));
                pliki.AddRange(Directory.GetFiles(sciezka, "*.jpeg"));
                pliki.AddRange(Directory.GetFiles(sciezka, "*.tif"));
                //pliki.AddRange(Directory.GetFiles(sciezka, "*.tiff"));

                //MessageBox.Show(sciezka);

                pliki.Sort();

                MessageBox.Show("" + pliki.Count);
                MessageBox.Show(sciezka);


                for (int i = 0; i < pliki.Count; i++)
                {
                    try
                    {
                        if ((pliki[i].ToLower().LastIndexOf(".jpg") != -1 && pliki[i].ToLower().LastIndexOf(".jpg") == (pliki[i].Length - 4)) || (pliki[i].ToLower().LastIndexOf(".jpeg") != -1 && pliki[i].ToLower().LastIndexOf(".jpeg") == (pliki[i].Length - 5)) || (pliki[i].ToLower().LastIndexOf(".tif") != -1 && pliki[i].ToLower().LastIndexOf(".tif") == (pliki[i].Length - 4)) || (pliki[i].ToLower().LastIndexOf(".tiff") != -1 && pliki[i].ToLower().LastIndexOf(".tiff") == (pliki[i].Length - 5)))
                        {
                            //if(pliki[i].ToLower().LastIndexOf(".tif") == (pliki[i].Length - 5))
                            //  continue;
                            //Zdjecie z = new Zdjecie(pliki[i]);
                            pliki_przefiltrowane.Add(pliki[i]);
                            //bw.ReportProgress(0, z);
                            //zdjecia.Add(z);
                        }
                    }
                    catch (ArgumentException)
                    {
                        //MessageBox.Show("testowo: plik nie jest w poprawnym formacie ");
                        MessageBox.Show("Plik: \"" + pliki[i].Substring(pliki[i].LastIndexOf("\\") + 1) + "\" mimo poprawnego rozszezenie nie zawiera zdjêcia", pliki[i], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                MessageBox.Show("" + zdjecia.Count);


                if (pliki_przefiltrowane.Count != 0)
                {
                    //dodaj_do_albumu(zdjecia, sciezka);
                    dodaj_kolekcje_do_bazy(pliki_przefiltrowane);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " - Odmowa dostêpu", sciezka, MessageBoxButtons.OK, MessageBoxIcon.Error);

                DirTreeNode dd = new DirTreeNode("napis");

                this.SelectedNode = dd;
            }
            //return zdjecia.ToArray();
        }

        private void dodaj_do_albumu(List<Zdjecie> lista, string sciezka)
        {
            Db baza = new Db();
            bool stan = true;
            int i=-1;

            if(lista.Count != 0)
                i = 0;

            baza.Polacz();

            try
            {
                DataSet ds = baza.Select("select nazwa_pliku from Zdjecie where sciezka=\'"+sciezka+"\' order by nazwa_pliku");

                foreach (DataTable t in ds.Tables)
                {                    
                    if (t.Rows.Count != lista.Count)
                    {
                        stan = false;
                    }
                    else
                    {
                        foreach (DataRow r in t.Rows)
                        {
                            if (i > -1 && i < lista.Count)
                            {
                                if (lista[i].NazwaPliku != ""+r[0])
                                {
                                    stan = false;
                                    break;
                                }
                                i++;
                            }
                        }
                    }

                    if (stan == false)
                    {
                        if (t.Rows.Count == 0)
                        {
                            //dodaj do bazy
                            MessageBox.Show("dodaj do bazy");

                            foreach (Zdjecie zd in lista)
                            {
                                baza.Insert_czesci("Zdjecie", "sciezka,nazwa_pliku", "\'" + sciezka + "\',\'" + zd.NazwaPliku + "\'");
                            }
                        }
                        else
                        {
                            MessageBox.Show("poszukaj zdjec");
                            //zawartosc bazy opisujace ten katalog nie zawiera wszyskich zdjec lub zawiera wiecej zdjec
                        }
                    }       
                    else
                    {
                        MessageBox.Show("jest ok");
                    }
                }               
            }
            catch (SqlException)
            {
                MessageBox.Show("blad prawdopodobnie w bazie");
            }

            baza.Rozlacz();
        }

        private List<string> Przefiltruj(string sciezka)
        {            
            List<string> pliki = new List<string>();
            List<string> pliki_przefiltrowane = new List<string>();
                       

            try
            {
                pliki.AddRange(Directory.GetFiles(sciezka, "*.jpg"));
                pliki.AddRange(Directory.GetFiles(sciezka, "*.jpeg"));
                pliki.AddRange(Directory.GetFiles(sciezka, "*.tif"));

                pliki.Sort();

                for (int i = 0; i < pliki.Count; i++)
                {
                    try
                    {
                        if ((pliki[i].ToLower().LastIndexOf(".jpg") != -1 && pliki[i].ToLower().LastIndexOf(".jpg") == (pliki[i].Length - 4)) || (pliki[i].ToLower().LastIndexOf(".jpeg") != -1 && pliki[i].ToLower().LastIndexOf(".jpeg") == (pliki[i].Length - 5)) || (pliki[i].ToLower().LastIndexOf(".tif") != -1 && pliki[i].ToLower().LastIndexOf(".tif") == (pliki[i].Length - 4)) || (pliki[i].ToLower().LastIndexOf(".tiff") != -1 && pliki[i].ToLower().LastIndexOf(".tiff") == (pliki[i].Length - 5)))
                        {                            
                            pliki_przefiltrowane.Add(pliki[i]);                         
                        }
                    }
                    catch (ArgumentException)
                    {                        
                        MessageBox.Show("Plik: \"" + pliki[i].Substring(pliki[i].LastIndexOf("\\") + 1) + "\" mimo poprawnego rozszezenie nie zawiera zdjêcia", pliki[i], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " - Odmowa dostêpu", sciezka, MessageBoxButtons.OK, MessageBoxIcon.Error);

                DirTreeNode dd = new DirTreeNode("napis");

                this.SelectedNode = dd;
            }

            return pliki_przefiltrowane;

        }
           

        private Zdjecie[] WybierzPlikiZdjec(BackgroundWorker bw, DirTreeNode Node)
        {
            List<Zdjecie> zdjecia = new List<Zdjecie>();
            List<string> pliki = new List<string>();

            try
            {
                pliki.AddRange(Directory.GetFiles(Node.Path, "*.jpg"));
                pliki.AddRange(Directory.GetFiles(Node.Path, "*.jpeg"));
                pliki.AddRange(Directory.GetFiles(Node.Path, "*.tif"));
                //pliki.AddRange(Directory.GetFiles(Node.Path, "*.tiff"));

                //MessageBox.Show(Node.Path);

                pliki.Sort();

                for (int i = 0; i < pliki.Count; i++)
                {
                    try
                    {
                        if ((pliki[i].ToLower().LastIndexOf(".jpg") != -1 && pliki[i].ToLower().LastIndexOf(".jpg") == (pliki[i].Length - 4)) || (pliki[i].ToLower().LastIndexOf(".jpeg") != -1 && pliki[i].ToLower().LastIndexOf(".jpeg") == (pliki[i].Length - 5)) || (pliki[i].ToLower().LastIndexOf(".tif") != -1 && pliki[i].ToLower().LastIndexOf(".tif") == (pliki[i].Length - 4)) || (pliki[i].ToLower().LastIndexOf(".tiff") != -1 && pliki[i].ToLower().LastIndexOf(".tiff") == (pliki[i].Length - 5)))
                        {
                            //if(pliki[i].ToLower().LastIndexOf(".tif") == (pliki[i].Length - 5))
                              //  continue;
                            Zdjecie z = new Zdjecie(pliki[i]);
                            bw.ReportProgress(0, z);
                            zdjecia.Add(z);
                        }
                    }
                    catch (ArgumentException)
                    {
                        //MessageBox.Show("testowo: plik nie jest w poprawnym formacie ");
                        MessageBox.Show("Plik: \"" + pliki[i].Substring(pliki[i].LastIndexOf("\\") + 1) + "\" mimo poprawnego rozszezenie nie zawiera zdjêcia", pliki[i], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                if (zdjecia.Count != 0)
                {
                    //dodaj_do_albumu(zdjecia, Node.Path);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " - Odmowa dostêpu", Node.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                DirTreeNode dd = new DirTreeNode("napis");

                this.SelectedNode = dd;
            }
            return zdjecia.ToArray();
        }

        private Zdjecie[] ZnajdzPlikiWKatalogu(BackgroundWorker bw, DirTreeNode Node)
        {
            List<Zdjecie> lista = new List<Zdjecie>();
            //lista.Add(new Zdjecie("c:\\IM000271.jpg"));
            //lista.Add(new Zdjecie("c:\\IM000271t.jpg"));

            //return lista.ToArray();

            List<string> katal_tab = new List<string>();
            List<Katalog> katalogi = new List<Katalog>();

            if(Node.FullPath.LastIndexOf("\\") > 4)
            {
                //MessageBox.Show(Node.Path);

                string s1 = "";
                char[] s2 = new char[Node.Path.Length];

                katal_tab.AddRange(Directory.GetDirectories(Node.Path));

                Node.Path.CopyTo(0,s2,0,Node.Path.LastIndexOf("\\"));

                for (int i = 0; i < s2.Length; i++)
                {
                    s1 += s2[i];
                }

                if (s2.Length == 2)
                {
                    katalogi.Add(new Katalog(s1 + "\\", true));
                }
                else
                {
                    katalogi.Add(new Katalog(s1, true));
                }                

                foreach (string t in katal_tab)
                {                 
                    katalogi.Add(new Katalog(t,false));
                }
            }


            Db baza = new Db();

            string m1 = "tagzdjecia:   ", m2 = "tag:   ", m3 = "zdjecie:   ";

            baza.Polacz();
            try
            {
                DataSet ds = baza.Select("select * from TagZdjecia");

                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        foreach (DataColumn c in t.Columns)
                            m1 += c.ColumnName + "=" + r[c.ColumnName] + "  !!  ";
                        
                    }
                }

                ds = baza.Select("select * from Tag");

                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        foreach (DataColumn c in t.Columns)
                            m2 += c.ColumnName + "=" + r[c.ColumnName] + "  !!  ";

                    }
                }

                ds = baza.Select("select * from Zdjecie");

                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        foreach (DataColumn c in t.Columns)
                            m3 += c.ColumnName + "=" + r[c.ColumnName] + "  !!  ";

                    }
                }

                //MessageBox.Show(m1);
                //MessageBox.Show(m2);
                //MessageBox.Show(m3);
            }
            catch (SqlException)
            {

            }


            baza.Rozlacz();


            return WybierzPlikiZdjec(bw,Node);
           
        }

        protected override void OnAfterExpand(TreeViewEventArgs e)
        {
            base.OnAfterExpand(e);
            

            if (e.Node.Text.IndexOf("A:\\") == 0 && e.Node.Text.LastIndexOf("\\") < 4)
            {
                e.Node.Text = "A:\\ " + "[" + DirTreeNode.Etykieta(e.Node.Text.Substring(0, 3)) + "]";
                e.Node.ImageIndex = Dyskietka_z;
                e.Node.SelectedImageIndex = Dyskietka_z;                
            }
            else if (e.Node.Text.IndexOf("B:\\") == 0 && e.Node.Text.LastIndexOf("\\") < 4)
            {
                e.Node.Text = "B:\\ " + "[" + DirTreeNode.Etykieta(e.Node.Text.Substring(0, 3)) + "]";
                e.Node.ImageIndex = Dyskietka_z;
                e.Node.SelectedImageIndex = Dyskietka_z;                
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            //e.Equals = 
        }

        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            base.OnNodeMouseClick(e);

            if (e.Button == MouseButtons.Right)
            {
                Context.Items.Clear();
                ToolStripItem toolStripItem = Context.Items.Add("Dodaj zawartosc katalogu " + ((DirTreeNode)e.Node).Path + " do kolekcji");
                toolStripItem.ToolTipText = ((DirTreeNode)e.Node).Path;
                toolStripItem.Click += new EventHandler(DodajDoKolekcji);
                toolStripItem = Context.Items.Add("Dodaj zawartosc katalogu " + ((DirTreeNode)e.Node).Path + " do Albumu");
                toolStripItem.ToolTipText = ((DirTreeNode)e.Node).Path;
                toolStripItem.Click += new EventHandler(DodajDoAlbumu);


                Context.Show(this, new Point(e.X, e.Y));             
            }
        }

        private void dodaj_kolekcje_do_bazy(List<string> lista)
        {
            Db baza = new Db();
            Int64 max = 0;

            baza.Polacz();
            try
            {
                DataSet ds = baza.Select("select max(id_zdjecia) from Zdjecie");

                foreach (DataTable t in ds.Tables)
                {                  
                    foreach (DataRow r in t.Rows)
                    {
                        try
                        {
                            max = (Int64)r[0];
                        }
                        catch (Exception)
                        {
                            max = -1;
                        }
                    }                
                }
                //MessageBox.Show("" + max);

                if (max == -1)
                    max = 1;
                else
                    max++;


                Dictionary<string, string> tablica;
                //string[][] tablica;

                foreach(string n in lista)
                {
                    try
                    {

                        tablica = Zdjecie.PobierzExifDoBazy(n);

                        try
                        {
                            MessageBox.Show(tablica["autor"]);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("autora nie ma");
                        }

                        try
                        {
                            MessageBox.Show(tablica["comment"]);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("comment nie ma");
                        }

                        try
                        {
                            MessageBox.Show(tablica["komentarz"]);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("komentarza nie ma");
                        }

                        try
                        {
                            MessageBox.Show(tablica["data_wykonania"]);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("data_wykonania nie ma");
                        }

                        try
                        {
                            MessageBox.Show(tablica["orientacja"]);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("orientacja nie ma");
                        }

                        try
                        {
                            MessageBox.Show(tablica["orientation"]);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("orientation nie ma");
                        }

                        /*foreach (KeyValuePair<string, string> s in tablica)
                        {
                            MessageBox.Show((string)s.Key);
                            MessageBox.Show((string)s.Value);
                        }*/
                        //MessageBox.Show("" + tablica["data_wykonania"]);

                    }
                    catch (Exception)
                    {

                    }
                }

            }
            catch (SqlException)
            {

            }


            baza.Rozlacz();

        }

        internal void DodajDoKolekcji(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;
            //MessageBox.Show("Dodaje zawartosc katalogu " + mn.ToolTipText + " do kolekcji!");
            //Dodaj_do_kolekcji ddk = new Dodaj_do_kolekcji(mn.ToolTipText);
            //ddk.Show();
            //Dodaj_katalog_do_bazy ddk = new Dodaj_katalog_do_bazy(mn.ToolTipText,this);
            //ddk.Show();

            List<string> lista = Przefiltruj(mn.ToolTipText);

            dodaj_kolekcje_do_bazy(lista);

        }

        internal void DodajDoAlbumu(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;
            //MessageBox.Show("Dodaje zawartosc katalogu " + mn.ToolTipText + " do kolekcji!");
            //Dodaj_do_kolekcji ddk = new Dodaj_do_kolekcji(mn.ToolTipText);
            //ddk.Show();
            //Dodaj_katalog_do_bazy ddk = new Dodaj_katalog_do_bazy(mn.ToolTipText,this);
            //ddk.Show();
            d_d_a(mn.ToolTipText);
            

        } 

        protected override void OnMouseClick(MouseEventArgs e)
        {   

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FileTree
            // 
            this.LabelEdit = true;
            this.ResumeLayout(false);

        }
    }
}