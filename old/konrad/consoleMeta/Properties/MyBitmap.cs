using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Properties
{
    class MyBitmap
    {
        public Bitmap image;

        public MyBitmap(string fileName)
        {
            image = new Bitmap(fileName);
        }

        public void Save(string ImgName, ImageFormat format)
        {
            image.Save(ImgName, format);
        }

        public void Dispose()
        {
            image.Dispose();
        }

        public void SetIIPhotoProperty(string value) {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            PropertyItem propItem = image.PropertyItems[0];
            propItem.Id = PropertyTags.IIPhotoTag;
            propItem.Type = 2;
            propItem.Value = encoding.GetBytes(value);
            propItem.Len = propItem.Value.Length;
            image.SetPropertyItem(propItem);
        }

        public string GetProperty(int propID) {
            PropertyItem i;
            try
            {
                string val;
                i = image.GetPropertyItem(propID);
                switch (i.Type)
                {
                    case 1: val = Encoding.Unicode.GetString(i.Value); break;
                    case 2: val = Encoding.ASCII.GetString(i.Value); break;
                    case 3: val = BitConverter.ToUInt16(i.Value, 0).ToString(); break;
                    default: val = "Value not supported"; break;
                }
                return val;
            }
            catch
            {
                return "";
            } 
        }
    }
}
