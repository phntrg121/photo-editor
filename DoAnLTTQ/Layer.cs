using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnLTTQ
{
    public class Layer : IDisposable
    {
        Bitmap image;
        string name;
        bool visible;

        public Layer(Bitmap image, string name, bool visible)
        {
            this.image = new Bitmap(image);
            this.name = name;
            this.visible = visible;
        } 
        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                image.Dispose();
            }
        }

        public ref Bitmap Image
        {
            get
            {
                return ref image;
            }
        }

        public string Name
        {
            set
            {
                if (value != "")
                    name = value;
            }
            get
            {
                return name;
            }
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
        }

        public void ChangeVisible()
        {
            visible = !visible;
        }
    }
}
