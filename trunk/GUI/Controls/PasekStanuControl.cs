using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace Photo
{
    //class Thre

    class PasekStanuControl : StatusStrip
    {
        private ToolStripStatusLabel wyszukiwanieLabel;
        private ToolStripStatusLabel katalogLabel;
        private ToolStripSeparator sep;

        public PasekStanuControl() :
            base()
        {
            InitializeComponent();

            this.Items.Add(wyszukiwanieLabel);
            this.Items.Add(sep);
            this.Items.Add(katalogLabel);
        }

        public void RozpoczetoWyszukiwanie(IWyszukiwanie wyszukiwanie)
        {
            wyszukiwanieLabel.Text = "Rozpoczęto: Wyszukiwanie";
            this.Invalidate();
        }

        public void ZakonczonoWyszukiwanie(IZdjecie[] zdjecia, Katalog[] k)
        {
            wyszukiwanieLabel.Text = "Zakończono: Wyszukiwanie";
        }

        public void RozpoczetoAkcje(string Nazwa)
        {
            wyszukiwanieLabel.Text = "Rozpoczęto: " + Nazwa;
        }

        public void ZakonczonoAkcje(string Nazwa)
        {
            wyszukiwanieLabel.Text = "Zakońcono: " + Nazwa;
        }

        public void ZmienionoZrodlo(string dir)
        {
            katalogLabel.Text = dir;
        }

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
