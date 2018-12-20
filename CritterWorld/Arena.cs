using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
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

            double theta1 = Math.Atan2(dm1.SpeedY, dm1.SpeedX) * 180 / Math.PI;
            e.Sprite1.FacingAngle = (int)theta1 + 90;

            double theta2 = Math.Atan2(dm2.SpeedY, dm2.SpeedX) * 180 / Math.PI;
            e.Sprite2.FacingAngle = (int)theta2 + 90;
        }

        public Arena()
        {
            float critterCount = 1;

            InitializeComponent();

            spriteSurface1.SpriteCollision += new System.EventHandler<SCG.TurboSprite.SpriteCollisionEventArgs>(this.surface_SpriteCollision);

            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < critterCount; i++)
            {
                Critter critter = new Critter(spriteSurface1, spriteEngine1);
            }

            spriteSurface1.Active = true;
            spriteSurface1.WraparoundEdges = true;
        }
    }
}
