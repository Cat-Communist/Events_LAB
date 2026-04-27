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

            foreach (var obj in myObjects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    txtLog.Text = $"[{DateTime.Now:HH::mm::ss::ff}] Игрок пересёкся с {obj}" + txtLog.Text;

                    if (obj == marker)
                    {
                        myObjects.Remove(marker);
                        marker = null;
                    }
                }

                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (marker != null)
            {
                float dx = marker.x - player.x;
                float dy = marker.y - player.y;

                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.x += dx * 2;
                player.y += dy * 2;
            }

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
