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

        private Random rnd = new Random(Guid.NewGuid().GetHashCode());

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

        private void SetupTerrain()
        {
            int mapWidth = terrainDensity;
            int mapHeight = (int)(terrainDensity * (float)_arena.Surface.Height / (float)_arena.Surface.Width);
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int maskX = x * _terrainMask.Width / mapWidth;
                    int maskY = y * _terrainMask.Height / mapHeight;
                    Color pixelColour = _terrainMask.GetPixel(maskX, maskY);
                    if (!(pixelColour.B >= 128 && pixelColour.G >= 128 && pixelColour.R >= 128))
                    {
                        int arenaX1 = x * _arena.Surface.Width / mapWidth;
                        int arenaX2 = (x + 1) * _arena.Surface.Width / mapWidth;
                        int arenaY1 = y * _arena.Surface.Height / mapHeight;
                        int arenaY2 = (y + 1) * _arena.Surface.Height / mapHeight;
                        Terrain terrainSprite = new Terrain(arenaX1, arenaX2, arenaY1, arenaY2);
                        _arena.AddSprite(terrainSprite);
                    }
                }
            }
        }

        private void Setup()
        {
            if (_arena == null || _terrainMask == null)
            {
                return;
            }

            Console.WriteLine("Loading terrain...");
            SetupTerrain();

            Console.WriteLine("Loading food...");
            for (int i = 0; i < 5; i++)
            {
                Food food;
                do
                {
                    int x = rnd.Next(50, _arena.Surface.Width - 50);
                    int y = rnd.Next(50, _arena.Surface.Height - 50);
                    food = new Food(x, y);
                }
                while (_arena.WillCollide(food));
                _arena.AddSprite(food);
            }

            Console.WriteLine("Loading bombs...");
            for (int i = 0; i < 5; i++)
            {
                Bomb bomb;
                do
                {
                    int x = rnd.Next(50, _arena.Surface.Width - 50);
                    int y = rnd.Next(50, _arena.Surface.Height - 50);
                    bomb = new Bomb(x, y);
                }
                while (_arena.WillCollide(bomb));
                _arena.AddSprite(bomb);
                bomb.LightFuse();
            }

            Console.WriteLine("Loading gifts...");
            for (int i = 0; i < 5; i++)
            {
                Gift gift;
                do
                {
                    int x = rnd.Next(50, _arena.Surface.Width - 50);
                    int y = rnd.Next(50, _arena.Surface.Height - 50);
                    gift = new Gift(x, y);
                }
                while (_arena.WillCollide(gift));
                _arena.AddSprite(gift);
            }

            Console.WriteLine("Level loaded.");
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
                Setup();
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
                Setup();
            }
        }
    }

}
