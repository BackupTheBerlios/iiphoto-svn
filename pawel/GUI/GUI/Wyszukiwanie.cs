using System;
using System.Collections.Generic;
using System.Text;

namespace Photo
{
    class Wyszukiwanie : IWyszukiwanie
    {
        private List<string> wynik = new List<string>();

        #region IWyszukiwanie Members

        public void And(IWyszukiwanie W)
        {
            wynik.AddRange(((Wyszukiwanie)W).wynik);
        }

        public void Or(IWyszukiwanie W)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void And(string Tabela, string Wartosc)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Or(string Tabela, string Wartosc)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void And(string WyrazenieSQL)
        {
            wynik.Add(WyrazenieSQL);
        }

        public void Or(string WyrazenieSQL)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IZdjecie[] PodajWynik()
        {
            List<MiniaturaControl> wynik = new List<MiniaturaControl>();
            foreach (string plik in this.wynik)
            {
                MiniaturaControl mini = new MiniaturaControl();
                mini.ImageLocation = plik;
                wynik.Add(mini);
            }
            return wynik.ToArray();
        }

        #endregion
    }
}
