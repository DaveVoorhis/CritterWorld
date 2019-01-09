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
    public class Food : BitmapSprite
    {
        public Food(int x, int y) : base((Bitmap)Image.FromFile("Resources/Images/Kiwi-Fruit.png"))
        {
            Position = new Point(x, y);
        }
    }

}
