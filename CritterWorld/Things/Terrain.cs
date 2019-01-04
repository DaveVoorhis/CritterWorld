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
    public class Terrain : PolygonSprite
    {
        public Terrain(int X1, int X2, int Y1, int Y2) :
            base(new PointF[]
            {
                new PointF(0, 0),
                new PointF(X2 - X1, 0),
                new PointF(X2 - X1, Y2 - Y1),
                new PointF(0, Y2 - Y1)
            })
        {
            Position = new Point(X1, Y1);
            Color = Color.DarkGreen;
        }

        private Timer timer = null;

        public void Nudge()
        {
            if (timer != null)
            {
                return;
            }
            timer = new Timer((e) =>
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                FacingAngle = rnd.Next(5, 15) * ((rnd.Next(0, 2) == 1) ? -1 : 1);
                Thread.Sleep(200);
                FacingAngle = 0;
                timer = null;
            }, null, 0, Timeout.Infinite);
        }
    }

}
