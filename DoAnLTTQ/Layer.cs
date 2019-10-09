using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DoAnLTTQ
{
    public class Layer : IDisposable
    {
        private Bitmap image;
        private Bitmap imageWithOpacity;
        private string name;
        private bool visible;
        private float opacity;

        public Layer(Bitmap image, string name, bool visible)
        {
            this.image = new Bitmap(image);
            this.imageWithOpacity = new Bitmap(image);
            this.name = name;
            this.visible = visible;
            this.opacity = 1f;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                image.Dispose();
                imageWithOpacity.Dispose();
            }
        }

        public ref Bitmap Image
        {
            get
            {
                return ref image;
            }
        }

        public Bitmap ImageWithOpacity
        {
            get
            {
                return SetOpacity();
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

        public float Opacity
        {
            set
            {
                opacity = value / 100;
            }
            get
            {
                return opacity * 100;
            }
        }

        private Bitmap SetOpacity()
        {
            if (imageWithOpacity != null) 
            {
                imageWithOpacity.Dispose();
                imageWithOpacity = null;
            }
            imageWithOpacity = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(imageWithOpacity))
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = opacity;
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(image, new Rectangle(0, 0, imageWithOpacity.Width, imageWithOpacity.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }
            return imageWithOpacity;
        }

        public void ChangeVisible()
        {
            visible = !visible;
        }
    }
}
