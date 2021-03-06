using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Reflection;

namespace Photo
{
    /// <summary>
    /// Klasa opisuj�ca polecenie operacji graficznej.
    /// </summary>
    public class PolecenieOperacji
    {
        /// <summary>
        /// Obiekt klasy IOperacja, kt�ra zostanie u�yta do wykonania operacji.
        /// </summary>
        private IOperacja Operacja;
        /// <summary>
        /// Stos argument�w dla wykonania operacji z pola Operacja.
        /// </summary>
        private Stack<object> Argumenty = new Stack<object>();

        /// <summary>
        /// </summary>
        /// <param name="Operacja">Operacja jaka ma by� wykonana.</param>
        /// <param name="Argumenty">Lista argument�w dla operacji.</param>
        public PolecenieOperacji(IOperacja Operacja, object[] Argumenty)
        {
            this.Operacja = Operacja;
            foreach (object arg in Argumenty)
            {
                this.Argumenty.Push(arg);
            }
        }

        /// <summary>
        /// Powoduje wykonanie operacji opisanej przez to polecenie na bitmapie.
        /// </summary>
        /// <param name="z">Bitmapa na kt�rej wykonywana jest operacja.</param>
        public void Wykonaj(Zdjecie z)
        {
            Operacja.Wykonaj(z, Argumenty);
        }
    }

    /// <summary>
    /// Delegat zazadania peracji
    /// </summary>
    /// <param name="polecenie">Polecenie operacji, o ktora zazadano</param>
    public delegate void ZazadanieOperacjiDelegate(PolecenieOperacji polecenie);

    /// <summary>
    /// Klasa wykonujaca czynnosci inicjalizujace Operacje
    /// </summary>
    class Operacje
    {
        private List<IOperacja> operacje = new List<IOperacja>();
        public event ZazadanieOperacjiDelegate ZazadanieOperacji;

        /// <summary>
        /// Metoda wczytujaca dynamicznie pluginy z Operacjami
        /// </summary>
        public void WczytajPluginy()
        {
            if (!System.IO.Directory.Exists(Config.katalogPluginow))
                return;
            string[] pluginy = System.IO.Directory.GetFiles(Config.katalogPluginow, "*.dll");
            foreach (string plugin in pluginy)
            {
                Assembly asm = Assembly.LoadFile(plugin);
                foreach (Type type in asm.GetTypes())
                {
                    ConstructorInfo ci = type.GetConstructor(new Type[] { });
                    if (ci != null)
                    {
                        object obj = ci.Invoke(new object[] { });
                        if (obj is IOperacja)
                            operacje.Add((IOperacja)obj);
                    }
                }
            }
        }

        /// <summary>
        /// Metoda wczytujaca wbudowane Operacje
        /// </summary>
        public void WczytajWbudowane()
        {
            operacje.Add(new XOR());
            operacje.Add(new Grayscale());
            operacje.Add(new Crop());
            operacje.Add(new Rotate(2));
            operacje.Add(new Rotate(1));
            operacje.Add(new Rotate(3));
        }

        /// <summary>
        /// Metoda dodajaca wczytane operacje do interfejsu graficznego
        /// </summary>
        /// <param name="tool">ToolStip na ktory ma zostac dodana operacja</param>
        /// <param name="filtry">ToolStripMenuItem na ktory ma zostac dodana operacja</param>
        public void WrzucDoGui(ToolStrip tool, ToolStripMenuItem filtry)
        {
            foreach (IOperacja operacja in operacje)
            {
                if (operacja.CzyNaToolbar() == true)
                {
                    ToolStripButton button = new ToolStripButton(operacja.NazwaOperacji, operacja.Ikona);
                    button.Tag = operacja;
                    button.Click += new EventHandler(button_Click);
                    tool.Items.Add(button);
                }
                else
                {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem(operacja.NazwaOperacji, operacja.Ikona);
                    menuItem.Tag = operacja;
                    menuItem.Click += new EventHandler(button_Click);
                    filtry.DropDownItems.Add(menuItem);
                }
            }
        }

