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
        private const int terrainDensity = 80;

        private Arena _arena;
        private Bitmap _terrainMask;

        public Level() { }

        public Level(Arena arena)
        {
            Arena = arena;
        }

        public Level(Arena arena, Bitmap terrainMask)
        {
            Arena = arena;
            TerrainMask = terrainMask;
        }

        private void SetupMask()
        {
            if (_arena == null || _terrainMask == null)
            {
                return;
            }
            int mapWidth = terrainDensity;
            int mapHeight = (int)(terrainDensity * (float)_arena.Height / (float)_arena.Width);
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

        public Arena Arena
        {
            get
            {
                return _arena;
            }
            set
            {
                _arena = value;
                SetupMask();
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
    }

}
