using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
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
        const int Dysk = 0;
        const int FOLDER = 1;
        const int Dyskietka = 2;
        const int Cdrom = 3;        

        private IWyszukiwanie W;

        public void GenerateImage()
        {
            ImageList list = new ImageList();
            list.Images.Add(Properties.Resources.Dysk);
            list.Images.Add(Properties.Resources.folder);
            list.Images.Add(Properties.Resources.Dyskietka);
            list.Images.Add(Properties.Resources.Cdrom);            
            ImageList = list;
        }
        public void Fill()
        {
            BeginUpdate();
            string[] drives = Directory.GetLogicalDrives();

            //Directory.g

            for (int i = 0; i < drives.Length; i++)
            {
                //drives[i].GetType
                DirTreeNode dn = new DirTreeNode(drives[i]);
                dn.ImageIndex = Dysk;

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

                MessageBox.Show(ex.Message);
                //tn.Nodes.Add(new TreeNode());
                //tn.Nodes.Clear();
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

        public class DirTreeNode : TreeNode
        {
            string path;
            int type;
            public virtual string Path { get { return path; } }

            public DirTreeNode(string s)
                : base(s)
            {
                path = s.ToLower();
                setLeaf(true);
                type = Dysk;
                ImageIndex = type;
                SelectedImageIndex = type;
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

        #endregion

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);

            try
            {
                string[] files = Directory.GetFiles(e.Node.FullPath, "*.jpg");
                string[] files2 = Directory.GetFiles(e.Node.FullPath, "*.jpeg");
                string[] files3 = Directory.GetFiles(e.Node.FullPath, "*.tif");
                string[] files4 = Directory.GetFiles(e.Node.FullPath, "*.tiff");

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


                List<Zdjecie> zdjecia = new List<Zdjecie>(z_f1 + z_f2 + z_f3 + z_f4);
                
                int ile = 0;
                for (i = 0; i < z_f1; i++)
                {
                    try
                    {
                        Zdjecie z = new Zdjecie(f1[i]);
                        zdjecia.Add(z);
                    }
                    catch (ArgumentException)
                    {
                        //MessageBox.Show("testowo: plik nie jest w poprawnym formacie ");
                        MessageBox.Show("Plik: \"" + files[i].Substring(files[i].LastIndexOf("\\")+1) + "\" mimo poprawnego rozszezenie nie zawiera zdjêcia", files[i], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                ile = i;
                for (i = 0; i < z_f2; i++)
                {
                    try
                    {
                        Zdjecie z = new Zdjecie(f2[i]);
                        zdjecia.Add(z);
                    }
                    catch (ArgumentException)
                    {
                        //MessageBox.Show("testowo: plik nie jest w poprawnym formacie ");

                        MessageBox.Show("Plik: \"" + files2[i].Substring(files2[i].LastIndexOf("\\") + 1) + "\" mimo poprawnego rozszezenie nie zawiera zdjêcia", files2[i], MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
                ile += i;
                for (i = 0; i < z_f3; i++)
                {
                    try
                    {
                        Zdjecie z = new Zdjecie(f3[i]);
                        zdjecia.Add(z);
                    }
                    catch (ArgumentException)
                    {
                        //MessageBox.Show("testowo: plik nie jest w poprawnym formacie ");
                        MessageBox.Show("Plik: \"" + files3[i].Substring(files3[i].LastIndexOf("\\") + 1) + "\" mimo poprawnego rozszezenie nie zawiera zdjêcia", files3[i], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                ile += i;
                for (i = 0; i < z_f4; i++)
                {
                    try
                    {
                        Zdjecie z = new Zdjecie(f4[i]);
                        zdjecia.Add(z);
                    }
                    catch (ArgumentException)
                    {
                        //MessageBox.Show("testowo: plik nie jest w poprawnym formacie ");
                        MessageBox.Show("Plik: \"" + files4[i].Substring(files4[i].LastIndexOf("\\") + 1) + "\" mimo poprawnego rozszezenie nie zawiera zdjêcia", files4[i], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                if (ZakonczonoWyszukiwanie != null)
                    ZakonczonoWyszukiwanie(zdjecia.ToArray());
            }
            catch (Exception)
            {
                MessageBox.Show("Odmowa dostêpu", e.Node.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}