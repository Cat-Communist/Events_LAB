using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame.Objects
{
    internal class Mob : BaseObject
    {
        public int TTL = 100;
        public static Random rnd = new Random();

        public Action<Mob> OnTimeEnd;

        public Mob() : base() {}
        public Mob(float x, float y, float angle) : base(x, y, angle) {}


        public override void Render(Graphics g)
        {
            g.FillEllipse(
                new SolidBrush(Color.GreenYellow),
                -15, -15,
                30, 30
            );

            g.DrawString(
                TTL.ToString(),
                new Font("Verdana", 8),
                new SolidBrush(Color.GreenYellow),
                10, 10
            );
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();

            path.AddEllipse(-15, -15, 30, 30);
            return path;
        }

        public override void Update()
        {
            TTL--;

            if (OnTimeEnd != null && TTL == 0)
                OnTimeEnd(this);
        }
    }
}
