using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SCG.TurboSprite;

namespace CritterWorld
{
    public partial class Arena : UserControl
    {
        public event EventHandler<SpriteEventEscaped> CritterEscaped;

        private const int critterLaunchSpacing = 31;

        private Random rnd = new Random(Guid.NewGuid().GetHashCode());

        private int critterStartX;
        private int critterStartY;

        private const int launchMarginLeft = 150;
        private const int launchMarginRight = 20;
        private const int launchMarginTop = 20;
        private const int launchMarginBottom = 20;

        private System.Timers.Timer fpsTimer = null;

        private string fpsPrompt = "FPS";
        private int activeFPSPromptState = 0;

        private string ActiveFPSPrompt()
        {
            activeFPSPromptState = (activeFPSPromptState + 1) % fpsPrompt.Length;
            return 
                fpsPrompt.Substring(0, activeFPSPromptState) + 
                fpsPrompt[activeFPSPromptState].ToString().ToLower() + 
                fpsPrompt.Substring(activeFPSPromptState + 1);
        }

        public bool WillCollide(Sprite sprite)
        {
            return spriteEngineMain.WillCollide(sprite);
        }

        public void AddSprite(Sprite sprite)
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

        public void AddTerrain(int arenaX1, int arenaX2, int arenaY1, int arenaY2)
        {
            Terrain terrainSprite = new Terrain(arenaX1, arenaX2, arenaY1, arenaY2);
            AddSprite(terrainSprite);
        }

        private delegate Sprite CreateSprite();
        private delegate void InitialiseSprite(Sprite sprite);

        private void FindEmptyPlaceFor(Sprite sprite)
        {
            do
            {
                int x = rnd.Next(launchMarginLeft, Surface.Width - launchMarginRight);
                int y = rnd.Next(launchMarginTop, Surface.Height - launchMarginBottom);
                sprite.Position = new Point(x, y);
            }
            while (WillCollide(sprite));
        }

        private void AddThings(int count, CreateSprite factory, InitialiseSprite initialiser = null)
        {
            for (int i = 0; i < count; i++)
            {
                Sprite sprite = factory();
                FindEmptyPlaceFor(sprite);
                AddSprite(sprite);
                initialiser?.Invoke(sprite);
            }
        }

        public void AddGifts(int count)
        {
            AddThings(count, () => new Gift());
        }

        public void AddBombs(int count)
        {
            AddThings(count, () => new Bomb(), sprite => ((Bomb)sprite).LightFuse());
        }

        public void AddFoods(int count)
        {
            AddThings(count, () => new Food());
        }

        public void AddCritter(Critter critter)
        {
            do
            {
                critterStartY += critterLaunchSpacing;
                if (critterStartY >= spriteSurfaceMain.Height - critterLaunchSpacing)
                {
                    critterStartY = critterLaunchSpacing;
                    critterStartX += 100;
                }
                critter.Position = new Point(critterStartX, critterStartY);
            }
            while (WillCollide(critter));
            AddSprite(critter);
        }

        public void AddEscapeHatch(Point position)
        {
            EscapeHatch escapeHatch = new EscapeHatch(position);
            AddSprite(escapeHatch);
        }

        public void ResetLaunchPosition()
        {
            critterStartX = 30;
            critterStartY = 0;
        }

        public void Launch()
        {
            if (spriteSurfaceMain.Active)
            {
                return;
            }

            spriteSurfaceMain.Active = true;

            TextSprite fps = new TextSprite("999 FPS", "Courier New", 10, FontStyle.Regular)
            {
                HorizontalAlignment = StringAlignment.Far,
                VerticalAlignment = StringAlignment.Far,
                Position = new Point(spriteSurfaceMain.Width - 5, spriteSurfaceMain.Height - 5),
                Color = Color.Gray,
                Alpha = 128
            };
            AddSprite(fps);

            fpsTimer = new System.Timers.Timer
            {
                Interval = 250
            };
            fpsTimer.Elapsed += (sender, e) => fps.Text = ActualFPS + " " + ActiveFPSPrompt();
            fpsTimer.Start();

            System.Timers.Timer critterStartupTimer = new System.Timers.Timer();
            critterStartupTimer.Interval = 1000;
            critterStartupTimer.AutoReset = false;
            critterStartupTimer.Elapsed += (sender, e) => spriteEngineMain.SpriteArray.OfType<Critter>().ToList().ForEach(critter => critter.Launch());
            critterStartupTimer.Start();
        }

        public void Shutdown()
        {
            if (fpsTimer != null)
            {
                fpsTimer.Stop();
                fpsTimer = null;
            }

            spriteEngineMain.Locked = true;
            spriteSurfaceMain.Active = false;

            spriteEngineMain.SpriteArray.OfType<Critter>().ToList().ForEach(critter => critter.Shutdown());

            spriteEngineMain.Clear();
            spriteEngineMain.Purge();

            ResetLaunchPosition();

            spriteEngineMain.Locked = false;
        }

