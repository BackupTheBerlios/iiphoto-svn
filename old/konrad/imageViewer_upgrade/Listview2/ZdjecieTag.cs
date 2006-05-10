using System;
using System.Collections.Generic;
using System.Text;

namespace Photo
{
    public class ZdjecieTag
    {
        string filename;

        public ZdjecieTag(string file)
        {
            this.filename = file;
        }
        public string Filename
        {
            get
            {
                return filename;
            }
            set
            {
                this.filename = value;
            }
        }
    }
}
