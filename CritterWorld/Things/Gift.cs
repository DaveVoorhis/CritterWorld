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
    public class Gift : BitmapSprite, ISensable, IVisible
    {
        public Gift() : base((Bitmap)Image.FromFile("Resources/Images/gift.png"))
        {
        }

        public string SensorSignature
        {
            get { return "Gift" + ":" + Position; }
        }
    }

}
