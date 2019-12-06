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
        bool selecting = false;
        Rectangle selection;
        public Select()
        {
            InitializeComponent();
        }
        public void GetLocation(ref MouseEventArgs e)
        {
            selecting = true;
            selection = new Rectangle(new Point(e.X, e.Y), new Size());
        }

    }
}
