using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnLTTQ.Tools
{
    class Picker
    {
        public Picker()
        {

        }

        public Color GetColor(ref Bitmap bmp, ref MouseEventArgs e)
        {
            return bmp.GetPixel(e.X, e.Y);
        }
    }
}
