using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Data;


namespace Photo
{
    /// <summary>
    /// Klasa implementujace interfejs IZdjecie. 
    /// Zawierajaca wszystkie potrzebne informacje oraz wykonujaca wszystkie wymagane operacje.
    /// </summary>
    public class Zdjecie : IZdjecie, IDisposable
    {
        private string iiphotoTag;
        Bitmap miniatura;
        Bitmap duze;
        string path;
        int Orientation;
        string format;
        Size size;
        bool edytowano;
        bool tylkoDoOdczytu;
        string autor = "", komentarz = "", orientacja = "";
        int orient = -1;

        /// <summary>
        /// Indeks zdjecia w kolekcji
        /// </summary>
        public int indeks;

        /// <summary>
        /// Zaznaczenie wykonane na zdjeciu
        /// </summary>
        public Rectangle Zaznaczenie;

        List<PolecenieOperacji> operacje = new List<PolecenieOperacji>();
        List<long> tagi = new List<long>();

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="Path">Lokalizacja zdjecia na dysku</param>
        public Zdjecie(string Path)
        {
            path = Path;
            Zaznaczenie = new Rectangle(0,0,0,0);
            iiphotoTag = "brak";            
            FileInfo fi = new FileInfo(Path);
            tylkoDoOdczytu = fi.IsReadOnly;
            //ZweryfikujZdjecie();
            WypelnijListeTagow();
        }

        /// <summary>
        /// Propercja zwraca lub ustawia IIPhotoTag, zapisany w danych Exif zdjecia
        /// </summary>
        public string Id
        {
            get
            {
                if (iiphotoTag.Equals("brak"))
                {
                    iiphotoTag = "";
                    using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                    {
                        try
                        {
                            using (Image image = Image.FromStream(stream,
                                /* useEmbeddedColorManagement = */ true,
                                /* validateImageData = */ false))
                            {
                                PropertyItem[] items = image.PropertyItems;
                                foreach (PropertyItem item in items)
                                {
                                    if (item.Id == PropertyTags.IIPhotoTag)
                                    {
                                        iiphotoTag = PropertyTags.ParseProp(item);
                                    }
                                }
                            }
                        }
                        catch (ArgumentException)
                        {
                        }
                    }
                }
                return iiphotoTag;
            }
            set
            {
                if (value != null)
                {
                    iiphotoTag = value;
                    string tempFileName = System.IO.Path.GetTempFileName();
                    using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                    {
                        try
                        {
                            using (Image image = Image.FromStream(stream, true, false))
                            {
                                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                                PropertyItem propItem = image.PropertyItems[0];
                                propItem.Id = PropertyTags.IIPhotoTag;
                                propItem.Type = 2;
                                propItem.Value = encoding.GetBytes(value);
                                propItem.Len = propItem.Value.Length;
                                image.SetPropertyItem(propItem);
                                image.Save(tempFileName, image.RawFormat);
                            }
                        }
                        catch (ArgumentException)
                        {
                            return;
                        }
                    }
                    File.Delete(Path);
                    File.Move(tempFileName, Path);
                }
            }
        }

        /// <summary>
        /// Metoda sprawdza czy zdjecie ma ustawiony IIPhotoTag
        /// </summary>
        /// <returns></returns>
        public bool CzyUstawioneId()
        {
            return !Id.Equals("");
        }

