using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCG.TurboSprite;

namespace CritterWorld
{
    public partial class Arena : Form
    {
        private void surface_SpriteCollision(object sender, SpriteCollisionEventArgs e)
        {
            DestinationMover dm1 = spriteEngine1.GetMover(e.Sprite1);
            DestinationMover dm2 = spriteEngine1.GetMover(e.Sprite2);
            float sx1 = dm1.SpeedX;
            float sy1 = dm1.SpeedY;
            float sx2 = dm2.SpeedX;
            float sy2 = dm2.SpeedY;
            dm1.SpeedX = sx2;
            dm1.SpeedY = sy2;
            dm2.SpeedX = sx1;
            dm2.SpeedY = sy1;

            double theta1 = Math.Atan2(dm1.SpeedY, dm1.SpeedX) * 180 / Math.PI;
            e.Sprite1.FacingAngle = (int)theta1 + 90;

            double theta2 = Math.Atan2(dm2.SpeedY, dm2.SpeedX) * 180 / Math.PI;
            e.Sprite2.FacingAngle = (int)theta2 + 90;
        }

        float[] Scale(float[] array, float scale)
        {
            float[] scaledArray = new float[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                scaledArray[i] = array[i] * scale;
            }
            return scaledArray;
        }

        /* Return a random value near the given value, but greater than or equal to minimum. */
        public static int Fuzzy(int nearThis, int minimum)
        {
            return Math.Max(minimum, nearThis + Sprite.RND.Next(-2, 2));
        }

        /* Return a random value near the given value. */
        public static int Fuzzy(int nearThis)
        {
            return nearThis + Sprite.RND.Next(-2, 2);
        }

        /* Return a random Point near the given x and y coordinates. */
        public static Point FuzzyPoint(int x, int y)
        {
            return new Point(Fuzzy(x), Fuzzy(y));
        }

        public float[] MakeCritterBody()
        {
            Point[] antenna = new Point[3];
            antenna[0] = FuzzyPoint(4, -10);
            antenna[1] = FuzzyPoint(8, -12);
            antenna[2] = FuzzyPoint(12, -8);

            Point[,] leg = new Point[3, 3];
            leg[0, 0] = FuzzyPoint(4, -3);
            leg[0, 1] = FuzzyPoint(7, -3);
            leg[0, 2] = FuzzyPoint(10, -3);

            leg[1, 0] = FuzzyPoint(4, 0);
            leg[1, 1] = FuzzyPoint(7, 0);
            leg[1, 2] = FuzzyPoint(10, 0);

            leg[2, 0] = FuzzyPoint(4, 3);
            leg[2, 1] = FuzzyPoint(7, 3);
            leg[2, 2] = FuzzyPoint(10, 3);

            List<Point> rightBody = new List<Point>();

            rightBody.Add(new Point(0, -8));
            rightBody.Add(FuzzyPoint(2, -6));
            rightBody.Add(antenna[0]);
            rightBody.Add(antenna[1]);
            rightBody.Add(antenna[2]);
            rightBody.Add(antenna[1]);
            rightBody.Add(antenna[0]);
            rightBody.Add(FuzzyPoint(2, -5));
            rightBody.Add(FuzzyPoint(2, -4));
            rightBody.Add(leg[0, 0]);
            rightBody.Add(leg[0, 1]);
            rightBody.Add(leg[0, 2]);
            rightBody.Add(leg[0, 1]);
            rightBody.Add(leg[0, 0]);
            rightBody.Add(FuzzyPoint(3, -3));
            rightBody.Add(leg[1, 0]);
            rightBody.Add(leg[1, 1]);
            rightBody.Add(leg[1, 2]);
            rightBody.Add(leg[1, 1]);
            rightBody.Add(leg[1, 0]);
            rightBody.Add(FuzzyPoint(3, 0));
            rightBody.Add(leg[2, 0]);
            rightBody.Add(leg[2, 1]);
            rightBody.Add(leg[2, 2]);
            rightBody.Add(leg[2, 1]);
            rightBody.Add(leg[2, 0]);
            rightBody.Add(FuzzyPoint(2, 3));
            rightBody.Add(FuzzyPoint(3, 5));
            rightBody.Add(new Point(0, 7));

            List<float> outvector = new List<float>();
            foreach (Point point in rightBody)
            {
                outvector.Add(point.X);
                outvector.Add(point.Y);
            }
            rightBody.Reverse();
            foreach (Point point in rightBody)
            {
                outvector.Add(-point.X);
                outvector.Add(point.Y);
            }

            return outvector.ToArray();
        }

        public Arena()
        {
            float scale = 1;
            float critterCount = 10;

            InitializeComponent();

            spriteSurface1.SpriteCollision += new System.EventHandler<SCG.TurboSprite.SpriteCollisionEventArgs>(this.surface_SpriteCollision);

            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < critterCount; i++)
            {
                Sprite s;

                s = new PolygonSprite(Scale(MakeCritterBody(), scale));

                int startX = rnd.Next(spriteSurface1.Width);
                int startY = rnd.Next(spriteSurface1.Height);

                s.Position = new Point(startX, startY);

                spriteEngine1.AddSprite(s);

                DestinationMover dm = spriteEngine1.GetMover(s);
                dm.Speed = rnd.Next(10) + 1;

                int destX = rnd.Next(spriteSurface1.Width);
                int destY = rnd.Next(spriteSurface1.Height);
                dm.Destination = new Point(destX, destY);
                dm.StopAtDestination = false;

                double theta = Math.Atan2(destY - startY, destX - startX) * 180 / Math.PI;
                s.FacingAngle = (int)theta + 90;
            }

            spriteSurface1.Active = true;
            spriteSurface1.WraparoundEdges = true;
        }
    }
}
