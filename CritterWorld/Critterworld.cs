using SCG.TurboSprite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CritterWorld
{
    public partial class Critterworld : Form
    {
        private int tickCount = 0;
        private Level level;
        private Competition competition;

        private String TickShow()
        {
            if (tickCount++ > 5)
            {
                tickCount = 0;
            }
            return new string('.', tickCount);
        }

        private void Shutdown()
        {
            arena.Shutdown();
            if (level != null)
            {
                level.Shutdown();
                level = null;
            }
            if (competition != null)
            {
                competition.Shutdown();
                competition = null;
            }
        }

        private void MenuStart_Click(object sender, EventArgs e)
        {
            Shutdown();
            level = new Level(arena, (Bitmap)Image.FromFile("Resources/TerrainMasks/Background05.png"));
            level.Launch();
        }

        private void MenuCompetionStart_Click(object sender, EventArgs e)
        {
            Shutdown();
            competition = new Competition(arena);
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background00.png")));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background01.png")));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background02.png")));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background03.png")));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background04.png")));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background05.png")));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background06.png")));
            competition.Launch();
        }

        private void MenuStop_Click(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Shutdown();
            Application.Exit();
        }

        public Critterworld()
        {
            InitializeComponent();

            String splashTextSrc = "CritterWorld";
            int index = 0;
            foreach (Char c in splashTextSrc)
            {
                FontPolygon splashTextDefinition = new FontPolygon(c.ToString(), "Arial", 150);
                PolygonSprite splashText = new PolygonSprite(splashTextDefinition.GetPoints());
                Size size = TextRenderer.MeasureText(c.ToString(), new Font("Arial", 150, FontStyle.Regular));
                splashText.Position = new Point(0 + (index++ * size.Width), 140);
                arena.AddSprite(splashText);
            }

            FontPolygon splashTextDefinition2 = new FontPolygon("2", "Arial", 250);
            PolygonSprite splashText2 = new PolygonSprite(splashTextDefinition2.GetPoints());
            splashText2.Position = new Point(250, 340);
            splashText2.Color = Color.Green;
            arena.AddSprite(splashText2);

            System.Timers.Timer fpsDisplayTimer = new System.Timers.Timer();
            fpsDisplayTimer.Interval = 250;
            fpsDisplayTimer.Elapsed += (sender, e) => Invoke(new Action(() => labelFPS.Text = arena.ActualFPS + " fps" + TickShow()));
            fpsDisplayTimer.Start();
        }
    }
}
