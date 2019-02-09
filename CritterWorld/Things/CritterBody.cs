using System;
using System.Collections.Generic;
using System.Drawing;
using SCG.TurboSprite;

namespace CritterWorld
{
    class CritterBody
    {
        private static Random rnd = new Random(Guid.NewGuid().GetHashCode());

        /* Return a random value near the given value with a specified range of values. */
        public static int Fuzzy(int nearThis, int range)
        {
            return nearThis + rnd.Next(-range, range);
        }

        /* Return a random value near the given value. */
        public static int Fuzzy(int nearThis)
        {
            return Fuzzy(nearThis, 2);
        }

        /* Return a random Point near the given x and y coordinates. */
        public static Point FuzzyPoint(int x, int y)
        {
            return new Point(Fuzzy(x), Fuzzy(y));
        }

        /* Return a model, scaled. */
        public static PointF[] Scale(PointF[] array, float scale)
        {
            PointF[] scaledArray = new PointF[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                scaledArray[i] = new PointF(array[i].X * scale, array[i].Y * scale);
            }
            return scaledArray;
        }

        private PointF[] body1;
        private PointF[] body2;

        public CritterBody()
        {
            Point[] antenna = new Point[3];
            antenna[0] = new Point(Fuzzy(4, 4), Fuzzy(-10, 4));
            antenna[1] = new Point(Fuzzy(8, 4), Fuzzy(-12, 4));
            antenna[2] = new Point(Fuzzy(12, 4), Fuzzy(-8, 4));

            Point[,] leg = new Point[3, 3];
            leg[0, 0] = FuzzyPoint(4, -5);
            leg[0, 1] = FuzzyPoint(7, -5);
            leg[0, 2] = FuzzyPoint(10, -5);

            leg[1, 0] = FuzzyPoint(4, 0);
            leg[1, 1] = FuzzyPoint(7, 0);
            leg[1, 2] = FuzzyPoint(10, 0);

            leg[2, 0] = FuzzyPoint(4, 5);
            leg[2, 1] = FuzzyPoint(7, 5);
            leg[2, 2] = FuzzyPoint(10, 5);

            List<Point> rightBody = new List<Point>();

            int[] legIndex = new int[3];

            rightBody.Add(new Point(0, -8));
            rightBody.Add(FuzzyPoint(2, -6));
            rightBody.Add(antenna[0]);
            rightBody.Add(antenna[1]);
            rightBody.Add(antenna[2]);
            rightBody.Add(antenna[1]);
            rightBody.Add(antenna[0]);
            rightBody.Add(FuzzyPoint(2, -5));
            rightBody.Add(FuzzyPoint(2, -4));
            legIndex[0] = rightBody.Count;
            rightBody.Add(leg[0, 0]);
            rightBody.Add(leg[0, 1]);
            rightBody.Add(leg[0, 2]);
            rightBody.Add(leg[0, 1]);
            rightBody.Add(leg[0, 0]);
            rightBody.Add(FuzzyPoint(3, -3));
            legIndex[1] = rightBody.Count;
            rightBody.Add(leg[1, 0]);
            rightBody.Add(leg[1, 1]);
            rightBody.Add(leg[1, 2]);
            rightBody.Add(leg[1, 1]);
            rightBody.Add(leg[1, 0]);
            rightBody.Add(FuzzyPoint(3, 0));
            legIndex[2] = rightBody.Count;
            rightBody.Add(leg[2, 0]);
            rightBody.Add(leg[2, 1]);
            rightBody.Add(leg[2, 2]);
            rightBody.Add(leg[2, 1]);
            rightBody.Add(leg[2, 0]);
            rightBody.Add(FuzzyPoint(2, 3));
            rightBody.Add(FuzzyPoint(3, 5));
            rightBody.Add(new Point(0, 7));

            List<PointF> outvector = new List<PointF>();
            rightBody.ForEach(point => outvector.Add(point));
            rightBody.Reverse();
            rightBody.ForEach(point => outvector.Add(new PointF(-point.X, point.Y)));

            body1 = outvector.ToArray();

            rightBody.Reverse();

            for (int i = 0; i < 3; i++)
            {
                rightBody[legIndex[i] + 1] = new Point(rightBody[legIndex[i] + 1].X, rightBody[legIndex[i] + 1].Y + 2);
                rightBody[legIndex[i] + 2] = new Point(rightBody[legIndex[i] + 2].X, rightBody[legIndex[i] + 2].Y + 5);
                rightBody[legIndex[i] + 3] = new Point(rightBody[legIndex[i] + 3].X, rightBody[legIndex[i] + 3].Y + 2);
            }

            outvector.Clear();
            rightBody.ForEach(point => outvector.Add(point));
            rightBody.Reverse();
            rightBody.ForEach(point => outvector.Add(new PointF(-point.X, point.Y)));

            body2 = outvector.ToArray();

            // Cross-leg tripod movement is more realistically bug-like.
            for (int i = 0; i < 5; i++)
            {            
                PointF tmp = body1[legIndex[0] + i - 1];
                body1[legIndex[0] + i - 1] = body2[legIndex[0] + i - 1];
                body2[legIndex[0] + i - 1] = tmp;
                tmp = body1[legIndex[2] + i - 1];
                body1[legIndex[2] + i - 1] = body2[legIndex[2] + i - 1];
                body2[legIndex[2] + i - 1] = tmp;
                tmp = body1[body1.Length - legIndex[1] - i];
                body1[body1.Length - legIndex[1] - i] = body2[body2.Length - legIndex[1] - i];
                body2[body2.Length - legIndex[1] - i] = tmp;
            }
        }

        public PointF[][] GetBody(float scale = 1)
        {
            PointF[][] frames = new PointF[2][];
            frames[0] = Scale(body1, scale);
            frames[1] = Scale(body2, scale);
            return frames;
        }
    }
}
