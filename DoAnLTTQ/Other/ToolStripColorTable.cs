using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnLTTQ.Other
{
    public class ToolStripColorTable : ProfessionalColorTable
    {
        public override Color MenuItemPressedGradientBegin => Color.Gray;
        public override Color MenuItemPressedGradientMiddle => Color.Gray;
        public override Color MenuItemPressedGradientEnd => Color.Gray;
        public override Color MenuItemSelected => Color.Gray;
        public override Color MenuBorder => Color.Gray;
        public override Color MenuItemBorder => Color.Transparent;
        public override Color ToolStripDropDownBackground => Color.DimGray;
        public override Color ToolStripContentPanelGradientBegin => Color.Red;
        public override Color ButtonCheckedGradientBegin => Color.Gray;
        public override Color ButtonCheckedGradientMiddle => Color.Gray;
        public override Color ButtonCheckedGradientEnd => Color.Gray;
        public override Color ButtonSelectedBorder => Color.DarkGray;
        public override Color ButtonSelectedGradientBegin => Color.Gray;
        public override Color ButtonSelectedGradientMiddle => Color.Gray;
        public override Color ButtonSelectedGradientEnd => Color.Gray;
        public override Color ButtonPressedGradientBegin => Color.Gray;
        public override Color ButtonPressedGradientMiddle => Color.Gray;
        public override Color ButtonPressedGradientEnd => Color.Gray;
        public override Color MenuItemSelectedGradientBegin => Color.Gray;
        public override Color MenuItemSelectedGradientEnd => Color.Gray;
        public override Color ImageMarginGradientBegin => Color.FromArgb(96, 96, 96);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(96, 96, 96);
        public override Color ImageMarginGradientEnd => Color.FromArgb(96, 96, 96);
    }

    public class MyToolStripRender : ToolStripProfessionalRenderer
    {
        public MyToolStripRender(ToolStripColorTable t) : base(t)
        { }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Text == "") return;
            base.OnRenderMenuItemBackground(e);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            return;
        }
    }

}
