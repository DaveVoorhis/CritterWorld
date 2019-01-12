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
    public class Gift : BitmapSprite
    {
        public Gift(Point position) : base((Bitmap)Image.FromFile("Resources/Images/gift.png"))
        {
            Position = position;
        }
    }

}
