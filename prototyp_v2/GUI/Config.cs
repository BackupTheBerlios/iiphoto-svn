using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Photo
{
    public class Config
    {
        string katalogDanychIIPhoto;

        public Config()
        {
            katalogDanychIIPhoto = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IIPhoto";
            if (!Directory.Exists(katalogDanychIIPhoto))
            {
                DirectoryInfo di = Directory.CreateDirectory(katalogDanychIIPhoto);
            }
        }

    }
}
