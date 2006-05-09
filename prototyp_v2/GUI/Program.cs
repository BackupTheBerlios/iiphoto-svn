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
            if (Config.katalogDanych.Equals(""))
                return;
            Application.Run(new PhotoForm());
        }
    }
}