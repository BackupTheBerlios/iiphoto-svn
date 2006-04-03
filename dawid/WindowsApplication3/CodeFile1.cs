using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

public class FileTree : TreeView
{
    public FileTree()
    {
        GenerateImage();
        Fill();
        this.BackColor = Color.Beige;
    }
    const int HD = 0;
    const int FOLDER = 1;
    
    protected void GenerateImage()
    {
        ImageList list = new ImageList();
        list.Images.Add(new Icon("ico\\hd.ico"));
        list.Images.Add(new Icon("ico\\folder.ico"));
        
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
            EndUpdate();
            MessageBox.Show(ex.Message);
            tn.Nodes.Clear();
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

        public DirTreeNode(string s): base(s)
        {
            path = s.ToLower();
            setLeaf(true);
            type = HD;
            ImageIndex = type;
            SelectedImageIndex = type;
        }
        public DirTreeNode(string s, int aType): base(new FileInfo(s).Name)
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
            catch(Exception)
            {
            }
        }        

        internal void populate()
        {
            ArrayList folder = new ArrayList();           

            string[] files = Directory.GetDirectories(Path);
            
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
}