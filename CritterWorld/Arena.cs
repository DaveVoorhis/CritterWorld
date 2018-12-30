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
using System.Threading;
using Timer = System.Windows.Forms.Timer;

namespace CritterWorld
{
    public partial class Arena : Form
    {
        private SpriteEngine spriteEngineDebug;

        private void Collide(Critter critter1, Critter critter2)
        {
            critter1.AssignRandomDestination();
            critter2.AssignRandomDestination();

            Sprite fight = new ParticleExplosionSprite(10, Color.DarkRed, Color.Red, 1, 5, 10)
            {
                Position = new Point((critter1.Position.X + critter2.Position.X) / 2, (critter1.Position.Y + critter2.Position.Y) / 2)
            };
            spriteEngineDebug.AddSprite(fight);
        }

        private void Collide(object sender, SpriteCollisionEventArgs collision)
        {
            if (collision.Sprite1.Data is Critter && collision.Sprite2.Data is Critter)
            {
                Collide((Critter)collision.Sprite1.Data, (Critter)collision.Sprite2.Data);
            }
        }

        int tickCount = 0;

        private String TickShow()
        {
            if (tickCount++ > 5)
            {
                tickCount = 0;
            }
            return new string('.', tickCount);
        }

        public Arena()
        {
            int critterCount = 25;

            InitializeComponent();

            spriteEngineDebug = new SpriteEngine(components)
            {
                Surface = spriteSurfaceMain,
                DetectCollisionSelf = false,
                DetectCollisionTag = 50
            };

            spriteSurfaceMain.SpriteCollision += (sender, collisionEvent) => Collide(sender, collisionEvent);

            int startX = 30;
            int startY = 30;

            for (int i = 0; i < critterCount; i++)
            {
                Critter critter = new Critter(spriteEngineMain, spriteEngineDebug, startX, startY);
                critter.AssignRandomDestination();

                startY += 30;
                if (startY >= spriteSurfaceMain.Height - 30) 
                {
                    startY = 30;
                    startX += 100;
                }
            }

            spriteSurfaceMain.Active = true;
            spriteSurfaceMain.WraparoundEdges = true;

            Timer fpsDisplayTimer = new Timer();
            fpsDisplayTimer.Interval = 500;
            fpsDisplayTimer.Tick += (sender, e) => labelFPS.Text = spriteSurfaceMain.ActualFPS + " fps" + TickShow();
            fpsDisplayTimer.Start();
        }
    }
}
