using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SCG.TurboSprite;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace CritterWorld
{
    class Critter
    {
        public const int maxThinkTimeMilliseconds = 1000;
        public const int maxThinkTimeOverrunViolations = 5;

        public int thinkTimeOverrunViolations = 0;
        public long thinkCount = 0;
        public long totalThinkTime = 0;

        private readonly bool showDestinationMarkers = false;

        private readonly SpriteEngine _spriteEngine;
        private readonly SpriteEngine _spriteEngineDebug;

        private PolygonSprite sprite;

        private static Random rnd = new Random(Guid.NewGuid().GetHashCode());

        private PolygonSprite destinationMarker = null;

        private void ClearDestinationMarker()
        {
            if (destinationMarker != null)
            {
                destinationMarker.Kill();
            }
        }

        public void ClearDestination()
        {
            AssignDestination((int)sprite.X, (int)sprite.Y);
        }

        private void CreateDestinationMarker(int destX, int destY)
        {
            ClearDestinationMarker();

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

        public void AssignDestination(int destX, int destY)
        {
            if (showDestinationMarkers)
            {
                CreateDestinationMarker(destX, destY);
            }
            TargetMover mover = (TargetMover)sprite.Mover;
            mover.Speed = rnd.Next(10) + 1;
            mover.Target = new Point(destX, destY);
            mover.StopAtTarget = true;
        }

        public void AssignRandomDestination()
        {
            int destX = rnd.Next(sprite.Surface.Width);
            int destY = rnd.Next(sprite.Surface.Height);
            AssignDestination(destX, destY);
        }

        public void Reverse()
        {
            // TODO - fix.
            ClearDestinationMarker();
            TargetMover mover = (TargetMover)sprite.Mover;
            mover.TargetFacingAngle = sprite.FacingAngle - 180;
            mover.StopAtTarget = false;
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

        public long TotalThinkTime
        {
            get
            {
                return totalThinkTime;
            }
        }

        public long ThinkCount
        {
            get
            {
                return thinkCount;
            }
        }

        public double AverageThinkTime
        {
            get
            {
                if (ThinkCount == 0)
                {
                    return double.NaN;
                }
                else
                {
                    return (double)TotalThinkTime / (double)ThinkCount;
                }
            }
        }

        protected internal void Think(Random random)
        {
            // Do things here.
            int rand = random.Next(0, 250);
            if (rand == 1)
            {
                Sprite shockwave = new ShockWaveSprite(5, 20, 10, Color.DarkBlue, Color.LightBlue);
                shockwave.Position = sprite.Position;
                shockwave.Mover = new SlaveMover(sprite);
                _spriteEngine.AddSprite(shockwave);
            }
        }

        private int moveCount = 0;

        public Critter(SpriteEngine spriteEngine, SpriteEngine spriteEngineDebug, int startX, int startY, int scale)
        {
            _spriteEngineDebug = spriteEngineDebug;
            _spriteEngine = spriteEngine;

            CritterBody body = new CritterBody();
            sprite = new PolygonSprite(body.GetBody(scale))
            {
                Data = this,
                LineWidth = 1,
                Color = Sprite.RandomColor(127),
                Position = new Point(startX, startY),
                FacingAngle = 90
            };

            TargetMover spriteMover = new TargetMover();
            spriteMover.SpriteReachedTarget += (sender, spriteEvent) => AssignRandomDestination();
            spriteMover.SpriteMoved += (sender, spriteEvent) =>
            {
                if (moveCount-- == 0)
                {
                    sprite.IncrementFrame();
                    moveCount = 5 - Math.Min(5, (int)spriteMover.Speed);
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
                spriteMover.TargetFacingAngle = (int)theta + 90;
            });

            Thread processThread = new Thread(() =>
            {
                Stopwatch stopwatch = new Stopwatch();
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                while (!sprite.Surface.IsDisposed && !sprite.Surface.Disposing && !sprite.Dead)
                {
                    if (sprite.Surface.Active)
                    {
                        try
                        {
                            stopwatch.Reset();
                            stopwatch.Start();
                            Think(rnd);
                            stopwatch.Stop();
                            long elapsed = stopwatch.ElapsedMilliseconds;
                            if (elapsed > 1000)
                            {
                                if (thinkTimeOverrunViolations >= maxThinkTimeOverrunViolations)
                                {
                                    Console.WriteLine("You were warned " + thinkTimeOverrunViolations + " times about thinking for too long. Now you may not think again.");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Warning #" + (++thinkTimeOverrunViolations) + " you have exceeded the maximum think time of " + maxThinkTimeMilliseconds + " by " + (elapsed - maxThinkTimeMilliseconds) + " milliseconds.");
                                }
                            }
                            totalThinkTime += elapsed;
                            thinkCount++;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Critter thinking crashed due to " + e);
                            break;
                        }
                    }
                    Thread.Sleep(5);
                }
            });
            processThread.Start();
            sprite.Surface.Disposed += (e, evt) => processThread.Abort();
            sprite.Died += (e, evt) => processThread.Abort();
        }
    }
}
