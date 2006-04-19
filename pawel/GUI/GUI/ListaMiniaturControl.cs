using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Photo
{
    public partial class ListaMiniaturControl : UserControl, IOpakowanieZdjec, IKontekst
    {
        public ListaMiniaturControl()
        {
            InitializeComponent();
        }

        private void miniatura_Click(object sender, EventArgs e)
        {
            listBox1.Items.Insert(0, System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            if (WybranoZdjecie != null)
                WybranoZdjecie((IZdjecie)sender);
        }

        public void RozmiescZdjecia()
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (IZdjecie zdjecie in zdjecia)
            {
                flowLayoutPanel1.Controls.Add((MiniaturaControl)zdjecie);
            }
            flowLayoutPanel1.Invalidate();
        }

        private bool Edycja;

        #region OpakowanieZdjec Members

        public event WybranoZdjecieDelegate WybranoZdjecie;

        private List<MiniaturaControl> zdjecia = new List<MiniaturaControl>();

        public IZdjecie this[int numer]
        {
            get
            {
                return zdjecia[numer];
            }
        }

        public int Ilosc
        {
            get
            {
                return zdjecia.Count;
            }
        }

        public void Dodaj(IZdjecie zdjecie)
        {
            MiniaturaControl mini = new MiniaturaControl(zdjecie);
            Dodaj(mini);
        }

        public void Dodaj(MiniaturaControl zdjecie)
        {
            DodajDoKontekstu(zdjecie);
            zdjecie.Click += new EventHandler(miniatura_Click);
            zdjecia.Add(zdjecie);
        }

        public void Usun(IZdjecie zdjecie)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Oproznij()
        {
            zdjecia.Clear();
            RozmiescZdjecia();
        }

        public void RozpocznijEdycje()
        {
            listBox1.Items.Insert(0, System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            Edycja = true;
        }

        public void ZakonczEdycje()
        {
            Edycja = false;
            foreach (IZdjecie zdjecie in zdjecia)
            {
                zdjecie.WykonajOperacje();
                zdjecie.UsunWszystkieOperacje();
            }
            listBox1.Items.Insert(0, System.Reflection.MethodInfo.GetCurrentMethod().ToString());
        }

        public void DodajOperacje(PolecenieOperacji operacja)
        {
            listBox1.Items.Insert(0, System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            if (Edycja == false)
            {
                RozpocznijEdycje();
                foreach (IZdjecie zdjecie in zdjecia)
                {
                    zdjecie.DodajOperacje(operacja);
                }
                ZakonczEdycje();
            }
            else
            {
                foreach (IZdjecie zdjecie in zdjecia)
                {
                    zdjecie.DodajOperacje(operacja);
                }
            }
        }

        public void UsunWszystkieOperacje()
        {
            foreach (IZdjecie zdjecie in zdjecia)
            {
                zdjecie.UsunWszystkieOperacje();
            }
        }

        public IZdjecie[] WybraneZdjecia()
        {
            return new IZdjecie[] { };
        }

        public void Wypelnij(IZdjecie[] zdjecia)
        {
            listBox1.Items.Insert(0, System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            Oproznij();
            foreach (IZdjecie zdjecie in zdjecia)
            {
                Dodaj((MiniaturaControl)zdjecie);
            }
            RozmiescZdjecia();
            if (WybranoZdjecie != null)
                WybranoZdjecie(null);
        }

        #endregion

        #region IKontekst Members

        public void DodajDoKontekstu(IZdjecie zdjecie)
        {
            zdjecie.ZmodyfikowanoZdjecie += new ZmodyfikowanoZdjecieDelegate(ZmodyfikowanoZdjecie);
        }

        public void UsunZKontekstu(IZdjecie zdjecie)
        {
            zdjecie.ZmodyfikowanoZdjecie -= new ZmodyfikowanoZdjecieDelegate(ZmodyfikowanoZdjecie);
        }

        public void ZmodyfikowanoZdjecie(IKontekst kontekst, IZdjecie zdjecie, RodzajModyfikacjiZdjecia rodzaj)
        {
            listBox1.Items.Insert(0, System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            Invalidate(true);
        }

        #endregion
    }
}
