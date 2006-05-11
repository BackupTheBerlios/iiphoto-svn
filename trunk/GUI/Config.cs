using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Photo
{
    public class Config
    {
        string katalogDanychIIPhoto;

        string plikBazyDanych;

        public Config()
        {
            plikBazyDanych = "iiphoto.db3";
            katalogDanychIIPhoto = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IIPhoto";
            if (!Directory.Exists(katalogDanychIIPhoto))
            {
                DirectoryInfo di = Directory.CreateDirectory(katalogDanychIIPhoto);
            }
        }

        public string katalogDanych 
        {
            get
            {
                return katalogDanychIIPhoto;
            }
        }

        public string plikBazy
        {
            get
            {
                return plikBazyDanych;
            }
        }
    }
}
