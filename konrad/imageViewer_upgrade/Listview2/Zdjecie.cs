using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Photo
{
    public class Zdjecie : IZdjecie
    {
        Bitmap bitmap;
        List<PolecenieOperacji> operacje = new List<PolecenieOperacji>();

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

        #region Zdjecie Members

        public void DodajOperacje(PolecenieOperacji polecenie)
        {
            operacje.Add(polecenie);
        }

        public void WykonajOperacje()
        {
            if (operacje.Count > 0)
            {
                foreach (PolecenieOperacji polecenie in operacje)
                {
                    polecenie.Wykonaj(bitmap);
                }
                if (ZmodyfikowanoZdjecie != null)
                    ZmodyfikowanoZdjecie(null, this, RodzajModyfikacjiZdjecia.Zawartosc);
            }
        }

        public void UsunWszystkieOperacje()
        {
            operacje.Clear();
        }

        public Image Miniatura
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public event ZmodyfikowanoZdjecieDelegate ZmodyfikowanoZdjecie;

        #endregion
    }
}
