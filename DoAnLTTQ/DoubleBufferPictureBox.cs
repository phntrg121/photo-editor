using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnLTTQ
{
    public partial class DoubleBufferPictureBox : PictureBox
    {
        public DoubleBufferPictureBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }

        public DoubleBufferPictureBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
