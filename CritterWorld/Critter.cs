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

        private DestinationMover destinationMover;
        private PolygonSprite sprite;

        public static PointF[] Scale(PointF[] array, float scale)
        {
            PointF[] scaledArray = new PointF[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                scaledArray[i] = new PointF(array[i].X * scale, array[i].Y * scale);
            }
            return scaledArray;
        }

        public DestinationMover GetMover()
        {
            return destinationMover;
        }

        public PolygonSprite GetSprite()
        {
            return sprite;
        }

        PolygonSprite destinationMarker = null;

        public void AssignRandomDestination(SpriteEngine spriteEngineDebug)
        {
            Random rnd = Sprite.RND;

            int destX = rnd.Next(sprite.Surface.Width);
            int destY = rnd.Next(sprite.Surface.Height);

            if (destinationMarker != null)
            {
                destinationMarker.Kill();
            }

            PointF[] markerPoly = new PointF[]
            {
                new PointF(-2, -2),
                new PointF(-2, 2),
                new PointF(2, 2),
                new PointF(2, -2)
            };
            destinationMarker = new PolygonSprite(markerPoly);
            destinationMarker.Position = new Point(destX, destY);
            destinationMarker.Color = Color.Red;

            spriteEngineDebug.AddSprite(destinationMarker);

            SpriteEngineDestination spriteEngine = (SpriteEngineDestination)sprite.Engine;
            DestinationMover mover = spriteEngine.GetMover(sprite);
            mover.Speed = rnd.Next(10) + 1;
            mover.Destination = new Point(destX, destY);
            mover.StopAtDestination = true;
        }

        public Critter(SpriteEngineDestination spriteEngine)
        {
            CritterBody body = new CritterBody();
            PointF[][] frames = new PointF[2][];
            frames[0] = Scale(body.GetBody1(), scale);
            frames[1] = Scale(body.GetBody2(), scale);
            sprite = new PolygonSprite(frames);
            sprite.Data = this;
            sprite.LineWidth = 1;

            spriteEngine.AddSprite(sprite);

            destinationMover = spriteEngine.GetMover(sprite);

            sprite.addProcessHandler(sprite =>
            {
                if (destinationMover.SpeedX == 0 && destinationMover.SpeedY == 0)
                {
                    return;
                }
                double theta = Sprite.RadToDeg((float)Math.Atan2(destinationMover.SpeedY, destinationMover.SpeedX));
                sprite.FacingAngle = (int)theta + 90;
            });
        }
    }
}
