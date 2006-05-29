using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

namespace Photo
{
    public class ZdjecieInfo
    {
        List<string> tagi;
        List<string> albumy;

        PropertyItem[] propItems;
        string nazwaPliku;
        string sciezka;
        Size rozmiar;
        string format;

        public ZdjecieInfo(PropertyItem[] pItems, string nazwa, string path, Size rozm, string f) {
            propItems = pItems;
            nazwaPliku = nazwa;
            sciezka = path;
            rozmiar = rozm;
            format = f;
        }

        public PropertyItem[] propertyItems
        {
            get
            {
                return propItems;
            }
        }
        public string NazwaPliku
        {
            get
            {
                return nazwaPliku;
            }
        }
        public string Sciezka
        {
            get
            {
                return sciezka;
            }
        }
        public Size Rozmiar
        {
            get
            {
                return rozmiar;
            }
        }
        public string Format
        {
            get
            {
                return format;
            }
        }
        public List<string> Tagi
        {
            get
            {
                if (tagi == null)
                    return new List<string>();
                return tagi;
            }
            set
            {
                tagi = value;
            }
        }
        public List<string> Albumy
        {
            get
            {
                if (albumy == null)
                    return new List<string>();
                return albumy;
            }
            set
            {
                albumy = value;
            }
        }
    }
}
