using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
using Photo;

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

        public void Wykonaj(System.Drawing.Bitmap Bitmap, System.Collections.Generic.Stack<object> Argumenty)
        {
            Rectangle rect = new Rectangle(new Point(0,0), Bitmap.Size);
            BitmapData bd = Bitmap.LockBits(rect, ImageLockMode.ReadWrite, Bitmap.PixelFormat);
            int bytes = bd.Width * bd.Height * 3;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(bd.Scan0, rgbValues, 0, bytes);
            for (int i = 0; i < bytes; i++)
            {
                rgbValues[i] = (byte)((int)rgbValues[i] & 0x7f);
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, bd.Scan0, bytes);
            Bitmap.UnlockBits(bd);
        }

        public Stack<object> PodajArgumenty()
        {
            return new Stack<object>();
        }

        #endregion

        #region IOperacja Members


        public bool CzyNaToolbar()
        {
            return false;
        }

        #endregion
    }
}
