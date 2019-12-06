using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnLTTQ.Tools
{
    public partial class Select : UserControl
    {
        public bool Selected { get; set; }
        Rectangle select_rect;
        PointF startP;
        public Select()
        {
            InitializeComponent();
        }
        public void GetLocation(PointF p)
        {
            startP = p;
        }

    }
}
