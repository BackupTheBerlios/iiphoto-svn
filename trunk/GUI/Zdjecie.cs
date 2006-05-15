using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.IO;

namespace Photo
{
    public class Zdjecie : IZdjecie, IDisposable
    {
        Bitmap miniatura;
        string path;
        int Orientation;
        string format;

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
            format = sprawdzFormatPliku();
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

        public string sprawdzFormatPliku()
        {
            if (this.Duze.RawFormat.Equals(ImageFormat.Jpeg))
                return "Jpeg";
            else if (this.Duze.RawFormat.Equals(ImageFormat.Tiff))
                return "Tiff";
            else
                return "";
        }

        public string FormatPliku
        {
            get
            {
                return format;
            }
        }

        public Image stworzMiniaturke(int maxSize)
        {
            int scaledH, scaledW;
            if (Duze.Height > Duze.Width)
            {
                scaledH = maxSize;
                scaledW = (int)Math.Round(
                    (double)(Duze.Width * scaledH) / Duze.Height);
            }
            else
            {
                scaledW = maxSize;
                scaledH = (int)Math.Round(
                    (double)(Duze.Height * scaledW) / Duze.Width);
            }
            return Duze.GetThumbnailImage(scaledW, scaledH, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), System.IntPtr.Zero);
        }

        public static Image stworzMiniaturke(string fileName, int maxSize)
        {
            Image i;
            string path = fileName.Substring(0, fileName.LastIndexOf('\\') + 1);
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (Image image = Image.FromStream(stream,
                    /* useEmbeddedColorManagement = */ true,
                    /* validateImageData = */ false))
                {
                    int scaledH, scaledW;
                    if (image.Height > image.Width)
                    {
                        scaledH = maxSize;
                        scaledW = (int)Math.Round(
                            (double)(image.Width * scaledH) / image.Height);
                    }
                    else
                    {
                        scaledW = maxSize;
                        scaledH = (int)Math.Round(
                            (double)(image.Height * scaledW) / image.Width);
                    }
                    i = image.GetThumbnailImage(scaledW, scaledH, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), System.IntPtr.Zero);
                }
            }
            return i;
        }

        public static bool ThumbnailCallback()
        {
            return true;
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

 /*       public void UstawIIPhotoProperty(string value)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            PropertyItem propItem = miniatura.PropertyItems[0];
            propItem.Id = PropertyTags.IIPhotoTag;
            propItem.Type = 2;
            propItem.Value = encoding.GetBytes(value);
            propItem.Len = propItem.Value.Length;
            miniatura.SetPropertyItem(propItem);
        }*/

        public string IIPhotoTag
        {
            get
            {
                PropertyItem item;
                try
                {
                    item = Duze.GetPropertyItem(PropertyTags.IIPhotoTag);
                }
                catch
                {
                    return "";
                }
                return PropertyTags.ParseProp(item);
            }
            set
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                PropertyItem propItem = Duze.PropertyItems[0];
                propItem.Id = PropertyTags.IIPhotoTag;
                propItem.Type = 2;
                propItem.Value = encoding.GetBytes(value);
                propItem.Len = propItem.Value.Length;
                Duze.SetPropertyItem(propItem);
            }
        }

        public static void UstawIIPhotoTag(string fileName, string value)
        {
            string path = fileName.Substring(0, fileName.LastIndexOf('\\') + 1);
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (Image image = Image.FromStream(stream,
                    /* useEmbeddedColorManagement = */ true,
                    /* validateImageData = */ false))
                {
                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    PropertyItem propItem = image.PropertyItems[0];
                    propItem.Id = PropertyTags.IIPhotoTag;
                    propItem.Type = 2;
                    propItem.Value = encoding.GetBytes(value);
                    propItem.Len = propItem.Value.Length;
                    image.SetPropertyItem(propItem);
                    image.Save(path + "img.tmp", image.RawFormat);
                }
            }
            File.Delete(fileName);
            File.Move(path + "img.tmp", fileName);
        }

        public void UseOrientationTag()
        {
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

        public static PropertyItem[] PobierzDaneExif(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (Image image = Image.FromStream(stream,
                    /* useEmbeddedColorManagement = */ true,
                    /* validateImageData = */ false))
                {
                    return image.PropertyItems;
                }
            }
        }
        public PropertyItem[] PobierzDaneExif()
        {
            return Duze.PropertyItems;
        }

        public static Dictionary<string, string> PobierzExifDoBazy(string fileName)
        {
            PropertyItem[] propertyItems = Zdjecie.PobierzDaneExif(fileName);
            Dictionary<int, string> defaults = PropertyTags.defaultExifDoBazy;
            Dictionary<string, string> d = new Dictionary<string,string>();
            string propertyValue;

            foreach (PropertyItem pItem in propertyItems)
            {
                if (defaults.ContainsKey(pItem.Id))
                {
                    propertyValue = PropertyTags.ParseProp(pItem);
                    d.Add(defaults[pItem.Id], propertyValue);
                }
            }
            return d;
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
