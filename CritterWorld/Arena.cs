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
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public SpriteSurface Surface
        {
            get { return spriteSurfaceMain; }
        }

        public void AddGifts(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Gift gift;
                do
                {
                    int x = rnd.Next(50, Surface.Width - 50);
                    int y = rnd.Next(50, Surface.Height - 50);
                    gift = new Gift(x, y);
                }
                while (WillCollide(gift));
                AddSprite(gift);
            }
        }

        public void AddBombs(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Bomb bomb;
                do
                {
                    int x = rnd.Next(50, Surface.Width - 50);
                    int y = rnd.Next(50, Surface.Height - 50);
                    bomb = new Bomb(x, y);
                }
                while (WillCollide(bomb));
                AddSprite(bomb);
                bomb.LightFuse();
            }
        }

        public void AddFoods(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Food food;
                do
                {
                    int x = rnd.Next(50, Surface.Width - 50);
                    int y = rnd.Next(50, Surface.Height - 50);
                    food = new Food(x, y);
                }
                while (WillCollide(food));
                AddSprite(food);
            }
        }

        private void Collide(Critter critter1, Critter critter2)
        {
            if (critter1.Mover == null || critter2.Mover == null)
            {
                return;
            }

            Sound.PlayBump();

            critter1.Bounceback();
            critter2.Bounceback();

            critter1.AssignRandomDestination();
            critter2.AssignRandomDestination();

            Sprite fight = new ParticleExplosionSprite(10, Color.DarkRed, Color.Red, 1, 5, 10)
            {
                Position = new Point((critter1.Position.X + critter2.Position.X) / 2, (critter1.Position.Y + critter2.Position.Y) / 2)
            };
            AddSprite(fight);
        }

        private void Collide(Critter critter, Terrain terrain)
        {
            if (critter.Mover == null)
            {
                return;
            }

            Sound.PlayZap();

            critter.Bounceback();
            critter.AssignRandomDestination();

            terrain.Nudge();

            Sprite bump = new ParticleExplosionSprite(10, Color.LightBlue, Color.White, 1, 2, 5)
            {
                Position = new Point((critter.Position.X + terrain.Position.X) / 2, (critter.Position.Y + terrain.Position.Y) / 2)
            };
            AddSprite(bump);
        }

        private void Collide(Critter critter, Bomb bomb)
        {
            Sound.PlayBoom();

            Sprite spew = new StarFieldSprite(100, 5, 5, 10)
            {
                Position = bomb.Position
            };
            AddSprite(spew);
            Sprite explosion = new ParticleFountainSprite(250, Color.DarkGray, Color.White, 1, 3, 20)
            {
                Position = bomb.Position
            };
            AddSprite(explosion);
            critter.Mover = null;
            System.Timers.Timer explosionTimer = new System.Timers.Timer
            {
                Interval = 250,
                AutoReset = false
            };
            explosionTimer.Elapsed += (sender, e) =>
            {
                explosion.Kill();
                spew.Kill();
                critter.StopAndSmoke(Color.Black, Color.Brown);
            };
            explosionTimer.Start();
            bomb.Kill();

            AddBombs(1);
        }

        private void Collide(Critter critter, Food food)
        {
            Sound.PlayGulp();
            food.Kill();
            AddFoods(1);
        }

        private void Collide(Critter critter, Gift gift)
        {
            Sound.PlayYay();
            gift.Kill();
            AddGifts(1);
        }

        private void Collide(object sender, SpriteCollisionEventArgs collision)
        {
            Sprite sprite1 = collision.Sprite1;
            Sprite sprite2 = collision.Sprite2;
            if (sprite1 is Critter && sprite2 is Critter)
            {
                Collide((Critter)sprite1, (Critter)sprite2);
            }
            else if (sprite1 is Critter && sprite2 is Terrain)
            {
                Collide((Critter)sprite1, (Terrain)sprite2);
            }
            else if (sprite1 is Terrain && sprite2 is Critter)
            {
                Collide((Critter)sprite2, (Terrain)sprite1);
            }
            else if (sprite1 is Critter && sprite2 is Bomb)
            {
                Collide((Critter)sprite1, (Bomb)sprite2);
            }
            else if (sprite1 is Bomb && sprite2 is Critter)
            {
                Collide((Critter)sprite2, (Bomb)sprite1);
            }
            else if (sprite1 is Critter && sprite2 is Food)
            {
                Collide((Critter)sprite1, (Food)sprite2);
            }
            else if (sprite1 is Food && sprite2 is Critter)
            {
                Collide((Critter)sprite2, (Food)sprite1);
            }
            else if (sprite1 is Critter && sprite2 is Gift)
            {
                Collide((Critter)sprite1, (Gift)sprite2);
            }
            else if (sprite1 is Gift && sprite2 is Critter)
            {
                Collide((Critter)sprite2, (Gift)sprite1);
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
            const int critterCount = 25;
            const int scale = 1;

            InitializeComponent();

            spriteSurfaceMain.SpriteCollision += (sender, collisionEvent) => Collide(sender, collisionEvent);

            Level testLevel = new Level(this, (Bitmap)Image.FromFile("Images/TerrainMasks/Background05.png"));

            int startX = 30;
            int startY = 0;
            for (int i = 0; i < critterCount; i++)
            {
                Critter critter = null;
                do
                {
                    startY += 30;
                    if (startY >= spriteSurfaceMain.Height - 30)
                    {
                        startY = 30;
                        startX += 100;
                    }
                    critter = new Critter(startX, startY, scale);
                }
                while (WillCollide(critter));
                spriteEngineMain.AddSprite(critter);
            }

            System.Timers.Timer critterStartupTimer = new System.Timers.Timer();
            critterStartupTimer.Interval = 3000;
            critterStartupTimer.AutoReset = false;
            critterStartupTimer.Elapsed += (sender, e) =>
            {
                Sprite[] sprites = spriteEngineMain.Sprites.ToArray();
                foreach (Sprite sprite in sprites)
                {
                    if (sprite is Critter)
                    {
                        ((Critter)sprite).Startup();
                    }
                }
            };
            critterStartupTimer.Start();

            spriteSurfaceMain.Active = true;
            spriteSurfaceMain.WraparoundEdges = true;

            System.Timers.Timer fpsDisplayTimer = new System.Timers.Timer();
            fpsDisplayTimer.Interval = 250;
            fpsDisplayTimer.Elapsed += (sender, e) => labelFPS.Invoke(new Action(() => labelFPS.Text = spriteSurfaceMain.ActualFPS + " fps" + TickShow()));
            fpsDisplayTimer.Start();
        }
    }
}
