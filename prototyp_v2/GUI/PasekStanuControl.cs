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
            Label.Text = "Rozpoczęto : Wyszukiwanie.";
            this.Invalidate();
        }

        public void ZakonczonoWyszukiwanie(IZdjecie[] zdjecia)
        {
            Label.Text = "Zakończono : Wyszukiwanie.";
        }

        public void RozpoczetoAkcje(string Nazwa)
        {
            Label.Text = "Rozpoczęto : " + Nazwa + ".";
        }

        public void ZakonczonoAkcje(string Nazwa)
        {
            Label.Text = "Zakońcono : " + Nazwa + ".";
        }
    }
}