        public int CountOfActiveCritters
        {
            get
            {
                return spriteEngineMain.SpriteArray.OfType<Critter>().Where(critter => !critter.Dead && !critter.Stopped).Count();
            }
        }

        private void Collide(Critter critter1, Critter critter2)
        {
            if (critter1.Mover is NullMover || critter2.Mover is NullMover)
            {
                return;
            }

            Sound.PlayBump();

            critter1.Bounceback();
            critter2.Bounceback();

            critter1.FightWith(critter2.NumberNameAndAuthor);
            critter2.FightWith(critter1.NumberNameAndAuthor);

            Sprite fight = new ParticleExplosionSprite(10, Color.DarkRed, Color.Red, 1, 5, 10)
            {
                Position = new Point((critter1.Position.X + critter2.Position.X) / 2, (critter1.Position.Y + critter2.Position.Y) / 2)
            };
            AddSprite(fight);
        }

        private void Collide(Critter critter, Terrain terrain)
        {
            if (critter.Mover is NullMover)
            {
                return;
            }

            Sound.PlayZap();

            critter.Bounceback();
            critter.Bump();

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

            critter.Bombed();
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
            critter.Mover = new NullMover();
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

            FindEmptyPlaceFor(bomb);
        }

        private void Collide(Critter critter, Food food)
        {
            Sound.PlayGulp();
            critter.Ate();
            FindEmptyPlaceFor(food);
        }

        private void Collide(Critter critter, Gift gift)
        {
            Sound.PlayYay();
            critter.Scored();
            FindEmptyPlaceFor(gift);
        }

        private void Collide(Critter critter, EscapeHatch hatch)
        {
            Sound.PlayCheer();

            Sprite shockwaveCritter = new ShockWaveSprite(5, 20, 20, Color.DarkGreen, Color.LightGreen);
            shockwaveCritter.Position = critter.Position;
            AddSprite(shockwaveCritter);

            Sprite shockwaveHatch = new ShockWaveSprite(5, 30, 20, Color.DarkGreen, Color.LightGreen);
            shockwaveHatch.Position = hatch.Position;
            AddSprite(shockwaveHatch);

            critter.Escaped();

            CritterEscaped?.Invoke(this, new SpriteEventEscaped(critter));
        }

        private void Collide(object sender, SpriteCollisionEventArgs collision)
        {
            Sprite sprite1 = collision.Sprite1;
            Sprite sprite2 = collision.Sprite2;

            if (sprite1 is Critter critter_a1 && sprite2 is Critter critter_a2)
            {
                Collide(critter_a1, critter_a2);
            }
            else if (sprite1 is Critter critter_a3 && sprite2 is Terrain terrain)
            {
                Collide(critter_a3, terrain);
            }
            else if (sprite1 is Terrain terrain_a1 && sprite2 is Critter critter_a4)
            {
                Collide(critter_a4, terrain_a1);
            }
            else if (sprite1 is Critter critter_a5 && sprite2 is Bomb bomb_a1)
            {
                Collide(critter_a5, bomb_a1);
            }
            else if (sprite1 is Bomb bomb_a2 && sprite2 is Critter critter_a6)
            {
                Collide(critter_a6, bomb_a2);
            }
            else if (sprite1 is Critter critter_a7 && sprite2 is Food food_a1)
            {
                Collide(critter_a7, food_a1);
            }
            else if (sprite1 is Food food_a2 && sprite2 is Critter critter_a8)
            {
                Collide(critter_a8, food_a2);
            }
            else if (sprite1 is Critter critter_a9 && sprite2 is Gift gift_a1)
            {
                Collide(critter_a9, gift_a1);
            }
            else if (sprite1 is Gift gift_a2 && sprite2 is Critter critter_a10)
            {
                Collide(critter_a10, gift_a2);
            }
            else if (sprite1 is Critter critter_a11 && sprite2 is EscapeHatch escapeHatch_a1)
            {
                Collide(critter_a11, escapeHatch_a1);
            }
            else if (sprite1 is EscapeHatch escapeHatch_a2 && sprite2 is Critter critter_a12)
            {
                Collide(critter_a12, escapeHatch_a2);
            }
        }

        public Arena()
        {
            ResetLaunchPosition();

            InitializeComponent();

            spriteSurfaceMain.SpriteCollision += Collide;

            spriteSurfaceMain.WraparoundEdges = true;
        }

        public Critter[] GetCritters()
        {
            return spriteEngineMain.SpriteArray.OfType<Critter>().ToArray();
        }
    }

    public class SpriteEventEscaped : SpriteEventArgs
    {
        public SpriteEventEscaped(Sprite sprite) : base(sprite) { }
    }
}

