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
            DisplayGameOver();
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Shutdown();
            Application.Exit();
        }

        private void DisplayGameOver()
        {
            Sprite splashText = new TextSprite("GAME OVER", "Arial", 100, FontStyle.Regular);
            arena.AddSprite(splashText);
            splashText.Position = new Point(arena.Width / 2, arena.Height / 2);
        }

        private void DisplayLogo()
        {
            Sprite splashText = new TextSprite("CritterWorld", "Arial", 150, FontStyle.Regular);
            arena.AddSprite(splashText);
            splashText.Position = new Point(arena.Width / 2, arena.Height / 2 - 100);

            TextSprite splashTextVersion = new TextSprite("2", "Arial", 250, FontStyle.Bold);
            arena.AddSprite(splashTextVersion);
            splashTextVersion.Position = new Point(arena.Width / 2, arena.Height / 2 + 150);
            splashTextVersion.Color = Color.Green;
        }

        public Critterworld()
        {
            InitializeComponent();

            DisplayLogo();

            System.Timers.Timer fpsDisplayTimer = new System.Timers.Timer();
            fpsDisplayTimer.Interval = 250;
            fpsDisplayTimer.Elapsed += (sender, e) => Invoke(new Action(() => labelFPS.Text = arena.ActualFPS + " fps" + TickShow()));
            fpsDisplayTimer.Start();
        }
    }
}
