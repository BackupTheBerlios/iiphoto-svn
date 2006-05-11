using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Photo
{
    public static class Config
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
    }
}
