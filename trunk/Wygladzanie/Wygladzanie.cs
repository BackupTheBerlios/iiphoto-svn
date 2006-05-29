using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
using Photo;

namespace Wygladzanie
{
    /// <summary>
    /// Pryk³adowa implementacja operacji graficznej.
    /// Do referencji projektu nale¿y do³¹czyæ plik exe programu przegl¹darki.
    /// Nastêpnie nale¿y zaimplementowaæ interfejs GUI.IOperacja.
    /// </summary>
    public class Wygladzanie : Photo.IOperacja
    {
        private int kodOperacji;

        #region IOperacja Members

        public string NazwaOperacji
        {
            get { return "Wygladzanie"; }
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
                return null;
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
                Photo.BitmapFilter.GaussianBlur(z.Duze, 4);
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
                Bitmap blured = new Bitmap(Math.Abs(z.Zaznaczenie.Width), Math.Abs(z.Zaznaczenie.Height), z.Duze.PixelFormat);
                Graphics g = Graphics.FromImage(blured);
                g.DrawImage(z.Duze, new Rectangle(0, 0, blured.Width, blured.Height), z.Zaznaczenie, GraphicsUnit.Pixel);
                g.Dispose();
                Photo.BitmapFilter.GaussianBlur(blured, 4);
                g = Graphics.FromImage(z.Duze);
                g.DrawImage(blured, z.Zaznaczenie);
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
