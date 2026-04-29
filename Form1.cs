using MiniGame.Objects;

namespace MiniGame
{
    public partial class Form1 : Form
    {
        int scoreCount = 0;
        Player player;
        Marker marker;
        List<BaseObject> myObjects = new();
        public Form1()
        {
            var rnd = new Random();
            InitializeComponent();

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);

            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересёкся с {obj}\n" + txtLog.Text;
            };

            player.OnMarkerOverlap += (m) =>
            {
                myObjects.Remove(m);
                marker = null;
            };

            player.OnMobOverlap += (mob) =>
            {
                myObjects.Remove(mob);
                myObjects.Add(Generate());
                scoreTxt.Text = $"Очки: {++scoreCount}";
            };

            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);

            myObjects.Add(marker);
            myObjects.Add(player);
            myObjects.Add(Generate());
            myObjects.Add(Generate());
        }

        private Mob Generate()
        {
            var mob =  new Mob
            {
                x = 100f + (float)(Mob.rnd.Next() % (pbMain.Width - 200)),
                y = 100f + (float)(Mob.rnd.Next() % (pbMain.Height - 200)),
                angle = 0
            };

            mob.OnTimeEnd += m =>
            {
                myObjects.Remove(m);
                myObjects.Add(Generate());
            };

            return mob;
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            updatePlayer();

            foreach (var obj in myObjects.ToList())
            { 
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                }
            }

            foreach (var obj in myObjects.ToList())
            {
                obj.Update();
            }

            foreach (var obj in myObjects.ToList())
            { 
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.x - player.x;
                float dy = marker.y - player.y;

                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.vx += dx * 0.5f;
                player.vy += dy * 0.5f;

                player.angle = 90 - MathF.Atan2(player.vx, player.vy) * 180 / MathF.PI;
            }

            player.vx -= player.vx * 0.1f;
            player.vy -= player.vy * 0.1f;

            player.x += player.vx;
            player.y += player.vy;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                myObjects.Add(marker);
            }

            marker.x = e.X;
            marker.y = e.Y;
        }
    }
}
