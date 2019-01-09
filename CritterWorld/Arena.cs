using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCG.TurboSprite;
using System.Threading;

namespace CritterWorld
{
    public partial class Arena : UserControl
    {
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());

        private int critterStartX;
        private int critterStartY;

        private const int launchMarginX = 50;
        private const int launchMarginY = 50;

        private bool WillCollide(Sprite sprite)
        {
            return spriteEngineMain.WillCollide(sprite);
        }

        private void AddSprite(Sprite sprite)
        {
            spriteEngineMain.AddSprite(sprite);
        }

        public SpriteSurface Surface
        {
            get
            {
                return spriteSurfaceMain;
            }
        }

        public int ActualFPS
        {
            get
            {
                return spriteSurfaceMain.ActualFPS;
            }
        }

        public void AddGifts(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Gift gift;
                do
                {
                    int x = rnd.Next(launchMarginX, Surface.Width - launchMarginX);
                    int y = rnd.Next(launchMarginY, Surface.Height - launchMarginY);
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
                    int x = rnd.Next(launchMarginX, Surface.Width - launchMarginX);
                    int y = rnd.Next(launchMarginY, Surface.Height - launchMarginY);
                    bomb = new Bomb(x, y);
                }
                while (WillCollide(bomb));
                AddSprite(bomb);
                bomb.LightFuse();
            }
        }

        public void AddTerrain(int arenaX1, int arenaX2, int arenaY1, int arenaY2)
        {
            Terrain terrainSprite = new Terrain(arenaX1, arenaX2, arenaY1, arenaY2);
            AddSprite(terrainSprite);
        }

        public void AddFoods(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Food food;
                do
                {
                    int x = rnd.Next(launchMarginX, Surface.Width - launchMarginX);
                    int y = rnd.Next(launchMarginY, Surface.Height - launchMarginY);
                    food = new Food(x, y);
                }
                while (WillCollide(food));
                AddSprite(food);
            }
        }

        public void AddCritter(Critter critter)
        {
            do
            {
                critterStartY += 30;
                if (critterStartY >= spriteSurfaceMain.Height - 30)
                {
                    critterStartY = 30;
                    critterStartX += 100;
                }
                critter.Position = new Point(critterStartX, critterStartY);
            }
            while (WillCollide(critter));
            spriteEngineMain.AddSprite(critter);
        }

        public void ResetLaunchPosition()
        {
            critterStartX = 30;
            critterStartY = 0;
        }

        public void Launch()
        {
            spriteSurfaceMain.Active = true;

            System.Timers.Timer critterStartupTimer = new System.Timers.Timer();
            critterStartupTimer.Interval = 2000;
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
        }

        public void Shutdown()
        {
            spriteSurfaceMain.Active = false;
            Sprite[] sprites = spriteEngineMain.Sprites.ToArray();
            foreach (Sprite sprite in sprites)
            {
                if (sprite is Critter)
                {
                    ((Critter)sprite).Shutdown();
                }
            }
            spriteEngineMain.Clear();
            Thread.Sleep(500);
            spriteSurfaceMain.Active = true;
            Thread.Sleep(500);
            spriteSurfaceMain.Active = false;
            ResetLaunchPosition();
        }

        public int CountOfActiveCritters
        {
            get
            {
                int count = 0;
                Sprite[] sprites = spriteEngineMain.Sprites.ToArray();
                foreach (Sprite sprite in sprites)
                {
                    if (sprite is Critter && !sprite.Dead && !((Critter)sprite).Stopped)
                    {
                        count++;
                    }
                }
                return count;
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

        public Arena()
        {
            ResetLaunchPosition();

            InitializeComponent();

            spriteSurfaceMain.SpriteCollision += (sender, collisionEvent) => Collide(sender, collisionEvent);

            spriteSurfaceMain.WraparoundEdges = true;
        }
    }
}

