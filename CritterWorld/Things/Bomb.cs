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
    public class Bomb : BitmapSprite, ISensable, IVisible
    {
        private Sprite spark;

        public Bomb(Point position) : base((Bitmap)Image.FromFile("Resources/Images/bomb.png"))
        {
            Position = position;
        }

        public void LightFuse()
        {
            if (spark != null)
            {
                return;
            }
            spark = new ParticleFountainSprite(10, Color.LightGray, Color.White, 1, 1, 3);
            spark.Position = new Point((int)(X - WidthHalf + 1), (int)(Y - HeightHalf + 1));
            Engine?.AddSprite(spark);
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

        public override void Kill()
        {
            ExtinguishFuse();
            base.Kill();
        }

        public string SensorSignature
        {
            get { return "Bomb" + ":" + Position; }
        }
    }

}
