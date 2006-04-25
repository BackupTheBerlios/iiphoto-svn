using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Photo
{
    public class Zdjecie : IZdjecie, IDisposable
    {
        Bitmap miniatura;
        string path;

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

        public Zdjecie(string Path)
        {
            path = Path;
            miniatura = new Bitmap(path);
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

        public Bitmap Duze
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public event ZmodyfikowanoZdjecieDelegate ZmodyfikowanoZdjecie;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.miniatura.Dispose();
        }

        #endregion
    }
}
