using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SCG.TurboSprite;
using System.Windows.Forms;
using System.Threading;

namespace CritterWorld
{
    class Critter
    {
        private readonly bool showDestinationMarkers = false;
        private const float scale = 1;

        private readonly SpriteEngine _spriteEngineDebug;

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
        
        PolygonSprite destinationMarker = null;

        private void CreateDestinationMarker(int destX, int destY)
        {
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

            _spriteEngineDebug.AddSprite(destinationMarker);
        }

        public void AssignRandomDestination()
        {
            Random rnd = Sprite.RND;

            int destX = rnd.Next(sprite.Surface.Width);
            int destY = rnd.Next(sprite.Surface.Height);

            if (showDestinationMarkers)
            {
                CreateDestinationMarker(destX, destY);
            }

            SpriteEngine spriteEngine = sprite.Engine;
            DestinationMover mover = (DestinationMover)sprite.Mover;
            mover.Speed = rnd.Next(10) + 1;
            mover.Destination = new Point(destX, destY);
            mover.StopAtDestination = true;
        }

        public Point Position
        {
            get
            {
                return sprite.Position;
            }
            set
            {
                sprite.Position = value;
            }
        }

        protected internal void Think()
        {
            // Do things here.
        }

        private int moveCount = 0;

        public Critter(SpriteEngine spriteEngine, SpriteEngine spriteEngineDebug, int startX, int startY)
        {
            _spriteEngineDebug = spriteEngineDebug;

            CritterBody body = new CritterBody();
            PointF[][] frames = new PointF[2][];
            frames[0] = Scale(body.GetBody1(), scale);
            frames[1] = Scale(body.GetBody2(), scale);
            sprite = new PolygonSprite(frames)
            {
                Data = this,
                LineWidth = 1,
                Color = Sprite.RandomColor(127),
                Position = new Point(startX, startY)
            };

            DestinationMover spriteMover = new DestinationMover();
            spriteMover.SpriteReachedDestination += (sender, e) => AssignRandomDestination();
            spriteMover.SpriteMoved += (sender, e) =>
            {
                if (moveCount-- == 0)
                {
                    sprite.IncrementFrame();
                    moveCount = Math.Max(0, 10 - (int)spriteMover.Speed);
                }
            };
            sprite.Mover = spriteMover;

            spriteEngine.AddSprite(sprite);

            sprite.addProcessHandler(sprite =>
            {
                if (spriteMover.SpeedX == 0 && spriteMover.SpeedY == 0)
                {
                    return;
                }
                double theta = Sprite.RadToDeg((float)Math.Atan2(spriteMover.SpeedY, spriteMover.SpeedX));
                sprite.FacingAngle = (int)theta + 90;
            });

            Thread processThread = new Thread(() =>
            {
                while (!sprite.Dead && !sprite.Surface.Disposing && !sprite.Surface.IsDisposed)
                {
                    if (sprite.Surface.Active)
                    {
                        Think();
                    }
                    Thread.Sleep(5);
                }
            });
            processThread.Start();
        }
    }
}
