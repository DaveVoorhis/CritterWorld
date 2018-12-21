using System;
using System.Collections.Generic;
using System.Drawing;
using SCG.TurboSprite;

namespace CritterWorld
{
    class CritterBody
    {
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

        private PointF[] body1;
        private PointF[] body2;

        public CritterBody()
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
            foreach (Point point in rightBody)
            {
                outvector.Add(point);
            }
            rightBody.Reverse();
            foreach (Point point in rightBody)
            {
                outvector.Add(new PointF(-point.X, point.Y));
            }

            body1 = outvector.ToArray();

            rightBody.Reverse();

            for (int i = 0; i < 3; i++) 
            {
                rightBody[legIndex[i] + 1] = new Point(rightBody[legIndex[i] + 1].X, rightBody[legIndex[i] + 1].Y + 2);
                rightBody[legIndex[i] + 2] = new Point(rightBody[legIndex[i] + 2].X, rightBody[legIndex[i] + 2].Y + 5);
                rightBody[legIndex[i] + 3] = new Point(rightBody[legIndex[i] + 3].X, rightBody[legIndex[i] + 3].Y + 2);
            }

            outvector.Clear();
            foreach (Point point in rightBody)
            {
                outvector.Add(point);
            }
            rightBody.Reverse();
            foreach (Point point in rightBody)
            {
                outvector.Add(new PointF(-point.X, point.Y));
            }

            body2 = outvector.ToArray();
        }

        public PointF[] GetBody1()
        {
            return body1;
        }

        public PointF[] GetBody2()
        {
            return body2;
        }
    }
}
