using System;
using System.Collections.Generic;
using System.Text;

namespace Listview2
{
    class ImageTag
    {
        string filename;

        public ImageTag(string file)
        {
            this.filename = file;
        }
        public string Filename {
            get {
                return filename;
            }
            set
            {
                this.filename = value;
            }
        }
    }
}
