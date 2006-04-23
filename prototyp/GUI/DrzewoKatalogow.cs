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
        const int HD = 0;
        const int FOLDER = 1;

        private IWyszukiwanie W;

        public void GenerateImage()
        {
            ImageList list = new ImageList();
            list.Images.Add(Properties.Resources.hd);
            list.Images.Add(Properties.Resources.folder);

            ImageList = list;
        }
        public void Fill()
        {
            BeginUpdate();
            string[] drives = Directory.GetLogicalDrives();
            for (int i = 0; i < drives.Length; i++)
            {
                DirTreeNode dn = new DirTreeNode(drives[i]);
                dn.ImageIndex = HD;

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
                type = HD;
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

                if (files.Length == 0 && type == HD)
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
                //string[] files4 = Directory.GetFiles(e.Node.FullPath, "*.tiff");

                Zdjecie[] zdjecia = new Zdjecie[files.Length + files2.Length + files3.Length];
                int i = 0;
                int ile = 0;
                for (i = 0; i < files.Length; i++)
                {
                    zdjecia[i] = new Zdjecie(files[i]);
                }
                ile = i;
                for (i = 0; i < files2.Length; i++)
                {
                    zdjecia[ile + i] = new Zdjecie(files2[i]);
                }
                ile += i;
                for (i = 0; i < files3.Length; i++)
                {
                    zdjecia[ile + i] = new Zdjecie(files3[i]);
                }
                /*ile += i;
                for (i = 0; i < files4.Length; i++)
                {
                    zdjecia[ile + i] = new Zdjecie(files4[i]);
                }*/
                if (ZakonczonoWyszukiwanie != null)
                    ZakonczonoWyszukiwanie(zdjecia);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Odmowa dostêpu", e.Node.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}