using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Photo
{
    /// <summary>
    /// Klasa opakowuj�ca drzewo katalogow zawiera obiekty: Filetree i Button
    /// </summary>
    public partial class DrzewoOpakowane : UserControl
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public DrzewoOpakowane()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metoda wykonywana po naci�nieciu klawisza od�wie� Tworzy drzewo katalog�w od nowa
        /// </summary>
        private void button1_Click_1(object sender, EventArgs e)
        {
            fileTree1.Delete();
            fileTree1.Fill();  
        }

        /// <summary>
        /// Metoda wy�wietlaj�ca zawarto�� katlogu
        /// </summary>
        /// <param name="k">katalog kt�ry ma by� wy�wietlony</param>
        public void ZaladujZawartoscKatalogu(Katalog k)
        {
            fileTree1.ZaladujZawartoscKatalogu(k);
        }

        /// <summary>
        /// Metoda do zmiany fokusa. Fokus po naci�ni�ciu kombinacji klawiszy Control+F przeskakuje do okna wy�wietlaj�cego miniaturki
        /// </summary>
        private void fileTree1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.F))
            {
                if (ZabierzFocus != null)
                    ZabierzFocus();
            }
        }

    }
}
