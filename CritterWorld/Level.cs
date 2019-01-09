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

        const int critterCount = 25;
        const int scale = 1;

        const int foodCount = 5;
        const int giftCount = 5;
        const int bombCount = 5;

        public Level() { }

        public Level(Arena arena)
        {
            Arena = arena;
        }

        public Level(Bitmap terrainMask)
        {
            TerrainMask = terrainMask;
        }

        public Level(Arena arena, Bitmap terrainMask)
        {
            Arena = arena;
            TerrainMask = terrainMask;
        }

        public Arena Arena { get; set; }

        public Bitmap TerrainMask { get; set; }

        public int CountOfActiveCritters
        {
            get
            {
                return Arena.CountOfActiveCritters;
            }
        }

        private void SetupTerrain()
        {
            int mapWidth = terrainDensity;
            int mapHeight = (int)(terrainDensity * (float)Arena.Surface.Height / (float)Arena.Surface.Width);
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int maskX = x * TerrainMask.Width / mapWidth;
                    int maskY = y * TerrainMask.Height / mapHeight;
                    Color pixelColour = TerrainMask.GetPixel(maskX, maskY);
                    if (!(pixelColour.B >= 128 && pixelColour.G >= 128 && pixelColour.R >= 128))
                    {
                        int arenaX1 = x * Arena.Surface.Width / mapWidth;
                        int arenaX2 = (x + 1) * Arena.Surface.Width / mapWidth;
                        int arenaY1 = y * Arena.Surface.Height / mapHeight;
                        int arenaY2 = (y + 1) * Arena.Surface.Height / mapHeight;
                        Arena.AddTerrain(arenaX1, arenaX2, arenaY1, arenaY2);
                    }
                }
            }
        }

        public void Launch()
        {
            if (Arena == null || TerrainMask == null)
            {
                return;
            }

            SetupTerrain();

            Arena.AddFoods(foodCount);
            Arena.AddBombs(bombCount);
            Arena.AddGifts(giftCount);

            for (int i = 0; i < critterCount; i++)
            {
                Critter critter = new Critter(scale);
                Arena.AddCritter(critter);
            }

            Arena.Launch();
        }

        public void Shutdown()
        {
            Arena.Shutdown();
        }
    }

}
