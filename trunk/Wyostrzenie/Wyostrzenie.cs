using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
//using Photo;

namespace Wyostrzenie
{
    /// <summary>
    /// Pryk³adowa implementacja operacji graficznej.
    /// Do referencji projektu nale¿y do³¹czyæ plik exe programu przegl¹darki.
    /// Nastêpnie nale¿y zaimplementowaæ interfejs GUI.IOperacja.
    /// </summary>
    public class Wyostrzenie : Photo.IOperacja
    {
        private int kodOperacji;

        #region IOperacja Members

        public string NazwaOperacji
        {
            get { return "Wyostrzenie"; }
        }

        int Photo.IOperacja.KodOperacji
        {
            get
            {
                return kodOperacji;
            }
            set
            {
                kodOperacji = value;
            }
        }

        public Image Ikona
        {
            get
            {
                return Properties.Resources.filtr;
            }
        }

        public string Autor
        {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        public string Wersja
        {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        public string Kontakt
        {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        public void Wykonaj(Photo.Zdjecie z, System.Collections.Generic.Stack<object> Argumenty)
        {
            if (z.Zaznaczenie.IsEmpty)
            {
                Photo.BitmapFilter.Sharpen(z.Duze, 10);
            }
            else
            {
                if (z.Zaznaczenie.Width < 0)
                {
                    z.Zaznaczenie.X += z.Zaznaczenie.Width;
                    z.Zaznaczenie.Width *= -1;
                }
                if (z.Zaznaczenie.Height < 0)
                {
                    z.Zaznaczenie.Y += z.Zaznaczenie.Height;
                    z.Zaznaczenie.Height *= -1;
                }
                Bitmap sharpened = new Bitmap(Math.Abs(z.Zaznaczenie.Width), Math.Abs(z.Zaznaczenie.Height), z.Duze.PixelFormat);
                Graphics g = Graphics.FromImage(sharpened);
                g.DrawImage(z.Duze, new Rectangle(0, 0, sharpened.Width, sharpened.Height), z.Zaznaczenie, GraphicsUnit.Pixel);
                g.Dispose();
                Photo.BitmapFilter.Sharpen(sharpened, 10);
                g = Graphics.FromImage(z.Duze);
                g.DrawImage(sharpened, z.Zaznaczenie);
                g.Dispose();
            }
        }

        public Stack<object> PodajArgumenty()
        {
            return new Stack<object>();
        }

        public bool CzyNaToolbar()
        {
            return false;
        }

        #endregion
    }
}
