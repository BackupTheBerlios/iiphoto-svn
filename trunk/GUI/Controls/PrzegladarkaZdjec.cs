using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;

namespace Photo
{
    public partial class PrzegladarkaZdjec : UserControl, IOpakowanieZdjec
    {
        public PrzegladarkaZdjec()
        {
            InitializeComponent();
            Context = new ContextMenuStrip();
        }
        public void SetThumbnailView()
        {
            if (panele.SelectedTab == zdjecie1Tab)
            {
                // zamykanie dytora
            }
            panele.SelectedTab = miniatury1Tab;
        }
        public void SetImageView()
        {
            panele.SelectedTab = zdjecie1Tab;
        }

        public WidokMiniatur Thumbnailview
        {
            get
            {
                return widokMiniatur1;
            }
        }
        public WidokZdjecia Imageview
        {
            get
            {
                return widokZdjecia1;
            }
        }        
        public IOpakowanieZdjec AktywneOpakowanie
        {
            get
            {
                if (panele.SelectedTab == miniatury1Tab)
                {
                    return Thumbnailview;
                }
                else
                {
                    return Imageview;
                }
            }
        }

        #region IOpakowanieZdjec Members

        public IZdjecie this[int numer]
        {
            get { return AktywneOpakowanie[numer]; }
        }

        public int Ilosc
        {
            get { return AktywneOpakowanie.Ilosc; }
        }

        public void Dodaj(IZdjecie zdjecie)
        {
            Thumbnailview.Dodaj(zdjecie);
            if (AktywneOpakowanie != Thumbnailview)
                SetThumbnailView();
        }

        public void Usun(IZdjecie zdjecie)
        {
            AktywneOpakowanie.Usun(zdjecie);
        }

        public void Oproznij()
        {
            AktywneOpakowanie.Oproznij();
        }

        public IZdjecie[] WybraneZdjecia
        {
            get
            {
                return AktywneOpakowanie.WybraneZdjecia;
            }
        }

        public void RozpocznijEdycje()
        {
            AktywneOpakowanie.RozpocznijEdycje();
        }

        public void ZakonczEdycje()
        {
            AktywneOpakowanie.ZakonczEdycje();
        }

        public void DodajOperacje(PolecenieOperacji operacja)
        {
            AktywneOpakowanie.DodajOperacje(operacja);
        }

        public void UsunWszystkieOperacje()
        {
            AktywneOpakowanie.UsunWszystkieOperacje();
        }

        public void Wypelnij(IZdjecie[] zdjecia, Katalog[] katalogi)
        {
            if (AktywneOpakowanie != Thumbnailview)
                SetThumbnailView();
            Thumbnailview.Wypelnij(zdjecia, katalogi);
        }

        #endregion

