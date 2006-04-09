using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;

/*
 Pierwszy parametr to sciezka do zdjecia wejsciowego, 
 * Drugi to sciezka do zdjecia w ktorym bedzie zapisany nasz tag przykladowy + odczytane jakies dane z Exif'a
 */

namespace Properties
{
    class Program
    {
        static void Main(string[] args)
        {
            MyBitmap img = new MyBitmap(args[0]);
            Console.WriteLine("Tag na poczatku: {0}", img.GetProperty(PropertyTags.IIPhotoTag));
            img.SetIIPhotoProperty("testowyTag");
            img.Save(args[1], ImageFormat.Jpeg);
            img.Dispose();

            img = new MyBitmap(args[1]);
            Console.WriteLine("Tag po wpisaniu: {0}", img.GetProperty(PropertyTags.IIPhotoTag));
            Console.WriteLine("Exif: {0}", img.GetProperty(PropertyTags.EquipMake));
            Console.WriteLine("Exif: {0}", img.GetProperty(PropertyTags.EquipModel));
            Console.WriteLine("Exif: {0}", img.GetProperty(PropertyTags.Orientation));
            Console.WriteLine("Exif: {0}", img.GetProperty(PropertyTags.ExifFlash));
            img.Dispose();
            Console.ReadLine();
        }
    }
}
