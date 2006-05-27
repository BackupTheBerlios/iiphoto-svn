using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Photo
{
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

        public static string katalogAplikacji
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IIPhoto";
            }
        }

        public static string katalogMojeDokumenty
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }

        public static string katalogMojeObrazy
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
        }

        public static string katalogPulpit
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
        }

        public static int RozmiarMiniatury
        {
            get
            {
                return rozmiarMiniatury;
            }
        }

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

        public static string katalogMiniatur
        {
            get
            {
                string katalog = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IIPhoto\\thumbs";
                if (!Directory.Exists(katalog))
                {
                    DirectoryInfo di = Directory.CreateDirectory(katalog);
                }
                return katalog;
            }
        }

        public static string plikBazy = "iiphoto.db3";
    }
}
