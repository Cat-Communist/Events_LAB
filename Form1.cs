using MiniGame.Objects;

namespace MiniGame
{
    public partial class Form1 : Form
    {
        Player player;
        Marker marker;
        List<BaseObject> myObjects = new();
        public Form1()
        {
            InitializeComponent();

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);

            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересёкся с {obj}\n" + txtLog.Text;
            };

            player.onMarkerOverlap += (m) =>
            {
                myObjects.Remove(m);
                marker = null;
            };

            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);

            myObjects.Add(marker);
            myObjects.Add(player);
            myObjects.Add(new MyRectangle(50, 50, 0));
            myObjects.Add(new MyRectangle(100, 100, 45));
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
                    obj.Overlap(player);
                }
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
