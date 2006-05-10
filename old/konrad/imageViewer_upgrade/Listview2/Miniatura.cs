using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Photo
{
    public class Miniatura : IMiniatura
    {
        Bitmap bitmap;
        List<PolecenieOperacji> operacje = new List<PolecenieOperacji>();

        public MiniaturaTag tag;

        public Miniatura()
        {
        }

        public Miniatura(string filename) : this()
        {
            this.bitmap = new Bitmap(filename);
        }

        public Bitmap Bitmap
        {
            set
            {
                bitmap.Tag = this;
                bitmap = Bitmap;
            }
            get
            {
                return bitmap;
            }
        }
    }
}
