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
    public class Bomb : BitmapSprite
    {
        private Sprite spark;

        public Bomb(int x, int y) : base((Bitmap)Image.FromFile("Images/bomb.png"))
        {
            Position = new Point(x, y);
        }

        public void LightFuse()
        {
            if (spark != null)
            {
                return;
            }
            spark = new ParticleFountainSprite(10, Color.LightGray, Color.White, 1, 1, 3);
            spark.Position = new Point((int)(X - WidthHalf + 1), (int)(Y - HeightHalf + 1));
            Engine.AddSprite(spark);
        }

        public void ExtinguishFuse()
        {
            if (spark == null)
            {
                return;
            }
            spark.Kill();
            spark = null;
        }
    }

}
