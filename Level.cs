using System;

public class Level
{
    private Arena _arena;
    private Bitmap _terrainMask;

	public Level(Arena arena)
	{
        _arena = arena;
	}

    private void setupMask()
    {
        int width = panel.ClientSize.Width;
        int height = panel.ClientSize.Height;
        Bitmap backgroundImage = (Bitmap)panel.BackgroundImage;
        Color pixelColour;

        map = new int[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                pixelColour = backgroundImage.GetPixel(x, y);
                if (pixelColour.B >= 128 && pixelColour.G >= 128 && pixelColour.R >= 128)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = 255;
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
        }
    }

    public void ObtainTerrainMaskFromFile(string fileName)
    {
        TerrainMask = BitMap.FromFile(fileName);        
    }
}
