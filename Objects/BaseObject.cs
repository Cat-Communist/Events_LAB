using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace MiniGame.Objects
{
    internal class BaseObject
    {
        public float x;
        public float y;
        public float angle;

        public Action<BaseObject, BaseObject> OnOverlap;

        public BaseObject(float x, float y, float angle)
        {
            this.x = x;
            this.y = y;
            this.angle = angle;
        }

        public Matrix GetTransform()
        {
            var matrix = new Matrix();
            matrix.Translate(x, y);
            matrix.Rotate(angle);

            return matrix;
        }

        public virtual void Render(Graphics g) { }

        public virtual GraphicsPath GetGraphicsPath()
        {
            return new GraphicsPath();
        }

        public virtual bool Overlaps(BaseObject obj, Graphics g)
        {
            var path1 = this.GetGraphicsPath();
            var path2 = obj.GetGraphicsPath();

            path1.Transform(this.GetTransform());
            path2.Transform(obj.GetTransform());

            var region = new Region(path1);
            region.Intersect(path2);

            return !region.IsEmpty(g);
        }

        public virtual void Overlap(BaseObject obj)
        {
            if (this.OnOverlap != null)
            {
                this.OnOverlap(this, obj);
            }
        }
    }
}
