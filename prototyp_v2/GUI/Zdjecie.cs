using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Photo
{
    public class Zdjecie : IZdjecie, IDisposable
    {
        Bitmap miniatura;
        string path;
        int Orientation;

        List<PolecenieOperacji> operacje = new List<PolecenieOperacji>();

        public Bitmap Miniatura
        {
            set
            {
                miniatura.Tag = this;
                miniatura = Miniatura;
            }
            get
            {
                return miniatura;
            }
        }

        public Bitmap Duze
        {
            get
            {
                return miniatura;
            }
        }

        public Zdjecie(string Path)
        {
            path = Path;
            miniatura = new Bitmap(path);
            UseOrientationTag();
        }

        public string NazwaPliku
        {
            get
            {
                return path.Substring(path.LastIndexOf('\\') + 1);
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

        public string FormatPliku
        {
            get
            {
                if (this.Duze.RawFormat.Equals(ImageFormat.Jpeg))
                    return "Jpeg";
                else if (this.Duze.RawFormat.Equals(ImageFormat.Tiff))
                    return "Tiff";
                else
                    return "";
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
                    polecenie.Wykonaj(miniatura);
                }
                if (ZmodyfikowanoZdjecie != null)
                    ZmodyfikowanoZdjecie(null, this, RodzajModyfikacjiZdjecia.Zawartosc);
            }
        }

        public void UsunWszystkieOperacje()
        {
            operacje.Clear();
        }

        public event ZmodyfikowanoZdjecieDelegate ZmodyfikowanoZdjecie;

        #endregion

        #region Meta

        public void SetIIPhotoProperty(string value)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            PropertyItem propItem = miniatura.PropertyItems[0];
            propItem.Id = PropertyTags.IIPhotoTag;
            propItem.Type = 2;
            propItem.Value = encoding.GetBytes(value);
            propItem.Len = propItem.Value.Length;
            miniatura.SetPropertyItem(propItem);
        }

        public string GetProperty(int propID)
        {
            PropertyItem i;
            string val;
              
            foreach(int id in miniatura.PropertyIdList)
            {
                if (id == propID)
                {
                    i = miniatura.GetPropertyItem(propID);
                    switch (i.Type)
                    {
                        case 1: val = Encoding.Unicode.GetString(i.Value); break;
                        case 2: val = Encoding.ASCII.GetString(i.Value); break;
                        case 3: val = BitConverter.ToUInt16(i.Value, 0).ToString(); break;
                        default: val = "Value not supported"; break;
                    }
                    return PropertyTags.ParseProp(propID, val);
                }
            }
            return "";
        }

        public void UseOrientationTag()
        {
            int Orientation;
            foreach (int id in miniatura.PropertyIdList)
            {
                if (id == PropertyTags.Orientation)
                {
                    Orientation = BitConverter.ToUInt16(miniatura.GetPropertyItem(id).Value, 0);
                    switch (Orientation)
                    {
                        case 2:
                            this.Miniatura.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            break;
                        case 3:
                            this.Miniatura.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case 4:
                            this.Miniatura.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            break;
                        case 5:
                            this.Miniatura.RotateFlip(RotateFlipType.Rotate90FlipY);
                            break;
                        case 6:
                            this.Miniatura.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 7:
                            this.Miniatura.RotateFlip(RotateFlipType.Rotate270FlipY);
                            break;
                        case 8:
                            this.Miniatura.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }
                }
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.miniatura.Dispose();
        }

        #endregion
    }
}
