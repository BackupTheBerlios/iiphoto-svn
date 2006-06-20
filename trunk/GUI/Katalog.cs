using System;
using System.Collections.Generic;
using System.Text;

namespace Photo
{
    /// <summary>
    /// Klasa zawierajaca podstawowe informacje o katalogu
    /// </summary>
    public class Katalog
    {
        string path;
        bool do_gory;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="p">Sciezka katalogu na dysku twardym</param>
        /// <param name="d">Czy katalog jest linkiem do katalogu nadrzednego</param>
        public Katalog(string p, bool d)
        {
            path = p;
            do_gory = d;
        }

        /// <summary>
        /// Propercja zwracajaca sciezke katalogu
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }
        }

        /// <summary>
        /// Propercja zwracajaca wartosc boolowska, czy katalog jest linkiem do katalogu nadrzednego
        /// </summary>
        public bool CzyDoGory
        {
            get
            {
                return do_gory;
            }
        }

    }
}
