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
    /// Klasa opakowuj¹ca drzewo katalogow zawiera obiekty: Filetree i Button
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
        /// Metoda wykonywana po naciœnieciu klawisza odœwie¿ Tworzy drzewo katalogów od nowa
        /// </summary>
        private void button1_Click_1(object sender, EventArgs e)
        {
            fileTree1.Delete();
            fileTree1.Fill();  
        }

        /// <summary>
        /// Metoda wyœwietlaj¹ca zawartoœæ katlogu
        /// </summary>
        /// <param name="k">katalog który ma byæ wyœwietlony</param>
        public void ZaladujZawartoscKatalogu(Katalog k)
        {
            fileTree1.ZaladujZawartoscKatalogu(k);
        }

        /// <summary>
        /// Metoda do zmiany fokusa. Fokus po naciœniêciu kombinacji klawiszy Control+F przeskakuje do okna wyœwietlaj¹cego miniaturki
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
