using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Photo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.WriteLine("Testowo wypisz nazwe pliku bazy: {0}", Properties.Settings.Default.plikBazyDanych);
            new Config();
            Application.Run(new PhotoForm());
        }
    }
}