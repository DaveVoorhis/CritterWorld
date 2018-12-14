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
            // e.Sprite1.Kill();
            // e.Sprite2.Kill();
        }

        public Arena()
        {
            InitializeComponent();

            spriteSurface1.SpriteCollision += new System.EventHandler<SCG.TurboSprite.SpriteCollisionEventArgs>(this.surface_SpriteCollision);

            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 1000; i++)
            {
                PolygonSprite s = new PolygonSprite(0, -10, 5, 10, 0, 5, -5, 10);
                if (rnd.Next(100) > 50)
                {
                    if (rnd.Next(100) > 50)
                        s.Spin = SpinType.Clockwise;
                    else
                        s.Spin = SpinType.CounterClockwise;
                    s.SpinSpeed = rnd.Next(20) + 1;
                }
                s.Position = new Point(rnd.Next(spriteSurface1.Width), rnd.Next(spriteSurface1.Height));
                spriteEngine1.AddSprite(s);
                DestinationMover dm = spriteEngine1.GetMover(s);
                dm.Speed = rnd.Next(10) + 1;
                dm.Destination = new Point(rnd.Next(spriteSurface1.Width), rnd.Next(spriteSurface1.Height));
                dm.StopAtDestination = false;
            }

            spriteSurface1.Active = true;
            spriteSurface1.WraparoundEdges = true;

        }
    }
}
