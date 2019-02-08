using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCG.TurboSprite.SpriteMover
{
    // Cause a Sprite to visit a defined set of destinations.
    public class Route
    {
        public event EventHandler<EventRouteFinished> Finished;
        public event EventHandler<SpriteEventMoved> SpriteMoved;

        private Sprite _sprite;
        private List<Destination> route = new List<Destination>();
        private int currentDestinationIndex = -1;

        public bool Repeat { get; set; } = false;

        public Route(Sprite sprite)
        {
            _sprite = sprite;
        }

        public void Add(Destination destination)
        {
            route.Add(destination);
        }

        public void Add(int x, int y, int speed)
        {
            Add(new Destination(new Point(x, y), speed));
        }

        public void Start()
        {
            if (route.Count == 0)
            {
                return;
            }
            TargetMover mover = new TargetMover();
            mover.SpriteMoved += (e, evt) => SpriteMoved?.Invoke(this, new SpriteEventMoved(_sprite));
            mover.SpriteReachedTarget += (e, evt) =>
            {
                currentDestinationIndex++;
                if (currentDestinationIndex >= route.Count)
                {
                    mover.Speed = 0;
                    if (!Repeat)
                    {
                        Finished?.Invoke(this, new EventRouteFinished());
                        return;
                    }
                    else
                    {
                        currentDestinationIndex = 0;
                    }
                }
                mover.Target = route[currentDestinationIndex].Target;
                mover.Speed = route[currentDestinationIndex].Speed;
            };
            currentDestinationIndex = 0;
            mover.Target = route[currentDestinationIndex].Target;
            mover.Speed = route[currentDestinationIndex].Speed;
            _sprite.Mover = mover;
        }
    }

    public class EventRouteFinished : EventArgs { }

    public class Destination
    {
        public Point Target { get; internal set; }

        public int Speed { get; internal set; }

        public Destination(Point target, int speed)
        {
            Target = target;
            Speed = speed;
        }
    }
}
