using System;
using System.Collections.Generic;
using System.Text;

namespace Photo
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
