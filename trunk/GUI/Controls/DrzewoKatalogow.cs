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
        private const int Pictures_2 = 6;
        private const int Cdrom_z_2 = 7;
        private const int Computer = 8;
        private const int Pulpit = 9;
        private const int User = 10;
        
        private bool czy_otwiera = false;

        private DirTreeNode kat_MKomputer;

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
            list.Images.Add(Properties.Resources.Pictures_2);
            list.Images.Add(Properties.Resources.Cdrom_z_2);
            list.Images.Add(Properties.Resources.Computer);
            list.Images.Add(Properties.Resources.Pulpit);   
            list.Images.Add(Properties.Resources.User);
            

            
            ImageList = list;
        }

        public void Fill()
        {
            BeginUpdate();

            DirTreeNode kat_Pulpit = new DirTreeNode(Config.katalogPulpit, Pulpit, true);
            kat_Pulpit.Text = "Pulpit";
            //kat_Pulpit.Path = Config.katalogPulpit;

            try
            {
                string[] n = Directory.GetDirectories(Config.katalogPulpit);

                if (n.Length == 0)
                    kat_Pulpit.Nodes.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + e.Message);
            }

            Nodes.Add(kat_Pulpit);

            DirTreeNode kat_MDokumenty = new DirTreeNode(Config.katalogMojeDokumenty,User,true);
            kat_MDokumenty.Text = "Moje Dokumenty";            

            try
            {
                string[] n = Directory.GetDirectories(Config.katalogMojeDokumenty);

                if (n.Length == 0)
                    kat_MDokumenty.Nodes.Clear();
                    
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + e.Message);
            }

            Nodes.Add(kat_MDokumenty);

            DirTreeNode kat_MObrazy = new DirTreeNode(Config.katalogMojeObrazy, Pictures_2, true);
            kat_MObrazy.Text = "Moje Obrazy";

            try
            {
                string[] n = Directory.GetDirectories(Config.katalogMojeObrazy);

                if (n.Length == 0)
                    kat_MObrazy.Nodes.Clear();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + e.Message);
            }

            Nodes.Add(kat_MObrazy);            

            kat_MKomputer = new DirTreeNode("Mój Komputer", Computer, true);
            kat_MKomputer.Text = "Mój Komputer";
            Nodes.Add(kat_MKomputer);

            //kat_MKomputer.Nodes.Clear();

            string[] drives = Directory.GetLogicalDrives();

            foreach (string tempDrive in drives)
            {
                DirTreeNode dn = new DirTreeNode(tempDrive);                
                kat_MKomputer.Nodes.Add(dn);
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

            MessageBox.Show(path);

            if(path.IndexOf("Mój Komputer") == 0)
                path = path.ToLower().Substring("Mój Komputer".Length, path.Length - "Mój Komputer".Length);
            else
                path = path.ToLower();

            MessageBox.Show(path);

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
                //Nodes.Insert(tn.Index, tn);
                kat_MKomputer.Nodes.Insert(tn.Index, tn);
                                
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
                        ImageIndex = Cdrom_z_2;
                        SelectedImageIndex = Cdrom_z_2;
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

            public DirTreeNode(string s, int aType, bool z)
                : base(s)
            {

                path = s.ToLower();
                type = aType;
                

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
                        ImageIndex = Cdrom_z_2;
                        SelectedImageIndex = Cdrom_z_2;
                    }
                    else
                    {
                        ImageIndex = getDriveType(s);
                        SelectedImageIndex = getDriveType(s);
                    }
                }
                ImageIndex = type;
                SelectedImageIndex = type;

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
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString() + e.Message);
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
                ArrayList folder = new ArrayList();;

                if (tn.FullPath != "Mój Komputer")
                {

                    DirTreeNode dnn = new DirTreeNode(tn.Text);
                    int gdzie = tn.Index;

                    string[] files = Directory.GetDirectories(Path);
                    Array.Sort(files);

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
                else
                {
                    string[] drives = Directory.GetLogicalDrives();

                    foreach (string tempDrive in drives)
                    {
                        DirTreeNode dn = new DirTreeNode(tempDrive);
                        folder.Add(dn);                        
                    }
                    Nodes.Clear();
                    foreach (DirTreeNode dtn in folder)
                    {
                        Nodes.Add(dtn);
                    }
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
            czy_otwiera = false;
            //MessageBox.Show("numer: " + e.Node.SelectedImageIndex);
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);
            //czy_otwiera = true;
        }

        internal void d_d_a(string sciezka)
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

        internal List<string> Przefiltruj(string sciezka)
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
           

        private Zdjecie[] WybierzPlikiZdjec(DirTreeNode Node)
        {
            List<Zdjecie> zdjecia = new List<Zdjecie>();
            List<string> pliki = new List<string>();
            string path;

            try
            {                
                pliki.AddRange(Directory.GetFiles(Node.Path, "*.jpg"));
                pliki.AddRange(Directory.GetFiles(Node.Path, "*.jpeg"));
                pliki.AddRange(Directory.GetFiles(Node.Path, "*.tif"));
            
                pliki.Sort();

                for (int i = 0; i < pliki.Count; i++)
                {
                    try
                    {
                        if ((pliki[i].ToLower().LastIndexOf(".jpg") != -1 && pliki[i].ToLower().LastIndexOf(".jpg") == (pliki[i].Length - 4)) || (pliki[i].ToLower().LastIndexOf(".jpeg") != -1 && pliki[i].ToLower().LastIndexOf(".jpeg") == (pliki[i].Length - 5)) || (pliki[i].ToLower().LastIndexOf(".tif") != -1 && pliki[i].ToLower().LastIndexOf(".tif") == (pliki[i].Length - 4)) || (pliki[i].ToLower().LastIndexOf(".tiff") != -1 && pliki[i].ToLower().LastIndexOf(".tiff") == (pliki[i].Length - 5)))
                        {
                            Zdjecie z = new Zdjecie(pliki[i]);
                            //bw.ReportProgress(0, z);
                            zdjecia.Add(z);
                        }
                    }
                    catch (ArgumentException)
                    {
                        
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

        private Zdjecie[] ZnajdzPlikiWKatalogu(DirTreeNode Node)
        {
            List<Zdjecie> lista = new List<Zdjecie>();
            List<string> katal_tab = new List<string>();
            List<Katalog> katalogi = new List<Katalog>();

            if (Node.Path.Length > 4)
            {
                string s1 = "";
               
                katal_tab.AddRange(Directory.GetDirectories(Node.Path));

                s1 = Node.Path.Substring(0, Node.Path.LastIndexOf("\\"));

                if (s1.Length == 2)
                {
                    katalogi.Add(new Katalog(s1 + "\\", true));
                }
                else
                {
                    katalogi.Add(new Katalog(s1, true));
                }

                foreach (string t in katal_tab)
                {
                    katalogi.Add(new Katalog(t, false));
                }
            }      
            
            return WybierzPlikiZdjec(Node);           
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

            //czy_otwiera = false;
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
            else if (e.Button == MouseButtons.Left && czy_otwiera == false)
            {
                //MessageBox.Show(e.Node.FullPath);
                if (e.Node.Text != "Mój Komputer")
                {

                    if (RozpoczetoWyszukiwanie != null)
                        RozpoczetoWyszukiwanie(null);


                    Zdjecie[] zdjecia = ZnajdzPlikiWKatalogu((DirTreeNode)e.Node);
                    if (ZakonczonoWyszukiwanie != null)
                        ZakonczonoWyszukiwanie(zdjecia);

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

                //MessageBox.Show("" + e.Node.FullPath.IndexOf("Mój Komputer"));
            }

            czy_otwiera = false;
        }

        internal void dodaj_kolekcje_do_bazy(List<string> lista)
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
                        if (!(r[0] is DBNull)) 
                            max = (Int64)r[0];
                        else 
                            max = 0;
                    }                
                }
                //MessageBox.Show("" + max);

                max++;

                Dictionary<string, string> tablica;
                //string[][] tablica;
                string autor = "", komentarz = "", data_wykonania = "", orientacja = "", sciezka = "", nazwa_pliku = "";
                int orient = -1;

                foreach(string n in lista)
                {
                    try
                    {

                        tablica = Zdjecie.PobierzExifDoBazy(n);

                        if (tablica.ContainsKey("autor"))
                        {
                            autor = tablica["autor"];
                            //MessageBox.Show(autor);
                        }

                        if (tablica.ContainsKey("komentarz"))
                        {
                            komentarz = tablica["komentarz"];
                            //MessageBox.Show(komentarz);
                        }

                        if (tablica.ContainsKey("data_wykonania"))
                        {
                            data_wykonania = tablica["data_wykonania"];
                            //MessageBox.Show(data_wykonania);
                        }

                        if (tablica.ContainsKey("orientacja"))
                        {
                            orientacja = tablica["orientacja"];
                            if (orientacja == "Normal")
                                orient = 0;
                            else
                                orient = 1;
                            //MessageBox.Show(orientacja);
                            //MessageBox.Show("" + orient);
                        }

                        sciezka = n.Substring(0, n.LastIndexOf("\\"));
                        if (sciezka.Length == 2)
                            sciezka += "\\";

                        nazwa_pliku = n.Substring(n.LastIndexOf("\\") + 1, n.Length - n.LastIndexOf("\\") - 1);

                        //MessageBox.Show(sciezka);
                        //MessageBox.Show(nazwa_pliku);

                        //Zdjecie z = new Zdjecie(n);

                        //MessageBox.Show(z.IIPhotoTag);
                        

                        if (Zdjecie.ZwrocIIPhotoTag(n).Equals(""))
                        {
                            MessageBox.Show("dodaje tag");

                            //z.IIPhotoTag = "" + max;
                            Zdjecie.UstawIIPhotoTag(n, max.ToString());

                            try
                            {
                                baza.Insert_czesci("zdjecie", "sciezka,data_dodania,data_wykonania,komentarz,autor,nazwa_pliku,orientacja", "'" + sciezka + "',current_date,null,'" + komentarz + "','" + autor + "','" + nazwa_pliku + "'," + orient);
                            }
                            catch (SqlException)
                            {
                                MessageBox.Show("bladsql");
                            }
                        }

                        

                        /*foreach (KeyValuePair<string, string> s in tablica)
                        {
                            MessageBox.Show((string)s.Key);
                            MessageBox.Show((string)s.Value);
                        }*/
                        //MessageBox.Show("" + tablica["data_wykonania"]);

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }

                    max++;
                }

            }
            catch (SqlException e) 
            {
                MessageBox.Show(e.ToString());
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
            
            Dodaj_katalog_do_bazy ddk = new Dodaj_katalog_do_bazy(mn.ToolTipText,this);
            ddk.Show();            
        }
        
        protected override void OnAfterCollapse(TreeViewEventArgs e)
        {
            base.OnAfterCollapse(e);
            czy_otwiera = true;
        }

        protected override void OnBeforeCollapse(TreeViewCancelEventArgs e)
        {
            base.OnBeforeCollapse(e);
            czy_otwiera = true;
        }

        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            base.OnBeforeExpand(e);
            czy_otwiera = true;
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