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
        Bitmap duze;
        string path;
        int Orientation;
        string format;
        public Rectangle Zaznaczenie;
        int maxSize;
        Size size;

        List<PolecenieOperacji> operacje = new List<PolecenieOperacji>();

        public Zdjecie(string Path)
        {
            path = Path;
            maxSize = Config.maxRozmiarMiniatury;
            Zaznaczenie = new Rectangle(0,0,0,0);
        }

        public Bitmap Miniatura
        {
            set
            {
                //miniatura.Tag = this;
                miniatura = value;
            }
            get
            {
                if (miniatura != null)
                    return miniatura;
                else
                {
                    string folder = Path.Substring(0, Path.LastIndexOf('\\') + 1);
                    using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                    {
                        try
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
                                size = image.Size;
                                format = Zdjecie.sprawdzFormatPliku(image);
                                miniatura = (Bitmap)image.GetThumbnailImage(scaledW, scaledH, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), System.IntPtr.Zero);
                                Orientation = SprawdzOrientacje(image);
                                UzyjOrientacji(miniatura);
                            }
                        }
                        catch (ArgumentException)
                        {
                            return null;
                        }
                    }

                    return miniatura;
                }
            }
        }

        public Bitmap Duze
        {
            get
            {
                if (duze != null)
                    return duze;
                else
                {
                    duze = new Bitmap(Path);
                    UzyjOrientacji(duze);
                    return duze;
                }
            }
            set
            {
                duze = value;
            }
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

        public Size Rozmiar
        {
            get
            {
                return size;
            }
        }

        public static string sprawdzFormatPliku(Image i)
        {
            if (i.RawFormat.Equals(ImageFormat.Jpeg))
                return "Jpeg";
            else if (i.RawFormat.Equals(ImageFormat.Tiff))
                return "Tiff";
            else
                return "";
        }

        public string FormatPliku
        {
            get
            {
                if (format == null)
                {
                    if (Miniatura == null)
                        return "Nieznany";
                    else
                        return format;
                }
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

        public static Bitmap stworzMiniaturke(string fileName, int maxSize)
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
            return (Bitmap)i;
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
                    polecenie.Wykonaj(this);
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
                try
                {
                    using (Image image = Image.FromStream(stream, true, false))
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
                catch (ArgumentException)
                {
                    return;
                }
            }
            File.Delete(fileName);
            File.Move(path + "img.tmp", fileName);
        }

        public static string ZwrocIIPhotoTag(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    using (Image image = Image.FromStream(stream,
                        /* useEmbeddedColorManagement = */ true,
                        /* validateImageData = */ false))
                    {
                        PropertyItem[] items = image.PropertyItems;
                        foreach (PropertyItem item in items)
                        {
                            if (item.Id == PropertyTags.IIPhotoTag)
                            {
                                return PropertyTags.ParseProp(item);
                            }
                        }
                        return "";
                    }
                }
                catch (ArgumentException)
                {
                    return "";
                }
            }
        }


        public int SprawdzOrientacje(Image srcImg)
        {
            foreach (int id in srcImg.PropertyIdList)
            {
                if (id == PropertyTags.Orientation)
                {
                    return BitConverter.ToUInt16(srcImg.GetPropertyItem(id).Value, 0);
                }
            }
            return 1;
        }

        public void UzyjOrientacji(Bitmap i)
        {
                    switch (Orientation)
                    {
                        case 2:
                            i.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            break;
                        case 3:
                            i.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case 4:
                            i.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            break;
                        case 5:
                            i.RotateFlip(RotateFlipType.Rotate90FlipY);
                            break;
                        case 6:
                            i.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 7:
                            i.RotateFlip(RotateFlipType.Rotate270FlipY);
                            break;
                        case 8:
                            i.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }
        }

        public static PropertyItem[] PobierzDaneExif(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    using (Image image = Image.FromStream(stream, true, false))
                    {
                        return image.PropertyItems;
                    }
                }
                catch (ArgumentException)
                {
                    return new PropertyItem[0];
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
                    if (!d.ContainsKey(defaults[pItem.Id]) && !propertyValue.Equals(""))
                    {
                        d.Add(defaults[pItem.Id], propertyValue);
                    }
                }
            }
            return d;
        }

        #endregion

        public void Zapisz()
        {
            Duze.Save(Path);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (miniatura != null)
                miniatura.Dispose();
            if (duze != null)
                duze.Dispose();
        }

        #endregion
    }
}
