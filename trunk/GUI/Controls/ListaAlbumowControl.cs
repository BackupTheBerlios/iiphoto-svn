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
        private bool czy_otwiera = false, czy_otwarte_albumy = false, zaznaczony = false;
        //Dictionary<int, string> slownik;
        public List<Int64> lista_tagow, lista_albumow;
        public List<string> lista_nazw_tagow, lista_nazw_albumow;
        private int ilosc_zaznaczonych_albumow, ilosc_zaznaczonych_tagow;
        //private bool czy_wszystkie_tagi_ustawione = false, czy_wszystkie_albumy_ustawione = false;
        //private string albumy = "";
        private string odnalezionyPlik;

        public event ZmieninoTagiDelegate ZmienionoTagi;
        public event ZmienionoZrodloDelegate ZmienionoZrodlo;

        public ListaAlbumowControl()
        {
            InitializeComponent();
            Context = new ContextMenuStrip();

            lista_albumow = new List<Int64>();
            lista_tagow = new List<Int64>();
            lista_nazw_albumow = new List<string>();
            lista_nazw_tagow = new List<string>();
            ilosc_zaznaczonych_albumow = 0;
            ilosc_zaznaczonych_tagow = 0;

            Wypelnij();

            tagi.Expand();
            albumy.Expand();
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


                DataSet dataSet = baza.Select("select id_tagu,nazwa from Tag where album=1 order by nazwa asc;");

                foreach (DataTable t in dataSet.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        if (!(r[0] is DBNull))
                        {
                            alb.Nodes.Add(new TreeNode((string)r[1]));
                            lista_albumow.Add((Int64)r[0]);
                            lista_nazw_albumow.Add((string)r[1]);
                        }                        
                    }
                }

                dataSet = baza.Select("select nazwa,id_tagu from Tag where album=0 order by nazwa asc;");
                
                foreach (DataTable t in dataSet.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        if (!(r[0] is DBNull))
                        {
                            ta.Nodes.Add(new TreeNode((string)r[0]));
                            lista_tagow.Add((Int64)r[1]);
                            lista_nazw_tagow.Add((string)r[0]);                
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

        private Int64 znajdzNumer(string nazwa)
        {
            for (int i = 0; i < lista_nazw_tagow.Count; i++)
            {                
                if (lista_nazw_tagow[i] == nazwa)
                {             
                    return lista_tagow[i];
                }
            }
            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Zdjecie> zdjecia = null;
            List<Zdjecie> przefi = new List<Zdjecie>();

            if (ilosc_zaznaczonych_albumow != 0)
            {
                if (ilosc_zaznaczonych_tagow != 0)
                {
                    string sql = "";

                    foreach (TreeNode tr in tagi.Nodes)
                    {
                        if (tr.Checked == true)
                        {
                            sql += "and id_tagu=" + znajdzNumer(tr.Text) + " ";
                        }   
                    }

                    sql = sql.Substring(3, sql.Length - 3);

                    //MessageBox.Show(sql);

                    if (RozpoczetoWyszukiwanie != null)
                        RozpoczetoWyszukiwanie(null);

                    try
                    {
                        zdjecia = PokazPlikiZAlbumu(null, true);
                        //MessageBox.Show("dlugosc: " + zdjecia.Count);
                        Db baza = new Db();
                        baza.Polacz();

                        string tag;

                        

                        //MessageBox.Show("select * from TagZdjecia where id_zdjecia=" + "tag_zdjecia" + " and ( " + sql + " )");
                        int do_ilu = zdjecia.Count;

                        for (int i = 0; i < do_ilu; i++)
                        {
                            tag = zdjecia[i].Id;//zdjecia[i]..IIPhotoTag;

                            if (tag != "")
                            {
                                try
                                {
                                    DataSet ds = baza.Select("select max(id_zdjecia) from TagZdjecia where id_zdjecia = " + tag + " and (" + sql + ")");

                                    foreach (DataTable t in ds.Tables)
                                    {
                                        foreach (DataRow r in t.Rows)
                                        {
                                            if (!(r[0] is DBNull))
                                            {                                                
                                                przefi.Add(zdjecia[i]);
                                            }
                                        }
                                    }
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show("blad bazy: " + ex.Message);
                                }
                            }
                        }
                        baza.Rozlacz();

                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Brak dostêpu do wybranego katalogu.");
                        return;
                    }

                   // Zdjecie[] tablica = zdjecia.ToArray();

                    if (ZakonczonoWyszukiwanie != null)
                        ZakonczonoWyszukiwanie(przefi.ToArray(), new Katalog[0]);

                    //przefiltrowac
                }
                else
                {
                    if (RozpoczetoWyszukiwanie != null)
                        RozpoczetoWyszukiwanie(null);

                    try
                    {
                        zdjecia = PokazPlikiZAlbumu(null, true);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Brak dostêpu do wybranego katalogu.");
                        return;
                    }

                    if (ZakonczonoWyszukiwanie != null)
                        ZakonczonoWyszukiwanie(zdjecia.ToArray(), new Katalog[0]);
                    //wyswietlic pare albomow
                }



            }
            else
            {
                //tu przeladowac okienko z miniaturkami chyba
            }

            /*odswiez();
            IZdjecie[] zdjecia = Wyszukaj().PodajWynik();
            if (ZakonczonoWyszukiwanie != null)
                ZakonczonoWyszukiwanie(zdjecia);
             */
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

        private List<Zdjecie> ZwrocZdjeciaZAlbumu(TreeNode Node)
        {
            Db baza = new Db();

            Dictionary<Int64, string> nieOdnalezione = new Dictionary<long,string>();
            //List<Int64> nieOdnalezione = new List<Int64>();
            List<Zdjecie> lista = new List<Zdjecie>();

            DataSet ds = null;
            string pelna_sciezka;

            baza.Polacz();

            try
            {
                if (Node.FullPath.IndexOf("Albumy") == 0 && Node.FullPath.Length > "Albumy".Length)
                {
                    ds = baza.Select("select sciezka,nazwa_pliku,id_zdjecia from zdjecie where id_zdjecia in (select id_zdjecia from TagZdjecia where id_tagu in (select id_tagu from Tag where album=1 and nazwa=\'" + Node.FullPath.Substring("Albumy".Length + 1, Node.FullPath.Length - ("Albumy".Length + 1)) + "\'))");
                }
                else if (Node.FullPath.IndexOf("Albumy") == 0)
                {
                    ds = baza.Select("select sciezka,nazwa_pliku,id_zdjecia from zdjecie where id_zdjecia in (select id_zdjecia from TagZdjecia where id_tagu in (select id_tagu from Tag where album=1))");
                }
                pelna_sciezka = "";

                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        if (!(r[0] is DBNull))
                        {
                            pelna_sciezka = r[0] + "\\" + r[1];

                            if (System.IO.File.Exists(pelna_sciezka) == true)
                            {
                                Zdjecie z = new Zdjecie(pelna_sciezka);
                                lista.Add(z);
                            }
                            else
                            {
                                nieOdnalezione.Add((Int64)r[2], pelna_sciezka);                                
                            }
                        }
                    }
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("blad bazy");
            }

            baza.Rozlacz();

            if (nieOdnalezione.Count > 0)
            {
                foreach (KeyValuePair<long, string> kv in nieOdnalezione)
                {
                    ZnajdzPliki zp = new ZnajdzPliki(kv);
                    zp.FormClosing += new FormClosingEventHandler(zp_FormClosing);
                    DialogResult dr = zp.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        Console.WriteLine(odnalezionyPlik + " OK");
                        //Zdjecie odnalezioneZdjecie = new Zdjecie(odnalezionyPlik);
                        /*odnalezioneZdjecie.
                         * Tutaj trzeba cos zrobic z wyszukanym zdjeciem
                        lista.Add(new Zdjecie(odnalezionyPlik))*/
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        Console.WriteLine("CANCEL");
                    }
                    else if (dr == DialogResult.Abort)
                    {
                        Console.WriteLine("ABORT");
                        break;
                    }
                }
            }

            return lista;
        }

        void zp_FormClosing(object sender, FormClosingEventArgs e)
        {
            odnalezionyPlik = ((ZnajdzPliki)sender).plikOdnaleziony;
        }
        
        private List<Zdjecie> PokazPlikiZAlbumu(TreeNode Node, bool czy_kilka) //czy_kilka jak true to kilka albumow sie zwraca
        {
            string albumy_string = "";

            if (czy_kilka == false)
            {
                if (Node.FullPath != "Albumy")
                {
                    if (ZmienionoZrodlo != null)
                        ZmienionoZrodlo("Album: " + Node.FullPath.Substring("Albumy".Length + 1, Node.FullPath.Length - ("Albumy".Length + 1)));
                }
                else
                {
                    albumy_string = "Albumy: ";

                    foreach (TreeNode tr in albumy.Nodes)
                    {
                        albumy_string += tr.FullPath.Substring("Albumy".Length + 1, tr.FullPath.Length - ("Albumy".Length + 1)) + ", ";
                    }

                    if (ZmienionoZrodlo != null)
                        ZmienionoZrodlo(albumy_string.Substring(0, albumy_string.Length - 2));
                }
                 
                return ZwrocZdjeciaZAlbumu(Node);
            }
            else
            {
                List<Zdjecie> lista_zdjec = new List<Zdjecie>();

                albumy_string = "Albumy: ";

                foreach (TreeNode tr in albumy.Nodes)
                {
                    if (tr.Checked == true)
                    {
                        albumy_string += tr.FullPath.Substring("Albumy".Length + 1, tr.FullPath.Length - ("Albumy".Length + 1)) + ", ";
                        lista_zdjec.AddRange(ZwrocZdjeciaZAlbumu(tr));//MessageBox.Show("dlugosc: " + lista_zdjec.Count);                        
                    }
                }

                if (ZmienionoZrodlo != null)
                    ZmienionoZrodlo(albumy_string.Substring(0, albumy_string.Length - 2));

                return lista_zdjec;
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

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
                for (int i = 0; i < lista_nazw_albumow.Count; i++)
                {
                    if (lista_nazw_albumow[i] == mn.ToolTipText)
                    {
                        baza.Delete("TagZdjecia", "id_tagu=" + lista_albumow[i]);
                        lista_nazw_albumow.RemoveAt(i);
                        lista_albumow.RemoveAt(i);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString() + ex.Message);
            }
            baza.Rozlacz();

            odswiez();
        }

        private void UsunZawartoscAlbumu(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;
            Int64 id_tagu = -1;
            Db baza = new Db();

            baza.Polacz();

            try
            {
                
                DataSet dataSet = baza.Select("select id_tagu from Tag where album=1 and nazwa=\'" + mn.ToolTipText + "\';");
                
                foreach (DataTable t in dataSet.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        id_tagu = (Int64)r[0];
                    }
                }                

                baza.Delete("TagZdjecia", "id_tagu=" + id_tagu);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString() + ex.Message);
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
                for (int i = 0; i < lista_nazw_tagow.Count; i++)
                {
                    if (lista_nazw_tagow[i] == mn.ToolTipText)
                    {
                        baza.Delete("TagZdjecia", "id_tagu=" + lista_tagow[i]);
                        lista_nazw_tagow.RemoveAt(i);
                        lista_tagow.RemoveAt(i);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString() + ex.Message);
            }
            baza.Rozlacz();

            odswiez();
        }


        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
           //MessageBox.Show("wchodzi");

            if (czy_otwarte_albumy == false)
            {
                if (albumy.IsExpanded == true)
                {
                    czy_otwiera = true;
                    czy_otwarte_albumy = true;
                }
                else
                {
                    czy_otwiera = false;
                }
            }
            else
            {
                if (albumy.IsExpanded == false)
                {
                    czy_otwiera = true;
                    czy_otwarte_albumy = false;
                }
                else
                {
                    czy_otwiera = false;
                }
            }

            if(zaznaczony == true)
                czy_otwiera = true;


            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.FullPath.IndexOf("Albumy") == 0)
                {

                    Context.Items.Clear();                    
                    
                    ToolStripItem toolStripItem = Context.Items.Add("Dodaj Album");
                    toolStripItem.Click += new EventHandler(DodajAlbum);
                    toolStripItem = Context.Items.Add("Usun Zawartoœæ Albumu");
                    toolStripItem.ToolTipText = e.Node.Text;
                    toolStripItem.Click += new EventHandler(UsunZawartoscAlbumu);
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
            else if (e.Button == MouseButtons.Left && czy_otwiera == false)
            {
                //MessageBox.Show("wchodzi2222");                
                if (e.Node.FullPath.IndexOf("Albumy") == 0)
                {

                    this.treeView1.SelectedNode = e.Node;
                    //this.treeView1.Select(true, true);

                    List<Zdjecie> zdjecia = null;

                    if (RozpoczetoWyszukiwanie != null)
                        RozpoczetoWyszukiwanie(null);

                    try
                    {
                        zdjecia = PokazPlikiZAlbumu(e.Node,false);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Brak dostêpu do wybranego katalogu.");
                        return;
                    }

                    if (ZakonczonoWyszukiwanie != null)
                        ZakonczonoWyszukiwanie(zdjecia.ToArray(), new Katalog[0]);
                    czy_otwiera = true;
                }
            }

            zaznaczony = false;
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //MessageBox.Show("check");
            int czy_al_zaz = 0, czy_ta_zaz = 0;

            if (e.Node.FullPath == "Albumy")// && czy_wszystkie_albumy_ustawione == false)
            {
                if (e.Node.Checked == true)
                {
                    czy_al_zaz = 1;
                    foreach (TreeNode t in e.Node.Nodes)
                    {
                        t.Checked = true;
                        ilosc_zaznaczonych_albumow = lista_albumow.Count;
                    }
                }
                else
                {
                    czy_al_zaz = 2;
                    foreach (TreeNode t in e.Node.Nodes)
                    {
                        t.Checked = false;
                        ilosc_zaznaczonych_albumow = 0;
                    }
                }
            }

            if (e.Node.FullPath == "Tagi")// && czy_wszystkie_tagi_ustawione == false)
            {
                if (e.Node.Checked == true)
                {
                    czy_ta_zaz = 1;
                    foreach (TreeNode t in e.Node.Nodes)
                    {
                        t.Checked = true;
                        ilosc_zaznaczonych_tagow = lista_tagow.Count;
                    }
                }
                else
                {
                    czy_ta_zaz = 2;
                    foreach (TreeNode t in e.Node.Nodes)
                    {
                        t.Checked = false;
                        ilosc_zaznaczonych_tagow = 0;
                    }
                }
            }

            if (czy_al_zaz == 0)
            {
                if (e.Node.FullPath.IndexOf("Albumy") == 0 && e.Node.FullPath.Length != "Albumy".Length)
                {
                    if (e.Node.Checked == true)
                    {
                        ilosc_zaznaczonych_albumow += 1;
                    }
                    else
                    {
                        ilosc_zaznaczonych_albumow -= 1;
                    }
                }
            }
            /*else if (e.Node.FullPath == "Tagi")
            {
                if (e.Node.Checked == true)
                {
                    ilosc_zaznaczonych_tagow = 0;
                }
                else
                {
                    ilosc_zaznaczonych_tagow = lista_tagow.Count;
                }

            }*/
            if (czy_ta_zaz == 0)
            {
                if (e.Node.FullPath.IndexOf("Tagi") == 0 && e.Node.FullPath.Length != "Tagi".Length)
                {
                    if (e.Node.Checked == true)
                    {
                        ilosc_zaznaczonych_tagow += 1;
                    }
                    else
                    {
                        ilosc_zaznaczonych_tagow -= 1;
                    }
                }
            }


            if (e.Node.FullPath.IndexOf("Tagi") == 0)
            {
                List<Int64> lista_tagow_do_przekazania = new List<Int64>();

                //MessageBox.Show(e.Node.FullPath);

                foreach (TreeNode t in tagi.Nodes)
                {
                    if (t.Checked == true)
                    {                
                        lista_tagow_do_przekazania.Add(znajdzNumer(t.Text));
                    } 
                }

                if (ZmienionoTagi != null)
                    ZmienionoTagi(lista_tagow_do_przekazania);                
            }

            /*if (ilosc_zaznaczonych_tagow == 0 && czy_wszystkie_tagi_ustawione == true)
            {
                tagi.Checked = false;
                czy_wszystkie_tagi_ustawione = false;
                MessageBox.Show("1");
            }
            if (ilosc_zaznaczonych_tagow == tagi.Nodes.Count && czy_wszystkie_tagi_ustawione == false)
            {
                tagi.Checked = true;
                czy_wszystkie_tagi_ustawione = true;
                MessageBox.Show("2");
            }

            if (ilosc_zaznaczonych_albumow == 0 && czy_wszystkie_albumy_ustawione == true)
            {
                albumy.Checked = false;
                czy_wszystkie_albumy_ustawione = false;
                MessageBox.Show("3");
            }
            if (ilosc_zaznaczonych_albumow == albumy.Nodes.Count && czy_wszystkie_albumy_ustawione == false)
            {
                albumy.Checked = true;
                czy_wszystkie_albumy_ustawione = true;
                MessageBox.Show("4");
            }*/

            //MessageBox.Show("ilosc zaznaczonych albumow: " + ilosc_zaznaczonych_albumow);
            //MessageBox.Show("ilosc zaznaczonych tagow: " + ilosc_zaznaczonych_tagow);
            
        }

        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            //czy_otwiera = true;
            //MessageBox.Show("after collapse");
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            //czy_otwiera = true;
            //MessageBox.Show("after expand");
        }

        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            //czy_otwiera = true;
            //MessageBox.Show("before collapse");
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //czy_otwiera = true;
            //MessageBox.Show("before expand");
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            //czy_otwiera = false;
        }

        private void treeView1_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            zaznaczony = true;

            

            //MessageBox.Show(e.Node.FullPath);

            /*if (e.Node.FullPath == "Albumy")
            {
                if (e.Node.Checked == true)
                {
                    ilosc_zaznaczonych_albumow = 0;
                }
                else
                {
                    ilosc_zaznaczonych_albumow = lista_albumow.Count;
                }
                
            }
            else */
            

            //MessageBox.Show("ilosc zaznaczonych albumow: " + ilosc_zaznaczonych_albumow);
            //MessageBox.Show("ilosc zaznaczonych tagow: " + ilosc_zaznaczonych_tagow);
            //MessageBox.Show("ilosc albumow: " + lista_albumow.Count);

        }        
    }
}
