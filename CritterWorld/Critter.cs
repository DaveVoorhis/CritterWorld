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
        private PolygonSpriteAnimated sprite;

        PointF[] Scale(PointF[] array, float scale)
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

        public Sprite GetSprite()
        {
            return sprite;
        }

        public Critter(SpriteSurface spriteSurface, SpriteEngineDestination spriteEngine)
        {
            CritterBody body = new CritterBody();
            PointF[][] frames = new PointF[2][];
            frames[0] = Scale(body.GetBody1(), scale);
            frames[1] = Scale(body.GetBody2(), scale);
            sprite = new PolygonSpriteAnimated(frames);
            sprite.LineWidth = 2;

            spriteEngine.AddSprite(sprite);

            destinationMover = spriteEngine.GetMover(sprite);

            sprite.addProcessHandler(sprite =>
            {
                if (destinationMover.SpeedX == 0 && destinationMover.SpeedY == 0)
                {
                    return;
                }
                double theta = Math.Atan2(destinationMover.SpeedY, destinationMover.SpeedX) * 180 / Math.PI;
                sprite.FacingAngle = (int)theta + 90;
            });
        }
    }
}
