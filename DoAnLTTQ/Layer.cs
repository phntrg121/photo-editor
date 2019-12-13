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
        private Stack<Bitmap> bmpList;
        private Stack<byte> oldOp;

        public Layer(Bitmap image, string name, bool visible)
        {
            bmpList = new Stack<Bitmap>();
            bmpList.Push(new Bitmap(image));
            this.image = bmpList.Peek();
            this.imageWithOpacity = new Bitmap(image);

            this.name = name;
            this.visible = visible;
            this.opacity = 1f;
            oldOp = new Stack<byte>();
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

        public Bitmap Image
        {
            get
            {
                return image;
            }
        }

        public void Stacking()
        {
            bmpList.Push(new Bitmap(bmpList.Peek()));
            image = bmpList.Peek();
        }

        public void UnStacking()
        {
            image.Dispose();
            bmpList.Pop();
            image = bmpList.Peek();
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
                oldOp.Push((byte)(opacity*100));
                opacity = value / 100;
            }
            get
            {
                return opacity * 100;
            }
        }
        
        public void RestoreOpacity()
        {
            opacity = (float)oldOp.Peek()/100;
            oldOp.Pop();
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
