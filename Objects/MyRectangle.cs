using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame.Objects
{
    internal class MyRectangle : BaseObject
    {
        public MyRectangle(float x, float y, float angle) : base(x, y, angle)
        {
        }

        public override void Render(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.Yellow), -20, -15, 40, 30);
            g.DrawRectangle(new Pen(Color.Red, 2), -20, -15, 40, 30);
        }
    }
}
