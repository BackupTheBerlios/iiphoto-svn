using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Photo.DialogBoxes
{
    /// <summary>
    /// Formatka pyta sie uzytkownika czy zapisac zmiany w zdjeciu
    /// </summary>
    public partial class CzyZapisac : Form
    {
        RodzajDecyzji decyzja;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="path">Sciezka do pliku</param>
        public CzyZapisac(string path)
        {
            InitializeComponent();
            textBox1.Text = "Czy zapisaæ niezapisane zmiany w pliku " + path + "?";
        }

        /// <summary>
        /// Propercja zwraca decyzje uzytkownika
        /// </summary>
        public RodzajDecyzji Decyzja
        {
            get
            {
                return decyzja;
            }
        }

        private void Yes_Button_Click(object sender, EventArgs e)
        {
            decyzja = RodzajDecyzji.Tak;
            this.Close();
        }

        private void YesForAll_Click(object sender, EventArgs e)
        {
            decyzja = RodzajDecyzji.TakDlaWszystkich;
            this.Close();
        }

        private void No_Button_Click(object sender, EventArgs e)
        {
            decyzja = RodzajDecyzji.Nie;
            this.Close();
        }

        private void NoForAll_Click(object sender, EventArgs e)
        {
            decyzja = RodzajDecyzji.NieDlaWszystkich;
            this.Close();
        }
    }
}