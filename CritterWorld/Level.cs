using SCG.TurboSprite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterWorld
{
    public class Level
    {
        private const int mapWidth = 60;
        private const int mapHeight = 60;

        private Arena _arena;
        private Bitmap _terrainMask;

        public Level(Arena arena)
        {
            _arena = arena;
        }

        private void SetupMask()
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int maskX = x * _terrainMask.Width / mapWidth;
                    int maskY = y * _terrainMask.Height / mapHeight;
                    Color pixelColour = _terrainMask.GetPixel(maskX, maskY);
                    if (!(pixelColour.B >= 128 && pixelColour.G >= 128 && pixelColour.R >= 128))
                    {
                        int arenaX1 = x * _arena.Width / mapWidth;
                        int arenaX2 = (x + 1) * _arena.Width / mapWidth;
                        int arenaY1 = y * _arena.Height / mapHeight;
                        int arenaY2 = (y + 1) * _arena.Height / mapHeight;
                        Terrain terrainSprite = new Terrain(arenaX1, arenaX2, arenaY1, arenaY2);
                        _arena.AddSprite(terrainSprite);
                    }
                }
            }
        }

        public Bitmap TerrainMask
        {
            get
            {
                return _terrainMask;
            }
            set
            {
                _terrainMask = value;
                SetupMask();
            }
        }

        public void ObtainTerrainMaskFromFile(string fileName)
        {
            TerrainMask = (Bitmap)Image.FromFile(fileName);
        }
    }

    internal class Terrain : PolygonSprite
    {
        public Terrain(int X1, int X2, int Y1, int Y2) : 
            base(new PointF[]
            {
                new PointF(0, 0),
                new PointF(X2 - X1, 0),
                new PointF(X2 - X1, Y2 - Y1),
                new PointF(0, Y2 - Y1)
            })
        {
            Position = new Point(X1, Y1);
            Color = Color.DarkGreen;
        }
    }

}
