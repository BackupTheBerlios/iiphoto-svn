using System;
using System.Collections.Generic;
using System.Text;

namespace Listview2
{
    class ThumbnailTag
    {
        string path;

        public ThumbnailTag(string path) {
            this.path = path;
        }
        public string Filename {
            get {
                return path.Substring(path.LastIndexOf('\\')+1);
            }
        }
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                this.path = value;
            }
        }
    }
}
