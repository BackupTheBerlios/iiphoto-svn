using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace WindowsApplication3
{
    public partial class Form1 : Form
    {
        private FileTree tr;

        public Form1()
        {
            InitializeComponent();
            Init_Tree();            
        }

        private void Init_Tree()
        {
            tr = new FileTree();            
            tr.Height = tabPage1.Height;
            tr.Width = tabPage1.Width;
            tr.Visible = true;
            tabPage1.Controls.Add(tr);  
            tr.AfterSelect += new TreeViewEventHandler(tr_AfterSelect);
            //this.toolStripComboBox1.Click += new System.EventHandler(this.toolStripComboBox1_Click);
        }

        private void toolStripContainer1_LeftToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        

        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
        private void tr_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //MessageBox.Show(e.Node.ToString);//e.Node);
            MessageBox.Show(""+e.Node.FullPath); //sender.ToString
        }

       
    }

    
}