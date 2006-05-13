using System;
using System.Collections.Generic;
using System.Text;

namespace Photo
{
    public class Katalog
    {
        string path;
        bool do_gory;

        public Katalog(string p, bool d)
        {
            path = p;
            do_gory = d;
        }

        public string Path
        {
            get
            {
                return path;
            }
        }

        public bool CzyDoGory
        {
            get
            {
                return do_gory;
            }

        }

    }
}
