using SCG.TurboSprite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CritterWorld
{
    public class Bomb
    {
        public Bomb(Arena arena)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            Sprite bomb = new BitmapSprite((Bitmap)Image.FromFile("Images/bomb.png"));
            int x;
            int y;
            do
            {
                x = rnd.Next(50, arena.Surface.Width - 50);
                y = rnd.Next(50, arena.Surface.Height - 50);
                bomb.Position = new Point(x, y);
            }
            while (arena.WillCollide(bomb));
            arena.AddSprite(bomb);

            Sprite spark = new ParticleFountainSprite(20, Color.LightGray, Color.White, 1, 2, 5);
            spark.Position = new Point(x - bomb.WidthHalf + 1, y - bomb.HeightHalf + 1);
            arena.AddSprite(spark);
        }
    }

}
