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
            critter1.Bounceback();
            critter2.Bounceback();

            critter1.AssignRandomDestination();
            critter2.AssignRandomDestination();

            Sprite fight = new ParticleExplosionSprite(10, Color.DarkRed, Color.Red, 1, 5, 10)
            {
                Position = new Point((critter1.Position.X + critter2.Position.X) / 2, (critter1.Position.Y + critter2.Position.Y) / 2)
            };
            spriteEngineDebug.AddSprite(fight);
        }

        private void Collide(Critter critter, Terrain terrain)
        {
            critter.Bounceback();
            critter.AssignRandomDestination();

            terrain.Nudge();

            Sprite bump = new ParticleExplosionSprite(10, Color.Gray, Color.LightGray, 1, 2, 5)
            {
                Position = new Point((critter.Position.X + terrain.Position.X) / 2, (critter.Position.Y + terrain.Position.Y) / 2)
            };
            spriteEngineDebug.AddSprite(bump);
        }

        private void Collide(object sender, SpriteCollisionEventArgs collision)
        {
            if (collision.Sprite1.Data is Critter && collision.Sprite2.Data is Critter)
            {
                Collide((Critter)collision.Sprite1.Data, (Critter)collision.Sprite2.Data);
            }
            else if (collision.Sprite1.Data is Critter && collision.Sprite2 is Terrain)
            {
                Collide((Critter)collision.Sprite1.Data, (Terrain)collision.Sprite2);
            }
            else if (collision.Sprite1 is Terrain && collision.Sprite2.Data is Critter)
            {
                Collide((Critter)collision.Sprite2.Data, (Terrain)collision.Sprite1);
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

        public bool WillCollide(Sprite sprite)
        {
            return spriteEngineMain.WillCollide(sprite);
        }

        public void AddSprite(Sprite sprite)
        {
            spriteEngineMain.AddSprite(sprite);
        }

        public Arena()
        {
            const int critterCount = 5;
            const int scale = 1;

            InitializeComponent();

            spriteEngineDebug = new SpriteEngine(components)
            {
                Surface = spriteSurfaceMain,
                DetectCollisionSelf = false,
                DetectCollisionTag = 50
            };

            spriteSurfaceMain.SpriteCollision += (sender, collisionEvent) => Collide(sender, collisionEvent);

            Level testLevel = new Level(this, (Bitmap)Image.FromFile("Images/TerrainMasks/Background05.png"));

            int startX = 30;
            int startY = 30;
            for (int i = 0; i < critterCount; i++)
            {
                Critter critter = null;
                do
                {
                    try
                    {
                        critter = new Critter(spriteEngineMain, spriteEngineDebug, startX, startY, scale);
                        critter.AssignRandomDestination();
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("Note: Critter relocated to avoid terrain.");
                    }
                    startY += 30;
                    if (startY >= spriteSurfaceMain.Height - 30)
                    {
                        startY = 30;
                        startX += 100;
                    }
                } while (critter == null);
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