        void button_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            IOperacja operacja = (IOperacja)item.Tag;
            PolecenieOperacji polecenie = new PolecenieOperacji(operacja, operacja.PodajArgumenty().ToArray());
            ZazadanieOperacji(polecenie);
        }
    }

    /// <summary>
    /// Klasa wykonujaca odwrocenie kolorow zdjecia
    /// </summary>
    class XOR : IOperacja
    {
        private int kodOperacji;

        #region IOperacja Members

        public string NazwaOperacji
        {
            get { return "Negatyw"; }
        }

        int IOperacja.KodOperacji
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
                return Properties.Resources.xor;
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

        public void Wykonaj(Zdjecie z, System.Collections.Generic.Stack<object> Argumenty)
        {
            if (z.Zaznaczenie.IsEmpty)
            {
                BitmapFilter.Invert(z.Duze);
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
                Bitmap xored = new Bitmap(Math.Abs(z.Zaznaczenie.Width), Math.Abs(z.Zaznaczenie.Height), z.Duze.PixelFormat);
                Graphics g = Graphics.FromImage(xored);
                g.DrawImage(z.Duze, new Rectangle(0, 0, xored.Width, xored.Height), z.Zaznaczenie, GraphicsUnit.Pixel);
                g.Dispose();
                BitmapFilter.Invert(xored);
                g = Graphics.FromImage(z.Duze);
                g.DrawImage(xored, z.Zaznaczenie);
                g.Dispose();
            }
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

    /// <summary>
    /// Klasa wykonujaca konwersje zdjecia do odcieni szarosci
    /// </summary>
    class Grayscale : IOperacja
    {
        #region IOperacja Members

        private int kodOperacji;

        public string NazwaOperacji
        {
            get { return "Skala szaro�ci"; }
        }

        public int KodOperacji
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
            get { return Properties.Resources.desaturate; }
        }

        public string Autor
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public string Wersja
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public string Kontakt
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Stack<object> PodajArgumenty()
        {
            return new Stack<object>();
        }

        public void Wykonaj(Zdjecie z, Stack<object> Argumenty)
        {
            if (z.Zaznaczenie.IsEmpty)
            {
                BitmapFilter.GrayScale(z.Duze);
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
                Bitmap grayed = new Bitmap(Math.Abs(z.Zaznaczenie.Width), Math.Abs(z.Zaznaczenie.Height), z.Duze.PixelFormat);
                Graphics g = Graphics.FromImage(grayed);
                g.DrawImage(z.Duze, new Rectangle(0, 0, grayed.Width, grayed.Height), z.Zaznaczenie, GraphicsUnit.Pixel);
                g.Dispose();
                BitmapFilter.GrayScale(grayed);
                g = Graphics.FromImage(z.Duze);
                g.DrawImage(grayed, z.Zaznaczenie);
                g.Dispose();
            }
        }

        public bool CzyNaToolbar()
        {
            return false;
        }

        #endregion
    }

    /// <summary>
    /// Klasa wykonujaca obcinanie zdjecia do podanych rozmiarow
    /// </summary>
    class Crop : IOperacja
    {
        #region IOperacja Members

        private int kodOperacji;

        public string NazwaOperacji
        {
            get { return "Wytnij"; }
        }

        public int KodOperacji
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
            get { return Properties.Resources.crop; }
        }

        public string Autor
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public string Wersja
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public string Kontakt
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Stack<object> PodajArgumenty()
        {
            return new Stack<object>();
        }

        public void Wykonaj(Zdjecie z, Stack<object> Argumenty)
        {
            if (z.Zaznaczenie.Width != 0 && z.Zaznaczenie.Height != 0)
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
                Bitmap cropped = new Bitmap(Math.Abs(z.Zaznaczenie.Width), Math.Abs(z.Zaznaczenie.Height), z.Duze.PixelFormat);
                Graphics g = Graphics.FromImage(cropped);
                g.DrawImage(z.Duze, new Rectangle(0, 0, cropped.Width, cropped.Height), z.Zaznaczenie, GraphicsUnit.Pixel);
                g.Dispose();
                foreach (PropertyItem pi in z.Duze.PropertyItems)
                {
                    cropped.SetPropertyItem(pi);
                }
                z.Duze = cropped;
            }
        }

        public bool CzyNaToolbar()
        {
            return false;
        }

        #endregion
    }

    /// <summary>
    /// Klasa wykonujaca obroty zdjecia
    /// </summary>
    class Rotate : IOperacja
    {
        #region IOperacja Members

        private int kodOperacji;

        public Rotate(int k)
        {
            kodOperacji = k;
        }

        public string NazwaOperacji
        {
            get 
            {
                switch (KodOperacji)
                {
                    case 1: return "90� CW";
                    case 2: return "90� CCW";
                    case 3: return "180� CW";
                    default: return "";
                }
            }
        }

        public int KodOperacji
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
                switch (KodOperacji)
                {
                    case 1: return Properties.Resources.rotate_90;
                    case 2: return Properties.Resources.rotate_270;
                    case 3: return Properties.Resources.rotate_180;
                    default: return null;
                }
            }
        }

        public string Autor
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public string Wersja
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public string Kontakt
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Stack<object> PodajArgumenty()
        {
            return new Stack<object>();
        }

        public void Wykonaj(Zdjecie z, Stack<object> Argumenty)
        {
            switch (KodOperacji)
            {
                case 1:
                    z.Duze.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 2:
                    z.Duze.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case 3:
                    z.Duze.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
            }
            z.Duze = Zdjecie.FromImage(z.Duze);
        }

        public bool CzyNaToolbar()
        {
            switch (KodOperacji)
            {
                case 1: return true;
                case 2: return true;
                case 3: return false;
            }
            return false;
        }

        #endregion
    }
}