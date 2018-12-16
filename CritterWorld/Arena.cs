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

        float[] Scale(float[] array, float scale)
        {
            float[] scaledArray = new float[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                scaledArray[i] = array[i] * scale;
            }
            return scaledArray;
        }

        public Arena()
        {
            float scale = 1;

            float[] body = {
                // abdomen
                -3,  -5,
                3,   -5,
                5,   -3,
                5,   3,
                3,   5,
                -3,  5,
                -5,  3,
                -5,  -3,
                -3,  -5,
                // head
                -2,  -5,
                -2,  -9,
                2,   -9,
                2,   -5,
                -2,  -5,
                // left eye
                -2,  -9,
                -1,  -9,
                -1,  -7,
                -2,  -7,
                -2,  -5,
                // right eye
                2,   -5,
                2,   -9,
                1,   -9,
                1,   -7,
                2,   -7,
                2,   -5,
                // Antennae
                2,   -5,
                -3,  -12,
                -5,  -12,
                -1,  -9,
                1,   -9,
                5,   -12,
                3,   -12,
                -2,  -5,
                // Legs
                -2,  -5,
                -10, 5,
                -5,  0,
                -11, -1,
                -5,  -1,
                -10, -7,
                -5,  0,
                5,   0,
                11,  -1,
                5,   -1,
                10,  -7,
                5,   0,
                10,  5,
                2,   -5,
            };

            InitializeComponent();

            spriteSurface1.SpriteCollision += new System.EventHandler<SCG.TurboSprite.SpriteCollisionEventArgs>(this.surface_SpriteCollision);

            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 20; i++)
            {
                Sprite s;

                if (i % 2 == 0)
                    s = new BitmapSprite(new Bitmap("Images/Robo0_01.png"));
                else
                    s = new PolygonSprite(Scale(body, scale));
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
