using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Photo
{
    public static class Config
    {
        static string katalogDanychIIPhoto;

        static Config()
        {
            katalogDanychIIPhoto = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IIPhoto";
            if (!Directory.Exists(katalogDanychIIPhoto))
            {
                DirectoryInfo di = Directory.CreateDirectory(katalogDanychIIPhoto);
            }
        }

        public static string katalogDanych {
            get
            {
                return katalogDanychIIPhoto;
            }
        }
    }
}
