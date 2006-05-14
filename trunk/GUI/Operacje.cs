
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
    /// Klasa opisuj¹ca polecenie operacji graficznej.
    /// </summary>
    public class PolecenieOperacji
    {
        /// <summary>
        /// Obiekt klasy IOperacja, która zostanie u¿yta do wykonania operacji.
        /// </summary>
        private IOperacja Operacja;
        /// <summary>
        /// Stos argumentów dla wykonania operacji z pola Operacja.
        /// </summary>
        private Stack<object> Argumenty = new Stack<object>();

        /// <summary>
        /// </summary>
        /// <param name="Operacja">Operacja jaka ma byæ wykonana.</param>
        /// <param name="Argumenty">Lista argumentów dla operacji.</param>
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
        /// <param name="bitmap">Bitmapa na której wykonywana jest operacja.</param>
        public void Wykonaj(Bitmap bitmap)
        {
            Operacja.Wykonaj(bitmap, Argumenty);
        }
    }

    public delegate void ZazadanieOperacjiDelegate(PolecenieOperacji polecenie);

    class Operacje
    {
        private List<IOperacja> operacje = new List<IOperacja>();
        public event ZazadanieOperacjiDelegate ZazadanieOperacji;

        public void WczytajPluginy()
        {
            Assembly asm = Assembly.LoadFile(System.IO.Path.GetFullPath("..\\..\\..\\Wyostrzenie\\bin\\Debug\\Wyostrzenie.dll"));
            foreach (Type type in asm.GetTypes())
            {
                ConstructorInfo ci = type.GetConstructor(new Type[] { });
                if (ci != null)
                {
                    IOperacja o = (IOperacja)ci.Invoke(new object[] { });
                    operacje.Add(o);
                }
            }
        }

        public void WczytajWbudowane()
        {
            operacje.Add(new XOR());
            operacje.Add(new Grayscale());
        }

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

    class XOR : IOperacja
    {
        private int kodOperacji;

        #region IOperacja Members

        public string NazwaOperacji
        {
            get { return "XOR"; }
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
                return new Bitmap("..\\..\\icons\\xor.png");
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
            Rectangle rect = new Rectangle(new Point(0, 0), Bitmap.Size);
            BitmapData bd = Bitmap.LockBits(rect, ImageLockMode.ReadWrite, Bitmap.PixelFormat);
            int bytes = bd.Width * bd.Height * 3;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(bd.Scan0, rgbValues, 0, bytes);
            for (int i = 0; i < bytes; i++)
            {
                rgbValues[i] = (byte)((int)rgbValues[i] ^ 0xff);
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

    class Grayscale : IOperacja
    {
        #region IOperacja Members

        private int kodOperacji;

        public string NazwaOperacji
        {
            get { return "Grayscale"; }
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
            get { return Properties.Resources.Grayscale; }
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

        public void Wykonaj(Bitmap Bitmap, Stack<object> Argumenty)
        {
            BitmapFilter.GrayScale(Bitmap);
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