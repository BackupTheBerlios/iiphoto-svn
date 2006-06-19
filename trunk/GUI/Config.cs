using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Photo
{
    /// <summary>
    /// Klasa dostarcza podstawowe dane, jak zmienne srodowiskowe, aktualna konfiguracja.
    /// </summary>
    public class Config
    {
        private static int rozmiarMiniatury = 100;

        public static void sprawdzWarunkiPoczatkowe()
        {
            if (!Directory.Exists(katalogAplikacji))
            {
                DirectoryInfo di = Directory.CreateDirectory(katalogAplikacji);
            }
        }

        /// <summary>
        /// Zwraca katalog domowy aplikacji
        /// </summary>
        public static string katalogAplikacji
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IIPhoto";
            }
        }

        /// <summary>
        /// Zwraca katalog "Moje Dokumenty" aktualnego uzytkownika
        /// </summary>
        public static string katalogMojeDokumenty
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }

        /// <summary>
        /// Zwraca katalog "Moje Obrazy" aktualnego uzytkownika
        /// </summary>
        public static string katalogMojeObrazy
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
        }

        /// <summary>
        /// Zwraca katalog "Pulpit" aktualnego uzytkownika
        /// </summary>
        public static string katalogPulpit
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
        }

        /// <summary>
        /// Zwraca aktualny rozmiar miniatur
        /// </summary>
        public static int RozmiarMiniatury
        {
            get
            {
                return rozmiarMiniatury;
            }
        }

        /// <summary>
        /// Umozliwia zmiane rozmiaru miniatur
        /// </summary>
        /// <param name="size"></param>
        public static void ZmienRozmiarMiniatury(int size)
        {
            switch (size)
            {
                case 1: rozmiarMiniatury = 75; break;
                case 2: rozmiarMiniatury = 90; break;
                case 3: rozmiarMiniatury = 120; break;
                default: rozmiarMiniatury = 90; break;
            }
        }

        /// <summary>
        /// Nazwa pliku z baza danych SQLite
        /// </summary>
        public static string plikBazy = "iiphoto.db3";
    }
}
