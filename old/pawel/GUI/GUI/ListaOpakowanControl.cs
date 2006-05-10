using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace Photo
{
    class ListaOpakowanControl : TabControl, IOpakowanieZdjec
    {
        public void WynikWyszukiwania(IZdjecie[] zdjecia)
        {
            TabPages[0].Controls.Clear();
            ListaMiniaturControl miniatury = new ListaMiniaturControl();
            miniatury.WybranoZdjecie += WybranoZdjecie;
            miniatury.Dock = DockStyle.Fill;
            miniatury.Wypelnij(zdjecia);
            TabPages[0].Controls.Add(miniatury);
        }

        #region IOpakowanieZdjec Members

        private IOpakowanieZdjec AktywneOpakowanie
        {
            get
            {
                IOpakowanieZdjec opakowanie = null;
                foreach (Control control in SelectedTab.Controls)
                {
                    if (control is IOpakowanieZdjec)
                        opakowanie = (IOpakowanieZdjec)control;
                }
                return opakowanie;
            }
        }

        public IZdjecie this[int numer]
        {
            get 
            {
                if (AktywneOpakowanie != null)
                    return AktywneOpakowanie[numer];
                return null;
            }
        }

        public int Ilosc
        {
            get
            {
                if (AktywneOpakowanie != null)
                    return AktywneOpakowanie.Ilosc;
                return 0;
            }
        }

        public void Dodaj(IZdjecie zdjecie)
        {
            if (AktywneOpakowanie != null)
                AktywneOpakowanie.Dodaj(zdjecie);
        }

        public void Usun(IZdjecie zdjecie)
        {
            if (AktywneOpakowanie != null)
                AktywneOpakowanie.Usun(zdjecie);
        }

        public void Oproznij()
        {
            if (AktywneOpakowanie != null)
                AktywneOpakowanie.Oproznij();
        }

        public IZdjecie[] WybraneZdjecia()
        {
            if (AktywneOpakowanie != null)
                return AktywneOpakowanie.WybraneZdjecia();
            return new IZdjecie[] { };
        }

        public void RozpocznijEdycje()
        {
            if (AktywneOpakowanie != null)
                AktywneOpakowanie.RozpocznijEdycje();
        }

        public void ZakonczEdycje()
        {
            if (AktywneOpakowanie != null)
                AktywneOpakowanie.ZakonczEdycje();
        }

        public void DodajOperacje(PolecenieOperacji operacja)
        {
            if (AktywneOpakowanie != null)
                AktywneOpakowanie.DodajOperacje(operacja);
        }

        public void UsunWszystkieOperacje()
        {
            if (AktywneOpakowanie != null)
                AktywneOpakowanie.UsunWszystkieOperacje();
        }

        public event WybranoZdjecieDelegate WybranoZdjecie;

        public void Wypelnij(IZdjecie[] zdjecia)
        {
            if (AktywneOpakowanie != null)
                AktywneOpakowanie.Wypelnij(zdjecia);
        }

        #endregion
    }
}