        /// <summary>
        /// Metoda usuwa IIPhotoTag ze zdjecia
        /// </summary>
        public void UsunId()
        {
            if (tylkoDoOdczytu)
                return;
            if (!iiphotoTag.Equals(""))
            {
                iiphotoTag = "";
                string tempFileName = System.IO.Path.GetTempFileName();
                using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        using (Image image = Image.FromStream(stream, true, false))
                        {
                            image.RemovePropertyItem(PropertyTags.IIPhotoTag);
                            image.Save(tempFileName, image.RawFormat);
                        }
                    }
                    catch (ArgumentException)
                    {
                        return;
                    }
                }
                File.Delete(Path);
                File.Move(tempFileName, Path);
            }
        }

        /// <summary>
        /// Metoda usuwa zdjecie i zwraca wynik czy zdjecie zostalo usuniete
        /// </summary>
        /// <returns>Zmienna typu bool, mowiaca czy zdjecie zostalo poprawnie usuniete</returns>
        public bool Usun()
        {
            if (tylkoDoOdczytu)
            {
                MessageBox.Show("Plik tylko do odczytu! Nie można usunąć.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (MessageBox.Show("Czy napewno chcesz usunąć \"" + Path + "\"", "Potwierdzenie usunięcia pliku", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return false;

            if (CzyUstawioneId() == true)
            {
                UsunZdjecieZBazy();
            }

            System.IO.File.Delete(Path);
            return true;
        }

        /// <summary>
        /// Metoda zwraca lub ustawia miniature zdjecia
        /// </summary>
        public Bitmap Miniatura
        {
            set
            {
                miniatura = value;
            }
            get
            {
                if (miniatura != null)
                    return miniatura;
                else
                {
                    if (!System.IO.File.Exists(Path))
                        return null;
                    string folder = Path.Substring(0, Path.LastIndexOf('\\') + 1);
                    using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                    {
                        try
                        {
                            using (Image image = Image.FromStream(stream,
                                /* useEmbeddedColorManagement = */ true,
                                /* validateImageData = */ false))
                            {
                                int scaledH, scaledW;
                                if (image.Height > image.Width)
                                {
                                    scaledH = Config.RozmiarMiniatury;
                                    scaledW = (int)Math.Round(
                                        (double)(image.Width * scaledH) / image.Height);
                                }
                                else
                                {
                                    scaledW = Config.RozmiarMiniatury;
                                    scaledH = (int)Math.Round(
                                        (double)(image.Height * scaledW) / image.Width);
                                }
                                size = image.Size;
                                format = Zdjecie.sprawdzFormatPliku(image);
                                miniatura = (Bitmap)image.GetThumbnailImage(scaledW, scaledH, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), System.IntPtr.Zero);
                                Orientation = SprawdzOrientacje(image);
                                //UzyjOrientacji(miniatura);
                            }
                        }
                        catch (ArgumentException)
                        {
                            return null;
                        }
                    }

                    return miniatura;
                }
            }
        }

        /// <summary>
        /// Metoda statyczna tworzaca nowa bitmape z podanego obrazu
        /// </summary>
        /// <param name="i">Obraz</param>
        /// <returns>Bitmapa</returns>
        public static Bitmap FromImage(Image i)
        {
            Bitmap from = new Bitmap(i);
            foreach (PropertyItem pi in i.PropertyItems)
            {
                from.SetPropertyItem(pi);
            }
            return from;
        }

        /// <summary>
        /// Metoda zwraca lub ustawia faktyczne zdjecie
        /// </summary>
        public Bitmap Duze
        {
            get
            {
                if (duze != null)
                    return duze;
                else
                {
                    using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                    {
                        using (Image image = Image.FromStream(stream,
                            /* useEmbeddedColorManagement = */ true,
                            /* validateImageData = */ false))
                        {
                            duze = Zdjecie.FromImage(image);
                        }
                    }
                    //UzyjOrientacji(duze);
                    return duze;
                }
            }
            set
            {
                duze = value;
            }
        }

        /// <summary>
        /// Metoda zwraca nazwe pliku z ktorego wczytane jest dane zdjecie
        /// </summary>
        public string NazwaPliku
        {
            get
            {
                return path.Substring(path.LastIndexOf('\\') + 1);
            }
        }

        /// <summary>
        /// Propercja zwraca lokalizacje zdjecia na dysku
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                this.path = value;
            }
        }

        /// <summary>
        /// Propercja zwraca/ustawia liste tagow, ktore posiada zdjecie
        /// </summary>
        public List<Int64> Tagi
        {
            get
            {
                return tagi;
            }
            set
            {
                this.tagi = value;
            }
        }

        /// <summary>
        /// Propercja zwraca rozmiar zdjecia
        /// </summary>
        public Size Rozmiar
        {
            get
            {
                return size;
            }
        }

        /// <summary>
        /// Medota zwraca format podanego zdjecia
        /// </summary>
        /// <param name="i">Obraz</param>
        /// <returns>Napis z formatem pliku</returns>
        public static string sprawdzFormatPliku(Image i)
        {
            if (i.RawFormat.Equals(ImageFormat.Jpeg))
                return "Jpeg";
            else
                return "";
        }

        /// <summary>
        /// Propercja zwraca format zdjecia
        /// </summary>
        public string FormatPliku
        {
            get
            {
                if (format == null)
                {
                    if (Miniatura == null)
                        return "Nieznany";
                    else
                        return format;
                }
                return format;
            }
        }

        /// <summary>
        /// Metoda tworzy miniature o podanym maksymalnym rozmiarze
        /// </summary>
        /// <param name="maxSize">Maksymalny rozmiar miniatury</param>
        /// <returns>Miniatura zdjecia</returns>
        public Bitmap stworzMiniaturke(int maxSize)
        {
            int scaledH, scaledW;
            if (Duze.Height > Duze.Width)
            {
                scaledH = maxSize;
                scaledW = (int)Math.Round(
                    (double)(Duze.Width * scaledH) / Duze.Height);
            }
            else
            {
                scaledW = maxSize;
                scaledH = (int)Math.Round(
                    (double)(Duze.Height * scaledW) / Duze.Width);
            }
            return (Bitmap)Duze.GetThumbnailImage(scaledW, scaledH, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), System.IntPtr.Zero);
        }

        /// <summary>
        /// Metoda tworzy miniature o podanym maksymalnym rozmiarze z pliku na dysku
        /// </summary>
        /// <param name="fileName">Sciezka do zdjecia</param>
        /// <param name="maxSize">Maksymalny rozmiar miniatury</param>
        /// <returns>Miniatura zdjecia</returns>
        public static Bitmap stworzMiniaturke(string fileName, int maxSize)
        {
            Image i;
            string path = fileName.Substring(0, fileName.LastIndexOf('\\') + 1);
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (Image image = Image.FromStream(stream,
                    /* useEmbeddedColorManagement = */ true,
                    /* validateImageData = */ false))
                {
                    int scaledH, scaledW;
                    if (image.Height > image.Width)
                    {
                        scaledH = maxSize;
                        scaledW = (int)Math.Round(
                            (double)(image.Width * scaledH) / image.Height);
                    }
                    else
                    {
                        scaledW = maxSize;
                        scaledH = (int)Math.Round(
                            (double)(image.Height * scaledW) / image.Width);
                    }
                    i = image.GetThumbnailImage(scaledW, scaledH, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), System.IntPtr.Zero);
                }
            }
            return (Bitmap)i;
        }

        private static bool ThumbnailCallback()
        {
            return true;
        }


        #region Baza

        /// <summary>
        /// Metoda usuwa tagi zdjecia z bazy danych
        /// </summary>
        public void UsunTagi()
        {
            Db baza = new Db();

            baza.Polacz();
            try
            {
                baza.Delete("TagZdjecia", "id_zdjecia = " + Id + " and id_tagu in (select id_tagu from Tag where album=0)");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("blad sql" + ex.Message);
            }

            baza.Rozlacz();
        }

        /// <summary>
        /// Metoda usuwa zdjecie z albomow w bazie danych
        /// </summary>
        public void UsunAlbumy()
        {
            Db baza = new Db();

            baza.Polacz();
            try
            {
                baza.Delete("TagZdjecia", "id_zdjecia = " + Id + " and id_tagu in (select id_tagu from Tag where album=1)");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("blad sql" + ex.Message);
            }

            baza.Rozlacz();
        }
        
        /// <summary>
        /// Metoda usuwa zdjecie z bazy danych
        /// </summary>
        public void UsunZdjecieZBazy()
        {
            Db baza = new Db();

            baza.Polacz();
            try
            {
                baza.Delete("Zdjecie", "id_zdjecia = " + Id);
                baza.Delete("TagZdjecia", "id_zdjecia = " + Id);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("blad sql" + ex.Message);
            }

            baza.Rozlacz();
        }      

        /// <summary>
        /// Metoda zwraca nazwy tagow z bazy danych
        /// </summary>
        /// <returns>Lista nazw tagow</returns>
        public List<string> ZwrocNazwyTagow()
        {
            Db baza = new Db();
            List<string> lista = new List<string>();
            if (!CzyUstawioneId())
                return lista;

            baza.Polacz();

            try
            {
                DataSet ds = baza.Select("select nazwa from Tag where id_tagu in (select id_tagu from TagZdjecia where id_zdjecia =" + this.Id + ") and album = 0");

                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        if (!(r[0] is DBNull))
                        {
                            lista.Add((string)r[0]);                            
                        }
                    }
                }                
            }
            catch (SqlException ex)
            {
                MessageBox.Show("blad sql: " + ex.Message);
            }

            baza.Rozlacz();

            return lista;
        }

        /// <summary>
        /// Metoda zwraca date dodania do bazy danych
        /// </summary>
        /// <returns>Data dodania do bazy danych</returns>
        public string ZwrocDateDodaniaDoKolekcji()
        {
            Db baza = new Db();
            string data = "";
            if (!CzyUstawioneId())
                return data;

            baza.Polacz();

            try
            {
                DataSet ds = baza.Select("select data_dodania from Zdjecie where id_zdjecia =" + this.Id);

                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        if (!(r[0] is DBNull))
                        {
                            data = ((DateTime)r[0]).ToLocalTime().ToString();                            
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("blad sql: " + ex.Message);
            }

            baza.Rozlacz();

            return data;
        }

        /// <summary>
        /// Metoda zwraca nazwy albomow do ktorych nalezy zdjecie
        /// </summary>
        /// <returns>Lista nazw albomow</returns>
        public List<string> ZwrocNazwyAlbumow()
        {
            Db baza = new Db();
            List<string> lista = new List<string>();
            if (!CzyUstawioneId())
                return lista;

            baza.Polacz();

            try
            {
                DataSet ds = baza.Select("select nazwa from Tag where id_tagu in (select id_tagu from TagZdjecia where id_zdjecia =" + this.Id + ") and album = 1");

                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        if (!(r[0] is DBNull))
                        {
                            lista.Add((string)r[0]);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("blad sql: " + ex.Message);
            }

            baza.Rozlacz();

            return lista;
        }

        /// <summary>
        /// Metoda aktualizuje dane o zdjeciu w bazie danych
        /// </summary>
        public void AktualizujBaze()
        {
            if (CzyUstawioneId())
            {
                Db baza = new Db();

                baza.Polacz();

                try
                {
                    ZczytajPola();
                    baza.Update("Zdjecie", "komentarz=\'" + komentarz + "\', autor=\'" + autor + "\', orientacja=" + orient + " where id_zdjecia=" + Id);
                    //MessageBox.Show("Update3: komentarz=\'" + komentarz + "\', autor=\'" + autor + "\', orientacja=" + orient + " where id_zdjecia=" + Id);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("blad sql: " + ex.Message);
                }

                baza.Rozlacz();
            }

        }

        /// <summary>
        /// Metoda wypelnia tagi danymi z bazy
        /// </summary>
        public void WypelnijListeTagow()
        {
            if(CzyUstawioneId())
            {
                tagi.Clear();
                Db baza = new Db();
                baza.Polacz();
                try
                {
                    DataSet ds = baza.Select("select id_tagu from TagZdjecia where id_zdjecia=" + this.Id + " and id_tagu in (select id_tagu from Tag where album = 0)");

                    foreach (DataTable t in ds.Tables)
                    {
                        foreach (DataRow r in t.Rows)
                        {
                            if (!(r[0] is DBNull))
                            {
                                tagi.Add((Int64)r[0]);
                                //MessageBox.Show("" + r[0]);
                            }                             
                        }
                    }
                }
                catch(SqlException ex)
                {
                    MessageBox.Show("blad bazy: " + ex.Message);
                }
                baza.Rozlacz();
            }
        }

        /// <summary>
        /// Metoda sprawdza czy jezeli zdejecie mialo tag to czy ten tag skasowac czy nie
        /// </summary>
        public void ZweryfikujZdjecie()
        {
            string sciezka_z_bazy = "", nazwa_pliku_z_bazy = "", sciezka = "", nazwa_pliku = "";
            Db baza = new Db();            

            baza.Polacz();

            if (CzyUstawioneId() == true)
            {
                try
                {
                    DataSet ds = baza.Select("select sciezka, nazwa_pliku from Zdjecie where id_zdjecia=" + Id);

                    foreach (DataTable t in ds.Tables)
                    {
                        foreach (DataRow r in t.Rows)
                        {
                            if (!(r[0] is DBNull))
                            {
                                sciezka_z_bazy = (string)r[0];
                                nazwa_pliku_z_bazy = (string)r[1];
                            }
                        }
                    }

                    if (nazwa_pliku_z_bazy != "" && sciezka_z_bazy != "")
                    {

                        if (path.Equals(sciezka_z_bazy + "\\" + nazwa_pliku_z_bazy) == false)
                        {
                            //sprawdzic czy zdjecie istnieje na dysku tam gdzie powinno
                            //jesli tak i ma id ten sam to kasujemy z obecnego id
                            //jesli nie to uaktualniamy baze
                            sciezka = path.Substring(0, path.LastIndexOf("\\"));
                            if (sciezka.Length == 2)
                                sciezka += "\\";

                            nazwa_pliku = path.Substring(path.LastIndexOf("\\") + 1, path.Length - path.LastIndexOf("\\") - 1);


                            if (System.IO.File.Exists(sciezka_z_bazy + "\\" + nazwa_pliku_z_bazy) == true)
                            {
                                Zdjecie z = new Zdjecie(sciezka_z_bazy + "\\" + nazwa_pliku_z_bazy);
                                //jezeli ma ten sam tag to kasujemy 
                                //jesli inny tag to i w bazie nie ma wartosci dla tego tagu to kasujemy tag
                                //jesli inny tag i w bazie cos jest toaktualizujemy sciezke i nazwe plku

                                if (z.Id == Id)
                                {
                                    //kasuj id z this
                                    this.UsunId();
                                }
                                else
                                {
                                    //aktualizacja bazy dla this czyli sciezke i nazwe pliku                                    
                                    try
                                    {
                                        //ZczytajPola();
                                        baza.Update("Zdjecie", "sciezka=\'" + sciezka + "\', nazwa_pliku=\'" + nazwa_pliku + "\' where id_zdjecia=" + Id);
                                        //, komentarz=\'" + komentarz + "\', autor=\'" + autor + "\', orientacja=" + orient + "
                                        //MessageBox.Show("Update1: sciezka=\'" + sciezka + "\', nazwa_pliku=\'" + nazwa_pliku + "\', komentarz=\'" + komentarz + "\', autor=\'" + autor + "\', orientacja=" + orient + " where id_zdjecia=" + Id);
                                    }
                                    catch (SqlException ex)
                                    {
                                        MessageBox.Show("blad sql: " + ex.Message);
                                    }
                                }
                            }
                            else
                            {
                                //aktualizacja bazy dla this czyli sciezke i nazwe pliku                                
                                try
                                {
                                    baza.Update("Zdjecie", "sciezka=\'" + sciezka + "\', nazwa_pliku=\'" + nazwa_pliku + "\' where id_zdjecia=" + Id);
                                    //ZczytajPola();
                                    //baza.Update("Zdjecie", "sciezka=\'" + sciezka + "\', nazwa_pliku=\'" + nazwa_pliku + "\', komentarz=\'" + komentarz + "\', autor=\'" + autor + "\', orientacja=" + orient + " where id_zdjecia=" + Id);
                                    //MessageBox.Show("Update2: sciezka=\'" + sciezka + "\', nazwa_pliku=\'" + nazwa_pliku + "\', komentarz=\'" + komentarz + "\', autor=\'" + autor + "\', orientacja=" + orient + " where id_zdjecia=" + Id);
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show("blad sql: " + ex.Message);
                                }
                            }
                        }
                        else
                        {//gdy zdjecie zgadza sie ze sciezka to robimy update komentarz, autor itd
                            try
                            {
                                //ZczytajPola();
                                //baza.Update("Zdjecie", "komentarz=\'" + komentarz + "\', autor=\'" + autor + "\', orientacja=" + orient + " where id_zdjecia=" + Id);
                                //MessageBox.Show("Update3: komentarz=\'" + komentarz + "\', autor=\'" + autor + "\', orientacja=" + orient + " where id_zdjecia=" + Id);
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show("blad sql: " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        this.UsunId();
                    }
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            baza.Rozlacz();
        }

        /// <summary>
        /// Medota zczytuje dane Exif ze zdjecia
        /// </summary>
        private void ZczytajPola()
        {
            Dictionary<string, string> tablica;

            autor = "";
            komentarz = "";            
            orientacja = "";            
            orient = -1;

            try
            {
                tablica = PobierzExifDoBazy();

                if (tablica.ContainsKey("autor"))
                {
                    autor = tablica["autor"];
                }
                if (tablica.ContainsKey("komentarz"))
                {
                    komentarz = tablica["komentarz"];
                }                
                if (tablica.ContainsKey("orientacja"))
                {
                    orientacja = tablica["orientacja"];
                    if (orientacja == "Normal")
                        orient = 0;
                    else
                        orient = 1;
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Metoda dodaje zdjecie do bazy danych
        /// </summary>
        /// <returns>Czy operacja sie powiodla</returns>
        public bool DodajDoKolekcji()
        {
            if (tylkoDoOdczytu)
                return false;
            Db baza = new Db();            

            baza.Polacz();

            ZweryfikujZdjecie();

            if (CzyUstawioneId() == false)
            {
                //zczytanie z bazy i dodanie tagu do zdjecia
                
                Dictionary<string, string> tablica;

                string autor = "", komentarz = "", data_wykonania = "", orientacja = "", sciezka = "", nazwa_pliku = "";
                int orient = -1;

                try
                {
                    tablica = PobierzExifDoBazy();

                    if (tablica.ContainsKey("autor"))
                    {
                        autor = tablica["autor"];
                    }
                    if (tablica.ContainsKey("komentarz"))
                    {
                        komentarz = tablica["komentarz"];
                    }
                    if (tablica.ContainsKey("data_wykonania"))
                    {
                        data_wykonania = tablica["data_wykonania"];
                    }
                    if (tablica.ContainsKey("orientacja"))
                    {
                        orientacja = tablica["orientacja"];
                        if (orientacja == "Normal")
                            orient = 0;
                        else
                            orient = 1;
                    }

                    sciezka = path.Substring(0, path.LastIndexOf("\\"));
                    if (sciezka.Length == 2)
                        sciezka += "\\";

                    nazwa_pliku = path.Substring(path.LastIndexOf("\\") + 1, path.Length - path.LastIndexOf("\\") - 1);
                    
                    try
                    {
                        baza.Insert_czesci("zdjecie", "sciezka,data_dodania,data_wykonania,komentarz,autor,nazwa_pliku,orientacja", "'" + sciezka + "',current_time,null,'" + komentarz + "','" + autor + "','" + nazwa_pliku + "'," + orient);
                        //MessageBox.Show("dodano do bazy");

                        DataSet ds = baza.Select("select id_zdjecia from Zdjecie where sciezka=\'" + sciezka + "\' and nazwa_pliku=\'" + nazwa_pliku + "\'");

                        foreach (DataTable t in ds.Tables)
                        {
                            foreach (DataRow r in t.Rows)
                            {
                                if (!(r[0] is DBNull))
                                {
                                    Id = "" + r[0];
                                }
                            }
                        }
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show("bladsql");
                    }
                    
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }                
            }
            
            baza.Rozlacz();
            return true;
        }

        /// <summary>
        /// Metoda zwraca lokalizacje zdjecia na podstawie danych z bazy
        /// </summary>
        private string PathZBazy
        {
            get
            {
                string pathFromDb;
                Db db = new Db();
                db.Polacz();
                DataSet ds = db.Select("select sciezka from Zdjecie where id_zdjecia = " + Id);
                DataRow dr = ds.Tables[0].Rows[0];
                if (!(dr[0] is DBNull))
                {
                    pathFromDb = (string)dr[0];
                }
                else
                {
                    pathFromDb = "";
                }
                db.Rozlacz();
                return pathFromDb;
            }
            set
            {
                Db db = new Db();
                db.Polacz();
                db.Update("Zdjecie", "sciezka = '" + value + "' where id_zdjecia = " + Id);
                db.Rozlacz();
            }
        }

        #endregion

        #region Zdjecie Members

        /// <summary>
        /// Metoda dodaje operacje do wykonania na zdjeciu
        /// </summary>
        /// <param name="polecenie">Nowe polecenie wykonania operacji</param>
        public void DodajOperacje(PolecenieOperacji polecenie)
        {
            operacje.Add(polecenie);
        }

        /// <summary>
        /// Metoda wykonuje wszystkie oczekujace operacje
        /// </summary>
        public void WykonajOperacje()
        {
            if (operacje.Count > 0)
            {
                edytowano = true;
                foreach (PolecenieOperacji polecenie in operacje)
                {
                    polecenie.Wykonaj(this);
                }
                AktualizujMiniature();
                if (ZmodyfikowanoZdjecie != null)
                    ZmodyfikowanoZdjecie(null, this, RodzajModyfikacjiZdjecia.Zawartosc);
            }
        }

        /// <summary>
        /// Metoda aktualuzuje miniature
        /// </summary>
        public void AktualizujMiniature()
        {
            Miniatura = stworzMiniaturke(Config.RozmiarMiniatury);
        }

        /// <summary>
        /// Metoda usuwa wszystkie oczekujace operacje
        /// </summary>
        public void UsunWszystkieOperacje()
        {
            operacje.Clear();
        }

        /// <summary>
        /// Zdarzenie informujace o modyfikacji zdjecia
        /// </summary>
        public event ZmodyfikowanoZdjecieDelegate ZmodyfikowanoZdjecie;

        /// <summary>
        /// Metoda tworzy miniature do Widoku Miniatur i ja zwraca
        /// </summary>
        /// <returns>Miniatura do Widoku Miniatur</returns>
        public Image StworzMiniatureDoWidokuMiniatur()
        {
            int maxSize = Config.RozmiarMiniatury;
            int posX, posY;
            Bitmap newBitmap = new Bitmap(maxSize + 2, maxSize + 2);
            Graphics MyGraphics = Graphics.FromImage(newBitmap);

            if (Miniatura.Height > Miniatura.Width)
            {
                posX = (maxSize - Miniatura.Width) / 2;
                posY = 0;
            }
            else
            {
                posX = 0;
                posY = (maxSize - Miniatura.Height) / 2;
            }

            Rectangle MyRectan = new Rectangle(posX + 1, posY + 1, Miniatura.Width, Miniatura.Height);
            Pen p;
            if (CzyUstawioneId() && tylkoDoOdczytu)
            {
                p = new Pen(Color.LightBlue, 3);
            }
            else if (CzyUstawioneId())
            {
                p = new Pen(Color.Green, 3);
            }
            else if (tylkoDoOdczytu)
            {
                p = new Pen(Color.FromArgb(255,141, 138), 3);
            }
            else
            {
                p = new Pen(Color.LightGray, 1);
            }

            MyGraphics.DrawRectangle(p, 0, 0, maxSize + 1, maxSize + 1);
            p.Dispose();
            MyGraphics.DrawImage(Miniatura, MyRectan);
            MyGraphics.Dispose();
            return newBitmap;
        }

        #endregion

        #region Meta

        /// <summary>
        /// Metoda sprawdza orientacje w danych Exif zdjecia
        /// </summary>
        /// <param name="srcImg">Zdjecie z ktorego ma zostac odczytana orientacja</param>
        /// <returns>Wartosc okreslajaca orientacje zdjecia</returns>
        public static int SprawdzOrientacje(Image srcImg)
        {
            foreach (int id in srcImg.PropertyIdList)
            {
                if (id == PropertyTags.Orientation)
                {
                    return BitConverter.ToUInt16(srcImg.GetPropertyItem(id).Value, 0);
                }
            }
            return 1;
        }

        /// <summary>
        /// Metoda obraca zdjecie zgodnie z orientacja zapisana w danych Exif
        /// </summary>
        /// <param name="i"></param>
        public void UzyjOrientacji(Bitmap i)
        {
                    switch (Orientation)
                    {
                        case 2:
                            i.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            break;
                        case 3:
                            i.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case 4:
                            i.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            break;
                        case 5:
                            i.RotateFlip(RotateFlipType.Rotate90FlipY);
                            break;
                        case 6:
                            i.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 7:
                            i.RotateFlip(RotateFlipType.Rotate270FlipY);
                            break;
                        case 8:
                            i.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }
        }

        /// <summary>
        /// Metoda zwraca tablice z danymi Exif zdjecia
        /// </summary>
        /// <param name="fileName">Sciezka zdjecia</param>
        /// <returns>Tablica z danymi Exif</returns>
        public static PropertyItem[] PobierzDaneExif(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    using (Image image = Image.FromStream(stream, true, false))
                    {
                        return image.PropertyItems;
                    }
                }
                catch (ArgumentException)
                {
                    return new PropertyItem[0];
                }
            }            
        }

        /// <summary>
        /// Metoda zwraca tablice z danymi Exif zdjecia
        /// </summary>
        /// <returns>Tablica z danymi Exif</returns>
        public PropertyItem[] PobierzDaneExif()
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    using (Image image = Image.FromStream(stream,
                        /* useEmbeddedColorManagement = */ true,
                        /* validateImageData = */ false))
                    {
                        return image.PropertyItems;
                    }
                }
                catch (ArgumentException)
                {
                    iiphotoTag = "";
                }
            }
            return null;
        }

        /// <summary>
        /// Metoda zwraca slownik z danymi Exif, ktore zostana wpisane do bazy danych
        /// </summary>
        /// <returns>Slownik z danymi Exif</returns>
        public Dictionary<string, string> PobierzExifDoBazy()
        {
            PropertyItem[] propertyItems = Zdjecie.PobierzDaneExif(Path);
            Dictionary<int, string> defaults = PropertyTags.defaultExifDoBazy;
            Dictionary<string, string> d = new Dictionary<string,string>();
            string propertyValue;

            foreach (PropertyItem pItem in propertyItems)
            {
                if (defaults.ContainsKey(pItem.Id))
                {
                    propertyValue = PropertyTags.ParseProp(pItem);
                    if (!d.ContainsKey(defaults[pItem.Id]) && !propertyValue.Equals(""))
                    {
                        d.Add(defaults[pItem.Id], propertyValue);
                    }
                }
            }
            return d;
        }

        #endregion

        /// <summary>
        /// Propercja ustawiajaca/sprawdzajaca czy zdjecie bylo edytowane
        /// </summary>
        public bool Edytowano
        {
            get
            {
                return edytowano;
            }
            set
            {
                edytowano = value;
            }
        }

        /// <summary>
        /// Metoda zapisujaca zdjecie na dysku
        /// </summary>
        public void Zapisz()
        {
            if (duze == null || Edytowano == false)
                return;
            if (tylkoDoOdczytu)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "JPEG Images (*.jpg,*.jpeg)|*.jpg;*.jpeg";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string strImgName = sfd.FileName;
                    if (strImgName.EndsWith("jpg") || strImgName.EndsWith("jpeg"))
                    {
                        Duze.Save(strImgName, ImageFormat.Jpeg);
                        edytowano = false;
                    }
                }
            }
            else
            {
                Duze.Save(Path, ImageFormat.Jpeg);
                edytowano = false;
            }
        }

        /// <summary>
        /// Metoda usuwajaca bitmape zdjecia z pamieci
        /// </summary>
        public void DisposeDuze() 
        {
            if (duze != null)
            {
                duze.Dispose();
                duze = null;
            }
        }

        /// <summary>
        /// Metoda usuwajaca bitmape miniatury zdjecia z pamieci
        /// </summary>
        public void DisposeMini()
        {
            if (miniatura != null)
            {
                miniatura.Dispose();
                miniatura = null;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Metoda usuwajaca bitmapy zdjecia z pamieci
        /// </summary>
        public void Dispose()
        {
            DisposeMini();
            DisposeDuze();
        }

        #endregion

        /// <summary>
        /// Metoda resetujaca identyfikator
        /// </summary>
        public void ResetujId() 
        {
            iiphotoTag = "brak";
        }

        /// <summary>
        /// Metoda odswiezajaca tagi zdjecia
        /// </summary>
        public void ResetujTagi()
        {
            WypelnijListeTagow();
        }

        /// <summary>
        /// Metoda usuwajaca zdjecie o podanym identyfikatorze z albumu
        /// </summary>
        /// <param name="id">Identyfikator zdjecia</param>
        internal static void UsunZAlbumu(long id)
        {
            Db baza = new Db();
            baza.Polacz();
            try
            {
                baza.Delete("TagZdjecia", "id_zdjecia=" + id + " and id_tagu in (select id_tagu from Tag where album=1)");

            }
            catch (SqlException ex)
            {
                MessageBox.Show("blad sql: " + ex.Message);
            }
            baza.Rozlacz();            
        }

        
    }
}
