using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace Photo
{
    /// <summary>
    /// Formatka dzieiczaca po StatusStrip, wyswietlajaca wazne dla uzytkownika informacje.
    /// </summary>
    class PasekStanuControl : StatusStrip
    {
        private ToolStripStatusLabel wyszukiwanieLabel;
        private ToolStripStatusLabel katalogLabel;
        private ToolStripSeparator sep;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public PasekStanuControl() :
            base()
        {
            InitializeComponent();

            this.Items.Add(wyszukiwanieLabel);
            this.Items.Add(sep);
            this.Items.Add(katalogLabel);
        }

        /// <summary>
        /// Metoda informujaca uzytkownika o rozpoczeniu wyszukiwania
        /// </summary>
        public void RozpoczetoWyszukiwanie(IWyszukiwanie wyszukiwanie)
        {
            wyszukiwanieLabel.Text = "Rozpoczêto: Wyszukiwanie";
            this.Invalidate();
        }

        /// <summary>
        /// Metoda informujaca uzytkownika o zakonczeniu wyszukiwania
        /// </summary>
        public void ZakonczonoWyszukiwanie(IZdjecie[] zdjecia, Katalog[] k, bool czyzdrzewa)
        {
            wyszukiwanieLabel.Text = "Zakoñczono: Wyszukiwanie";
        }

        /// <summary>
        /// Metoda informujaca uzytkownika o rozpoczeniu akcji
        /// </summary>
        /// <param name="Nazwa">Nazwa akcji do wyswietlenia</param>
        public void RozpoczetoAkcje(string Nazwa)
        {
            wyszukiwanieLabel.Text = "Rozpoczêto: " + Nazwa;
        }

        /// <summary>
        /// Metoda informujaca uzytkownika o zakonczeniu akcji
        /// </summary>
        /// <param name="Nazwa">Nazwa akcji do wyswietlenia</param>
        public void ZakonczonoAkcje(string Nazwa)
        {
            wyszukiwanieLabel.Text = "Zakoñcono: " + Nazwa;
        }

        /// <summary>
        /// Metoda informujaca uzytkownika o zmianie zrodla wyswietlanych zdjec
        /// </summary>
        /// <param name="dir">Nazwa zrodla do wyswietlenia</param>
        public void ZmienionoZrodlo(string dir)
        {
            katalogLabel.Text = dir;
        }

        /// <summary>
        /// Metoda inicjalizujaca formatke
        /// </summary>
        private void InitializeComponent()
        {
            //Label Wyszukiwanie
            wyszukiwanieLabel = new ToolStripStatusLabel();
            wyszukiwanieLabel.Text = "Aplikacja uruchomiona";

            //Separator
            sep = new ToolStripSeparator();

            //Label Katalog
            katalogLabel = new ToolStripStatusLabel();
            katalogLabel.Spring = true;
            katalogLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        }
    }
}
