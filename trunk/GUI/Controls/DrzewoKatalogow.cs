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
    /// <summary>
    /// Klasa implementuj¹ca interfejs IWyszukiwacz i drzewo wykorzystywana do tworzenia drzewa katalogów
    /// </summary>
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

        static bool czy_otwiera = false;

        private DirTreeNode kat_MKomputer, zaznaczony, ostatni;
        private string FullPath_zaznaczonego = "";

        private ContextMenuStrip Context;
        /// <summary>
        /// Konstruktor generuje liste ikon wykorzystywanych w drzewie katalogów
        /// </summary>
        public FileTree()
        {
            GenerateImage();
            this.BackColor = Color.Beige;
            Context = new ContextMenuStrip();
            czy_otwiera = false;
            
        }

        /// <summary>
        /// Metoda generuj¹ca i wype³niaj¹ca liste ikonami wykorzystywanymi w drzewie katalogów
        /// </summary>
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

        /// <summary>
        /// Metoda wype³niaj¹ca drzewo katalogow
        /// </summary>
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

            DirTreeNode kat_MDokumenty = new DirTreeNode(Config.katalogMojeDokumenty, User, true);
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

            kat_MKomputer.Expand();

            EndUpdate();

            BeforeExpand += new TreeViewCancelEventHandler(prepare);
            AfterCollapse += new TreeViewEventHandler(clear);
        }
        /// <summary>
        /// Metoda do otwierania wez³a drzewa
        /// </summary>
        /// <param name="path">sciezka katalogu do otwrcia</param>
        public void Open(string path)
        {
            TreeNodeCollection nodes = Nodes;
            DirTreeNode subnode = null;
            int i, n;            

            if (path.IndexOf("Mój Komputer") == 0)
                path = path.ToLower().Substring("Mój Komputer".Length, path.Length - "Mój Komputer".Length);
            else
                path = path.ToLower();
            
            Nodes.Clear();            

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

        /// <summary>
        /// Metoda przygotowuj¹ca wêze³ drzewa do otwarcia
        /// </summary>        
        void prepare(object sender, TreeViewCancelEventArgs e)
        {
            BeginUpdate();
            DirTreeNode tn = (DirTreeNode)e.Node;
            //zaznaczony = (DirTreeNode)e.Node;
            try
            {
                tn.populate(tn);
                EndUpdate();
            }
            catch (Exception ex)
            {
                if (tn.Path.Length == 3)
                {
                    MessageBox.Show(ex.Message + " - Odmowa dostêpu", e.Node.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //e.Node.                
                    e.Node.Remove();
                    //Nodes.Insert(tn.Index, tn);
                    kat_MKomputer.Nodes.Insert(tn.Index, tn);
                    //zaznaczony.Collapse(false);               
                }
                else
                {                    
                    //DirNode("", false);
                }
                EndUpdate();
            }
        }

        /// <summary>
        /// Metoda czyszcz¹ca drzewo (w przypadku aplikacji nie sa usuwane wezly ktore zostaly zamkniete jest to spowodowane odbudowaniem drzewa gdy zmiana katalogu odbywa sie za pomoca wdoku miniatur)
        /// </summary>        
        void clear(object sender, TreeViewEventArgs e)
        {
            BeginUpdate();
            DirTreeNode tn = (DirTreeNode)e.Node;
            //tn.setLeaf(true);
            EndUpdate();
        }

        /// <summary>
        /// Metoda usuwaj¹ca wszystkie wez³y z drzewa katalogów
        /// </summary>
        public void Delete()
        {
            BeginUpdate();
            //Nodes.RemoveAt(0);
            Nodes.Clear();
            EndUpdate();
        }

        /// <summary>
        /// Klasa imlementuj¹ca TreeNode czyli odpowiada za tworzenie wez³ów drzewa katalogow
        /// </summary>
        public class DirTreeNode : TreeNode
        {
            string path;
            int type;
            public virtual string Path { get { return path; } }

            /// <summary>
            /// Konstruktor
            /// </summary>
            /// <param name="s">scie¿ka wêz³a</param>
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

            /// <summary>
            /// Konstruktor
            /// </summary>
            /// <param name="aType">typ ikony jaka ma byc u¿yta</param>
            /// <param name="s">scie¿ka wêz³a</param>
            /// <param name="z">informacja czy dodajemy wêz³y do g³ównego trzonu czy jakieœ podrzedne katalogi</param>
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

            /// <summary>
            /// Konstruktor
            /// </summary>
            /// <param name="label">jak¹ etykiete wypisaæ przy urz¹dzeniu</param>
            /// <param name="s">scie¿ka wêz³a</param>
            public DirTreeNode(string s, string label)
                : this(s)
            {
                Text = label;
            }

            /// <summary>
            /// Konstruktor
            /// </summary>
            /// <param name="aType">typ ikony jak¹ ma byæ u¿yta</param>
            /// <param name="s">scie¿ka wêz³a</param>
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
                    //MessageBox.Show(e.ToString() + e.Message);
                }
            }

            /// <summary>
            /// Metoda wyznaczaj¹ca typ urz¹dzenia
            /// </summary>
            /// <param name="driveLetter">nazwa urz¹dzenia</param>
            /// <returns>zwraca typ urz¹dzenia</returns>
            [DllImport("kernel32.dll")]
            public static extern long GetDriveType(string driveLetter);

            /// <summary>
            /// Metoda wykorzystywana do wy³uskania etykiety urz¹dzenia ( partycja, cdrom, floppy )
            /// </summary>
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


            /// <summary>
            /// Metoda zwraca typ urz¹dzenia
            /// </summary>
            /// <param name="drive">nazwa urz¹dzenia</param>
            public int getDriveType(string drive)
            {
                if ((GetDriveType(drive) & 5) == 5) return Cdrom;//cd
                if ((GetDriveType(drive) & 3) == 3) return Dysk;//fixed
                if ((GetDriveType(drive) & 2) == 2) return Dysk;//removable
                if ((GetDriveType(drive) & 4) == 4) return Dyskietka;//remote disk
                if ((GetDriveType(drive) & 6) == 6) return Dyskietka;//ram disk
                return 0;
            }

            /// <summary>
            /// Metoda zwracaj¹ca nazwê/etykiete urz¹dzenia
            /// </summary>
            /// <param name="drive">nazwa urz¹dzenia</param>
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

            /// <summary>
            /// Metoda zwracaj¹ca etykiete urz¹dzenia
            /// </summary>
            /// <param name="drive">nazwa urz¹dzenia</param>
            public static string Etykieta(string drive)
            {
                return GetDriveName(drive);
            }

            /// <summary>
            /// Metoda tworz¹ca i dodaj¹ca wêz³y do drzewa katalogów
            /// </summary>
            /// <param name="tn">wêze³ drzewa który zostanie otwarty</param>
            internal void populate(DirTreeNode tn)
            {
                ArrayList folder = new ArrayList(); ;

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
                    czy_otwiera = false;
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

            /// <summary>
            /// Metoda ustawiaj¹ca dany wêze³ czy ma byæ liœciem i nie mieæ ju¿ potomków czy nie 
            /// </summary>
            /// <param name="b">czy ma byc liœciem</param>
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
        public event ZmienionoZrodloDelegate ZmienionoZrodlo;
        public event ZmienionoTagiDelegate ZmienionoTagi;
        public event ZmienionoIdsDelegate ZmienionoIds;

        #endregion

        /// <summary>
        /// Metoda wywo³uj¹ca sie przed zaznaczeniem wêz³a
        /// </summary>
        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            base.OnBeforeSelect(e);
            czy_otwiera = false;
            //this.SelectedNode = new TreeNode("cos");
            //this.Select(true, true);
            //MessageBox.Show("numer: " + e.Node.SelectedImageIndex);
        }

        /// <summary>
        /// Metoda wywo³uj¹ca sie po zaznaczeniem wêz³a
        /// </summary>
        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);
            czy_otwiera = false;
            //this.SelectedNode = new TreeNode("cos");
            //this.Select(true, true);
        }

        /// <summary>
        /// Metoda dodaj¹ca katalog do albumu
        /// </summary>
        /// <param name="sciezka">sciezka katalogu</param>
        internal void d_d_a(string sciezka)
        {
            List<Zdjecie> zdjecia = new List<Zdjecie>();
            List<string> pliki_przefiltrowane = new List<string>();

            try
            {
                pliki_przefiltrowane = Przefiltruj(sciezka);

                if (pliki_przefiltrowane.Count != 0)
                {
                    dodaj_kolekcje_do_bazy(pliki_przefiltrowane);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " - Odmowa dostêpu", sciezka, MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
        }

        /// <summary>
        /// Metoda filtruj¹ca pliki w katalogu
        /// </summary>
        /// <param name="sciezka">sciezka katalogu</param>
        /// <returns>zwraca liste przefiltrowana czyli tylko pliki ze zdjêciami</returns>
        internal List<string> Przefiltruj(string sciezka)
        {
            List<string> pliki = new List<string>();
            List<string> pliki_przefiltrowane = new List<string>();

            try
            {
                pliki.AddRange(Directory.GetFiles(sciezka, "*.jpg"));
                pliki.AddRange(Directory.GetFiles(sciezka, "*.jpeg"));

                pliki.Sort();

                for (int i = 0; i < pliki.Count; i++)
                {
                    try
                    {
                        if ((pliki[i].ToLower().LastIndexOf(".jpg") != -1 && pliki[i].ToLower().LastIndexOf(".jpg") == (pliki[i].Length - 4)) || (pliki[i].ToLower().LastIndexOf(".jpeg") != -1 && pliki[i].ToLower().LastIndexOf(".jpeg") == (pliki[i].Length - 5)))
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
            }

            return pliki_przefiltrowane;
        }

        /// <summary>
        /// Metoda tworz¹ca obiekty zdjêc i dodaj¹ca je do kontrolki wyœwietlaj¹cej
        /// </summary>
        /// <param name="Node">wêz³ dla którego tworzymy obiekty zdjêæ</param>
        /// <returns>zwraca tablice obiektów zdjêæ</returns>
        private Zdjecie[] WybierzPlikiZdjec(DirTreeNode Node)
        {
            List<Zdjecie> zdjecia = new List<Zdjecie>();
            List<string> pliki = Przefiltruj(Node.Path);            
            
            Zdjecie z;
            if (Node.Text != "Mój Komputer")
            {
                try
                {
                    for (int i = 0; i < pliki.Count; i++)
                    {
                        try
                        {
                            if ((pliki[i].ToLower().LastIndexOf(".jpg") != -1 && pliki[i].ToLower().LastIndexOf(".jpg") == (pliki[i].Length - 4)) || (pliki[i].ToLower().LastIndexOf(".jpeg") != -1 && pliki[i].ToLower().LastIndexOf(".jpeg") == (pliki[i].Length - 5)) )
                            {                                
                                z = new Zdjecie(pliki[i]);                                
                                z.ZweryfikujZdjecie();                                
                                zdjecia.Add(z);
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
                    MessageBox.Show(e.Message + " - Odmowa dostêpu", Node.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return zdjecia.ToArray();
        }

        /// <summary>
        /// Metoda wywo³ana po otworzeniu danego wêz³a drzewa
        /// </summary>
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
            czy_otwiera = true;            
        }

        /// <summary>
        /// Metoda informuj¹ca za pomoc¹ delegatów kontrolkê wyœwietlaj¹ca o zawartoœci katalogu w zdjêcia i podkatalogi
        /// </summary>
        /// <param name="zap">wêz³ dla ktorego wykonujemy zapytanie</param>
        private void PowiadomOZawartosciKatalogu(DirTreeNode zap)
        {
            List<Zdjecie> zdjecia = new List<Zdjecie>();
            List<Katalog> katalogi = new List<Katalog>();

            if (ZmienionoZrodlo != null)
                ZmienionoZrodlo(zap.Path);

            if (RozpoczetoWyszukiwanie != null)
                RozpoczetoWyszukiwanie(null);

            try
            {
                katalogi.AddRange(ZnajdzKatalogiWKatalogu((DirTreeNode)zap));
                
                if (!zap.Path.Equals("mój komputer"))
                {
                    //MessageBox.Show(zap.Path);
                    zdjecia.AddRange(WybierzPlikiZdjec((DirTreeNode)zap));
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Brak dostêpu do wybranego katalogu.");
                return;
            }            

            if (ZakonczonoWyszukiwanie != null)
                ZakonczonoWyszukiwanie(zdjecia.ToArray(), katalogi.ToArray(), true);

        }

        /// <summary>
        /// Metoda wywo³ana po klikniêciu na wêze³
        /// w zale¿noœci od przycisku myszki który klik¹³
        /// je¿eli lewy zostaje wywo³ana medota informuj¹ca o zawartoœci danego wêz³a
        /// je¿eli prawy jest tworzone menu kontekstowe
        /// </summary>
        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            base.OnNodeMouseClick(e);            

            if (e.Button == MouseButtons.Right)
            {
                Context.Items.Clear();
                ToolStripItem toolStripItem = Context.Items.Add("Dodaj zawartosc katalogu " + ((DirTreeNode)e.Node).Path + " do kolekcji",Properties.Resources.dodaj_do_kolekcji);
                toolStripItem.ToolTipText = ((DirTreeNode)e.Node).Path;
                toolStripItem.Click += new EventHandler(DodajDoKolekcji);
                toolStripItem = Context.Items.Add("Dodaj zawartosc katalogu " + ((DirTreeNode)e.Node).Path + " do Albumu",Properties.Resources.Contacts_d);
                toolStripItem.ToolTipText = ((DirTreeNode)e.Node).Path;
                toolStripItem.Click += new EventHandler(DodajDoAlbumu);
                toolStripItem = Context.Items.Add("Dodaj tagi dla katalogu " + ((DirTreeNode)e.Node).Path,Properties.Resources.new_tag);
                toolStripItem.ToolTipText = ((DirTreeNode)e.Node).Path;
                toolStripItem.Click += new EventHandler(DodajTagiDlaKatalogu);
                toolStripItem = Context.Items.Add("Usuñ tagi dla katalogu " + ((DirTreeNode)e.Node).Path,Properties.Resources.delete_tag);
                toolStripItem.ToolTipText = ((DirTreeNode)e.Node).Path;
                toolStripItem.Click += new EventHandler(UsunTagiDlaKatalogu);
                toolStripItem = Context.Items.Add("Usuñ zawartoœæ katalogu " + ((DirTreeNode)e.Node).Path + " z kolekcji",Properties.Resources.Recycle_Bin_Full_d);
                toolStripItem.ToolTipText = ((DirTreeNode)e.Node).Path;
                toolStripItem.Click += new EventHandler(UsunZKolekcji);


                Context.Show(this, new Point(e.X, e.Y));
            }
            else if (e.Button == MouseButtons.Left && czy_otwiera == false)
            {                
                zaznaczony = (DirTreeNode)e.Node;
                FullPath_zaznaczonego = zaznaczony.FullPath;

                DirNode("", false);                
            }

            czy_otwiera = false;
        }

        /// <summary>
        /// Metoda informuje kontrolke wyœwietlaj¹ca o nowej zawartoœci albo katalogu nadrzêdnego albo podkatalogu
        /// </summary>
        /// <param name="k">katalog którego zawartoœæ ma byæ wyœwietlona</param>
        public void ZaladujZawartoscKatalogu(Katalog k)
        {
            if (k.CzyDoGory == true)
            {
                DirNode(k.Path, true);
            }
            else
            {
                DirNode(k.Path, false);
            }
        }
        /// <summary>
        /// Metoda do nawigacji za pomoc¹ klawiatury po drzewie. Naciœniecie klawisza Enter na zaznaczonym wêŸle drzewa spowoduje jego eyœwietlenie
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            DirTreeNode wezel = (DirTreeNode)this.SelectedNode;
            if (wezel != null)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    zaznaczony = wezel;
                    DirNode("", false);
                }
            }
        }

        /// <summary>
        /// Metoda która dotosowuje wygl¹d drzewa do sytuacji w widoku miniatur
        /// </summary>
        /// <param name="czy_do_gory">czy katalog nadrzêdny</param>
        /// <param name="napis">sciezka katalogu do wyœwietlenia</param>
        public void DirNode(string napis, bool czy_do_gory)
        {
            string nn = "";
            
            try
            {
                nn = PootwierajDrzewoDoZaznaczonego(zaznaczony);                

                if (czy_do_gory)
                {
                    if (nn == "")
                    {
                        zaznaczony = (DirTreeNode)zaznaczony.Parent;
                        zaznaczony.Collapse(true);
                    }

                    FullPath_zaznaczonego = zaznaczony.FullPath;
                }
                else
                {
                    if (napis != "" && nn == "")
                    {
                        zaznaczony.Expand();

                        TreeNodeCollection kolekcja = zaznaczony.Nodes;
                        DirTreeNode ppp = zaznaczony;

                        foreach (TreeNode t in kolekcja)
                        {
                            ppp = (DirTreeNode)t;
                            if (napis.ToLower().CompareTo(ppp.Path) == 0)
                            {
                                break;
                            }
                        }
                        zaznaczony = ppp;
                        FullPath_zaznaczonego = zaznaczony.FullPath;//FullPath_zaznaczonego + "\\" + napis.Substring(napis.LastIndexOf("\\") + 1, napis.Length - napis.LastIndexOf("\\") - 1);
                    }

                }
                PowiadomOZawartosciKatalogu(zaznaczony);
                UstawienieEtykietyDyskietki(zaznaczony);
            }            
            catch(Exception)
            {
                MessageBox.Show("Dany katalog zosta³ usuniety drzewo zostanie zwiniête", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Delete();
                Fill();
                zaznaczony = kat_MKomputer;
                FullPath_zaznaczonego = zaznaczony.FullPath;
                PowiadomOZawartosciKatalogu(zaznaczony);
                UstawienieEtykietyDyskietki(zaznaczony);                 
            }           
        }

        /// <summary>
        /// Metoda otwiera drzewo do zaznaczonego wêz³a
        /// </summary>
        /// <param name="zaz">wêz³ do którego nale¿y pootwieraæ</param>
        private string PootwierajDrzewoDoZaznaczonego(DirTreeNode zaz)
        {
            TreeNodeCollection kolekcja = this.Nodes;

            ostatni = null;

            string sciezka = FullPath_zaznaczonego, nazwa_pliku = "", do_sprawdzenia = "";

            string[] spl = sciezka.Split("\\".ToCharArray());

            int licznik = 0, ilosc_w_sciezka = spl.Length - 1;

            bool stan = false, znaleziono_wezl = false, dysk = false, czy_byl_blad = false;

            while (stan == false)
            {
                foreach (DirTreeNode dn in kolekcja)
                {
                    if (licznik == spl.Length)
                    {
                        stan = true;
                        czy_byl_blad = true;
                        break;
                    }

                    if (licznik < spl.Length-1)
                    {
                        /*MessageBox.Show("spl[licznik].IndexOf(\":\") " + spl[licznik].IndexOf(":"));
                        MessageBox.Show("spl[licznik].Length " + spl[licznik].Length);
                        MessageBox.Show("spl[licznik + 1].IndexOf(\"[\") " + spl[licznik + 1].IndexOf("["));
                        MessageBox.Show("spl[licznik + 1].LastIndexOf(\"]\") " + spl[licznik + 1].LastIndexOf("]"));
                        MessageBox.Show("spl[licznik + 1].Length - 1) " + (spl[licznik + 1].Length - 1));
                        */
                        if (spl[licznik].IndexOf(":") == 1 && spl[licznik].Length == 2 && spl[licznik + 1].IndexOf("[") == 1 && spl[licznik + 1].LastIndexOf("]") == (spl[licznik + 1].Length - 1))
                        {
                            do_sprawdzenia = spl[licznik] + "\\" + spl[licznik + 1];
                            nazwa_pliku = dn.FullPath.Substring(dn.FullPath.LastIndexOf("\\") - 2, dn.FullPath.Length - dn.FullPath.LastIndexOf("\\") + 2);
                            //licznik++;
                            dysk = true;
                        }
                        else
                        {
                            do_sprawdzenia = spl[licznik];
                            nazwa_pliku = dn.FullPath.Substring(dn.FullPath.LastIndexOf("\\") + 1, dn.FullPath.Length - dn.FullPath.LastIndexOf("\\") - 1);
                        }
                    }
                    else
                    {
                        do_sprawdzenia = spl[licznik];
                        nazwa_pliku = dn.FullPath.Substring(dn.FullPath.LastIndexOf("\\") + 1, dn.FullPath.Length - dn.FullPath.LastIndexOf("\\") - 1);
                    }

                    //MessageBox.Show(nazwa_pliku + "==" + do_sprawdzenia);
                    //MessageBox.Show("dn.f: " + dn.FullPath);

                    
                    if (do_sprawdzenia.CompareTo(nazwa_pliku) == 0)
                    {
                        //MessageBox.Show(sciezka);
                        //dn.Collapse();

                        dn.Expand();
                        // MessageBox.Show("dn.f: " + dn.FullPath);
                        kolekcja = dn.Nodes;

                        if (sciezka.CompareTo(dn.FullPath) == 0)
                        {
                            dn.Collapse();
                            zaznaczony = dn;
                            FullPath_zaznaczonego = zaznaczony.FullPath;
                            stan = true;
                            //this.SelectedNode = zaznaczony;
                            //this.Select(true, true);
                        }
                        
                        znaleziono_wezl = true;

                        ostatni = dn;
                        //MessageBox.Show("ostatni: " + ostatni.FullPath);
                        break;
                    }
                }

                if (dysk == true)
                {
                    licznik++;
                    dysk = false;
                }


                if (znaleziono_wezl == false)
                {
                    stan = true;
                    //ostatni.
                    zaznaczony = ostatni;
                    zaznaczony.Collapse();
                    //MessageBox.Show("brak wezla");
                    //MessageBox.Show("ostatni: " + ostatni.FullPath);
                    czy_byl_blad = true;
                    //MessageBox.Show("licznik: " + licznik);
//                    MessageBox.Show("Dany katalog zosta³ usuniêty przez inny program aplikacja powróci do rodzica katalogu który istnieje - Odmowa dostêpu", "B³¹d braku katalogu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //MessageBox.Show("wezl znaleziony");
                    znaleziono_wezl = false;
                    //MessageBox.Show("ostatni: " + ostatni.FullPath);
                }
                //MessageBox.Show("ostatni2: " + ostatni.FullPath);

                licznik++;
            }

            if (czy_byl_blad == true)
            {
                return "cos";
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// Metoda ustawia etykiete dyskietki
        /// </summary>
        /// <param name="zap">wêze³ zaznaczony</param>
        private void UstawienieEtykietyDyskietki(DirTreeNode zap)
        {
            string etykieta;

            if (zap.Text.IndexOf("A:\\") == 0 && zap.Text.LastIndexOf("\\") < 4)
            {
                etykieta = DirTreeNode.Etykieta(zap.Text.Substring(0, 3));
                if (etykieta != "")
                {
                    zap.Text = "A:\\" + " [" + etykieta + "]";
                    zap.ImageIndex = Dyskietka_z;
                    zap.SelectedImageIndex = Dyskietka_z;
                }
                else
                {
                    zap.Text = "A:\\" + " [Floppy]";
                    zap.ImageIndex = Dyskietka;
                    zap.SelectedImageIndex = Dyskietka;
                }
            }
            else if (zap.Text.IndexOf("B:\\") == 0 && zap.Text.LastIndexOf("\\") < 4)
            {
                etykieta = DirTreeNode.Etykieta(zap.Text.Substring(0, 3));
                if (etykieta != "")
                {
                    zap.Text = "B:\\" + " [" + etykieta + "]";
                    zap.ImageIndex = Dyskietka_z;
                    zap.SelectedImageIndex = Dyskietka_z;
                }
                else
                {
                    zap.Text = "B:\\" + " [Floppy]";
                    zap.ImageIndex = Dyskietka;
                    zap.SelectedImageIndex = Dyskietka;
                }
            }
        }

        /// <summary>
        /// Metoda zwracaj¹ca tablice katalogów z danego katalogu
        /// </summary>
        /// <param name="Node">wêz³ drzewa</param>
        /// <returns>zwraca tablice katalogów</returns>
        private Katalog[] ZnajdzKatalogiWKatalogu(DirTreeNode Node)
        {
            List<string> katal_tab = new List<string>();
            List<Katalog> katalogi = new List<Katalog>();

            //MessageBox.Show(Node.Text);

            if (Node.Text != "Mój Komputer")
            {

                try
                {
                    katal_tab.AddRange(Directory.GetDirectories(Node.Path));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " - Odmowa dostêpu", Node.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //if (Node.Path.Length > 3)
                //{
                    string s1 = "";

                    s1 = Node.Path.Substring(0, Node.Path.LastIndexOf("\\"));

                    if (Node.Text != "Pulpit" && Node.Text != "Moje Dokumenty" && Node.Text != "Moje Obrazy")
                    {
                        if (s1.Length == 2)
                        {
                            katalogi.Add(new Katalog(s1 + "\\", true));
                        }
                        else
                        {
                            katalogi.Add(new Katalog(s1, true));
                        }
                    }

                //}
                
                katal_tab.Sort();
                foreach (string t in katal_tab)
                {
                    katalogi.Add(new Katalog(t, false));
                }
            }
            else
            {
                try
                {
                    katal_tab.AddRange(Directory.GetLogicalDrives());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " - Odmowa dostêpu654646464", Node.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                katal_tab.Sort();
                foreach (string t in katal_tab)
                {
                    //MessageBox.Show(t);
                    katalogi.Add(new Katalog(t, false));
                }
            }
            return katalogi.ToArray();
        }


        /// <summary>
        /// Metoda dodaje wpisy do bazy
        /// </summary>
        /// <param name="lista">lista plików o których nale¿y dodaæ wpisy do bazy</param>
        internal void dodaj_kolekcje_do_bazy(List<string> lista)
        {            
            StringBuilder sb = new StringBuilder("Nie uda³o siê dodaæ do kolekcji nastepuj¹cych zdjêæ:\n");
            bool nieUdaloSie = false;
            foreach (string n in lista)
            {
                try
                {
                    Zdjecie z = new Zdjecie(n);                    
                    z.ZweryfikujZdjecie();
                    if (z.DodajDoKolekcji() == false)
                    {
                        sb.Append(z.Path + "\n");
                        if (nieUdaloSie == false)
                        {
                            nieUdaloSie = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }               
            }           
            if (nieUdaloSie)
                MessageBox.Show(sb.ToString());
        }

        /// <summary>
        /// Metoda dodaje kolekcje do bazy
        /// </summary>
        internal void DodajDoKolekcji(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;            
            List<string> lista = Przefiltruj(mn.ToolTipText);
            dodaj_kolekcje_do_bazy(lista);

            if (ZmienionoIds != null)
                ZmienionoIds();
        }

        /// <summary>
        /// Metoda usuwa tagi dla katalogu
        /// </summary>
        private void UsunTagiDlaKatalogu(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;

            List<string> lista_stringow = Przefiltruj(mn.ToolTipText);            

            foreach (string plik in lista_stringow)
            {
                Zdjecie z = new Zdjecie(plik);                
                z.ZweryfikujZdjecie();

                if (z.CzyUstawioneId() == true)
                {
                    z.UsunTagi();
                }                
            }

            if (ZmienionoTagi != null)
                ZmienionoTagi();
        }

        /// <summary>
        /// Metoda usuwa katalog z bazy
        /// </summary>
        private void UsunZKolekcji(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;

            List<string> lista_stringow = Przefiltruj(mn.ToolTipText);

            foreach (string plik in lista_stringow)
            {
                Zdjecie z = new Zdjecie(plik);
                z.ZweryfikujZdjecie();

                if (z.CzyUstawioneId() == true)
                {
                    z.UsunZdjecieZBazy();
                    z.UsunId();
                }
            }

            if (ZmienionoIds != null)
                ZmienionoIds();
        }

        /// <summary>
        /// Metoda dodaje tagi dla katalogu
        /// </summary>
        private void DodajTagiDlaKatalogu(object sender, EventArgs e)
        {            
            ToolStripItem mn = (ToolStripItem)sender;

            List<string> lista_stringow = Przefiltruj(mn.ToolTipText);
            List<Zdjecie> lista_zdjec = new List<Zdjecie>();

            foreach (string plik in lista_stringow)
            {
                Zdjecie z = new Zdjecie(plik);
                z.ZweryfikujZdjecie();
                lista_zdjec.Add(z);
            }

            Dodaj_tagi_do_zdjecia dtdz = new Dodaj_tagi_do_zdjecia(lista_zdjec,true);
            dtdz.ZmienionoTagi += new ZmienionoTagiDelegate(this.ZmienionoTagi);
            dtdz.Show();           
        }

        /// <summary>
        /// Metoda dodaje katalog do albumów
        /// </summary>
        internal void DodajDoAlbumu(object sender, EventArgs e)
        {
            ToolStripItem mn = (ToolStripItem)sender;

            List<string> lista_stringow = Przefiltruj(mn.ToolTipText);
            List<Zdjecie> lista_zdjec = new List<Zdjecie>();
            foreach (string s in lista_stringow)
            {
                Zdjecie z = new Zdjecie(s);
                z.ZweryfikujZdjecie();
                lista_zdjec.Add(z);
            }

            Dodaj_albumy_do_zdjecia dadz = new Dodaj_albumy_do_zdjecia(lista_zdjec, this, mn.ToolTipText);
            dadz.Show();

            //Dodaj_katalog_do_bazy ddk = new Dodaj_katalog_do_bazy(mn.ToolTipText, this);
            //ddk.Show();
        }

        /// <summary>
        /// Metoda wywo³ana po zamkniêciu wêz³a drzewa
        /// </summary>
        protected override void OnAfterCollapse(TreeViewEventArgs e)
        {
            base.OnAfterCollapse(e);
            czy_otwiera = true;
            //MessageBox.Show("after colapse");
        }

        /// <summary>
        /// Metoda wywo³ana przed zamkniêciu wêz³a drzewa
        /// </summary>
        protected override void OnBeforeCollapse(TreeViewCancelEventArgs e)
        {
            base.OnBeforeCollapse(e);
            czy_otwiera = true;
            //MessageBox.Show("before colapse");
        }

        /// <summary>
        /// Metoda wywo³ana przed otwarciem wêz³a drzewa
        /// </summary>
        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            base.OnBeforeExpand(e);
            czy_otwiera = true;
            //MessageBox.Show("before expand");
        }

        /// <summary>
        /// Metoda inicjalizuj¹ca obiekt drzewa katalogów
        /// </summary>
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