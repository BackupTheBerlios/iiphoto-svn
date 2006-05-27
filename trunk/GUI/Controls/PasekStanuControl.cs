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
            Label.Text = "Rozpocz�to : Wyszukiwanie.";
            this.Invalidate();
        }

        public void ZakonczonoWyszukiwanie(IZdjecie[] zdjecia, Katalog[] k)
        {
            Label.Text = "Zako�czono : Wyszukiwanie.";
        }

        public void RozpoczetoAkcje(string Nazwa)
        {
            Label.Text = "Rozpocz�to : " + Nazwa + ".";
        }

        public void ZakonczonoAkcje(string Nazwa)
        {
            Label.Text = "Zako�cono : " + Nazwa + ".";
        }
    }
}