        private void widokMiniatur_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                List<Zdjecie> lista = ZwrocZdjeciaZaznaczone();
                if (lista.Count != 0)
                {
                    Context.Items.Clear();
                    ToolStripItem toolStripItem;                

                    if (lista.Count == 1)
                    {
                        //lista[0].i
                        //string s = lista[0].Id;
                        if (lista[0].CzyUstawioneId() == false)
                        {
                            toolStripItem = Context.Items.Add("Dodaj zaznaczenie do kolekcji");
                            toolStripItem.Click += new EventHandler(DodajZaznaczenieDoKolekcji);
                        }
                        else
                        {
                            toolStripItem = Context.Items.Add("Uaktualizuj Tagi");
                            toolStripItem.Click += new EventHandler(UaktualizujTagi);
                        }
                    }
                    else
                    {
                        toolStripItem = Context.Items.Add("Dodaj zaznaczenie do kolekcji");
                        toolStripItem.Click += new EventHandler(DodajZaznaczenieDoKolekcji);
                        //ewentualnie dla kilku a to pozniej
                        toolStripItem = Context.Items.Add("Dodaj Tagi");
                        toolStripItem.Click += new EventHandler(UaktualizujTagi);
                    }
                    toolStripItem = Context.Items.Add("Dodaj zaznaczone do Albumu");
                    toolStripItem.Click += new EventHandler(DodajZaznaczoneDoAlbumu);
                    toolStripItem = Context.Items.Add("Usuñ zaznaczone zdjecia");
                    toolStripItem.Click += new EventHandler(UsunZdjecie);

                    Context.Show(this.Thumbnailview, new Point(e.X, e.Y));
                }
            }
        }

        private List<Zdjecie> ZwrocZdjeciaZaznaczone()
        {
            List<Zdjecie> lista = new List<Zdjecie>();
            Zdjecie[] zdjecia = (Zdjecie[])Thumbnailview.WybraneZdjecia;
            if (zdjecia.Length != 0)
            {
                for (int i = 0; i < zdjecia.Length; i++)
                {
                    lista.Add(zdjecia[i]);
                }
            }
            return lista;
        }
        

        private void DodajTagiDlaKilku(object sender, EventArgs e)
        {
            List<Zdjecie> lista = ZwrocZdjeciaZaznaczone();

            Dodaj_tagi_do_zdjecia dtdz = new Dodaj_tagi_do_zdjecia(lista);
            dtdz.Show();
        }

        private void UaktualizujTagi(object sender, EventArgs e)
        {
            List<Zdjecie> lista = ZwrocZdjeciaZaznaczone();

            //if (lista.Count == 1)
            //{                
                Dodaj_tagi_do_zdjecia dtdz = new Dodaj_tagi_do_zdjecia(lista);
                dtdz.Show();
            //}
        }

        private void DodajZaznaczenieDoKolekcji(object sender, EventArgs e)
        {
            List<Zdjecie> lista = ZwrocZdjeciaZaznaczone();

            if (lista.Count != 0)
            {
                dodaj_kolekcje_do_bazy(lista);
            }
        }

        private void DodajZaznaczoneDoAlbumu(object sender, EventArgs e)
        {
            List<Zdjecie> lista = ZwrocZdjeciaZaznaczone();

            if (lista.Count != 0)
            {
                Dodaj_katalog_do_bazy ddk = new Dodaj_katalog_do_bazy(lista[0].Path, this);
                ddk.Show();
            }

        }

        public void dodaj_kolekcje_do_bazy(List<Zdjecie> lista)
        {
            StringBuilder sb = new StringBuilder("Nie uda³o siê dodaæ do kolekcji nastepuj¹cych zdjêæ:\n");
            bool nieUdaloSie = false;
            foreach (Zdjecie z in lista)
            {
                if (z.DodajDoKolekcji() == false)
                {
                    sb.Append(z.Path + "\n");
                    if (nieUdaloSie == false)
                    {
                        nieUdaloSie = true;
                    }
                }
            }
            if (nieUdaloSie)
                MessageBox.Show(sb.ToString());
        }

        public List<string> dodaj_kolekcje_do_bazy()
        {
            List<Zdjecie> lista = ZwrocZdjeciaZaznaczone();
            List<string> lista_sciezek = new List<string>();
            StringBuilder sb = new StringBuilder("Nie uda³o siê dodaæ do kolekcji nastepuj¹cych zdjêæ:\n");
            bool nieUdaloSie = false;

            foreach (Zdjecie z in lista)
            {
                if (z.DodajDoKolekcji() == false)
                {
                    sb.Append(z.Path + "\n");
                    if (nieUdaloSie == false)
                    {
                        nieUdaloSie = true;
                    }
                }
                else
                {
                    lista_sciezek.Add(z.Path);
                }
            }
            if (nieUdaloSie)
                MessageBox.Show(sb.ToString());
            
            return lista_sciezek;
        }

        private void UsunZdjecie(object sender, EventArgs e)
        {
            Zdjecie[] zdjecia = (Zdjecie[])Thumbnailview.WybraneZdjecia;
            if (zdjecia.Length != 0)
            {
                for (int i = 0; i < zdjecia.Length; i++)
                {
                    try
                    {
                        zdjecia[i].Usun();
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("Wybrane zdjêcie nie mo¿e zostaæ odnalezione!", "B³¹d!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        MessageBox.Show("Katalog z wybranym zdjêciem nie mo¿e zostaæ odnaleziony!", "B³¹d!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void widokMiniatur_selectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Zdjecie[] zdjecia = (Zdjecie[])Thumbnailview.WybraneZdjecia;
                if (zdjecia != null & zdjecia.Length == 1)
                {
                    ZdjecieInfo info = new ZdjecieInfo(Zdjecie.PobierzDaneExif(zdjecia[0].Path), zdjecia[0].NazwaPliku, zdjecia[0].Path, new Size(zdjecia[0].Rozmiar.Width, zdjecia[0].Rozmiar.Height), zdjecia[0].FormatPliku);
                    if (ZaznaczonoZdjecie != null)
                        ZaznaczonoZdjecie(info);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Wybrane zdjêcie nie mo¿e zostaæ odnalezione!", "B³¹d!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Katalog z wybranym zdjêciem nie mo¿e zostaæ odnaleziony!", "B³¹d!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void panele_onSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((TabControl)sender).SelectedTab == zdjecie1Tab && widokZdjecia1.czyZaladowaneZdjecie)
                {
                    ZdjecieInfo info = widokZdjecia1.pobierzInfoZdjecia;
                    if (ZaznaczonoZdjecie != null)
                        ZaznaczonoZdjecie(info);
                }
                if (((TabControl)sender).SelectedTab == zdjecie1Tab)
                {
                    if (WlaczonoWidokZdjecia != null)
                        WlaczonoWidokZdjecia();
                }
                if (((TabControl)sender).SelectedTab == miniatury1Tab)
                {
                    if (WlaczonoWidokZdjecia != null)
                        WylaczonoWidokZdjecia();
                }
            } catch (FileNotFoundException)
                {
                    MessageBox.Show("Wybrane zdjêcie nie mo¿e zostaæ odnalezione na dysku!", "B³¹d!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Katalog z wybranym zdjêciem nie mo¿e zostaæ odnaleziony!", "B³¹d!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
        }

        private void wybranoItem(ListViewItem listViewItem)
        {
            if ((WidokMiniatur.listViewTag)listViewItem.Tag == WidokMiniatur.listViewTag.zdjecie)
            {
                try
                {
                    Zdjecie[] z = new Zdjecie[] { (Zdjecie)Thumbnailview[listViewItem.ImageIndex - Thumbnailview.IloscKatalogow] };
                    this.widokZdjecia1.Wypelnij(z);
                    this.SetImageView();
                    if (WybranoZdjecie != null)
                        WybranoZdjecie(z[0]);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("Wybrane zdjêcie nie mo¿e zostaæ odnalezione na dysku!", "B³¹d!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Katalog z wybranym zdjêciem nie mo¿e zostaæ odnaleziony!", "B³¹d!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                try
                {
                    Katalog k = Thumbnailview.Katalogi[listViewItem.ImageIndex];
                    if (WybranoKatalog != null)
                        WybranoKatalog(k);
                    //MessageBox.Show("Wybrano katalog " + k.Path);
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Wybrany katalog nie mo¿e zostaæ odnaleziony na dysku!", "B³¹d!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void widokMiniatur_DoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem listViewItem = ((WidokMiniatur)sender).FocusedItem;
            if (listViewItem != null)
                wybranoItem(listViewItem);
        }

        private void widokMiniatur1_keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ListViewItem listViewItem = ((WidokMiniatur)sender).FocusedItem;
                if (listViewItem != null)
                    wybranoItem(listViewItem);
            }
            else if (e.KeyData == Keys.Back)
            {
                Katalog[] katalogi = Thumbnailview.Katalogi;
                for (int i = 0; i < katalogi.Length; i++) 
                {
                    if (katalogi[i].CzyDoGory == true)
                    {
                        if (WybranoKatalog != null)
                            WybranoKatalog(katalogi[i]);
                        break;
                    }
                }
            }
            else if (e.KeyData == Keys.R)
            {
                IZdjecie[] zdjecia = Thumbnailview.WybraneZdjecia;
                for (int i = 0; i < zdjecia.Length; i++)
                {
                    Rotate r = new Rotate(1);

                    zdjecia[i].DodajOperacje(new PolecenieOperacji(r, new object[0]));
                    zdjecia[i].WykonajOperacje();
                }
            }
            else if (e.KeyData == Keys.L)
            {
                IZdjecie[] zdjecia = Thumbnailview.WybraneZdjecia;
                for (int i = 0; i < zdjecia.Length; i++)
                {
                    Rotate r = new Rotate(2);

                    zdjecia[i].DodajOperacje(new PolecenieOperacji(r, new object[0]));
                    zdjecia[i].WykonajOperacje();
                }
            }
            else if (e.KeyData == (Keys.Control | Keys.F))
            {
                if (ZabierzFocus != null)
                    ZabierzFocus();
            }
            else if (e.KeyData == (Keys.Control | Keys.A))
            {
                for (int i = Thumbnailview.Katalogi.Length; i < Thumbnailview.Items.Count; i++)
                {
                    Thumbnailview.Items[i].Selected = true;
                }
            }
        }

        public bool WezFocus() {
            return widokMiniatur1.Focus();
        }

        private void widokZdjecia1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                int nastepne = Imageview.Zdjecie.indeks + 1;
                if (nastepne >= Thumbnailview.Ilosc)
                {
                    nastepne = Thumbnailview.IloscKatalogow;
                    MessageBox.Show("Nie ma ju¿ wiêcej zdjêæ w tym katalogu. Zostanie otwarte pierwsze.");
                }
                IZdjecie z = Thumbnailview.ZdjecieZIndeksem(nastepne);
                if (z != null)
                    Imageview.Wypelnij(new IZdjecie[1] { z });
            }
            else if (e.KeyData == Keys.Back)
            {
                int poprzednie = Imageview.Zdjecie.indeks - 1;
                if (poprzednie < Thumbnailview.IloscKatalogow)
                {
                    poprzednie = Thumbnailview.Ilosc - 1;
                    MessageBox.Show("To by³o pierwsze zdjêcie w katalogu. Zostanie otwarte ostatnie.");
                }
                IZdjecie z = Thumbnailview.ZdjecieZIndeksem(poprzednie);
                if (z != null)
                    Imageview.Wypelnij(new IZdjecie[1] { z });
            }
            else if (e.KeyData == Keys.R)
            {
                Rotate r = new Rotate(1);
                Imageview.DodajOperacje(new PolecenieOperacji(r, new object[0]));
            }
            else if (e.KeyData == Keys.L)
            {
                Rotate r = new Rotate(1);
                Imageview.DodajOperacje(new PolecenieOperacji(r, new object[0]));
            }
        }

        internal void ZmienionoRozmiarMiniatury()
        {
            widokMiniatur1.LargeImageList.ImageSize = new Size(Config.RozmiarMiniatury, Config.RozmiarMiniatury);
            IZdjecie[] zdjecia = widokMiniatur1.Zdjecia;
            foreach (Zdjecie z in zdjecia)
            {
                z.DisposeMini();
            }
            widokMiniatur1.Wypelnij(zdjecia, widokMiniatur1.Katalogi);
            widokMiniatur1.Refresh();
        }

        public void PokazAktualnyKatalog(string katalog) 
        {
            miniatury1Tab.Text = "Widok miniatur (" + katalog + ")";
        }
    }
}


