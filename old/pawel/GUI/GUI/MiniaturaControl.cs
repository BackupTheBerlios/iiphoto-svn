using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Photo
{
    public partial class MiniaturaControl : UserControl, IZdjecie
    {
        List<PolecenieOperacji> operacje = new List<PolecenieOperacji>();

        public MiniaturaControl()
        {
            InitializeComponent();
            pictureBox1.Click += new EventHandler(pictureBox1_Click);
        }

        public MiniaturaControl(IZdjecie zdjecie)
        {
            InitializeComponent();
            pictureBox1.Click += new EventHandler(pictureBox1_Click);
        }

        new public event EventHandler Click;

        void pictureBox1_Click(object sender, EventArgs e)
        {
            zaznaczone = !zaznaczone;
            if (Click != null)
            {
                if (zaznaczone)
                    Click(this, e);
                else
                    Click(null, e);
            }
        }

        public string ImageLocation
        {
            set
            {
                pictureBox1.ImageLocation = value;
            }
            get
            {
                return pictureBox1.ImageLocation;
            }
        }

        public bool zaznaczone
        {
            get
            {
                return (BackColor == Color.YellowGreen) ? true : false;
            }

            set
            {
                if (value == true)
                {
                    BackColor = Color.YellowGreen;
                }
                else
                {
                    BackColor = Color.Olive;
                }
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
                    polecenie.Wykonaj((Bitmap)pictureBox1.Image);
                }
                if (ZmodyfikowanoZdjecie != null)
                    ZmodyfikowanoZdjecie(null, this, RodzajModyfikacjiZdjecia.Zawartosc);
            }
        }

        public void UsunWszystkieOperacje()
        {
            operacje.Clear();
        }

        public Image Miniatura
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public event ZmodyfikowanoZdjecieDelegate ZmodyfikowanoZdjecie;

        #endregion
    }
}
