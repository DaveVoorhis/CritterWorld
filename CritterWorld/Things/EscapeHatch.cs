using SCG.TurboSprite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CritterWorld
{
    public class EscapeHatch : BitmapSprite, ISensable, IVisible
    {
        public EscapeHatch(Point position) : base((Bitmap)Image.FromFile("Resources/Images/Goal.png"))
        {
            Position = position;
        }

        public string SensorSignature
        {
            get { return "EscapeHatch" + ":" + Position; }
        }

    }

}
