using SCG.TurboSprite;
using SCG.TurboSprite.SpriteMover;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace CritterWorld
{
    public partial class Critterworld : Form
    {
        // Level duration in seconds.
        const int levelDuration = 10 * 1; 

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
            LevelTimerStop();
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

        private void StartOneLevel()
        {
            Shutdown();
            LevelTimerStart();
            level = new Level(arena, (Bitmap)Image.FromFile("Resources/TerrainMasks/Background05.png"));
            level.Launch();
        }

        private void MenuStart_Click(object sender, EventArgs e)
        {
            StartOneLevel();
        }

        private void MenuCompetionStart_Click(object sender, EventArgs e)
        {
            Shutdown();
            LevelTimerStart();
            competition = new Competition(arena);
            competition.Finished += (sndr, ev) => DisplayGameOver();
            competition.FinishedLevel += (sndr, ev) => LevelTimerStart();
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background00.png")));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background01.png")));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background02.png")));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background03.png")));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background04.png")));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background05.png")));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background06.png")));
            competition.Launch();
        }

        private void NextLevel()
        {
            if (competition != null)
            {
                LevelTimerStart();
                competition.NextLevel();
            }
            else if (level != null)
            {
                StartOneLevel();
            }
        }

        private void MenuNextLevel_Click(object sender, EventArgs e)
        {
            NextLevel();
        }

        private void MenuStop_Click(object sender, EventArgs e)
        {
            DisplayGameOver();
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Shutdown();
            Application.Exit();
        }

        private void DisplayGameOver()
        {
            Shutdown();
            LevelTimerStop();
            TextSprite splashText = new TextSprite("GAME OVER", "Arial", 1, FontStyle.Regular);
            splashText.Mover = new TextGrower(1, 100, 3);
            arena.AddSprite(splashText);
            splashText.Position = new Point(arena.Width / 2, arena.Height / 2);
            System.Timers.Timer gameOverTimer = new System.Timers.Timer();
            gameOverTimer.AutoReset = false;
            gameOverTimer.Interval = 5000;
            gameOverTimer.Elapsed += (sender, e) =>
            {
                Shutdown();
                DisplaySplash();
            };
            gameOverTimer.Start();
            arena.Launch();
        }

        private void DisplayCritterworldText()
        {
            Sprite splashText = new TextSprite("CritterWorld", "Arial", 150, FontStyle.Regular);
            arena.AddSprite(splashText);
            splashText.Position = new Point(arena.Width / 2, arena.Height / 2 - 100);
            TextTwitcher splashTextTwitcher = new TextTwitcher
            {
                PositionTwitchRange = 2,
                SizeTwitchPercentage = 0
            };
            splashText.Mover = splashTextTwitcher;
        }

        private void DisplayVersion()
        {
            TextSprite splashTextVersion = new TextSprite("2", "Arial", 250, FontStyle.Bold);
            arena.AddSprite(splashTextVersion);
            splashTextVersion.Position = new Point(arena.Width / 2, arena.Height / 2 + 150);
            splashTextVersion.Color = Color.Green;
            TextTwitcher splashTextVersionTwitcher = new TextTwitcher
            {
                PositionTwitchRange = 3,
                SizeTwitchPercentage = 50
            };
            splashTextVersion.Mover = splashTextVersionTwitcher;
        }

        private void DisplayWanderingCritter()
        {
            PolygonSprite wanderer = new PolygonSprite((new CritterBody()).GetBody());
            wanderer.Color = Sprite.RandomColor(127);
            wanderer.Processors += sprite =>
            {
                TargetMover spriteMover = (TargetMover)sprite.Mover;
                if (spriteMover == null || (spriteMover.SpeedX == 0 && spriteMover.SpeedY == 0))
                {
                    return;
                }
                double theta = Sprite.RadToDeg((float)Math.Atan2(spriteMover.SpeedY, spriteMover.SpeedX));
                spriteMover.TargetFacingAngle = (int)theta + 90;
            };
            int margin = 20;
            int speed = 4;
            int moveCount = 0;
            Route route = new Route(wanderer);
            route.SpriteMoved += (sender, spriteEvent) =>
            {
                if (moveCount-- == 0)
                {
                    wanderer.IncrementFrame();
                    moveCount = 5 - Math.Min(5, speed);
                }
            };
            route.Add(margin, margin, speed);
            route.Add(arena.Width - margin, margin, speed);
            route.Add(arena.Width - margin, arena.Height / 2 - margin, speed);
            route.Add(margin, arena.Height / 2 - margin, speed);
            route.Add(margin, arena.Height - margin, speed);
            route.Add(arena.Width - margin, arena.Height - margin, speed);
            route.Add(arena.Width - margin, arena.Height / 2 - margin, speed);
            route.Add(margin, arena.Height / 2 - margin, speed);
            route.Repeat = true;
            arena.AddSprite(wanderer);
            route.Start();
        }

        private void DisplaySplash()
        {
            LevelTimerStop();
            DisplayCritterworldText();
            DisplayVersion();
            DisplayWanderingCritter();
            arena.Launch();
        }

        private System.Timers.Timer levelTimer = null;
        private int countDown;

        private void Tick()
        {
            countDown--;
            if (countDown <= 0)
            {
                NextLevel();
            }
            else
            {
                Invoke(new Action(() => levelTimeoutProgress.Value = countDown * 100 / levelDuration));
            }
        }

        private void LevelTimerStart()
        {
            if (levelTimer == null)
            {
                levelTimer = new System.Timers.Timer();
                levelTimer.Interval = 1000;
                levelTimer.AutoReset = true;
                levelTimer.Elapsed += (sender, e) => Tick();
            }
            levelTimer.Stop();
            levelTimer.Start();
            countDown = levelDuration;
            Invoke(new Action(() => levelTimeoutProgress.Value = 100));
        }

        private void LevelTimerStop()
        {
            if (levelTimer != null)
            {
                levelTimer.Stop();
                Invoke(new Action(() => levelTimeoutProgress.Value = 0));
            }
        }

        public Critterworld()
        {
            InitializeComponent();

            Width = 1000;
            Height = 800 + Height - arena.Height;

            labelVersion.Text = Version.VersionName;

            DisplaySplash();

            System.Timers.Timer fpsDisplayTimer = new System.Timers.Timer();
            fpsDisplayTimer.Interval = 250;
            fpsDisplayTimer.Elapsed += (sender, e) => Invoke(new Action(() => labelFPS.Text = arena.ActualFPS + " FPS" + TickShow()));
            fpsDisplayTimer.Start();
        }
    }
}
