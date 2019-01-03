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
            int width = _arena.Width;
            int height = _arena.Height;
            Color pixelColour;

            bool[,] terrain = new bool[mapWidth, mapHeight];
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int maskX = x * _terrainMask.Width / mapWidth;
                    int maskY = y * _terrainMask.Height / mapHeight;
                    pixelColour = _terrainMask.GetPixel(maskX, maskY);
                    terrain[x, y] = !(pixelColour.B >= 128 && pixelColour.G >= 128 && pixelColour.R >= 128);
                }
            }

            List<VBlock> vblocks = new List<VBlock>();
            List<HBlock> hblocks = new List<HBlock>();

            for (int y = 0; y < mapHeight; y++)
            {
                int xRunStart = -1;
                bool running = false;
                int x;
                for (x = 0; x < mapWidth; x++)
                {
                    if (terrain[x, y] && !running)
                    {
                        xRunStart = x;
                        running = true;
                    }
                    else if (!terrain[x, y] && running)
                    {
                        hblocks.Add(new HBlock(xRunStart, x, y));
                        xRunStart = -1;
                        running = false;
                    }
                }
                if (running)
                {
                    hblocks.Add(new HBlock(xRunStart, x - 1, y));
                }
            }

            for (int x = 0; x < mapWidth; x++)
            {
                int yRunStart = -1;
                bool running = false;
                int y;
                for (y = 0; y < mapHeight; y++)
                {
                    if (terrain[x, y] && !running)
                    {
                        yRunStart = y;
                        running = true;
                    } else if (!terrain[x, y] && running)
                    {
                        vblocks.Add(new VBlock(x, yRunStart, y));
                        yRunStart = -1;
                        running = false;
                    }
                }
                if (running)
                {
                    vblocks.Add(new VBlock(x, yRunStart, y - 1));
                }
            }

            foreach (HBlock hblock in hblocks)
            {
                int arenaX1 = hblock.X1 * width / mapWidth;
                int arenaX2 = hblock.X2 * width / mapWidth;
                int arenaY1 = hblock.Y * height / mapHeight;
                int arenaY2 = arenaY1 + height / mapHeight;

                Terrain terrainSprite = new Terrain(arenaX1, arenaX2, arenaY1, arenaY2);
                _arena.AddSprite(terrainSprite);
            }

            foreach (VBlock vblock in vblocks)
            {
                int arenaX1 = vblock.X * width / mapWidth;
                int arenaX2 = arenaX1 + width / mapWidth;
                int arenaY1 = vblock.Y1 * height / mapHeight;
                int arenaY2 = vblock.Y2 * height / mapHeight;

                Terrain terrainSprite = new Terrain(arenaX1, arenaX2, arenaY1, arenaY2);
                _arena.AddSprite(terrainSprite);
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
        public Terrain(int X1, int X2, int Y1, int Y2) : base(new PointF[]
            {
                new PointF(0, 0),
                new PointF(X2 - X1, 0),
                new PointF(X2 - X1, Y2 - Y1),
                new PointF(0, Y2 - Y1)
            })
        {
            Position = new Point(X1, Y1);
            Color = Sprite.RandomColor(64);
        }
    }

    internal class HBlock
    {
        public HBlock(int x1, int x2, int y)
        {
            X1 = x1;
            X2 = x2;
            Y = y;
        }
        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return "HBlock { X1 = " + X1 + ", X2 = " + X2 + ", Y = " + Y + "}";
        }
    }

    internal class VBlock
    {
        public VBlock(int x, int y1, int y2)
        {
            X = x;
            Y1 = y1;
            Y2 = y2;
        }
        public int X { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }

        public override string ToString()
        {
            return "VBlock { X = " + X + ", Y1 = " + Y1 + ", Y2 = " + Y2 + "}";
        }
    }

}
