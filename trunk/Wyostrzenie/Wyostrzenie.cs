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
            Photo.BitmapFilter.Sharpen(Bitmap, 5);
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
