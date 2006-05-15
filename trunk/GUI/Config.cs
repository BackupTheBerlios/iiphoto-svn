using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Photo
{
    public class Config
    {
        public static string katalogAplikacji
        {
            get
            {
                string katalog = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IIPhoto";
                if (!Directory.Exists(katalog))
                {
                    DirectoryInfo di = Directory.CreateDirectory(katalog);
                }
                return katalog;
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

        public static int maxRozmiarMiniatury
        {
            get
            {
                return 120;
            }
        }

        public static string plikBazy = "iiphoto.db3";
    }
}
