using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace Photo
{
    //class Thre

    class PasekStanuControl : StatusStrip
    {
        private ToolStripStatusLabel Label = new ToolStripStatusLabel();

        public PasekStanuControl() :
            base()
        {
            this.Items.Add(Label);
        }

        public void RozpoczetoWyszukiwanie(IWyszukiwanie wyszukiwanie)
        {
            Label.Text = "Rozpoczêto : Wyszukiwanie.";
            this.Invalidate();
        }

        public void ZakonczonoWyszukiwanie(IZdjecie[] zdjecia, Katalog[] k)
        {
            Label.Text = "Zakoñczono : Wyszukiwanie.";
        }

        public void RozpoczetoAkcje(string Nazwa)
        {
            Label.Text = "Rozpoczêto : " + Nazwa + ".";
        }

        public void ZakonczonoAkcje(string Nazwa)
        {
            Label.Text = "Zakoñcono : " + Nazwa + ".";
        }
    }
}
