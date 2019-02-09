using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterWorld
{
    // Competition or free-run levels.
    class Levels
    {
        private static string MaskPath { get; } = "Resources/TerrainMasks";

        public static Level[] TheLevels { get; } = new Level[]
        {
            new Level((Bitmap)Image.FromFile(MakeFilename("Background00.png")), new Point(600, 228))
            {
                FoodCount = 20,
                GiftCount = 20,
                BombCount = 2
            },
            new Level((Bitmap)Image.FromFile(MakeFilename("Background01.png")), new Point(600, 247))
            {
                FoodCount = 10,
                GiftCount = 10,
                BombCount = 5
            },
            new Level((Bitmap)Image.FromFile(MakeFilename("Background02.png")), new Point(505, 434))
            {
                FoodCount = 10,
                GiftCount = 10,
                BombCount = 10
            },
            new Level((Bitmap)Image.FromFile(MakeFilename("Background03.png")), new Point(234, 256))
            {
                FoodCount = 10,
                GiftCount = 5,
                BombCount = 10
            },
            new Level((Bitmap)Image.FromFile(MakeFilename("Background04.png")), new Point(282, 255))
            {
                FoodCount = 10,
                GiftCount = 5,
                BombCount = 10
            },
            new Level((Bitmap)Image.FromFile(MakeFilename("Background05.png")), new Point(210, 366))
            {
                FoodCount = 10,
                GiftCount = 5,
                BombCount = 10
            },
            new Level((Bitmap)Image.FromFile(MakeFilename("Background06.png")), new Point(280, 360))
            {
                FoodCount = 10,
                GiftCount = 5,
                BombCount = 20
            }
        };

        private static string MakeFilename(string baseName)
        {
            return MaskPath + "/" + baseName;
        }

    }
}
