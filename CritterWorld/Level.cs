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
        private const int critterSpriteScale = 1;

        public int FoodCount { get; set; } = 5;

        public int GiftCount { get; set; } = 5;

        public int BombCount { get; set; }  = 5;

        public Level() { }

        public Level(Arena arena)
        {
            Arena = arena;
        }

        public Level(Bitmap terrainMask, Point escapeHatch)
        {
            TerrainMask = terrainMask;
            EscapeHatch = escapeHatch;
        }

        public Level(Arena arena, Bitmap terrainMask, Point escapeHatch)
        {
            Arena = arena;
            TerrainMask = terrainMask;
            EscapeHatch = escapeHatch;
        }

        public Arena Arena { get; set; }

        public Bitmap TerrainMask { get; set; }

        public Point EscapeHatch { get; set; }

        public int CountOfActiveCritters
        {
            get
            {
                return Arena.CountOfActiveCritters;
            }
        }

        private void SetupTerrain()
        {
            int mapWidth = PropertiesManager.Properties.TerrainDetailFactor;
            int mapHeight = (int)(PropertiesManager.Properties.TerrainDetailFactor * (float)Arena.Surface.Height / Arena.Surface.Width);
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

        public void Setup()
        {
            if (Arena == null || TerrainMask == null)
            {
                return;
            }

            SetupTerrain();

            Arena.AddFoods(FoodCount);
            Arena.AddBombs(BombCount);
            Arena.AddGifts(GiftCount);

            if (EscapeHatch.X != 0 && EscapeHatch.Y != 0)
            {
                int escapeX = EscapeHatch.X * Arena.Surface.Width / TerrainMask.Width;
                int escapeY = EscapeHatch.Y * Arena.Surface.Height / TerrainMask.Height;
                Arena.AddEscapeHatch(new Point(escapeX, escapeY));
            }
        }
    }

}
