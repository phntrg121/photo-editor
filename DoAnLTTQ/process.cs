using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnLTTQ
{
    class process
    {
        public process()
            {
            }
        public static bool ConvertToGray(Bitmap b)
        {
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    Color cl = b.GetPixel(i, j);

                    int rl = cl.R;
                    int gl = cl.G;
                    int bl = cl.B;
                    int gray = (byte)(.299 * rl + .587 * gl + .114 * bl);
                    rl = gray;
                    gl = gray;
                    bl = gray;
                    b.SetPixel(i, j, Color.FromArgb(rl, gl, bl));
                   


                }

            }
            return true;
        }

    }


}
