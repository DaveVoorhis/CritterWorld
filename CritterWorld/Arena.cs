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
        Random rnd = Sprite.RND;

        SpriteEngine spriteEngineDebug;

        private void Collide(object sender, SpriteCollisionEventArgs e)
        {
            Critter critter1 = (Critter)e.Sprite1.Data;
            Critter critter2 = (Critter)e.Sprite2.Data;

            critter1.AssignRandomDestination();
            critter2.AssignRandomDestination();

            Sprite fight = new ParticleExplosionSprite(10, Color.DarkRed, Color.Red, 1, 5, 10)
            {
                Position = new Point((e.Sprite1.Position.X + e.Sprite2.Position.X) / 2, (e.Sprite1.Position.Y + e.Sprite2.Position.Y) / 2)
            };
            spriteEngineDebug.AddSprite(fight);
        }

        public Arena()
        {
            float critterCount = 20;

            InitializeComponent();

            spriteEngineDebug = new SpriteEngine(components)
            {
                Surface = spriteSurfaceMain,
                DetectCollisionSelf = false,
                DetectCollisionTag = 50
            };

            spriteSurfaceMain.SpriteCollision += (sender, e) => Collide(sender, e);

            int startX = 30;
            int startY = 30;

            for (int i = 0; i < critterCount; i++)
            {
                Critter critter = new Critter(spriteEngineMain, spriteEngineDebug);
                critter.GetSprite().Position = new Point(startX, startY);
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
            fpsDisplayTimer.Interval = 1000;
            fpsDisplayTimer.Tick += (sender, e) => labelFPS.Text = spriteSurfaceMain.ActualFPS + " fps";
            fpsDisplayTimer.Start();
        }
    }
}
