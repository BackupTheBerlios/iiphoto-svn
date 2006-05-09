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
using Photo;

namespace Photo
{
    public class FileTree : TreeView, IWyszukiwacz
    {        
        public FileTree()
        {
            GenerateImage();            
            this.BackColor = Color.Beige;
        }

        public const int Dysk = 0;
        public const int FOLDER = 1;
        public const int Dyskietka = 2;
        public const int Cdrom = 3;
        public const int Cdrom_z = 4;
        public const int Dyskietka_z = 5;

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

            foreach (string tempDrive in drives)
            {
                DirTreeNode dn = new DirTreeNode(tempDrive);
                Nodes.Add(dn);
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
                tn.populate();
                EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - Odmowa dostêpu", e.Node.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                
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

            internal void populate()
            {
                ArrayList folder = new ArrayList();

                //string

                string[] files = Directory.GetDirectories(Path);
                Array.Sort(files);

                if (files.Length == 0 && type == Dysk)
                    return;

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

        private Zdjecie[] ZnajdzPlikiWKatalogu(BackgroundWorker bw, DirTreeNode Node)
        {
            List<Zdjecie> zdjecia = new List<Zdjecie>();
            List<string> pliki = new List<string>();

            try
            {
                pliki.AddRange(Directory.GetFiles(Node.Path, "*.jpg"));
                pliki.AddRange(Directory.GetFiles(Node.Path, "*.jpeg"));
                pliki.AddRange(Directory.GetFiles(Node.Path, "*.tif"));
                pliki.AddRange(Directory.GetFiles(Node.Path, "*.tiff"));

                pliki.Sort();

                for (int i = 0; i < pliki.Count; i++)
                {
                    try
                    {
                        if ((pliki[i].ToLower().LastIndexOf(".jpg") != -1 && pliki[i].ToLower().LastIndexOf(".jpg") == (pliki[i].Length - 4)) || (pliki[i].ToLower().LastIndexOf(".jpeg") != -1 && pliki[i].ToLower().LastIndexOf(".jpeg") == (pliki[i].Length - 5)) || (pliki[i].ToLower().LastIndexOf(".tif") != -1 && pliki[i].ToLower().LastIndexOf(".tif") == (pliki[i].Length - 4)) || (pliki[i].ToLower().LastIndexOf(".tiff") != -1 && pliki[i].ToLower().LastIndexOf(".tiff") == (pliki[i].Length - 5)))                        
                        {
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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " - Odmowa dostêpu", Node.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);



                /*if (Node.Text.Length == 3)
                {
                    if (Node.Text.CompareTo("A:\\") == 0 || Node.Text.CompareTo("B:\\") == 0 )
                    {
                        Node.ImageIndex = Dyskietka;
                        Node.SelectedImageIndex = Dyskietka;
                    }
                }
*/
                DirTreeNode dd = new DirTreeNode("napis");

                this.SelectedNode = dd;
                //this.SelectedNode = null;


            }
            return zdjecia.ToArray();
           
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