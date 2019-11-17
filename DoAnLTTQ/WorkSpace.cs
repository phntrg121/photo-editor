using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnLTTQ
{
    public partial class WorkSpace : UserControl
    {
        public History History { get; set; }
        public DrawSpace DrawSpace { get; set; }
        public LayerContainer LayerContainer { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool Saved { get; set; }
        public bool Stored { get; set; }
        public WorkSpace(DrawSpace ds, LayerContainer lc, History h)
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            AutoScroll = true;
            DrawSpace = ds;
            LayerContainer = lc;
            History = h;
            Saved = true;
            Stored = false;
            this.Controls.Add(DrawSpace);
        }
    }
}
