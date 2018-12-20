using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SCG.TurboSprite;
using System.Windows.Forms;

namespace CritterWorld
{
    class Critter
    {
        private const float scale = 1;

        PointF[] Scale(PointF[] array, float scale)
        {
            PointF[] scaledArray = new PointF[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                scaledArray[i] = new PointF(array[i].X * scale, array[i].Y * scale);
            }
            return scaledArray;
        }

        private bool alternateBody = false;

        public Critter(SpriteSurface spriteSurface, SpriteEngineDestination spriteEngine)
        {
            PolygonSprite sprite;
            Random rnd = Sprite.RND;

            CritterBody body = new CritterBody();
            sprite = new PolygonSprite(Scale(body.GetBody1(), scale));

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += (obj, eventArgs) =>
            {
                if (alternateBody)
                {
                    sprite.Points = body.GetBody2();
                }
                else
                {
                    sprite.Points = body.GetBody1();
                }
                alternateBody = !alternateBody;
            };
            timer.Start();

            int startX = rnd.Next(spriteSurface.Width);
            int startY = rnd.Next(spriteSurface.Height);

            sprite.Position = new Point(startX, startY);

            spriteEngine.AddSprite(sprite);

            DestinationMover dm = spriteEngine.GetMover(sprite);
            dm.Speed = rnd.Next(10) + 1;

            int destX = rnd.Next(spriteSurface.Width);
            int destY = rnd.Next(spriteSurface.Height);
            dm.Destination = new Point(destX, destY);
            dm.StopAtDestination = false;

            double theta = Math.Atan2(destY - startY, destX - startX) * 180 / Math.PI;
            sprite.FacingAngle = (int)theta + 90;
        }
    }
}
