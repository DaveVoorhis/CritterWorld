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
    public class Food : BitmapSprite, ISensable
    {
        public Food(Point position) : base((Bitmap)Image.FromFile("Resources/Images/Kiwi-Fruit.png"))
        {
            Position = position;
        }
    }

}
