using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Photo
{
    partial class WidokMiniatur : System.Windows.Forms.ListView, IOpakowanieZdjec, IKontekst
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // WidokMiniatur
            // 
            miniatury = new List<Miniatura>();
            //Activate double buffering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            this.defaultImageSize = 120;
            this.ResumeLayout(false);

        }
    }
}
