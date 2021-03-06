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
            //Fill();
            this.BackColor = Color.Beige;
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

            //string[] tempString = Directory.GetLogicalDrives();

            //DriveInfo tempInfo = new DriveInfo("", 0, "My Computer");
            //availableDrives.Add(tempInfo);

            foreach (string tempDrive in drives)
            {
                /* int tempType = getDriveType(tempDrive);
                 string tempName = GetDriveName(tempDrive);
                 tempInfo = new DriveInfo(tempDrive, tempType, tempName);
                 availableDrives.Add(tempInfo);
                 */
                //string tempName = GetDriveName(tempDrive);

                DirTreeNode dn = new DirTreeNode(tempDrive);

                //dn.Text = tempName + "(" + tempDrive + ")";
                Nodes.Add(dn);

            }

            //Directory.g

            //for (int i = 0; i < drives.Length; i++)
            //{
            //drives[i].GetType

            //}
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

                MessageBox.Show(ex.Message);
                //tn.Nodes.Add(new TreeNode());
                //tn.Nodes.Clear();
                tn.Collapse();
                //tn.Parent.Collapse();
                //tn.Nodes.Clear();
                tn.Collapse();
                e.Node.Remove();
                //tn.
                //Nodes.Add(tn);
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
            Nodes.RemoveAt(0);
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
                    //dn = new DirTreeNode(tempDrive, tempDrive + " [Floppy]");
                    // if (GetDriveName(s) != "")
                    //{
                    // Text = s + " [Floppy]";
                    //Text = s + " [" + GetDriveName(s) + "]";
                    // ImageIndex = Dyskietka_z;
                    // SelectedImageIndex = Dyskietka_z;
                    // }
                    // else
                    // {
                    Text = s + " [Floppy]";
                    ImageIndex = Dyskietka;
                    SelectedImageIndex = Dyskietka;
                    // }                    
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
                //type = Dysk;
                //ImageIndex = type;
                //SelectedImageIndex = type;
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

            const int Dysk = 0;
            const int FOLDER = 1;
            const int Dyskietka = 2;
            const int Cdrom = 3;
            const int Cdrom_z = 4;
            const int Dyskietka_z = 5;

            //private IWyszukiwanie Wsad;




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

            public string GetDriveName(string drive)
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

            public string Etykieta(string drive)
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

                //wyjate


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

            DirTreeNode dn = new DirTreeNode("napis");

            if (e.Node.Text.IndexOf("A:\\") == 0 && e.Node.Text.LastIndexOf("\\") < 4)
            {
                e.Node.Text = "A:\\ " + "[" + dn.Etykieta(e.Node.Text.Substring(0, 3)) + "]";
                e.Node.ImageIndex = 5;
                e.Node.SelectedImageIndex = 5;
            }
            else if (e.Node.Text.IndexOf("B:\\") == 0 && e.Node.Text.LastIndexOf("\\") < 4)
            {
                e.Node.Text = "B:\\ " + "[" + dn.Etykieta(e.Node.Text.Substring(0, 3)) + "]";
                e.Node.ImageIndex = 5;
                e.Node.SelectedImageIndex = 5;
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

            try
            {
                string[] files = Directory.GetFiles(Node.Path, "*.jpg");
                string[] files2 = Directory.GetFiles(Node.Path, "*.jpeg");
                string[] files3 = Directory.GetFiles(Node.Path, "*.tif");
                string[] files4 = Directory.GetFiles(Node.Path, "*.tiff");

                string[] f1 = new string[files.Length];
                string[] f2 = new string[files2.Length];
                string[] f3 = new string[files3.Length];
                string[] f4 = new string[files4.Length];


                int i = 0;

                int z_f1 = 0;
                for (i = 0; i < files.Length; i++)
                {
                    if (files[i].ToLower().LastIndexOf(".jpg") == (files[i].Length - 4))
                    {
                        f1[z_f1] = files[i];
                        z_f1++;
                    }
                }
                int z_f2 = 0;
                for (i = 0; i < files2.Length; i++)
                {
                    //string str = files2[i].ToLower();
                    if (files2[i].ToLower().LastIndexOf(".jpeg") == (files2[i].Length - 5))
                    {
                        f2[z_f2] = files2[i];
                        z_f2++;
                    }
                }
                int z_f3 = 0;
                for (i = 0; i < files3.Length; i++)
                {
                    if (files3[i].ToLower().LastIndexOf(".tif") == (files3[i].Length - 4))
                    {
                        f3[z_f3] = files3[i];
                        z_f3++;
                    }
                }

                int z_f4 = 0;
                for (i = 0; i < files4.Length; i++)
                {
                    if (files4[i].ToLower().LastIndexOf(".tiff") == (files4[i].Length - 5))
                    {
                        f4[z_f4] = files4[i];
                        z_f4++;
                    }
                }

                zdjecia = new List<Zdjecie>(z_f1 + z_f2 + z_f3 + z_f4);

                int ile = 0;
                for (i = 0; i < z_f1; i++)
                {
                    try
                    {
                        Zdjecie z = new Zdjecie(f1[i]);
                        bw.ReportProgress(0, z);
                        zdjecia.Add(z);
                    }
                    catch (ArgumentException)
                    {
                        //MessageBox.Show("testowo: plik nie jest w poprawnym formacie ");
                        MessageBox.Show("Plik: \"" + files[i].Substring(files[i].LastIndexOf("\\") + 1) + "\" mimo poprawnego rozszezenie nie zawiera zdj�cia", files[i], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                ile = i;
                for (i = 0; i < z_f2; i++)
                {
                    try
                    {
                        Zdjecie z = new Zdjecie(f2[i]);
                        bw.ReportProgress(0, z);
                        zdjecia.Add(z);
                    }
                    catch (ArgumentException)
                    {
                        //MessageBox.Show("testowo: plik nie jest w poprawnym formacie ");

                        MessageBox.Show("Plik: \"" + files2[i].Substring(files2[i].LastIndexOf("\\") + 1) + "\" mimo poprawnego rozszezenie nie zawiera zdj�cia", files2[i], MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
                ile += i;
                for (i = 0; i < z_f3; i++)
                {
                    try
                    {
                        Zdjecie z = new Zdjecie(f3[i]);
                        bw.ReportProgress(0, z);
                        zdjecia.Add(z);
                    }
                    catch (ArgumentException)
                    {
                        //MessageBox.Show("testowo: plik nie jest w poprawnym formacie ");
                        MessageBox.Show("Plik: \"" + files3[i].Substring(files3[i].LastIndexOf("\\") + 1) + "\" mimo poprawnego rozszezenie nie zawiera zdj�cia", files3[i], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                ile += i;
                for (i = 0; i < z_f4; i++)
                {
                    try
                    {
                        Zdjecie z = new Zdjecie(f4[i]);
                        bw.ReportProgress(0, z);
                        zdjecia.Add(z);
                    }
                    catch (ArgumentException)
                    {
                        //MessageBox.Show("testowo: plik nie jest w poprawnym formacie ");
                        MessageBox.Show("Plik: \"" + files4[i].Substring(files4[i].LastIndexOf("\\") + 1) + "\" mimo poprawnego rozszezenie nie zawiera zdj�cia", files4[i], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " - Odmowa dost�pu", Node.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                /*if (e.Node.Text.Length == 3)
                {
                    if (e.Node.Text.CompareTo("A:\\") == 0 || e.Node.Text.CompareTo("B:\\") == 0 )
                    {

                    }
                    MessageBox.Show("numer: "+e.Node.SelectedImageIndex);
                    //e.Node.ImageIndex = getDriveType(e.Node.Text);

                    this.Refresh();
                    //this.


                }*/

                DirTreeNode dd = new DirTreeNode("napis");

                this.SelectedNode = dd;


            }

            return zdjecia.ToArray();
        }

        protected override void OnAfterExpand(TreeViewEventArgs e)
        {
            base.OnAfterExpand(e);


            DirTreeNode dn = new DirTreeNode("napis");

            if (e.Node.Text.IndexOf("A:\\") == 0 && e.Node.Text.LastIndexOf("\\") < 4)
            {
                e.Node.Text = "A:\\ " + "[" + dn.Etykieta(e.Node.Text.Substring(0, 3)) + "]";
                e.Node.ImageIndex = 5;
                e.Node.SelectedImageIndex = 5;
            }
            else if (e.Node.Text.IndexOf("B:\\") == 0 && e.Node.Text.LastIndexOf("\\") < 4)
            {
                e.Node.Text = "B:\\ " + "[" + dn.Etykieta(e.Node.Text.Substring(0, 3)) + "]";
                e.Node.ImageIndex = 5;
                e.Node.SelectedImageIndex = 5;
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